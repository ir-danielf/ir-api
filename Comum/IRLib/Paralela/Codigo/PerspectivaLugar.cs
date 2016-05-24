/**************************************************
* Arquivo: PerspectivaLugar.cs
* Gerado: 24/05/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Text;
namespace IRLib.Paralela
{

    public class PerspectivaLugar : PerspectivaLugar_B
    {

        public enum EnumAcaoPerspectivaLugar
        {
            Adicionar,
            Remover,
            Manter,
            Editar,
            Nenhuma
        }

        public PerspectivaLugar() { }

        public PerspectivaLugar(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Inserir novo(a) PerspectivaLugar
        /// </summary>
        /// <returns></returns>	
        public EstruturaPerspectivaLugar InserirComRetorno()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cPerspectivaLugar");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tPerspectivaLugar(ID, SetorID, Descricao) ");
                sql.Append("VALUES (@ID,@001,'@002')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.Descricao.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return (new EstruturaPerspectivaLugar()
                {
                    ID = this.Control.ID,
                    Descricao = this.Descricao.Valor,
                    SetorID = this.SetorID.Valor,
                    Acao = EnumAcaoPerspectivaLugar.Adicionar
                });

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

        /// <summary>
        /// Atualiza PerspectivaLugar
        /// </summary>
        /// <returns></returns>	
        public EstruturaPerspectivaLugar AtualizarComRetorno()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cPerspectivaLugar WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tPerspectivaLugar SET SetorID = @001, Descricao = '@002' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.Descricao.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return (new EstruturaPerspectivaLugar()
                {
                    ID = this.Control.ID,
                    Descricao = this.Descricao.Valor,
                    SetorID = this.SetorID.Valor,
                    Acao = EnumAcaoPerspectivaLugar.Editar
                });

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



        public List<EstruturaPerspectivaLugar> CarregarPorSetor(int setorID)
        {
            try
            {
                List<EstruturaPerspectivaLugar> lstRetorno = new List<EstruturaPerspectivaLugar>();
                string sql = @"SELECT ID, Descricao FROM tPerspectivaLugar (NOLOCK) WHERE SetorID = " + setorID;
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lstRetorno.Add(new EstruturaPerspectivaLugar()
                    {
                        ID = bd.LerInt("ID"),
                        SetorID = setorID,
                        Descricao = bd.LerString("Descricao"),
                        Acao = EnumAcaoPerspectivaLugar.Nenhuma,
                    });
                }

                return lstRetorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void ExcluirItems(List<EstruturaPerspectivaLugar> Items)
        {
            try
            {
                bd.IniciarTransacao();

                for (int i = 0; i < Items.Count; i++)
                    this.Excluir(bd, Items[i].ID);

                bd.FinalizarTransacao();
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


        /// <summary>
        /// Exclui PerspectivaLugar com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cPerspectivaLugar WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "D");
                InserirLog(bd);

                string sqlDelete = "DELETE FROM tPerspectivaLugar WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void InserirControle(BD bd, string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cPerspectivaLugar (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xPerspectivaLugar (ID, Versao, SetorID, Descricao) ");
                sql.Append("SELECT ID, @V, SetorID, Descricao FROM tPerspectivaLugar WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    public class PerspectivaLugarLista : PerspectivaLugarLista_B
    {

        public PerspectivaLugarLista() { }

        public PerspectivaLugarLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
