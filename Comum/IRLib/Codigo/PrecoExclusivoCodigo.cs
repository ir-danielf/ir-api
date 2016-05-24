/**************************************************
* Arquivo: PrecoExclusivoCodigo.cs
* Gerado: 11/11/2008
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Data;

namespace IRLib
{

    public class PrecoExclusivoCodigo : PrecoExclusivoCodigo_B
    {

        public PrecoExclusivoCodigo() { }

        public PrecoExclusivoCodigo(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>		
        /// Obter todos os Codigos de Precos Exclusivos
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {
            try
            {

                DataTable tabela = new DataTable("PrecoExclusivoCodigo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("DescontoID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));

                string sql = "SELECT ID, DescontoID, Codigo FROM tPrecoExclusivoCodigo ORDER BY Codigo";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["DescontoID"] = bd.LerInt("DescontoID");
                    linha["Codigo"] = bd.LerString("Codigo");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter todos os Codigos de Precos Exclusivos por Preco
        /// </summary>
        /// <returns></returns>
        public override DataTable ListagemPorPrecoExclusivo(int precoexclusivoid)
        {
            try
            {

                DataTable tabela = new DataTable("PrecoExclusivoCodigo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("DescontoID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));

                string sql = "SELECT ID, DescontoID, Codigo FROM tPrecoExclusivoCodigo WHERE DescontoID = " + precoexclusivoid + " ORDER BY Codigo";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["DescontoID"] = bd.LerInt("DescontoID");
                    linha["Codigo"] = bd.LerString("Codigo");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter todos os Codigos de Precos Exclusivos por Codigo
        /// </summary>
        /// <returns></returns>
        public override DataTable ListagemPorCodigo(int precoexclusivoid, string codigo)
        {
            try
            {

                DataTable tabela = new DataTable("PrecoExclusivoCodigo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("DescontoID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));

                string sql = "SELECT ID, DescontoID, Codigo FROM tPrecoExclusivoCodigo WHERE Codigo = '" + codigo + "' AND DescontoID = " + precoexclusivoid;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["DescontoID"] = bd.LerInt("DescontoID");
                    linha["Codigo"] = bd.LerString("Codigo");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter todos os Codigos de Precos Exclusivos por Preco e Codigo
        /// </summary>
        /// <returns></returns>
        public override DataTable ListagemPorCodigo(string codigo)
        {
            try
            {

                DataTable tabela = new DataTable("PrecoExclusivoCodigo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("DescontoID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));

                string sql = "SELECT ID, DescontoID, Codigo FROM tPrecoExclusivoCodigo WHERE Codigo = '" + codigo + "'";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["DescontoID"] = bd.LerInt("DescontoID");
                    linha["Codigo"] = bd.LerString("Codigo");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter todos os Codigos de Precos Exclusivos por Codigo
        /// </summary>
        /// <returns></returns>
        public override int InsereCodigos(int precoexclusivoid, string[] codigo)
        {
            int contadorCodigosInseridos = 0;
            BD bdConsulta = new BD();
            BD bdInserir = new BD();

            try
            {
                bdInserir.IniciarTransacao();

                foreach (string strCodigo in codigo)
                {
                    IDataReader oConsulta = bdConsulta.Consulta("SELECT ID FROM tPrecoExclusivoCodigo (NOLOCK) WHERE Codigo = '" + strCodigo + "' AND DescontoID = " + precoexclusivoid);
                    if (!oConsulta.Read())
                    {
                        bdInserir.Executar("INSERT INTO tPrecoExclusivoCodigo(DescontoID, Codigo) VALUES (" + precoexclusivoid + ", '" + strCodigo + "')");
                        contadorCodigosInseridos++;
                    }
                }

                bdInserir.FinalizarTransacao();
            }
            catch
            {
                contadorCodigosInseridos = 0;
                bdInserir.DesfazerTransacao();
            }
            finally
            {
                bdConsulta.Fechar();
                bdInserir.Fechar();
            }


            return contadorCodigosInseridos;
        }

    }

    public class PrecoExclusivoCodigoLista : PrecoExclusivoCodigoLista_B
    {

        public PrecoExclusivoCodigoLista() { }

        public PrecoExclusivoCodigoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
