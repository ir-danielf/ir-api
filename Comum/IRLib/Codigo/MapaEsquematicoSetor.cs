/**************************************************
* Arquivo: MapaEsquematicoSetor.cs
* Gerado: 27/05/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Text;

namespace IRLib
{

    public class MapaEsquematicoSetor : MapaEsquematicoSetor_B
    {


        public MapaEsquematicoSetor() { }

        public MapaEsquematicoSetor(int usuarioIDLogado) : base(usuarioIDLogado) { }


        protected void InserirControle(BD bd, string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cMapaEsquematicoSetor (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xMapaEsquematicoSetor (ID, Versao, MapaID, SetorID, Coordenadas) ");
                sql.Append("SELECT ID, @V, MapaID, SetorID, Coordenadas FROM tMapaEsquematicoSetor WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) MapaEsquematicoSetor
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cMapaEsquematicoSetor");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tMapaEsquematicoSetor(ID, MapaID, SetorID, Coordenadas) ");
                sql.Append("VALUES (@ID,@001,@002,'@003')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.MapaID.ValorBD);
                sql.Replace("@002", this.SetorID.ValorBD);
                sql.Replace("@003", this.Coordenadas.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle(bd, "I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Atualiza MapaEsquematicoSetor
        /// </summary>
        /// <returns></returns>	
        public bool Atualizar(BD bd)
        {

            try
            {
                string sqlVersion = "SELECT MAX(Versao) FROM cMapaEsquematicoSetor WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "U");
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tMapaEsquematicoSetor SET MapaID = @001, SetorID = @002, Coordenadas = '@003' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.MapaID.ValorBD);
                sql.Replace("@002", this.SetorID.ValorBD);
                sql.Replace("@003", this.Coordenadas.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui MapaEsquematicoSetor com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public  bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cMapaEsquematicoSetor WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "D");
                InserirLog(bd);

                string sqlDelete = "DELETE FROM tMapaEsquematicoSetor WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class MapaEsquematicoSetorLista : MapaEsquematicoSetorLista_B
    {

        public MapaEsquematicoSetorLista() { }

        public MapaEsquematicoSetorLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
