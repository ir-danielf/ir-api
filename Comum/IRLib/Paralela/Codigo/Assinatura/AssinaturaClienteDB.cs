/******************************************************
* Arquivo AssinaturaClienteDB.cs
* Gerado em: 04/01/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "AssinaturaCliente_B"

    public abstract class AssinaturaCliente_B : BaseBD
    {

        public clienteid ClienteID = new clienteid();
        public assinaturaid AssinaturaID = new assinaturaid();
        public setorid SetorID = new setorid();
        public precotipoid PrecoTipoID = new precotipoid();
        public lugarid LugarID = new lugarid();
        public assinaturaanoid AssinaturaAnoID = new assinaturaanoid();
        public status Status = new status();
        public vendabilheteriaid VendaBilheteriaID = new vendabilheteriaid();
        public acao Acao = new acao();
        public assinaturaclienteid AssinaturaClienteID = new assinaturaclienteid();
        public timestamp TimeStamp = new timestamp();
        public usuarioid UsuarioID = new usuarioid();
        public desmembrada Desmembrada = new desmembrada();
        public agregadoid AgregadoID = new agregadoid();

        public AssinaturaCliente_B() { }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaCliente
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaCliente WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.AssinaturaID.ValorBD = bd.LerInt("AssinaturaID").ToString();
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.PrecoTipoID.ValorBD = bd.LerInt("PrecoTipoID").ToString();
                    this.LugarID.ValorBD = bd.LerInt("LugarID").ToString();
                    this.AssinaturaAnoID.ValorBD = bd.LerInt("AssinaturaAnoID").ToString();
                    this.Status.ValorBD = bd.LerString("Status");
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.Acao.ValorBD = bd.LerString("Acao");
                    this.AssinaturaClienteID.ValorBD = bd.LerInt("AssinaturaClienteID").ToString();
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.Desmembrada.ValorBD = bd.LerString("Desmembrada");
                    this.AgregadoID.ValorBD = bd.LerInt("AgregadoID").ToString();
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();

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

        /// <summary>
        /// Inserir novo(a) AssinaturaCliente
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaCliente(ClienteID, AssinaturaID, SetorID, PrecoTipoID, LugarID, AssinaturaAnoID, Status, VendaBilheteriaID, Acao, AssinaturaClienteID, TimeStamp, UsuarioID, Desmembrada, AgregadoID) ");
                sql.Append("VALUES (@001,@002,@003,@004,@005,@006,'@007',@008,'@009',@010,'@011',@012,'@013','@014'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.AssinaturaID.ValorBD);
                sql.Replace("@003", this.SetorID.ValorBD);
                sql.Replace("@004", this.PrecoTipoID.ValorBD);
                sql.Replace("@005", this.LugarID.ValorBD);
                sql.Replace("@006", this.AssinaturaAnoID.ValorBD);
                sql.Replace("@007", this.Status.ValorBD);
                sql.Replace("@008", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@009", this.Acao.ValorBD);
                sql.Replace("@010", this.AssinaturaClienteID.ValorBD);
                sql.Replace("@011", this.TimeStamp.ValorBD);
                sql.Replace("@012", this.UsuarioID.ValorBD);
                sql.Replace("@013", this.Desmembrada.ValorBD);
                sql.Replace("@014", this.AgregadoID.ValorBD);

                this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                bd.Fechar();

                return this.Control.ID > 0;

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

        /// <summary>
        /// Inserir novo(a) AssinaturaCliente
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tAssinaturaCliente(ClienteID, AssinaturaID, SetorID, PrecoTipoID, LugarID, AssinaturaAnoID, Status, VendaBilheteriaID, Acao, AssinaturaClienteID, TimeStamp, UsuarioID, Desmembrada,AgregadoID) ");
            sql.Append("VALUES (@001,@002,@003,@004,@005,@006,'@007',@008,'@009',@010,'@011',@012,'@013','@014'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.AssinaturaID.ValorBD);
            sql.Replace("@003", this.SetorID.ValorBD);
            sql.Replace("@004", this.PrecoTipoID.ValorBD);
            sql.Replace("@005", this.LugarID.ValorBD);
            sql.Replace("@006", this.AssinaturaAnoID.ValorBD);
            sql.Replace("@007", this.Status.ValorBD);
            sql.Replace("@008", this.VendaBilheteriaID.ValorBD);
            sql.Replace("@009", this.Acao.ValorBD);
            sql.Replace("@010", this.AssinaturaClienteID.ValorBD);
            sql.Replace("@011", this.TimeStamp.ValorBD);
            sql.Replace("@012", this.UsuarioID.ValorBD);
            sql.Replace("@013", this.Desmembrada.ValorBD);
            sql.Replace("@014", this.AgregadoID.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza AssinaturaCliente
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaCliente SET ClienteID = @001, AssinaturaID = @002, SetorID = @003, PrecoTipoID = @004, LugarID = @005, AssinaturaAnoID = @006, Status = '@007', VendaBilheteriaID = @008, Acao = '@009', AssinaturaClienteID = @010, TimeStamp = '@011', UsuarioID = @012, Desmembrada = '@013', AgregadoID=@014 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.AssinaturaID.ValorBD);
                sql.Replace("@003", this.SetorID.ValorBD);
                sql.Replace("@004", this.PrecoTipoID.ValorBD);
                sql.Replace("@005", this.LugarID.ValorBD);
                sql.Replace("@006", this.AssinaturaAnoID.ValorBD);
                sql.Replace("@007", this.Status.ValorBD);
                sql.Replace("@008", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@009", this.Acao.ValorBD);
                sql.Replace("@010", this.AssinaturaClienteID.ValorBD);
                sql.Replace("@011", this.TimeStamp.ValorBD);
                sql.Replace("@012", this.UsuarioID.ValorBD);
                sql.Replace("@013", this.Desmembrada.ValorBD);
                sql.Replace("@014", this.AgregadoID.ValorBD);

                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

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

        /// <summary>
        /// Atualiza AssinaturaCliente
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tAssinaturaCliente SET ClienteID = @001, AssinaturaID = @002, SetorID = @003, PrecoTipoID = @004, LugarID = @005, AssinaturaAnoID = @006, Status = '@007', VendaBilheteriaID = @008, Acao = '@009', AssinaturaClienteID = @010, TimeStamp = '@011', UsuarioID = @012, Desmembrada = '@013', AgregadoID=@014 ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.AssinaturaID.ValorBD);
            sql.Replace("@003", this.SetorID.ValorBD);
            sql.Replace("@004", this.PrecoTipoID.ValorBD);
            sql.Replace("@005", this.LugarID.ValorBD);
            sql.Replace("@006", this.AssinaturaAnoID.ValorBD);
            sql.Replace("@007", this.Status.ValorBD);
            sql.Replace("@008", this.VendaBilheteriaID.ValorBD);
            sql.Replace("@009", this.Acao.ValorBD);
            sql.Replace("@010", this.AssinaturaClienteID.ValorBD);
            sql.Replace("@011", this.TimeStamp.ValorBD);
            sql.Replace("@012", this.UsuarioID.ValorBD);
            sql.Replace("@013", this.Desmembrada.ValorBD);
            sql.Replace("@014", this.AgregadoID.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui AssinaturaCliente com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tAssinaturaCliente WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);
                bd.Fechar();

                bool result = Convert.ToBoolean(x);
                return result;

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

        /// <summary>
        /// Exclui AssinaturaCliente com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tAssinaturaCliente WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui AssinaturaCliente
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {

            try
            {
                return this.Excluir(this.Control.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public override void Limpar()
        {

            this.ClienteID.Limpar();
            this.AssinaturaID.Limpar();
            this.SetorID.Limpar();
            this.PrecoTipoID.Limpar();
            this.LugarID.Limpar();
            this.AssinaturaAnoID.Limpar();
            this.Status.Limpar();
            this.VendaBilheteriaID.Limpar();
            this.Acao.Limpar();
            this.AssinaturaClienteID.Limpar();
            this.TimeStamp.Limpar();
            this.UsuarioID.Limpar();
            this.Desmembrada.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
            this.AgregadoID.Limpar();
        }

        public override void Desfazer()
        {

            this.ClienteID.Desfazer();
            this.AssinaturaID.Desfazer();
            this.SetorID.Desfazer();
            this.PrecoTipoID.Desfazer();
            this.LugarID.Desfazer();
            this.AssinaturaAnoID.Desfazer();
            this.Status.Desfazer();
            this.VendaBilheteriaID.Desfazer();
            this.Acao.Desfazer();
            this.AssinaturaClienteID.Desfazer();
            this.TimeStamp.Desfazer();
            this.UsuarioID.Desfazer();
            this.Desmembrada.Desfazer();
            this.AgregadoID.Desfazer();
        }

        public class clienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class assinaturaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class setorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SetorID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class precotipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoTipoID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class lugarid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LugarID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class assinaturaanoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaAnoID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class status : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Status";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class vendabilheteriaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class acao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Acao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class assinaturaclienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaClienteID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class timestamp : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "TimeStamp";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class usuarioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UsuarioID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class desmembrada : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Desmembrada";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class agregadoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AgregadoID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        /// <summary>
        /// Obtem uma tabela estruturada com todos os campos dessa classe.
        /// </summary>
        /// <returns></returns>
        public static DataTable Estrutura()
        {

            //Isso eh util para desacoplamento.
            //A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.

            try
            {

                DataTable tabela = new DataTable("AssinaturaCliente");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("AssinaturaID", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("PrecoTipoID", typeof(int));
                tabela.Columns.Add("LugarID", typeof(int));
                tabela.Columns.Add("AssinaturaAnoID", typeof(int));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("Desmembrada", typeof(bool));
                tabela.Columns.Add("AgregadoID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "AssinaturaClienteLista_B"

    public abstract class AssinaturaClienteLista_B : BaseLista
    {

        protected AssinaturaCliente assinaturaCliente;

        // passar o Usuario logado no sistema
        public AssinaturaClienteLista_B()
        {
            assinaturaCliente = new AssinaturaCliente();
        }

        public AssinaturaCliente AssinaturaCliente
        {
            get { return assinaturaCliente; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaCliente especifico
        /// </summary>
        public override IBaseBD this[int indice]
        {
            get
            {
                if (indice < 0 || indice >= lista.Count)
                {
                    return null;
                }
                else
                {
                    int id = (int)lista[indice];
                    assinaturaCliente.Ler(id);
                    return assinaturaCliente;
                }
            }
        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        /// <param name="tamanhoMax">Informe o tamanho maximo que a lista pode ter</param>
        /// <returns></returns>		
        public void Carregar(int tamanhoMax)
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tAssinaturaCliente";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaCliente";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        public override void Carregar()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tAssinaturaCliente";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaCliente";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche AssinaturaCliente corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                assinaturaCliente.Ler(id);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui o item corrente da lista
        /// </summary>
        /// <returns></returns>
        public override bool Excluir()
        {

            try
            {

                bool ok = assinaturaCliente.Excluir();
                if (ok)
                    lista.RemoveAt(Indice);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui todos os itens da lista carregada
        /// </summary>
        /// <returns></returns>
        public override bool ExcluirTudo()
        {

            try
            {
                if (lista.Count == 0)
                    Carregar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {

                bool ok = false;

                if (lista.Count > 0)
                { //verifica se tem itens

                    try
                    {
                        string ids = ToString();

                        string sqlDelete = "DELETE FROM tAssinaturaCliente WHERE ID in (" + ids + ")";

                        int x = bd.Executar(sqlDelete);
                        bd.Fechar();

                        ok = Convert.ToBoolean(x);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                { //nao tem itens na lista
                    //Devolve true como se os itens ja tivessem sido excluidos, com a premissa dos ids existirem de fato.
                    ok = true;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inseri novo(a) AssinaturaCliente na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaCliente.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaCliente.Control.ID);
                    Indice = lista.Count - 1;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obtem uma tabela de todos os campos de AssinaturaCliente carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaCliente");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("AssinaturaID", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("PrecoTipoID", typeof(int));
                tabela.Columns.Add("LugarID", typeof(int));
                tabela.Columns.Add("AssinaturaAnoID", typeof(int));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("Desmembrada", typeof(bool));
                tabela.Columns.Add("AgregadoID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaCliente.Control.ID;
                        linha["ClienteID"] = assinaturaCliente.ClienteID.Valor;
                        linha["AssinaturaID"] = assinaturaCliente.AssinaturaID.Valor;
                        linha["SetorID"] = assinaturaCliente.SetorID.Valor;
                        linha["PrecoTipoID"] = assinaturaCliente.PrecoTipoID.Valor;
                        linha["LugarID"] = assinaturaCliente.LugarID.Valor;
                        linha["AssinaturaAnoID"] = assinaturaCliente.AssinaturaAnoID.Valor;
                        linha["Status"] = assinaturaCliente.Status.Valor;
                        linha["VendaBilheteriaID"] = assinaturaCliente.VendaBilheteriaID.Valor;
                        linha["Acao"] = assinaturaCliente.Acao.Valor;
                        linha["AssinaturaClienteID"] = assinaturaCliente.AssinaturaClienteID.Valor;
                        linha["TimeStamp"] = assinaturaCliente.TimeStamp.Valor;
                        linha["UsuarioID"] = assinaturaCliente.UsuarioID.Valor;
                        linha["Desmembrada"] = assinaturaCliente.Desmembrada.Valor;
                        linha["AgregadoID"] = assinaturaCliente.AgregadoID.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioAssinaturaCliente");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("AssinaturaID", typeof(int));
                    tabela.Columns.Add("SetorID", typeof(int));
                    tabela.Columns.Add("PrecoTipoID", typeof(int));
                    tabela.Columns.Add("LugarID", typeof(int));
                    tabela.Columns.Add("AssinaturaAnoID", typeof(int));
                    tabela.Columns.Add("Status", typeof(string));
                    tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                    tabela.Columns.Add("Acao", typeof(string));
                    tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                    tabela.Columns.Add("TimeStamp", typeof(DateTime));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("Desmembrada", typeof(bool));
                    tabela.Columns.Add("AgregadoID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ClienteID"] = assinaturaCliente.ClienteID.Valor;
                        linha["AssinaturaID"] = assinaturaCliente.AssinaturaID.Valor;
                        linha["SetorID"] = assinaturaCliente.SetorID.Valor;
                        linha["PrecoTipoID"] = assinaturaCliente.PrecoTipoID.Valor;
                        linha["LugarID"] = assinaturaCliente.LugarID.Valor;
                        linha["AssinaturaAnoID"] = assinaturaCliente.AssinaturaAnoID.Valor;
                        linha["Status"] = assinaturaCliente.Status.Valor;
                        linha["VendaBilheteriaID"] = assinaturaCliente.VendaBilheteriaID.Valor;
                        linha["Acao"] = assinaturaCliente.Acao.Valor;
                        linha["AssinaturaClienteID"] = assinaturaCliente.AssinaturaClienteID.Valor;
                        linha["TimeStamp"] = assinaturaCliente.TimeStamp.Valor;
                        linha["UsuarioID"] = assinaturaCliente.UsuarioID.Valor;
                        linha["Desmembrada"] = assinaturaCliente.Desmembrada.Valor;
                        linha["AgregadoID"] = assinaturaCliente.AgregadoID.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Retorna um IDataReader com ID e o Campo.
        /// </summary>
        /// <param name="campo">Informe o campo. Exemplo: Nome</param>
        /// <returns></returns>
        public override IDataReader ListaPropriedade(string campo)
        {

            try
            {
                string sql;
                switch (campo)
                {
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "AssinaturaID":
                        sql = "SELECT ID, AssinaturaID FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY AssinaturaID";
                        break;
                    case "SetorID":
                        sql = "SELECT ID, SetorID FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY SetorID";
                        break;
                    case "PrecoTipoID":
                        sql = "SELECT ID, PrecoTipoID FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY PrecoTipoID";
                        break;
                    case "LugarID":
                        sql = "SELECT ID, LugarID FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY LugarID";
                        break;
                    case "AssinaturaAnoID":
                        sql = "SELECT ID, AssinaturaAnoID FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY AssinaturaAnoID";
                        break;
                    case "Status":
                        sql = "SELECT ID, Status FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY Status";
                        break;
                    case "VendaBilheteriaID":
                        sql = "SELECT ID, VendaBilheteriaID FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaID";
                        break;
                    case "Acao":
                        sql = "SELECT ID, Acao FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY Acao";
                        break;
                    case "AssinaturaClienteID":
                        sql = "SELECT ID, AssinaturaClienteID FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY AssinaturaClienteID";
                        break;
                    case "TimeStamp":
                        sql = "SELECT ID, TimeStamp FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY TimeStamp";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "Desmembrada":
                        sql = "SELECT ID, Desmembrada FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY Desmembrada";
                        break;
                    case "AgregadoID":
                        sql = "SELECT ID, AgregadoID FROM tAssinaturaCliente WHERE " + FiltroSQL + " ORDER BY AgregadoID";
                        break;
                    default:
                        sql = null;
                        break;
                }

                IDataReader dataReader = bd.Consulta(sql);

                bd.Fechar();

                return dataReader;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve um array dos IDs que compoem a lista
        /// </summary>
        /// <returns></returns>		
        public override int[] ToArray()
        {

            try
            {

                int[] a = (int[])lista.ToArray(typeof(int));

                return a;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve uma string dos IDs que compoem a lista concatenada por virgula
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            try
            {

                StringBuilder idsBuffer = new StringBuilder();

                int n = lista.Count;
                for (int i = 0; i < n; i++)
                {
                    int id = (int)lista[i];
                    idsBuffer.Append(id + ",");
                }

                string ids = "";

                if (idsBuffer.Length > 0)
                {
                    ids = idsBuffer.ToString();
                    ids = ids.Substring(0, ids.Length - 1);
                }

                return ids;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "AssinaturaClienteException"

    [Serializable]
    public class AssinaturaClienteException : Exception
    {

        public AssinaturaClienteException() : base() { }

        public AssinaturaClienteException(string msg) : base(msg) { }

        public AssinaturaClienteException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}