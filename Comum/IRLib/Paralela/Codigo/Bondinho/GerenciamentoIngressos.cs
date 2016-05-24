/**************************************************
* Arquivo: GerenciamentoIngressos.cs
* Gerado: 27/08/2012
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class GerenciamentoIngressos : GerenciamentoIngressos_B
    {
        public const int PRECOHORAMARCADA = 6;
        public const int PRECOFREEPASS = 7;
        public const int PRECONORMAL = 8;

        public GerenciamentoIngressos() { }

        public GerenciamentoIngressos(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaGerenciamentoIngressos> buscarIngressos(int ApresentacaoSetorID)
        {
            try
            {

                List<EstruturaGerenciamentoIngressos> lista = new List<EstruturaGerenciamentoIngressos>();

                string sql = string.Format(@"Exec BuscaGerenciamentoIngressos  {0} ", ApresentacaoSetorID);

                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    string data = bd.LerString("Horario");
                    lista.Add(new EstruturaGerenciamentoIngressos
                    {
                        GerenciamentoIngressosID = bd.LerInt("ID"),
                        Evento = bd.LerString("Evento"),
                        Data = bd.LerString("Data"),
                        TipoPreco = bd.LerString("PrecoTipo"),
                        PrecoTipoID = bd.LerInt("PrecoTipoID"),
                        ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID"),
                        Horario = data.Length > 0 ? data.Substring(0, 2) + ":" + data.Substring(2, 2) : "",
                        Label = bd.LerString("Label"),
                        Vendido = bd.LerInt("Vendido"),
                        Disponivel = bd.LerInt("Disponivel")
                    });

                }

                return lista;

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

        public void CarregarEstrutura(EstruturaGerenciamentoIngressos eGI)
        {
            this.Control.ID = eGI.GerenciamentoIngressosID;
            this.ApresentacaoSetorID.Valor = eGI.ApresentacaoSetorID;
            this.PrecoTipoID.Valor = eGI.PrecoTipoID;
            this.Label.Valor = eGI.Label;
            this.Horario.Valor = eGI.Horario;
            this.Disponivel.Valor = eGI.Disponivel;
        }

        public List<EstruturaIDNome> CarregarPrecos(int apresentacaoSetorID)
        {
            try
            {
                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                string sql = string.Format(@"select distinct tp.PrecoTipoID as ID, tpt.Nome
                            from tPreco tp(nolock)
                            inner join tPrecoTipo tpt(nolock) on tp.PrecoTipoID=tpt.ID
                            where ApresentacaoSetorID = {0} ", apresentacaoSetorID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaIDNome
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome")
                    });
                }

                return lista;

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

        public bool PossuiHorario(string horario, int gerenciamentoIngressoID, int apresentacaoSetorID)
        {

            try
            {

                bool retorno = false;

                string sql = @" select * 
                    from tGerenciamentoIngressos  gi(nolock) 
                    where Horario = '" + horario + "' and ApresentacaoSetorID = " + apresentacaoSetorID;

                if (gerenciamentoIngressoID > 0)
                    sql += "AND ID != " + gerenciamentoIngressoID;

                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    retorno = true;
                }

                return retorno;

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

        public string CarregarData(int apresentacaoSetorID)
        {
            try
            {

                string Data = "";

                string sql = string.Format(@"select  SUBSTRING(ap.Horario,7,2) +'/'+ SUBSTRING(ap.Horario,5,2) +'/'+ SUBSTRING(ap.Horario,1,4) as Data
                                    from  tApresentacaoSetor aps(nolock) 
                                    inner join tApresentacao ap(nolock) on aps.ApresentacaoID=ap.ID
                                    where aps.ID =  {0} ", apresentacaoSetorID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    Data = bd.LerString("Data");

                }

                return Data;

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

        public string BuscaHorario(int gerenciamentoIngressosID)
        {
            try
            {

                string Horario = "";

                string sql = string.Format(@"select Horario from tGerenciamentoIngressos where ID =  {0} ", gerenciamentoIngressosID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    Horario = bd.LerString("Horario");

                }

                return Horario;

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
    }

    public class GerenciamentoIngressosLista : GerenciamentoIngressosLista_B
    {

        public GerenciamentoIngressosLista() { }

        public GerenciamentoIngressosLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
