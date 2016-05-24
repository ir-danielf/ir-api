using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class VoucherBO : MasterBO<VoucherADO>
    {
        public VoucherBO(MasterADOBase ado = null) : base(ado) { }

        public IPagedList<Voucher> Listar(int pageNumber, int pageSize, int parceiroID, enumVoucherStatus enumvoucherstatus, string busca = null)
        {
            VendaBilheteriaBO vendaBO = new VendaBilheteriaBO(ado);
            var lista = ado.Listar(pageNumber, pageSize, parceiroID, enumvoucherstatus, busca);
            foreach(var voucher in lista)
            {
                if(voucher.VendaBilheteriaID != null && voucher.VendaBilheteriaID.Value > 0)
                {
                    voucher.tVendaBilheteria = vendaBO.ConsultarComIngressosResumido(voucher.VendaBilheteriaID.Value);
                }
            }
            return lista;
        }

        public List<Voucher> Listar(string[] codigos)
        {
            return ado.Listar(codigos);
        }

        public int ContarAreas(int areaID)
        {

            return ado.ContarAreas(areaID);
        }

        public int Contar(int parceiroID, enumVoucherStatus enumvoucherstatus, string busca = null)
        {

            return ado.Contar(parceiroID, enumvoucherstatus, busca);
        }

        /// <summary>
        /// Salvar Voucher
        /// </summary>
        /// <param name="voucher"></param>
        public void Salvar(Voucher voucher, bool updateDataBase = true)
        {
            ado.Salvar(voucher, updateDataBase);
        }

        /// <summary>
        /// Salvar Voucher
        /// </summary>
        /// <param name="voucher"></param>
        public void Salvar()
        {
            ado.Salvar();
        }

        /// <summary>
        /// Retorna um Voucher de um determinado parceiro
        /// </summary>
        /// <param name="parceiro"></param>
        /// <param name="codigoVoucher"></param>
        /// <returns></returns>
        public Voucher Consultar(int idParceiro, string codigoVoucher)
        {
            Voucher voucher = ado.Consultar(idParceiro, codigoVoucher);

            return voucher;
        }

        /// <summary>
        /// Retorna um determinado Voucher
        /// </summary>
        /// <param name="urlContextoParceiro"></param>
        /// <param name="codigoVoucher"></param>
        /// <returns></returns>
        public Voucher Consultar(int codigoVoucher)
        {
            return ado.Consultar(codigoVoucher);
        }

        /// <summary>
        /// Verfica se um Voucher está disponível e caso não esteja retorna a mensagem
        /// </summary>
        /// <param name="voucher"></param>
        /// <param name="sessionId"></param>
        /// <param name="minExpiracao"></param>
        /// <returns></returns>
        public RetornoModel<Voucher> Verificar(Voucher voucher, bool consultarStatusAtualizado = false)
        {
            if(consultarStatusAtualizado)
            {
                voucher.StatusAsEnum = ado.ConsultarStatusAtualizado(voucher.ID);
            }
            
            switch(voucher.StatusAsEnum)
            {
                case enumVoucherStatus.disponivel:
                    return new RetornoModel<Voucher>() { Sucesso = true, Mensagem = "OK", Retorno = voucher }; 
                case enumVoucherStatus.inativo:
                    return new RetornoModel<Voucher>() { Sucesso = false, Mensagem = "Voucher está inativo" }; 
                case enumVoucherStatus.capturado:
                    return new RetornoModel<Voucher>() { Sucesso = false, Mensagem = "Voucher já foi utilizado" };
                default:
                    return new RetornoModel<Voucher>() { Sucesso = false, Mensagem = "Voucher com status inválido" }; 

            }
            
        }

        public bool AtualizarStatus(Voucher voucher, enumVoucherStatus statusAtual)
        {
            return ado.AtualizarStatus(voucher, statusAtual);
        }

        public int ContarPracas(int pracaID)
        {
            return ado.ContarPracas(pracaID);
        }

        public int ContarClasses(int classeID)
        {
            return ado.ContarClasses(classeID);
        }

        public List<VoucherClienteRetorno> Consultar(string cpf)
        {
            return ado.Consultar(cpf);

        }
    }
}
