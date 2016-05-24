/******************************************************
* Arquivo ValeIngressoDB.cs
* Gerado em: 15/03/2010
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "ValeIngresso_B"

    public abstract class ValeIngresso_B : BaseBD
    {

        public valeingressotipoid ValeIngressoTipoID = new valeingressotipoid();
        public codigotroca CodigoTroca = new codigotroca();
        public datacriacao DataCriacao = new datacriacao();
        public dataexpiracao DataExpiracao = new dataexpiracao();
        public status Status = new status();
        public vendabilheteriaid VendaBilheteriaID = new vendabilheteriaid();
        public clienteid ClienteID = new clienteid();
        public sessionid SessionID = new sessionid();
        public timestampreserva TimeStampReserva = new timestampreserva();
        public lojaid LojaID = new lojaid();
        public usuarioid UsuarioID = new usuarioid();
        public canalid CanalID = new canalid();
        public clientenome ClienteNome = new clientenome();
        public codigobarra CodigoBarra = new codigobarra();

        public ValeIngresso_B() { }

        /// <summary>
        /// Preenche todos os atributos de ValeIngresso
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tValeIngresso WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ValeIngressoTipoID.ValorBD = bd.LerInt("ValeIngressoTipoID").ToString();
                    this.CodigoTroca.ValorBD = bd.LerString("CodigoTroca");
                    this.DataCriacao.ValorBD = bd.LerString("DataCriacao");
                    this.DataExpiracao.ValorBD = bd.LerString("DataExpiracao");
                    this.Status.ValorBD = bd.LerString("Status");
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.SessionID.ValorBD = bd.LerString("SessionID");
                    this.TimeStampReserva.ValorBD = bd.LerString("TimeStampReserva");
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.ClienteNome.ValorBD = bd.LerString("ClienteNome");
                    this.CodigoBarra.ValorBD = bd.LerString("CodigoBarra");
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
        /// Inserir novo(a) ValeIngresso
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tValeIngresso");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tValeIngresso(ID, ValeIngressoTipoID, CodigoTroca, DataCriacao, DataExpiracao, Status, VendaBilheteriaID, ClienteID, SessionID, TimeStampReserva, LojaID, UsuarioID, CanalID, ClienteNome, CodigoBarra) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005',@006,@007,'@008','@009',@010,@011,@012,'@013','@014')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ValeIngressoTipoID.ValorBD);
                sql.Replace("@002", this.CodigoTroca.ValorBD);
                sql.Replace("@003", this.DataCriacao.ValorBD);
                sql.Replace("@004", this.DataExpiracao.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@007", this.ClienteID.ValorBD);
                sql.Replace("@008", this.SessionID.ValorBD);
                sql.Replace("@009", this.TimeStampReserva.ValorBD);
                sql.Replace("@010", this.LojaID.ValorBD);
                sql.Replace("@011", this.UsuarioID.ValorBD);
                sql.Replace("@012", this.CanalID.ValorBD);
                sql.Replace("@013", this.ClienteNome.ValorBD);
                sql.Replace("@014", this.CodigoBarra.ValorBD);

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
        /// Atualiza ValeIngresso
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tValeIngresso SET ValeIngressoTipoID = @001, CodigoTroca = '@002', DataCriacao = '@003', DataExpiracao = '@004', Status = '@005', VendaBilheteriaID = @006, ClienteID = @007, SessionID = '@008', TimeStampReserva = '@009', LojaID = @010, UsuarioID = @011, CanalID = @012, ClienteNome = '@013', CodigoBarra = '@014' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ValeIngressoTipoID.ValorBD);
                sql.Replace("@002", this.CodigoTroca.ValorBD);
                sql.Replace("@003", this.DataCriacao.ValorBD);
                sql.Replace("@004", this.DataExpiracao.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@007", this.ClienteID.ValorBD);
                sql.Replace("@008", this.SessionID.ValorBD);
                sql.Replace("@009", this.TimeStampReserva.ValorBD);
                sql.Replace("@010", this.LojaID.ValorBD);
                sql.Replace("@011", this.UsuarioID.ValorBD);
                sql.Replace("@012", this.CanalID.ValorBD);
                sql.Replace("@013", this.ClienteNome.ValorBD);
                sql.Replace("@014", this.CodigoBarra.ValorBD);

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
        /// Exclui ValeIngresso com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tValeIngresso WHERE ID=" + id;

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
        /// Exclui ValeIngresso
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

            this.ValeIngressoTipoID.Limpar();
            this.CodigoTroca.Limpar();
            this.DataCriacao.Limpar();
            this.DataExpiracao.Limpar();
            this.Status.Limpar();
            this.VendaBilheteriaID.Limpar();
            this.ClienteID.Limpar();
            this.SessionID.Limpar();
            this.TimeStampReserva.Limpar();
            this.LojaID.Limpar();
            this.UsuarioID.Limpar();
            this.CanalID.Limpar();
            this.ClienteNome.Limpar();
            this.CodigoBarra.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.ValeIngressoTipoID.Desfazer();
            this.CodigoTroca.Desfazer();
            this.DataCriacao.Desfazer();
            this.DataExpiracao.Desfazer();
            this.Status.Desfazer();
            this.VendaBilheteriaID.Desfazer();
            this.ClienteID.Desfazer();
            this.SessionID.Desfazer();
            this.TimeStampReserva.Desfazer();
            this.LojaID.Desfazer();
            this.UsuarioID.Desfazer();
            this.CanalID.Desfazer();
            this.ClienteNome.Desfazer();
            this.CodigoBarra.Desfazer();
        }

        public class valeingressotipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValeIngressoTipoID";
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

        public class codigotroca : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoTroca";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 14;
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

        public class datacriacao : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataCriacao";
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

        public class dataexpiracao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataExpiracao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 8;
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

        public class sessionid : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "SessionID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 255;
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

        public class timestampreserva : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "TimeStampReserva";
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

        public class lojaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LojaID";
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

        public class canalid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CanalID";
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

        public class clientenome : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteNome";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
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

        public class codigobarra : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoBarra";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
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

                DataTable tabela = new DataTable("ValeIngresso");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ValeIngressoTipoID", typeof(int));
                tabela.Columns.Add("CodigoTroca", typeof(string));
                tabela.Columns.Add("DataCriacao", typeof(DateTime));
                tabela.Columns.Add("DataExpiracao", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("SessionID", typeof(string));
                tabela.Columns.Add("TimeStampReserva", typeof(DateTime));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("ClienteNome", typeof(string));
                tabela.Columns.Add("CodigoBarra", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "ValeIngressoLista_B"

    public abstract class ValeIngressoLista_B : BaseLista
    {

        protected ValeIngresso valeIngresso;

        // passar o Usuario logado no sistema
        public ValeIngressoLista_B()
        {
            valeIngresso = new ValeIngresso();
        }

        public ValeIngresso ValeIngresso
        {
            get { return valeIngresso; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ValeIngresso especifico
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
                    valeIngresso.Ler(id);
                    return valeIngresso;
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
                    sql = "SELECT ID FROM tValeIngresso";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tValeIngresso";

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
                    sql = "SELECT ID FROM tValeIngresso";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tValeIngresso";

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
        /// Preenche ValeIngresso corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                valeIngresso.Ler(id);

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

                bool ok = valeIngresso.Excluir();
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

                        string sqlDelete = "DELETE FROM tValeIngresso WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) ValeIngresso na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = valeIngresso.Inserir();
                if (ok)
                {
                    lista.Add(valeIngresso.Control.ID);
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
        /// Obtem uma tabela de todos os campos de ValeIngresso carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ValeIngresso");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ValeIngressoTipoID", typeof(int));
                tabela.Columns.Add("CodigoTroca", typeof(string));
                tabela.Columns.Add("DataCriacao", typeof(DateTime));
                tabela.Columns.Add("DataExpiracao", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("SessionID", typeof(string));
                tabela.Columns.Add("TimeStampReserva", typeof(DateTime));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("ClienteNome", typeof(string));
                tabela.Columns.Add("CodigoBarra", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = valeIngresso.Control.ID;
                        linha["ValeIngressoTipoID"] = valeIngresso.ValeIngressoTipoID.Valor;
                        linha["CodigoTroca"] = valeIngresso.CodigoTroca.Valor;
                        linha["DataCriacao"] = valeIngresso.DataCriacao.Valor;
                        linha["DataExpiracao"] = valeIngresso.DataExpiracao.Valor;
                        linha["Status"] = valeIngresso.Status.Valor;
                        linha["VendaBilheteriaID"] = valeIngresso.VendaBilheteriaID.Valor;
                        linha["ClienteID"] = valeIngresso.ClienteID.Valor;
                        linha["SessionID"] = valeIngresso.SessionID.Valor;
                        linha["TimeStampReserva"] = valeIngresso.TimeStampReserva.Valor;
                        linha["LojaID"] = valeIngresso.LojaID.Valor;
                        linha["UsuarioID"] = valeIngresso.UsuarioID.Valor;
                        linha["CanalID"] = valeIngresso.CanalID.Valor;
                        linha["ClienteNome"] = valeIngresso.ClienteNome.Valor;
                        linha["CodigoBarra"] = valeIngresso.CodigoBarra.Valor;
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

                DataTable tabela = new DataTable("RelatorioValeIngresso");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ValeIngressoTipoID", typeof(int));
                    tabela.Columns.Add("CodigoTroca", typeof(string));
                    tabela.Columns.Add("DataCriacao", typeof(DateTime));
                    tabela.Columns.Add("DataExpiracao", typeof(string));
                    tabela.Columns.Add("Status", typeof(string));
                    tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("SessionID", typeof(string));
                    tabela.Columns.Add("TimeStampReserva", typeof(DateTime));
                    tabela.Columns.Add("LojaID", typeof(int));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("CanalID", typeof(int));
                    tabela.Columns.Add("ClienteNome", typeof(string));
                    tabela.Columns.Add("CodigoBarra", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ValeIngressoTipoID"] = valeIngresso.ValeIngressoTipoID.Valor;
                        linha["CodigoTroca"] = valeIngresso.CodigoTroca.Valor;
                        linha["DataCriacao"] = valeIngresso.DataCriacao.Valor;
                        linha["DataExpiracao"] = valeIngresso.DataExpiracao.Valor;
                        linha["Status"] = valeIngresso.Status.Valor;
                        linha["VendaBilheteriaID"] = valeIngresso.VendaBilheteriaID.Valor;
                        linha["ClienteID"] = valeIngresso.ClienteID.Valor;
                        linha["SessionID"] = valeIngresso.SessionID.Valor;
                        linha["TimeStampReserva"] = valeIngresso.TimeStampReserva.Valor;
                        linha["LojaID"] = valeIngresso.LojaID.Valor;
                        linha["UsuarioID"] = valeIngresso.UsuarioID.Valor;
                        linha["CanalID"] = valeIngresso.CanalID.Valor;
                        linha["ClienteNome"] = valeIngresso.ClienteNome.Valor;
                        linha["CodigoBarra"] = valeIngresso.CodigoBarra.Valor;
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
                    case "ValeIngressoTipoID":
                        sql = "SELECT ID, ValeIngressoTipoID FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY ValeIngressoTipoID";
                        break;
                    case "CodigoTroca":
                        sql = "SELECT ID, CodigoTroca FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY CodigoTroca";
                        break;
                    case "DataCriacao":
                        sql = "SELECT ID, DataCriacao FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY DataCriacao";
                        break;
                    case "DataExpiracao":
                        sql = "SELECT ID, DataExpiracao FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY DataExpiracao";
                        break;
                    case "Status":
                        sql = "SELECT ID, Status FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY Status";
                        break;
                    case "VendaBilheteriaID":
                        sql = "SELECT ID, VendaBilheteriaID FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaID";
                        break;
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "SessionID":
                        sql = "SELECT ID, SessionID FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY SessionID";
                        break;
                    case "TimeStampReserva":
                        sql = "SELECT ID, TimeStampReserva FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY TimeStampReserva";
                        break;
                    case "LojaID":
                        sql = "SELECT ID, LojaID FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY LojaID";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "CanalID":
                        sql = "SELECT ID, CanalID FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY CanalID";
                        break;
                    case "ClienteNome":
                        sql = "SELECT ID, ClienteNome FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY ClienteNome";
                        break;
                    case "CodigoBarra":
                        sql = "SELECT ID, CodigoBarra FROM tValeIngresso WHERE " + FiltroSQL + " ORDER BY CodigoBarra";
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

    #region "ValeIngressoException"

    [Serializable]
    public class ValeIngressoException : Exception
    {

        public ValeIngressoException() : base() { }

        public ValeIngressoException(string msg) : base(msg) { }

        public ValeIngressoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}