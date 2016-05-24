using IRLib;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace IRLib
{
    public class GeradorPDFeTicket : IDisposable
    {
        MemoryStream ms = null;
        static Bitmap renderbitmap = null;
        static Exception exceptionThead = null;
        public GeradorPDFeTicket() { }

        public MemoryStream LereGerarHtml(List<IngressoImpressao> listaIngressos)
        {
            try
            {
                string htmlOriginal = File.ReadAllText(ConfigurationManager.AppSettings["Template_eTicket"]);

                List<Bitmap> imagens = new List<Bitmap>();
                EventoTaxaEntrega entrega = new EventoTaxaEntrega();

                foreach (var item in listaIngressos)
                {
                    string html = htmlOriginal.Replace("##Senha##", item.Senha)
                        .Replace("##DataVenda##", item.DataVenda)
                        .Replace("##EventoID##", item.EventoID.ToString())
                        .Replace("##Evento##", item.Evento)
                        .Replace("##Apresentacao##", item.Apresentacao)
                        .Replace("##Setor##", item.Setor)
                        .Replace("##Codigo##", item.Codigo)
                        .Replace("##CodigoBarra##", item.CodigoBarra)
                        .Replace("##Valor##", item.Valor.ToString("c"))
                        .Replace("##Preco##", item.Preco)
                        .Replace("##LocalNome##", item.Local)
                        .Replace("##Acesso##", item.Acesso)
                        .Replace("##EnderecoLocal##", item.EnderecoLocal)
                        .Replace("##NumeroLocal##", item.NumeroLocal)
                        .Replace("##BairroLocal##", item.BairroLocal)
                        .Replace("##CepLocal##", item.CepLocal)
                        .Replace("##CidadeLocal##", item.CidadeLocal)
                        .Replace("##EstadoLocal##", item.EstadoLocal)
                        .Replace("##EnderecoCliente##", item.EnderecoCliente)
                        .Replace("##NumeroCliente##", item.NumeroCliente)
                        .Replace("##ComplementoCliente##", item.ComplementoCliente)
                        .Replace("##BairroCliente##", item.BairroCliente)
                        .Replace("##EstadoCliente##", item.EstadoCliente)
                        .Replace("##CidadeCliente##", item.CidadeCliente)
                        .Replace("##CepCliente##", item.CepCliente)
                        .Replace("##Email##", item.Email)
                        .Replace("##DDDTelefone##", item.DDDTelefone)
                        .Replace("##Telefone##", item.Telefone)
                        .Replace("##DDDCelular##", item.DDDCelular)
                        .Replace("##Celular##", item.Celular)
                        .Replace("##ClienteNome##", item.ClienteNome)
                        .Replace("##ClienteCPFCNPJ##", item.ClienteCPFCNPJ)
                        .Replace("##FormaPagamento##", item.FormaPagamento)
                        .Replace("##ValorConveniencia##", item.ValorConveniencia.ToString("c"))
                        .Replace("##ValorTotal##", item.ValorTotal)
                        .Replace("##Canal##", item.Canal)
                        .Replace("##Alvara##", item.Alvara)
                        .Replace("##AVCB##", item.AVCB)
                        .Replace("##Fonte##", item.FonteImposto)
                        .Replace("##Imposto##", item.PorcentagemImposto.ToString());

                    imagens.Add(RenderBitmap(html));
                }

                return CreatePdf(imagens);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Render(object html)
        {
            try
            {
                System.Drawing.Rectangle crop = new System.Drawing.Rectangle(0, 0, 0, 0);

                using (WebBrowser wb = new WebBrowser())
                {
                    wb.ScrollBarsEnabled = false;
                    wb.ScriptErrorsSuppressed = true;
                    wb.DocumentText = html.ToString();

                    while (wb.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();

                    wb.Width = wb.Document.Body.ScrollRectangle.Width;
                    wb.Height = wb.Document.Body.ScrollRectangle.Height;

                    using (Bitmap bitmap = new Bitmap(wb.Width, wb.Height))
                    {
                        bitmap.SetResolution(600, 600);

                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(crop.Left, crop.Top, wb.Width - crop.Width - crop.Left, wb.Height - crop.Height - crop.Top);
                        wb.DrawToBitmap(bitmap, rect);
                        wb.Dispose();

                        renderbitmap = bitmap.Clone(rect, bitmap.PixelFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionThead = ex;
            }
        }

        private Bitmap RenderBitmap(string html)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(Render));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(html);
            thread.Join();

            while (thread.IsAlive && renderbitmap == null)
                continue;

            if (exceptionThead == null)
                return renderbitmap;
            else
                throw exceptionThead;
        }

        private MemoryStream CreatePdf(List<Bitmap> imagens)
        {
            try
            {
                using (Document doc = new Document())
                {
                    ms = new MemoryStream();

                    using (PdfWriter writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();

                        foreach (var imagem in imagens)
                        {
                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagem, BaseColor.WHITE);
                            img.SetAbsolutePosition(0, 0);

                            doc.SetPageSize(new iTextSharp.text.Rectangle(0, 0, imagem.Width, imagem.Height, 0));
                            doc.SetMargins(0, 0, 0, 0);
                            doc.NewPage();
                            writer.DirectContent.AddImage(img);
                        }

                        writer.CloseStream = false;
                        doc.Close();
                        ms.Position = 0;

                        return ms;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            if (ms != null) ms.Dispose();
            if (renderbitmap != null) renderbitmap.Dispose();
        }
       
    }
}
