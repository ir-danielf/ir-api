/**************************************************
* Arquivo: Caixa.cs
* Gerado: 10/05/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Data;

namespace IRLib
{

    public class Caixa : Caixa_B
    {

        public Caixa() { }

        public Caixa(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>		
        /// Retorna true se este usuario tem caixa em aberto, retorna false caso contrario.
        /// </summary>
        /// <returns></returns>
        public override bool Aberto(int usuarioid)
        {
            return this.Aberto(usuarioid, -1);
        }

        /// <summary>		
        /// Retorna true se este usuario tem caixa em aberto nessa loja, retorna false caso contrario.
        /// </summary>
        /// <returns></returns>
        public override bool Aberto(int usuarioid, int lojaid)
        {
            try
            {
                string sql = "SELECT * FROM tCaixa (NOLOCK) WHERE DataFechamento = '' AND UsuarioID = " + usuarioid;

                // Reutilização do método. Posso usar a busca tanto para usuario quanto para usuario e loja.
                if (lojaid > -1)
                    sql += "AND LojaID = " + lojaid;

                bd.Consulta(sql);

                // Verifica se existe caixa aberto para este usuário.
                if (!bd.Consulta().Read())
                {
                    bd.Fechar();
                    this.Control.ID = 0;
                    this.UsuarioID.Valor = usuarioid;
                    this.LojaID.Valor = lojaid;
                    this.ApresentacaoID.Valor = 0;
                    this.SaldoInicial.Valor = 0;
                    this.Comissao.Valor = 0;

                    return false; // Não existe caixa aberto.
                }
                else
                {
                    // Existe!
                    // Popula o objeto com informações do banco.
                    this.Control.ID = bd.LerInt("ID");
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.SaldoInicial.ValorBD = bd.LerDecimal("SaldoInicial").ToString();
                    this.DataAbertura.ValorBD = bd.LerString("DataAbertura");
                    this.DataFechamento.ValorBD = bd.LerString("DataFechamento");
                    this.Comissao.ValorBD = bd.LerInt("Comissao").ToString();

                    bd.Fechar();
                    return this.Control.ID > 0; // Retorna true 
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar(); // Sempre fechará a conexão.
            }
        }

        public bool VerificaCaixaAntigo(int usuarioid, int perfilid, int canalid)
        {
            string sql = "PR_VERIFICA_PERFIL_CAIXA " + usuarioid;

            bd.Consulta(sql);

            // Verifica se existe caixa aberto para este usuário.
            if (!bd.Consulta().Read() || perfilid == Perfil.SAC_SUPERVISOR_NOVO)
            {
                return true; // Caixa deve ser fechado
            }
            else
            {
                return false;
            }
        }

        public bool deveFecharCaixa(int usuarioid)
        {
            string sql = "VerificaUltimoCaixa " + usuarioid;
            bool retorno = false;
            bd.Consulta(sql);            

            string DataAbertura = "";
            string DataFechamento = "";
            string DataAtual = "";

            if (bd.Consulta().Read())
            {
                DataAbertura = bd.LerString("DataAbertura");
                DataFechamento = bd.LerString("DataFechamento");
                DataAtual = bd.LerString("DataAtual").Substring(0,8) + "000000";
            }

            if (DataFechamento == "")
            {
                if (DataAbertura != "")
                {
                    if (Convert.ToInt32(DataAbertura) < Convert.ToInt32(DataAtual))
                    {
                        retorno = true;
                    }    
                }                
            }

            return retorno;

        }

        /// <summary>
        /// carrega os dados para os relatórios: Caixa Resumo e Caixa Detalhes. 
        /// Os caixas somente de 6 meses e os usuários com status ='L'
        /// </summary>
        /// <param name="empresaID"></param>
        /// <returns></returns>
        public DataSet carregarLocalCaixas(int localID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                DateTime seisMeses = DateTime.Now.AddMonths(-6);

                string sql = "SELECT DISTINCT tCanal.ID AS CanalID, tCanal.Nome AS Canal " +
                    "FROM tCanal (NOLOCK), tLoja (NOLOCK), tCaixa (NOLOCK), tUsuario (NOLOCK), tLocal (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tLocal.EmpresaID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tLocal.ID=" + localID + " " +
                    "ORDER BY tCanal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow canal = canais.NewRow();
                    canal["ID"] = bd.LerInt("CanalID");
                    canal["Nome"] = bd.LerString("Canal");
                    canais.Rows.Add(canal);
                }

                sql = "SELECT DISTINCT tCanal.ID AS CanalID, tLoja.ID AS LojaID, tLoja.Nome AS Loja " +
                    "FROM tCanal (NOLOCK), tLoja (NOLOCK), tCaixa (NOLOCK), tUsuario (NOLOCK), tLocal (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tLocal.EmpresaID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tLocal.ID=" + localID + " " +
                    "ORDER BY tLoja.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);
                }

                sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario " +
                    "FROM tCanal (NOLOCK), tLoja (NOLOCK), tCaixa (NOLOCK), tUsuario (NOLOCK), tLocal (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tLocal.EmpresaID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tLocal.ID=" + localID + " AND tCaixa.DataAbertura > '" + seisMeses.ToString("yyyyMMddHHmmss") + "' " +
                    "ORDER BY tUsuario.Nome";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {

                    DataRow usuario = usuarios.NewRow();
                    usuario["ID"] = bd.LerInt("UsuarioID");
                    usuario["Nome"] = bd.LerString("Usuario");
                    usuario["LojaID"] = bd.LerInt("LojaID");
                    usuarios.Rows.Add(usuario);
                }



                sql = "SELECT DISTINCT tCanal.ID AS CanalID, tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID,SUBSTRING(tCaixa.DataAbertura,1,8) AS DataAbertura " +
                    "FROM tCanal (NOLOCK), tLoja (NOLOCK), tCaixa (NOLOCK), tUsuario (NOLOCK), tLocal (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tLocal.EmpresaID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tLocal.ID=" + localID + " AND tCaixa.DataAbertura > '" + seisMeses.ToString("yyyyMMddHHmmss") + "' " +
                    "GROUP BY tCanal.ID , tLoja.ID ,tUsuario.ID,tCaixa.DataAbertura " +
                    "ORDER BY  SUBSTRING(tCaixa.DataAbertura,1,8) DESC";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerString("DataAbertura");
                    caixa["Nome"] = bd.LerStringFormatoData("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);
                }

                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);

                return buffer;

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

        public DataSet carregarLocalCaixasSemFiltro(int localID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                string sql = "SELECT DISTINCT tCanal.ID AS CanalID, tCanal.Nome AS Canal " +
                    "FROM tCanal (NOLOCK), tLoja (NOLOCK), tCaixa (NOLOCK), tUsuario (NOLOCK), tLocal (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tLocal.EmpresaID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tLocal.ID=" + localID + " " +
                    "ORDER BY tCanal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow canal = canais.NewRow();
                    canal["ID"] = bd.LerInt("CanalID");
                    canal["Nome"] = bd.LerString("Canal");
                    canais.Rows.Add(canal);
                }

                sql = "SELECT DISTINCT tCanal.ID AS CanalID, tLoja.ID AS LojaID, tLoja.Nome AS Loja " +
                    "FROM tCanal (NOLOCK), tLoja (NOLOCK), tCaixa (NOLOCK), tUsuario (NOLOCK), tLocal (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tLocal.EmpresaID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tLocal.ID=" + localID + " " +
                    "ORDER BY tLoja.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);
                }

                sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario " +
                    "FROM tCanal (NOLOCK), tLoja (NOLOCK), tCaixa (NOLOCK), tUsuario (NOLOCK), tLocal (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tLocal.EmpresaID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tLocal.ID=" + localID + " " +
                    "ORDER BY tUsuario.Nome";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {

                    DataRow usuario = usuarios.NewRow();
                    usuario["ID"] = bd.LerInt("UsuarioID");
                    usuario["Nome"] = bd.LerString("Usuario");
                    usuario["LojaID"] = bd.LerInt("LojaID");
                    usuarios.Rows.Add(usuario);
                }



                sql = "SELECT DISTINCT tCanal.ID AS CanalID, tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                    "FROM tCanal (NOLOCK), tLoja (NOLOCK), tCaixa (NOLOCK), tUsuario (NOLOCK), tLocal (NOLOCK) " +
                    "WHERE tCanal.EmpresaID=tLocal.EmpresaID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tLocal.ID=" + localID + " " +
                    "ORDER BY tCaixa.ID DESC";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);
                }

                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);

                return buffer;

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

        /// <summary>
        /// carrega os dados para os relatórios: Caixa Resumo e Caixa Detalhes. 
        /// Os caixas somente de 6 meses e os usuários com status ='L'
        /// </summary>
        /// <param name="empresaID"></param>
        /// <returns></returns>
        public DataSet carregarCanalCaixas(int canalID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));
                DateTime seisMeses = DateTime.Now.AddMonths(-6);

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));

                //string sql = "SELECT DISTINCT tLoja.ID AS LojaID, tLoja.Nome AS Loja, tLoja.CanalID " +
                //    "FROM tLoja,tCaixa,tUsuario " +
                //    "WHERE tLoja.CanalID=" + canalID + " AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID " +
                //    "ORDER BY tLoja.Nome";

                string sql = "dbo.sp_getCanalCaixas @CanalID = " + canalID + ", @Meses = '" + seisMeses.ToString("yyyyMMddHHmmss") + "', @Tipo = 'Loja' ";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);

                }
                //sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario " +
                //            "FROM tLoja,tCaixa,tUsuario " +
                //            "WHERE tLoja.CanalID=" + canalID + " AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tCaixa.DataAbertura > '" + seisMeses.ToString("yyyyMMddHHmmss") + "' " +
                //            "ORDER BY tUsuario.Nome";

                sql = "dbo.sp_getCanalCaixas @CanalID = " + canalID + ", @Meses = '" + seisMeses.ToString("yyyyMMddHHmmss") + "', @Tipo = 'Usuario' ";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow usuario = usuarios.NewRow();
                    usuario["ID"] = bd.LerInt("UsuarioID");
                    usuario["Nome"] = bd.LerString("Usuario");
                    usuario["LojaID"] = bd.LerInt("LojaID");
                    usuarios.Rows.Add(usuario);

                }

                //sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                //             "FROM tLoja,tCaixa,tUsuario " +
                //             "WHERE tLoja.CanalID=" + canalID + " AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID " + " AND tCaixa.DataAbertura > '" + seisMeses.ToString("yyyyMMddHHmmss") + "' " +
                //             "ORDER BY tCaixa.DataAbertura DESC, tCaixa.ID DESC";

                sql = "dbo.sp_getCanalCaixas @CanalID = " + canalID + ", @Tipo = 'Caixa' ";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoData("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);
                }
                //sql = "SELECT DISTINCT tCanal.ID AS CanalID, tCanal.Nome AS Canal " +
                //       "FROM tCanal,tLoja,tCaixa,tUsuario " +
                //       "WHERE  tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tCanal.ID=" + canalID + " " +
                //       "ORDER BY tCanal.Nome";

                sql = "dbo.sp_getCanalCaixas @CanalID = " + canalID + ", @Tipo = 'Canal' ";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow canal = canais.NewRow();
                    canal["ID"] = bd.LerInt("CanalID");
                    canal["Nome"] = bd.LerString("Canal");
                    canais.Rows.Add(canal);
                }

                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);
                buffer.Tables.Add(canais);

                return buffer;

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

        public DataSet carregarCanalCaixasSemFiltro(int canalID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));

                //string sql = "SELECT DISTINCT tLoja.ID AS LojaID, tLoja.Nome AS Loja, tLoja.CanalID " +
                //    "FROM tLoja,tCaixa,tUsuario " +
                //    "WHERE tLoja.CanalID=" + canalID + " AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID " +
                //    "ORDER BY tLoja.Nome";

                string sql = "sp_getCanalCaixasSemFiltro @CanalID = " + canalID + ", @Tipo = 'Loja'";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);

                }
                //sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario " +
                //            "FROM tLoja,tCaixa,tUsuario " +
                //            "WHERE tLoja.CanalID=" + canalID + " AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID " +
                //            "ORDER BY tUsuario.Nome";

                sql = "sp_getCanalCaixasSemFiltro @CanalID = " + canalID + ", @Tipo = 'Usuario'";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow usuario = usuarios.NewRow();
                    usuario["ID"] = bd.LerInt("UsuarioID");
                    usuario["Nome"] = bd.LerString("Usuario");
                    usuario["LojaID"] = bd.LerInt("LojaID");
                    usuarios.Rows.Add(usuario);

                }

                //sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                //             "FROM tLoja,tCaixa,tUsuario " +
                //             "WHERE tLoja.CanalID=" + canalID + " AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID " +
                //             "ORDER BY tCaixa.ID DESC";

                sql = "sp_getCanalCaixasSemFiltro @CanalID = " + canalID + ", @Tipo = 'Caixa'";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);
                }
                //sql = "SELECT DISTINCT tCanal.ID AS CanalID, tCanal.Nome AS Canal " +
                //       "FROM tCanal,tLoja,tCaixa,tUsuario " +
                //       "WHERE  tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tCanal.ID=" + canalID + " " +
                //       "ORDER BY tCanal.Nome";

                sql = "sp_getCanalCaixasSemFiltro @CanalID = " + canalID + ", @Tipo = 'Canal'";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow canal = canais.NewRow();
                    canal["ID"] = bd.LerInt("CanalID");
                    canal["Nome"] = bd.LerString("Canal");
                    canais.Rows.Add(canal);
                }

                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);
                buffer.Tables.Add(canais);

                return buffer;

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

        /// <summary>
        /// carrega os dados para os relatórios: Caixa Resumo e Caixa Detalhes. 
        /// Os caixas somente de 6 meses e os usuários com status ='L'
        /// </summary>
        /// <param name="empresaID"></param>
        /// <returns></returns>
        public DataSet carregarUsuarioCaixas(int canalID, int usuarioID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));

                DateTime seisMeses = DateTime.Now.AddMonths(-6);

                //string sql = "SELECT tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                //    "FROM tLoja,tCaixa " +
                //    "WHERE tLoja.CanalID=" + canalID + " AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=" + usuarioID + " " + " AND tCaixa.DataAbertura > '" + seisMeses.ToString("yyyyMMddHHmmss") + "' " +
                //    "ORDER BY tCaixa.DataAbertura DESC, tCaixa.ID DESC";
                string sql = "sp_getCaixasPorUsuario @CanalID = " + canalID + ", @UsuarioID = " + usuarioID + ", @Meses = '" + seisMeses.ToString("yyyyMMddHHmmss") + "' ";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoData("DataAbertura");
                    caixas.Rows.Add(caixa);

                }
                bd.Fechar();

                buffer.Tables.Add(caixas);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet carregarUsuarioCaixasSemFiltro(int canalID, int usuarioID)
        {
            try
            {
                DataSet buffer = new DataSet("Buffer");

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                string sql = "SELECT DISTINCT tLoja.ID AS LojaID, tLoja.Nome AS Loja, tLoja.CanalID " +
                                    "FROM tLoja,tCaixa,tUsuario " +
                                    "WHERE tLoja.CanalID=" + canalID + " AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID= " + usuarioID + " " +
                                    "ORDER BY tLoja.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);

                }

                bd.Fechar();

                buffer.Tables.Add(lojas);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet carregarUsuarioCaixas(int usuarioID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));

                DateTime seisMeses = DateTime.Now.AddMonths(-6);

                string sql = "SELECT tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                    "FROM tLoja,tCaixa " +
                    "WHERE tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=" + usuarioID + " " + " AND tCaixa.DataAbertura > '" + seisMeses.ToString("yyyyMMddHHmmss") + "' " +
                    "ORDER BY tCaixa.ID DESC";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixas.Rows.Add(caixa);

                }
                bd.Fechar();

                buffer.Tables.Add(caixas);

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable carregarUsuarioCaixas(int usuarioID, int LojaID, DateTime periodoInicial, DateTime periodoFinal)
        {
            try
            {
                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Usuario", typeof(string));
                caixas.Columns.Add("Loja", typeof(string));
                caixas.Columns.Add("Canal", typeof(string));
                caixas.Columns.Add("DataAbertura", typeof(string));
                caixas.Columns.Add("DataFechamento", typeof(string));

                string sql = string.Format(@"SELECT tc.ID AS CaixaID, tu.Nome AS Usuario, tl.Nome AS Loja, tcl.Nome AS Canal, tc.DataAbertura, tc.DataFechamento
                FROM tCaixa AS tc (NOLOCK)
                INNER JOIN tLoja AS tl (NOLOCK) ON tl.ID = tc.LojaID
                INNER JOIN tCanal AS tcl (NOLOCK) ON tcl.ID = tl.CanalID
                INNER JOIN tUsuario AS tu (NOLOCK) ON tu.ID = tc.UsuarioID
                WHERE tc.LojaID = tl.ID 
                AND tc.UsuarioID = {0} 
                AND tl.ID = {1}
                AND tc.DataAbertura > '{2}' AND tc.DataAbertura < '{3}'
                ORDER BY tc.ID DESC", usuarioID, LojaID, periodoInicial.ToString("yyyyMMddHHmm"), periodoFinal.ToString("yyyyMMddHHmm"));

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Usuario"] = bd.LerString("Usuario");
                    caixa["Loja"] = bd.LerString("Loja");
                    caixa["Canal"] = bd.LerString("Canal");
                    caixa["DataAbertura"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixa["DataFechamento"] = bd.LerStringFormatoSemanaDataHora("DataFechamento");
                    caixas.Rows.Add(caixa);
                }

                return caixas;
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

        /// <summary>
        /// Carrega Detalhes do Caixa
        /// </summary>
        /// <param name="vendaBilheteriaID"></param>
        /// <returns></returns>
        public DataTable carregaDetalhes(int caixaID)
        {

            DataTable tabela = new DataTable("CaixaDetalhe");

            tabela.Columns.Add("CaixaID", typeof(int));
            tabela.Columns.Add("LojaID", typeof(int));
            tabela.Columns.Add("CanalID", typeof(int));
            tabela.Columns.Add("EmpresaID", typeof(int));
            tabela.Columns.Add("RegionalID", typeof(int));
            tabela.Columns.Add("SaldoInicial", typeof(decimal));
            tabela.Columns.Add("Comissao", typeof(int));
            tabela.Columns.Add("DataAbertura", typeof(string));
            tabela.Columns.Add("DataFechamento", typeof(string));
            tabela.Columns.Add("LojaNome", typeof(string));
            tabela.Columns.Add("CanalNome", typeof(string));
            tabela.Columns.Add("EmpresaNome", typeof(string));

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tCaixa.ID AS CaixaID, " +
                    "   tLoja.ID AS LojaID, " +
                    "   tCanal.ID AS CanalID, " +
                    "   tEmpresa.ID AS EmpresaID, " +
                    "   tEmpresa.RegionalID, " +
                    "   tCaixa.SaldoInicial, " +
                    "   tCaixa.DataAbertura, " +
                    "   tCaixa.DataFechamento, " +
                    "   tCaixa.Comissao, " +
                    "   tLoja.Nome AS LojaNome, " +
                    "   tCanal.Nome AS CanalNome, " +
                    "   tEmpresa.Nome AS EmpresaNome " +
                    "FROM " +
                    "   tLoja " +
                    "INNER JOIN " +
                    "   tCaixa " +
                    "ON " +
                    "   tLoja.ID = tCaixa.LojaID " +
                    "INNER JOIN " +
                    "   tCanal " +
                    "ON " +
                    "   tLoja.CanalID = tCanal.ID " +
                    "INNER JOIN " +
                    "   tEmpresa " +
                    "ON " +
                    "   tCanal.EmpresaID = tEmpresa.ID " +
                    "WHERE " +
                    "   tCaixa.ID = " + caixaID))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["CaixaID"] = bd.LerInt("CaixaID");
                        linha["LojaID"] = bd.LerInt("LojaID");
                        linha["CanalID"] = bd.LerInt("CanalID");
                        linha["EmpresaID"] = bd.LerInt("EmpresaID");
                        linha["RegionalID"] = bd.LerInt("RegionalID");
                        linha["SaldoInicial"] = bd.LerDecimal("SaldoInicial");
                        linha["Comissao"] = bd.LerInt("Comissao");
                        linha["DataAbertura"] = bd.LerDateTime("DataAbertura");
                        linha["DataFechamento"] = bd.LerDateTime("DataFechamento");
                        linha["LojaNome"] = bd.LerString("LojaNome");
                        linha["CanalNome"] = bd.LerString("CanalNome");
                        linha["EmpresaNome"] = bd.LerString("EmpresaNome");
                        tabela.Rows.Add(linha);
                    }
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }

        /// <summary>
        /// carrega os dados para os relatórios: Caixa Resumo e Caixa Detalhes. 
        /// Os caixas somente de 6 meses e os usuários com status ='L'
        /// </summary>
        /// <param name="empresaID"></param>
        /// <returns></returns>
        public DataSet carregarCaixas(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                DateTime seisMeses = DateTime.Now.AddMonths(-6);

                string sql = "SELECT DISTINCT tCanal.ID AS CanalID, tCanal.Nome AS Canal " +
                    "FROM tEmpresa,tCanal,tLoja,tCaixa,tUsuario " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tEmpresa.ID=" + empresaID + " " +
                    "ORDER BY tCanal.Nome";


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow canal = canais.NewRow();
                    canal["ID"] = bd.LerInt("CanalID");
                    canal["Nome"] = bd.LerString("Canal");
                    canais.Rows.Add(canal);
                }

                sql = "SELECT DISTINCT tCanal.ID AS CanalID, tLoja.ID AS LojaID, tLoja.Nome AS Loja " +
                    "FROM tEmpresa,tCanal,tLoja,tCaixa,tUsuario " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tEmpresa.ID=" + empresaID + " " +
                    "ORDER BY tLoja.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);
                }

                sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario " +
                      "FROM tEmpresa,tCanal,tLoja,tCaixa,tUsuario " +
                      "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tEmpresa.ID=" + empresaID + "AND tCaixa.DataAbertura > '" + seisMeses.ToString("yyyyMMddHHmmss") + "' " +
                      "ORDER BY tUsuario.Nome";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {

                    DataRow usuario = usuarios.NewRow();
                    usuario["ID"] = bd.LerInt("UsuarioID");
                    usuario["Nome"] = bd.LerString("Usuario");
                    usuario["LojaID"] = bd.LerInt("LojaID");
                    usuarios.Rows.Add(usuario);
                }


                sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario, tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                      "FROM tEmpresa,tCanal,tLoja,tCaixa,tUsuario " +
                      "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tEmpresa.ID=" + empresaID + " AND tCaixa.DataAbertura > '" + seisMeses.ToString("yyyyMMddHHmmss") + "' " +
                      "ORDER BY tCaixa.DataAbertura DESC, tCaixa.ID DESC";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);
                }

                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);

                return buffer;

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

        /// <summary>
        /// carrega os dados para os relatórios: Caixa Resumo e Caixa Detalhes. 
        /// Os caixas somente ativos, de 6 meses e os usuários com status ='L'
        /// </summary>
        /// <param name="empresaID"></param>
        /// <returns></returns>
        public DataSet carregarCaixasAtivos(int empresaID)
        {
            try
            {
                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                DateTime seisMeses = DateTime.Now.AddMonths(-6);

                string sql = string.Format("sp_getCanalCaixasPorEmpresa @EmpresaID = {0}, @Tipo = '{1}'", empresaID, "Canal");
                    
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow canal = canais.NewRow();
                    canal["ID"] = bd.LerInt("CanalID");
                    canal["Nome"] = bd.LerString("Canal");
                    canais.Rows.Add(canal);
                }

                sql = string.Format("sp_getCanalCaixasPorEmpresa @EmpresaID = {0}, @Tipo = '{1}'", empresaID, "Loja");

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);
                }

                sql = string.Format("sp_getCanalCaixasPorEmpresa @EmpresaID = {0}, @Tipo = '{1}'", empresaID, "Usuario");

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow usuario = usuarios.NewRow();
                    usuario["ID"] = bd.LerInt("UsuarioID");
                    usuario["Nome"] = bd.LerString("Usuario");
                    usuario["LojaID"] = bd.LerInt("LojaID");
                    usuarios.Rows.Add(usuario);
                }


                sql = string.Format("sp_getCanalCaixasPorEmpresa @EmpresaID = {0}, @Tipo = '{1}', @RangeData = '{2}'", empresaID, "Caixa", seisMeses.ToString("yyyyMMddHHmmss"));

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerString("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);
                }

                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);

                return buffer;

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

        public DataSet carregarCaixasSemFiltro(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                string sql = "SELECT DISTINCT tCanal.ID AS CanalID, tCanal.Nome AS Canal " +
                    "FROM tEmpresa,tCanal,tLoja,tCaixa,tUsuario " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tEmpresa.ID=" + empresaID + " " +
                    "ORDER BY tCanal.Nome";


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow canal = canais.NewRow();
                    canal["ID"] = bd.LerInt("CanalID");
                    canal["Nome"] = bd.LerString("Canal");
                    canais.Rows.Add(canal);
                }

                sql = "SELECT DISTINCT tCanal.ID AS CanalID, tLoja.ID AS LojaID, tLoja.Nome AS Loja " +
                    "FROM tEmpresa,tCanal,tLoja,tCaixa,tUsuario " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tEmpresa.ID=" + empresaID + " " +
                    "ORDER BY tLoja.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);
                }

                sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario " +
                      "FROM tEmpresa,tCanal,tLoja,tCaixa,tUsuario " +
                      "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tEmpresa.ID=" + empresaID + " " +
                      "ORDER BY tUsuario.Nome";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {

                    DataRow usuario = usuarios.NewRow();
                    usuario["ID"] = bd.LerInt("UsuarioID");
                    usuario["Nome"] = bd.LerString("Usuario");
                    usuario["LojaID"] = bd.LerInt("LojaID");
                    usuarios.Rows.Add(usuario);
                }


                sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario, tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                      "FROM tEmpresa,tCanal,tLoja,tCaixa,tUsuario " +
                      "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.ID=tLoja.CanalID AND tCaixa.LojaID=tLoja.ID AND tCaixa.UsuarioID=tUsuario.ID AND tEmpresa.ID=" + empresaID + " " +
                      "ORDER BY tCaixa.DataAbertura DESC, tCaixa.ID DESC";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);
                }

                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);

                return buffer;

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

        public DataSet carregarCaixasAtivasSemFiltro(int empresaID)
        {

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable canais = new DataTable("Canal");
                canais.Columns.Add("ID", typeof(int));
                canais.Columns.Add("Nome", typeof(string));

                DataTable lojas = new DataTable("Loja");
                lojas.Columns.Add("ID", typeof(int));
                lojas.Columns.Add("Nome", typeof(string));
                lojas.Columns.Add("CanalID", typeof(int));

                DataTable usuarios = new DataTable("Usuario");
                usuarios.Columns.Add("ID", typeof(int));
                usuarios.Columns.Add("Nome", typeof(string));
                usuarios.Columns.Add("LojaID", typeof(int));

                DataTable caixas = new DataTable("Caixa");
                caixas.Columns.Add("ID", typeof(int));
                caixas.Columns.Add("Nome", typeof(string));
                caixas.Columns.Add("LojaID", typeof(int));
                caixas.Columns.Add("UsuarioID", typeof(int));

                string sql = "SELECT DISTINCT tCanal.ID AS CanalID, tCanal.Nome AS Canal " +
                             "FROM tEmpresa " +
                             "INNER JOIN tCanal ON tCanal.EmpresaID=tEmpresa.ID " +
                             "INNER JOIN tLoja ON tCanal.ID=tLoja.CanalID " +
                             "INNER JOIN tCaixa ON tCaixa.LojaID=tLoja.ID " +
                             "INNER JOIN tUsuario ON tCaixa.UsuarioID=tUsuario.ID " +
                             "WHERE tEmpresa.ID=" + empresaID + " AND tCanal.Ativo = 'T' " +
                             "ORDER BY tCanal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    DataRow canal = canais.NewRow();
                    canal["ID"] = bd.LerInt("CanalID");
                    canal["Nome"] = bd.LerString("Canal");
                    canais.Rows.Add(canal);
                }

                sql = "SELECT DISTINCT tCanal.ID AS CanalID, tLoja.ID AS LojaID, tLoja.Nome AS Loja " +
                      "FROM tEmpresa " +
                      "INNER JOIN tCanal ON tCanal.EmpresaID=tEmpresa.ID " +
                      "INNER JOIN tLoja ON tCanal.ID=tLoja.CanalID " +
                      "INNER JOIN tCaixa ON tCaixa.LojaID=tLoja.ID " +
                      "INNER JOIN tUsuario ON tCaixa.UsuarioID=tUsuario.ID " +
                      "WHERE tEmpresa.ID=" + empresaID + " AND tCanal.Ativo = 'T' " +
                      "ORDER BY tLoja.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow loja = lojas.NewRow();
                    loja["ID"] = bd.LerInt("LojaID");
                    loja["Nome"] = bd.LerString("Loja");
                    loja["CanalID"] = bd.LerInt("CanalID");
                    lojas.Rows.Add(loja);
                }

                sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario " +
                      "FROM tEmpresa " +
                      "INNER JOIN tCanal ON tCanal.EmpresaID=tEmpresa.ID " +
                      "INNER JOIN tLoja ON tCanal.ID=tLoja.CanalID " +
                      "INNER JOIN tCaixa ON tCaixa.LojaID=tLoja.ID " +
                      "INNER JOIN tUsuario ON tCaixa.UsuarioID=tUsuario.ID " +
                      "WHERE tEmpresa.ID=" + empresaID + " AND tCanal.Ativo = 'T' " +
                      "ORDER BY tUsuario.Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {

                    DataRow usuario = usuarios.NewRow();
                    usuario["ID"] = bd.LerInt("UsuarioID");
                    usuario["Nome"] = bd.LerString("Usuario");
                    usuario["LojaID"] = bd.LerInt("LojaID");
                    usuarios.Rows.Add(usuario);
                }


                sql = "SELECT DISTINCT tLoja.ID AS LojaID, tUsuario.ID AS UsuarioID, tUsuario.Nome AS Usuario, tCaixa.ID AS CaixaID, tCaixa.DataAbertura " +
                      "FROM tEmpresa " +
                      "INNER JOIN tCanal ON tCanal.EmpresaID=tEmpresa.ID " +
                      "INNER JOIN tLoja ON tCanal.ID=tLoja.CanalID " +
                      "INNER JOIN tCaixa ON tCaixa.LojaID=tLoja.ID " +
                      "INNER JOIN tUsuario ON tCaixa.UsuarioID=tUsuario.ID " +
                      "WHERE tEmpresa.ID=" + empresaID + " AND tCanal.Ativo = 'T' " +
                      "ORDER BY tCaixa.DataAbertura DESC, tCaixa.ID DESC";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow caixa = caixas.NewRow();
                    caixa["ID"] = bd.LerInt("CaixaID");
                    caixa["Nome"] = bd.LerStringFormatoSemanaDataHora("DataAbertura");
                    caixa["LojaID"] = bd.LerInt("LojaID");
                    caixa["UsuarioID"] = bd.LerInt("UsuarioID");
                    caixas.Rows.Add(caixa);
                }

                buffer.Tables.Add(canais);
                buffer.Tables.Add(lojas);
                buffer.Tables.Add(usuarios);
                buffer.Tables.Add(caixas);

                return buffer;

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

        /// <summary>
        /// Abrir caixa.
        /// </summary>
        /// <param name="usuarioid"></param>
        /// <param name="lojaid"></param>
        /// <param name="apresentacaoid"></param>
        /// <param name="saldoinicial"></param>
        /// <returns></returns>
        public override bool Abrir(int usuarioid, int lojaid, int apresentacaoid, decimal saldoinicial)
        {

            try
            {

                string sql = "SELECT ID,DataAbertura " +
                    "FROM tCaixa " +
                    "WHERE UsuarioID=" + usuarioid + " AND " +
                    "LojaID=" + lojaid + " AND ApresentacaoID=" + apresentacaoid + " AND DataFechamento=''";

                object resp = bd.ConsultaValor(sql);

                bd.Fechar();

                bool ok = (resp != null);

                if (ok)
                {
                    int id = Convert.ToInt32(resp);
                    this.Ler(id);
                }
                else
                {
                    //criar um registro na tabela
                    this.DataAbertura.Valor = System.DateTime.Now;
                    this.ApresentacaoID.Valor = apresentacaoid;
                    this.UsuarioID.Valor = usuarioid;
                    this.LojaID.Valor = lojaid;
                    this.ApresentacaoID.Valor = apresentacaoid;
                    this.DataFechamento.ValorBD = "";
                    this.SaldoInicial.Valor = saldoinicial;
                    Loja oLoja = new Loja();
                    oLoja.Ler(lojaid);
                    Canal oCanal = new Canal();
                    oCanal.Ler(oLoja.CanalID.Valor);
                    this.Comissao.Valor = oCanal.Comissao.Valor;
                    ok = this.Inserir();
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Fechar caixa.
        /// </summary>
        public bool Fechar()
        {
            bool retorno = false;

            bd.IniciarTransacao();

            try
            {
                retorno = Fechar(bd);

                if (retorno)
                    bd.FinalizarTransacao();
                else
                    bd.DesfazerTransacao();
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

            return retorno;
        }

        private bool Fechar(BD bd)
        {
            try
            {
                this.DataFechamento.Valor = System.DateTime.Now;

                string sql = "UPDATE tCaixa SET DataFechamento = '" + this.DataFechamento.ValorBD + "' WHERE DataFechamento = '' AND ID = " + this.Control.ID;

                bd.Executar(sql);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public int FechamentoCaixas(string dataLimite)
        {
            List<ClientObjects.EstruturaFechamentoCaixa> listaCaixas = new List<IRLib.ClientObjects.EstruturaFechamentoCaixa>();
            ClientObjects.EstruturaFechamentoCaixa oCaixa;
            BD bdLeitura = new BD();

            try
            {
                using (IDataReader oDataReader = bdLeitura.Consulta("" +
                    "SELECT " +
                    "	tCaixa.ID AS CaixaID, " +
                    "	tCanal.EmpresaID " +
                    "FROM tCaixa (NOLOCK) " +
                    "INNER JOIN tLoja (NOLOCK) ON tLoja.ID = tCaixa.LojaID " +
                    "INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID " +
                    "WHERE " +
                    "	(tCaixa.DataAbertura < '" + dataLimite + "999999') " +
                    "AND " +
                    "	(tCaixa.DataFechamento = '') " +
                    "ORDER BY " +
                    "	tCaixa.ID "))
                {
                    while (oDataReader.Read())
                    {
                        oCaixa = new IRLib.ClientObjects.EstruturaFechamentoCaixa();
                        oCaixa.CaixaID = bdLeitura.LerInt("CaixaID");
                        oCaixa.EmpresaID = bdLeitura.LerInt("EmpresaID");

                        listaCaixas.Add(oCaixa);
                    }
                }

                for (int contadorCaixas = 0; contadorCaixas < listaCaixas.Count; contadorCaixas++)
                {
                    this.Control.ID = listaCaixas[contadorCaixas].CaixaID;
                    if (!Fechar(bdLeitura))
                        throw new Exception("Não foi possível fechar os caixas.");
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                bdLeitura.Fechar();
            }

            return listaCaixas.Count;
        }

        /// <summary>
        /// Quantidade de Ingressos por Caixa e por Status
        /// </summary>
        public override int QuantidadeIngressos(string status)
        {

            try
            {

                BD bd = new BD();

                string sql = "SELECT Count(tIngressoLog.ID) AS Qtde " +
                    "FROM tIngressoLog (nolock) " +
                    "WHERE tIngressoLog.Acao='" + status + "' AND tIngressoLog.CaixaID=" + this.Control.ID;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                int qtde = 0;

                if (obj != null)
                    qtde = (int)obj;

                bd.Fechar();

                return qtde;

            }
            catch (Exception erro)
            {
                throw erro;
            }

        }

        public decimal ValorIngressos(string status)
        {
            decimal valor = 0;
            try
            {
                // Obtendo dados através de SQL
                BD bancoObterValor = new BD();
                string sql =
                    "SELECT     SUM(tPreco.Valor) AS Valor, tIngressoLog.CaixaID, tIngressoLog.Acao " +
                    "FROM       tIngressoLog INNER JOIN " +
                    "tPreco ON tIngressoLog.PrecoID = tPreco.ID " +
                    "GROUP BY tIngressoLog.CaixaID, tIngressoLog.Acao " +
                    "HAVING     (tIngressoLog.CaixaID = " + this.Control.ID + ") AND (tIngressoLog.Acao = N'" + status + "') ";
                bancoObterValor.Consulta(sql);
                if (bancoObterValor.Consulta().Read())
                {
                    valor = bancoObterValor.LerInt("Valor");
                }
                bancoObterValor.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return valor;
        }

        public decimal ValorConveniencia(string status)
        {
            decimal valor = 0;
            try
            {
                // Obtendo dados através de SQL
                BD bancoObterValor = new BD();
                string sql =
                    "SELECT   tIngressoLog.CaixaID, tIngressoLog.Acao, SUM(tVendaBilheteriaItem.TaxaConvenienciaValor) AS Valor " +
                    "FROM     tIngressoLog INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID " +
                    "GROUP BY tIngressoLog.CaixaID, tIngressoLog.Acao " +
                    "HAVING     (tIngressoLog.CaixaID = " + this.Control.ID + ") AND (tIngressoLog.Acao = N'" + status + "') ";
                bancoObterValor.Consulta(sql);
                if (bancoObterValor.Consulta().Read())
                {
                    valor = bancoObterValor.LerInt("Valor");
                }
                bancoObterValor.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return valor;
        }

        public decimal ValorEntrega(string status)
        {
            decimal valor = 0;
            try
            {
                // Obtendo dados através de SQL
                BD bancoObterValor = new BD();
                string sql =
                    "SELECT        tIngressoLog.CaixaID, tIngressoLog.Acao, SUM(DISTINCT tVendaBilheteria.TaxaEntregaValor) AS Valor " +
                    "FROM          tIngressoLog INNER JOIN " +
                    "tVendaBilheteria ON tIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID " +
                    "GROUP BY tIngressoLog.CaixaID, tIngressoLog.Acao " +
                    "HAVING        (tIngressoLog.CaixaID = " + this.Control.ID + ") AND (tIngressoLog.Acao = N'" + status + "') ";
                bancoObterValor.Consulta(sql);
                if (bancoObterValor.Consulta().Read())
                {
                    valor = bancoObterValor.LerInt("Valor");
                }
                bancoObterValor.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return valor;
        }

        /// <summary>
        /// VendaSenha por período do Caixa (servirá pra venda)
        /// E ação do log, etc
        /// </summary>
        public override string VendaSenhaPorPeriodoCaixaSQL(string dataInicial, string dataFinal, string acao, bool comCortesia,
            int apresentacaoID, int eventoID, int localID, int empresaID,
            int usuarioID, int lojaID, int canalID)
        {
            try
            {
                // Condições variadas
                string condicaoCortesia;
                if (comCortesia)
                    condicaoCortesia = ">=0";
                else
                    condicaoCortesia = "=0"; // sem cortesia
                string condicaoFiltro = " ";
                if (apresentacaoID > 0)
                    condicaoFiltro += " AND (tApresentacaoSetor.ApresentacaoID = " + apresentacaoID + ") ";
                if (eventoID > 0)
                    condicaoFiltro += " AND (tApresentacao.EventoID = " + eventoID + ") ";
                if (localID > 0)
                    condicaoFiltro += " AND (tEvento.LocalID = " + localID + ") ";
                if (usuarioID > 0)
                    condicaoFiltro += " AND (tCaixa.UsuarioID = " + usuarioID + ") ";
                if (lojaID > 0)
                    condicaoFiltro += " AND (tCaixa.LojaID = " + lojaID + ") ";
                if (canalID > 0)
                    condicaoFiltro += " AND (tLoja.CanalID = " + canalID + ") ";
                if (empresaID > 0 && localID > 0)
                {
                    condicaoFiltro += " AND (tLocal.EmpresaID = " + empresaID + ") " +
                        " AND (tApresentacao.DisponivelRelatorio = 'T') ";
                }
                else
                {
                    // Perfil especial, indiferente DisponivelRelatorio
                }
                if (empresaID > 0 && canalID > 0)
                {
                    condicaoFiltro += " AND (tCanal.EmpresaID = " + empresaID + ") " +
                        " AND (tApresentacao.DisponivelRelatorio = 'T') ";
                }
                else
                {
                    // Perfil especial, indiferente DisponivelRelatorio
                }
                if (empresaID > 0 && canalID == 0 && localID == -1)
                {
                    // vendas por canal
                    condicaoFiltro += " AND (tCanal.EmpresaID = " + empresaID + ") " +
                        " AND (tApresentacao.DisponivelRelatorio = 'T') ";
                }
                else
                {
                    // Perfil especial, indiferente DisponivelRelatorio
                }
                if (empresaID > 0 && canalID == -1 && localID == 0)
                {
                    // vendas por evento
                    condicaoFiltro += " AND (tLocal.EmpresaID = " + empresaID + ") " +
                        " AND (tApresentacao.DisponivelRelatorio = 'T') ";
                }
                else
                {
                    // Perfil especial, indiferente DisponivelRelatorio
                }
                string sql =
                    "SELECT DISTINCT tVendaBilheteria.Senha " +
                    "FROM     tCaixa INNER JOIN " +
                    "tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                    "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                    "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                    "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                    "tEvento ON tApresentacao.EventoID = tEvento.ID INNER JOIN " +
                    "tLocal ON tEvento.LocalID = tLocal.ID INNER JOIN " +
                    "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                    "tCanal ON tLoja.CanalID = tCanal.ID INNER JOIN " +
                    "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                    "WHERE    (tCaixa.DataAbertura >= '" + dataInicial + "') AND (tCaixa.DataAbertura <= '" + dataFinal + "') AND (tIngressoLog.Acao IN (" + acao + ")) " +
                    "AND (tIngressoLog.CortesiaID " + condicaoCortesia + ") " + condicaoFiltro;
                return sql;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de VendaSenhaPorPeriodoCaixaSQL

        /// <summary>
        /// IngressoLogID por período do Caixa (servirá pra venda)
        /// E ação do log, etc
        /// </summary>
        public override string IngressoLogIDsPorPeriodoCaixaSQL(string dataInicial, string dataFinal, string acao, bool comCortesia,
            int apresentacaoID, int eventoID, int localID, int empresaID,
            int usuarioID, int lojaID, int canalID, string tipoLinha, bool disponivel,
            bool vendasCanal, bool empresaVendeIngressos, bool empresaPromoveEventos)
        {
            try
            {
                if (empresaID != 0 && !empresaVendeIngressos && !empresaPromoveEventos) // a empresa deve pertencer a uma das situações
                    return "";
                #region Condições comuns para os 2 tipos de vendas
                string condicaoCortesia;
                if (comCortesia)
                    condicaoCortesia = ">=0";
                else
                    condicaoCortesia = "=0"; // sem cortesia
                string condicaoFiltro = " ";
                if (apresentacaoID > 0)
                    condicaoFiltro += " AND (tApresentacaoSetor.ApresentacaoID = " + apresentacaoID + ") ";
                if (localID > 0)
                    condicaoFiltro += " AND (tEvento.LocalID = " + localID + ") ";
                if (usuarioID > 0)
                    condicaoFiltro += " AND (tCaixa.UsuarioID = " + usuarioID + ") ";
                if (lojaID > 0)
                    condicaoFiltro += " AND (tCaixa.LojaID = " + lojaID + ") ";
                if (disponivel)
                    condicaoFiltro += " AND (tApresentacao.DisponivelRelatorio = 'T') ";
                #endregion
                #region Vendas por Evento
                if (!vendasCanal && empresaID > 0)
                {
                    // Se a empresa só vende ingressos e não promove eventos, não tem vendas por Evento
                    if (empresaVendeIngressos && !empresaPromoveEventos)
                        return "";
                    // Se a empresa só promove eventos e não vende ingressos, mostrar as vendas dos eventos da empresa
                    if (!empresaVendeIngressos && empresaPromoveEventos)
                    {
                        condicaoFiltro += " AND (tLocal.EmpresaID = " + empresaID + ") ";
                        if (eventoID > 0)
                        {
                            // selecionou um evento
                            condicaoFiltro += " AND (tEvento.ID = " + eventoID + ") ";
                        }
                    }
                    // Se a empresa promove eventos e vende ingressos, mostrar as vendas dos eventos da empresa
                    if (empresaVendeIngressos && empresaPromoveEventos)
                    {
                        condicaoFiltro += " AND (tLocal.EmpresaID = " + empresaID + ") ";
                        if (eventoID > 0)
                        {
                            // selecionou um evento
                            condicaoFiltro += " AND (tEvento.ID = " + eventoID + ") ";
                        }
                    }
                }
                #endregion
                #region Vendas por Canal
                if (vendasCanal && empresaID > 0)
                {
                    // Se a empresa não vende ingressos e só promove eventos, não tem vendas por Canal
                    if (!empresaVendeIngressos && empresaPromoveEventos)
                        return "";
                    if (eventoID > 0)
                    {
                        // Situação especial para Caixas por Evento
                        condicaoFiltro += " AND (tEvento.ID = " + eventoID + ") ";
                    }
                    // Se a empresa não promove eventos e só vende ingressos, mostrar as vendas dos canais da empresa
                    if (empresaVendeIngressos && !empresaPromoveEventos)
                    {
                        if (canalID > 0)
                        {
                            // selecionou um canal
                            condicaoFiltro += " AND (tCanal.ID = " + canalID + ") ";
                        }
                        else
                        {
                            condicaoFiltro += " AND (tCanal.EmpresaID = " + empresaID + ") ";
                        }
                    }
                    // Se a empresa promove eventos e vende ingressos, 
                    // mostrar todas as vendas dos eventos desta empresa inclusive os canais que não são desta empresa
                    if (empresaVendeIngressos && empresaPromoveEventos)
                    {
                        condicaoFiltro += " AND (tLocal.EmpresaID = " + empresaID + ") ";
                        if (canalID > 0)
                        {
                            // selecionou um canal
                            condicaoFiltro += " AND (tCanal.ID = " + canalID + ") ";
                        }
                        if (canalID == -1)
                        {
                            // todos canais de uma empresa que vende e promove
                            Empresa empresa = new Empresa();
                            empresa.Ler(empresaID);
                            DataTable canaisTabela = empresa.CanaisQueVendem(null);
                            string canaisID = "";
                            for (int contador = 0; contador < canaisTabela.Rows.Count; contador++)
                            {
                                if (contador == 0)
                                {
                                    canaisID = Convert.ToInt32(canaisTabela.Rows[contador]["ID"]).ToString();
                                }
                                else
                                {
                                    canaisID = canaisID + "," + Convert.ToInt32(canaisTabela.Rows[contador]["ID"]).ToString();
                                }
                            }
                            condicaoFiltro += " AND (tCanalEvento.CanalID IN (" + canaisID + ")) ";
                        }
                    }
                }
                #endregion
                #region Usado no Caixas para Conferência
                if (empresaID == 0)
                {
                    // Eventos discriminados e Canais discriminados então eventoID>0 e canalID>0
                    // Eventos discriminados e Canais agrupados então eventoID>0 
                    // Eventos agrupados e Canais discriminados então canalID>0
                    // Eventos agrupados e Canais agrupados não existe então eventoID=0 e canalID=0 não entraria em nenhum if
                    if (eventoID > 0)
                    {
                        condicaoFiltro += " AND (tEvento.ID = " + eventoID + ") ";
                    }
                    if (canalID > 0)
                    {
                        condicaoFiltro += " AND (tCanal.ID = " + canalID + ") ";
                    }
                }
                #endregion
                string sql =
                    "SELECT DISTINCT tIngressoLog.ID AS IngressoLogID " +
                    "FROM     tCaixa INNER JOIN " +
                    "tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                    "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                    "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                    "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                    "tEvento ON tApresentacao.EventoID = tEvento.ID INNER JOIN " +
                    "tLocal ON tEvento.LocalID = tLocal.ID INNER JOIN " +
                    "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                    "tCanal ON tLoja.CanalID = tCanal.ID INNER JOIN " +
                    "tUsuario ON tCaixa.UsuarioID = tUsuario.ID INNER JOIN " +
                    "tCanalEvento ON tCanal.ID = tCanalEvento.CanalID INNER JOIN " +
                    "tEvento tEvento_1 ON tCanalEvento.EventoID = tEvento_1.ID INNER JOIN " +
                    "tLocal tLocal_1 ON tEvento_1.LocalID = tLocal_1.ID " +
                    "WHERE    (tCaixa.DataAbertura >= '" + dataInicial + "') AND (tCaixa.DataAbertura <= '" + dataFinal + "') AND (tIngressoLog.Acao IN (" + acao + ")) " +
                    "AND (tIngressoLog.CortesiaID " + condicaoCortesia + ") " + condicaoFiltro;
                BD obterDados = new BD();
                obterDados.Consulta(sql);
                // Foi discretizado os IDs para melhorar desempenho
                string IDs = "";
                if (obterDados.Consulta().Read())
                {
                    IDs += obterDados.LerString("IngressoLogID");
                }
                while (obterDados.Consulta().Read())
                {
                    IDs = IDs + "," + obterDados.LerString("IngressoLogID");
                }
                obterDados.Fechar();
                return IDs;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public override DataTable LinhasVendasGerenciaisDias(string ingressoLogIDs)
        {
            try
            {
                DataTable tabela = Utilitario.EstruturaVendasGerenciais();
                if (ingressoLogIDs != "")
                {
                    // Obtendo dados através de SQL
                    BD obterDados = new BD();
                    string sql =
                        "SELECT DISTINCT LEFT(tCaixa.DataAbertura, 8) AS Dia " +
                        "FROM            tCaixa INNER JOIN " +
                        "tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                        "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                        "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " +
                        "ORDER BY LEFT(tCaixa.DataAbertura, 8) DESC";
                    obterDados.Consulta(sql);
                    while (obterDados.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        //					linha["VariacaoLinhaID"]= obterDados.LerInt("ID");
                        string dia = obterDados.LerString("Dia");
                        string formato = dia.Substring(6, 2) + "/" + dia.Substring(4, 2) + "/" + dia.Substring(0, 4);
                        linha["VariacaoLinha"] = formato;
                        tabela.Rows.Add(linha);
                    }
                    obterDados.Fechar();
                }
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Dias

        /// <summary>
        /// Vendas Gerenciais por dia com Quantidade e Valores dos Ingressos dos Vendidos e Cancelados e Total
        /// Com porcentagem
        /// </summary>
        public override DataTable VendasGerenciaisDias(string dataInicial, string dataFinal, bool comCortesia,
            int apresentacaoID, int eventoID, int localID, int empresaID, bool vendasCanal, string tipoLinha, bool disponivel, bool empresaVendeIngressos, bool empresaPromoveEventos)
        {
            try
            {
                int usuarioID = 0;
                int lojaID = 0;
                int canalID = 0;
                if (vendasCanal)
                {
                    // se for por Canal, os parâmetro têm representações diferentes
                    usuarioID = apresentacaoID;
                    lojaID = eventoID;
                    canalID = localID;
                    apresentacaoID = 0;
                    eventoID = 0;
                    localID = 0;
                }
                // Variáveis usados no final do Grid (totalizando)
                int quantidadeVendidosTotais = 0;
                int quantidadeCanceladosTotais = 0;
                int quantidadeTotalTotais = 0;
                decimal valoresVendidosTotais = 0;
                decimal valoresCanceladosTotais = 0;
                decimal valoresTotalTotais = 0;
                decimal quantidadeVendidosTotaisPorcentagem = 0;
                decimal quantidadeCanceladosTotaisPorcentagem = 0;
                decimal quantidadeTotalTotaisPorcentagem = 0;
                decimal valoresVendidosTotaisPorcentagem = 0;
                decimal valoresCanceladosTotaisPorcentagem = 0;
                decimal valoresTotalTotaisPorcentagem = 0;
                #region Obter os Caixas nos intervalos especificados
                IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos e cancelados
                string ingressoLogIDsTotais = IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos + "," + ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsVendidos = IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsCancelados = IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                DataTable tabela = LinhasVendasGerenciaisDias(ingressoLogIDsTotais);
                int totaisVendidos = QuantidadeIngressosPorDia(ingressoLogIDsVendidos, ""); // quando não especifica dia, são todos os dias
                int totaisCancelados = QuantidadeIngressosPorDia(ingressoLogIDsCancelados, "");
                int totaisTotal = totaisVendidos - totaisCancelados;
                decimal valoresVendidos = ValorIngressosPorDia(ingressoLogIDsVendidos, "");
                decimal valoresCancelados = ValorIngressosPorDia(ingressoLogIDsCancelados, "");
                decimal valoresTotal = valoresVendidos - valoresCancelados;
                #endregion
                // Para cada dia no período especificado, calcular
                foreach (DataRow linha in tabela.Rows)
                {
                    string dia = Convert.ToString(linha["VariacaoLinha"]);
                    #region Quantidade
                    // Vendidos
                    linha["Qtd Vend"] = QuantidadeIngressosPorDia(ingressoLogIDsVendidos, dia);
                    if (totaisVendidos > 0)
                        linha["% Vend"] = (decimal)Convert.ToInt32(linha["Qtd Vend"]) / (decimal)totaisVendidos * 100;
                    else
                        linha["% Vend"] = 0;
                    // Cancelados
                    linha["Qtd Canc"] = QuantidadeIngressosPorDia(ingressoLogIDsCancelados, dia);
                    if (totaisCancelados > 0)
                        linha["% Canc"] = (decimal)Convert.ToInt32(linha["Qtd Canc"]) / (decimal)totaisCancelados * 100;
                    else
                        linha["% Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["Qtd Total"] = Convert.ToInt32(linha["Qtd Vend"]) - Convert.ToInt32(linha["Qtd Canc"]);
                    if (totaisTotal > 0)
                        linha["% Total"] = (decimal)Convert.ToInt32(linha["Qtd Total"]) / (decimal)totaisTotal * 100;
                    else
                        linha["% Total"] = 0;
                    // Totalizando
                    quantidadeVendidosTotais += Convert.ToInt32(linha["Qtd Vend"]);
                    quantidadeCanceladosTotais += Convert.ToInt32(linha["Qtd Canc"]);
                    quantidadeTotalTotais += Convert.ToInt32(linha["Qtd Total"]);
                    quantidadeVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% Vend"]);
                    quantidadeCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% Canc"]);
                    quantidadeTotalTotaisPorcentagem += Convert.ToDecimal(linha["% Total"]);
                    // Formato
                    linha["% Total"] = Convert.ToDecimal(linha["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Vend"] = Convert.ToDecimal(linha["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Canc"] = Convert.ToDecimal(linha["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                    #region Valor
                    // Vendidos
                    linha["R$ Vend"] = ValorIngressosPorDia(ingressoLogIDsVendidos, dia);
                    if (valoresVendidos > 0)
                        linha["% R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]) / valoresVendidos * 100;
                    else
                        linha["% R$ Vend"] = 0;
                    // Cancelados
                    linha["R$ Canc"] = ValorIngressosPorDia(ingressoLogIDsCancelados, dia);
                    if (valoresCancelados > 0)
                        linha["% R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]) / valoresCancelados * 100;
                    else
                        linha["% R$ Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Vend"]) - Convert.ToDecimal(linha["R$ Canc"]);
                    if (valoresTotal > 0)
                        linha["% R$ Total"] = Convert.ToDecimal(linha["R$ Total"]) / valoresTotal * 100;
                    else
                        linha["% R$ Total"] = 0;
                    // Totalizando
                    valoresVendidosTotais += Convert.ToDecimal(linha["R$ Vend"]);
                    valoresCanceladosTotais += Convert.ToDecimal(linha["R$ Canc"]);
                    valoresTotalTotais += Convert.ToDecimal(linha["R$ Total"]);
                    valoresVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Vend"]);
                    valoresCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Canc"]);
                    valoresTotalTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Total"]);
                    // Formato
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linha["% R$ Total"] = Convert.ToDecimal(linha["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Vend"] = Convert.ToDecimal(linha["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Canc"] = Convert.ToDecimal(linha["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                }
                if (tabela.Rows.Count > 0)
                {
                    DataRow linhaTotais = tabela.NewRow();
                    // Totais
                    linhaTotais["VariacaoLinha"] = "Totais";
                    linhaTotais["Qtd Total"] = quantidadeTotalTotais;
                    linhaTotais["Qtd Vend"] = quantidadeVendidosTotais;
                    linhaTotais["Qtd Canc"] = quantidadeCanceladosTotais;
                    linhaTotais["% Total"] = quantidadeTotalTotaisPorcentagem;
                    linhaTotais["% Vend"] = quantidadeVendidosTotaisPorcentagem;
                    linhaTotais["% Canc"] = quantidadeCanceladosTotaisPorcentagem;
                    linhaTotais["R$ Total"] = valoresTotalTotais;
                    linhaTotais["R$ Vend"] = valoresVendidosTotais;
                    linhaTotais["R$ Canc"] = valoresCanceladosTotais;
                    linhaTotais["% R$ Total"] = valoresTotalTotaisPorcentagem;
                    linhaTotais["% R$ Vend"] = valoresVendidosTotaisPorcentagem;
                    linhaTotais["% R$ Canc"] = valoresCanceladosTotaisPorcentagem;
                    // Formato
                    linhaTotais["% Total"] = Convert.ToDecimal(linhaTotais["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Vend"] = Convert.ToDecimal(linhaTotais["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Canc"] = Convert.ToDecimal(linhaTotais["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["R$ Total"] = Convert.ToDecimal(linhaTotais["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Vend"] = Convert.ToDecimal(linhaTotais["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Canc"] = Convert.ToDecimal(linhaTotais["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["% R$ Total"] = Convert.ToDecimal(linhaTotais["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Vend"] = Convert.ToDecimal(linhaTotais["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Canc"] = Convert.ToDecimal(linhaTotais["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    tabela.Rows.Add(linhaTotais);
                }
                tabela.Columns["VariacaoLinha"].ColumnName = "Dia";
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        /// <summary>
        /// Obter quantidade de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeIngressosPorDia(string ingressoLogIDs, string dia)
        {
            try
            {
                int quantidade = 0;
                if (ingressoLogIDs != "")
                {
                    // Obtendo dados
                    string sql = "";
                    if (dia != "")
                    {
                        string diaBD = dia.Substring(6, 4) + dia.Substring(3, 2) + dia.Substring(0, 2);
                        sql =
                            "SELECT COUNT(tCaixa.DataAbertura) AS QuantidadeIngressos " +
                            "FROM tCaixa INNER JOIN " +
                            "tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                            "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                            "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID " +
                            "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " +
                            "GROUP BY LEFT(tCaixa.DataAbertura, 8) " +
                            "HAVING (LEFT(tCaixa.DataAbertura, 8) = '" + diaBD + "') " +
                            "ORDER BY LEFT(tCaixa.DataAbertura, 8) DESC";
                    }
                    else
                    {
                        sql =
                            "SELECT COUNT(tCaixa.DataAbertura) AS QuantidadeIngressos " +
                            "FROM  tCaixa INNER JOIN " +
                            "tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                            "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                            "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID " +
                            "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) ";
                    }
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Quantidade de dia
                        if (bd.Consulta().Read())
                        {
                            quantidade = bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    else
                    {
                        // Quantidade de todos dias
                        while (bd.Consulta().Read())
                        {
                            quantidade += bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    bd.Fechar();
                }
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de QuantidadeIngressosPorDia

        /// <summary>
        /// Obter valor de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override decimal ValorIngressosPorDia(string ingressoLogIDs, string dia)
        {
            try
            {
                int valor = 0;
                if (ingressoLogIDs != "")
                {
                    // Obtendo dados
                    string sql = "";
                    if (dia != "")
                    {
                        string diaBD = dia.Substring(6, 4) + dia.Substring(3, 2) + dia.Substring(0, 2);
                        sql =
                            "SELECT     LEFT(tCaixa.DataAbertura, 8) AS Dia, SUM(tPreco.Valor) AS Valor " +
                            "FROM       tCaixa INNER JOIN " +
                            "tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                            "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                            "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                            "tPreco ON tIngressoLog.PrecoID = tPreco.ID " +
                            "WHERE      (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " +
                            "GROUP BY LEFT(tCaixa.DataAbertura, 8) " +
                            "HAVING (LEFT(tCaixa.DataAbertura, 8) = '" + diaBD + "') ";
                    }
                    else
                    {
                        sql =
                            "SELECT        SUM(tPreco.Valor) AS Valor " +
                            "FROM            tCaixa INNER JOIN " +
                            "tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
                            "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                            "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                            "tPreco ON tIngressoLog.PrecoID = tPreco.ID " +
                            "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) ";
                    }
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Valor do dia
                        if (bd.Consulta().Read())
                        {
                            valor = bd.LerInt("Valor");
                        }
                    }
                    else
                    {
                        // Valor de todos dias
                        while (bd.Consulta().Read())
                        {
                            valor += bd.LerInt("Valor");
                        }
                    }
                    bd.Fechar();
                }
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ValorIngressosPorDia

        /// <summary>
        /// Usado no relatório de Vendas Detalhe
        /// </summary>
        /// <param name="caixaID"></param>
        /// <returns></returns>
        public override DataTable VendasDetalhe()
        {

            DataTable tabela = Utilitario.EstruturaVendasDetalhe();
            try
            {
                BD bd = new BD();
                VendaBilheteria vendaBilheteria = new VendaBilheteria();
                // Vendidos 
                string sqlVendidos =
                    "SELECT        tVendaBilheteria.Status, tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, tEvento.Nome AS Evento, tApresentacao.Horario, tSetor.Nome AS Setor, tIngresso.Codigo, tPreco.Valor,  " +
                    "tVendaBilheteria.TaxaConvenienciaValorTotal, tIngressoLog.Acao, tIngressoLog.Obs, tIngressoLog.ID, tVendaBilheteria.CaixaID, tVendaBilheteria.TaxaEntregaValor " +
                    "FROM            tIngressoLog INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                    "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID INNER JOIN " +
                    "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                    "tEvento ON tApresentacao.EventoID = tEvento.ID INNER JOIN " +
                    "tPreco ON tIngressoLog.PrecoID = tPreco.ID " +
                    "WHERE        (tIngressoLog.Acao IN ('" + IngressoLog.VENDER + "')) AND (tVendaBilheteria.CaixaID = " + this.Control.ID + ") " +
                    "ORDER BY tVendaBilheteria.Status, tVendaBilheteria.DataVenda, tEvento.Nome, tApresentacao.Horario, tSetor.Nome ";
                bd.Consulta(sqlVendidos);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["Status"] = "Vendidos";
                    linha["Senha"] = bd.LerString("Senha");
                    linha["DataVenda"] = bd.LerStringFormatoDataHora("DataVenda");
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentação"] = bd.LerStringFormatoDataHora("Horario");
                    linha["Setor/Produto"] = bd.LerString("Setor");
                    linha["Ingresso"] = bd.LerString("Codigo");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["Entrega"] = bd.LerDecimal("TaxaEntregaValor");
                    linha["Conveniência"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                    linha["Total"] = Convert.ToDecimal(linha["Valor"]) + Convert.ToDecimal(linha["Entrega"]) + Convert.ToDecimal(linha["Conveniência"]);
                    linha["Quantidade"] = 1;
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                // Cancelados 
                string sqlCancelados =
                    "SELECT        tVendaBilheteria.Status, tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, tEvento.Nome AS Evento, tApresentacao.Horario, tSetor.Nome AS Setor, tIngresso.Codigo, tPreco.Valor,  " +
                    "tVendaBilheteria.TaxaConvenienciaValorTotal, tIngressoLog.Acao, tIngressoLog.Obs, tIngressoLog.ID, tVendaBilheteria.CaixaID, tVendaBilheteria.TaxaEntregaValor " +
                    "FROM            tIngressoLog INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                    "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID INNER JOIN " +
                    "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                    "tEvento ON tApresentacao.EventoID = tEvento.ID INNER JOIN " +
                    "tPreco ON tIngressoLog.PrecoID = tPreco.ID " +
                    "WHERE        (tIngressoLog.Acao IN ('" + IngressoLog.CANCELAR + "')) AND (tVendaBilheteria.CaixaID = " + this.Control.ID + ") " +
                    "ORDER BY tVendaBilheteria.Status, tVendaBilheteria.DataVenda, tEvento.Nome, tApresentacao.Horario, tSetor.Nome ";
                bd.Consulta(sqlCancelados);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["Status"] = "Cancelados";
                    linha["Senha"] = bd.LerString("Senha");
                    linha["DataVenda"] = bd.LerStringFormatoDataHora("DataVenda");
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentação"] = bd.LerStringFormatoDataHora("Horario");
                    linha["Setor/Produto"] = bd.LerString("Setor");
                    linha["Ingresso"] = bd.LerString("Codigo");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["Entrega"] = bd.LerDecimal("TaxaEntregaValor");
                    linha["Conveniência"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                    linha["Total"] = Convert.ToDecimal(linha["Valor"]) + Convert.ToDecimal(linha["Entrega"]) + Convert.ToDecimal(linha["Conveniência"]);
                    linha["Quantidade"] = 1;
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                // Reimpressos
                string sqlReimpressos =
                    "SELECT        tVendaBilheteria.Status, tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, tEvento.Nome AS Evento, tApresentacao.Horario, tSetor.Nome AS Setor, tIngresso.Codigo, tPreco.Valor,  " +
                    "tVendaBilheteriaItem.TaxaConvenienciaValor, tIngressoLog.Acao, tIngressoLog.Obs AS Motivo, tIngressoLog.ID, tVendaBilheteria.CaixaID, tVendaBilheteria.TaxaEntregaValor " +
                    "FROM          tIngressoLog INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tPreco ON tIngresso.PrecoID = tPreco.ID INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID AND tPreco.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                    "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID INNER JOIN " +
                    "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                    "tEvento ON tApresentacao.EventoID = tEvento.ID " +
                    "WHERE        (tIngressoLog.Acao IN ('R')) AND (tVendaBilheteria.CaixaID  = " + this.Control.ID + ") " +
                    "ORDER BY tVendaBilheteria.Status, tEvento.Nome, tApresentacao.Horario, tVendaBilheteria.DataVenda, tSetor.Nome ";
                bd.Consulta(sqlReimpressos);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["Status"] = "Reimpressos";
                    linha["Senha"] = bd.LerString("Senha");
                    linha["DataVenda"] = bd.LerStringFormatoDataHora("DataVenda");
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentação"] = bd.LerStringFormatoDataHora("Horario");
                    linha["Setor/Produto"] = bd.LerString("Setor");
                    linha["Ingresso"] = bd.LerString("Codigo") + " - " + bd.LerString("Motivo");
                    linha["Valor"] = 0;
                    linha["Entrega"] = 0;
                    linha["Conveniência"] = 0;
                    linha["Total"] = 0;
                    linha["Quantidade"] = 0;
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return tabela;
        } // fim de FechamentoCaixa		

        public override DataTable Reimpressos()
        {
            DataTable tabela = Utilitario.EstruturaReimpressos(); //ver este select
            try
            {
                BD bd = new BD();
                string sqlReimpressos =
                    "SELECT        tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, tIngressoLog.Acao, tIngressoLog.Obs AS Motivo, tIngressoLog.ID, tVendaBilheteria.CaixaID, tEvento.Nome AS Evento " +
                    "FROM            tIngressoLog INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                    "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                    "tEvento ON tApresentacao.EventoID = tEvento.ID " +
                    "WHERE        (tIngressoLog.ID IN " +
                    "(SELECT (tIngressoLog.ID) AS IngressoLogID " +
                    "FROM            tIngressoLog INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID " +
                    "WHERE        (tIngressoLog.Acao IN (N'" + IngressoLog.REIMPRIMIR + "')) AND (tVendaBilheteria.CaixaID = " + this.Control.ID + "))) ";
                bd.Consulta(sqlReimpressos);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["Status"] = "Reimpressos";
                    linha["Senha"] = bd.LerString("Senha");
                    linha["DataVenda"] = bd.LerStringFormatoDataHora("DataVenda");
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Motivo"] = bd.LerString("Motivo");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try		
            return tabela;
        }

        /// <summary>		
        /// Obter pagamentos(recebimentos) permitidos do canal deste Caixa
        /// </summary>
        /// <returns></returns>
        public string FormaPagamentos()
        {
            try
            {
                string pagamentos = "";
                string sql =
                    "SELECT        tCanalFormaPagamento.FormaPagamentoID, tCanalFormaPagamento.CanalID, tCaixa.ID " +
                    "FROM            tCanalFormaPagamento INNER JOIN " +
                    "tCanal ON tCanalFormaPagamento.CanalID = tCanal.ID INNER JOIN " +
                    "tLoja ON tCanal.ID = tLoja.CanalID INNER JOIN " +
                    "tCaixa ON tLoja.ID = tCaixa.LojaID " +
                    "WHERE        (tCaixa.ID = " + this.Control.ID + ") ";
                bd.Consulta(sql);
                bool primeiraVez = true;
                while (bd.Consulta().Read())
                {
                    if (primeiraVez)
                    {
                        pagamentos = bd.LerInt("FormaPagamentoID").ToString();
                        primeiraVez = false;
                    }
                    else
                    {
                        pagamentos = pagamentos + "," + bd.LerInt("FormaPagamentoID").ToString();
                    }
                }
                bd.Fechar();
                return pagamentos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter Empresa, Canal e Loja
        /// </summary>
        /// <returns></returns>
        public string ObterEmpresaCanalLoja()
        {
            try
            {
                string resulatdo = "";
                string sql =
                    "SELECT        tEmpresa.Nome AS Empresa, tCanal.Nome AS Canal, tLoja.Nome AS Loja " +
                    "FROM            tCaixa INNER JOIN " +
                    "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                    "tCanal ON tLoja.CanalID = tCanal.ID INNER JOIN " +
                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID " +
                    "WHERE        (tCaixa.ID = " + this.Control.ID + ") ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    resulatdo = bd.LerString("Loja") + ", " + bd.LerString("Canal") + ", " + bd.LerString("Empresa");
                }
                bd.Fechar();
                return resulatdo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EhCanalIR(int CaixaID)
        {
            BD bd = new BD();
            try
            {
                bd.Consulta(@"SELECT Distinct e.EmpresaPromove, e.EmpresaVende 
                                FROM tCaixa	cx(NOLOCK)
                                INNER JOIN	  tLoja		l(NOLOCK) ON cx.LojaID = l.ID
                                INNER JOIN	  tCanal	ca(NOLOCK) ON 	l.CanalID = ca.ID
                                INNER JOIN	  tEmpresa	e(NOLOCK) ON  ca.EmpresaID = e.ID
                                WHERE cx.ID = " + CaixaID);

                if (!bd.Consulta().Read())
                {
                    bd.Fechar();
                    return false;
                }
                else
                {
                    if (!bd.LerBoolean("EmpresaPromove") && bd.LerBoolean("EmpresaVende"))
                    {
                        bd.Fechar();
                        return true;
                    }
                    else
                    {
                        bd.Fechar();
                        return false;
                    }
                }
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

        internal string BuscaUsuario(int caixaID)
        {
            try
            {
                string sql =
                    string.Format(@"SELECT tUsuario.Nome 
                        FROM  tCaixa (NOLOCK)
                        INNER JOIN tUsuario (NOLOCK) ON tCaixa.UsuarioID = tUsuario.ID
                        WHERE tCaixa.ID = {0} ", caixaID);
                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Erro ao procurar o Usuário");
                return bd.LerString("Nome");
            }
            finally
            {
                bd.Fechar();
            }
        }

        internal int buscaCaixaConciliacao()
        {
            try
            {
                string sql = string.Format(@" select MIN(ID) as ID from tCaixa where ConciliacaoID is null ");
                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Erro ao Buscar Caixa");
                return bd.LerInt("ID");
            }
            finally
            {
                bd.Fechar();
            }
        }
    } // fim da classe

    public class CaixaLista : CaixaLista_B
    {

        public CaixaLista() { }

        public CaixaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
