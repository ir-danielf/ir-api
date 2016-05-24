/**************************************************
* Arquivo: AssinaturaAno.cs
* Gerado: 09/09/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class AssinaturaConfig : AssinaturaConfig_B
    {

        public AssinaturaConfig() { }

        public AssinaturaConfig(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public string GetNome(int id)
        {
            try
            {
                return bd.ConsultaValor("Select Nome FROM tAssinaturaConfig(NOLOCK) Where ID = " + id).ToString();
            }
            catch 
            {
                return string.Empty;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int GetAssinaturaTipoID(int id)
        {
            try
            {
                return Convert.ToInt32(bd.ConsultaValor("Select AssinaturaTipoID FROM tAssinaturaConfig(NOLOCK) Where ID = " + id));
            }
            catch
            {
                return 0;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int GetAnoAtivoAssinatura(int id)
        {
            try
            {
                return Convert.ToInt32(bd.ConsultaValor("Select AnoAtivoAssinatura FROM tAssinaturaConfig(NOLOCK) Where ID = " + id));
            }
            catch
            {
                return 0;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int GetAnoAtivoBancoIngressos(int id)
        {
            try
            {
                return Convert.ToInt32(bd.ConsultaValor("Select AnoAtivoBancoIngressos FROM tAssinaturaConfig(NOLOCK) Where ID = " + id));
            }
            catch
            {
                return 0;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int GetAnoAtivo(int id)
        {
            try
            {
                return Convert.ToInt32(bd.ConsultaValor("Select AnoAtivo FROM tAssinaturaConfig(NOLOCK) Where ID = " + id));
            }
            catch
            {
                return 0;
            }
            finally
            {
                bd.Fechar();
            }
        }


        public int GetAssinaturaTipoBancoIngresso(int id)
        {
            try
            {
                return Convert.ToInt32(bd.ConsultaValor("Select AssinaturaTipoIDBancoIngresso FROM tAssinaturaConfig(NOLOCK) Where ID = " + id));
            }
            catch
            {
                return 0;
            }
            finally
            {
                bd.Fechar();
            }
        }
        
    }

    

}
