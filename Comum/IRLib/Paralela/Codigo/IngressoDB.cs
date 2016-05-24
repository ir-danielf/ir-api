/******************************************************
* Arquivo IngressoDB.cs
* Gerado em: 20/03/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "Ingresso_B"

    public abstract class Ingresso_B : BaseBD
    {

        public apresentacaosetorid ApresentacaoSetorID = new apresentacaosetorid();
        public precoid PrecoID = new precoid();
        public lugarid LugarID = new lugarid();
        public gerenciamentoingressosID GerenciamentoIngressosID = new gerenciamentoingressosID();
        public usuarioid UsuarioID = new usuarioid();
        public cortesiaid CortesiaID = new cortesiaid();
        public bloqueioid BloqueioID = new bloqueioid();
        public codigo Codigo = new codigo();
        public codigobarra CodigoBarra = new codigobarra();
        public codigobarracliente CodigoBarraCliente = new codigobarracliente();
        public status Status = new status();
        public setorid SetorID = new setorid();
        public apresentacaoid ApresentacaoID = new apresentacaoid();
        public eventoid EventoID = new eventoid();
        public localid LocalID = new localid();
        public empresaid EmpresaID = new empresaid();
        public lojaid LojaID = new lojaid();
        public vendabilheteriaid VendaBilheteriaID = new vendabilheteriaid();
        public clienteid ClienteID = new clienteid();
        public pacoteid PacoteID = new pacoteid();
        public pacotegrupo PacoteGrupo = new pacotegrupo();
        public classificacao Classificacao = new classificacao();
        public grupo Grupo = new grupo();
        public sessionid SessionID = new sessionid();
        public timestampreserva TimeStampReserva = new timestampreserva();
        public codigosequencial CodigoSequencial = new codigosequencial();
        public codigoimpressao CodigoImpressao = new codigoimpressao();
        public assinaturaclienteid AssinaturaClienteID = new assinaturaclienteid();
        public serieid SerieID = new serieid();
        public compraguid CompraGUID = new compraguid();

        public Ingresso_B() { }

        /// <summary>
        /// Preenche todos os atributos de Ingresso
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tIngresso WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ApresentacaoSetorID.ValorBD = bd.LerInt("ApresentacaoSetorID").ToString();
                    this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
                    this.LugarID.ValorBD = bd.LerInt("LugarID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.CortesiaID.ValorBD = bd.LerInt("CortesiaID").ToString();
                    this.BloqueioID.ValorBD = bd.LerInt("BloqueioID").ToString();
                    this.Codigo.ValorBD = bd.LerString("Codigo");
                    this.CodigoBarra.ValorBD = bd.LerString("CodigoBarra");
                    this.CodigoBarraCliente.ValorBD = bd.LerString("CodigoBarraCliente");
                    this.Status.ValorBD = bd.LerString("Status");
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.PacoteID.ValorBD = bd.LerInt("PacoteID").ToString();
                    this.PacoteGrupo.ValorBD = bd.LerString("PacoteGrupo");
                    this.Classificacao.ValorBD = bd.LerInt("Classificacao").ToString();
                    this.Grupo.ValorBD = bd.LerInt("Grupo").ToString();
                    this.SessionID.ValorBD = bd.LerString("SessionID");
                    this.TimeStampReserva.ValorBD = bd.LerString("TimeStampReserva");
                    this.CodigoSequencial.ValorBD = bd.LerInt("CodigoSequencial").ToString();
                    this.CodigoImpressao.ValorBD = bd.LerInt("CodigoImpressao").ToString();
                    this.AssinaturaClienteID.ValorBD = bd.LerInt("AssinaturaClienteID").ToString();
                    this.SerieID.ValorBD = bd.LerInt("SerieID").ToString();
                    this.CompraGUID.ValorBD = bd.LerString("CompraGUID");
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
        /// Inserir novo(a) Ingresso
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tIngresso(ApresentacaoSetorID, PrecoID, LugarID, UsuarioID, CortesiaID, BloqueioID, Codigo, CodigoBarra, CodigoBarraCliente, Status, SetorID, ApresentacaoID, EventoID, LocalID, EmpresaID, LojaID, VendaBilheteriaID, ClienteID, PacoteID, PacoteGrupo, Classificacao, Grupo, SessionID, TimeStampReserva, CodigoSequencial, CodigoImpressao, AssinaturaClienteID, SerieID, CompraGUID) ");
                sql.Append("VALUES (@001,@002,@003,@004,@005,@006,'@007','@008','@009','@010',@011,@012,@013,@014,@015,@016,@017,@018,@019,'@020',@021,@022,'@023','@024',@025,@026,@027,@028,'@029'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@002", this.PrecoID.ValorBD);
                sql.Replace("@003", this.LugarID.ValorBD);
                sql.Replace("@004", this.UsuarioID.ValorBD);
                sql.Replace("@005", this.CortesiaID.ValorBD);
                sql.Replace("@006", this.BloqueioID.ValorBD);
                sql.Replace("@007", this.Codigo.ValorBD);
                sql.Replace("@008", this.CodigoBarra.ValorBD);
                sql.Replace("@009", this.CodigoBarraCliente.ValorBD);
                sql.Replace("@010", this.Status.ValorBD);
                sql.Replace("@011", this.SetorID.ValorBD);
                sql.Replace("@012", this.ApresentacaoID.ValorBD);
                sql.Replace("@013", this.EventoID.ValorBD);
                sql.Replace("@014", this.LocalID.ValorBD);
                sql.Replace("@015", this.EmpresaID.ValorBD);
                sql.Replace("@016", this.LojaID.ValorBD);
                sql.Replace("@017", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@018", this.ClienteID.ValorBD);
                sql.Replace("@019", this.PacoteID.ValorBD);
                sql.Replace("@020", this.PacoteGrupo.ValorBD);
                sql.Replace("@021", this.Classificacao.ValorBD);
                sql.Replace("@022", this.Grupo.ValorBD);
                sql.Replace("@023", this.SessionID.ValorBD);
                sql.Replace("@024", this.TimeStampReserva.ValorBD);
                sql.Replace("@025", this.CodigoSequencial.ValorBD);
                sql.Replace("@026", this.CodigoImpressao.ValorBD);
                sql.Replace("@027", this.AssinaturaClienteID.ValorBD);
                sql.Replace("@028", this.SerieID.ValorBD);
                sql.Replace("@029", this.CompraGUID.ValorBD);

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
        /// Inserir novo(a) Ingresso
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tIngresso(ApresentacaoSetorID, PrecoID, LugarID, UsuarioID, CortesiaID, BloqueioID, Codigo, CodigoBarra, CodigoBarraCliente, Status, SetorID, ApresentacaoID, EventoID, LocalID, EmpresaID, LojaID, VendaBilheteriaID, ClienteID, PacoteID, PacoteGrupo, Classificacao, Grupo, SessionID, TimeStampReserva, CodigoSequencial, CodigoImpressao, AssinaturaClienteID, SerieID, CompraGUID) ");
            sql.Append("VALUES (@001,@002,@003,@004,@005,@006,'@007','@008','@009','@010',@011,@012,@013,@014,@015,@016,@017,@018,@019,'@020',@021,@022,'@023','@024',@025,@026,@027,@028,'@029'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
            sql.Replace("@002", this.PrecoID.ValorBD);
            sql.Replace("@003", this.LugarID.ValorBD);
            sql.Replace("@004", this.UsuarioID.ValorBD);
            sql.Replace("@005", this.CortesiaID.ValorBD);
            sql.Replace("@006", this.BloqueioID.ValorBD);
            sql.Replace("@007", this.Codigo.ValorBD);
            sql.Replace("@008", this.CodigoBarra.ValorBD);
            sql.Replace("@009", this.CodigoBarraCliente.ValorBD);
            sql.Replace("@010", this.Status.ValorBD);
            sql.Replace("@011", this.SetorID.ValorBD);
            sql.Replace("@012", this.ApresentacaoID.ValorBD);
            sql.Replace("@013", this.EventoID.ValorBD);
            sql.Replace("@014", this.LocalID.ValorBD);
            sql.Replace("@015", this.EmpresaID.ValorBD);
            sql.Replace("@016", this.LojaID.ValorBD);
            sql.Replace("@017", this.VendaBilheteriaID.ValorBD);
            sql.Replace("@018", this.ClienteID.ValorBD);
            sql.Replace("@019", this.PacoteID.ValorBD);
            sql.Replace("@020", this.PacoteGrupo.ValorBD);
            sql.Replace("@021", this.Classificacao.ValorBD);
            sql.Replace("@022", this.Grupo.ValorBD);
            sql.Replace("@023", this.SessionID.ValorBD);
            sql.Replace("@024", this.TimeStampReserva.ValorBD);
            sql.Replace("@025", this.CodigoSequencial.ValorBD);
            sql.Replace("@026", this.CodigoImpressao.ValorBD);
            sql.Replace("@027", this.AssinaturaClienteID.ValorBD);
            sql.Replace("@028", this.SerieID.ValorBD);
            sql.Replace("@029", this.CompraGUID.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza Ingresso
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tIngresso SET ApresentacaoSetorID = @001, PrecoID = @002, LugarID = @003, UsuarioID = @004, CortesiaID = @005, BloqueioID = @006, Codigo = '@007', CodigoBarra = '@008', CodigoBarraCliente = '@009', Status = '@010', SetorID = @011, ApresentacaoID = @012, EventoID = @013, LocalID = @014, EmpresaID = @015, LojaID = @016, VendaBilheteriaID = @017, ClienteID = @018, PacoteID = @019, PacoteGrupo = '@020', Classificacao = @021, Grupo = @022, SessionID = '@023', TimeStampReserva = '@024', CodigoSequencial = @025, CodigoImpressao = @026, AssinaturaClienteID = @027, SerieID = @028, CompraGUID = '@029' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@002", this.PrecoID.ValorBD);
                sql.Replace("@003", this.LugarID.ValorBD);
                sql.Replace("@004", this.UsuarioID.ValorBD);
                sql.Replace("@005", this.CortesiaID.ValorBD);
                sql.Replace("@006", this.BloqueioID.ValorBD);
                sql.Replace("@007", this.Codigo.ValorBD);
                sql.Replace("@008", this.CodigoBarra.ValorBD);
                sql.Replace("@009", this.CodigoBarraCliente.ValorBD);
                sql.Replace("@010", this.Status.ValorBD);
                sql.Replace("@011", this.SetorID.ValorBD);
                sql.Replace("@012", this.ApresentacaoID.ValorBD);
                sql.Replace("@013", this.EventoID.ValorBD);
                sql.Replace("@014", this.LocalID.ValorBD);
                sql.Replace("@015", this.EmpresaID.ValorBD);
                sql.Replace("@016", this.LojaID.ValorBD);
                sql.Replace("@017", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@018", this.ClienteID.ValorBD);
                sql.Replace("@019", this.PacoteID.ValorBD);
                sql.Replace("@020", this.PacoteGrupo.ValorBD);
                sql.Replace("@021", this.Classificacao.ValorBD);
                sql.Replace("@022", this.Grupo.ValorBD);
                sql.Replace("@023", this.SessionID.ValorBD);
                sql.Replace("@024", this.TimeStampReserva.ValorBD);
                sql.Replace("@025", this.CodigoSequencial.ValorBD);
                sql.Replace("@026", this.CodigoImpressao.ValorBD);
                sql.Replace("@027", this.AssinaturaClienteID.ValorBD);
                sql.Replace("@028", this.SerieID.ValorBD);
                sql.Replace("@029", this.CompraGUID.ValorBD);

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
        /// Atualiza Ingresso
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tIngresso SET ApresentacaoSetorID = @001, PrecoID = @002, LugarID = @003, UsuarioID = @004, CortesiaID = @005, BloqueioID = @006, Codigo = '@007', CodigoBarra = '@008', CodigoBarraCliente = '@009', Status = '@010', SetorID = @011, ApresentacaoID = @012, EventoID = @013, LocalID = @014, EmpresaID = @015, LojaID = @016, VendaBilheteriaID = @017, ClienteID = @018, PacoteID = @019, PacoteGrupo = '@020', Classificacao = @021, Grupo = @022, SessionID = '@023', TimeStampReserva = '@024', CodigoSequencial = @025, CodigoImpressao = @026, AssinaturaClienteID = @027, SerieID = @028, CompraGUID = '@029' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
            sql.Replace("@002", this.PrecoID.ValorBD);
            sql.Replace("@003", this.LugarID.ValorBD);
            sql.Replace("@004", this.UsuarioID.ValorBD);
            sql.Replace("@005", this.CortesiaID.ValorBD);
            sql.Replace("@006", this.BloqueioID.ValorBD);
            sql.Replace("@007", this.Codigo.ValorBD);
            sql.Replace("@008", this.CodigoBarra.ValorBD);
            sql.Replace("@009", this.CodigoBarraCliente.ValorBD);
            sql.Replace("@010", this.Status.ValorBD);
            sql.Replace("@011", this.SetorID.ValorBD);
            sql.Replace("@012", this.ApresentacaoID.ValorBD);
            sql.Replace("@013", this.EventoID.ValorBD);
            sql.Replace("@014", this.LocalID.ValorBD);
            sql.Replace("@015", this.EmpresaID.ValorBD);
            sql.Replace("@016", this.LojaID.ValorBD);
            sql.Replace("@017", this.VendaBilheteriaID.ValorBD);
            sql.Replace("@018", this.ClienteID.ValorBD);
            sql.Replace("@019", this.PacoteID.ValorBD);
            sql.Replace("@020", this.PacoteGrupo.ValorBD);
            sql.Replace("@021", this.Classificacao.ValorBD);
            sql.Replace("@022", this.Grupo.ValorBD);
            sql.Replace("@023", this.SessionID.ValorBD);
            sql.Replace("@024", this.TimeStampReserva.ValorBD);
            sql.Replace("@025", this.CodigoSequencial.ValorBD);
            sql.Replace("@026", this.CodigoImpressao.ValorBD);
            sql.Replace("@027", this.AssinaturaClienteID.ValorBD);
            sql.Replace("@028", this.SerieID.ValorBD);
            sql.Replace("@029", this.CompraGUID.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui Ingresso com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tIngresso WHERE ID=" + id;

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
        /// Exclui Ingresso com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tIngresso WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui Ingresso
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

            this.ApresentacaoSetorID.Limpar();
            this.PrecoID.Limpar();
            this.LugarID.Limpar();
            this.UsuarioID.Limpar();
            this.CortesiaID.Limpar();
            this.BloqueioID.Limpar();
            this.Codigo.Limpar();
            this.CodigoBarra.Limpar();
            this.CodigoBarraCliente.Limpar();
            this.Status.Limpar();
            this.SetorID.Limpar();
            this.ApresentacaoID.Limpar();
            this.EventoID.Limpar();
            this.LocalID.Limpar();
            this.EmpresaID.Limpar();
            this.LojaID.Limpar();
            this.VendaBilheteriaID.Limpar();
            this.ClienteID.Limpar();
            this.PacoteID.Limpar();
            this.PacoteGrupo.Limpar();
            this.Classificacao.Limpar();
            this.Grupo.Limpar();
            this.SessionID.Limpar();
            this.TimeStampReserva.Limpar();
            this.CodigoSequencial.Limpar();
            this.CodigoImpressao.Limpar();
            this.AssinaturaClienteID.Limpar();
            this.SerieID.Limpar();
            this.CompraGUID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.ApresentacaoSetorID.Desfazer();
            this.PrecoID.Desfazer();
            this.LugarID.Desfazer();
            this.UsuarioID.Desfazer();
            this.CortesiaID.Desfazer();
            this.BloqueioID.Desfazer();
            this.Codigo.Desfazer();
            this.CodigoBarra.Desfazer();
            this.CodigoBarraCliente.Desfazer();
            this.Status.Desfazer();
            this.SetorID.Desfazer();
            this.ApresentacaoID.Desfazer();
            this.EventoID.Desfazer();
            this.LocalID.Desfazer();
            this.EmpresaID.Desfazer();
            this.LojaID.Desfazer();
            this.VendaBilheteriaID.Desfazer();
            this.ClienteID.Desfazer();
            this.PacoteID.Desfazer();
            this.PacoteGrupo.Desfazer();
            this.Classificacao.Desfazer();
            this.Grupo.Desfazer();
            this.SessionID.Desfazer();
            this.TimeStampReserva.Desfazer();
            this.CodigoSequencial.Desfazer();
            this.CodigoImpressao.Desfazer();
            this.AssinaturaClienteID.Desfazer();
            this.SerieID.Desfazer();
            this.CompraGUID.Desfazer();
        }

        public class apresentacaosetorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoSetorID";
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

        public class precoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoID";
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

        public class gerenciamentoingressosID : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "GerenciamentoIngressosID";
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

        public class cortesiaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CortesiaID";
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

        public class bloqueioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "BloqueioID";
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

        public class codigo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Codigo";
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
                    return 30;
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

        public class codigobarracliente : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoBarraCliente";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 30;
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
                    return 0;
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

        public class apresentacaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoID";
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

        public class eventoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EventoID";
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

        public class localid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalID";
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

        public class empresaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaID";
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

        public class pacoteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PacoteID";
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

        public class pacotegrupo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PacoteGrupo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 6;
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

        public class classificacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Classificacao";
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

        public class grupo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Grupo";
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

        public class codigosequencial : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoSequencial";
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

        public class codigoimpressao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoImpressao";
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

        public class serieid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SerieID";
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

        public class compraguid : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CompraGUID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 120;
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

                DataTable tabela = new DataTable("Ingresso");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("LugarID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("CortesiaID", typeof(int));
                tabela.Columns.Add("BloqueioID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("CodigoBarraCliente", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("PacoteID", typeof(int));
                tabela.Columns.Add("PacoteGrupo", typeof(string));
                tabela.Columns.Add("Classificacao", typeof(int));
                tabela.Columns.Add("Grupo", typeof(int));
                tabela.Columns.Add("SessionID", typeof(string));
                tabela.Columns.Add("TimeStampReserva", typeof(DateTime));
                tabela.Columns.Add("CodigoSequencial", typeof(int));
                tabela.Columns.Add("CodigoImpressao", typeof(int));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                tabela.Columns.Add("SerieID", typeof(int));
                tabela.Columns.Add("CompraGUID", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataRow InfoVendido();

        public abstract DataRow InfoReservado();

        public abstract DataTable Historico();

        public abstract int TotalPorCanalEvento(int canalid, int eventoid);

        public abstract int QuantidadePorBloqueio(int apresentacaosetorid, int bloqueioid);

        public abstract string SenhaVenda(string codigo, int apresentacaosetorid);

        public abstract bool ChecarStatusCancelar();

        public abstract bool Entregar(int usuarioid, int caixaid, int canalid, int lojaid, int empresaid, string obs);

        public abstract void CancelarPacote();

        public abstract bool EmpacotarReserva(int[] ingressosids);

        public abstract bool ReservarPacote(int[] ingressosids);

        public abstract bool CancelarReserva(int usuarioid);

        public abstract bool CancelarReserva();

        public abstract bool CancelarReservas(int usuarioid);

        public abstract bool CancelarReservas(int[] ingressosids);

        public abstract bool ReImprimir(string motivo);

        public abstract bool PreImprimir(int caixaid, int lojaid, int canalid, int empresaid);

        public abstract bool TransferirPreImpresso(int caixaid, int lojaid, int canalid, int empresaid);

        public abstract bool Desbloquear(int usuarioid, int empresaid);

        public abstract bool Bloquear(int bloqueioid, int usuarioid, int empresaid);

        public abstract bool Reservar();

        public abstract void Identifica(int apresentacaoid, int setorid);

        public abstract bool ExcluirCascata();

        public abstract DataTable EventoApresentacaoSetor();

        public abstract bool VendidoUmaVez();

        public abstract int VendaBilheteriaItemID();

        public abstract DataTable InfoReservados(int[] ingressosids, CTLib.BD database, int lojaid);

        public abstract bool NovoNaoMarcado(int apresentacaosetorid, int eventoid, int apresentacaoid, int setorid, int empresaid, int localid, int bloqueioid, int codigo, BD database, int codigosequencial, string codigobarra);

        public abstract bool NovoMarcado(int apresentacaosetorid, int eventoid, int apresentacaoid, int setorid, int empresaid, int localid, int bloqueioid, int lugarid, int qtdelugar, int qtdebloqueada, string codigo, int grupo, int classificacao, string tiposetor, BD database, int codigosequencial, string[] codigosdebarra);

        public abstract DataTable InfoVendidos(int[] ingressosids);

        public abstract int VerificaCodigoPrecoExclusivo(int codigo);

        public abstract bool AtualizaCodigoPrecoExclusivo(int ingressosid, int codigo);

        public abstract bool zerarCodigoPrecoExclusivo(int clienteid, string sessionid);

    }

    #endregion

    #region "IngressoLista_B"

    public abstract class IngressoLista_B : BaseLista
    {

        protected Ingresso ingresso;

        // passar o Usuario logado no sistema
        public IngressoLista_B()
        {
            ingresso = new Ingresso();
        }

        public Ingresso Ingresso
        {
            get { return ingresso; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Ingresso especifico
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
                    ingresso.Ler(id);
                    return ingresso;
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
                    sql = "SELECT ID FROM tIngresso";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tIngresso";

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
                    sql = "SELECT ID FROM tIngresso";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tIngresso";

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
        /// Preenche Ingresso corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                ingresso.Ler(id);

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

                bool ok = ingresso.Excluir();
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

                        string sqlDelete = "DELETE FROM tIngresso WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) Ingresso na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = ingresso.Inserir();
                if (ok)
                {
                    lista.Add(ingresso.Control.ID);
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
        /// Obtem uma tabela de todos os campos de Ingresso carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Ingresso");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("LugarID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("CortesiaID", typeof(int));
                tabela.Columns.Add("BloqueioID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("CodigoBarraCliente", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("PacoteID", typeof(int));
                tabela.Columns.Add("PacoteGrupo", typeof(string));
                tabela.Columns.Add("Classificacao", typeof(int));
                tabela.Columns.Add("Grupo", typeof(int));
                tabela.Columns.Add("SessionID", typeof(string));
                tabela.Columns.Add("TimeStampReserva", typeof(DateTime));
                tabela.Columns.Add("CodigoSequencial", typeof(int));
                tabela.Columns.Add("CodigoImpressao", typeof(int));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                tabela.Columns.Add("SerieID", typeof(int));
                tabela.Columns.Add("CompraGUID", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = ingresso.Control.ID;
                        linha["ApresentacaoSetorID"] = ingresso.ApresentacaoSetorID.Valor;
                        linha["PrecoID"] = ingresso.PrecoID.Valor;
                        linha["LugarID"] = ingresso.LugarID.Valor;
                        linha["UsuarioID"] = ingresso.UsuarioID.Valor;
                        linha["CortesiaID"] = ingresso.CortesiaID.Valor;
                        linha["BloqueioID"] = ingresso.BloqueioID.Valor;
                        linha["Codigo"] = ingresso.Codigo.Valor;
                        linha["CodigoBarra"] = ingresso.CodigoBarra.Valor;
                        linha["CodigoBarraCliente"] = ingresso.CodigoBarraCliente.Valor;
                        linha["Status"] = ingresso.Status.Valor;
                        linha["SetorID"] = ingresso.SetorID.Valor;
                        linha["ApresentacaoID"] = ingresso.ApresentacaoID.Valor;
                        linha["EventoID"] = ingresso.EventoID.Valor;
                        linha["LocalID"] = ingresso.LocalID.Valor;
                        linha["EmpresaID"] = ingresso.EmpresaID.Valor;
                        linha["LojaID"] = ingresso.LojaID.Valor;
                        linha["VendaBilheteriaID"] = ingresso.VendaBilheteriaID.Valor;
                        linha["ClienteID"] = ingresso.ClienteID.Valor;
                        linha["PacoteID"] = ingresso.PacoteID.Valor;
                        linha["PacoteGrupo"] = ingresso.PacoteGrupo.Valor;
                        linha["Classificacao"] = ingresso.Classificacao.Valor;
                        linha["Grupo"] = ingresso.Grupo.Valor;
                        linha["SessionID"] = ingresso.SessionID.Valor;
                        linha["TimeStampReserva"] = ingresso.TimeStampReserva.Valor;
                        linha["CodigoSequencial"] = ingresso.CodigoSequencial.Valor;
                        linha["CodigoImpressao"] = ingresso.CodigoImpressao.Valor;
                        linha["AssinaturaClienteID"] = ingresso.AssinaturaClienteID.Valor;
                        linha["SerieID"] = ingresso.SerieID.Valor;
                        linha["CompraGUID"] = ingresso.CompraGUID.Valor;
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

                DataTable tabela = new DataTable("RelatorioIngresso");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                    tabela.Columns.Add("PrecoID", typeof(int));
                    tabela.Columns.Add("LugarID", typeof(int));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("CortesiaID", typeof(int));
                    tabela.Columns.Add("BloqueioID", typeof(int));
                    tabela.Columns.Add("Codigo", typeof(string));
                    tabela.Columns.Add("CodigoBarra", typeof(string));
                    tabela.Columns.Add("CodigoBarraCliente", typeof(string));
                    tabela.Columns.Add("Status", typeof(string));
                    tabela.Columns.Add("SetorID", typeof(int));
                    tabela.Columns.Add("ApresentacaoID", typeof(int));
                    tabela.Columns.Add("EventoID", typeof(int));
                    tabela.Columns.Add("LocalID", typeof(int));
                    tabela.Columns.Add("EmpresaID", typeof(int));
                    tabela.Columns.Add("LojaID", typeof(int));
                    tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("PacoteID", typeof(int));
                    tabela.Columns.Add("PacoteGrupo", typeof(string));
                    tabela.Columns.Add("Classificacao", typeof(int));
                    tabela.Columns.Add("Grupo", typeof(int));
                    tabela.Columns.Add("SessionID", typeof(string));
                    tabela.Columns.Add("TimeStampReserva", typeof(DateTime));
                    tabela.Columns.Add("CodigoSequencial", typeof(int));
                    tabela.Columns.Add("CodigoImpressao", typeof(int));
                    tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                    tabela.Columns.Add("SerieID", typeof(int));
                    tabela.Columns.Add("CompraGUID", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ApresentacaoSetorID"] = ingresso.ApresentacaoSetorID.Valor;
                        linha["PrecoID"] = ingresso.PrecoID.Valor;
                        linha["LugarID"] = ingresso.LugarID.Valor;
                        linha["UsuarioID"] = ingresso.UsuarioID.Valor;
                        linha["CortesiaID"] = ingresso.CortesiaID.Valor;
                        linha["BloqueioID"] = ingresso.BloqueioID.Valor;
                        linha["Codigo"] = ingresso.Codigo.Valor;
                        linha["CodigoBarra"] = ingresso.CodigoBarra.Valor;
                        linha["CodigoBarraCliente"] = ingresso.CodigoBarraCliente.Valor;
                        linha["Status"] = ingresso.Status.Valor;
                        linha["SetorID"] = ingresso.SetorID.Valor;
                        linha["ApresentacaoID"] = ingresso.ApresentacaoID.Valor;
                        linha["EventoID"] = ingresso.EventoID.Valor;
                        linha["LocalID"] = ingresso.LocalID.Valor;
                        linha["EmpresaID"] = ingresso.EmpresaID.Valor;
                        linha["LojaID"] = ingresso.LojaID.Valor;
                        linha["VendaBilheteriaID"] = ingresso.VendaBilheteriaID.Valor;
                        linha["ClienteID"] = ingresso.ClienteID.Valor;
                        linha["PacoteID"] = ingresso.PacoteID.Valor;
                        linha["PacoteGrupo"] = ingresso.PacoteGrupo.Valor;
                        linha["Classificacao"] = ingresso.Classificacao.Valor;
                        linha["Grupo"] = ingresso.Grupo.Valor;
                        linha["SessionID"] = ingresso.SessionID.Valor;
                        linha["TimeStampReserva"] = ingresso.TimeStampReserva.Valor;
                        linha["CodigoSequencial"] = ingresso.CodigoSequencial.Valor;
                        linha["CodigoImpressao"] = ingresso.CodigoImpressao.Valor;
                        linha["AssinaturaClienteID"] = ingresso.AssinaturaClienteID.Valor;
                        linha["SerieID"] = ingresso.SerieID.Valor;
                        linha["CompraGUID"] = ingresso.CompraGUID.Valor;
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
                    case "ApresentacaoSetorID":
                        sql = "SELECT ID, ApresentacaoSetorID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY ApresentacaoSetorID";
                        break;
                    case "PrecoID":
                        sql = "SELECT ID, PrecoID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY PrecoID";
                        break;
                    case "LugarID":
                        sql = "SELECT ID, LugarID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY LugarID";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "CortesiaID":
                        sql = "SELECT ID, CortesiaID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY CortesiaID";
                        break;
                    case "BloqueioID":
                        sql = "SELECT ID, BloqueioID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY BloqueioID";
                        break;
                    case "Codigo":
                        sql = "SELECT ID, Codigo FROM tIngresso WHERE " + FiltroSQL + " ORDER BY Codigo";
                        break;
                    case "CodigoBarra":
                        sql = "SELECT ID, CodigoBarra FROM tIngresso WHERE " + FiltroSQL + " ORDER BY CodigoBarra";
                        break;
                    case "CodigoBarraCliente":
                        sql = "SELECT ID, CodigoBarraCliente FROM tIngresso WHERE " + FiltroSQL + " ORDER BY CodigoBarraCliente";
                        break;
                    case "Status":
                        sql = "SELECT ID, Status FROM tIngresso WHERE " + FiltroSQL + " ORDER BY Status";
                        break;
                    case "SetorID":
                        sql = "SELECT ID, SetorID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY SetorID";
                        break;
                    case "ApresentacaoID":
                        sql = "SELECT ID, ApresentacaoID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY ApresentacaoID";
                        break;
                    case "EventoID":
                        sql = "SELECT ID, EventoID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;
                    case "LocalID":
                        sql = "SELECT ID, LocalID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY LocalID";
                        break;
                    case "EmpresaID":
                        sql = "SELECT ID, EmpresaID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY EmpresaID";
                        break;
                    case "LojaID":
                        sql = "SELECT ID, LojaID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY LojaID";
                        break;
                    case "VendaBilheteriaID":
                        sql = "SELECT ID, VendaBilheteriaID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaID";
                        break;
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "PacoteID":
                        sql = "SELECT ID, PacoteID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY PacoteID";
                        break;
                    case "PacoteGrupo":
                        sql = "SELECT ID, PacoteGrupo FROM tIngresso WHERE " + FiltroSQL + " ORDER BY PacoteGrupo";
                        break;
                    case "Classificacao":
                        sql = "SELECT ID, Classificacao FROM tIngresso WHERE " + FiltroSQL + " ORDER BY Classificacao";
                        break;
                    case "Grupo":
                        sql = "SELECT ID, Grupo FROM tIngresso WHERE " + FiltroSQL + " ORDER BY Grupo";
                        break;
                    case "SessionID":
                        sql = "SELECT ID, SessionID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY SessionID";
                        break;
                    case "TimeStampReserva":
                        sql = "SELECT ID, TimeStampReserva FROM tIngresso WHERE " + FiltroSQL + " ORDER BY TimeStampReserva";
                        break;
                    case "CodigoSequencial":
                        sql = "SELECT ID, CodigoSequencial FROM tIngresso WHERE " + FiltroSQL + " ORDER BY CodigoSequencial";
                        break;
                    case "CodigoImpressao":
                        sql = "SELECT ID, CodigoImpressao FROM tIngresso WHERE " + FiltroSQL + " ORDER BY CodigoImpressao";
                        break;
                    case "AssinaturaClienteID":
                        sql = "SELECT ID, AssinaturaClienteID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY AssinaturaClienteID";
                        break;
                    case "SerieID":
                        sql = "SELECT ID, SerieID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY SerieID";
                        break;
                    case "CompraGUID":
                        sql = "SELECT ID, CompraGUID FROM tIngresso WHERE " + FiltroSQL + " ORDER BY CompraGUID";
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

    #region "IngressoException"

    [Serializable]
    public class IngressoException : Exception
    {

        public IngressoException() : base() { }

        public IngressoException(string msg) : base(msg) { }

        public IngressoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}