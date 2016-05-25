/**************************************************
* Arquivo: Sangria.cs
* Gerado: 13/02/2012
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class Sangria : Sangria_B
    {

        public Sangria() { }

        public void RegistrarSangria(BD _bd, int vendaBilheteriaID, int motivoID, int eventoID, string nome, string identificacao)
        {
            this.VendaBilheteriaID.Valor = vendaBilheteriaID;
            this.MotivoID.Valor = motivoID;
            this.EventoID.Valor = eventoID;
            this.Nome.Valor = nome;
            this.Identificacao.Valor = identificacao;
            if (!this.Inserir(_bd))            
                throw new Exception("Erro ao Gerar Sangria.");
            
        }

        public List<EstruturaSangriaImpressao> CarregarListaReimpressao(int CaixaID, int CanalID, bool supervisor, DateTime data)
        {
            try
            {

                List<EstruturaSangriaImpressao> lstSangria = new List<EstruturaSangriaImpressao>();

                string filtroSangria = " tLoja.CanalID = " + CanalID;

                if (supervisor)
                {
                    filtroSangria += " AND tCaixa.DataAbertura like '" + data.ToString("yyyyMMdd") + "%' ";
                }
                else
                {
                    filtroSangria += " AND tCaixa.ID = " + CaixaID;
                }

                string sql = @"SELECT tVendaBilheteria.ValorTotal, tVendaBilheteria.Senha,tVendaBilheteria.DataVenda,
                                tUsuario.Nome as Usuario,tSangria.Nome,tSangria.Identificacao,tMotivo.Motivo,tEvento.Nome as Evento, tEvento.ID as EventoID
                                FROM tVendaBilheteria(NOLOCK)
                                INNER JOIN tSangria(NOLOCK) ON tVendaBilheteria.ID = tSangria.VendaBilheteriaID
                                INNER JOIN tCaixa(NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                                INNER JOIN tUsuario(NOLOCK) ON tUsuario.ID = tCaixa.UsuarioID
                                INNER JOIN tLoja(NOLOCK) ON tCaixa.LojaID = tLoja.ID
                                INNER JOIN tMotivo(NOLOCK) ON tSangria.MotivoID = tMotivo.ID
                                LEFT JOIN tEvento (NOLOCK) ON tSangria.EventoID = tEvento.ID
                                WHERE" + filtroSangria;


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lstSangria.Add(new EstruturaSangriaImpressao
                    {

                        Valor = bd.LerDecimal("ValorTotal"),
                        Senha = bd.LerString("Senha"),
                        Usuario = bd.LerString("Usuario"),
                        Responsavel = bd.LerString("Nome"),
                        Identificacao = bd.LerString("Identificacao"),
                        Motivo = bd.LerString("Motivo"),
                        Data = bd.LerDateTime("DataVenda"),
                        Evento = bd.LerString("Evento"),
                        EventoID = bd.LerInt("EventoID")
                    });
                }

                return lstSangria;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaSangriaImpressao BuscarSangrias(string Senha)
        {
            try
            { 
                EstruturaSangriaImpressao retorno = new EstruturaSangriaImpressao();


                string sql = @"SELECT tVendaBilheteria.ValorTotal, tVendaBilheteria.Senha,tVendaBilheteria.DataVenda,
                                tUsuario.Nome as Usuario,tSangria.Nome,tSangria.Identificacao,tMotivo.Motivo
                                FROM tVendaBilheteria(NOLOCK)
                                INNER JOIN tSangria(NOLOCK) ON tVendaBilheteria.ID = tSangria.VendaBilheteriaID
                                INNER JOIN tCaixa(NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                                INNER JOIN tUsuario(NOLOCK) ON tUsuario.ID = tCaixa.UsuarioID
                                INNER JOIN tLoja(NOLOCK) ON tCaixa.LojaID = tLoja.ID
                                INNER JOIN tMotivo(NOLOCK) ON tSangria.MotivoID = tMotivo.ID
                                WHERE tVendaBilheteria.Senha = '" + Senha + "'";


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno = new EstruturaSangriaImpressao
                    {

                        Valor = bd.LerDecimal("ValorTotal"),
                        Senha = bd.LerString("Senha"),
                        Usuario = bd.LerString("Usuario"),
                        Responsavel = bd.LerString("Nome"),
                        Identificacao = bd.LerString("Identificacao"),
                        Motivo = bd.LerString("Motivo"),
                        Data = bd.LerDateTime("DataVenda"),

                    };
                }

                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaRelatoriosSangria> RelatorioEventoSangria(int EventoID, int CanalID, int CaixaID, DateTime DataInicial, DateTime DataFinal)
        {
            try
            {
                List<EstruturaRelatoriosSangria> retorno = new List<EstruturaRelatoriosSangria>();

                //EVENTOID,CANALID,CAIXAID,DATAINI,DATAFI
                string dataIni = DataInicial == DateTime.MinValue ? "0" : DataInicial.ToString("yyyyMMddHHmmss");
                string dataFim = DataFinal == DateTime.MaxValue ? "0" : DataFinal.ToString("yyyyMMddHHmmss");

                string sql = @"EXEC Rel_Sangria " + EventoID + " , " + CanalID + " , " + CaixaID + " , " + dataIni + " , " + dataFim;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaRelatoriosSangria
                    {
                        Caixa = bd.LerInt("Caixa"),
                        Valor = bd.LerDecimal("ValorSangria"),
                        Responsavel = bd.LerString("Responsavel"),
                        Identificacao = bd.LerString("Identificacao"),
                        Motivo = bd.LerString("Motivo"),
                        Data = bd.LerDateTime("Data"),
                        Canal = bd.LerString("Canal"),
                        CanalID = bd.LerInt("CanalID"),
                        Evento = bd.LerString("Evento"),
                        EventoID = bd.LerInt("EventoID"),
                        Login = bd.LerString("Login"),
                        Supervisor = bd.LerString("Supervisor")

                    });
                }

                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaRelatoriosSangria> RelatorioCanalSangria(int EventoID, int CanalID, int CaixaID, DateTime DataInicial, DateTime DataFinal)
        {
            try
            {
                List<EstruturaRelatoriosSangria> retorno = new List<EstruturaRelatoriosSangria>();

                //EVENTOID,CANALID,CAIXAID,DATAINI,DATAFI

                string dataIni = DataInicial == DateTime.MinValue ? "0" : DataInicial.ToString("yyyyMMddHHmmss");
                string dataFim = DataFinal == DateTime.MaxValue ? "0" : DataFinal.ToString("yyyyMMddHHmmss");

                string sql = @"EXEC Rel_Sangria " + EventoID + " , " + CanalID + " , " + CaixaID + " , " + dataIni + " , " + dataFim;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaRelatoriosSangria
                    {
                        Caixa = bd.LerInt("Caixa"),
                        DataAbertura = bd.LerDateTime("DataAbertura"),
                        Valor = bd.LerDecimal("ValorSangria"),
                        Responsavel = bd.LerString("Responsavel"),
                        Identificacao = bd.LerString("Identificacao"),
                        Motivo = bd.LerString("Motivo"),
                        Data = bd.LerDateTime("Data"),
                        Canal = bd.LerString("Canal"),
                        CanalID = bd.LerInt("CanalID"),
                        Evento = bd.LerString("Evento"),
                        EventoID = bd.LerInt("EventoID"),
                        Login = bd.LerString("Login"),
                        Supervisor = bd.LerString("Supervisor")

                    });
                }

                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaRelatoriosSangria> RelatorioCaixaSangria(int EventoID, int CanalID, int CaixaID, DateTime DataInicial, DateTime DataFinal)
        {
            try
            {
                List<EstruturaRelatoriosSangria> retorno = new List<EstruturaRelatoriosSangria>();

                string dataIni = DataInicial == DateTime.MinValue ? "0" : DataInicial.ToString("yyyyMMddHHmmss");
                string dataFim = DataFinal == DateTime.MaxValue ? "0" : DataFinal.ToString("yyyyMMddHHmmss");

                //EVENTOID,CANALID,CAIXAID,DATAINI,DATAFI
                string sql = @"EXEC Rel_Sangria " + EventoID + " , " + CanalID + " , " + CaixaID + " , " + dataIni + " , " + dataFim;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaRelatoriosSangria
                    {
                        Caixa = bd.LerInt("Caixa"),
                        Valor = bd.LerDecimal("ValorSangria"),
                        Responsavel = bd.LerString("Responsavel"),
                        Identificacao = bd.LerString("Identificacao"),
                        Motivo = bd.LerString("Motivo"),
                        Data = bd.LerDateTime("Data"),
                        Canal = bd.LerString("Canal"),
                        CanalID = bd.LerInt("CanalID"),
                        Evento = bd.LerString("Evento"),
                        EventoID = bd.LerInt("EventoID"),
                        Login = bd.LerString("Login"),
                        Supervisor = bd.LerString("Supervisor")
                    });
                }

                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaIDNome> BuscaEventos(bool todos)
        {
            try
            {
                string sql = @"SELECT DISTINCT tSangria.EventoID as ID, tEvento.Nome as Nome 
                                FROM tSangria (NOLOCK)
                                INNER JOIN tEvento (NOLOCK) on tSangria.EventoID = tEvento.ID";

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existe eventos com Sangria");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                if (todos)
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = 0,
                        Nome = "Todos",
                    });

                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());


                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaIDNome> BuscaCanal(bool todos)
        {
            try
            {
                string sql = @"SELECT DISTINCT tCanal.ID , tCanal.Nome 
                                FROM tSangria (NOLOCK)
                                INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tSangria.VendaBilheteriaID
                                INNER JOIN tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID
                                INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID
                                INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID";

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existe canais com Sangria");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                if (todos)
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = 0,
                        Nome = "Todos",
                    });

                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());


                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaIDNome> BuscaCaixa(bool todos)
        {
            try
            {
                string sql = @"SELECT DISTINCT
                                tCaixa.ID,tCaixa.ID as Nome
                                FROM tSangria (NOLOCK)
                                INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tSangria.VendaBilheteriaID
                                INNER JOIN tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID";

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existe caixas com Sangria");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                if (todos)
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = 0,
                        Nome = "Todos",
                    });
                }

                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());


                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }


    }

    public class SangriaLista : SangriaLista_B
    {

        public SangriaLista() { }

    }

}
