using System;
using System.Data;
using CTLib;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace IRLib.Paralela
{

    public class EventoTipoPagamento : EventoTipoPagamento_B
    {

        public EventoTipoPagamento() { }

        public EventoTipoPagamento(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EventoTipoPagamento> Listar(int EventoID)
        {
            List<EventoTipoPagamento> listaEventoTipoPagamento = new List<EventoTipoPagamento>();


            try
            {
                string sql = string.Format(@"SELECT * FROM tEventoTipoPagamento WHERE EventoID = {0}", EventoID);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {

                    EventoTipoPagamento oEventoTipoPagamento = new EventoTipoPagamento();

                    oEventoTipoPagamento.Control.ID = bd.LerInt("ID");
                    oEventoTipoPagamento.EventoID.Valor = bd.LerInt("EventoID");
                    oEventoTipoPagamento.Dias.Valor = bd.LerInt("Dias");
                    oEventoTipoPagamento.FormaPagamentoID.Valor = bd.LerInt("FormaPagamentoID");

                    listaEventoTipoPagamento.Add(oEventoTipoPagamento);

                }


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return listaEventoTipoPagamento;
        }


        public bool Alterar()
        {
            try
            {
                
                bd.IniciarTransacao();

                string sqlControlID = string.Format(@"
                                                    SELECT ID
                                                    FROM tEventoTipoPagamento                
                                                    WHERE EventoID = {0} AND FormaPagamentoID = {1} ", this.EventoID.Valor, this.FormaPagamentoID.Valor);

                object objID = bd.ConsultaValor(sqlControlID);


                // Se a consulta retornar NULL, o objeto nao foi inserido ainda no banco.
                // Então precisa Inserir
                if (objID == null)
                    return false;



                string sqlVersion = "SELECT MAX(Versao) FROM cEventoTipoPagamento WHERE ID=" + objID;

                object obj = bd.ConsultaValor(sqlVersion);


                int versao = Convert.ToInt32(obj);
                this.Control.Versao = versao;
                this.Control.ID = Convert.ToInt32(objID);

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tEventoTipoPagamento SET EventoID = @001, FormaPagamentoID = @002, Dias = @003 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", objID.ToString());

                sql.Replace("@001", this.EventoID.ValorBD);

                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                sql.Replace("@003", this.Dias.ValorBD);


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



    }

    public class EventoTipoPagamentoLista : EventoTipoPagamentoLista_B
    {

        public EventoTipoPagamentoLista() { }

        public EventoTipoPagamentoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}