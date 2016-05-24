using Admin.Areas.Logistica.Models;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRCore.BusinessObject
{
    public class PlanilhaBO : MasterBO<PlanilhaADO>
    {
        public PlanilhaBO(MasterADOBase ado = null) : base(ado) { }

        private string caminho = string.Empty;

        private enumRastreioStatus enumTipo;

        public List<ExcelModelo> carregaExcel(HttpPostedFileBase arquivo, enumRastreioStatus enumTipo, string pathFileSys, string usuarioLogin)
        {
            try
            {
                this.enumTipo = enumTipo;
                string nomeArquivo = enumTipo.ToString() + "_" + usuarioLogin + "_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".xls";
                caminho = Path.Combine(pathFileSys, nomeArquivo);
                arquivo.SaveAs(caminho);
                var retorno = atualizaTabela(CarregaArquivo(caminho).ToList());
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ExcelModelo> atualizaTabela(List<ExcelModelo> excel)
        {
            VendaBilheteriaBO _VendaBilheteriaBO = new VendaBilheteriaBO();

            tVendaBilheteriaEntrega _tVendaBilheteriaEntrega;
            PlanilhaADO _PlanilhaADO;

            List<ExcelModelo> listaRetorno = new List<ExcelModelo>();

            int idBilheteriaEntrega;
            foreach (ExcelModelo item in excel)
            {
                idBilheteriaEntrega = 0;
                if (!String.IsNullOrEmpty(item.Senha))
                {
                    _PlanilhaADO = new PlanilhaADO();
                    _tVendaBilheteriaEntrega = new tVendaBilheteriaEntrega();

                    switch (enumTipo)
                    {
                        case enumRastreioStatus.EntregueViaSedex:
                            _tVendaBilheteriaEntrega.Tipo = "S";
                            idBilheteriaEntrega = _PlanilhaADO.BuscaSedex(item.CodigoRastreamento);
                            break;
                        case enumRastreioStatus.EntregueViaMensageiro:
                            _tVendaBilheteriaEntrega.Tipo = "F";
                            idBilheteriaEntrega = _PlanilhaADO.BuscaFlash(item.Tipo, Convert.ToDateTime( item.DataHoraOcorrencia), item.StatusTexto);
                            break;
                    }

                    if (idBilheteriaEntrega.Equals(0))
                    {
                        var retornoConsulta = _VendaBilheteriaBO.Consultar(item.Senha);
                        if (retornoConsulta != null)
                        {
                            _tVendaBilheteriaEntrega.VendaBilheteriaID = retornoConsulta.ID;

                            _tVendaBilheteriaEntrega.EmailEnviado = false;
                            _tVendaBilheteriaEntrega.CodigoRastreamento = item.CodigoRastreamento;
                            _tVendaBilheteriaEntrega.StatusTexto = item.StatusTexto;

                            string status = "";
                            if (!string.IsNullOrEmpty(item.NomeRecebedor))
                                status = "Recebido por: " + item.NomeRecebedor;
                            if (!string.IsNullOrEmpty(item.RG))
                                status += ((!string.IsNullOrEmpty(status)) ? ", Documento: " : "Documento: ") + item.RG;
                            if (!string.IsNullOrEmpty(item.GrauParentesco))
                                status += ((!string.IsNullOrEmpty(status)) ? ", Parentesco: " : "Parentesco: ") + item.GrauParentesco;
                            if (!string.IsNullOrEmpty(status))
                            {
                                status = " (" + status + ")";
                                _tVendaBilheteriaEntrega.StatusTexto += status;
                            }

                            DateTime data;
                            if (DateTime.TryParse(item.DataHoraOcorrencia, out data))
                                _tVendaBilheteriaEntrega.DataHoraOcorrencia = data;

                            _PlanilhaADO.Salvar(_tVendaBilheteriaEntrega);
                        }
                        else
                        {
                            DateTime data;
                            if (DateTime.TryParse(item.DataHoraOcorrencia, out data))
                                item.DataHoraOcorrencia = data.ToShortDateString();

                            item.StatusTexto = "Não foi encontrado venda para a senha. ";
                            listaRetorno.Add(item);
                        }
                    }
                    else
                    {
                        item.StatusTexto = "Item já existente na base. ";
                        listaRetorno.Add(item);
                    }
                }
            }
            return listaRetorno;
        }

        private IEnumerable<ExcelModelo> CarregaArquivo(string caminho)
        {
            ExcelQueryFactory excel = new ExcelQueryFactory(caminho);

            if (excel != null)
            {
                excel.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Jet;

                switch (enumTipo)
                {
                    case enumRastreioStatus.EntregueViaMensageiro:
                        excel.AddMapping<ExcelModelo>(o => o.DataHoraOcorrencia, "Data de Postagem");
                        excel.AddMapping<ExcelModelo>(o => o.StatusTexto, "Último Status");
                        excel.AddMapping<ExcelModelo>(o => o.Senha, "AR");
                        excel.AddMapping<ExcelModelo>(o => o.GrauParentesco, "Grau de Parentesco");
                        excel.AddMapping<ExcelModelo>(o => o.NomeRecebedor, "Nome do Recebedor");
                        excel.AddMapping<ExcelModelo>(o => o.RG, "RG");
                        break;
                    case enumRastreioStatus.EntregueViaSedex:
                        excel.AddMapping<ExcelModelo>(o => o.DataHoraOcorrencia, "datax");
                        excel.AddMapping<ExcelModelo>(o => o.CodigoRastreamento, "registro");
                        excel.AddMapping<ExcelModelo>(o => o.Senha, "destinatario");
                        break;

                }

                foreach (ExcelModelo item in excel.Worksheet<ExcelModelo>(excel.GetWorksheetNames().FirstOrDefault()))
                {

                    if (item.DataHoraOcorrencia == null)
                        item.DataHoraOcorrencia = DateTime.MinValue.ToString();

                    item.CodigoRastreamento = item.CodigoRastreamento + "BR";

                    //if (item.CodigoRastreamento == null)
                    //    item.CodigoRastreamento = string.Empty;

                    //if (item.StatusTexto == null)
                    //    item.StatusTexto = string.Empty;

                    //if (item.Senha == null)
                    //    item.Senha = string.Empty;

                    //if (item.Tipo == null)
                    //    item.Tipo = string.Empty;

                    yield return item;
                }
            }
            else
                yield return null;
        }
    }
}
