/**************************************************
* Arquivo: FormaPagamentoTipo.cs
* Gerado: 28/08/2009
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace IRLib.Paralela
{

    public class FormaPagamentoTipo : FormaPagamentoTipo_B
    {

        public FormaPagamentoTipo() { }

        public FormaPagamentoTipo(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>		
        /// Obter todos os tipos de formas de pagamento
        /// </summary>
        /// <returns></returns>
        public DataTable Todas(string registroZero)
        {
            DataTable tabela = new DataTable("FormaPagamentoTipo");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {

                if (!String.IsNullOrEmpty(registroZero))
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                using (IDataReader oDataReader = bd.Consulta("SELECT ID, Nome FROM tFormaPagamentoTipo (NOLOCK) ORDER BY Nome"))
                {
                    DataRow linha;
                    while (oDataReader.Read())
                    {
                        linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        tabela.Rows.Add(linha);
                    }
                }
                bd.Fechar();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }


        public List<IRLib.Paralela.ClientObjects.EstruturaIDNome> CarregarLista(string registroZero)
        {
         
            List<IRLib.Paralela.ClientObjects.EstruturaIDNome> listaEstrutura = new List<IRLib.Paralela.ClientObjects.EstruturaIDNome>();

            try
            {
                if (!String.IsNullOrEmpty(registroZero))
                    listaEstrutura.Add(new EstruturaIDNome() { ID = 0, Nome = registroZero });
                
                using (IDataReader oDataReader = bd.Consulta("SELECT ID, Nome FROM tFormaPagamentoTipo (NOLOCK) ORDER BY Nome"))
                {
                    while (oDataReader.Read())
                    {                        
                        EstruturaIDNome oEstruturaIDNome = new EstruturaIDNome();

                        oEstruturaIDNome.ID = bd.LerInt("ID");
                        oEstruturaIDNome.Nome = bd.LerString("Nome");

                        listaEstrutura.Add(oEstruturaIDNome);
                    }
                }

                bd.Fechar();

            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return listaEstrutura;
        
        }



        public List<EstruturaIDNome> TodasList(string registroZero)
        {
            try
            {
                List<EstruturaIDNome> retorno = new List<EstruturaIDNome>();

                if (registroZero != null)
                    retorno.Add(new EstruturaIDNome { ID = 0, Nome = registroZero });

                using (IDataReader oDataReader = bd.Consulta("SELECT ID, Nome FROM tFormaPagamentoTipo (NOLOCK) ORDER BY Nome"))
                {

                    while (oDataReader.Read())
                    {
                        retorno.Add(new EstruturaIDNome { ID = bd.LerInt("ID"), Nome = bd.LerString("Nome") });
                    }
                }
                bd.Fechar();

                return retorno;
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

    public class FormaPagamentoTipoLista : FormaPagamentoTipoLista_B
    {

        public FormaPagamentoTipoLista() { }

        public FormaPagamentoTipoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
