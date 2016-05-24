/**************************************************
* Arquivo: EventoEntregaControle.cs
* Gerado: 14/01/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace IRLib
{
    public class EventoEntregaControle : EventoEntregaControle_B
    {
        public EventoEntregaControle() { }

        public EventoEntregaControle(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public EstruturaEventoEntregaControle Carregar(int eventoID, int controleID)
        {
            try
            {
                EstruturaEventoEntregaControle eEECAux = new EstruturaEventoEntregaControle();
                string sql = @"SELECT  tEventoEntregaControle.ID, tEventoEntregaControle.EventoID, tEventoEntregaControle.EntregaControleID, tEventoEntregaControle.DiasTriagem,
                CASE WHEN LEN(tEventoEntregaControle.ProcedimentoEntrega) > 0
	                THEN tEventoEntregaControle.ProcedimentoEntrega 
	                ELSE
		                CASE WHEN LEN(tEntregaControle.ProcedimentoEntrega) > 0
			                THEN tEntregaControle.ProcedimentoEntrega
			                ELSE tEntrega.ProcedimentoEntrega
		                END
	                END AS ProcedimentoEntrega
                FROM tEventoEntregaControle 
                INNER JOIN tEntregaControle ON tEntregaControle.ID = tEventoEntregaControle.EntregaControleID
                INNER JOIN tEntrega ON tEntrega.ID = tEntregaControle.EntregaID WHERE EventoID = " + eventoID + " and EntregaControleID = " + controleID;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    eEECAux.ID = bd.LerInt("ID");
                    eEECAux.EventoID = bd.LerInt("EventoID");
                    eEECAux.EntregaControleID = bd.LerInt("EntregaControleID");
                    eEECAux.ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega");
                    eEECAux.DiasTriagem = bd.LerInt("DiasTriagem");
                }

                bd.Fechar();

                return eEECAux;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EstruturaEventoEntregaControle> Carregar(List<int> eventoIDs, int controleID)
        {
            try
            {
                List<EstruturaEventoEntregaControle> retorno = new List<EstruturaEventoEntregaControle>();
                EstruturaEventoEntregaControle eEECAux = new EstruturaEventoEntregaControle();
                string sql = @"SELECT  tEventoEntregaControle.ID, tEventoEntregaControle.EventoID, tEventoEntregaControle.EntregaControleID, tEventoEntregaControle.DiasTriagem,
                CASE WHEN LEN(tEventoEntregaControle.ProcedimentoEntrega) > 0
	                THEN tEventoEntregaControle.ProcedimentoEntrega 
	                ELSE
		                CASE WHEN LEN(tEntregaControle.ProcedimentoEntrega) > 0
			                THEN tEntregaControle.ProcedimentoEntrega
			                ELSE tEntrega.ProcedimentoEntrega
		                END
	                END AS ProcedimentoEntrega
                FROM tEventoEntregaControle 
                INNER JOIN tEntregaControle ON tEntregaControle.ID = tEventoEntregaControle.EntregaControleID
                INNER JOIN tEntrega ON tEntrega.ID = tEntregaControle.EntregaID WHERE EventoID IN (" + Utilitario.ArrayToString(eventoIDs.ToArray()) + ") and EntregaControleID = " + controleID;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    eEECAux.ID = bd.LerInt("ID");
                    eEECAux.EventoID = bd.LerInt("EventoID");
                    eEECAux.EntregaControleID = bd.LerInt("EntregaControleID");
                    eEECAux.ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega");
                    eEECAux.DiasTriagem = bd.LerInt("DiasTriagem");

                    retorno.Add(eEECAux);
                }

                bd.Fechar();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtribuirEstrutura(EstruturaEventoEntregaControle estEC)
        {
            this.Control.ID = estEC.ID;
            this.EventoID.Valor = estEC.EventoID;
            this.EntregaControleID.Valor = estEC.EntregaControleID;
            this.ProcedimentoEntrega.Valor = estEC.ProcedimentoEntrega;
            this.DiasTriagem.Valor = estEC.DiasTriagem;
        }

        public void Distribuir(int eventoID, int EntregaID)
        {
            try
            {
                List<EstruturaEventoEntregaControle> listaEEECAux = new List<EstruturaEventoEntregaControle>();

                string procedimento = this.ProcedimentoEntrega.Valor;
                int dias = this.DiasTriagem.Valor;

                string sql = string.Empty;

                sql = @"SELECT tEventoEntregaControle.* FROM tEventoEntregaControle 
                            LEFT JOIN tEntregaControle ON tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
                            LEFT JOIN tEntrega ON tEntrega.ID = tEntregaControle.EntregaID 
                            WHERE EventoID = " + eventoID + " AND tEntregaControle.EntregaID = " + EntregaID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    listaEEECAux.Add(new EstruturaEventoEntregaControle
                    {
                        ID = bd.LerInt("ID"),
                        EventoID = bd.LerInt("EventoID"),
                        EntregaControleID = bd.LerInt("EntregaControleID"),
                        ProcedimentoEntrega = procedimento,
                        DiasTriagem = dias
                    });
                };

                bd.Fechar();


                foreach (EstruturaEventoEntregaControle estEC in listaEEECAux)
                {
                    AtribuirEstrutura(estEC);
                    Atualizar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Inserir(BD bd)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEventoEntregaControle(EventoID, EntregaControleID,ProcedimentoEntrega,DiasTriagem) ");
                sql.Append("VALUES (@002,@003,'',0) SELECT SCOPE_IDENTITY(); ");


                sql.Replace("@002", this.EventoID.ValorBD);
                sql.Replace("@003", this.EntregaControleID.ValorBD);

                int id = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

                bool result = (id >= 1);

                if (result)
                {
                    this.Control.ID = id;
                    InserirControle("I", bd);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void InserirControle(string acao , BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cEventoEntregaControle (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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


        public override bool Inserir()
        {
            try
            {
                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();

                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEventoEntregaControle(EventoID, EntregaControleID, ProcedimentoEntrega, DiasTriagem) ");
                sql.Append("VALUES (@001,@002,'@003',@004); SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.EntregaControleID.ValorBD);
                sql.Replace("@003", this.ProcedimentoEntrega.ValorBD);
                sql.Replace("@004", this.DiasTriagem.ValorBD);

                int id = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

                bool result = (id >= 1);

                if (result)
                {
                    this.Control.ID = id;
                    InserirControle("I");
                }
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

        public static void InsereEntregaPadrao(BD bd, int eventoID)
        {
            
            bd.ExecutarComParametros(@"
                                INSERT INTO tEventoEntregaControle(EventoID, EntregaControleID,ProcedimentoEntrega,DiasTriagem)
                                SELECT @EventoID, tEntregaControle.ID, '', 0
								FROM tEntregaControle
								INNER JOIN tEntrega ON tEntregaControle.EntregaID = tEntrega.ID
								WHERE Padrao='T' ", new SqlParameter("@EventoID", eventoID));

        }


    }

    public class EventoEntregaControleLista : EventoEntregaControleLista_B
    {
        public EventoEntregaControleLista() { }

        public EventoEntregaControleLista(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }
}
