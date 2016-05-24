/**************************************************
* Arquivo: EventoTipos.cs
* Gerado: 11/11/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class EventoTipos : EventoTipos_B
    {

        public EventoTipos() { }

        public EventoTipos(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obter todos os tipos
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {

            return null;

        }

        public List<IRLib.Paralela.ClientObjects.EstruturaEventoTipos> Carregar()
        {

            List<IRLib.Paralela.ClientObjects.EstruturaEventoTipos> Lista = new List<IRLib.Paralela.ClientObjects.EstruturaEventoTipos>();
            StringBuilder stbSQL = new StringBuilder();
            try
            {
                stbSQL.Append("SELECT tEventoTipo.ID AS TipoID, tEventoTipo.Nome AS Tipo, ");
                stbSQL.Append("tEventoSubtipo.ID as SubtipoID, tEventoSubtipo.Descricao AS Subtipo ");
                stbSQL.Append("FROM tEventoTipo ");
                stbSQL.Append("INNER JOIN tEventoSubtipo ON tEventoTipo.ID = tEventoSubtipo.EventoTipoID ");
                stbSQL.Append("ORDER BY tEventoTipo.ID");

                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    IRLib.Paralela.ClientObjects.EstruturaEventoTipos Item = new IRLib.Paralela.ClientObjects.EstruturaEventoTipos();
                    Item.TipoID = bd.LerInt("TipoID");
                    Item.Tipo = bd.LerString("Tipo");
                    Item.SubtipoID = bd.LerInt("SubtipoID");
                    Item.Subtipo = bd.LerString("Subtipo");
                    Item.Adicionar = false;
                    Lista.Add(Item);
                }
                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<IRLib.Paralela.ClientObjects.EstruturaEventoTipos> CarregarSelecionados(int EventoID)
        {

            List<IRLib.Paralela.ClientObjects.EstruturaEventoTipos> Lista = new List<IRLib.Paralela.ClientObjects.EstruturaEventoTipos>();
            StringBuilder stbSQL = new StringBuilder();
            try
            {
                stbSQL.Append("SELECT ID, TipoID, SubtipoID ");
                stbSQL.Append("FROM tEventoTipos ");
                stbSQL.Append("WHERE EventoID = " + this.EventoID + " ");
                stbSQL.Append("ORDER BY ID");

                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    IRLib.Paralela.ClientObjects.EstruturaEventoTipos Item = new IRLib.Paralela.ClientObjects.EstruturaEventoTipos();
                    Item.TipoID = bd.LerInt("TipoID");
                    Item.SubtipoID = bd.LerInt("SubtipoID");
                    Item.Adicionar = true;
                    Lista.Add(Item);
                }
                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable CarregarTodosTipos()
        {
            DataTable dttConsulta = this.EstruturaDataTable();
            DataRow dtr;
            StringBuilder stbSQL = new StringBuilder();
            try
            {
                stbSQL.Append("SELECT tEventoTipo.ID AS TipoID, tEventoTipo.Nome AS Tipo, ");
                stbSQL.Append("tEventoSubtipo.ID as SubtipoID, tEventoSubtipo.Descricao AS Subtipo ");
                stbSQL.Append("FROM tEventoTipo ");
                stbSQL.Append("INNER JOIN tEventoSubtipo ON tEventoTipo.ID = tEventoSubtipo.EventoTipoID ");
                stbSQL.Append("ORDER BY tEventoTipo.ID");

                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    dtr = dttConsulta.NewRow();
                    dtr["TipoID"] = bd.LerInt("TipoID");
                    dtr["Tipo"] = bd.LerString("Tipo");
                    dtr["SubtipoID"] = bd.LerInt("SubtipoID");
                    dtr["Subtipo"] = bd.LerString("Subtipo");
                    dtr["Adicionar"] = true;
                    dttConsulta.Rows.Add(dtr);
                }
                return dttConsulta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable CarregarTodosTiposSelecionados(int EventoID)
        {
            DataTable dttConsulta = this.EstruturaDataTable();
            DataRow dtr;
            StringBuilder stbSQL = new StringBuilder();
            try
            {
                stbSQL.Append("Select ");
                stbSQL.Append("tEventoTipo.ID AS TipoID, ");
                stbSQL.Append("tEventoTipo.Nome AS Categoria, ");
                stbSQL.Append("tEventoSubtipo.ID as SubtipoID, ");
                stbSQL.Append("tEventoSubtipo.Descricao AS Gênero, ");
                stbSQL.Append("IsNull(tEventoTipos.ID,0) AS ID ");
                stbSQL.Append("FROM tEventoSubtipo ");
                stbSQL.Append("INNER JOIN tEventoTipo (NOLOCK) ON tEventoTipo.ID = tEventoSubtipo.EventoTipoID ");
                stbSQL.Append("LEFT JOIN tEventoTipos (NOLOCK) ON tEventoTipos.EventoID = " + EventoID + " ");
                stbSQL.Append("AND tEventoTipos.EventoTipoID = tEventoTipo.ID ");
                stbSQL.Append("AND tEventoTipos.EventoSubtipoID = tEventoSubTipo.ID ");

                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    dtr = dttConsulta.NewRow();
                    dtr["ID"] = bd.LerInt("ID");
                    dtr["TipoID"] = bd.LerInt("TipoID");
                    dtr["Categoria"] = bd.LerString("Categoria");
                    dtr["SubtipoID"] = bd.LerInt("SubtipoID");
                    dtr["Gênero"] = bd.LerString("Gênero");
                    if (bd.LerInt("ID") == 0)
                    {
                        dtr["Adicionar"] = false;
                        dtr["AdicionarAntigo"] = false;
                    }
                    else
                    {
                        dtr["Adicionar"] = true;
                        dtr["AdicionarAntigo"] = true;
                    }
                    dttConsulta.Rows.Add(dtr);
                }
                return dttConsulta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable EstruturaDataTable()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("ID", typeof(int));
            dtt.Columns.Add("TipoID", typeof(int));
            dtt.Columns.Add("Categoria", typeof(string));
            dtt.Columns.Add("SubtipoID", typeof(int));
            dtt.Columns.Add("Gênero", typeof(string));
            dtt.Columns.Add("Adicionar", typeof(bool));
            dtt.Columns.Add("AdicionarAntigo", typeof(bool));
            return dtt;
        }

        public bool Inserir(BD bd)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tEventoTipos");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEventoTipos(ID,EventoID, EventoTipoID, EventoSubtipoID) ");
                sql.Append("VALUES (@ID,@001,@002, @003)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.Valor.ToString());
                sql.Replace("@002", this.EventoTipoID.Valor.ToString());
                sql.Replace("@003", this.EventoSubtipoID.Valor.ToString());


                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                //if (result)
                //    InserirControle("I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 Consultar(BD bd)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT ID FROM tEventoTipos WHERE ");
                stbSQL.Append("EventoID =" + this.EventoID.Valor);
                stbSQL.Append(" AND EventoSubtipoID =" + this.EventoSubtipoID.Valor);
                stbSQL.Append(" AND EventoTipoID = " + this.EventoTipoID.Valor);

                return Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 ConsultarQuantidadeTotal(BD bd)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Count(ID) FROM tEventoTipos WHERE ");
                stbSQL.Append("EventoID =" + this.EventoID.Valor);
                return Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool Excluir(BD bd)
        {
            try
            {
                //string sqlSelect = "SELECT MAX(Versao) FROM tEventoTipos WHERE ID=" + this.Control.ID;
                //object obj = bd.ConsultaValor(sqlSelect);
                //int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                //this.Control.Versao = versao;

                //InserirControle("D");
                //InserirLog();

                string sqlDelete = "DELETE FROM tEventoTipos WHERE ID=" + this.Control.ID;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Atualizar(List<EstruturaEventoTipos> tiposSalvar, List<EstruturaEventoTipos> tiposRemover)
        {
            try
            {
                EventoTipos oTipos;
                bd.IniciarTransacao();
                foreach (EstruturaEventoTipos eTipo in tiposSalvar)
                {
                    oTipos = new EventoTipos(this.Control.UsuarioID);
                    oTipos.EventoID.Valor = eTipo.EventoID;
                    oTipos.EventoSubtipoID.Valor = eTipo.SubtipoID;
                    oTipos.EventoTipoID.Valor = eTipo.TipoID;
                    //Int32 ok =oTipos.Consultar(bd);
                    //if (ok == 0)
                    if (!oTipos.Inserir(bd))
                        throw new EventoTiposException("Erro ao Incluir o Tipo/Subtipo");
                }
                foreach (EstruturaEventoTipos eTipo in tiposRemover)
                {
                    oTipos = new EventoTipos(this.Control.UsuarioID);
                    oTipos.Control.ID = eTipo.ID;
                    oTipos.EventoID.Valor = eTipo.EventoID;
                    oTipos.EventoSubtipoID.Valor = eTipo.SubtipoID;
                    oTipos.EventoTipoID.Valor = eTipo.TipoID;
                    //Int32 ok = oTipos.Consultar(bd);
                    //if (ok != 0)
                    if (!oTipos.Excluir(bd))
                        throw new EventoTiposException("Erro ao Excluir o Tipo/Subtipo");
                }
                //int quantidade = this.ConsultarQuantidadeTotal(bd);
                //bool result = (quantidade > 0);
                //if (!result)
                //    throw new EventoTiposException("O Evento selecionado deve conter pelo menos Um Tipo/Subtipo selecionado");

                bd.FinalizarTransacao();
                return true;
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

        public bool InserirControle(BD bd, string acao)
        {
            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cEventoTipos (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InserirLog(BD bd)
        {
            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xEventoTipos (ID, Versao, EventoID, EventoTipoID, EventoSubtipoID) ");
                sql.Append("SELECT ID, @V, EventoID, EventoTipoID, EventoSubtipoID FROM tEventoTipos WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                int x = bd.Executar(sql.ToString());
                bool result = (x == 1);
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable EstruturaInserir()
        {
            DataTable dtt = new DataTable("Tipos");
            dtt.Columns.Add("EventoID");
            dtt.Columns.Add("TipoID");
            dtt.Columns.Add("SubtipoID");
            dtt.Columns.Add("Adicionar");
            return dtt;
        }
    }

    public class EventoTiposLista : EventoTiposLista_B
    {

        public EventoTiposLista() { }

        public EventoTiposLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
