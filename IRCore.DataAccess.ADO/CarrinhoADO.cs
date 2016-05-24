using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using IRCore.Util;

namespace IRCore.DataAccess.ADO
{
    public class CarrinhoADO : MasterADO<dbSite>
    {
        public CarrinhoADO(MasterADOBase ado = null) : base(ado) { }

        public Carrinho ConsultarIngresso(int ingressoID, string sessionID, enumCarrinhoStatus status)
        {
            string sql = @"SELECT TOP 1 * 
                           FROM Carrinho (NOLOCK) WHERE IngressoID = @ingressoID AND SessionID = @sessionID and status = @status";

            return this.conSite.Query<Carrinho>(sql, new
            {
                ingressoID = ingressoID,
                sessionID = sessionID,
                status = status.ValueAsString()
            }).FirstOrDefault();
        }

        private Carrinho addresultComMapeamento(Carrinho carrinho, Setor setor, Apresentacao apresentacao, Evento evento, Local local, EventoSubtipo eventoSubTipo, Tipo tipo)
        {
            carrinho.SetorObject = setor;
            carrinho.SetorID = setor.IR_SetorID;
            carrinho.Setor = setor.Nome;
            carrinho.ApresentacaoDataHora = apresentacao.Horario;
            carrinho.ApresentacaoID = apresentacao.IR_ApresentacaoID;
            carrinho.ApresentacaoObject = apresentacao;
            carrinho.Evento = evento.Nome;
            carrinho.EventoID = evento.IR_EventoID;
            carrinho.EventoObject = evento;
            carrinho.LocalID = local.IR_LocalID;
            carrinho.Local = local.Nome;
            carrinho.LocalImagemNome = local.Imagem;
            carrinho.EventoObject.SubtipoID = (eventoSubTipo != null) ? (int?)eventoSubTipo.IR_SubtipoID : null;
            carrinho.EventoObject.Subtipo = eventoSubTipo;
            carrinho.EventoObject.Tipo = tipo;
            carrinho.EventoObject.TipoID = (tipo != null) ? (int?)tipo.IR_TipoID : null;

            return carrinho;
        }

