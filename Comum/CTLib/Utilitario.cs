using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;


namespace CTLib 
{

	public sealed class Utilitario 
	{

		/// <summary>
		/// Converte um lista de Inteiros para string separado por vírgula
		/// </summary>
		public static string TabelaParaString(DataTable vetorInteiro, string campo)  
		{
			string caracteres="";
			try 
			{
				for (int indice=0; indice< vetorInteiro.Rows.Count; indice++) 
				{ 
					if (indice == 0) 
						caracteres = Convert.ToString(vetorInteiro.Rows[indice][campo]);
					else
						caracteres = caracteres + "," + Convert.ToString(vetorInteiro.Rows[indice][campo]);
				}
			} 
			catch (Exception erro) 
			{
				throw erro;
			}
			return caracteres;
		}
        /// <summary>
        /// Método para ser usado no evento KeyPress do text box. passando a e.KeyChar...caso seja um caracter inválido
        /// o método retorna um char vazio.
        /// Exemplo de uso : e.KeyChar = CTLib.Utilitario.ValidaCampoDecimal(e.KeyChar); 
        /// kim
        /// </summary>
        /// <param name="keyPress"></param>
        /// <returns></returns>
        public static char ValidaCampoDecimal(char keyPress)
        {
            try
            {
                if (keyPress == '.')
                    keyPress = ',';
                //não deixa escrever letras
                if (!Char.IsNumber(keyPress) && keyPress != ',' && keyPress != '\b') //'\b' é backspace
                {
                    return new char();
                }
                else
                    return keyPress;

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Método para ser usado no evento KeyPress do text box. passando a e.KeyChar...caso seja um caracter inválido
        /// o método retorna um char vazio.
        /// Exemplo de uso : e.KeyChar = CTLib.Utilitario.ValidaCampoNumerico(e.KeyChar); 
        /// kim
        /// </summary>
        /// <param name="keyPress"></param>
        /// <returns></returns>
        public static char ValidaCampoNumerico(char keyPress)
        {
            try
            {
                //não deixa escrever letras
                if (!Char.IsNumber(keyPress) && keyPress != '\b') //'\b' é backspace
                {
                    return new char();
                }
                else
                    return keyPress;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<string> ConvertEnumToListString(Type tipoDoEnum)
        {

            System.Reflection.FieldInfo[] infoCampos = tipoDoEnum.GetFields();
            List<string> retorno = new List<string>();
            foreach (System.Reflection.FieldInfo campo in infoCampos)
            {
                retorno.Add(campo.ToString());
            }
            return retorno;


        }

		/// <summary>
		/// Converte um lista de Inteiros para string separado por vírgula
		/// </summary>
		public static string ListaInteiroParaString(ArrayList vetorInteiro)  
		{
			string caracteres="";
			try 
			{
				for (int indice=0; indice< vetorInteiro.Count; indice++) 
				{ 
					if (indice == 0) 
						caracteres = Convert.ToString(vetorInteiro[indice]);
					else
						caracteres = caracteres + "," + Convert.ToString(vetorInteiro[indice]);
				}
			} 
			catch (Exception erro) 
			{
				throw erro;
			}
			return caracteres;
		}		
		/// <summary>
		/// Converte um vetor de Inteiros para string separado por vírgula
		/// </summary>
		public static string VetorInteiroParaString(int[] vetorInteiro)  
		{
			string caracteres="";
			try 
			{
				for (int indice=0; indice< vetorInteiro.Length; indice++) 
				{ 
					if (indice == 0) 
						caracteres = Convert.ToString(vetorInteiro[indice]);
					else
						caracteres = caracteres + "," + Convert.ToString(vetorInteiro[indice]);
				}
			} 
			catch (Exception erro) 
			{
				throw erro;
			}
			return caracteres;
		}

		//Monta uma string separada por vírgulas a partir de um vetor de string
		public static string VetorStringParaString(string[] vetorString)  
		{
			string caracteres="";
			try 
			{
				for (int indice=0; indice< vetorString.Length; indice++) 
				{ 
					if (indice == 0) 
						caracteres = vetorString[indice];
					else
						caracteres = caracteres + "," + vetorString[indice];
				}
			} 
			catch (Exception erro) 
			{
				throw erro;
			}
			return caracteres;
		}
		

		/// <summary>
		/// Converte um vetor de String para vetor de Inteiros
		/// </summary>
		public static int[] VetorStringParaInteiro( string[] vetorString)  
		{
			int[] vetorInteiros = new int[vetorString.Length];
			try 
			{
				for (int indice=0; indice< vetorString.Length; indice++) 
				{ 
					vetorInteiros[indice] = Convert.ToInt32(vetorString[indice]);
				}
			} 
			catch (Exception erro) 
			{
				throw erro;
			}
			return vetorInteiros;
		}
		/// <summary>
		/// Converte um vetor de String para vetor de Decimais
		/// </summary>
		public static decimal[] VetorStringParaDecimal( string[] vetorString)  
		{
			decimal[] vetorInteiros = new decimal[vetorString.Length];
			try 
			{
				for (int indice=0; indice< vetorString.Length; indice++) 
				{ 
					vetorInteiros[indice] = Convert.ToDecimal(vetorString[indice]);
				}
			} 
			catch (Exception erro) 
			{
				throw erro;
			}
			return vetorInteiros;
		}


      
	}

	/// <summary>
	/// Classe que envolve contas aritméticas
	/// </summary>
	public sealed class Operador 
	{
		public static decimal DividirPorZero(decimal divisor, decimal dividendo) 
		{
			decimal resultado =0;
			if (dividendo==0) 
			{
				resultado = 0;
			}
			else
			{
				resultado = divisor/dividendo;
			}
			return resultado;
		}
	}

	/// <summary>
	/// Classe que envolve a depuração
	/// </summary>
	public sealed class Depurador 
	{
		/// <summary>
		/// Mostra linha a linha, e para cada linha mostra os campos
		/// </summary>
		/// <param name="tabelaInformada"></param>
		public static void MostraTabela(DataTable tabelaInformada) 
		{
			try 
			{
				Debug.WriteLine("");
				Debug.WriteLine(tabelaInformada.TableName);
				Debug.WriteLine("======================");
				for (int contadorLinha =0; contadorLinha< tabelaInformada.Rows.Count; contadorLinha++) 
				{
					Debug.WriteLine("Registro " + contadorLinha.ToString());
					DataRow linha = tabelaInformada.Rows[contadorLinha];
					foreach(DataColumn umaLinhaColuna in linha.Table.Columns) 
					{
						string coluna = umaLinhaColuna.ColumnName;
						Debug.WriteLine(umaLinhaColuna.ColumnName +" = "+ linha[coluna].ToString());
					}
				}
			} 
			catch (Exception erro) 
			{
				throw erro;
			}
		}
	}

	public class Caracteres
	{
		/// <summary>
		/// Usado em SELECT para ganhar desempenho
		/// </summary>
		public static string ConverterINemOR(string campoInformado, string conteudoIN) 
		{
			try 
			{
				string[] vetorConteudo = conteudoIN.Split(','); 
				string resultado = "";
				bool primeiraVez = true;
				foreach (string conteudoAtual in vetorConteudo) 
				{
					if (primeiraVez) 
					{
						resultado = resultado + campoInformado + "='" + conteudoAtual +"' ";
						primeiraVez= false;
					} 
					else 
					{
						resultado = resultado + " OR " + campoInformado + "='" + conteudoAtual +"' ";
					}
				}
				return resultado;
			} 
			catch (Exception erro) 
			{
				throw erro;
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class TabelaMemoria : System.Data.DataTable 
	{
		# region Propriedades Locais
		private DataTable tabelaOrigem = null;
		private DataTable tabelaDestino = null;
		private string campo ="";
		private object conteudo = null;
		# endregion 
		# region Propriedades Globais
		public object Conteudo 
		{
			get {return conteudo; }
			set {conteudo = value; }
		}
		public string Campo 
		{
			get {return campo; }
			set {campo = value; }
		}
		public DataTable TabelaOrigem 
		{
			get {return tabelaOrigem; }
			set {tabelaOrigem = value; }
		}
		public DataTable TabelaDestino 
		{
			get {return tabelaDestino; }
			set {tabelaDestino = value; }
		}
		# endregion 

		public static DataTable LeftJoin (DataTable First, DataTable Second, DataColumn[] FJC, DataColumn[] SJC)
		{
			//Create Empty Table
			DataTable table = new DataTable("Join");

			// Use a DataSet to leverage DataRelation
			using(DataSet ds = new DataSet())
			{
				//Add Copy of Tables
				ds.Tables.AddRange(new DataTable[]{First.Copy(),Second.Copy()});

				//Identify Joining Columns from First
				DataColumn[] parentcolumns = new DataColumn[FJC.Length];

				for(int i = 0; i < parentcolumns.Length; i++)
				{
					parentcolumns[i] = ds.Tables[0].Columns[FJC[i].ColumnName];
				}

				//Identify Joining Columns from Second
				DataColumn[] childcolumns = new DataColumn[SJC.Length];

				for(int i = 0; i < childcolumns.Length; i++)
				{
					childcolumns[i] = ds.Tables[1].Columns[SJC[i].ColumnName];
				}

				//Create DataRelation
				DataRelation r = new DataRelation(string.Empty,parentcolumns,childcolumns,false);
				ds.Relations.Add(r);

				//Create Columns for JOIN table
				for(int i = 0; i < First.Columns.Count; i++)
				{
					table.Columns.Add(First.Columns[i].ColumnName, First.Columns[i].DataType);
				}

				for(int i = 0; i < Second.Columns.Count; i++)
				{
					//Beware Duplicates
					if(!table.Columns.Contains(Second.Columns[i].ColumnName))
						table.Columns.Add(Second.Columns[i].ColumnName, Second.Columns[i].DataType);
					else
						table.Columns.Add(Second.Columns[i].ColumnName + "_Second", Second.Columns[i].DataType);
				}


				//Loop through First table
				table.BeginLoadData();

				foreach(DataRow firstrow in ds.Tables[0].Rows)
				{
					//Get "joined" rows
					DataRow[] childrows = firstrow.GetChildRows(r);
					if(childrows != null && childrows.Length > 0)
					{
						object[] parentarray = firstrow.ItemArray;
						foreach(DataRow secondrow in childrows)
						{
							object[] secondarray = secondrow.ItemArray;
							object[] joinarray = new object[parentarray.Length+secondarray.Length];
							Array.Copy(parentarray,0,joinarray,0,parentarray.Length);
							Array.Copy(secondarray,0,joinarray,parentarray.Length,secondarray.Length);
							table.LoadDataRow(joinarray,true);
						}
					}
					else
					{
						object[] parentarray = firstrow.ItemArray;
						object[] joinarray = new object[parentarray.Length];
						Array.Copy(parentarray,0,joinarray,0,parentarray.Length);
						table.LoadDataRow(joinarray,true);
					}
				}
				table.EndLoadData();
			}

			return table;
		} 


		/// <summary>
		/// "SQL Inner Join" entre duas tabelas.
		/// </summary>
		/// <param name="First">Primeira tabela</param>
		/// <param name="Second">Segunda tabela</param>
		/// <param name="FJC">Campo de verificação da Primeira tabela</param>
		/// <param name="SJC">Campo de verificação da Primeira tabela</param>
		/// <returns>DataTable com o resultado do Inner Join</returns>
		public static DataTable Join (DataTable First, DataTable Second, DataColumn[] FJC, DataColumn[] SJC)

		{

			//Create Empty Table

			DataTable table = new DataTable("Join");

			// Use a DataSet to leverage DataRelation

			using(DataSet ds = new DataSet())

			{

				//Add Copy of Tables

				ds.Tables.AddRange(new DataTable[]{First.Copy(),Second.Copy()});

				//Identify Joining Columns from First

				DataColumn[] parentcolumns = new DataColumn[FJC.Length];

				for(int i = 0; i < parentcolumns.Length; i++)

				{

					parentcolumns[i] = ds.Tables[0].Columns[FJC[i].ColumnName];

				}

				//Identify Joining Columns from Second

				DataColumn[] childcolumns = new DataColumn[SJC.Length];

				for(int i = 0; i < childcolumns.Length; i++)

				{

					childcolumns[i] = ds.Tables[1].Columns[SJC[i].ColumnName];

				}

				//Create DataRelation

				DataRelation r = new DataRelation(string.Empty,parentcolumns,childcolumns,false);

				ds.Relations.Add(r);

				//Create Columns for JOIN table

				for(int i = 0; i < First.Columns.Count; i++)

				{

					table.Columns.Add(First.Columns[i].ColumnName, First.Columns[i].DataType);

				}

				for(int i = 0; i < Second.Columns.Count; i++)

				{

					//Beware Duplicates

					if(!table.Columns.Contains(Second.Columns[i].ColumnName))

						table.Columns.Add(Second.Columns[i].ColumnName, Second.Columns[i].DataType);

					else

						table.Columns.Add(Second.TableName + "." + Second.Columns[i].ColumnName, Second.Columns[i].DataType);

				}

				//Loop through First table

				table.BeginLoadData();

				foreach(DataRow firstrow in ds.Tables[0].Rows)

				{

					//Get "joined" rows

					DataRow[] childrows = firstrow.GetChildRows(r);

					if(childrows != null && childrows.Length > 0)
					{
						object[] parentarray = firstrow.ItemArray;

						foreach(DataRow secondrow in childrows)

						{
                            object[] secondarray = secondrow.ItemArray;

							object[] joinarray = new object[parentarray.Length+secondarray.Length];

							Array.Copy(parentarray,0,joinarray,0,parentarray.Length);

							Array.Copy(secondarray,0,joinarray,parentarray.Length,secondarray.Length);

							table.LoadDataRow(joinarray,true);
						}
					}
				}
				table.EndLoadData();
			}
			return table;
		}


		public TabelaMemoria() 
		{
		}
		/// <summary>
		/// Alterar primeiro registro
		/// </summary>
		public bool AlterarPrimeiroRegistro()
		{
			try
			{				
				bool resultado = false;
				if (tabelaOrigem.Rows.Count>0) 
				{
					tabelaDestino.Rows[0][campo] = conteudo;
					resultado = true;
				} 
				else 
				{
					resultado = false;
				}
				return resultado;
			}
			catch(Exception erro)
			{
				throw erro;
			}
		}
		/// <summary>
		/// Inserir primeiro registro
		/// </summary>
		public bool InserirPrimeiroRegistro()
		{
			try
			{				
				bool resultado = false;
				if (tabelaOrigem.Rows.Count >= 0) 
				{
					CopiaEstruturaParaDestino(tabelaOrigem);
					DataRow primeiraLinha = tabelaDestino.NewRow();
					primeiraLinha[campo] = conteudo;
					tabelaDestino.Rows.Add(primeiraLinha);
					foreach(DataRow umaLinha in tabelaOrigem.Rows) 
						tabelaDestino.ImportRow(umaLinha);
					resultado = true;
				} 
				else 
				{
					tabelaDestino = null;
					resultado = false;
				}
				return resultado;
			}
			catch(Exception erro)
			{
				throw erro;
			}
		}
		/// <summary>
		/// Pesquisa um número inteiro em um campo
		/// </summary>
		public bool PesquisaCampo(int inteiro, string campo)
		{
			foreach (DataRow linha in tabelaOrigem.Rows) 
			{
				if (inteiro == Convert.ToInt32(linha[campo]))
					return true;
			}
			// caso não encontre
			return false;
		}
		/// <summary>
		/// Retorna uma tabela vazia com um registro zero
		/// </summary>
		/// <param name="umRegistro"></param>
		/// <returns></returns>
		public DataTable TabelaVazia(string umRegistro)
		{
			DataTable tabela = new DataTable("Vazia");
			tabela.Columns.Add("ID", typeof(int));
			tabela.Columns.Add("Nome", typeof(string));
			DataRow linha = tabela.NewRow();
			linha["ID"]= 0;
			linha["Nome"]= umRegistro;
			tabela.Rows.Add(linha);
			return tabela;
		}
		/// <summary>
		/// Converte DataRow [] para DataTable
		/// </summary>
		/// <param name="linhas"></param>
		/// <returns></returns>
		public bool DataRowsParaTabelaDestino(DataRow[] linhas)
		{
			bool resultado = true;
			try
			{				
				if (linhas.Length>0) 
				{
					// ImportRow só funciona para tabelas de mesma estrutura 
					EstruturaDataRowsParaDestino(linhas);
					foreach(DataRow umaLinha in linhas) 
						tabelaDestino.ImportRow(umaLinha);
				} 
				else 
				{
					tabelaDestino = null;
					resultado = false;
				}
			}
			catch(Exception erro)
			{
				throw erro;
			}
			return resultado;
		}
		/// <summary>
		/// Pega uma estrutura baseado em um DataRow[] 
		/// Pois ImportRow só funciona para tabelas de mesma estrutura 
		/// </summary>
		/// <param name="linhas"></param>
		/// <returns></returns>
		public bool EstruturaDataRowsParaDestino(DataRow[] linhas)
		{
			bool resultado = true;
			try
			{				
				if (linhas.Length>0) 
				{
					tabelaDestino = new DataTable();
					foreach(DataColumn umaLinhaColuna in linhas[0].Table.Columns) 
					{
						DataColumn coluna = new  DataColumn();
						coluna.DataType = System.Type.GetType(umaLinhaColuna.DataType.FullName);
						coluna.ColumnName = umaLinhaColuna.ColumnName;
						tabelaDestino.Columns.Add(coluna);
					}
				} 
				else 
				{
					tabelaDestino = null;
					resultado = false;
				}
			}
			catch(Exception erro)
			{
				throw erro;
			}
			return resultado;
		}
		/// <summary>
		/// Pega uma estrutura baseado em uma tabela
		/// E copia para tabela origem
		/// </summary>
		public bool CopiaEstruturaParaOrigem(DataTable tabela)
		{
			bool resultado = true;
			try
			{				
				if (tabela != null) 
				{
					tabelaOrigem = new DataTable();
					foreach(DataColumn coluna in tabela.Columns) 
					{
						DataColumn colunaNova = new  DataColumn();
						colunaNova.DataType = System.Type.GetType(coluna.DataType.FullName);
						colunaNova.ColumnName = coluna.ColumnName;
						tabelaOrigem.Columns.Add(colunaNova);
					}
				} 
				else 
				{
					tabelaOrigem = null;
					resultado = false;
				}
			}
			catch(Exception erro)
			{
				throw erro;
			}
			return resultado;
		}
		/// <summary>
		/// Pega uma estrutura baseado em uma tabela
		/// E copia para tabela destino
		/// </summary>
		public bool CopiaEstruturaParaDestino(DataTable tabela)
		{
			bool resultado = true;
			try
			{				
				if (tabela != null) 
				{
					tabelaDestino = new DataTable();
					foreach(DataColumn coluna in tabela.Columns) 
					{
						DataColumn colunaNova = new  DataColumn();
						colunaNova.DataType = System.Type.GetType(coluna.DataType.FullName);
						colunaNova.ColumnName = coluna.ColumnName;
						tabelaDestino.Columns.Add(colunaNova);
					}
				} 
				else 
				{
					tabelaDestino = null;
					resultado = false;
				}
			}
			catch(Exception erro)
			{
				throw erro;
			}
			return resultado;
		}
		/// <summary>
		/// Elimina campos que tem duas últimas letras = 'ID'
		/// E copia para tabela destino
		/// </summary>
		public bool SemID()
		{
			bool resultado = true;
			try
			{				
				if (tabelaOrigem != null) 
				{
					tabelaDestino = tabelaOrigem.Copy();
					foreach (DataColumn coluna in tabelaOrigem.Columns) 
					{
						if (coluna.ColumnName.Substring(coluna.ColumnName.Length-2, 2) == "ID") 
						{
							tabelaDestino.Columns.Remove(coluna.ColumnName);
						}
					}
				} 
				else 
				{
					tabelaDestino = null;
					resultado = false;
				}
			}
			catch(Exception erro)
			{
				throw erro;
			}
			return resultado;
		}
		/// <summary>
		/// Unir origem na tabela destino
		/// As estruturas devem ser as mesmas
		/// </summary>
		/// <returns></returns>
		public bool UnirTabelas()
		{
			bool resultado = true;
			try
			{
				if (tabelaOrigem != null) 
				{
					foreach(DataRow umaLinha in tabelaOrigem.Rows) 
						tabelaDestino.ImportRow(umaLinha);
				} 
				else 
				{
					tabelaDestino = null;
					resultado = false;
				}
			}
			catch(Exception erro)
			{
				throw erro;
			}
			return resultado;
		}

		public static DataTable Intersect(DataTable First, DataTable Second)

		{

			//Get reference to Columns in First

			DataColumn[] firstcolumns  = new DataColumn[First.Columns.Count];

			for(int i = 0; i < firstcolumns.Length; i++)

			{

				firstcolumns[i] = First.Columns[i];

			}

			//Get reference to Columns in Second

			DataColumn[] secondcolumns  = new DataColumn[Second.Columns.Count];

			for(int i = 0; i < secondcolumns.Length; i++)

			{

				secondcolumns[i] = Second.Columns[i];

			}

			//JOIN ON all columns

			DataTable table = Join(First, Second, firstcolumns, secondcolumns);

			table.TableName = "Intersect";

			return table;

		}

 

		private static bool RowEqual(object[] Values, object[] OtherValues)

		{

			if(Values == null)

				return false;

 

			for(int i = 0; i < Values.Length; i++)

			{

				if(!Values[i].Equals(OtherValues[i]))

					return false;

			}                      

			return true;

		} 

 

 

		public static DataTable Distinct(DataTable Table, DataColumn[] Columns, string filtro, string order)

		{

			//Empty table

			DataTable table = new DataTable("Distinct");

			//Sort variable

			string sort = string.Empty;

 

			//Add Columns & Build Sort expression

			for(int i = 0; i < Columns.Length; i++)

			{

				table.Columns.Add(Columns[i].ColumnName,Columns[i].DataType);

				if (order.Length == 0)
					sort += Columns[i].ColumnName + ",";

			}

			if (order.Length == 0)
				sort = sort.Substring(0,sort.Length-1);
			else
				sort = order;
			

			//Select all rows and sort

            DataRow[] sortedrows = Table.Select(filtro.Length == 0 ? "1=1" : filtro, sort);



            object[] currentrow = null;

            object[] previousrow = null;

     

			table.BeginLoadData();

			foreach(DataRow row in sortedrows)

			{

				//Current row

				currentrow = new object[Columns.Length];

				for(int i = 0; i < Columns.Length; i++)

				{

					currentrow[i] = row[Columns[i].ColumnName];

				}

 

				//Match Current row to previous row

				if(!RowEqual(previousrow, currentrow))

					table.LoadDataRow(currentrow,true);

 

				//Previous row

				previousrow = new object[Columns.Length];

				for(int i = 0; i < Columns.Length; i++)

				{

					previousrow[i] = row[Columns[i].ColumnName];

				}

 

			}

			table.EndLoadData();

			return table;

 

		}

		public static DataTable Distinct(DataTable Table, DataColumn Column, string filtro)

		{

			return Distinct(Table, new DataColumn[]{Column}, filtro, string.Empty);

		}


		public static DataTable DistinctComFiltro(DataTable Table, string Column, string filtro)

		{

			return Distinct(Table, Table.Columns[Column], filtro);

		}



		public static DataTable DistinctComFiltro(DataTable Table, string filtro, params string[] Columns)

		{
			DataColumn[] columns = new DataColumn[Columns.Length];

			for(int i = 0; i < Columns.Length; i++)

			{

				columns[i] = Table.Columns[Columns[i]];

     

			}

			return Distinct(Table, columns, filtro, string.Empty);

		}

		public static DataTable Distinct(DataTable Table, string coluna)
		{
			return Distinct(Table, Table.Columns[coluna],string.Empty);
		}
		
		
		public static DataTable Distinct(DataTable Table, params string[] Columns)

		{

			DataColumn[] columns = new DataColumn[Columns.Length];

			for(int i = 0; i < Columns.Length; i++)

			{

				columns[i] = Table.Columns[Columns[i]];

     

			}

			return Distinct(Table, columns, string.Empty, string.Empty);

		}

		public static DataTable DistinctSort(DataTable Table, string sort, string filtro, params string[] Columns)

		{

			DataColumn[] columns = new DataColumn[Columns.Length];

			for(int i = 0; i < Columns.Length; i++)

			{

				columns[i] = Table.Columns[Columns[i]];

     

			}

			return Distinct(Table, columns, filtro, sort);

		}

		public static DataTable Distinct(DataTable Table)

		{

			DataColumn[] columns = new DataColumn[Table.Columns.Count];

			for(int i = 0; i < Table.Columns.Count; i++)

			{

				columns[i] = Table.Columns[i];

     

			}

			return Distinct(Table, columns, string.Empty, string.Empty);

		}

	} // fim de classe TabelaMemoria
}
