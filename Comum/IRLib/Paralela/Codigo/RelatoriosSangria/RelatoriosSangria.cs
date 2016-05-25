using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{
    public class RelatoriosSangria
    {
        public RelatoriosSangria() { }

        public string corrigirData(string data, string hora)
        {
            string retorno = "";

            string[] dataCorreta = data.Split('/');

            Array.Reverse(dataCorreta);

            foreach (var item in dataCorreta)
            {
                retorno += item;
            }
            retorno += hora.Replace(":", "");

            return retorno;
        }

        public List<EstruturaRelatoriosSangria> RelatorioCaixaSangria(int EventoID, int CanalID, int CaixaID, DateTime DataInicial, DateTime DataFinal)
        {
            try
            {
                List<EstruturaRelatoriosSangria> retorno = new List<EstruturaRelatoriosSangria>();

                Sangria oSangria = new Sangria();

                retorno = oSangria.RelatorioCanalSangria(EventoID, CanalID, CaixaID, DataInicial, DataFinal);

                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }


        }

        public List<EstruturaRelatoriosSangria> RelatorioCanalSangria(int EventoID, int CanalID, int CaixaID, DateTime DataInicial, DateTime DataFinal)
        {
            try
            {
                List<EstruturaRelatoriosSangria> retorno = new List<EstruturaRelatoriosSangria>();


                Sangria oSangria = new Sangria();

                retorno = oSangria.RelatorioCanalSangria(EventoID, CanalID, CaixaID, DataInicial, DataFinal);



                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }


        }

        public List<EstruturaRelatoriosSangria> RelatorioEventoSangria(int EventoID, int CanalID, int CaixaID, DateTime DataInicial, DateTime DataFinal)
        {
            try
            {
                List<EstruturaRelatoriosSangria> retorno = new List<EstruturaRelatoriosSangria>();
                Sangria oSangria = new Sangria();

                retorno = oSangria.RelatorioCanalSangria(EventoID, CanalID, CaixaID, DataInicial, DataFinal);

                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }


        }
    }
}
