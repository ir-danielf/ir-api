using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using IRLib.ClientObjects;

namespace IRCore.BusinessObject
{
    public class CotaBO : MasterBO<CotaADO>
    {
        public CotaBO(MasterADOBase ado) : base(ado) { }
        public CotaBO() : base(null) { }

        /// <summary>
        /// Método que varre os itens de um carrinho e carrega a propriedade CotaItem quando houver cota.
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        public CompraModel CarregarCotaInformacao(CompraModel compra)
        {
            if (VerificarCota(compra))
            {
                ParceiroBO parceiroBO = new ParceiroBO(ado);
                DonoIngressoBO donoIngressoBO = new DonoIngressoBO(ado);
                //Carrega o carrinho no formato antigo vindo do IRLIb
                var carrinhoList = new IngressoRapido.Lib.CarrinhoLista().CarregarDadosPorClienteID(compra.ClienteID, compra.SessionID, IngressoRapido.Lib.CarrinhoLista.Status.Reservado, 0);
                foreach (var carrinhoIRLib in carrinhoList)
                {
                    if (carrinhoIRLib.CotaItem != null)
                    {
                        Carrinho carrinho = compra.CarrinhoItens.Where(x => x.ID == carrinhoIRLib.ID).FirstOrDefault();
                        if (carrinho != null)
                        {
                            carrinho.CotaItemObject = new tCotaItem()
                            {
                                ID = carrinhoIRLib.CotaItem.ID,
                                ValidaBinAsBool = carrinhoIRLib.CotaItem.ValidaBin,
                                NominalAsBool = carrinhoIRLib.CotaItem.Nominal,
                                ValidaCodigoPromocional = carrinhoIRLib.CotaItem.ValidaCodidoPromocional,
                                Quantidade = carrinhoIRLib.CotaItem.Quantidade,
                                Termo = carrinhoIRLib.CotaItem.Termo,
                                TemTermo = carrinhoIRLib.CotaItem.TemTermo,
                                TextoValidacao = carrinhoIRLib.CotaItem.TextoValidacao,
                                Parceiro = carrinhoIRLib.CotaItem.ParceiroID > 0 ? parceiroBO.Consultar(carrinhoIRLib.CotaItem.ParceiroID) : null,
                                ParceiroID = carrinhoIRLib.CotaItem.ParceiroID,
                                CodigoPromocional = carrinhoIRLib.CotaItem.CodigoPromocional,
                                Obrigatoriedade = carrinhoIRLib.CotaItem.ObrigatoriedadeID > 0 ? ConsultarObrigatoriedade(carrinhoIRLib.CotaItem.ObrigatoriedadeID) : null,
                                ObrigatoriedadeID = carrinhoIRLib.CotaItem.ObrigatoriedadeID,
                                DonoIngresso = carrinhoIRLib.CotaItem.DonoID > 0 ? donoIngressoBO.Consultar(carrinhoIRLib.CotaItem.DonoID) : null,
                                Verificado = false,
                                Mensagem = string.Empty,
                                Cota = ado.ConsultarPorCotaItem(carrinhoIRLib.CotaItem.ID)
                            };
                            //Se tiver Cota, seta o ID dela na propriedade CotaID
                            if (carrinho.CotaItemObject.Cota != null)
                                carrinho.CotaItemObject.CotaID = carrinho.CotaItemObject.Cota.ID;
                        }
                    }
                }
            }
            return AtualizarStatusPendente(compra);
        }

        /// <summary>
        /// Verifica se existe um item no carrinho com Cota
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        public bool VerificarCota(CompraModel compra)
        {
            return compra.CarrinhoItens.Where(x => (x.CotaItemID != null && x.CotaItemID > 0) || (x.CotaItemIDAPS != null && x.CotaItemIDAPS > 0)).FirstOrDefault() != null;
        }

