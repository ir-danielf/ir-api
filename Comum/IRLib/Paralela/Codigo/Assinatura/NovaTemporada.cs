using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela
{


    public partial class NovaTemporada
    {
        public class MapeamentoInfo
        {
            public int AnteriorID { get; set; }
            public int NovoID { get; set; }

            public int NovoAnoID { get; set; }
        }

        public List<MapeamentoInfo> Mapeamento { get; set; }
        public int TemporadaAnterior { get; set; }
        public int TemporadaNova { get; set; }
        public int TipoID { get; set; }





        public void GerarNovaTemporada()
        {


            var assinatura = new Assinatura(1657);

            string assinaturas = "";

            Mapeamento.ForEach(delegate(MapeamentoInfo info)
            {
                assinaturas += info.AnteriorID + ",";
            });

            assinaturas = assinaturas.Substring(0, assinaturas.Length - 1);

            string sql = @"SELECT
                        tAssinaturaCliente.*
                        FROM tAssinatura(NOLOCK)
                        INNER JOIN tAssinaturaAno (NOLOCK) ON tAssinaturaAno.AssinaturaID = tAssinatura.ID
                        INNER JOIN tASsinaturaCliente (NOLOCK) ON tAssinaturaAno.ID = tAssinaturaCliente.AssinaturaAnoID
                        WHERE 
                        tAssinatura.Nome NOT LIKE '%eleazar%' AND
                        ASsinaturaTipoID = "+ TipoID +" AND Ano = "+ this.TemporadaAnterior +@" AND tAssinaturaCliente.Status IN ('R', 'N', 'T') AND StatusImportacao = ''
                        AND tAssinatura.ID IN ("+ assinaturas +")";

            BD bd = new BD();
            BD bdAno = new BD();
            bd.Consulta(sql);

            var assinaturaID = 0;
            var novaAssinaturaID = 0;

            
            try
            {

                while (bd.Consulta().Read())
                {
                    try
                    {
                        assinaturaID = bd.LerInt("AssinaturaID");

                        var n = Mapeamento.Where(c => c.AnteriorID.Equals(assinaturaID));
                        if (n == null)
                            continue;
                        else
                            novaAssinaturaID = n.FirstOrDefault().NovoID;


                        var anoID = Convert.ToInt32(bdAno.ConsultaValor("SELECT ID FROM tAssinaturaAno(NOLOCK) WHERE Ano = " + TemporadaNova + " AND AssinaturaID = " + novaAssinaturaID));
                        if (anoID == 0)
                            continue;

                        var assinaturaBloqueioID = Convert.ToInt32(bdAno.ConsultaValor("SELECT BloqueioID FROM tAssinatura(NOLOCK) WHERE ID = " + novaAssinaturaID));
                        if (assinaturaBloqueioID == 0)
                            continue;



                        EstruturaAssinaturaBloqueio item = new EstruturaAssinaturaBloqueio
                        {
                            ClienteID = bd.LerInt("ClienteID"),
                            AssinaturaID = novaAssinaturaID,
                            LugarID = bd.LerInt("LugarID"),
                            AssinaturaAnoID = anoID,
                            SetorID = bd.LerInt("SetorID"),
                            AssinaturaBloqueioID = assinaturaBloqueioID
                        };



                        var lista = new List<EstruturaAssinaturaBloqueio>();
                        lista.Add(item);
                        assinatura.Associar(bd.LerInt("ClienteID"), lista, novaAssinaturaID, anoID);

                        bdAno.Executar("UPDATE tAssinaturaCliente SET StatusImportacao = 'Importado' WHERE ID = " + bd.LerInt("ID"));

                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
                bdAno.Fechar();
            }





        }
    }
}
