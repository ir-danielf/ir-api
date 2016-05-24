/******************************************************
* Arquivo ApresentacaoDB.cs
* Gerado em: 06/06/2014
* Autor: Celeritas Ltda
*******************************************************/

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib
{

    #region "Apresentacao_B"

    public abstract class Apresentacao_B : BaseBD
    {

        public eventoid EventoID = new eventoid();
        public horario Horario = new horario();
        public disponivelvenda DisponivelVenda = new disponivelvenda();
        public disponivelajuste DisponivelAjuste = new disponivelajuste();
        public disponivelrelatorio DisponivelRelatorio = new disponivelrelatorio();
        public versaoimagemingresso VersaoImagemIngresso = new versaoimagemingresso();
        public versaoimagemvale VersaoImagemVale = new versaoimagemvale();
        public versaoimagemvale2 VersaoImagemVale2 = new versaoimagemvale2();
        public versaoimagemvale3 VersaoImagemVale3 = new versaoimagemvale3();
        public impressao Impressao = new impressao();
        public localimagemmapaid LocalImagemMapaID = new localimagemmapaid();
        public descricaopadrao DescricaoPadrao = new descricaopadrao();
        public descricao Descricao = new descricao();
        public ultimocodigoimpresso UltimoCodigoImpresso = new ultimocodigoimpresso();
        public cotaid CotaID = new cotaid();
        public quantidade Quantidade = new quantidade();
        public quantidadeporcliente QuantidadePorCliente = new quantidadeporcliente();
        public mapaesquematicoid MapaEsquematicoID = new mapaesquematicoid();
        public programacao Programacao = new programacao();
        public codigoprogramacao CodigoProgramacao = new codigoprogramacao();
        public sincronizado Sincronizado = new sincronizado();
        public alvara Alvara = new alvara();
        public avcb AVCB = new avcb();
        public dataemissaoalvara DataEmissaoAlvara = new dataemissaoalvara();
        public datavalidadealvara DataValidadeAlvara = new datavalidadealvara();
        public dataemissaoavcb DataEmissaoAvcb = new dataemissaoavcb();
        public datavalidadeavcb DataValidadeAvcb = new datavalidadeavcb();
        public lotacao Lotacao = new lotacao();
        public cancelada Cancelada = new cancelada();

        public Apresentacao_B() { }

        // passar o Usuario logado no sistema
        public Apresentacao_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Apresentacao
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tApresentacao WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.Horario.ValorBD = bd.LerString("Horario");
                    this.DisponivelVenda.ValorBD = bd.LerString("DisponivelVenda");
                    this.DisponivelAjuste.ValorBD = bd.LerString("DisponivelAjuste");
                    this.DisponivelRelatorio.ValorBD = bd.LerString("DisponivelRelatorio");
                    this.VersaoImagemIngresso.ValorBD = bd.LerInt("VersaoImagemIngresso").ToString();
                    this.VersaoImagemVale.ValorBD = bd.LerInt("VersaoImagemVale").ToString();
                    this.VersaoImagemVale2.ValorBD = bd.LerInt("VersaoImagemVale2").ToString();
                    this.VersaoImagemVale3.ValorBD = bd.LerInt("VersaoImagemVale3").ToString();
                    this.Impressao.ValorBD = bd.LerString("Impressao");
                    this.LocalImagemMapaID.ValorBD = bd.LerInt("LocalImagemMapaID").ToString();
                    this.DescricaoPadrao.ValorBD = bd.LerString("DescricaoPadrao");
                    this.Descricao.ValorBD = bd.LerString("Descricao");
                    this.UltimoCodigoImpresso.ValorBD = bd.LerInt("UltimoCodigoImpresso").ToString();
                    this.CotaID.ValorBD = bd.LerInt("CotaID").ToString();
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                    this.MapaEsquematicoID.ValorBD = bd.LerInt("MapaEsquematicoID").ToString();
                    this.Programacao.ValorBD = bd.LerString("Programacao");
                    this.CodigoProgramacao.ValorBD = bd.LerString("CodigoProgramacao");
                    this.Sincronizado.ValorBD = bd.LerString("Sincronizado");
                    this.Alvara.ValorBD = bd.LerString("Alvara");
                    this.AVCB.ValorBD = bd.LerString("AVCB");
                    this.DataEmissaoAlvara.ValorBD = bd.LerString("DataEmissaoAlvara");
                    this.DataValidadeAlvara.ValorBD = bd.LerString("DataValidadeAlvara");
                    this.DataEmissaoAvcb.ValorBD = bd.LerString("DataEmissaoAvcb");
                    this.DataValidadeAvcb.ValorBD = bd.LerString("DataValidadeAvcb");
                    this.Lotacao.ValorBD = bd.LerInt("Lotacao").ToString();
                    this.Cancelada.ValorBD = bd.LerString("Cancelada");
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

        }

        /// <summary>
        /// Preenche todos os atributos de Apresentacao do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xApresentacao WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.Horario.ValorBD = bd.LerString("Horario");
                    this.DisponivelVenda.ValorBD = bd.LerString("DisponivelVenda");
                    this.DisponivelAjuste.ValorBD = bd.LerString("DisponivelAjuste");
                    this.DisponivelRelatorio.ValorBD = bd.LerString("DisponivelRelatorio");
                    this.VersaoImagemIngresso.ValorBD = bd.LerInt("VersaoImagemIngresso").ToString();
                    this.VersaoImagemVale.ValorBD = bd.LerInt("VersaoImagemVale").ToString();
                    this.VersaoImagemVale2.ValorBD = bd.LerInt("VersaoImagemVale2").ToString();
                    this.VersaoImagemVale3.ValorBD = bd.LerInt("VersaoImagemVale3").ToString();
                    this.Impressao.ValorBD = bd.LerString("Impressao");
                    this.LocalImagemMapaID.ValorBD = bd.LerInt("LocalImagemMapaID").ToString();
                    this.DescricaoPadrao.ValorBD = bd.LerString("DescricaoPadrao");
                    this.Descricao.ValorBD = bd.LerString("Descricao");
                    this.UltimoCodigoImpresso.ValorBD = bd.LerInt("UltimoCodigoImpresso").ToString();
                    this.CotaID.ValorBD = bd.LerInt("CotaID").ToString();
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                    this.MapaEsquematicoID.ValorBD = bd.LerInt("MapaEsquematicoID").ToString();
                    this.Programacao.ValorBD = bd.LerString("Programacao");
                    this.CodigoProgramacao.ValorBD = bd.LerString("CodigoProgramacao");
                    this.Sincronizado.ValorBD = bd.LerString("Sincronizado");
                    this.Alvara.ValorBD = bd.LerString("Alvara");
                    this.AVCB.ValorBD = bd.LerString("AVCB");
                    this.DataEmissaoAlvara.ValorBD = bd.LerString("DataEmissaoAlvara");
                    this.DataValidadeAlvara.ValorBD = bd.LerString("DataValidadeAlvara");
                    this.DataEmissaoAvcb.ValorBD = bd.LerString("DataEmissaoAvcb");
                    this.DataValidadeAvcb.ValorBD = bd.LerString("DataValidadeAvcb");
                    this.Lotacao.ValorBD = bd.LerInt("Lotacao").ToString();
                    this.Cancelada.ValorBD = bd.LerString("Cancelada");
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirControle(string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cApresentacao (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirLog()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xApresentacao (ID, Versao, EventoID, Horario, DisponivelVenda, DisponivelAjuste, DisponivelRelatorio, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, Impressao, LocalImagemMapaID, DescricaoPadrao, Descricao, UltimoCodigoImpresso, CotaID, Quantidade, QuantidadePorCliente, MapaEsquematicoID, Programacao, CodigoProgramacao, Sincronizado, Alvara, AVCB, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, Cancelada) ");
                sql.Append("SELECT ID, @V, EventoID, Horario, DisponivelVenda, DisponivelAjuste, DisponivelRelatorio, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, Impressao, LocalImagemMapaID, DescricaoPadrao, Descricao, UltimoCodigoImpresso, CotaID, Quantidade, QuantidadePorCliente, MapaEsquematicoID, Programacao, CodigoProgramacao, Sincronizado, Alvara, AVCB, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, Cancelada FROM tApresentacao WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) Apresentacao
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cApresentacao");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tApresentacao(ID, EventoID, Horario, DisponivelVenda, DisponivelAjuste, DisponivelRelatorio, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, Impressao, LocalImagemMapaID, DescricaoPadrao, Descricao, UltimoCodigoImpresso, CotaID, Quantidade, QuantidadePorCliente, MapaEsquematicoID, Programacao, CodigoProgramacao, Sincronizado, Alvara, AVCB, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, Cancelada) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005',@006,@007,@008,@009,'@010',@011,'@012','@013',@014,@015,@016,@017,@018,'@019','@020','@021','@022','@023','@024','@025','@026','@027',@028, '@029')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.Horario.ValorBD);
                sql.Replace("@003", this.DisponivelVenda.ValorBD);
                sql.Replace("@004", this.DisponivelAjuste.ValorBD);
                sql.Replace("@005", this.DisponivelRelatorio.ValorBD);
                sql.Replace("@006", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale.ValorBD);
                sql.Replace("@008", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@009", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@010", this.Impressao.ValorBD);
                sql.Replace("@011", this.LocalImagemMapaID.ValorBD);
                sql.Replace("@012", this.DescricaoPadrao.ValorBD);
                sql.Replace("@013", this.Descricao.ValorBD);
                sql.Replace("@014", this.UltimoCodigoImpresso.ValorBD);
                sql.Replace("@015", this.CotaID.ValorBD);
                sql.Replace("@016", this.Quantidade.ValorBD);
                sql.Replace("@017", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@018", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@019", this.Programacao.ValorBD);
                sql.Replace("@020", this.CodigoProgramacao.ValorBD);
                sql.Replace("@021", this.Sincronizado.ValorBD);
                sql.Replace("@022", this.Alvara.ValorBD);
                sql.Replace("@023", this.AVCB.ValorBD);
                sql.Replace("@024", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@025", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@026", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@027", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@028", this.Lotacao.ValorBD);
                sql.Replace("@029", this.Cancelada.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Inserir novo(a) Apresentacao
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cApresentacao");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tApresentacao(ID, EventoID, Horario, DisponivelVenda, DisponivelAjuste, DisponivelRelatorio, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, Impressao, LocalImagemMapaID, DescricaoPadrao, Descricao, UltimoCodigoImpresso, CotaID, Quantidade, QuantidadePorCliente, MapaEsquematicoID, Programacao, CodigoProgramacao, Sincronizado, Alvara, AVCB, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, Cancelada) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005',@006,@007,@008,@009,'@010',@011,'@012','@013',@014,@015,@016,@017,@018,'@019','@020','@021','@022','@023','@024','@025','@026','@027',@028, '@029')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.Horario.ValorBD);
                sql.Replace("@003", this.DisponivelVenda.ValorBD);
                sql.Replace("@004", this.DisponivelAjuste.ValorBD);
                sql.Replace("@005", this.DisponivelRelatorio.ValorBD);
                sql.Replace("@006", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale.ValorBD);
                sql.Replace("@008", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@009", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@010", this.Impressao.ValorBD);
                sql.Replace("@011", this.LocalImagemMapaID.ValorBD);
                sql.Replace("@012", this.DescricaoPadrao.ValorBD);
                sql.Replace("@013", this.Descricao.ValorBD);
                sql.Replace("@014", this.UltimoCodigoImpresso.ValorBD);
                sql.Replace("@015", this.CotaID.ValorBD);
                sql.Replace("@016", this.Quantidade.ValorBD);
                sql.Replace("@017", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@018", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@019", this.Programacao.ValorBD);
                sql.Replace("@020", this.CodigoProgramacao.ValorBD);
                sql.Replace("@021", this.Sincronizado.ValorBD);
                sql.Replace("@022", this.Alvara.ValorBD);
                sql.Replace("@023", this.AVCB.ValorBD);
                sql.Replace("@024", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@025", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@026", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@027", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@028", this.Lotacao.ValorBD);
                sql.Replace("@029", this.Cancelada.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza Apresentacao
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cApresentacao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                string CotaID;

                if (this.CotaID == null)
                {
                    CotaID = "null";
                    this.CotaID = new cotaid();
                    this.CotaID.Valor = 0;
                }
                else
                    CotaID = this.CotaID.ValorBD;

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tApresentacao SET EventoID = @001, Horario = '@002', DisponivelVenda = '@003', DisponivelAjuste = '@004', DisponivelRelatorio = '@005', VersaoImagemIngresso = @006, VersaoImagemVale = @007, VersaoImagemVale2 = @008, VersaoImagemVale3 = @009, Impressao = '@010', LocalImagemMapaID = @011, DescricaoPadrao = '@012', Descricao = '@013', UltimoCodigoImpresso = @014, CotaID = @015, Quantidade = @016, QuantidadePorCliente = @017, MapaEsquematicoID = @018, Programacao = '@019', CodigoProgramacao = '@020', Sincronizado = '@021', Alvara = '@022', AVCB = '@023', DataEmissaoAlvara = '@024', DataValidadeAlvara = '@025', DataEmissaoAvcb = '@026', DataValidadeAvcb = '@027', Lotacao = @028, Cancelada = '@029' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.Horario.ValorBD);
                sql.Replace("@003", this.DisponivelVenda.ValorBD);
                sql.Replace("@004", this.DisponivelAjuste.ValorBD);
                sql.Replace("@005", this.DisponivelRelatorio.ValorBD);
                sql.Replace("@006", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale.ValorBD);
                sql.Replace("@008", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@009", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@010", this.Impressao.ValorBD);
                sql.Replace("@011", this.LocalImagemMapaID.ValorBD);
                sql.Replace("@012", this.DescricaoPadrao.ValorBD);
                sql.Replace("@013", this.Descricao.ValorBD);
                sql.Replace("@014", this.UltimoCodigoImpresso.ValorBD);
                sql.Replace("@015", CotaID);
                sql.Replace("@016", this.Quantidade.ValorBD);
                sql.Replace("@017", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@018", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@019", this.Programacao.ValorBD);
                sql.Replace("@020", this.CodigoProgramacao.ValorBD);
                sql.Replace("@021", this.Sincronizado.ValorBD);
                sql.Replace("@022", this.Alvara.ValorBD);
                sql.Replace("@023", this.AVCB.ValorBD);
                sql.Replace("@024", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@025", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@026", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@027", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@028", this.Lotacao.ValorBD);
                sql.Replace("@029", this.Cancelada.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Atualiza Apresentacao
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cApresentacao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tApresentacao SET EventoID = @001, Horario = '@002', DisponivelVenda = '@003', DisponivelAjuste = '@004', DisponivelRelatorio = '@005', VersaoImagemIngresso = @006, VersaoImagemVale = @007, VersaoImagemVale2 = @008, VersaoImagemVale3 = @009, Impressao = '@010', LocalImagemMapaID = @011, DescricaoPadrao = '@012', Descricao = '@013', UltimoCodigoImpresso = @014, CotaID = @015, Quantidade = @016, QuantidadePorCliente = @017, MapaEsquematicoID = @018, Programacao = '@019', CodigoProgramacao = '@020', Sincronizado = '@021', Alvara = '@022', AVCB = '@023', DataEmissaoAlvara = '@024', DataValidadeAlvara = '@025', DataEmissaoAvcb = '@026', DataValidadeAvcb = '@027', Lotacao = @028, Cancelada = '@029' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.Horario.ValorBD);
                sql.Replace("@003", this.DisponivelVenda.ValorBD);
                sql.Replace("@004", this.DisponivelAjuste.ValorBD);
                sql.Replace("@005", this.DisponivelRelatorio.ValorBD);
                sql.Replace("@006", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale.ValorBD);
                sql.Replace("@008", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@009", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@010", this.Impressao.ValorBD);
                sql.Replace("@011", this.LocalImagemMapaID.ValorBD);
                sql.Replace("@012", this.DescricaoPadrao.ValorBD);
                sql.Replace("@013", this.Descricao.ValorBD);
                sql.Replace("@014", this.UltimoCodigoImpresso.ValorBD);
                sql.Replace("@015", this.CotaID.ValorBD);
                sql.Replace("@016", this.Quantidade.ValorBD);
                sql.Replace("@017", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@018", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@019", this.Programacao.ValorBD);
                sql.Replace("@020", this.CodigoProgramacao.ValorBD);
                sql.Replace("@021", this.Sincronizado.ValorBD);
                sql.Replace("@022", this.Alvara.ValorBD);
                sql.Replace("@023", this.AVCB.ValorBD);
                sql.Replace("@024", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@025", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@026", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@027", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@028", this.Lotacao.ValorBD);
                sql.Replace("@029", this.Cancelada.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui Apresentacao com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cApresentacao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tApresentacao WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Exclui Apresentacao com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cApresentacao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tApresentacao WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui Apresentacao
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

            this.EventoID.Limpar();
            this.Horario.Limpar();
            this.DisponivelVenda.Limpar();
            this.DisponivelAjuste.Limpar();
            this.DisponivelRelatorio.Limpar();
            this.VersaoImagemIngresso.Limpar();
            this.VersaoImagemVale.Limpar();
            this.VersaoImagemVale2.Limpar();
            this.VersaoImagemVale3.Limpar();
            this.Impressao.Limpar();
            this.LocalImagemMapaID.Limpar();
            this.DescricaoPadrao.Limpar();
            this.Descricao.Limpar();
            this.UltimoCodigoImpresso.Limpar();
            this.CotaID.Limpar();
            this.Quantidade.Limpar();
            this.QuantidadePorCliente.Limpar();
            this.MapaEsquematicoID.Limpar();
            this.Programacao.Limpar();
            this.CodigoProgramacao.Limpar();
            this.Sincronizado.Limpar();
            this.Alvara.Limpar();
            this.AVCB.Limpar();
            this.DataEmissaoAlvara.Limpar();
            this.DataValidadeAlvara.Limpar();
            this.DataEmissaoAvcb.Limpar();
            this.DataValidadeAvcb.Limpar();
            this.Lotacao.Limpar();
            this.Cancelada.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EventoID.Desfazer();
            this.Horario.Desfazer();
            this.DisponivelVenda.Desfazer();
            this.DisponivelAjuste.Desfazer();
            this.DisponivelRelatorio.Desfazer();
            this.VersaoImagemIngresso.Desfazer();
            this.VersaoImagemVale.Desfazer();
            this.VersaoImagemVale2.Desfazer();
            this.VersaoImagemVale3.Desfazer();
            this.Impressao.Desfazer();
            this.LocalImagemMapaID.Desfazer();
            this.DescricaoPadrao.Desfazer();
            this.Descricao.Desfazer();
            this.UltimoCodigoImpresso.Desfazer();
            this.CotaID.Desfazer();
            this.Quantidade.Desfazer();
            this.QuantidadePorCliente.Desfazer();
            this.MapaEsquematicoID.Desfazer();
            this.Programacao.Desfazer();
            this.CodigoProgramacao.Desfazer();
            this.Sincronizado.Desfazer();
            this.Alvara.Desfazer();
            this.AVCB.Desfazer();
            this.DataEmissaoAlvara.Desfazer();
            this.DataValidadeAlvara.Desfazer();
            this.DataEmissaoAvcb.Desfazer();
            this.DataValidadeAvcb.Desfazer();
            this.Lotacao.Desfazer();
            this.Cancelada.Desfazer();
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

        public class horario : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "Horario";
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

        public class disponivelvenda : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "DisponivelVenda";
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

        public class disponivelajuste : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "DisponivelAjuste";
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

        public class disponivelrelatorio : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "DisponivelRelatorio";
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

        public class versaoimagemingresso : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemIngresso";
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

        public class versaoimagemvale : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemVale";
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

        public class versaoimagemvale2 : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemVale2";
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

        public class versaoimagemvale3 : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemVale3";
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

        public class impressao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Impressao";
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

        public class localimagemmapaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalImagemMapaID";
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

        public class descricaopadrao : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "DescricaoPadrao";
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

        public class descricao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Descricao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
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

        public class ultimocodigoimpresso : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UltimoCodigoImpresso";
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

        public class cotaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CotaID";
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

        public class quantidade : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Quantidade";
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

        public class quantidadeporcliente : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadePorCliente";
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

        public class mapaesquematicoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MapaEsquematicoID";
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

        public class programacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Programacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 2000;
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

        public class codigoprogramacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoProgramacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 100;
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

        public class sincronizado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Sincronizado";
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

        public class alvara : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Alvara";
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

        public class avcb : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "AVCB";
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

        public class dataemissaoalvara : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataEmissaoAlvara";
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

        public class datavalidadealvara : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataValidadeAlvara";
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

        public class dataemissaoavcb : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataEmissaoAvcb";
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

        public class datavalidadeavcb : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataValidadeAvcb";
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

        public class lotacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Lotacao";
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


        public class cancelada : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Cancelada";
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

                DataTable tabela = new DataTable("Apresentacao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("Horario", typeof(DateTime));
                tabela.Columns.Add("DisponivelVenda", typeof(bool));
                tabela.Columns.Add("DisponivelAjuste", typeof(bool));
                tabela.Columns.Add("DisponivelRelatorio", typeof(bool));
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));
                tabela.Columns.Add("Impressao", typeof(string));
                tabela.Columns.Add("LocalImagemMapaID", typeof(int));
                tabela.Columns.Add("DescricaoPadrao", typeof(bool));
                tabela.Columns.Add("Descricao", typeof(string));
                tabela.Columns.Add("UltimoCodigoImpresso", typeof(int));
                tabela.Columns.Add("CotaID", typeof(int));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                tabela.Columns.Add("MapaEsquematicoID", typeof(int));
                tabela.Columns.Add("Programacao", typeof(string));
                tabela.Columns.Add("CodigoProgramacao", typeof(string));
                tabela.Columns.Add("Sincronizado", typeof(string));
                tabela.Columns.Add("Alvara", typeof(string));
                tabela.Columns.Add("AVCB", typeof(string));
                tabela.Columns.Add("DataEmissaoAlvara", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAlvara", typeof(DateTime));
                tabela.Columns.Add("DataEmissaoAvcb", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAvcb", typeof(DateTime));
                tabela.Columns.Add("Lotacao", typeof(int));
                tabela.Columns.Add("Cancelada", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract int[] Novo(DataSet info);

        public abstract DataTable Ingressos(int canalid, string status);

        public abstract int QuantidadeDisponivel(int setorid);

        public abstract DataTable Caixas();

        public abstract DataTable Fechamento();

        public abstract DataTable Fechamento(int caixaid);

        public abstract DataTable Setores();

        public abstract DataTable VendaProduto();

        public abstract DataTable VendaProduto(int caixaid);

        public abstract bool ExcluirCascata();

        public abstract DataTable PorcentagemIngressosStatus(string apresentacoes);

        public abstract DataTable QuantidadeIngressosStatus(string apresentacoes);

        public abstract int TotalIngressos(string apresentacoes);

        public abstract DataTable VendasGerenciais(string datainicial, string datafinal, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, bool vendascanal, string tipolinha, bool disponivel, bool empresavendeingressos, bool empresapromoveeventos);

        public abstract DataTable LinhasVendasGerenciais(string ingressologids);

        public abstract int QuantidadeIngressosPorApresentacao(string ingressologids);

        public abstract decimal ValorIngressosPorApresentacao(string ingressologids);

    }
    #endregion

    #region "ApresentacaoLista_B"

    public abstract class ApresentacaoLista_B : BaseLista
    {

        private bool backup = false;
        protected Apresentacao apresentacao;

        // passar o Usuario logado no sistema
        public ApresentacaoLista_B()
        {
            apresentacao = new Apresentacao();
        }

        // passar o Usuario logado no sistema
        public ApresentacaoLista_B(int usuarioIDLogado)
        {
            apresentacao = new Apresentacao(usuarioIDLogado);
        }

        public Apresentacao Apresentacao
        {
            get { return apresentacao; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Apresentacao especifico
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
                    apresentacao.Ler(id);
                    return apresentacao;
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
                    sql = "SELECT ID FROM tApresentacao";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tApresentacao";

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
                    sql = "SELECT ID FROM tApresentacao";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tApresentacao";

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
        /// Carrega a lista pela tabela x (de backup)
        /// </summary>
        public void CarregarBackup()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM xApresentacao";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xApresentacao";

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

                backup = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche Apresentacao corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    apresentacao.Ler(id);
                else
                    apresentacao.LerBackup(id);

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

                bool ok = apresentacao.Excluir();
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

                    Ultimo();
                    //fazer varredura de traz pra frente.
                    do
                        ok = Excluir();
                    while (ok && Anterior());

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
        /// Inseri novo(a) Apresentacao na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = apresentacao.Inserir();
                if (ok)
                {
                    lista.Add(apresentacao.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Apresentacao carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Apresentacao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("Horario", typeof(DateTime));
                tabela.Columns.Add("DisponivelVenda", typeof(bool));
                tabela.Columns.Add("DisponivelAjuste", typeof(bool));
                tabela.Columns.Add("DisponivelRelatorio", typeof(bool));
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));
                tabela.Columns.Add("Impressao", typeof(string));
                tabela.Columns.Add("LocalImagemMapaID", typeof(int));
                tabela.Columns.Add("DescricaoPadrao", typeof(bool));
                tabela.Columns.Add("Descricao", typeof(string));
                tabela.Columns.Add("UltimoCodigoImpresso", typeof(int));
                tabela.Columns.Add("CotaID", typeof(int));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                tabela.Columns.Add("MapaEsquematicoID", typeof(int));
                tabela.Columns.Add("Programacao", typeof(string));
                tabela.Columns.Add("CodigoProgramacao", typeof(string));
                tabela.Columns.Add("Sincronizado", typeof(string));
                tabela.Columns.Add("Alvara", typeof(string));
                tabela.Columns.Add("AVCB", typeof(string));
                tabela.Columns.Add("DataEmissaoAlvara", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAlvara", typeof(DateTime));
                tabela.Columns.Add("DataEmissaoAvcb", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAvcb", typeof(DateTime));
                tabela.Columns.Add("Lotacao", typeof(int));
                tabela.Columns.Add("Cancelada", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = apresentacao.Control.ID;
                        linha["EventoID"] = apresentacao.EventoID.Valor;
                        linha["Horario"] = apresentacao.Horario.Valor;
                        linha["DisponivelVenda"] = apresentacao.DisponivelVenda.Valor;
                        linha["DisponivelAjuste"] = apresentacao.DisponivelAjuste.Valor;
                        linha["DisponivelRelatorio"] = apresentacao.DisponivelRelatorio.Valor;
                        linha["VersaoImagemIngresso"] = apresentacao.VersaoImagemIngresso.Valor;
                        linha["VersaoImagemVale"] = apresentacao.VersaoImagemVale.Valor;
                        linha["VersaoImagemVale2"] = apresentacao.VersaoImagemVale2.Valor;
                        linha["VersaoImagemVale3"] = apresentacao.VersaoImagemVale3.Valor;
                        linha["Impressao"] = apresentacao.Impressao.Valor;
                        linha["LocalImagemMapaID"] = apresentacao.LocalImagemMapaID.Valor;
                        linha["DescricaoPadrao"] = apresentacao.DescricaoPadrao.Valor;
                        linha["Descricao"] = apresentacao.Descricao.Valor;
                        linha["UltimoCodigoImpresso"] = apresentacao.UltimoCodigoImpresso.Valor;
                        linha["CotaID"] = apresentacao.CotaID.Valor;
                        linha["Quantidade"] = apresentacao.Quantidade.Valor;
                        linha["QuantidadePorCliente"] = apresentacao.QuantidadePorCliente.Valor;
                        linha["MapaEsquematicoID"] = apresentacao.MapaEsquematicoID.Valor;
                        linha["Programacao"] = apresentacao.Programacao.Valor;
                        linha["CodigoProgramacao"] = apresentacao.CodigoProgramacao.Valor;
                        linha["Sincronizado"] = apresentacao.Sincronizado.Valor;
                        linha["Alvara"] = apresentacao.Alvara.Valor;
                        linha["AVCB"] = apresentacao.AVCB.Valor;
                        linha["DataEmissaoAlvara"] = apresentacao.DataEmissaoAlvara.Valor;
                        linha["DataValidadeAlvara"] = apresentacao.DataValidadeAlvara.Valor;
                        linha["DataEmissaoAvcb"] = apresentacao.DataEmissaoAvcb.Valor;
                        linha["DataValidadeAvcb"] = apresentacao.DataValidadeAvcb.Valor;
                        linha["Lotacao"] = apresentacao.Lotacao.Valor;
                        linha["Cancelada"] = apresentacao.Cancelada.Valor;
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

                DataTable tabela = new DataTable("RelatorioApresentacao");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EventoID", typeof(int));
                    tabela.Columns.Add("Horario", typeof(DateTime));
                    tabela.Columns.Add("DisponivelVenda", typeof(bool));
                    tabela.Columns.Add("DisponivelAjuste", typeof(bool));
                    tabela.Columns.Add("DisponivelRelatorio", typeof(bool));
                    tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                    tabela.Columns.Add("VersaoImagemVale", typeof(int));
                    tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                    tabela.Columns.Add("VersaoImagemVale3", typeof(int));
                    tabela.Columns.Add("Impressao", typeof(string));
                    tabela.Columns.Add("LocalImagemMapaID", typeof(int));
                    tabela.Columns.Add("DescricaoPadrao", typeof(bool));
                    tabela.Columns.Add("Descricao", typeof(string));
                    tabela.Columns.Add("UltimoCodigoImpresso", typeof(int));
                    tabela.Columns.Add("CotaID", typeof(int));
                    tabela.Columns.Add("Quantidade", typeof(int));
                    tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                    tabela.Columns.Add("MapaEsquematicoID", typeof(int));
                    tabela.Columns.Add("Programacao", typeof(string));
                    tabela.Columns.Add("CodigoProgramacao", typeof(string));
                    tabela.Columns.Add("Sincronizado", typeof(string));
                    tabela.Columns.Add("Alvara", typeof(string));
                    tabela.Columns.Add("AVCB", typeof(string));
                    tabela.Columns.Add("DataEmissaoAlvara", typeof(DateTime));
                    tabela.Columns.Add("DataValidadeAlvara", typeof(DateTime));
                    tabela.Columns.Add("DataEmissaoAvcb", typeof(DateTime));
                    tabela.Columns.Add("DataValidadeAvcb", typeof(DateTime));
                    tabela.Columns.Add("Lotacao", typeof(int));
                    tabela.Columns.Add("Cancelada", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EventoID"] = apresentacao.EventoID.Valor;
                        linha["Horario"] = apresentacao.Horario.Valor;
                        linha["DisponivelVenda"] = apresentacao.DisponivelVenda.Valor;
                        linha["DisponivelAjuste"] = apresentacao.DisponivelAjuste.Valor;
                        linha["DisponivelRelatorio"] = apresentacao.DisponivelRelatorio.Valor;
                        linha["VersaoImagemIngresso"] = apresentacao.VersaoImagemIngresso.Valor;
                        linha["VersaoImagemVale"] = apresentacao.VersaoImagemVale.Valor;
                        linha["VersaoImagemVale2"] = apresentacao.VersaoImagemVale2.Valor;
                        linha["VersaoImagemVale3"] = apresentacao.VersaoImagemVale3.Valor;
                        linha["Impressao"] = apresentacao.Impressao.Valor;
                        linha["LocalImagemMapaID"] = apresentacao.LocalImagemMapaID.Valor;
                        linha["DescricaoPadrao"] = apresentacao.DescricaoPadrao.Valor;
                        linha["Descricao"] = apresentacao.Descricao.Valor;
                        linha["UltimoCodigoImpresso"] = apresentacao.UltimoCodigoImpresso.Valor;
                        linha["CotaID"] = apresentacao.CotaID.Valor;
                        linha["Quantidade"] = apresentacao.Quantidade.Valor;
                        linha["QuantidadePorCliente"] = apresentacao.QuantidadePorCliente.Valor;
                        linha["MapaEsquematicoID"] = apresentacao.MapaEsquematicoID.Valor;
                        linha["Programacao"] = apresentacao.Programacao.Valor;
                        linha["CodigoProgramacao"] = apresentacao.CodigoProgramacao.Valor;
                        linha["Sincronizado"] = apresentacao.Sincronizado.Valor;
                        linha["Alvara"] = apresentacao.Alvara.Valor;
                        linha["AVCB"] = apresentacao.AVCB.Valor;
                        linha["DataEmissaoAlvara"] = apresentacao.DataEmissaoAlvara.Valor;
                        linha["DataValidadeAlvara"] = apresentacao.DataValidadeAlvara.Valor;
                        linha["DataEmissaoAvcb"] = apresentacao.DataEmissaoAvcb.Valor;
                        linha["DataValidadeAvcb"] = apresentacao.DataValidadeAvcb.Valor;
                        linha["Lotacao"] = apresentacao.Lotacao.Valor;
                        linha["Cancelada"] = apresentacao.Cancelada.Valor;
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
                    case "EventoID":
                        sql = "SELECT ID, EventoID FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;
                    case "Horario":
                        sql = "SELECT ID, Horario FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY Horario";
                        break;
                    case "DisponivelVenda":
                        sql = "SELECT ID, DisponivelVenda FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY DisponivelVenda";
                        break;
                    case "DisponivelAjuste":
                        sql = "SELECT ID, DisponivelAjuste FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY DisponivelAjuste";
                        break;
                    case "DisponivelRelatorio":
                        sql = "SELECT ID, DisponivelRelatorio FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY DisponivelRelatorio";
                        break;
                    case "VersaoImagemIngresso":
                        sql = "SELECT ID, VersaoImagemIngresso FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY VersaoImagemIngresso";
                        break;
                    case "VersaoImagemVale":
                        sql = "SELECT ID, VersaoImagemVale FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY VersaoImagemVale";
                        break;
                    case "VersaoImagemVale2":
                        sql = "SELECT ID, VersaoImagemVale2 FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY VersaoImagemVale2";
                        break;
                    case "VersaoImagemVale3":
                        sql = "SELECT ID, VersaoImagemVale3 FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY VersaoImagemVale3";
                        break;
                    case "Impressao":
                        sql = "SELECT ID, Impressao FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY Impressao";
                        break;
                    case "LocalImagemMapaID":
                        sql = "SELECT ID, LocalImagemMapaID FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY LocalImagemMapaID";
                        break;
                    case "DescricaoPadrao":
                        sql = "SELECT ID, DescricaoPadrao FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY DescricaoPadrao";
                        break;
                    case "Descricao":
                        sql = "SELECT ID, Descricao FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY Descricao";
                        break;
                    case "UltimoCodigoImpresso":
                        sql = "SELECT ID, UltimoCodigoImpresso FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY UltimoCodigoImpresso";
                        break;
                    case "CotaID":
                        sql = "SELECT ID, CotaID FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY CotaID";
                        break;
                    case "Quantidade":
                        sql = "SELECT ID, Quantidade FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY Quantidade";
                        break;
                    case "QuantidadePorCliente":
                        sql = "SELECT ID, QuantidadePorCliente FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY QuantidadePorCliente";
                        break;
                    case "MapaEsquematicoID":
                        sql = "SELECT ID, MapaEsquematicoID FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY MapaEsquematicoID";
                        break;
                    case "Programacao":
                        sql = "SELECT ID, Programacao FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY Programacao";
                        break;
                    case "CodigoProgramacao":
                        sql = "SELECT ID, CodigoProgramacao FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY CodigoProgramacao";
                        break;
                    case "Sincronizado":
                        sql = "SELECT ID, Sincronizado FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY Sincronizado";
                        break;
                    case "Alvara":
                        sql = "SELECT ID, Alvara FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY Alvara";
                        break;
                    case "AVCB":
                        sql = "SELECT ID, AVCB FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY AVCB";
                        break;
                    case "DataEmissaoAlvara":
                        sql = "SELECT ID, DataEmissaoAlvara FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY DataEmissaoAlvara";
                        break;
                    case "DataValidadeAlvara":
                        sql = "SELECT ID, DataValidadeAlvara FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY DataValidadeAlvara";
                        break;
                    case "DataEmissaoAvcb":
                        sql = "SELECT ID, DataEmissaoAvcb FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY DataEmissaoAvcb";
                        break;
                    case "DataValidadeAvcb":
                        sql = "SELECT ID, DataValidadeAvcb FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY DataValidadeAvcb";
                        break;
                    case "Lotacao":
                        sql = "SELECT ID, Lotacao FROM tApresentacao WHERE " + FiltroSQL + " ORDER BY Lotacao";
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

    #region "ApresentacaoException"

    [Serializable]
    public class ApresentacaoException : Exception
    {

        public ApresentacaoException() : base() { }

        public ApresentacaoException(string msg) : base(msg) { }

        public ApresentacaoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}