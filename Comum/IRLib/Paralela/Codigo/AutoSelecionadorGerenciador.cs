using CTLib;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;

namespace IRLib.Paralela
{

    /// <summary>
    /// Gerenciador do AutoSelecionador
    /// </summary>
    public class AutoSelecionadorGerenciador : MarshalByRefObject
    {

        public enum TipoSetores
        {
            Ambos,
            Marcados,
            NaoMarcados
        }

        [Flags]
        public enum TipoDisponibilidade
        {
            Nula = 0,
            Vender = 1,
            Ajustar = 2,
            GerarRelatorio = 4
        }

        Apresentacao.Disponibilidade disponibilidade;

        private int empresaID;
        private int localID;
        private int canalID;
        private int usuarioID;

        private int perfilID;
        private int regionalID;

        public int RegionalID
        {
            get { return regionalID; }
            set { regionalID = value; }
        }

        private TipoSetores tipoSetores;
        private string tipoSetoresDesc;
        private bool somenteApresentacoesQueNaoPassaram;
        private bool somenteApresentacoesQueJaPassaram;
        private bool somenteApresentacoesDisponiveisAjuste;

        public AutoSelecionadorGerenciador()
        {
            disponibilidade = Apresentacao.Disponibilidade.Nula;

            canalID = 0;
            usuarioID = 0;
            localID = 0;
            empresaID = 0;

            perfilID = 0;

            tipoSetores = TipoSetores.Ambos;

            tipoSetoresDesc = "";

            somenteApresentacoesQueNaoPassaram = false;
            somenteApresentacoesQueJaPassaram = false;
            somenteApresentacoesDisponiveisAjuste = false;

        }

        public bool SomenteApresentacoesQueNaoPassaram
        {
            get { return somenteApresentacoesQueNaoPassaram; }
            set { somenteApresentacoesQueNaoPassaram = value; }
        }

        public bool SomenteApresentacoesQueJaPassaram
        {
            get { return somenteApresentacoesQueJaPassaram; }
            set { somenteApresentacoesQueJaPassaram = value; }
        }

        public bool SomenteApresentacoesDisponiveisAjuste
        {
            get { return somenteApresentacoesDisponiveisAjuste; }
            set { somenteApresentacoesDisponiveisAjuste = value; }
        }

        public bool IncluirRegional { get; set; }

        public TipoSetores SetoresTipo
        {
            get { return tipoSetores; }
            set { tipoSetores = value; }
        }

        public string SetoresTipoDesc
        {
            get { return tipoSetoresDesc; }
            set { tipoSetoresDesc = value; }
        }

        public int EmpresaID
        {
            set { empresaID = value; }
        }

        public int LocalID
        {
            set { localID = value; }
        }

        public int UsuarioID
        {
            set { usuarioID = value; }
        }

        public int CanalID
        {
            set { canalID = value; }
        }

        public int PerfilID
        {
            set { perfilID = value; }
        }

        public TipoDisponibilidade Disponibilidade
        {
            set { disponibilidade = (Apresentacao.Disponibilidade)value; }
            get { return (TipoDisponibilidade)disponibilidade; }
        }

        public int ApresentacaoSetorID(int apresentacaoID, int setorID)
        {

            ApresentacaoSetor aS = new ApresentacaoSetor();
            return aS.ApresentacaoSetorID(apresentacaoID, setorID);

        }

        private DataTable unirTabelas(DataTable tabela1, DataTable tabela2)
        {

            try
            {

                foreach (DataRow linha in tabela2.Rows)
                    tabela1.ImportRow(linha);

                return tabela1;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarCanais()
        {

            try
            {

                if (empresaID != 0)
                    return carregarCanais(empresaID);

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));
                canais.Columns.Add("TaxaConveniencia", typeof(int));
                canais.Columns.Add("EmpresaID", typeof(int));

                BD bd = new BD();

                string clausulaRegional = string.Empty;
                if (this.RegionalID != 0)
                    clausulaRegional = " AND tEmpresa.RegionalID = " + this.RegionalID + " ";

                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tCanal.ID AS CanalID, tCanal.Nome AS Canal, tCanal.TaxaConveniencia, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tCanal (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID " +
                    clausulaRegional +
                    "ORDER BY tEmpresa.Nome,tCanal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresa["RegionalID"] = bd.LerInt("RegionalID");
                        empresas.Rows.Add(empresa);
                    }

                    if (canais.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("CanalID")).Length == 0)
                    {
                        DataRow canal = canais.NewRow();
                        canal["ID"] = bd.LerInt("CanalID");
                        canal["Nome"] = bd.LerString("Canal");
                        canal["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        canal["EmpresaID"] = bd.LerInt("EmpresaID");
                        canais.Rows.Add(canal);
                    }

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(canais);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarCanais(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));
                canais.Columns.Add("TaxaConveniencia", typeof(int));

                BD bd = new BD();

                string sql = "SELECT ID,Nome,TaxaConveniencia FROM tCanal (NOLOCK) " +
                    "WHERE EmpresaID=" + empresaID + " " +
                    "ORDER BY Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow canal = canais.NewRow();
                    canal["ID"] = bd.LerInt("ID");
                    canal["Nome"] = bd.LerString("Nome");
                    canal["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                    canais.Rows.Add(canal);
                }
                bd.Fechar();

                buffer.Tables.Add(canais);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarLojas()
        {

            try
            {

                if (empresaID != 0)
                    return carregarLojas(empresaID);

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));
                canais.Columns.Add("TaxaConveniencia", typeof(int));
                canais.Columns.Add("EmpresaID", typeof(int));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                BD bd = new BD();

                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tCanal.ID AS CanalID, tCanal.Nome AS Canal, tCanal.TaxaConveniencia, tLoja.ID AS LojaID, tLoja.Nome AS Loja, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tCanal (NOLOCK),tLoja (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID " +
                    "ORDER BY tEmpresa.Nome,tCanal.Nome,tLoja.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresa["RegionalID"] = bd.LerInt("RegionalID");
                        empresas.Rows.Add(empresa);
                    }

                    if (canais.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("CanalID")).Length == 0)
                    {
                        DataRow canal = canais.NewRow();
                        canal["ID"] = bd.LerInt("CanalID");
                        canal["Nome"] = bd.LerString("Canal");
                        canal["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        canal["EmpresaID"] = bd.LerInt("EmpresaID");
                        canais.Rows.Add(canal);
                    }

                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarLojas(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));
                canais.Columns.Add("TaxaConveniencia", typeof(int));
                canais.Columns.Add("EmpresaID", typeof(int));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                BD bd = new BD();

                string sql = "SELECT tCanal.ID AS CanalID, tCanal.Nome AS Canal, tCanal.TaxaConveniencia, tLoja.ID AS LojaID, tLoja.Nome AS Loja " +
                    "FROM tEmpresa (NOLOCK),tCanal (NOLOCK),tLoja (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tEmpresa.ID=" + empresaID + " " +
                    "ORDER BY tCanal.Nome,tLoja.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (canais.Select("ID=" + bd.LerInt("CanalID")).Length == 0)
                    {
                        DataRow canal = canais.NewRow();
                        canal["ID"] = bd.LerInt("CanalID");
                        canal["Nome"] = bd.LerString("Canal");
                        canal["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        canal["EmpresaID"] = bd.LerInt("EmpresaID");
                        canais.Rows.Add(canal);
                    }

                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);

                }
                bd.Fechar();

                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarUsuarios()
        {

            try
            {

                if (empresaID != 0)
                    return carregarUsuarios(empresaID);

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));
                canais.Columns.Add("TaxaConveniencia", typeof(int));
                canais.Columns.Add("EmpresaID", typeof(int));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                BD bd = new BD();

