using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class ParcelamentoIR : ParcelamentoIR_B
    {

        public ParcelamentoIR() { }

        public ParcelamentoIR(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaParcelamentoIR> BuscaParcelamento()
        {
            try
            {
                List<EstruturaParcelamentoIR> retorno = new List<EstruturaParcelamentoIR>();

                string sql = "SELECT ID, Parcelas, Coeficiente  FROM tParcelamento(nolock)";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaParcelamentoIR
                    {
                        ID = bd.LerInt("ID"),
                        Parcelas = bd.LerInt("Parcelas"),
                        Coeficiente = bd.LerDecimal("Coeficiente")
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

        public static decimal BuscaCoeficiente(int Parcela)
        {
            BD bd = new BD();

            try
            {
                decimal Coeficiente = 0;

                string sql = "SELECT Coeficiente FROM tParcelamento(nolock) WHERE Parcelas = " + Parcela;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    Coeficiente = bd.LerDecimal("Coeficiente");

                return Coeficiente;
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
    }

    public class ParcelamentoIRLista : ParcelamentoIRLista_B
    {
        public ParcelamentoIRLista() { }

        public ParcelamentoIRLista(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }
}