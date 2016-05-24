/**************************************************
* Arquivo: ValeIngressoLog.cs
* Gerado: 09/11/2009
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Text;

namespace IRLib
{

    public class ValeIngressoLog : ValeIngressoLog_B
    {

        public ValeIngressoLog() { }

        public enum enumAcao
        {
            Vender = 'V',
            Imprimir = 'I',
            Reimprimir = 'R',
            Trocar = 'T',
            Cancelar = 'C'
        }

        

        /// <summary>
        /// Inserir novo(a) ValeIngressoLog
        /// </summary>
        /// <returns></returns>	
        public string StringInserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO tValeIngressoLog(Acao, TimeStamp, ValeIngressoID, UsuarioID, VendaBilheteriaID, VendaBilheteriaItemID, EmpresaID, CaixaID, LojaID, CanalID, CodigoTroca, CodigoBarra, ClienteNome, Obs,MotivoId, SupervisorID) ");
                sql.Append("VALUES ('@001',@002,@003,'@004',@005,@006,@007,@008,@009,@010,'@011','@012','@013','@014','@015', '@016')");

                sql.Replace("@001", this.Acao.ValorBD);
                sql.Replace("@002", this.TimeStamp.ValorBD);
                sql.Replace("@003", this.ValeIngressoID.ValorBD);
                sql.Replace("@004", this.UsuarioID.ValorBD);
                sql.Replace("@005", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@006", this.VendaBilheteriaItemID.ValorBD);
                sql.Replace("@007", this.EmpresaID.ValorBD);
                sql.Replace("@008", this.CaixaID.ValorBD);
                sql.Replace("@009", this.LojaID.ValorBD);
                sql.Replace("@010", this.CanalID.ValorBD);
                sql.Replace("@011", this.CodigoTroca.ValorBD);
                sql.Replace("@012", this.CodigoBarra.ValorBD);
                sql.Replace("@013", this.ClienteNome.ValorBD);
                sql.Replace("@014", this.Obs.ValorBD);
                sql.Replace("@015", this.MotivoID.ValorBD);
                sql.Replace("@016", this.SupervisorID.ValorBD);

                return sql.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static string StatusDescritivo(string letraAcao)
        {
            string resultado = "";
            switch (letraAcao)
            {
                case "V":
                    resultado = "Vendido";
                    break;
                case "C":
                    resultado = "Cancelado";
                    break;
                case "I":

                    resultado = "Impresso";
                    break;
                case "T":
                    resultado = "Trocado";
                    break;
                default:
                    resultado = letraAcao;
                    break;
            }
            return resultado;
        }

        public static string StatusDetalhado(string letraAcao, int tipoEntrega)
        {
            string resultado = "";
            switch (letraAcao)
            {
                case "V":
                    resultado = "Vendido";
                    break;
                case "C":
                    resultado = "Cancelado";
                    break;
                case "I":
                    switch (tipoEntrega)
                    {
                        case -1:
                            resultado = "Aguardando Retirada";
                            break;
                        case 0:
                            resultado = "Impresso";
                            break;
                        default:
                            resultado = "Em Trânsito";
                            break;
                    }

                    break;
                case "T":
                    resultado = "Trocado";
                    break;
                default:
                    resultado = letraAcao;
                    break;
            }
            return resultado;
        }

        public static string StatusDetalhado(string letraAcao, string tipoEntrega)
        {
            string resultado = "";
            switch (letraAcao)
            {
                case "V":
                    resultado = "Vendido";
                    break;
                case "C":
                    resultado = "Cancelado";
                    break;
                case "I":
                    switch (tipoEntrega)
                    {
                        case Entrega.RETIRADA:
                        case Entrega.RETIRADABILHETERIA:
                            resultado = "Aguardando Retirada";
                            break;
                        case Entrega.NORMAL:
                        case Entrega.AGENDADA:
                            resultado = "Em Trânsito";
                            break;
                        default:
                            resultado = "Impresso";
                            break;
                    }

                    break;
                case "T":
                    resultado = "Trocado";
                    break;
                default:
                    resultado = letraAcao;
                    break;
            }
            return resultado;
        }


        public static object AcaoDescritiva(string letraAcao)
        {
            string resultado = "";
            switch (letraAcao)
            {
                case "V":
                    resultado = "Vender";
                    break;
                case "C":
                    resultado = "Cancelar";
                    break;
                case "I":
                    resultado = "Imprimir";
                    break;
                case "R":
                    resultado = "ReImprimir";
                    break;
                case "T":
                    resultado = "Trocar";
                    break;
                default:
                    resultado = letraAcao;
                    break;
            }
            return resultado;
        }
    }

    public class ValeIngressoLogLista : ValeIngressoLogLista_B
    {

        public ValeIngressoLogLista() { }



    }

}