                string sql = "SELECT DISTINCT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tCanal.ID AS CanalID, tCanal.Nome AS Canal, tCanal.TaxaConveniencia, tLoja.ID AS LojaID, tLoja.Nome AS Loja, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tCanal (NOLOCK),tLoja (NOLOCK),tCaixa (NOLOCK),tUsuario (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID " +
                    "ORDER BY tEmpresa.Nome,tCanal.Nome,tLoja.Nome,tUsuario.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresa["RegionalID"] = bd.LerInt("RegionalID");
                        empresas.Rows.Add(empresa);
                    }

                    if (canais.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("CanalID")).Length == 0)
                    {
                        DataRow canal = canais.NewRow();
                        canal["ID"] = bd.LerInt("CanalID");
                        canal["Nome"] = bd.LerString("Canal");
                        canal["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        canal["EmpresaID"] = bd.LerInt("EmpresaID");
                        canais.Rows.Add(canal);
                    }

                    if (lojas.Select("CanalID=" + bd.LerInt("CanalID") + " AND ID=" + bd.LerInt("LojaID")).Length == 0)
                    {
                        DataRow loja = lojas.NewRow();
                        loja["ID"] = bd.LerInt("LojaID");
                        loja["Nome"] = bd.LerString("Loja");
                        loja["CanalID"] = bd.LerInt("CanalID");
                        lojas.Rows.Add(loja);
                    }

                    DataRow usuario = usuarios.NewRow();
                    usuario["ID"] = bd.LerInt("UsuarioID");
                    usuario["Nome"] = bd.LerString("Usuario");
                    usuario["LojaID"] = bd.LerInt("LojaID");
                    usuarios.Rows.Add(usuario);

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarUsuarios(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));
                canais.Columns.Add("TaxaConveniencia", typeof(int));
                canais.Columns.Add("EmpresaID", typeof(int));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                BD bd = new BD();

                string sql = "SELECT DISTINCT tCanal.ID AS CanalID, tCanal.Nome AS Canal, tCanal.TaxaConveniencia, tLoja.ID AS LojaID, tLoja.Nome AS Loja, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario " +
                    "FROM tEmpresa (NOLOCK),tCanal (NOLOCK),tLoja (NOLOCK),tCaixa (NOLOCK),tUsuario (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tEmpresa.ID=" + empresaID + " " +
                    "ORDER BY tCanal.Nome,tLoja.Nome,tUsuario.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (canais.Select("ID=" + bd.LerInt("CanalID")).Length == 0)
                    {
                        DataRow canal = canais.NewRow();
                        canal["ID"] = bd.LerInt("CanalID");
                        canal["Nome"] = bd.LerString("Canal");
                        canal["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        canal["EmpresaID"] = bd.LerInt("EmpresaID");
                        canais.Rows.Add(canal);
                    }

                    if (lojas.Select("CanalID=" + bd.LerInt("CanalID") + " AND ID=" + bd.LerInt("LojaID")).Length == 0)
                    {
                        DataRow loja = lojas.NewRow();
                        loja["ID"] = bd.LerInt("LojaID");
                        loja["Nome"] = bd.LerString("Loja");
                        loja["CanalID"] = bd.LerInt("CanalID");
                        lojas.Rows.Add(loja);
                    }

                    DataRow usuario = usuarios.NewRow();
                    usuario["ID"] = bd.LerInt("UsuarioID");
                    usuario["Nome"] = bd.LerString("Usuario");
                    usuario["LojaID"] = bd.LerInt("LojaID");
                    usuarios.Rows.Add(usuario);

                }
                bd.Fechar();

                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarCaixas()
        {

            try
            {

                if (empresaID != 0)
                    return carregarCaixas(empresaID);
                else if (canalID != 0)
                    if (usuarioID != 0)
                        return carregarUsuarioCaixas(canalID, usuarioID);
                    else
                        return carregarCanalCaixas(canalID);
                else if (usuarioID != 0)
                    return carregarUsuarioCaixas(usuarioID);
                else if (localID != 0)
                    return carregarLocalCaixas(localID);

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));
                canais.Columns.Add("EmpresaID", typeof(int));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                BD bd = new BD();

                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tCanal.ID AS CanalID, tCanal.Nome AS Canal, tLoja.ID AS LojaID, tLoja.Nome AS Loja, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario, tCaixa.ID AS CaixaID, tCaixa.DataAbertura, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tCanal (NOLOCK),tLoja (NOLOCK),tCaixa (NOLOCK),tUsuario (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID " +
                    "ORDER BY tEmpresa.Nome,tCanal.Nome,tLoja.Nome,tUsuario.Nome,tCaixa.ID DESC";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresas.Rows.Add(empresa);
                    }

                    if (canais.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("CanalID")).Length == 0)
                    {
                        DataRow canal = canais.NewRow();
                        canal["ID"] = bd.LerInt("CanalID");
                        canal["Nome"] = bd.LerString("Canal");
                        canal["EmpresaID"] = bd.LerInt("EmpresaID");
                        canais.Rows.Add(canal);
                    }

                    if (lojas.Select("CanalID=" + bd.LerInt("CanalID") + " AND ID=" + bd.LerInt("LojaID")).Length == 0)
                    {
                        DataRow loja = lojas.NewRow();
                        loja["ID"] = bd.LerInt("LojaID");
                        loja["Nome"] = bd.LerString("Loja");
                        loja["CanalID"] = bd.LerInt("CanalID");
                        lojas.Rows.Add(loja);
                    }

                    if (usuarios.Select("LojaID=" + bd.LerInt("LojaID") + " AND ID=" + bd.LerInt("UsuarioID")).Length == 0)
                    {
                        DataRow usuario = usuarios.NewRow();
                        usuario["ID"] = bd.LerInt("UsuarioID");
                        usuario["Nome"] = bd.LerString("Usuario");
                        usuario["LojaID"] = bd.LerInt("LojaID");
                        usuarios.Rows.Add(usuario);
                    }

                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarCaixas(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                BD bd = new BD();

                string sql = "SELECT tCanal.ID AS CanalID, tCanal.Nome AS Canal, tLoja.ID AS LojaID, tLoja.Nome AS Loja, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario, tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                    "FROM tEmpresa (NOLOCK),tCanal (NOLOCK),tLoja (NOLOCK),tCaixa (NOLOCK),tUsuario (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tEmpresa.ID=" + empresaID + " " +
                    "ORDER BY tCanal.Nome,tLoja.Nome,tUsuario.Nome,tCaixa.ID DESC";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (canais.Select("ID=" + bd.LerInt("CanalID")).Length == 0)
                    {
                        DataRow canal = canais.NewRow();
                        canal["ID"] = bd.LerInt("CanalID");
                        canal["Nome"] = bd.LerString("Canal");
                        canais.Rows.Add(canal);
                    }

                    if (lojas.Select("CanalID=" + bd.LerInt("CanalID") + " AND ID=" + bd.LerInt("LojaID")).Length == 0)
                    {
                        DataRow loja = lojas.NewRow();
                        loja["ID"] = bd.LerInt("LojaID");
                        loja["Nome"] = bd.LerString("Loja");
                        loja["CanalID"] = bd.LerInt("CanalID");
                        lojas.Rows.Add(loja);
                    }

                    if (usuarios.Select("LojaID=" + bd.LerInt("LojaID") + " AND ID=" + bd.LerInt("UsuarioID")).Length == 0)
                    {
                        DataRow usuario = usuarios.NewRow();
                        usuario["ID"] = bd.LerInt("UsuarioID");
                        usuario["Nome"] = bd.LerString("Usuario");
                        usuario["LojaID"] = bd.LerInt("LojaID");
                        usuarios.Rows.Add(usuario);
                    }

                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);

                }
                bd.Fechar();

                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarLocalCaixas(int localID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                BD bd = new BD();

                string sql = "SELECT tCanal.ID AS CanalID, tCanal.Nome AS Canal, tLoja.ID AS LojaID, tLoja.Nome AS Loja, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario, tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                    "FROM tCanal (NOLOCK),tLoja (NOLOCK),tCaixa (NOLOCK),tUsuario (NOLOCK),tLocal (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tLocal.EmpresaID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tLocal.ID=" + localID + " " +
                    "ORDER BY tCanal.Nome,tLoja.Nome,tUsuario.Nome,tCaixa.ID DESC";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (canais.Select("ID=" + bd.LerInt("CanalID")).Length == 0)
                    {
                        DataRow canal = canais.NewRow();
                        canal["ID"] = bd.LerInt("CanalID");
                        canal["Nome"] = bd.LerString("Canal");
                        canais.Rows.Add(canal);
                    }

                    if (lojas.Select("CanalID=" + bd.LerInt("CanalID") + " AND ID=" + bd.LerInt("LojaID")).Length == 0)
                    {
                        DataRow loja = lojas.NewRow();
                        loja["ID"] = bd.LerInt("LojaID");
                        loja["Nome"] = bd.LerString("Loja");
                        loja["CanalID"] = bd.LerInt("CanalID");
                        lojas.Rows.Add(loja);
                    }

                    if (usuarios.Select("LojaID=" + bd.LerInt("LojaID") + " AND ID=" + bd.LerInt("UsuarioID")).Length == 0)
                    {
                        DataRow usuario = usuarios.NewRow();
                        usuario["ID"] = bd.LerInt("UsuarioID");
                        usuario["Nome"] = bd.LerString("Usuario");
                        usuario["LojaID"] = bd.LerInt("LojaID");
                        usuarios.Rows.Add(usuario);
                    }

                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);

                }
                bd.Fechar();

                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarCanalCaixas(int canalID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                BD bd = new BD();

                string sql = "SELECT tLoja.ID AS LojaID, tLoja.Nome AS Loja, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario, tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                    "FROM tLoja (NOLOCK),tCaixa (NOLOCK),tUsuario (NOLOCK) " +
                    "WHERE tLoja.CanalID=" + canalID + " AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID " +
                    "ORDER BY tLoja.Nome,tUsuario.Nome,tCaixa.ID DESC";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (lojas.Select("ID=" + bd.LerInt("LojaID")).Length == 0)
                    {
                        DataRow loja = lojas.NewRow();
                        loja["ID"] = bd.LerInt("LojaID");
                        loja["Nome"] = bd.LerString("Loja");
                        lojas.Rows.Add(loja);
                    }

                    if (usuarios.Select("LojaID=" + bd.LerInt("LojaID") + " AND ID=" + bd.LerInt("UsuarioID")).Length == 0)
                    {
                        DataRow usuario = usuarios.NewRow();
                        usuario["ID"] = bd.LerInt("UsuarioID");
                        usuario["Nome"] = bd.LerString("Usuario");
                        usuario["LojaID"] = bd.LerInt("LojaID");
                        usuarios.Rows.Add(usuario);
                    }

                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);

                }
                bd.Fechar();

                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarUsuarioCaixas(int usuarioID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));

                BD bd = new BD();

                string sql = "SELECT tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                    "FROM tCaixa (NOLOCK) " +
                    "WHERE tCaixa.UsuarioID=" + usuarioID + " " +
                    "ORDER BY tCaixa.ID DESC";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixas.Rows.Add(caixa);

                }
                bd.Fechar();

                buffer.Tables.Add(caixas);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarUsuarioCaixas(int canalID, int usuarioID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));

                BD bd = new BD();

                string sql = "SELECT tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                    "FROM tLoja (NOLOCK),tCaixa (NOLOCK) " +
                    "WHERE tLoja.CanalID=" + canalID + " AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=" + usuarioID + " " +
                    "ORDER BY tCaixa.ID DESC";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixas.Rows.Add(caixa);

                }
                bd.Fechar();

                buffer.Tables.Add(caixas);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarLocais()
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));
                locais.Columns.Add("EmpresaID", typeof(int));

                BD bd = new BD();

                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tLocal.ID AS LocalID, tLocal.Nome AS Local, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID " +
                    "ORDER BY tEmpresa.Nome,tLocal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresa["RegionalID"] = bd.LerInt("RegionalID");
                        empresas.Rows.Add(empresa);
                    }

                    DataRow local = locais.NewRow();
                    local["ID"] = bd.LerInt("LocalID");
                    local["Nome"] = bd.LerString("Local");
                    local["EmpresaID"] = bd.LerInt("EmpresaID");
                    locais.Rows.Add(local);

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(locais);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarEventos()
        {

            try
            {

                if (empresaID != 0)
                    return carregarEventos(empresaID);
                else if (localID != 0)
                    return carregarLocalEventos(localID);

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));
                locais.Columns.Add("EmpresaID", typeof(int));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("LocalID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string sql = "SELECT DISTINCT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tLocal.ID AS LocalID, tLocal.Nome AS Local, tEvento.ID AS EventoID, tEvento.Nome AS Evento, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEmpresa.Nome,tLocal.Nome,tEvento.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresas.Rows.Add(empresa);
                    }

                    if (locais.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("LocalID")).Length == 0)
                    {
                        DataRow local = locais.NewRow();
                        local["ID"] = bd.LerInt("LocalID");
                        local["Nome"] = bd.LerString("Local");
                        local["EmpresaID"] = bd.LerInt("EmpresaID");
                        locais.Rows.Add(local);
                    }

                    DataRow evento = eventos.NewRow();
                    evento["ID"] = bd.LerInt("EventoID");
                    evento["Nome"] = bd.LerString("Evento");
                    evento["LocalID"] = bd.LerInt("LocalID");
                    eventos.Rows.Add(evento);

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarEventos(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("LocalID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroEmpresa = (empresaID > 0) ? "AND tEmpresa.ID=" + empresaID + " " : "";

                string sql = "SELECT DISTINCT tLocal.ID AS LocalID, tLocal.Nome AS Local, tEvento.ID AS EventoID, tEvento.Nome AS Evento " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " + filtroEmpresa +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tLocal.Nome,tEvento.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (locais.Select("ID=" + bd.LerInt("LocalID")).Length == 0)
                    {
                        DataRow local = locais.NewRow();
                        local["ID"] = bd.LerInt("LocalID");
                        local["Nome"] = bd.LerString("Local");
                        locais.Rows.Add(local);
                    }

                    DataRow evento = eventos.NewRow();
                    evento["ID"] = bd.LerInt("EventoID");
                    evento["Nome"] = bd.LerString("Evento");
                    evento["LocalID"] = bd.LerInt("LocalID");
                    eventos.Rows.Add(evento);

                }
                bd.Fechar();

                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarLocalEventos(int localID)
        {

            if (localID <= 0)
                throw new AutoSelecionadorGerenciadorException("LocalID deve ser maior que zero.");

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string sql = "SELECT DISTINCT tEvento.ID AS EventoID, tEvento.Nome AS Evento " +
                    "FROM tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=" + localID + " " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEvento.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow evento = eventos.NewRow();
                    evento["ID"] = bd.LerInt("EventoID");
                    evento["Nome"] = bd.LerString("Evento");
                    eventos.Rows.Add(evento);

                }
                bd.Fechar();

                buffer.Tables.Add(eventos);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarEmpresaEventos()
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("EmpresaID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string clausulaRegional = string.Empty;
                if (this.RegionalID != 0)
                    clausulaRegional = "AND tEmpresa.RegionalID = " + this.RegionalID + " ";

                string sql = "SELECT DISTINCT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tEvento.ID AS EventoID, tEvento.Nome AS Evento , tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    clausulaRegional +
                    " ORDER BY tEmpresa.Nome,tEvento.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresa["RegionalID"] = bd.LerInt("RegionalID");
                        empresas.Rows.Add(empresa);
                    }

                    DataRow evento = eventos.NewRow();
                    evento["ID"] = bd.LerInt("EventoID");
                    evento["Nome"] = bd.LerString("Evento");
                    evento["EmpresaID"] = bd.LerInt("EmpresaID");
                    eventos.Rows.Add(evento);

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(eventos);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarApresentacoes()
        {

            BD bd = new BD();
            try
            {

                if (empresaID != 0)
                    return carregarApresentacoes(empresaID);
                else if (localID != 0)
                    return carregarLocalApresentacoes(localID);

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));
                locais.Columns.Add("EmpresaID", typeof(int));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("LocalID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));



                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string clausulaRegional = string.Empty;
                if (this.RegionalID != 0)
                    clausulaRegional = "AND tEmpresa.RegionalID = " + this.RegionalID + " ";


                //Empresas
                string sql = "SELECT DISTINCT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tEmpresa.RegionalID  " +
                          "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                          "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " + hoje +
                          disponivelVenda +
                          disponivelAjuste +
                          disponivelRelatorio +
                          clausulaRegional +
                          " ORDER BY tEmpresa.Nome";


                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow empresa = empresas.NewRow();
                    empresa["ID"] = bd.LerInt("EmpresaID");
                    empresa["Nome"] = bd.LerString("Empresa");
                    empresa["RegionalID"] = bd.LerInt("RegionalID");
                    empresas.Rows.Add(empresa);
                }
                bd.FecharConsulta();

                // Local
                sql = "SELECT DISTINCT  tEmpresa.ID AS EmpresaID, tLocal.ID AS LocalID, tLocal.Nome AS Local " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    clausulaRegional +
                    " ORDER BY tLocal.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow local = locais.NewRow();
                    local["ID"] = bd.LerInt("LocalID");
                    local["Nome"] = bd.LerString("Local");
                    local["EmpresaID"] = bd.LerInt("EmpresaID");
                    locais.Rows.Add(local);
                }
                bd.FecharConsulta();

                // Evento
                sql = "SELECT DISTINCT tEvento.LocalID, tEvento.ID AS EventoID, tEvento.Nome AS Evento " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    clausulaRegional +
                    " ORDER BY tEvento.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow evento = eventos.NewRow();
                    evento["ID"] = bd.LerInt("EventoID");
                    evento["Nome"] = bd.LerString("Evento");
                    evento["LocalID"] = bd.LerInt("LocalID");
                    eventos.Rows.Add(evento);
                }
                bd.FecharConsulta();

                // Apresentações
                sql = "SELECT DISTINCT tApresentacao.EventoID, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    clausulaRegional +
                    " ORDER BY tApresentacao.Horario";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow apresentacao = apresentacoes.NewRow();
                    apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                    apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                    apresentacao["EventoID"] = bd.LerInt("EventoID");
                    apresentacoes.Rows.Add(apresentacao);
                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }


        }

        private DataSet carregarApresentacoes(int empresaID)
        {

            BD bd = new BD();

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("LocalID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroEmpresa = (empresaID > 0) ? "AND tEmpresa.ID=" + empresaID + " " : "";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                // Local
                string sql = "SELECT DISTINCT tLocal.ID AS LocalID, tLocal.Nome AS Local " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " + filtroEmpresa + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tLocal.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow local = locais.NewRow();
                    local["ID"] = bd.LerInt("LocalID");
                    local["Nome"] = bd.LerString("Local");
                    locais.Rows.Add(local);
                }
                bd.FecharConsulta();

                // Evento
                sql = "SELECT DISTINCT tEvento.LocalID, tEvento.ID AS EventoID, tEvento.Nome AS Evento " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " + filtroEmpresa + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEvento.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow evento = eventos.NewRow();
                    evento["ID"] = bd.LerInt("EventoID");
                    evento["Nome"] = bd.LerString("Evento");
                    evento["LocalID"] = bd.LerInt("LocalID");
                    eventos.Rows.Add(evento);
                }
                bd.FecharConsulta();

                // Apresentações
                sql = "SELECT DISTINCT tApresentacao.EventoID, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " + filtroEmpresa + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tApresentacao.Horario";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow apresentacao = apresentacoes.NewRow();
                    apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                    apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                    apresentacao["EventoID"] = bd.LerInt("EventoID");
                    apresentacoes.Rows.Add(apresentacao);
                }
                bd.FecharConsulta();

                bd.Fechar();

                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        private DataSet carregarLocalApresentacoes(int localID)
        {

            if (localID <= 0)
                throw new AutoSelecionadorGerenciadorException("LocalID deve ser maior que zero.");

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string sql = "SELECT tEvento.ID AS EventoID, tEvento.Nome AS Evento, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao " +
                    "FROM tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=" + localID + " " + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEvento.Nome,tApresentacao.Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (eventos.Select("ID=" + bd.LerInt("EventoID")).Length == 0)
                    {
                        DataRow evento = eventos.NewRow();
                        evento["ID"] = bd.LerInt("EventoID");
                        evento["Nome"] = bd.LerString("Evento");
                        eventos.Rows.Add(evento);
                    }

                    DataRow apresentacao = apresentacoes.NewRow();
                    apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                    apresentacao["Nome"] = bd.LerStringFormatoDataHora("Apresentacao");
                    apresentacao["EventoID"] = bd.LerInt("EventoID");
                    apresentacoes.Rows.Add(apresentacao);

                }
                bd.Fechar();

                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarEmpresaApresentacoes()
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("EmpresaID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tEvento.ID AS EventoID, tEvento.Nome AS Evento, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEmpresa.Nome,tEvento.Nome,tApresentacao.Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresa["RegionalID"] = bd.LerInt("RegionalID");
                        empresas.Rows.Add(empresa);
                    }

                    if (eventos.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("EventoID")).Length == 0)
                    {
                        DataRow evento = eventos.NewRow();
                        evento["ID"] = bd.LerInt("EventoID");
                        evento["Nome"] = bd.LerString("Evento");
                        evento["EmpresaID"] = bd.LerInt("EmpresaID");
                        eventos.Rows.Add(evento);
                    }

                    DataRow apresentacao = apresentacoes.NewRow();
                    apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                    apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                    apresentacao["EventoID"] = bd.LerInt("EventoID");
                    apresentacoes.Rows.Add(apresentacao);

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarSetores()
        {

            try
            {

                if (empresaID != 0)
                    return carregarSetores(empresaID);
                else if (localID != 0)
                    return carregarLocalSetores(localID);
                else if (canalID != 0)
                    return carregarCanalSetores(canalID);

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));
                locais.Columns.Add("EmpresaID", typeof(int));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("LocalID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("Produto", typeof(bool));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroSetor;
                if (tipoSetores == TipoSetores.Marcados)
                    filtroSetor = "AND tSetor.LugarMarcado<>'" + Setor.Pista + "' ";
                else if (tipoSetores == TipoSetores.NaoMarcados)
                    filtroSetor = "AND tSetor.LugarMarcado='" + Setor.Pista + "' ";
                else
                    filtroSetor = "";

                if (tipoSetoresDesc != "")
                    tipoSetoresDesc = "AND tSetor.LugarMarcado in (" + tipoSetoresDesc + ") ";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tLocal.ID AS LocalID, tLocal.Nome AS Local, tEvento.ID AS EventoID, tEvento.Nome AS Evento, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao, tSetor.ID AS SetorID, tSetor.Nome AS Setor, tSetor.Produto, tSetor.LugarMarcado, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEmpresa.Nome,tLocal.Nome,tEvento.Nome,tApresentacao.Horario,tSetor.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresas.Rows.Add(empresa);
                    }

                    if (locais.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("LocalID")).Length == 0)
                    {
                        DataRow local = locais.NewRow();
                        local["ID"] = bd.LerInt("LocalID");
                        local["Nome"] = bd.LerString("Local");
                        local["EmpresaID"] = bd.LerInt("EmpresaID");
                        locais.Rows.Add(local);
                    }

                    if (eventos.Select("LocalID=" + bd.LerInt("LocalID") + " AND ID=" + bd.LerInt("EventoID")).Length == 0)
                    {
                        DataRow evento = eventos.NewRow();
                        evento["ID"] = bd.LerInt("EventoID");
                        evento["Nome"] = bd.LerString("Evento");
                        evento["LocalID"] = bd.LerInt("LocalID");
                        eventos.Rows.Add(evento);
                    }

                    if (apresentacoes.Select("EventoID=" + bd.LerInt("EventoID") + " AND ID=" + bd.LerInt("ApresentacaoID")).Length == 0)
                    {
                        DataRow apresentacao = apresentacoes.NewRow();
                        apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                        apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                        apresentacao["EventoID"] = bd.LerInt("EventoID");
                        apresentacoes.Rows.Add(apresentacao);
                    }

                    if (setores.Select("ApresentacaoID=" + bd.LerInt("ApresentacaoID") + " AND ID=" + bd.LerInt("SetorID")).Length == 0)
                    {
                        DataRow setor = setores.NewRow();
                        setor["ID"] = bd.LerInt("SetorID");
                        setor["Nome"] = bd.LerString("Setor");
                        setor["Produto"] = bd.LerBoolean("Produto");
                        setor["LugarMarcado"] = bd.LerString("LugarMarcado");
                        setor["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        setores.Rows.Add(setor);
                    }

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarSetores(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("LocalID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("Produto", typeof(bool));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroEmpresa = (empresaID > 0) ? "AND tEmpresa.ID=" + empresaID + " " : "";
                string filtroSetor;
                if (tipoSetores == TipoSetores.Marcados)
                    filtroSetor = "AND tSetor.LugarMarcado<>'" + Setor.Pista + "' ";
                else if (tipoSetores == TipoSetores.NaoMarcados)
                    filtroSetor = "AND tSetor.LugarMarcado='" + Setor.Pista + "' ";
                else
                    filtroSetor = "";

                if (tipoSetoresDesc != "")
                    tipoSetoresDesc = "AND tSetor.LugarMarcado in (" + tipoSetoresDesc + ") ";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string sql = "SELECT tLocal.ID AS LocalID, tLocal.Nome AS Local, tEvento.ID AS EventoID, tEvento.Nome AS Evento, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao, tSetor.ID AS SetorID, tSetor.Nome AS Setor, tSetor.Produto, tSetor.LugarMarcado " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID " + filtroSetor + tipoSetoresDesc + " " + filtroEmpresa + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tLocal.Nome,tEvento.Nome,tApresentacao.Horario,tSetor.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (locais.Select("ID=" + bd.LerInt("LocalID")).Length == 0)
                    {
                        DataRow local = locais.NewRow();
                        local["ID"] = bd.LerInt("LocalID");
                        local["Nome"] = bd.LerString("Local");
                        locais.Rows.Add(local);
                    }

                    if (eventos.Select("LocalID=" + bd.LerInt("LocalID") + " AND ID=" + bd.LerInt("EventoID")).Length == 0)
                    {
                        DataRow evento = eventos.NewRow();
                        evento["ID"] = bd.LerInt("EventoID");
                        evento["Nome"] = bd.LerString("Evento");
                        evento["LocalID"] = bd.LerInt("LocalID");
                        eventos.Rows.Add(evento);
                    }

                    if (apresentacoes.Select("EventoID=" + bd.LerInt("EventoID") + " AND ID=" + bd.LerInt("ApresentacaoID")).Length == 0)
                    {
                        DataRow apresentacao = apresentacoes.NewRow();
                        apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                        apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                        apresentacao["EventoID"] = bd.LerInt("EventoID");
                        apresentacoes.Rows.Add(apresentacao);
                    }

                    if (setores.Select("ApresentacaoID=" + bd.LerInt("ApresentacaoID") + " AND ID=" + bd.LerInt("SetorID")).Length == 0)
                    {
                        DataRow setor = setores.NewRow();
                        setor["ID"] = bd.LerInt("SetorID");
                        setor["Nome"] = bd.LerString("Setor");
                        setor["Produto"] = bd.LerBoolean("Produto");
                        setor["LugarMarcado"] = bd.LerString("LugarMarcado");
                        setor["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        setores.Rows.Add(setor);
                    }

                }
               bd.Fechar();

                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarLocalSetores(int localID)
        {

            DateTime inicio = DateTime.Now;
            BD bd = new BD();

            if (localID <= 0)
                throw new AutoSelecionadorGerenciadorException("LocalID deve ser maior que zero.");

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("Produto", typeof(bool));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));
                setores.Columns.Add("VersaoBackground", typeof(int)).DefaultValue = 0;


                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroSetor;
                if (tipoSetores == TipoSetores.Marcados)
                    filtroSetor = "AND tSetor.LugarMarcado<>'" + Setor.Pista + "' ";
                else if (tipoSetores == TipoSetores.NaoMarcados)
                    filtroSetor = "AND tSetor.LugarMarcado='" + Setor.Pista + "' ";
                else
                    filtroSetor = "";

                if (tipoSetoresDesc != "")
                    tipoSetoresDesc = "AND tSetor.LugarMarcado in (" + tipoSetoresDesc + ") ";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }


                string sql = "";


                // Evento
                sql = "SELECT DISTINCT tEvento.ID AS EventoID, tEvento.Nome AS Evento " +
                    "FROM tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK) " +
                    "WHERE tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND tEvento.LocalID=" + localID + " " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEvento.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow evento = eventos.NewRow();
                    evento["ID"] = bd.LerInt("EventoID");
                    evento["Nome"] = bd.LerString("Evento");
                    eventos.Rows.Add(evento);
                }


                // Apresentações
                sql = "SELECT DISTINCT tApresentacao.EventoID, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao " +
                    "FROM tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK) " +
                    "WHERE tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND tEvento.LocalID=" + localID + " " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tApresentacao.Horario";

                bd.Consulta().Close();
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow apresentacao = apresentacoes.NewRow();
                    apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                    apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                    apresentacao["EventoID"] = bd.LerInt("EventoID");
                    apresentacoes.Rows.Add(apresentacao);
                }
                bd.Consulta().Close();

                // Setores
                sql = "SELECT DISTINCT tApresentacaoSetor.ApresentacaoID, tSetor.ID AS SetorID, tSetor.Nome AS Setor, tSetor.Produto, tSetor.LugarMarcado, tSetor.VersaoBackground " +
                    "FROM tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK) " +
                    "WHERE tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND tEvento.LocalID=" + localID + " " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tSetor.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow setor = setores.NewRow();
                    setor["ID"] = bd.LerInt("SetorID");
                    setor["Nome"] = bd.LerString("Setor");
                    setor["Produto"] = bd.LerBoolean("Produto");
                    setor["LugarMarcado"] = bd.LerString("LugarMarcado");
                    setor["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    setor["VersaoBackground"] = bd.LerInt("VersaoBackground");
                    setores.Rows.Add(setor);
                }
                bd.Fechar();

                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);

                double tempo = ((TimeSpan)(DateTime.Now - inicio)).TotalMinutes;

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        private DataSet carregarCanalSetores(int canalID)
        {

            DateTime inicio = DateTime.Now;

            BD bd = new BD();


            if (canalID <= 0)
                throw new AutoSelecionadorGerenciadorException("CanalID deve ser maior que zero.");

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("Produto", typeof(bool));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));


                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroSetor;
                if (tipoSetores == TipoSetores.Marcados)
                    filtroSetor = "AND tSetor.LugarMarcado<>'" + Setor.Pista + "' ";
                else if (tipoSetores == TipoSetores.NaoMarcados)
                    filtroSetor = "AND tSetor.LugarMarcado='" + Setor.Pista + "' ";
                else
                    filtroSetor = "";

                if (tipoSetoresDesc != "")
                    tipoSetoresDesc = "AND tSetor.LugarMarcado in (" + tipoSetoresDesc + ") ";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string sql = "";




                // Evento
                sql = "SELECT DISTINCT tEvento.ID AS EventoID, tEvento.Nome AS Evento " +
                    "FROM tCanalEvento (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK) " +
                    "WHERE tEvento.ID=tApresentacao.EventoID AND tEvento.ID=tCanalEvento.EventoID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND tCanalEvento.CanalID=" + canalID + " " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEvento.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow evento = eventos.NewRow();
                    evento["ID"] = bd.LerInt("EventoID");
                    evento["Nome"] = bd.LerString("Evento");
                    eventos.Rows.Add(evento);
                }


                // Apresentações
                sql = "SELECT DISTINCT tApresentacao.EventoID, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao " +
                    "FROM tCanalEvento (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK) " +
                    "WHERE tEvento.ID=tApresentacao.EventoID AND tEvento.ID=tCanalEvento.EventoID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND tCanalEvento.CanalID=" + canalID + " " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tApresentacao.Horario ";

                bd.Consulta().Close();
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow apresentacao = apresentacoes.NewRow();
                    apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                    apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                    apresentacao["EventoID"] = bd.LerInt("EventoID");
                    apresentacoes.Rows.Add(apresentacao);
                }

                // Setores
                sql = "SELECT DISTINCT tApresentacao.ID AS ApresentacaoID, tSetor.ID AS SetorID, tSetor.Nome AS Setor, tSetor.Produto, tSetor.LugarMarcado " +
                    "FROM tCanalEvento (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK) " +
                    "WHERE tEvento.ID=tApresentacao.EventoID AND tEvento.ID=tCanalEvento.EventoID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND tCanalEvento.CanalID=" + canalID + " " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tSetor.Nome";

                bd.Consulta().Close();
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow setor = setores.NewRow();
                    setor["ID"] = bd.LerInt("SetorID");
                    setor["Nome"] = bd.LerString("Setor");
                    setor["Produto"] = bd.LerBoolean("Produto");
                    setor["LugarMarcado"] = bd.LerString("LugarMarcado");
                    setor["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    setores.Rows.Add(setor);
                }

                bd.Fechar();

                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);

                double total = ((TimeSpan)(DateTime.Now - inicio)).TotalMinutes;

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public DataSet CarregarEmpresaSetores()
        {


            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("EmpresaID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("Produto", typeof(bool));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroSetor;
                if (tipoSetores == TipoSetores.Marcados)
                    filtroSetor = "AND tSetor.LugarMarcado<>'" + Setor.Pista + "' ";
                else if (tipoSetores == TipoSetores.NaoMarcados)
                    filtroSetor = "AND tSetor.LugarMarcado='" + Setor.Pista + "' ";
                else
                    filtroSetor = "";

                if (tipoSetoresDesc != "")
                    tipoSetoresDesc = "AND tSetor.LugarMarcado in (" + tipoSetoresDesc + ") ";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tEvento.ID AS EventoID, tEvento.Nome AS Evento, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao, tSetor.ID AS SetorID, tSetor.Nome AS Setor, tSetor.Produto, tSetor.LugarMarcado , tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEmpresa.Nome,tEvento.Nome,tApresentacao.Horario,tSetor.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresa["RegionalID"] = bd.LerInt("RegionalID");
                        empresas.Rows.Add(empresa);
                    }

                    if (eventos.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("EventoID")).Length == 0)
                    {
                        DataRow evento = eventos.NewRow();
                        evento["ID"] = bd.LerInt("EventoID");
                        evento["Nome"] = bd.LerString("Evento");
                        evento["EmpresaID"] = bd.LerInt("EmpresaID");
                        eventos.Rows.Add(evento);
                    }

                    if (apresentacoes.Select("EventoID=" + bd.LerInt("EventoID") + " AND ID=" + bd.LerInt("ApresentacaoID")).Length == 0)
                    {
                        DataRow apresentacao = apresentacoes.NewRow();
                        apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                        apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                        apresentacao["EventoID"] = bd.LerInt("EventoID");
                        apresentacoes.Rows.Add(apresentacao);
                    }

                    if (setores.Select("ApresentacaoID=" + bd.LerInt("ApresentacaoID") + " AND ID=" + bd.LerInt("SetorID")).Length == 0)
                    {
                        DataRow setor = setores.NewRow();
                        setor["ID"] = bd.LerInt("SetorID");
                        setor["Nome"] = bd.LerString("Setor");
                        setor["Produto"] = bd.LerBoolean("Produto");
                        setor["LugarMarcado"] = bd.LerString("LugarMarcado");
                        setor["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        setores.Rows.Add(setor);
                    }

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());


                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarEmpresaLocaisSetores()
        {


            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));
                locais.Columns.Add("EmpresaID", typeof(int));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("LocalID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("Produto", typeof(bool));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroSetor;
                if (tipoSetores == TipoSetores.Marcados)
                    filtroSetor = "AND tSetor.LugarMarcado<>'" + Setor.Pista + "' ";
                else if (tipoSetores == TipoSetores.NaoMarcados)
                    filtroSetor = "AND tSetor.LugarMarcado='" + Setor.Pista + "' ";
                else
                    filtroSetor = "";

                if (tipoSetoresDesc != "")
                    tipoSetoresDesc = "AND tSetor.LugarMarcado in (" + tipoSetoresDesc + ") ";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tLocal.ID AS LocalID, tLocal.Nome AS Local, tEvento.ID AS EventoID, tEvento.Nome AS Evento, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao, tSetor.ID AS SetorID, tSetor.Nome AS Setor, tSetor.Produto, tSetor.LugarMarcado, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEmpresa.Nome,tEvento.Nome,tApresentacao.Horario,tSetor.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresa["RegionalID"] = bd.LerInt("RegionalID");
                        empresas.Rows.Add(empresa);
                    }

                    if (locais.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("LocalID")).Length == 0)
                    {
                        DataRow local = locais.NewRow();
                        local["ID"] = bd.LerInt("LocalID");
                        local["Nome"] = bd.LerString("Local");
                        local["EmpresaID"] = bd.LerInt("EmpresaID");
                        locais.Rows.Add(local);
                    }

                    if (eventos.Select("LocalID=" + bd.LerInt("LocalID") + " AND ID=" + bd.LerInt("EventoID")).Length == 0)
                    {
                        DataRow evento = eventos.NewRow();
                        evento["ID"] = bd.LerInt("EventoID");
                        evento["Nome"] = bd.LerString("Evento");
                        evento["LocalID"] = bd.LerInt("LocalID");
                        eventos.Rows.Add(evento);
                    }

                    if (apresentacoes.Select("EventoID=" + bd.LerInt("EventoID") + " AND ID=" + bd.LerInt("ApresentacaoID")).Length == 0)
                    {
                        DataRow apresentacao = apresentacoes.NewRow();
                        apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                        apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                        apresentacao["EventoID"] = bd.LerInt("EventoID");
                        apresentacoes.Rows.Add(apresentacao);
                    }

                    if (setores.Select("ApresentacaoID=" + bd.LerInt("ApresentacaoID") + " AND ID=" + bd.LerInt("SetorID")).Length == 0)
                    {
                        DataRow setor = setores.NewRow();
                        setor["ID"] = bd.LerInt("SetorID");
                        setor["Nome"] = bd.LerString("Setor");
                        setor["Produto"] = bd.LerBoolean("Produto");
                        setor["LugarMarcado"] = bd.LerString("LugarMarcado");
                        setor["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        setores.Rows.Add(setor);
                    }

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);

                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarPrecos()
        {

            try
            {

                if (empresaID != 0)
                    return carregarPrecos(empresaID);
                else if (localID != 0)
                    return carregarLocalPrecos(localID);

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));
                locais.Columns.Add("EmpresaID", typeof(int));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("LocalID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("Produto", typeof(bool));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));

                DataTable precos = new DataTable("Preco");
                precos.Columns.Add("ID", typeof(int));
                precos.Columns.Add("Nome", typeof(string));
                precos.Columns.Add("CorID", typeof(int));
                precos.Columns.Add("Valor", typeof(decimal));
                precos.Columns.Add("ApresentacaoSetorID", typeof(int));
                precos.Columns.Add("ApresentacaoID", typeof(int));
                precos.Columns.Add("SetorID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroSetor;
                if (tipoSetores == TipoSetores.Marcados)
                    filtroSetor = "AND tSetor.LugarMarcado<>'" + Setor.Pista + "' ";
                else if (tipoSetores == TipoSetores.NaoMarcados)
                    filtroSetor = "AND tSetor.LugarMarcado='" + Setor.Pista + "' ";
                else
                    filtroSetor = "";

                if (tipoSetoresDesc != "")
                    tipoSetoresDesc = "AND tSetor.LugarMarcado in (" + tipoSetoresDesc + ") ";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tLocal.ID AS LocalID, tLocal.Nome AS Local, tEvento.ID AS EventoID, tEvento.Nome AS Evento, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao, tSetor.ID AS SetorID, tSetor.Nome AS Setor, tSetor.Produto, tSetor.LugarMarcado, tPreco.ID AS PrecoID, tPreco.Nome AS Preco, tPreco.Valor, tPreco.CorID, tPreco.ApresentacaoSetorID, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK),tPreco (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND tPreco.ApresentacaoSetorID=tApresentacaoSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEmpresa.Nome,tLocal.Nome,tEvento.Nome,tApresentacao.Horario,tSetor.Nome,tPreco.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresas.Rows.Add(empresa);
                    }

                    if (locais.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("LocalID")).Length == 0)
                    {
                        DataRow local = locais.NewRow();
                        local["ID"] = bd.LerInt("LocalID");
                        local["Nome"] = bd.LerString("Local");
                        local["EmpresaID"] = bd.LerInt("EmpresaID");
                        locais.Rows.Add(local);
                    }

                    if (eventos.Select("LocalID=" + bd.LerInt("LocalID") + " AND ID=" + bd.LerInt("EventoID")).Length == 0)
                    {
                        DataRow evento = eventos.NewRow();
                        evento["ID"] = bd.LerInt("EventoID");
                        evento["Nome"] = bd.LerString("Evento");
                        evento["LocalID"] = bd.LerInt("LocalID");
                        eventos.Rows.Add(evento);
                    }

                    if (apresentacoes.Select("EventoID=" + bd.LerInt("EventoID") + " AND ID=" + bd.LerInt("ApresentacaoID")).Length == 0)
                    {
                        DataRow apresentacao = apresentacoes.NewRow();
                        apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                        apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                        apresentacao["EventoID"] = bd.LerInt("EventoID");
                        apresentacoes.Rows.Add(apresentacao);
                    }

                    if (setores.Select("ApresentacaoID=" + bd.LerInt("ApresentacaoID") + " AND ID=" + bd.LerInt("SetorID")).Length == 0)
                    {
                        DataRow setor = setores.NewRow();
                        setor["ID"] = bd.LerInt("SetorID");
                        setor["Nome"] = bd.LerString("Setor");
                        setor["Produto"] = bd.LerBoolean("Produto");
                        setor["LugarMarcado"] = bd.LerString("LugarMarcado");
                        setor["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        setores.Rows.Add(setor);
                    }

                    DataRow preco = precos.NewRow();
                    preco["ID"] = bd.LerInt("PrecoID");
                    preco["Nome"] = bd.LerString("Preco");
                    preco["CorID"] = bd.LerInt("CorID");
                    preco["Valor"] = bd.LerDecimal("Valor");
                    preco["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    preco["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    preco["SetorID"] = bd.LerInt("SetorID");
                    precos.Rows.Add(preco);

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);
                buffer.Tables.Add(precos);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarPrecos(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("LocalID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("Produto", typeof(bool));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));

                DataTable precos = new DataTable("Preco");
                precos.Columns.Add("ID", typeof(int));
                precos.Columns.Add("Nome", typeof(string));
                precos.Columns.Add("CorID", typeof(int));
                precos.Columns.Add("Valor", typeof(decimal));
                precos.Columns.Add("ApresentacaoSetorID", typeof(int));
                precos.Columns.Add("ApresentacaoID", typeof(int));
                precos.Columns.Add("SetorID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroEmpresa = (empresaID > 0) ? "AND tEmpresa.ID=" + empresaID + " " : "";

                string filtroSetor;
                if (tipoSetores == TipoSetores.Marcados)
                    filtroSetor = "AND tSetor.LugarMarcado<>'" + Setor.Pista + "' ";
                else if (tipoSetores == TipoSetores.NaoMarcados)
                    filtroSetor = "AND tSetor.LugarMarcado='" + Setor.Pista + "' ";
                else
                    filtroSetor = "";

                if (tipoSetoresDesc != "")
                    tipoSetoresDesc = "AND tSetor.LugarMarcado in (" + tipoSetoresDesc + ") ";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string sql = "SELECT tLocal.ID AS LocalID, tLocal.Nome AS Local, tEvento.ID AS EventoID, tEvento.Nome AS Evento, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao, tSetor.ID AS SetorID, tSetor.Nome AS Setor, tSetor.Produto, tSetor.LugarMarcado, tPreco.ID AS PrecoID, tPreco.Nome AS Preco, tPreco.Valor, tPreco.CorID, tPreco.ApresentacaoSetorID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK),tPreco (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND tPreco.ApresentacaoSetorID=tApresentacaoSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID " + filtroSetor + tipoSetoresDesc + " " + filtroEmpresa + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tLocal.Nome,tEvento.Nome,tApresentacao.Horario,tSetor.Nome,tPreco.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (locais.Select("ID=" + bd.LerInt("LocalID")).Length == 0)
                    {
                        DataRow local = locais.NewRow();
                        local["ID"] = bd.LerInt("LocalID");
                        local["Nome"] = bd.LerString("Local");
                        locais.Rows.Add(local);
                    }

                    if (eventos.Select("LocalID=" + bd.LerInt("LocalID") + " AND ID=" + bd.LerInt("EventoID")).Length == 0)
                    {
                        DataRow evento = eventos.NewRow();
                        evento["ID"] = bd.LerInt("EventoID");
                        evento["Nome"] = bd.LerString("Evento");
                        evento["LocalID"] = bd.LerInt("LocalID");
                        eventos.Rows.Add(evento);
                    }

                    if (apresentacoes.Select("EventoID=" + bd.LerInt("EventoID") + " AND ID=" + bd.LerInt("ApresentacaoID")).Length == 0)
                    {
                        DataRow apresentacao = apresentacoes.NewRow();
                        apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                        apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                        apresentacao["EventoID"] = bd.LerInt("EventoID");
                        apresentacoes.Rows.Add(apresentacao);
                    }

                    if (setores.Select("ApresentacaoID=" + bd.LerInt("ApresentacaoID") + " AND ID=" + bd.LerInt("SetorID")).Length == 0)
                    {
                        DataRow setor = setores.NewRow();
                        setor["ID"] = bd.LerInt("SetorID");
                        setor["Nome"] = bd.LerString("Setor");
                        setor["Produto"] = bd.LerBoolean("Produto");
                        setor["LugarMarcado"] = bd.LerString("LugarMarcado");
                        setor["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        setores.Rows.Add(setor);
                    }

                    DataRow preco = precos.NewRow();
                    preco["ID"] = bd.LerInt("PrecoID");
                    preco["Nome"] = bd.LerString("Preco");
                    preco["CorID"] = bd.LerInt("CorID");
                    preco["Valor"] = bd.LerDecimal("Valor");
                    preco["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    preco["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    preco["SetorID"] = bd.LerInt("SetorID");
                    precos.Rows.Add(preco);

                }
                bd.Fechar();

                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);
                buffer.Tables.Add(precos);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarLocalPrecos(int localID)
        {

            if (localID <= 0)
                throw new AutoSelecionadorGerenciadorException("LocalID deve ser maior que zero.");

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("Produto", typeof(bool));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));

                DataTable precos = new DataTable("Preco");
                precos.Columns.Add("ID", typeof(int));
                precos.Columns.Add("Nome", typeof(string));
                precos.Columns.Add("CorID", typeof(int));
                precos.Columns.Add("Valor", typeof(decimal));
                precos.Columns.Add("ApresentacaoSetorID", typeof(int));
                precos.Columns.Add("ApresentacaoID", typeof(int));
                precos.Columns.Add("SetorID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroSetor;
                if (tipoSetores == TipoSetores.Marcados)
                    filtroSetor = "AND tSetor.LugarMarcado<>'" + Setor.Pista + "' ";
                else if (tipoSetores == TipoSetores.NaoMarcados)
                    filtroSetor = "AND tSetor.LugarMarcado='" + Setor.Pista + "' ";
                else
                    filtroSetor = "";

                if (tipoSetoresDesc != "")
                    tipoSetoresDesc = "AND tSetor.LugarMarcado in (" + tipoSetoresDesc + ") ";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string sql = "SELECT tEvento.ID AS EventoID, tEvento.Nome AS Evento, tApresentacao.ID AS ApresentacaoID, tApresentacao.Horario AS Apresentacao, tSetor.ID AS SetorID, tSetor.Nome AS Setor, tSetor.Produto, tSetor.LugarMarcado, tPreco.ID AS PrecoID, tPreco.Nome AS Preco, tPreco.Valor, tPreco.CorID, tPreco.ApresentacaoSetorID " +
                    "FROM tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK),tPreco (NOLOCK) " +
                    "WHERE tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND tPreco.ApresentacaoSetorID=tApresentacaoSetor.ID AND tEvento.LocalID=" + localID + " " + filtroSetor + tipoSetoresDesc + hoje +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY tEvento.Nome,tApresentacao.Horario,tSetor.Nome,tPreco.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (eventos.Select("ID=" + bd.LerInt("EventoID")).Length == 0)
                    {
                        DataRow evento = eventos.NewRow();
                        evento["ID"] = bd.LerInt("EventoID");
                        evento["Nome"] = bd.LerString("Evento");
                        eventos.Rows.Add(evento);
                    }

                    if (apresentacoes.Select("EventoID=" + bd.LerInt("EventoID") + " AND ID=" + bd.LerInt("ApresentacaoID")).Length == 0)
                    {
                        DataRow apresentacao = apresentacoes.NewRow();
                        apresentacao["ID"] = bd.LerInt("ApresentacaoID");
                        apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                        apresentacao["EventoID"] = bd.LerInt("EventoID");
                        apresentacoes.Rows.Add(apresentacao);
                    }

                    if (setores.Select("ApresentacaoID=" + bd.LerInt("ApresentacaoID") + " AND ID=" + bd.LerInt("SetorID")).Length == 0)
                    {
                        DataRow setor = setores.NewRow();
                        setor["ID"] = bd.LerInt("SetorID");
                        setor["Nome"] = bd.LerString("Setor");
                        setor["Produto"] = bd.LerBoolean("Produto");
                        setor["LugarMarcado"] = bd.LerString("LugarMarcado");
                        setor["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        setores.Rows.Add(setor);
                    }

                    DataRow preco = precos.NewRow();
                    preco["ID"] = bd.LerInt("PrecoID");
                    preco["Nome"] = bd.LerString("Preco");
                    preco["CorID"] = bd.LerInt("CorID");
                    preco["Valor"] = bd.LerDecimal("Valor");
                    preco["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    preco["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    preco["SetorID"] = bd.LerInt("SetorID");
                    precos.Rows.Add(preco);

                }
                bd.Fechar();

                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);
                buffer.Tables.Add(precos);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarEmpresaPrecos()
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("EmpresaID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Nome", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("Produto", typeof(bool));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));

                DataTable precos = new DataTable("Preco");
                precos.Columns.Add("ID", typeof(int));
                precos.Columns.Add("Nome", typeof(string));
                precos.Columns.Add("CorID", typeof(int));
                precos.Columns.Add("Valor", typeof(decimal));
                precos.Columns.Add("ApresentacaoSetorID", typeof(int));
                precos.Columns.Add("ApresentacaoID", typeof(int));
                precos.Columns.Add("SetorID", typeof(int));

                BD bd = new BD();

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                string filtroSetor;
                if (tipoSetores == TipoSetores.Marcados)
                    filtroSetor = "AND tSetor.LugarMarcado<>'" + Setor.Pista + "' ";
                else if (tipoSetores == TipoSetores.NaoMarcados)
                    filtroSetor = "AND tSetor.LugarMarcado='" + Setor.Pista + "' ";
                else
                    filtroSetor = "";

                if (tipoSetoresDesc != "")
                    tipoSetoresDesc = "AND tSetor.LugarMarcado in (" + tipoSetoresDesc + ") ";

                string hoje = "";
                if (somenteApresentacoesQueNaoPassaram)
                {
                    hoje = "AND tApresentacao.Horario >= '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }
                else if (somenteApresentacoesQueJaPassaram)
                {
                    hoje = "AND tApresentacao.Horario < '" + System.DateTime.Today.ToString("yyyyMMdd") + "000000" + "' ";
                }

                string clausulaRegional = string.Empty;
                if (this.RegionalID != 0)
                    clausulaRegional = "AND tEmpresa.RegionalID = " + this.RegionalID + " ";


                SqlConnection connection = (SqlConnection)bd.Cnn;

                SqlCommand comm = connection.CreateCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;

                // Carregar informações de Empresas
                string sql = @"SELECT DISTINCT tEmpresa.ID, tEmpresa.Nome, tEmpresa.RegionalID 
                        FROM tEmpresa (NOLOCK), tLocal (NOLOCK), tEvento (NOLOCK), tApresentacao (NOLOCK), tApresentacaoSetor (NOLOCK), tSetor (NOLOCK), tPreco (NOLOCK)
                        WHERE tLocal.EmpresaID = tEmpresa.ID AND tEvento.ID = tApresentacao.EventoID AND 
                        tEvento.LocalID = tLocal.ID AND tApresentacaoSetor.SetorID = tSetor.ID AND 
                        tApresentacaoSetor.ApresentacaoID = tApresentacao.ID AND 
                        tPreco.ApresentacaoSetorID = tApresentacaoSetor.ID " +
                        filtroSetor +
                        tipoSetoresDesc +
                        hoje +
                        disponivelVenda +
                        disponivelAjuste +
                        disponivelRelatorio +
                        clausulaRegional +
                        "ORDER BY tEmpresa.Nome";

                comm.CommandText = sql;
                adapter.Fill(empresas);

                comm = connection.CreateCommand();
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;


                // Carregar informações de Eventos
                sql = @"SELECT DISTINCT tEmpresa.ID AS EmpresaID, tEvento.ID, tEvento.Nome
                        FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK),tPreco (NOLOCK)
                        WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND 
                        tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND 
                        tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND 
                        tPreco.ApresentacaoSetorID=tApresentacaoSetor.ID " +
                        filtroSetor +
                        tipoSetoresDesc +
                        hoje +
                        disponivelVenda +
                        disponivelAjuste +
                        disponivelRelatorio +
                        clausulaRegional +
                        "ORDER BY tEvento.Nome";

                comm.CommandText = sql;
                adapter.Fill(eventos);

                // Carregar informações de Apresentações
                sql = @"SELECT DISTINCT tEvento.ID AS EventoID, tApresentacao.ID, tApresentacao.Horario AS Nome
                        FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK),tPreco (NOLOCK)
                        WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND 
                        tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND 
                        tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND 
                        tPreco.ApresentacaoSetorID=tApresentacaoSetor.ID " +
                        filtroSetor +
                        tipoSetoresDesc +
                        hoje +
                        disponivelVenda +
                        disponivelAjuste +
                        disponivelRelatorio +
                        clausulaRegional +
                        "ORDER BY tApresentacao.Horario";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow apresentacao = apresentacoes.NewRow();
                    apresentacao["ID"] = bd.LerInt("ID");
                    apresentacao["Nome"] = bd.LerStringFormatoSemanaDataHora("Nome");
                    apresentacao["EventoID"] = bd.LerInt("EventoID");
                    apresentacoes.Rows.Add(apresentacao);
                }
                bd.FecharConsulta();

                comm = connection.CreateCommand();
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;


                // Carregar informações de Setores
                sql = @"SELECT DISTINCT tApresentacao.ID AS ApresentacaoID, tSetor.ID, tSetor.Nome, 
                        CASE tSetor.Produto WHEN 'T' THEN 1 WHEN 'F' THEN 0 ELSE 0 END AS Produto, 
                        tSetor.LugarMarcado
                        FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK),tPreco (NOLOCK)
                        WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND 
                        tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND 
                        tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND 
                        tPreco.ApresentacaoSetorID=tApresentacaoSetor.ID " +
                        filtroSetor +
                        tipoSetoresDesc +
                        hoje +
                        disponivelVenda +
                        disponivelAjuste +
                        disponivelRelatorio +
                        clausulaRegional +
                        "ORDER BY tSetor.Nome";

                comm.CommandText = sql;
                adapter.Fill(setores);

                comm = connection.CreateCommand();
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;

                sql = @"SELECT DISTINCT tApresentacao.ID AS ApresentacaoID, tSetor.ID AS SetorID,
                        tPreco.ID, tPreco.Nome, tPreco.Valor, tPreco.CorID, tPreco.ApresentacaoSetorID
                        FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK),tPreco (NOLOCK)
                        WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND 
                        tEvento.LocalID=tLocal.ID AND tApresentacaoSetor.SetorID=tSetor.ID AND 
                        tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND 
                        tPreco.ApresentacaoSetorID=tApresentacaoSetor.ID " +
                        filtroSetor +
                        tipoSetoresDesc +
                        hoje +
                        disponivelVenda +
                        disponivelAjuste +
                        disponivelRelatorio +
                        clausulaRegional +
                        "ORDER BY tPreco.Nome";

                comm.CommandText = sql;
                adapter.Fill(precos);

                #region Codigo Antigo
                /*
                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tEvento.ID AS EventoID, " +
                    "tEvento.Nome AS Evento, tApresentacao.ID AS ApresentacaoID, " +
                    "tApresentacao.Horario AS Apresentacao, tSetor.ID AS SetorID, tSetor.Nome AS Setor, " +
                    "tSetor.Produto, tSetor.LugarMarcado, tPreco.ID AS PrecoID, tPreco.Nome AS Preco, " +
                    "tPreco.Valor, tPreco.CorID, tPreco.ApresentacaoSetorID " +
					"FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tEvento (NOLOCK),tApresentacao (NOLOCK),tApresentacaoSetor (NOLOCK),tSetor (NOLOCK),tPreco (NOLOCK) " +
					"WHERE tLocal.EmpresaID=tEmpresa.ID AND tEvento.ID=tApresentacao.EventoID AND tEvento.LocalID=tLocal.ID " +
                    "AND tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND " +
                    "tPreco.ApresentacaoSetorID=tApresentacaoSetor.ID " +
                    filtroSetor + 
                    tipoSetoresDesc + 
                    hoje +
					disponivelVenda+
					disponivelAjuste+
					disponivelRelatorio+
					" ORDER BY tEmpresa.Nome,tEvento.Nome,tApresentacao.Horario,tSetor.Nome,tPreco.Nome";

				bd.Consulta(sql);

				while(bd.Consulta().Read()){

					if (empresas.Select("ID="+bd.LerInt("EmpresaID")).Length == 0){
						DataRow empresa = empresas.NewRow();
						empresa["ID"]= bd.LerInt("EmpresaID");
						empresa["Nome"]= bd.LerString("Empresa");
						empresas.Rows.Add(empresa);
					}

					if (eventos.Select("EmpresaID="+bd.LerInt("EmpresaID")+" AND ID="+bd.LerInt("EventoID")).Length == 0){
						DataRow evento = eventos.NewRow();
						evento["ID"]= bd.LerInt("EventoID");
						evento["Nome"]= bd.LerString("Evento");
						evento["EmpresaID"]= bd.LerInt("EmpresaID");
						eventos.Rows.Add(evento);
					}

					if (apresentacoes.Select("EventoID="+bd.LerInt("EventoID")+" AND ID="+bd.LerInt("ApresentacaoID")).Length == 0){
						DataRow apresentacao = apresentacoes.NewRow();
						apresentacao["ID"]= bd.LerInt("ApresentacaoID");
						apresentacao["Nome"]= bd.LerStringFormatoSemanaDataHora("Apresentacao");
						apresentacao["EventoID"]= bd.LerInt("EventoID");
						apresentacoes.Rows.Add(apresentacao);
					}

					if (setores.Select("ApresentacaoID="+bd.LerInt("ApresentacaoID")+" AND ID="+bd.LerInt("SetorID")).Length == 0){
						DataRow setor = setores.NewRow();
						setor["ID"]= bd.LerInt("SetorID");
						setor["Nome"]= bd.LerString("Setor");
						setor["Produto"]= bd.LerBoolean("Produto");
						setor["LugarMarcado"]= bd.LerString("LugarMarcado");
						setor["ApresentacaoID"]= bd.LerInt("ApresentacaoID");
						setores.Rows.Add(setor);
					}

					DataRow preco = precos.NewRow();
					preco["ID"]= bd.LerInt("PrecoID");
					preco["Nome"]= bd.LerString("Preco");
					preco["CorID"]= bd.LerInt("CorID");
					preco["Valor"]= bd.LerDecimal("Valor");
					preco["ApresentacaoSetorID"]= bd.LerInt("ApresentacaoSetorID");
					preco["ApresentacaoID"]= bd.LerInt("ApresentacaoID");
					preco["SetorID"]= bd.LerInt("SetorID");
					precos.Rows.Add(preco);


                }
                */
                #endregion

                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);
                buffer.Tables.Add(precos);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarPacotes()
        {

            try
            {

                if (empresaID != 0)
                    return carregarPacotes(empresaID);
                else if (localID != 0)
                    return carregarLocalPacotes(localID);

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));
                locais.Columns.Add("EmpresaID", typeof(int));

                DataTable pacotes = new DataTable("Pacote");
                pacotes.Columns.Add("ID", typeof(int));
                pacotes.Columns.Add("Nome", typeof(string));
                pacotes.Columns.Add("LocalID", typeof(int));

                BD bd = new BD();

                string clausulaRegional = string.Empty;
                if (this.RegionalID != 0)
                    clausulaRegional = "AND tEmpresa.RegionalID = " + this.RegionalID + " ";

                string sql = "SELECT DISTINCT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tLocal.ID AS LocalID, tLocal.Nome AS Local, tPacote.ID AS PacoteID, tPacote.Nome AS Pacote, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tPacote (NOLOCK) " +
                    (somenteApresentacoesDisponiveisAjuste == true ? ", tPacoteItem (NOLOCK), tApresentacao (NOLOCK) " : "") +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tPacote.LocalID=tLocal.ID " +
                    (somenteApresentacoesDisponiveisAjuste == true ? "AND tPacoteItem.PacoteID = tPacote.ID AND tApresentacao.ID = tPacoteItem.ApresentacaoID AND tApresentacao.DisponivelAjuste = 'T' " : "") +
                    clausulaRegional +
                    "ORDER BY tEmpresa.Nome,tLocal.Nome,tPacote.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresa["RegionalID"] = bd.LerInt("RegionalID");
                        empresas.Rows.Add(empresa);
                    }

                    if (locais.Select("EmpresaID=" + bd.LerInt("EmpresaID") + " AND ID=" + bd.LerInt("LocalID")).Length == 0)
                    {
                        DataRow local = locais.NewRow();
                        local["ID"] = bd.LerInt("LocalID");
                        local["Nome"] = bd.LerString("Local");
                        local["EmpresaID"] = bd.LerInt("EmpresaID");
                        locais.Rows.Add(local);
                    }

                    DataRow pacote = pacotes.NewRow();
                    pacote["ID"] = bd.LerInt("PacoteID");
                    pacote["Nome"] = bd.LerString("Pacote");
                    pacote["LocalID"] = bd.LerInt("LocalID");
                    pacotes.Rows.Add(pacote);

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(locais);
                buffer.Tables.Add(pacotes);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarPacotes(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));

                DataTable pacotes = new DataTable("Pacote");
                pacotes.Columns.Add("ID", typeof(int));
                pacotes.Columns.Add("Nome", typeof(string));
                pacotes.Columns.Add("LocalID", typeof(int));

                BD bd = new BD();

                string filtroEmpresa = (empresaID > 0) ? "AND tEmpresa.ID=" + empresaID + " " : "";

                string sql = "SELECT tLocal.ID AS LocalID, tLocal.Nome AS Local, tPacote.ID AS PacoteID, tPacote.Nome AS Pacote " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tPacote (NOLOCK) " +
                    (somenteApresentacoesDisponiveisAjuste == true ? ", tPacoteItem (NOLOCK), tApresentacao (NOLOCK) " : "") +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tPacote.LocalID=tLocal.ID " + filtroEmpresa + " " +
                    (somenteApresentacoesDisponiveisAjuste == true ? "AND tPacoteItem.PacoteID = tPacote.ID AND tApresentacao.ID = tPacoteItem.ApresentacaoID AND tApresentacao.DisponivelAjuste = 'T' " : "") +
                    "ORDER BY tLocal.Nome,tPacote.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (locais.Select("ID=" + bd.LerInt("LocalID")).Length == 0)
                    {
                        DataRow local = locais.NewRow();
                        local["ID"] = bd.LerInt("LocalID");
                        local["Nome"] = bd.LerString("Local");
                        locais.Rows.Add(local);
                    }

                    DataRow pacote = pacotes.NewRow();
                    pacote["ID"] = bd.LerInt("PacoteID");
                    pacote["Nome"] = bd.LerString("Pacote");
                    pacote["LocalID"] = bd.LerInt("LocalID");
                    pacotes.Rows.Add(pacote);

                }
                bd.Fechar();

                buffer.Tables.Add(locais);
                buffer.Tables.Add(pacotes);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet carregarLocalPacotes(int localID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable pacotes = new DataTable("Pacote");
                pacotes.Columns.Add("ID", typeof(int));
                pacotes.Columns.Add("Nome", typeof(string));

                BD bd = new BD();

                string sql = "SELECT tPacote.ID AS PacoteID, tPacote.Nome AS Pacote " +
                    "FROM tPacote (NOLOCK) " +
                    "WHERE tPacote.LocalID=" + localID + " " +
                    "ORDER BY tPacote.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow pacote = pacotes.NewRow();
                    pacote["ID"] = bd.LerInt("PacoteID");
                    pacote["Nome"] = bd.LerString("Pacote");
                    pacotes.Rows.Add(pacote);

                }
                bd.Fechar();

                buffer.Tables.Add(pacotes);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet CarregarEmpresaPacotes()
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable empresas = new DataTable("Empresa");
                empresas.Columns.Add("ID", typeof(int));
                empresas.Columns.Add("Nome", typeof(string));
                empresas.Columns.Add("RegionalID", typeof(int));

                DataTable pacotes = new DataTable("Pacote");
                pacotes.Columns.Add("ID", typeof(int));
                pacotes.Columns.Add("Nome", typeof(string));
                pacotes.Columns.Add("EmpresaID", typeof(int));

                BD bd = new BD();

                string sql = "SELECT tEmpresa.ID AS EmpresaID, tEmpresa.Nome AS Empresa, tPacote.ID AS PacoteID, tPacote.Nome AS Pacote, tEmpresa.RegionalID " +
                    "FROM tEmpresa (NOLOCK),tLocal (NOLOCK),tPacote (NOLOCK) " +
                    "WHERE tLocal.EmpresaID=tEmpresa.ID AND tPacote.LocalID=tLocal.ID " +
                    "ORDER BY tEmpresa.Nome,tPacote.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    if (empresas.Select("ID=" + bd.LerInt("EmpresaID")).Length == 0)
                    {
                        DataRow empresa = empresas.NewRow();
                        empresa["ID"] = bd.LerInt("EmpresaID");
                        empresa["Nome"] = bd.LerString("Empresa");
                        empresa["RegionalID"] = bd.LerInt("RegionalID");
                        empresas.Rows.Add(empresa);
                    }

                    DataRow pacote = pacotes.NewRow();
                    pacote["ID"] = bd.LerInt("PacoteID");
                    pacote["Nome"] = bd.LerString("Pacote");
                    pacote["EmpresaID"] = bd.LerInt("EmpresaID");
                    pacotes.Rows.Add(pacote);

                }
                bd.Fechar();

                buffer.Tables.Add(empresas);
                buffer.Tables.Add(pacotes);
                if (IncluirRegional)
                    buffer.Tables.Add(this.CarregarRegionais());
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataTable CarregarRegionais()
        {
            BD bd = new BD();
            try
            {
                DataTable regionais = new DataTable("Regional");
                regionais.Columns.Add("ID", typeof(int));
                regionais.Columns.Add("Nome", typeof(string));

                SqlConnection connection = (SqlConnection)bd.Cnn;

                SqlCommand comm = connection.CreateCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;

                comm.CommandText = "SELECT ID, Nome FROM tRegional (NOLOCK) ORDER BY Nome";
                adapter.Fill(regionais);

                return regionais;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }

    }

    [Serializable]
    public class AutoSelecionadorGerenciadorException : Exception
    {

        public AutoSelecionadorGerenciadorException() : base() { }

        public AutoSelecionadorGerenciadorException(string msg) : base(msg) { }

        public AutoSelecionadorGerenciadorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

}
