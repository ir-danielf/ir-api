/**************************************************
* Arquivo: CanalValeIngresso.cs
* Gerado: 09/11/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Text;

namespace IRLib
{

    public class CanalValeIngresso : CanalValeIngresso_B
    {

        public CanalValeIngresso() { }

        public CanalValeIngresso(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public enum EnumAcaoCanal
        {
            Inserir = 'I',
            Remover = 'R',
            Sem_Acao = 'S'
        }
        /// <summary>
        /// Exclui CanalValeIngresso com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public bool Excluir(int id, BD bd)
        {

            try
            {

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCanalValeIngresso WHERE ID=" + id;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCanalValeIngresso WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

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

        }        /// <summary>
        /// Exclui CanalValeIngresso pelo CanalID + ValeIngressoTipoID
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public bool Excluir(int canalID,int ValeIngressoTipoID,  BD bd)
        {

            try
            {

                string sqlSelect = "SELECT ISNULL(MAX(Versao),0) AS Versao, t.ID FROM cCanalValeIngresso c (NOLOCK) "+
                                   " INNER JOIN tCanalValeIngresso t (NOLOCK) ON c.ID = t.ID "+
                                   " WHERE CanalID = "+canalID+" AND ValeIngressoTipoID = "+ValeIngressoTipoID+ " GROUP BY t.ID";
                bd.Consulta(sqlSelect);
                int versao = 0;
                int canalValeIngressoID = 0;
                while (bd.Consulta().Read())
                { 
                    versao = bd.LerInt("Versao");
                    canalValeIngressoID = bd.LerInt("ID");
                }

                this.Control.Versao = versao;
                this.Control.ID = canalValeIngressoID;
                bool result = false;
                if (this.Control.ID > 0)
                {
                    InserirControle("D");
                    InserirLog();

                    string sqlDelete = "DELETE FROM tCanalValeIngresso WHERE ID = " + canalValeIngressoID;

                    int x = bd.Executar(sqlDelete);

                     result = (x == 1);
                }
                bd.FinalizarTransacao();

                return result;

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
        public bool Inserir(BD bd)
        {
            try
            {
                this.Control.Versao = 0;

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tCanalValeIngresso(CanalID, ValeIngressoTipoID) ");
                sql.Append("VALUES (@001,@002); SELECT SCOPE_IDENTITY();");

               
                sql.Replace("@001", this.CanalID.ValorBD);
                sql.Replace("@002", this.ValeIngressoTipoID.ValorBD);

                object x = bd.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;


                if (this.Control.ID > 0)
                    InserirControle("I");
            
                bd.FinalizarTransacao();

                return this.Control.ID > 0;

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
    }

    public class CanalValeIngressoLista : CanalValeIngressoLista_B
    {

        public CanalValeIngressoLista() { }

        public CanalValeIngressoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
