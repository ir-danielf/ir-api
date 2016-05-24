using CTLib;
using System;
using System.Collections.Generic;

namespace IRLib
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class CupomDesconto : CupomDesconto_B
    {
        CupomDescontoLog Log = new CupomDescontoLog();

        bool validado = false;

        /// <summary>
        /// D = Disponivel 
        /// I = Indisponível
        /// </summary>
        public enum TipoStatus
        {
            Disponivel = 'D',
            Indisponivel = 'I'

        }

        public bool Validado
        {
            get { return validado; }
            set { validado = value; }
        }


        public override int ValidarCupom(string cupom)
        {
            BD bd = new BD();
            string status;
            string cupomHorarioValidacao;
            try
            {
                string sql = "SELECT TOP 1 * FROM tCupomDesconto WHERE Cupom='" + cupom +"' ORDER BY ID DESC";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    status = bd.LerString("Status");
                    cupomHorarioValidacao = bd.LerString("TimeStamp");
                    DateTime horario = DateTime.ParseExact(cupomHorarioValidacao, "yyyyMMddHHmmss", null);
                    
                    if ((char)TipoStatus.Disponivel != char.Parse(status))
                        throw new ApplicationException("Cupom não validado, por favor, ligue para 2163-2030 e valide seu cupom!");

                    if (horario <= DateTime.Now - TimeSpan.FromMinutes(30))
                        throw new ApplicationException("Tempo de validação do cupom expirado, por favor, ligue para 2163-2030 e valide seu cupom novamente.");
                    else
                        return bd.LerInt("ID");
                }
                else
                    throw new ApplicationException("Cupom inválido ou ainda não validado! Ligue para 2163-2030 para validar.");
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool ValidaCupomApresentacao(int apresentacaoID, string cupom)
        {
            this.Cupom.Valor = cupom;
            return validaCupomApresentacao(apresentacaoID);
        }

        bool validaCupomApresentacao(int apresentacaoID)
        {
            BD bd = new BD();
            try
            {
                object retornoBanco = bd.ConsultaValor("SELECT TOP 1 tCupomDescontoLog.ID FROM tCupomDescontoLog,tCupomDesconto " +
                                                        " WHERE tCupomDesconto.Cupom='" + this.Cupom.Valor +
                                                        "' AND ApresentacaoID = '" + apresentacaoID + "'");

                bool retorno = (retornoBanco is int);
                this.validado = retorno;
                return !retorno;

            }
            catch { return false; }
            finally
            {
                bd.Fechar();
            }
        }
        public List<int> ApresentacoesVendidasPorCupom(string cupom)
        {
            BD bd = new BD();
            List<int> apresentacoesID = new List<int>();
            try
            {
                string sql = "SELECT tCupomDescontoLog.ApresentacaoID FROM tCupomDesconto, tCupomDescontoLog "+
                             "WHERE tCupomDesconto.ID = tCupomDescontoLog.CupomID AND tCupomDesconto.Cupom = '" + cupom + "'";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    apresentacoesID.Add(bd.LerInt("ApresentacaoID"));
                }
                return apresentacoesID;

            }
            catch
            { throw; }
            finally
            {
                bd.Fechar();
            }
        }

        
        /// <summary>
        /// ValidaCupomApresentacao
        /// </summary>
        /// <returns>cupomID</returns>        
        public int ValidaCupomApresentacao(int[] apresentacoesID, string cupom)
        {
            try
            {
                // valida o cupom
                int cupomID = this.ValidarCupom(cupom);
                
                // preenche lista com Apresentacoes já adquiridas com uso de cupom Vivo
                List<int> lstApresentacoesVendidasPorCupom = new List<int>();
                lstApresentacoesVendidasPorCupom = this.ApresentacoesVendidasPorCupom(cupom);

                if (lstApresentacoesVendidasPorCupom.Count > 0)
                {
                    for (int i = 0; i < apresentacoesID.Length; i++)
                    {
                        if (lstApresentacoesVendidasPorCupom.Contains(apresentacoesID[i]))
                            throw new CupomDescontoException("Você já utilizou seu cupom VIVO para uma das apresentações");
                    }                    
                }

                // SUCESSO - retorna o cupomID para fazer Log no processo de Venda
                return cupomID; 
            }
            catch
            { throw; }
        }

    }
}