        public Carrinho ConsultarIngressoComMapeamento(int ingressoID, string sessionID, enumCarrinhoStatus status)
        {
            string queryString = @"
                            SELECT TOP 1
	                            Car.ID,Car.ClienteID,Car.Codigo,Car.LugarID,Car.EventoID,Car.IngressoID,Car.TipoLugar,Car.ApresentacaoID,Car.SetorID,Car.PrecoID,Car.LocalID,Car.Local,Car.Evento,Car.ApresentacaoDataHora,Car.Setor,Car.PrecoNome,Car.TimeStamp,Car.PrecoValor,Car.TaxaConveniencia,Car.SessionID,Car.Status,Car.TagOrigem,Car.PacoteGrupo,Car.PacoteNome,Car.Grupo,Car.Classificacao,Car.PrecoExclusivoCodigoID,Car.LocalImagemNome,Car.ValeIngressoTipoID,Car.ValidadeData,Car.ValeIngressoNome,Car.ValidadeDiasImpressao,Car.Acumulativo,Car.ValeIngressoID,Car.VendaBilheteriaID,Car.CotaItemID,Car.CotaItemIDAPS,Car.IsSpecial,Car.EmpresaID,Car.SerieID,Car.DonoID,Car.DonoCPF,Car.CodigoPromocional,Car.CotaVerificada,Car.ValorTaxaProcessamento,Car.PacoteID,Car.GerenciamentoIngressosID,Car.VoucherID,
	                            Setor.ID,Setor.IR_SetorID,Setor.Nome,Setor.LugarMarcado,Setor.ApresentacaoID,Setor.QtdeDisponivel,Setor.QuantidadeMapa,Setor.Obs,Setor.AprovadoPublicacao,Setor.PrincipalPrecoID,Setor.CodigoSala,Setor.NVendeLugar,
	                            Apr.ID,Apr.IR_ApresentacaoID,Apr.Horario,Apr.EventoID,Apr.UsarEsquematico,Apr.Programacao,Apr.CodigoProgramacao,Apr.CalcDiaDaSemana,Apr.CalcHorario,
	                            Eve.ID,Eve.IR_EventoID,Eve.Nome,Eve.LocalID,Eve.TipoID,Eve.Release,Eve.Obs,Eve.Imagem,Eve.Destaque,Eve.Prioridade,Eve.EntregaGratuita,Eve.RetiradaBilheteria,Eve.DisponivelAvulso,Eve.Parcelas,Eve.PublicarSemVendaMotivo,Eve.Publicar,Eve.SubtipoID,Eve.DataAberturaVenda,Eve.LocalImagemMapaID,Eve.LocalImagemNome,Eve.EscolherLugarMarcado,Eve.PalavraChave,Eve.ExibeQuantidade,Eve.BannersPadraoSite,Eve.MenorPeriodoEntrega,Eve.FilmeID,Eve.PermiteVendaPacote,Eve.PossuiTaxaProcessamento,Eve.LimiteMaximoIngressosEvento,Eve.LimiteMaximoIngressosEstado,Eve.ImagemDestaque,
	                            Loc.ID,Loc.IR_LocalID,Loc.Nome,Loc.Cidade,Loc.Estado,Loc.Obs,Loc.Endereco,Loc.CEP,Loc.DDDTelefone,Loc.Telefone,Loc.ComoChegar,Loc.TaxaMaximaEmpresa,Loc.BannersPadraoSite,Loc.EmpresaID,Loc.Pais,Loc.Imagem,Loc.CodigoPraca,Loc.Latitude,Loc.Longitude,Loc.LongitudeAsDecimal,Loc.LatitudeAsDecimal,
	                            EveST.ID,EveST.IR_SubtipoID,EveST.TipoID,EveST.Descricao,
	                            Tipo.ID,Tipo.IR_TipoID,Tipo.Nome,Tipo.Obs
                            FROM
	                            Carrinho AS Car (NOLOCK) JOIN
	                            Setor (NOLOCK) ON Car.SetorID = Setor.IR_SetorID AND Car.ApresentacaoID = Setor.ApresentacaoID JOIN
	                            Apresentacao AS Apr (NOLOCK) ON Car.ApresentacaoID = Apr.IR_ApresentacaoID JOIN
	                            Evento AS Eve (NOLOCK) ON Car.EventoID = Eve.IR_EventoID JOIN
	                            Local AS Loc (NOLOCK) ON Eve.LocalID = Loc.IR_LocalID LEFT JOIN
	                            EventoSubTipo AS EveST (NOLOCK) ON Eve.SubtipoID = EveST.IR_SubtipoID LEFT JOIN
	                            Tipo (NOLOCK) ON EveST.TipoID = Tipo.IR_TipoID
                            WHERE Car.IngressoID = @ingressoID and Car.Status = @status and car.sessionID = @sessionID";
            var query = conSite.Query<Carrinho, Setor, Apresentacao, Evento, Local, EventoSubtipo, Tipo, Carrinho>(queryString, addresultComMapeamento, new
            {
                ingressoID = ingressoID,
                status = status.ValueAsString(),
                sessionID = sessionID
            });
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Valida se preco pode ser utilizando em uma apresentaçao e setor
        /// </summary>
        /// <param name="apresentacaoID">Apresentacao</param>
        /// <param name="setorID">Setor</param>
        /// <param name="precoID">Preco</param>
        /// <returns>retorna "true" caso o preco possa ser utilizado nesse setor</returns>
        public bool ValidaPrecoID(int apresentacaoID, int setorID, int precoID)
        {
            tApresentacaoSetor apeSet = dbIngresso.tApresentacaoSetor.Where(a => a.ApresentacaoID == apresentacaoID && a.SetorID == setorID).FirstOrDefault();
            if (apeSet == null)
            {
                return false;
            }
            else
            {
                if (dbIngresso.tPreco.Where(p => p.ID == precoID && p.tApresentacaoSetor.ID == apeSet.ID).FirstOrDefault() == null)
                {
                    return false;
                }
            }
            return true;
        }

        public List<Carrinho> ListarVoucher(int idVoucher, string sessionId)
        {
            string statusReservado = enumCarrinhoStatus.reservado.ValueAsString();
            var result = (from item in dbSite.Carrinho
                          where item.VoucherID == idVoucher && item.SessionID == sessionId && item.Status == statusReservado
                          select item);
            return result.ToList();
        }

        public List<Carrinho> Listar(string sessionID, int clienteID, enumCarrinhoStatus status)
        {
            string queryString = @"
                                SELECT 
	                                Car.ID,Car.ClienteID,Car.Codigo,Car.LugarID,Car.EventoID,Car.IngressoID,Car.TipoLugar,Car.ApresentacaoID,Car.SetorID,Car.PrecoID,Car.LocalID,Car.Local,Car.Evento,Car.ApresentacaoDataHora,Car.Setor,Car.PrecoNome,Car.TimeStamp,Car.PrecoValor,Car.TaxaConveniencia,Car.SessionID,Car.Status,Car.TagOrigem,Car.PacoteGrupo,Car.PacoteNome,Car.Grupo,Car.Classificacao,Car.PrecoExclusivoCodigoID,Car.LocalImagemNome,Car.ValeIngressoTipoID,Car.ValidadeData,Car.ValeIngressoNome,Car.ValidadeDiasImpressao,Car.Acumulativo,Car.ValeIngressoID,Car.VendaBilheteriaID,Car.CotaItemID,Car.CotaItemIDAPS,Car.IsSpecial,Car.EmpresaID,Car.SerieID,Car.DonoID,Car.DonoCPF,Car.CodigoPromocional,Car.CotaVerificada,Car.ValorTaxaProcessamento,Car.PacoteID,Car.GerenciamentoIngressosID,Car.VoucherID
                                FROM
	                                Carrinho AS Car (NOLOCK)
                                WHERE
	                                SessionID = @SessionID AND Status = @Status AND ClienteID = @ClienteID";
            var query = conSite.Query<Carrinho>(queryString, new
            {
                SessionID = sessionID,
                Status = status.ValueAsString(),
                ClienteID = clienteID
            });
            return query.ToList();
        }

        public List<Carrinho> ListarComMapeamento(string sessionID, int clienteID, enumCarrinhoStatus status)
        {

            string queryString = @"
                            SELECT
	                            Car.ID,Car.ClienteID,Car.Codigo,Car.LugarID,Car.EventoID,Car.IngressoID,Car.TipoLugar,Car.ApresentacaoID,Car.SetorID,Car.PrecoID,Car.LocalID,Car.Local,Car.Evento,Car.ApresentacaoDataHora,Car.Setor,Car.PrecoNome,Car.TimeStamp,Car.PrecoValor,Car.TaxaConveniencia,Car.SessionID,Car.Status,Car.TagOrigem,Car.PacoteGrupo,Car.PacoteNome,Car.Grupo,Car.Classificacao,Car.PrecoExclusivoCodigoID,Car.LocalImagemNome,Car.ValeIngressoTipoID,Car.ValidadeData,Car.ValeIngressoNome,Car.ValidadeDiasImpressao,Car.Acumulativo,Car.ValeIngressoID,Car.VendaBilheteriaID,Car.CotaItemID,Car.CotaItemIDAPS,Car.IsSpecial,Car.EmpresaID,Car.SerieID,Car.DonoID,Car.DonoCPF,Car.CodigoPromocional,Car.CotaVerificada,Car.ValorTaxaProcessamento,Car.PacoteID,Car.GerenciamentoIngressosID,Car.VoucherID,
	                            Setor.ID,Setor.IR_SetorID,Setor.Nome,Setor.LugarMarcado,Setor.ApresentacaoID,Setor.QtdeDisponivel,Setor.QuantidadeMapa,Setor.Obs,Setor.AprovadoPublicacao,Setor.PrincipalPrecoID,Setor.CodigoSala,Setor.NVendeLugar,
	                            Apr.ID,Apr.IR_ApresentacaoID,Apr.Horario,Apr.EventoID,Apr.UsarEsquematico,Apr.Programacao,Apr.CodigoProgramacao,Apr.CalcDiaDaSemana,Apr.CalcHorario,
	                            Eve.ID,Eve.IR_EventoID,Eve.Nome,Eve.LocalID,Eve.TipoID,Eve.Release,Eve.Obs,Eve.Imagem,Eve.Destaque,Eve.Prioridade,Eve.EntregaGratuita,Eve.RetiradaBilheteria,Eve.DisponivelAvulso,Eve.Parcelas,Eve.PublicarSemVendaMotivo,Eve.Publicar,Eve.SubtipoID,Eve.DataAberturaVenda,Eve.LocalImagemMapaID,Eve.LocalImagemNome,Eve.EscolherLugarMarcado,Eve.PalavraChave,Eve.ExibeQuantidade,Eve.BannersPadraoSite,Eve.MenorPeriodoEntrega,Eve.FilmeID,Eve.PermiteVendaPacote,Eve.PossuiTaxaProcessamento,Eve.LimiteMaximoIngressosEvento,Eve.LimiteMaximoIngressosEstado,Eve.ImagemDestaque,
	                            Loc.ID,Loc.IR_LocalID,Loc.Nome,Loc.Cidade,Loc.Estado,Loc.Obs,Loc.Endereco,Loc.CEP,Loc.DDDTelefone,Loc.Telefone,Loc.ComoChegar,Loc.TaxaMaximaEmpresa,Loc.BannersPadraoSite,Loc.EmpresaID,Loc.Pais,Loc.Imagem,Loc.CodigoPraca,Loc.Latitude,Loc.Longitude,Loc.LongitudeAsDecimal,Loc.LatitudeAsDecimal,
	                            EveST.ID,EveST.IR_SubtipoID,EveST.TipoID,EveST.Descricao,
	                            Tipo.ID,Tipo.IR_TipoID,Tipo.Nome,Tipo.Obs
                            FROM
	                            Carrinho AS Car (NOLOCK) JOIN
	                            Setor (NOLOCK) ON Car.SetorID = Setor.IR_SetorID AND Car.ApresentacaoID = Setor.ApresentacaoID JOIN
	                            Apresentacao AS Apr (NOLOCK) ON Car.ApresentacaoID = Apr.IR_ApresentacaoID JOIN
	                            Evento AS Eve (NOLOCK) ON Car.EventoID = Eve.IR_EventoID JOIN
	                            Local AS Loc (NOLOCK) ON Eve.LocalID = Loc.IR_LocalID JOIN
	                            EventoSubTipo AS EveST (NOLOCK) ON Eve.SubtipoID = EveST.IR_SubtipoID JOIN
	                            Tipo (NOLOCK) ON EveST.TipoID = Tipo.IR_TipoID
                            WHERE
	                            Car.SessionID = @SessionID AND Car.ClienteID = @ClienteID AND Car.Status = @StatusReservado";
            var query = conSite.Query<Carrinho, Setor, Apresentacao, Evento, Local, EventoSubtipo, Tipo, Carrinho>(queryString, addresultComMapeamento, new
            {
                SessionID = sessionID,
                ClienteID = clienteID,
                StatusReservado = enumCarrinhoStatus.reservado.ValueAsString()
            });
            return query.ToList();
        }

        public List<Carrinho> Listar(string sessionID, enumCarrinhoStatus status)
        {
            string queryString = @"SELECT ID,ClienteID,Codigo,LugarID,EventoID,IngressoID,TipoLugar,ApresentacaoID,SetorID,PrecoID,LocalID,Local,Evento,ApresentacaoDataHora,Setor,PrecoNome,TimeStamp,PrecoValor,TaxaConveniencia,SessionID,Status,TagOrigem,PacoteGrupo,PacoteNome,Grupo,Classificacao,PrecoExclusivoCodigoID,LocalImagemNome,ValeIngressoTipoID,ValidadeData,ValeIngressoNome,ValidadeDiasImpressao,Acumulativo,ValeIngressoID,VendaBilheteriaID,CotaItemID,CotaItemIDAPS,IsSpecial,EmpresaID,SerieID,DonoID,DonoCPF,CodigoPromocional,CotaVerificada,ValorTaxaProcessamento,PacoteID,GerenciamentoIngressosID,VoucherID
                                    FROM Carrinho
                                    Where SessionID = @SessionID AND Status = @status";

            var query = conSite.Query<Carrinho>(queryString, new
            {
                SessionID = sessionID,
                status = status.ValueAsString()
            });
            return query.ToList();
        }

        public List<Carrinho> Consultar(string sessionID, int clienteID)
        {
            return dbSite.Carrinho.Where(s => s.SessionID == sessionID && s.ClienteID == clienteID).ToList();
        }

        public List<Carrinho> ListarVendaBilheteria(int vendaBilheteriaID)
        {
            return (from carrinho in dbSite.Carrinho
                    where carrinho.VendaBilheteriaID == vendaBilheteriaID
                    select carrinho).ToList();
        }

        public void VincularCliente(int clienteID, string sessionID)
        {
            conSite.Execute("UPDATE Carrinho SET ClienteID = @clienteID WHERE SessionID = @sessionID AND Status = 'R'", new { clienteID = clienteID, sessionID = sessionID });

            conIngresso.Execute("UPDATE tIngresso SET ClienteID = @clienteID WHERE SessionID = @sessionID AND Status = 'R'", new { clienteID = clienteID, sessionID = sessionID });
        }

        public void LimparReserva(string sessionID, enumIngressoStatus statusIngresso)
        {
            AtualizarStatus(sessionID, enumCarrinhoStatus.deletado);
            conIngresso.Execute("UPDATE tIngresso SET ClienteID = null, UsuarioID = null, SessionID = null, Status = @status, CotaItemID = null WHERE SessionID = @sessionID AND Status = 'R'", new
            {
                sessionID = sessionID,
                status = statusIngresso.ValueAsString()
            });
        }

        public void AtualizarStatus(string sessionID, enumCarrinhoStatus status)
        {
            conSite.Execute("UPDATE Carrinho SET Status = @status WHERE SessionID = @sessionID AND Status = 'R'", new
            {
                sessionID = sessionID,
                status = status.ValueAsString()
            });
        }

        public int Contar(string sessionID, enumCarrinhoStatus status)
        {

            int size = Convert.ToInt32(conSite.ExecuteScalar("SELECT count(ID) FROM Carrinho (nolock) WHERE SessionID = @sessionID AND  Status = @status", new
            {
                sessionID = sessionID,
                status = status.ValueAsString()
            }));
            return size;
        }

        public bool ReservarIngresso(Carrinho carrinho, tIngresso ingresso, enumIngressoStatus status)
        {
            var result = (conIngresso.Execute(@"UPDATE tIngresso SET 
                                ClienteID = @clienteID, 
                                UsuarioID = @usuarioID, 
                                SessionID = @sessionID, 
                                PrecoID = @precoID,
                                TimeStampReserva = @timeStampReserva,
                                Status = 'R'
                            WHERE ID = @id AND Status = @status", new
                                                                {
                                                                    clienteID = ingresso.ClienteID,
                                                                    sessionID = ingresso.SessionID,
                                                                    usuarioID = ingresso.UsuarioID,
                                                                    precoID = ingresso.PrecoID,
                                                                    timeStampReserva = ingresso.TimeStampReserva,
                                                                    id = ingresso.ID,
                                                                    status = status.ValueAsString()
                                                                }) > 0);

            if (result)
            {
                result = Salvar(carrinho);
            }
            return result;
        }

        public bool Remover(int id)
        {
            var sql = @"DELETE FROM CARRINHO WHERE ID = @ID";

            var linhas = conSite.Execute(sql, new { ID = id });

            return linhas > 0;

        }

        public string ConsultarTipoLugar(int ingressoID)
        {
            return (string)conIngresso.ExecuteScalar(@"select top 1 s.LugarMarcado from tIngresso(nolock) i
                                                join tApresentacaoSetor(nolock) tas on tas.ID = i.ApresentacaoSetorID
                                                join tSetor(nolock) s on s.ID = tas.SetorID
                                                where i.ID =  @ingressoID;"
                                                , new
                                                {
                                                    ingressoID = ingressoID
                                                }
            );
        }

        public string ConsultarTipoLugar(int apresentacaoID, int lugarID)
        {
            return (string)conIngresso.ExecuteScalar(@"select top 1 s.LugarMarcado from tIngresso (nolock) i
                                                        join tApresentacaoSetor(nolock) tas on tas.ID = i.ApresentacaoSetorID
                                                        join tSetor(nolock) s on s.ID = tas.SetorID
                                                       where i.ApresentacaoID =  @apresentacaoID and i.LugarID = @lugarID;"
                                                       , new
                                                       {
                                                           apresentacaoID = apresentacaoID,
                                                           lugarID = lugarID
                                                       }
            );
        }

        public bool Verificar(List<Carrinho> carrinho)
        {
            String strSql = "select count(a.ID) from tApresentacao(NOLOCK) a where a.ID in (" + String.Join(",", carrinho.Select(t => t.ApresentacaoID).ToList()) + ") and DisponivelVenda <> 'T'";
            LogUtil.Debug(string.Format("##CarrinhoADO.Verificar## SQL {0}", strSql));
            return conIngresso.ExecuteScalar<int>(strSql) == 0;
        }

        public bool RemoverDadosCota(int id)
        {
            var sql = "UPDATE Carrinho SET DonoID = 0, DonoCPF = '', CodigoPromocional = '', CotaVerificada = 0 WHERE id = @id";

            var linhas = conSite.Execute(sql, new { id });

            return linhas > 0;
        }
    }
}
