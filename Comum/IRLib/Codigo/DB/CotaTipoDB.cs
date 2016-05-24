using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.DB
{

    #region "CotaTipo_B"
    public abstract class CotaTipo_B : BaseBD
    {
        descricao Descricao = new descricao();
        usuarioID UsuarioID = new usuarioID();

        public CotaTipo_B() { }

        public CotaTipo_B(int ususariologado)
        {
            this.Control.UsuarioID = ususariologado;
        }
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCotaTipo WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Descricao.Valor = bd.LerInt("Descricao");
                    this.UsuarioID.Valor = bd.LerInt("UsuarioID");
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirLogInsert()
        {


            try
            {

                string strSql = @"INSERT INTO tCotaTipoLog(Descricao,UsuarioID,Acao,appName,userName)
                                  SELECT tct.Descricao,tct.UsuarioID,'U',APP_NAME(),USER_NAME() FROM dbo.tCotaTipo tct WHERE tct.ID = @I";


                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tCotaTipoLog(Descricao,UsuarioID,Acao,appName,userName)");
                sql.Append("VALUES ('@001',@002,I,APP_NAME(),USER_NAME()");
                sql.Replace("@001", this.Descricao.ValorBD);
                sql.Replace("@002", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

                bd.FinalizarTransacao();

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

        protected void InserirLogUpdate()
        {

            try
            {

                string strSql = @"INSERT INTO tCotaTipoLog(Descricao,UsuarioID,Acao,appName,userName)
                                  SELECT tct.Descricao,tct.UsuarioID,'U',APP_NAME(),USER_NAME() FROM dbo.tCotaTipo tct WHERE tct.ID = @I";


                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tCotaTipoLog(Descricao,UsuarioID,Acao,appName,userName)");
                sql.Append("VALUES ('@001',@002,U,APP_NAME(),USER_NAME()");
                sql.Replace("@001", this.Descricao.ValorBD);
                sql.Replace("@002", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

                bd.FinalizarTransacao();

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

        protected void InserirLogDelete()
        {

            try
            {

                string strSql = @"INSERT INTO tCotaTipoLog(Descricao,UsuarioID,Acao,appName,userName)
                                  SELECT tct.Descricao,tct.UsuarioID,'U',APP_NAME(),USER_NAME() FROM dbo.tCotaTipo tct WHERE tct.ID = @I";


                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tCotaTipoLog(Descricao,UsuarioID,Acao,appName,userName)");
                sql.Append("VALUES ('@001',@002,D,APP_NAME(),USER_NAME()");

                sql.Replace("@001", this.Descricao.ValorBD);
                sql.Replace("@002", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

                bd.FinalizarTransacao();

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
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCotaTipo (Descricao,UsuarioID)");
                sql.Append("VALUES ('@001',@002)");

                sql.Replace("001", this.Descricao.ValorBD);
                sql.Replace("002", this.Control.UsuarioID.ToString());
                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirLogInsert();

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

        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCotaTIpo SET Descricao = '@001' ,UsuarioID = @002");
                sql.Append("WHERE ID = @ID");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Descricao.ValorBD);
                sql.Replace("@002", this.Control.UsuarioID.ToString());

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);
                if (result)
                    InserirLogUpdate();

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

        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlDelete = "DELETE FROM tCotaTipo WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);
                if (result)
                {
                    InserirLogDelete();
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



        public override void Limpar()
        {

            this.Descricao.Limpar();
            this.UsuarioID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Descricao.Desfazer();
            this.UsuarioID.Desfazer();
        }

        public class descricao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Descricao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }
        public class usuarioID : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UsuarioID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }
    #endregion
    }
}