using System;
using System.Data;
using CTLib;
using System.Linq;
using System.Collections.Generic;

namespace IRLib
{

    public class CorreioCodigoRastreio : CorreioCodigoRastreio_B
    {

        public CorreioCodigoRastreio() { }

        public CorreioCodigoRastreio(int usuarioIDLogado) : base(usuarioIDLogado) { }
        
        public int VerificaQntdCodigoRastreioDisponivel(string pCodServicoCorreio)
        {

            string sql = string.Format(@"
                                SELECT COUNT (ID) 
                                FROM tCorreioCodigoRastreio
                                WHERE ISNULL(VendaBilheteriaEntregaID,0) = 0 AND ISNULL(VendaBilheteriaID,0) = 0 
                                    AND CodigoServico = {0}", pCodServicoCorreio);

            object qntdeCodigoDisponivel = bd.ConsultaValor(sql);
            bd.FecharConsulta();

            return qntdeCodigoDisponivel == null ? 0 : Convert.ToInt32(qntdeCodigoDisponivel);

        }

            
    }

    public class CorreioCodigoRastreioLista : CorreioCodigoRastreioLista_B
    {

        public CorreioCodigoRastreioLista() { }

        public CorreioCodigoRastreioLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}