
using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRLib
{
    public class LeituraCodigo : LeituraCodigo_B
    {
        public enum CodigoResposta
        {
            //[Description("Operação Aceita")]
            OperacaoAceita = 0,
            //[Description("Operação Aceita (White List)")]
            OperacaoAceitaWhiteList = 1,
            //[Description("Estrutura Código Inválida")]
            EstruturaCodigoInvalida = 2,
            //[Description("Não Permitido")]
            NaoPermitido = 3,
            //[Description("Ingresso Cancelado")]
            IngressoCancelado = 4,
            //[Description("Impressão Cancelada")]
            ImpressaoCancelada = 5,
            //[Description("Pre-Impresso Cancelado")]
            PreImpressoCancelado = 6,
            //[Description("Ingresso Reimpresso")]
            IngressoReimpresso = 7,
            //[Description("Reentrada Proibida")]
            ReentradaProibida = 8,
            //[Description("Lotado")]
            Lotado = 9,
            //[Description("Tamanho Inválido")]
            TamanhoInvalido = 10,
            //[Description("Máscara Inválida")]
            MascaraInvalida =11,
            //[Description("Não Numérico")]
            NaoNumerico = 12,
            //[Description("Não Cadastrado na Lista Branca")]
            NaoCadastradoListaBranca = 13,
            //[Description("Local Inválido")]
            LocalInvalido = 14,
            //[Description("Setor Inválido")]
            SetorInvalido = 15,
            //[Description("Rejeitado")]
            Rejeitado = 16,
            //[Description("Erro Gravando BD")]
            ErroGravandoBD = 17,
            //[Description("Sem Entrada Registrada")]
            SemEntradaRegistrada = 18
        }

        static string LerCodigoResposta(CodigoResposta en)
        {
            //Type type = en.GetType();
            //MemberInfo[] memInfo = type.GetMember(en.ToString());
            //if (memInfo != null && memInfo.Length > 0)
            //{
            //    object[] attrs = memInfo[0].GetCustomAttributes(typeof(Description),
            //    false);
            //    if (attrs != null && attrs.Length > 0)
            //        return ((Description)attrs[0]).Text;
            //}
            //return en.ToString();
            return string.Empty;
        }

        /// <summary>
        /// Inserir novo(a) LeituraCodigo
        /// </summary>
        /// <returns></returns>	
        public void Inserir(BD bd)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tLeituraCodigo(EventoID, ApresentacaoID, SetorID, DataLeitura, CodigoBarra, Portaria, CodigoResultado) ");
                sql.Append("VALUES (@001,@002,@003,'@004','@005','@006',@007)");

                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.ApresentacaoID.ValorBD);
                sql.Replace("@003", this.SetorID.ValorBD);
                sql.Replace("@004", this.DataLeitura.ValorBD);
                sql.Replace("@005", this.CodigoBarra.ValorBD);
                sql.Replace("@006", this.Portaria.ValorBD);
                sql.Replace("@007", this.CodigoResultado.ValorBD);

                object obj = bd.ConsultaValor(sql.ToString()+"; SELECT SCOPE_IDENTITY();");
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                if(id > 0)
                    this.Control.ID= id;
                else
                    throw new Exception("O LeituraCodigo não pode ser inserido");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///Insere os registros da lista na tLeituraCodigo. Se não conseguir inserir um ou mais registros, desfaz a transação.
        ///O retorno é a estrutura nescessária para a atualização da black list.
        /// </summary>
        /// <param name="listaInserir"></param>
        /// <returns>Sucesso de todos os inserts</returns>
        public List<EstruturaBlackList> AtualizaControleDeAcessoBlackList(List<EstruturaLeituraCodigo> listaInserir,int eventoID,int apresentacaoID,int setorID,DateTime ultimaSincronizacao)
        {
            try
            {
                BD bd = new BD();
                StringBuilder sql;
                List<EstruturaBlackList> retorno = new List<EstruturaBlackList>();
                List<EstruturaBlackList> retornoAux = new List<EstruturaBlackList>();//usada para trazer os itens da tIngressoLog
                EstruturaBlackList retornoItem;
                DateTime agora = DateTime.Now;
                CodigoBarra codigoBarra = new CodigoBarra();

                bd.IniciarTransacao();
                foreach (EstruturaLeituraCodigo registroAtual in listaInserir)
                {
                    sql = new StringBuilder();
                    sql.Append("INSERT INTO tLeituraCodigo(EventoID, ApresentacaoID, SetorID, DataLeitura, CodigoBarra, Portaria, CodigoResultado,Coletor) ");
                    sql.Append("VALUES (@001,@002,@003,'@004','@005','@006',@007,@008)");

                    sql.Replace("@001", registroAtual.EventoID.ToString());
                    sql.Replace("@002", registroAtual.ApresentacaoID.ToString());
                    sql.Replace("@003", registroAtual.SetorID.ToString());
                    sql.Replace("@004", registroAtual.DataLeitura.ToString("yyyyMMddHHmmss"));
                    sql.Replace("@005", registroAtual.CodigoBarra);
                    sql.Replace("@006", registroAtual.Portaria);
                    sql.Replace("@007", registroAtual.CodigoResultado.ToString());
                    sql.Replace("@008", registroAtual.Coletor.ToString());

                    bool ok = bd.Executar(sql.ToString()) == 1 ? true : false;
                    if (!ok)//Não conseguiu inserir, desfaz a transação
                    {
                        bd.DesfazerTransacao();
                        return retorno;//devolve a lista vazia
                    }
                }
                

                string filtroSetor = " AND SetorID= " + setorID;
                if (setorID == 0)//todos os setores
                    filtroSetor = " ";
                //Primeiro traz os itens inseridos na tLeituraCodigo depois da ultima atualização
                bd.Comando = "";
                bd.Consulta(@"SELECT DataLeitura, CodigoBarra, Portaria, CodigoResultado, Coletor
                            FROM tLeituraCodigo(NOLOCK) WHERE EventoID ="+eventoID+" AND ApresentacaoID ="+apresentacaoID+filtroSetor+" AND DataLeitura > '"+ ultimaSincronizacao.ToString("yyyyMMddHHmmss")+"'");
                while (bd.Consulta().Read())
                {
                    retornoItem = new EstruturaBlackList();
                    retornoItem.ApresentacaoID = apresentacaoID;
                    retornoItem.CodigoBarra = bd.LerString("CodigoBarra");
                    retornoItem.ColetorNumero = bd.LerInt("Coletor");
                    retornoItem.DataHoraInclusao = bd.LerDateTime("DataLeitura");
                    retornoItem.DataHoraSincronizacao = agora;
                    retornoItem.EventoID = eventoID;
                    retornoItem.Motivo = (LeituraCodigo.CodigoResposta)bd.LerInt("CodigoResultado");
                    retornoItem.Portaria = bd.LerString("Portaria");
                    retornoItem.SetorID = setorID;
                    retorno.Add(retornoItem);

                }
                bd.Consulta().Close();
                retornoAux = codigoBarra.BlackList(eventoID, apresentacaoID, setorID, ultimaSincronizacao);
                bd.FinalizarTransacao();
                foreach (EstruturaBlackList item in retornoAux)
                {
                    retorno.Add(item);
                }

               
                return retorno;
            }
            catch
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable InserirTxt(int eventoID, int apresentacaoID, int setorID, string[] linhas)
        {
            BD bd = new BD();
            bd.IniciarTransacao();

            DataTable oDataTable = new DataTable("Codigos");
            oDataTable.Columns.Add("CodigosEncontrados", typeof(int));
            oDataTable.Columns.Add("CodigosNaoEncontrados", typeof(int));
            DataRow linhaTable = oDataTable.NewRow();
            linhaTable["CodigosEncontrados"] = 0;
            linhaTable["CodigosNaoEncontrados"] = 0;

            CodigoBarra oCodigoBarra = new CodigoBarra();

            try
            {
                //varre as linhas do txt
                foreach (string linha in linhas)
                {
                    if (linha.Trim() == string.Empty)
                        continue;

                    //separa os itens em variáveis
                    string[] parameters = linha.Split(',');
                    string codigoBarra = parameters[0].Trim();
                    string dataLeitura = parameters[1].Trim();
                    string portaria = parameters[2].Trim();
                    string entradaSaida = parameters[3].Trim();
                    LeituraCodigo.CodigoResposta oCodigoResposta = (LeituraCodigo.CodigoResposta)int.Parse(parameters[4]);

                    //grava no banco, usando a transação
                    LeituraCodigo oLeituraCodigo = new LeituraCodigo();
                    oLeituraCodigo.EventoID.Valor = eventoID;
                    oLeituraCodigo.ApresentacaoID.Valor = apresentacaoID;
                    oLeituraCodigo.SetorID.Valor = setorID;
                    oLeituraCodigo.DataLeitura.Valor = DateTime.ParseExact(dataLeitura, "yyyyMMddHHmmss", null);
                    oLeituraCodigo.CodigoResultado.Valor = (int)oCodigoResposta;
                    oLeituraCodigo.CodigoBarra.Valor = codigoBarra;
                    oLeituraCodigo.Portaria.Valor = portaria;
                    oLeituraCodigo.Inserir(bd);

                    //verifica se o código existe, para posterior relatório
                    if (oCodigoBarra.ExisteLog(codigoBarra))
                        linhaTable["CodigosEncontrados"] = (int)linhaTable["CodigosEncontrados"] + 1;
                    else
                        linhaTable["CodigosNaoEncontrados"] = (int)linhaTable["CodigosNaoEncontrados"] + 1;
                }

                oDataTable.Rows.Add(linhaTable);
                bd.FinalizarTransacao();
                return oDataTable;
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable BorderoUrna(int eventoID, int apresentacaoID, int setorID)
        {
            BD bd = new BD();
            SqlDataAdapter oAdapter = new SqlDataAdapter();
            SqlCommand oCommand;
            DataTable oDataTableVenda = new DataTable("BorderoVenda");
            DataTable oDataTableEntrada = new DataTable("BorderoEntrada");
            DataTable oDataTableRetorno = new DataTable("Bordero");
            oDataTableRetorno.Columns.Add("Setor", typeof(string));
            oDataTableRetorno.Columns.Add("Preco", typeof(string));
            oDataTableRetorno.Columns.Add("Urna", typeof(int));
            oDataTableRetorno.Columns.Add("Venda", typeof(int));
            oDataTableRetorno.Columns.Add("NaoPresente", typeof(int));
            oDataTableRetorno.Columns.Add("PrecoID", typeof(int));
            oDataTableRetorno.Columns.Add("SetorID", typeof(int));

            DataTable oDataTableRetornoFinal = new DataTable("Bordero");
            oDataTableRetornoFinal.Columns.Add("SetorPreco", typeof(string));
            oDataTableRetornoFinal.Columns.Add("Urna", typeof(int));
            oDataTableRetornoFinal.Columns.Add("Venda", typeof(int));
            oDataTableRetornoFinal.Columns.Add("NaoPresente", typeof(int));

            try
            {
                string sql = @"SELECT 
                s.Nome Setor,
                p.Nome Preco,
                SetorID,
                PrecoID,
                COUNT(i.ID) Qtde
                FROM tIngresso i(NOLOCK)
                INNER JOIN tPreco p ON p.ID = i.PrecoID
                INNER JOIN tSetor s ON s.ID = i.SetorID
                WHERE Status IN ('I','E','R')  
                AND EventoID = @eventoID
                AND ApresentacaoID = @apresentacaoID ";

                if (setorID != 0)
                    sql += "AND SetorID = @setorID ";

                sql += "GROUP BY s.Nome, p.Nome, SetorID, PrecoID";

                sql = sql.Replace("@eventoID", eventoID.ToString()).Replace("@apresentacaoID", apresentacaoID.ToString()).Replace("@setorID", setorID.ToString());
                oCommand = new SqlCommand(sql, (SqlConnection)bd.Cnn);
                oAdapter.SelectCommand = oCommand;
                oAdapter.Fill(oDataTableVenda);

                sql = @"SELECT                
                SetorID, 
                CodigoBarra 
                FROM tLeituraCodigo lc
                WHERE CodigoResultado = 0
                AND EventoID = @eventoID                 
                AND ApresentacaoID = @apresentacaoID ";

                if (setorID != 0)
                    sql += "AND SetorID = @setorID ";                

                sql = sql.Replace("@eventoID", eventoID.ToString()).Replace("@apresentacaoID", apresentacaoID.ToString()).Replace("@setorID", setorID.ToString());

                oCommand = new SqlCommand(sql, (SqlConnection)bd.Cnn);
                oAdapter.SelectCommand = oCommand;
                oAdapter.Fill(oDataTableEntrada);

                DataRow novaLinha ;

                foreach (DataRow linha in oDataTableVenda.Rows)
                {
                    novaLinha = oDataTableRetorno.NewRow();
                    novaLinha["Setor"] = (string)linha["Setor"];
                    novaLinha["Preco"] = (string)linha["Preco"];
                    novaLinha["Urna"] = 0;
                    novaLinha["Venda"] = (int)linha["Qtde"];
                    novaLinha["NaoPresente"] = 0;
                    novaLinha["PrecoID"] = (int)linha["PrecoID"];
                    novaLinha["SetorID"] = (int)linha["SetorID"];

                    oDataTableRetorno.Rows.Add(novaLinha);
                }

                int precoID=0;
                CodigoBarra oCodigoBarra = new CodigoBarra();
                string codigoBarraDecriptado;
                DataRow[] retornoSelect;
                string precoCodigo;
                object retorno;
                foreach (DataRow linha in oDataTableEntrada.Rows)
                {
                    codigoBarraDecriptado = oCodigoBarra.DecodificarCodigoBarra((string)linha["CodigoBarra"]);
                    precoCodigo = codigoBarraDecriptado.Substring(codigoBarraDecriptado.Length - 2, 2); 
                    retorno = bd.ConsultaValor(@"SELECT PrecoID FROM tCodigoBarra 
                                    WHERE EventoID = @eventoID 
                                    AND ApresentacaoID = @apresentacaoID
                                    AND SetorID = @setorID
                                    AND PrecoCodigo = @precoCodigo"
                        .Replace("@eventoID",eventoID.ToString())
                        .Replace("@apresentacaoID",apresentacaoID.ToString())
                        .Replace("@setorID",setorID.ToString())
                        .Replace("@precoCodigo",precoCodigo));

                    if (retorno is int)
                        precoID = (int)retorno;
                    else
                        throw new Exception("PrecoID não encontrado na tCodigoBarra!");


                     retornoSelect = oDataTableRetorno.Select("PrecoID = " + precoID + " AND SetorID = " + (int)linha["SetorID"]);
                     if (retornoSelect.Length == 1)
                         retornoSelect[0]["Urna"] = (int)retornoSelect[0]["Urna"] + 1;
                     else
                         throw new Exception("Não existe venda do código de barra selecionado!");
                }

                DataRow[] ordenadorDataTable = oDataTableRetorno.Select("1=1", "Setor, Preco");

                string espacamento = "$"; //é convertido no momento do BIND no aspx
                int setorIDAtual=0;
                int setorIDAnterior=0;
                DataRow linhaNova;
                foreach (DataRow linha in ordenadorDataTable)
                {
                    setorIDAtual = (int)linha["SetorID"];

                    // É setor Novo. Insere a linha SOMENTE com setor.
                    if (setorIDAtual != setorIDAnterior)
                    {
                        linhaNova = oDataTableRetornoFinal.NewRow();
                        linhaNova["SetorPreco"] = (string)linha["Setor"];
                        oDataTableRetornoFinal.Rows.Add(linhaNova);
                    }

                    linhaNova = oDataTableRetornoFinal.NewRow();
                    linhaNova["SetorPreco"] = espacamento + (string)linha["Preco"];
                    linhaNova["Urna"] = (int)linha["Urna"];
                    linhaNova["Venda"] = (int)linha["Venda"];
                    linhaNova["NaoPresente"] = (int)linha["Venda"] - (int)linha["Urna"];


                    oDataTableRetornoFinal.Rows.Add(linhaNova);

                    setorIDAnterior = setorIDAtual;
                }

                return oDataTableRetornoFinal;
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

        public DataTable AuditoriaCodigoBarra(int eventoID, int apresentacaoID, int setorID)
        {
            DataTable oDataTable = new DataTable("CodigoBarra");
            oDataTable.Columns.Add("DataLeitura", typeof(string));
            oDataTable.Columns.Add("CodigoBarra", typeof(string));
            oDataTable.Columns.Add("Senha", typeof(string));
            oDataTable.Columns.Add("Canal", typeof(string));
            oDataTable.Columns.Add("Loja", typeof(string));
            oDataTable.Columns.Add("Operador", typeof(string));
            oDataTable.Columns.Add("CodigoIngresso", typeof(string));
            oDataTable.Columns.Add("CodigoResultado", typeof(string));

            BD bd = new BD();

            try
            {

                string sql = @"SELECT 
                DataLeitura, 
                lc.CodigoBarra, 
                vb.Senha,
                cn.Nome Canal,
                l.Nome Loja,
                u.Login Operador,
                i.Codigo CodigoIngresso,
                lc.CodigoResultado
                FROM tLeituraCodigo lc
                INNER JOIN tIngressoLog il (NOLOCK) ON il.CodigoBarra = lc.CodigoBarra
                INNER JOIN tIngresso i (NOLOCK)ON i.ID = il.IngressoID
                INNER JOIN tVendaBilheteria vb (NOLOCK) ON il.VendaBilheteriaID = vb.ID
                INNER JOIN tCaixa c (NOLOCK) ON il.CaixaID = c.ID
                INNER JOIN tLoja l (NOLOCK) ON l.ID = il.LojaID
                INNER JOIN tCanal cn (NOLOCK) ON il.CanalID = cn.ID
                INNER JOIN tUsuario u (NOLOCK) ON c.UsuarioID = u.ID
                WHERE lc.EventoID = @eventoID";
                if (apresentacaoID != 0)
                    sql += "AND lc.ApresentacaoID = @apresentacaoID";
                if (setorID != 0)
                    sql += "AND lc.SetorID = @setorID";

                sql = sql.Replace("@eventoID", eventoID.ToString())
                    .Replace("@apresentacaoID", apresentacaoID.ToString())
                    .Replace("@setorID", setorID.ToString());

                DataRow linhaNova;
                while (bd.Consulta(sql).Read())
                {
                    linhaNova = oDataTable.NewRow();
                    linhaNova["DataLeitura"] = bd.LerDateTime("DataLeitura").ToString("dd/MM/yyyy HH:mm");
                    linhaNova["CodigoBarra"] = bd.LerString("CodigoBarra");
                    linhaNova["Senha"] = bd.LerString("Senha");
                    linhaNova["Canal"] = bd.LerString("Canal");
                    linhaNova["Loja"] = bd.LerString("Loja");
                    linhaNova["Operador"] = bd.LerString("Operador");
                    linhaNova["CodigoIngresso"] = bd.LerString("CodigoIngresso");
                    linhaNova["CodigoResultado"] = LerCodigoResposta((CodigoResposta)bd.LerInt("CodigoResultado"));
                    oDataTable.Rows.Add(linhaNova);
                }
                return oDataTable;
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
    }
   
    //class Description : Attribute
    //{
    //    public string Text;
    //    public Description(string text)
    //    {
    //        Text = text;
    //    }
    //}
}
