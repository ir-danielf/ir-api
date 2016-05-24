/**************************************************
* Arquivo: ContratoPapel.cs
* Gerado: 16/03/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib
{

    public class ContratoPapel : ContratoPapel_B
    {

        public ContratoPapel() { }

        public ContratoPapel(int usuarioIDLogado) : base(usuarioIDLogado) { }

        #region Métodos de Manipulação do Contrato Papel

        #region Inserir
        /// <summary>
        /// Inserir novo(a) ContratoPapel
        /// </summary>
        /// <returns></returns>	
        internal bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cContratoPapel");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tContratoPapel(ID, ContratoID, CanalTipoID, IngressoNormalValor, PreImpressoValor, CortesiaValor) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ContratoID.ValorBD);
                sql.Replace("@002", this.CanalTipoID.ValorBD);
                sql.Replace("@003", this.IngressoNormalValor.ValorBD);
                sql.Replace("@004", this.PreImpressoValor.ValorBD);
                sql.Replace("@005", this.CortesiaValor.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Atualizar
        /// <summary>
        /// Atualiza ContratoPapel
        /// </summary>
        /// <returns></returns>	
        internal bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cContratoPapel WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tContratoPapel SET ContratoID = @001, CanalTipoID = '@002', IngressoNormalValor = '@003', PreImpressoValor = '@004', CortesiaValor = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ContratoID.ValorBD);
                sql.Replace("@002", this.CanalTipoID.ValorBD);
                sql.Replace("@003", this.IngressoNormalValor.ValorBD);
                sql.Replace("@004", this.PreImpressoValor.ValorBD);
                sql.Replace("@005", this.CortesiaValor.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Excluir

        /// <summary>
        /// Exclui ContratoPapel com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        internal bool Excluir(int id, BD bd)
        {

            try
            {

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cContratoPapel WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tContratoPapel WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool Excluir(BD bd)
        {
            return Excluir(this.Control.ID, bd);
        }

        #endregion

        #region Controle e Log

        protected void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cContratoPapel (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xContratoPapel (ID, Versao, ContratoID, CanalTipoID, IngressoNormalValor, PreImpressoValor, CortesiaValor) ");
                sql.Append("SELECT ID, @V, ContratoID, CanalTipoID, IngressoNormalValor, PreImpressoValor, CortesiaValor FROM tContratoPapel WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #endregion

        /// <summary>
        /// Captura os papeis de um contrato
        /// </summary>
        /// <param name="contratoID">Contrato ID</param>
        /// <returns></returns>
        public List<ClientObjects.EstruturaContratoPapel> carregaPapeis(int contratoID) 
        {
            List<ClientObjects.EstruturaContratoPapel> papeis = new List<IRLib.ClientObjects.EstruturaContratoPapel>();

            try 
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " + 
                    "	tContratoPapel.ID, " + 
                    "	tCanalTipo.ID AS CanalTipoID, " + 
                    "	tCanalTipo.Nome AS CanalTipoNome, " + 
                    "	tContratoPapel.IngressoNormalValor, " + 
                    "	tContratoPapel.PreImpressoValor, " + 
                    "	tContratoPapel.CortesiaValor  " + 
                    "FROM " + 
                    "	tCanalTipo " + 
                    "LEFT OUTER JOIN " + 
                    "	tContratoPapel " + 
                    "ON " + 
                    "	tContratoPapel.CanalTipoID = tCanalTipo.ID " + 
                    "AND " +
                    "	tContratoPapel.ContratoID = " + contratoID))
                {
                    ClientObjects.EstruturaContratoPapel papel;

                    while (oDataReader.Read())
                    {
                        papel = new IRLib.ClientObjects.EstruturaContratoPapel();

                        papel.ID = bd.LerInt("ID");
                        papel.CanalTipoID = bd.LerInt("CanalTipoID");
                        papel.CanalTipoNome = bd.LerString("CanalTipoNome");
                        papel.IngressoNormalValor = bd.LerDecimal("IngressoNormalValor");
                        papel.PreImpressoValor = bd.LerDecimal("PreImpressoValor");
                        papel.CortesiaValor = bd.LerDecimal("CortesiaValor");

                        papeis.Add(papel);
                    }
                }

                bd.Fechar();
            }
            catch 
            {

            }
            finally 
            {
                bd.Fechar();
            }

            return papeis;
        }

	

    }

    public class ContratoPapelLista : ContratoPapelLista_B
    {

        public ContratoPapelLista() { }

        public ContratoPapelLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
