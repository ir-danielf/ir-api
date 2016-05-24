using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using IRCore.DataAccess.ADO.Estrutura;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class VoucherADO : MasterADO<dbIngresso>
    {

        public VoucherADO(MasterADOBase ado = null) : base(ado, false) { }

        private List<Voucher> result;
        
        private int addResult(Voucher voucher, ParceiroMidia parceiro, ParceiroMidiaClasse classe, ParceiroMidiaPraca praca)
        {
            return addResult(voucher, parceiro, classe, null, praca);
        }
        private int addResult(Voucher voucher, ParceiroMidia parceiro, ParceiroMidiaClasse classe, ParceiroMidiaArea area, ParceiroMidiaPraca praca)
        {
            Voucher resultVoucher = result.FirstOrDefault(t => t.ID == voucher.ID);
            if (resultVoucher == null)
            {
                resultVoucher = voucher;
                resultVoucher.ParceiroMidiaArea = area;
                resultVoucher.ParceiroMidia = parceiro;
                resultVoucher.ParceiroMidiaClasse = classe;
                resultVoucher.ParceiroMidiaPraca = new List<ParceiroMidiaPraca>();

                result.Add(resultVoucher);
            }

            if(praca != null)
            {
                resultVoucher.ParceiroMidiaPraca.Add(praca);
                return praca.ID * voucher.ID;
            }
            
            return voucher.ID;
        }

        public Voucher Consultar(int id)
        {
            
            Voucher result = (from item in dbIngresso.Voucher
                              where item.ID == id
                              select item).AsNoTracking().FirstOrDefault();
            return result;
        }

        public IPagedList<Voucher> Listar(int pageNumber, int pageSize, int parceiroMediaID, enumVoucherStatus enumvoucherstatus = enumVoucherStatus.nenhum, string busca = null)
        {
            return ListarQuery(parceiroMediaID, enumvoucherstatus, busca).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        public List<Voucher> Listar(int parceiroMediaID, enumVoucherStatus enumvoucherstatus = enumVoucherStatus.nenhum, string busca = null)
        {
            return ListarQuery(parceiroMediaID, enumvoucherstatus, busca).AsNoTracking().ToList();
        }

        public List<Voucher> Listar(string[] codigos)
        {
            IQueryable<Voucher> result = dbIngresso.Voucher;
            result = result.Where(c => codigos.Contains(c.Codigo));

            return result.AsNoTracking().ToList();
        }

        public int Contar(int parceiroMediaID, enumVoucherStatus enumvoucherstatus = enumVoucherStatus.nenhum, string busca = null)
        {
            return ListarQuery(parceiroMediaID, enumvoucherstatus, busca).AsNoTracking().Count();
        }

        private IQueryable<Voucher> ListarQuery(int parceiroMediaID, enumVoucherStatus status = enumVoucherStatus.nenhum, string busca = null)
        {
            
            IQueryable<Voucher> result = dbIngresso.Voucher
                .Include(s => s.ParceiroMidia)
                .Include(s => s.ParceiroMidiaArea)
                .Include(s => s.ParceiroMidiaPraca)
                .Include(s => s.ParceiroMidiaClasse)
                .Where(t => t.ParceiroMidiaID == parceiroMediaID);
            
            // Filtrar por busca
            if (!string.IsNullOrEmpty(busca))
            {
                result = result.Where(t => t.Codigo.Contains(busca)
                                || t.ParceiroMidiaClasse.Nome.Contains(busca)
                                || (t.tCliente != null && t.tCliente.Nome.Contains(busca))
                                || (t.ParceiroMidiaArea != null && t.ParceiroMidiaArea.Nome.Contains(busca))
                                || (t.tVendaBilheteria.tIngresso.Count(x => x.tEvento.Nome.Contains(busca)) > 0));
            }

            if (status != enumVoucherStatus.nenhum && status != enumVoucherStatus.expirado)
            {
                string statusStr = ((char)status).ToString();
                //Se status for opção disponível é para carregar os bloqueados também
                result = result.Where(t => t.Status == statusStr);
            }

            if (status == enumVoucherStatus.disponivel)
            {
                DateTime date = DateTime.Now.Date;
                result = result.Where(t => t.DataExpiracao.CompareTo(date) > -1);
            }

            if (status == enumVoucherStatus.expirado)
            {
                DateTime date = DateTime.Now.Date;

                string stts = enumVoucherStatus.disponivel.ValueAsString();

                result = result.Where(t => t.DataExpiracao.CompareTo(date) == -1 && t.Status.Equals(stts));
            }

            result = result.OrderBy(t => t.Codigo);
            return result;
        }

        public List<VoucherClienteRetorno> Consultar(string cpf, int parceiroMidiaID = 6)
        {
            List<VoucherClienteRetorno> result = new List<VoucherClienteRetorno>(); ;

            var queryStr = @"select vo.Codigo as Voucher, vb.Senha as Senha, e.Nome as Evento, a.Horario as Apresentacao, s.Nome as Setor, i.Codigo as Ingresso
                            from tIngresso i (nolock)
                            inner join tVendaBilheteria vb (nolock) on vb.ID = i.VendaBilheteriaID
                            left  join Voucher vo (nolock) on vo.VendaBilheteriaID = vb.ID
                            inner join tCliente c (nolock) on c.ID = vb.ClienteID
                            inner join tEvento e (nolock) on e.ID = i.EventoID
                            inner join tApresentacao a (nolock) on a.EventoID = e.ID and a.ID = i.ApresentacaoID
                            inner join tSetor s (nolock) on s.ID = i.SetorID
                            where i.ParceiroMidiaID = @ParceiroMidiaID
                            and c.CPF = @Cpf  order by Senha desc";

            result = conIngresso.Query<VoucherClienteRetorno>(queryStr, new { ParceiroMidiaID = parceiroMidiaID, Cpf = cpf }).ToList();
            return result;

        }

        public Voucher Consultar(int idParceiro, string codigoVoucher)
        {

            var queryStr = @"Select
                         v.ID, v.Codigo, v.Status, v.SessionID, v.DataUso, v.ParceiroMidiaID, v.ClienteID, v.ParceiroMidiaClasseID, v.ParceiroMidiaAreaID, v.DataExpiracao, v.VendaBilheteriaID,
                         pm.ID, pm.Nome, pm.UrlContexto, pm.EmpresaID, pm.PrazoLiberacao, pm.Expiracao, pm.PaginaTitulo, pm.PaginaTexto, pm.PaginaRodape, pm.ImagemLogo, pm.IngressosPorVoucher,
                         pmc.ID, pmc.ParceiroMidiaID, pmc.Nome, pmc.Nivel,
                         pmp.ID, pmp.ParceiroMidiaID, pmp.CidadeNome
                    FROM Voucher (nolock) v
                INNER JOIN ParceiroMidia (nolock) pm ON v.ParceiroMidiaID = pm.ID
                INNER JOIN ParceiroMidiaClasse (nolock) pmc ON v.ParceiroMidiaClasseID = pmc.ID
                LEFT JOIN ParceiroMidiaPracaVoucher (nolock) pmpv ON v.ID = pmpv.VoucherID
                LEFT JOIN ParceiroMidiaPraca (nolock) pmp ON pmpv.ParceiroMidiaPracaID = pmp.ID
                WHERE pm.ID = @idParceiro and v.Codigo = @codigoVoucher";

            result = new List<Voucher>();
            var query = conIngresso.Query<Voucher, ParceiroMidia, ParceiroMidiaClasse, ParceiroMidiaPraca, int>(queryStr, addResult, new
            {
                idParceiro = idParceiro,
                codigoVoucher = codigoVoucher
            });


            return result.FirstOrDefault();
        }

        public enumVoucherStatus ConsultarStatusAtualizado(int id)
        {

            var queryStr = @"Select Top 1 v.Status
                              FROM Voucher (nolock) v
                             WHERE v.ID = @id";

            var status = conIngresso.Query<string>(queryStr, new { id = id }).FirstOrDefault();

            enumVoucherStatus enumStatus = enumVoucherStatus.nenhum;
            if (!string.IsNullOrEmpty(status))
            {
                enumStatus = (enumVoucherStatus)status[0];
            }
            return enumStatus;
        }


        public int ContarAreas(int areaID)
        {
            var result = (from item in dbIngresso.Voucher
                          where item.ParceiroMidiaAreaID == areaID
                          select item).AsNoTracking().Count();
            return result;
        }

        public int ContarPracas(int pracaID)
        {
            var result = (from item in dbIngresso.Voucher
                          where item.ParceiroMidiaPraca.Any(t => t.ID == pracaID)
                          select item).AsNoTracking().Count();
            return result;
        }

        public int ContarClasses(int classeID)
        {
            var result = (from item in dbIngresso.Voucher
                        where item.ParceiroMidiaClasseID == classeID
                          select item).AsNoTracking().Count();
            return result;
        }

        public bool AtualizarStatus(Voucher voucher, enumVoucherStatus statusAtual)
        {
            return conIngresso.Execute("UPDATE Voucher SET ClienteID = @clienteID, SessionID = @sessionID, DataUso = @dataUso, Status = @status, VendaBilheteriaID = @VendaBilheteriaID WHERE ID = @id AND Status = @statusAtual", new 
            {
                clienteID = voucher.ClienteID,
                sessionID = voucher.SessionID,
                dataUso = voucher.DataUso,
                status = voucher.Status,
                statusAtual = ((char)statusAtual).ToString(),
                vendaBilheteriaID = voucher.VendaBilheteriaID,
                id = voucher.ID
            }) > 0;
        }
    }
}
