using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject
{
    public class CaixaBO : MasterBO<CaixaADO>
    {
        public CaixaBO(MasterADOBase ado = null) : base(ado) { }

        public RetornoModel<tCaixa> ConsultarCaixaInternet(int usuarioId, tLoja loja, tCanal canal)
        {
            RetornoModel<tCaixa> retorno = new RetornoModel<tCaixa>();

            retorno.Sucesso = true;

            DateTime dataHoje = DateTime.Today;

            retorno.Retorno = ado.ConsultarAberto(usuarioId);

            bool abrirCaixa = false;

            if (retorno.Retorno != null)
            {
                if (retorno.Retorno.LojaID == loja.ID)
                {
                    if (retorno.Retorno.DataAberturaAsDateTime < dataHoje)
                    {
                        Fechar(retorno.Retorno);
                        abrirCaixa = true;
                    }
                }
                else
                {
                    Fechar(retorno.Retorno);
                    abrirCaixa = true;
                }
            }
            else
            {
                abrirCaixa = true;
            }

            if (abrirCaixa)
            {
                retorno.Retorno = Abrir(usuarioId, loja, canal);
            }
            return retorno;
        }

        public void Fechar(tCaixa caixa)
        {
            caixa.DataFechamentoAsDateTime = DateTime.Now;
            ado.Salvar(caixa, caixa.UsuarioID);
        }

        public tCaixa Abrir(int usuarioId, tLoja loja, tCanal canal, decimal saldoInicial = 0)
        {
            tCaixa caixa = new tCaixa();
            caixa.DataAberturaAsDateTime = DateTime.Now;
            caixa.ApresentacaoID = 0;
            caixa.UsuarioID = usuarioId;
            caixa.LojaID = loja.ID;
            caixa.DataFechamento = "";
            caixa.SaldoInicial = saldoInicial;
            caixa.Comissao = canal.Comissao;

            ado.Salvar(caixa, usuarioId);
            return caixa;
        }
    }
}