        /// <summary>
        /// Método que valida as cotas
        /// </summary>
        /// <returns></returns>
        public RetornoModel<CompraModel> ValidarCotas(CompraModel compra)
        {
            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();


            List<IngressoRapido.Lib.CotaItemValidar> cotaItemValidarList = new List<IngressoRapido.Lib.CotaItemValidar>();

            IngressoRapido.Lib.CarrinhoLista carrinhoItens = new IngressoRapido.Lib.CarrinhoLista().CarregarDadosPorClienteID(compra.ClienteID, compra.SessionID, IngressoRapido.Lib.CarrinhoLista.Status.Reservado, 0);

            foreach (var item in compra.CarrinhoItens.Where(x => x.CotaItemObject != null && !(x.CotaVerificada ?? false)))
            {
                item.CotaVerificada = true;
                var CotaItemValidar = new IngressoRapido.Lib.CotaItemValidar()
                {
                    IngressoID = item.IngressoID.Value,
                    Nominal = item.CotaItemObject.NominalAsBool,
                    TemParceiro = item.CotaItemObject.Parceiro != null,
                    Codigo = item.CotaItemObject.CodigoPromocional,
                    ClienteExiste = item.ClienteID > 0,
                    Mensagem = item.CotaItemObject.Mensagem,
                    Dono = new EstruturaDonoIngressoSite()
                };

                if (item.CotaItemObject.NominalAsBool)
                {
                    if (item.CotaItemObject.DonoIngresso == null)
                        item.CotaItemObject.DonoIngresso = new tDonoIngresso();

                    //Se a cota for nominal deve validar todos os campos abaixo
                    if ((item.CotaItemObject.Obrigatoriedade.NomeAsBool) && (string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.Nome)))
                    {
                        CotaItemValidar.Mensagem = item.CotaItemObject.Mensagem = "O nome é obrigatório.";
                        item.CotaVerificada = item.CotaItemObject.Verificado = false;
                    }
                    else if ((item.CotaItemObject.Obrigatoriedade.CPFAsBool) && (string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.CPF)))
                    {
                        CotaItemValidar.Mensagem = item.CotaItemObject.Mensagem = "O CPF é obrigatório.";
                        item.CotaVerificada = item.CotaItemObject.Verificado = false;
                    }
                    else if ((item.CotaItemObject.Obrigatoriedade.NomeResponsavelAsBool) && (string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.NomeResponsavel)))
                    {
                        CotaItemValidar.Mensagem = item.CotaItemObject.Mensagem = "O nome do responsável é obrigatório.";
                        item.CotaVerificada = item.CotaItemObject.Verificado = false;
                    }
                    else if ((item.CotaItemObject.Obrigatoriedade.CPFResponsavelAsBool) && (string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.CPFResponsavel)))
                    {
                        CotaItemValidar.Mensagem = item.CotaItemObject.Mensagem = "O CPF do responsável é obrigatório.";
                        item.CotaVerificada = item.CotaItemObject.Verificado = false;
                    }
                    else if ((item.CotaItemObject.Obrigatoriedade.TelefoneAsBool) && (string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.Telefone)))
                    {
                        CotaItemValidar.Mensagem = item.CotaItemObject.Mensagem = "O telefone é obrigatório.";
                        item.CotaVerificada = item.CotaItemObject.Verificado = false;
                    }
                    else if ((item.CotaItemObject.Obrigatoriedade.RGAsBool) && (string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.RG)))
                    {
                        CotaItemValidar.Mensagem = item.CotaItemObject.Mensagem = "O RG é obrigatório.";
                        item.CotaVerificada = item.CotaItemObject.Verificado = false;
                    }
                    else if ((item.CotaItemObject.Obrigatoriedade.DataNascimentoAsBool) && (string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.DataNascimento)))
                    {
                        CotaItemValidar.Mensagem = item.CotaItemObject.Mensagem = "A data de nascimento é obrigatória.";
                        item.CotaVerificada = item.CotaItemObject.Verificado = false;
                    }
                    else if ((item.CotaItemObject.Obrigatoriedade.EmailAsBool) && (string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.Email)))
                    {
                        CotaItemValidar.Mensagem = item.CotaItemObject.Mensagem = "O email é obrigatório.";
                        item.CotaVerificada = item.CotaItemObject.Verificado = false;
                    }

                    if (!string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.Telefone))
                    {
                        item.CotaItemObject.DonoIngresso.Telefone = item.CotaItemObject.DonoIngresso.Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                    }
                    if (!string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.CPF))
                    {
                        item.CotaItemObject.DonoIngresso.CPF = item.CotaItemObject.DonoIngresso.CPF.Replace("-", "").Replace(".", "");
                    }
                    if (!string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.CPFResponsavel))
                    {
                        item.CotaItemObject.DonoIngresso.CPFResponsavel = item.CotaItemObject.DonoIngresso.CPFResponsavel.Replace("-", "").Replace(".", "");
                    }
                    if (!string.IsNullOrEmpty(item.CotaItemObject.DonoIngresso.DataNascimento))
                    {
                        item.CotaItemObject.DonoIngresso.DataNascimento = item.CotaItemObject.DonoIngresso.DataNascimentoAsDateTime.Value.ToString("dd/MM/yyyy");
                    }

                    if (item.CotaVerificada ?? false)
                    {
                        CotaItemValidar.DonoID = item.CotaItemObject.DonoIngresso.ID;
                        CotaItemValidar.Dono = new IRLib.ClientObjects.EstruturaDonoIngressoSite()
                            {
                                CPF = item.CotaItemObject.DonoIngresso.CPF,
                                CPFResponsavel = item.CotaItemObject.CPFResponsavel,
                                DataNascimento = item.CotaItemObject.DonoIngresso.DataNascimento,
                                Email = item.CotaItemObject.DonoIngresso.Email,
                                Nome = item.CotaItemObject.DonoIngresso.Nome,
                                NomeResponsavel = item.CotaItemObject.DonoIngresso.NomeResponsavel,
                                RG = item.CotaItemObject.DonoIngresso.RG,
                                Telefone = item.CotaItemObject.DonoIngresso.Telefone
                            };

                    }
                }
                //Adiciona um item na lista de cotas a serem validados com valores do item atual caso a Cota esteja válida
                if (item.CotaVerificada ?? false)
                    cotaItemValidarList.Add(CotaItemValidar);
            }
            //Chama a verificação do IRLib
            cotaItemValidarList = carrinhoItens.VerificarCota(cotaItemValidarList);

            //seta os valores do carrinho com os valores retornados da validação da cota
            foreach (var item in compra.CarrinhoItens.Where(x => x.CotaItemObject != null))
            {
                var cotaValidada = cotaItemValidarList.Where(x => x.IngressoID == item.IngressoID).FirstOrDefault();
                if (cotaValidada != null)
                {
                    item.CotaVerificada = item.CotaItemObject.Verificado = cotaValidada.TipoRetorno == 0;
                    item.CotaItemObject.Mensagem = cotaValidada.Mensagem;
                    item.DonoID = cotaValidada.DonoID;
                    item.DonoCPF = cotaValidada.Dono.CPF;
                }
            }

            var cotasErro = compra.CarrinhoItens.Select(s => s.CotaItemObject).ToList().Where(w => !string.IsNullOrEmpty(w.Mensagem)).ToList();
            var mensagensCotasErro = string.Join<string>(" ", cotasErro.Select(s => s.Mensagem).Distinct());

            retorno.Retorno = AtualizarStatusPendente(compra);
            retorno.Sucesso = compra.CarrinhoItens.All(x => (x.CotaVerificada ?? false));
            retorno.Mensagem = retorno.Sucesso ? "OK" : mensagensCotasErro;
            return retorno;
        }

        public RetornoModel<CompraModel> ValidarCotasPorQuantidade(CompraModel compra, List<Carrinho> carrinhoItensValidar)
        {
            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();

            List<IngressoRapido.Lib.CotaItemValidar> cotaItemValidarList = new List<IngressoRapido.Lib.CotaItemValidar>();

            IngressoRapido.Lib.CarrinhoLista carrinhoItens = new IngressoRapido.Lib.CarrinhoLista().CarregarDadosPorClienteID(compra.ClienteID, compra.SessionID, IngressoRapido.Lib.CarrinhoLista.Status.Reservado, 0);

            foreach (var item in carrinhoItensValidar.Where(x => x.CotaItemObject != null && !(x.CotaVerificada ?? false)))
            {
                var CotaItemValidar = new IngressoRapido.Lib.CotaItemValidar()
                {
                    IngressoID = item.IngressoID.Value,
                    Nominal = item.CotaItemObject.NominalAsBool,
                    Quantidade = item.CotaItemObject.QuantidadeAsBool,
                    TemParceiro = item.CotaItemObject.Parceiro != null,
                    Codigo = item.CotaItemObject.CodigoPromocional,
                    ClienteExiste = item.ClienteID > 0,
                    Mensagem = item.CotaItemObject.Mensagem,
                    Dono = new EstruturaDonoIngressoSite()
                };

                item.CotaVerificada = item.CotaItemObject.QuantidadeAsBool && (!item.CotaItemObject.NominalAsBool && !item.CotaItemObject.ValidaBinAsBool && !item.CotaItemObject.ValidaCodigoPromocional);

                cotaItemValidarList.Add(CotaItemValidar);
            }

            //Chama a verificação do IRLib
            cotaItemValidarList = carrinhoItens.VerificarCotaPorQuantidade(cotaItemValidarList);

            //seta os valores do carrinho com os valores retornados da validação da cota
            foreach (var item in carrinhoItensValidar.Where(x => x.CotaItemObject != null))
            {
                var cotaValidada = cotaItemValidarList.Where(x => x.IngressoID == item.IngressoID).FirstOrDefault();
                if (cotaValidada != null)
                {
                    item.CotaVerificada = item.CotaItemObject.Verificado && cotaValidada.TipoRetorno == 0;
                    item.CotaItemObject.QuantidadeValidada = cotaValidada.TipoRetorno == 0;
                    item.CotaItemObject.Mensagem = cotaValidada.Mensagem;
                    item.DonoID = cotaValidada.DonoID;
                    item.DonoCPF = cotaValidada.Dono.CPF;
                }
            }

            var cotaItemObjects = carrinhoItensValidar.Select(s => s.CotaItemObject).ToList();

            var cotasErro = cotaItemObjects.Where(w => !string.IsNullOrEmpty(w.Mensagem)).ToList();
            var mensagensCotasErro = string.Join<string>(" ", cotasErro.Select(s => s.Mensagem).Distinct());

            retorno.Retorno = AtualizarStatusPendente(compra);
            retorno.Sucesso = cotaItemObjects.All(x => (x.QuantidadeValidada));
            retorno.Mensagem = retorno.Sucesso ? "OK" : mensagensCotasErro;
            return retorno;
        }

        public tObrigatoriedade ConsultarObrigatoriedade(int obrigatoriedadeID)
        {
            ObrigatoriedadeADO obrigatoriedadeADO = new ObrigatoriedadeADO(ado);
            return obrigatoriedadeADO.Consultar(obrigatoriedadeID);
        }

        public RetornoModel<CompraModel> VerificarLimite(CompraModel compra, List<Carrinho> carrinhoItensValidar)
        {
            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();

            if (compra.StatusCotaPendente.Quantidade)
            {
                return ValidarCotasPorQuantidade(compra, carrinhoItensValidar);
            }

            retorno.Retorno = compra;
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";

            return retorno;
        }

        public CompraModel AtualizarStatusPendente(CompraModel compra)
        {
            var cota = new CotaPendenteModel();
            var cotasPendentes = compra.CarrinhoItens.Where(x => x.CotaItemObject != null && (!(x.CotaVerificada ?? false))).Select(t => t.CotaItemObject);
            if (cotasPendentes.Count() > 0)
            {
                cota.Bin = cotasPendentes.Any(t => t.ValidaBinAsBool);
                cota.Nominal = cotasPendentes.Any(t => t.NominalAsBool);
                cota.Quantidade = cotasPendentes.Any(t => !t.QuantidadeValidada && t.QuantidadeAsBool);
                cota.Promocional = cotasPendentes.Any(t => t.ValidaCodigoPromocional);
                if (!cota.Nominal && !cota.Promocional && !cota.Quantidade)
                {
                    cota.NaoBin = !cotasPendentes.All(t => t.ValidaBinAsBool);
                }
            }
            else
            {
                cota.Nenhuma = true;
            }
            compra.StatusCotaPendente = cota;
            return compra;
        }
    }
}

