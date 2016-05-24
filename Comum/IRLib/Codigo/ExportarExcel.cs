using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace IRLib
{

    public class ExportarExcel
    {
        string template = Template.ExcelExportFile.ToString();
        HSSFWorkbook doc;

        public void ExportarTabela(string caminhoSalvar, DataTable TabelaParaConverter)
        {

            StringBuilder stringBuilder = new StringBuilder(template);
            StringBuilder tituloColunas = new StringBuilder();
            int indexColuna = 0;
            int indexLinha = 0;

            //popula os titulos do relatório na StringBuilder
            foreach (DataColumn col in TabelaParaConverter.Columns)
            {
                tituloColunas.Append("<td>");
                tituloColunas.Append(TabelaParaConverter.Columns[indexColuna].ColumnName.ToString());
                indexColuna++;
            }
            StringBuilder dados = new StringBuilder();


            //popula os dados do relatório na StringBuilder
            foreach (DataRow row in TabelaParaConverter.Rows)
            {
                dados.Append("<TR>");

                foreach (DataColumn col in TabelaParaConverter.Columns)
                {

                    dados.Append("<TD>" + TabelaParaConverter.Rows[indexLinha][col].ToString() + "</TD>");

                }

                dados.Append("</TR>");
                indexLinha++;
            }

            //replaces no stringBuilder com o html base
            stringBuilder.Replace("($titulo$)", tituloColunas.ToString());
            stringBuilder.Replace("($dados$)", dados.ToString());
            //escreve o arquivo
            StreamWriter sWriter = new StreamWriter(caminhoSalvar, false, Encoding.Default);
            sWriter.Write(stringBuilder);
            sWriter.Close();

        }

        /// <summary>
        /// Exporta levando em consideração a Primeira linha da DataTable como titulo
        /// </summary>
        /// <param name="caminhoSalvar"></param>
        /// <param name="TabelaParaConverter"></param>
        public void ExportarTabelaTitulo(string caminhoSalvar, DataTable TabelaParaConverter)
        {

            StringBuilder stringBuilder = new StringBuilder(template);
            StringBuilder tituloColunas = new StringBuilder();
            int indexColuna = 0;
            int indexLinha = 0;

            //popula os titulos do relatório na StringBuilder
            foreach (DataColumn col in TabelaParaConverter.Columns)
            {
                tituloColunas.Append("<td>");
                tituloColunas.Append(TabelaParaConverter.Columns[indexColuna].ColumnName.ToString());
                indexColuna++;
            }
            StringBuilder dados = new StringBuilder();

            //popula os dados do relatório na StringBuilder
            foreach (DataRow row in TabelaParaConverter.Rows)
            {
                dados.Append("<TR>");

                foreach (DataColumn col in TabelaParaConverter.Columns)
                {

                    dados.Append("<TD>" + TabelaParaConverter.Rows[indexLinha][col].ToString() + "</TD>");

                }

                dados.Append("</TR>");
                indexLinha++;
            }

            //replaces no stringBuilder com o html base
            stringBuilder.Replace("($titulo$)", tituloColunas.ToString());
            stringBuilder.Replace("($dados$)", dados.ToString());
            //escreve o arquivo
            StreamWriter sWriter = new StreamWriter(caminhoSalvar, false, Encoding.Default);
            sWriter.Write(stringBuilder);
            sWriter.Close();

        }


        /// <summary>
        /// Recebe um DataTable e devolve em formato string o html para ser salvo em Excel.
        /// </summary>
        /// <param name="TabelaParaConverter"></param>
        /// <returns></returns>
        public static string ExportarTabela(DataTable TabelaParaConverter)
        {
            string template = Template.ExcelExportFile.ToString();
            StringBuilder stringBuilder = new StringBuilder(template);
            StringBuilder tituloColunas = new StringBuilder();
            int indexColuna = 0;
            int indexLinha = 0;

            //popula os titulos do relatório na StringBuilder
            foreach (DataColumn col in TabelaParaConverter.Columns)
            {
                tituloColunas.Append("<td>");
                tituloColunas.Append(col.ColumnName.ToString());
                indexColuna++;
            }
            StringBuilder dados = new StringBuilder();


            //popula os dados do relatório na StringBuilder
            foreach (DataRow row in TabelaParaConverter.Rows)
            {
                dados.Append("<TR>");

                foreach (DataColumn col in TabelaParaConverter.Columns)
                {

                    dados.Append("<TD>" + TabelaParaConverter.Rows[indexLinha][col].ToString() + "</TD>");

                }

                dados.Append("</TR>");
                indexLinha++;
            }

            //replaces no stringBuilder com o html base
            stringBuilder.Replace("($titulo$)", tituloColunas.ToString());
            stringBuilder.Replace("($dados$)", dados.ToString());
            //Devolve o arquivo para salvar
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Método que gera uma planilha padrão. 
        /// </summary>
        /// <param name="tabela">Dados da tabela</param>
        /// <param name="caminhoSalvar">Diretório e nome do documento a ser salvo</param>
        /// <param name="nomePlanilha">Opcional. Nome da planilha exibida no documento</param>
        /// <param name="linhaInicial">Opcional. Linha inicial da planilha</param>
        /// <param name="colunaInicial">Opcional. Coluna inicial, use para omitir colunas a esquerda na planilha</param>
        public void GerarPlanilhaNPOI(DataTable tabela, string caminhoSalvar, string nomePlanilha = "plan1", int linhaInicial = 0, int colunaInicial = 0)
        {
            doc = new HSSFWorkbook();
            
            try
            {
                Sheet planilha = doc.CreateSheet(nomePlanilha);

                GerarHeadersNPOI(tabela, planilha, ref linhaInicial, colunaInicial);
                
                Row linha;

                for (int linhaPlan = linhaInicial, linhaDt = 0; linhaDt < tabela.Rows.Count; linhaPlan++, linhaDt++)
                {
                    DataRow linhaPlanilha = tabela.Rows[linhaDt];
                    linha = planilha.CreateRow(linhaPlan);

                    for (int colunaDt = colunaInicial, colunaPlan = 0; colunaDt < tabela.Columns.Count; colunaDt++, colunaPlan++)
                    {
                        string valor = linhaPlanilha[colunaDt].ToString();
                        linha.CreateCell(colunaPlan, CellType.STRING).SetCellValue(valor);
                    }
                }

                //apenas para redimensionar as colunas
                for (int colunaDt = colunaInicial, colunaPlan = 0; colunaDt < tabela.Columns.Count; colunaDt++, colunaPlan++)
                {
                    planilha.AutoSizeColumn(colunaPlan);
                }

                SalvarPlanilhaNPOI(caminhoSalvar);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Sobrecarga. Recebe um argumento Sheet caso algum conteudo ja tenha sido
        /// inserido na planilha.
        /// </summary>
        /// <param name="tabela">Dados da tabela</param>
        /// <param name="caminhoSalvar">Diretório e nome do documento a ser salvo</param>
        /// <param name="planilha">Sheet já criada e com conteudo</param>
        /// <param name="linhaInicial">Opcional. Linha inicial da planilha</param>
        /// <param name="colunaInicial">Opcional. Coluna inicial, use para omitir colunas a esquerda na planilha</param>
        public void GerarPlanilhaNPOI(DataTable tabela, string caminhoSalvar, Sheet planilha, int linhaInicial = 0, int colunaInicial = 0)
        {
            try
            {
                GerarHeadersNPOI(tabela, planilha, ref linhaInicial, colunaInicial);
                Row linha;

                for (int linhaPlan = linhaInicial, linhaDt = 0; linhaDt < tabela.Rows.Count; linhaPlan++, linhaDt++)
                {
                    DataRow linhaPlanilha = tabela.Rows[linhaDt];
                    linha = planilha.CreateRow(linhaPlan);

                    for (int colunaDt = colunaInicial, colunaPlan = 0; colunaDt < tabela.Columns.Count; colunaDt++, colunaPlan++)
                    {
                        string valor = linhaPlanilha[colunaDt].ToString();
                        linha.CreateCell(colunaPlan, CellType.STRING).SetCellValue(valor);
                    }
                }

                //apenas para redimensionar as colunas
                for (int colunaDt = colunaInicial, colunaPlan = 0; colunaDt < tabela.Columns.Count; colunaDt++, colunaPlan++)
                {
                    planilha.AutoSizeColumn(colunaPlan);
                }

                SalvarPlanilhaNPOI(caminhoSalvar);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GerarHeadersNPOI(DataTable tabela, Sheet planilha, ref int linhaInicial, int colunaInicial)
        {
            try
            {
                Row linha;
                Cell celula;

                NPOI.SS.UserModel.Font fonteCabecalho = doc.CreateFont();
                fonteCabecalho.FontName = "Arial";
                fonteCabecalho.Color = NPOI.HSSF.Util.HSSFColor.WHITE.index;
                fonteCabecalho.FontHeightInPoints = 10;
                fonteCabecalho.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

                CellStyle estiloCabecalho = doc.CreateCellStyle();
                estiloCabecalho.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.ROYAL_BLUE.index;
                estiloCabecalho.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.ROYAL_BLUE.index;
                estiloCabecalho.FillPattern = NPOI.SS.UserModel.FillPatternType.SOLID_FOREGROUND;
                estiloCabecalho.SetFont(fonteCabecalho);
                estiloCabecalho.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

                linha = planilha.CreateRow(linhaInicial);

                for (int colunaDt = colunaInicial, colunaPlan = 0; colunaDt < tabela.Columns.Count; colunaDt++, colunaPlan++)
                {
                    DataColumn cabecalho = tabela.Columns[colunaDt];
                    celula = linha.CreateCell(colunaPlan, CellType.STRING);
                    celula.CellStyle = estiloCabecalho;
                    celula.SetCellValue(cabecalho.ColumnName);
                }

                linhaInicial++;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SalvarPlanilhaNPOI(string caminhoSalvar)
        {
            FileStream arquivo = null;

            try
            {
                //Grava o arquivo
                arquivo = new FileStream(caminhoSalvar, FileMode.Create);
                doc.Write(arquivo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (arquivo != null)
                    arquivo.Close();
            }

        }


        public void ExportarListaNegra(DataTable blackList, string caminhoSalvar)
        {
            doc = new HSSFWorkbook();

            try
            {
                Sheet planilha = doc.CreateSheet();

                CellStyle estilo = doc.CreateCellStyle();
                estilo.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                estilo.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.ROYAL_BLUE.index;
                estilo.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.ROYAL_BLUE.index;
                estilo.FillPattern = NPOI.SS.UserModel.FillPatternType.SOLID_FOREGROUND;

                NPOI.SS.UserModel.Font fonte = doc.CreateFont();
                fonte.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                fonte.FontName = "Arial";
                fonte.FontHeightInPoints = 16;
                fonte.Color = NPOI.HSSF.Util.HSSFColor.WHITE.index;
                estilo.SetFont(fonte);

                Row linha = planilha.CreateRow(0);
                linha.HeightInPoints = 22;
                Cell celula = linha.CreateCell(0);
                celula.CellStyle = estilo;
                celula.SetCellValue(blackList.Rows[0][0].ToString());
                planilha.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));

                //pula 1 linha
                int linhaInicial = 2;

                //ignora a coluna com o nome do evento
                int colunaInicial = 1;

                GerarPlanilhaNPOI(blackList, caminhoSalvar, planilha, linhaInicial, colunaInicial);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
