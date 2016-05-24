/**************************************************
* Arquivo: EmpresaConta.cs
* Gerado: 18/03/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Text;

namespace IRLib
{

    public class EmpresaConta : EmpresaConta_B
    {

        public EmpresaConta() { }

        public EmpresaConta(int usuarioIDLogado) : base(usuarioIDLogado) { }

        private void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cEmpresaConta (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        private void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xEmpresaConta (ID, Versao, EmpresaID, Beneficiario, Banco, Agencia, Conta, CPFCNPJ, ContaPadrao) ");
                sql.Append("SELECT ID, @V, EmpresaID, Beneficiario, Banco, Agencia, Conta, CPFCNPJ, ContaPadrao FROM tEmpresaConta WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool Gravar()
        {
            BD bd = new BD();
            bool retGravar = true;

            try
            {
                bd.IniciarTransacao();

                bd.Executar("UPDATE tEmpresaConta SET ContaPadrao = 'F'");

                if (this.Control.ID == 0)
                    retGravar = this.Inserir(bd);
                else
                    retGravar = this.Atualizar(bd);

                bd.FinalizarTransacao();

            }
            catch
            {
                retGravar = false;
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return retGravar;
        }

        /// <summary>
        /// Inserir novo(a) EmpresaConta
        /// </summary>
        /// <returns></returns>	
        private bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEmpresaConta");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEmpresaConta(ID, EmpresaID, Beneficiario, Banco, Agencia, Conta, CPFCNPJ, ContaPadrao) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.Beneficiario.ValorBD);
                sql.Replace("@003", this.Banco.ValorBD);
                sql.Replace("@004", this.Agencia.ValorBD);
                sql.Replace("@005", this.Conta.ValorBD);
                sql.Replace("@006", this.CPFCNPJ.ValorBD);
                sql.Replace("@007", this.ContaPadrao.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I", bd);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Atualiza EmpresaConta
        /// </summary>
        /// <returns></returns>	
        private bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cEmpresaConta WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U", bd);
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEmpresaConta SET EmpresaID = @001, Beneficiario = '@002', Banco = '@003', Agencia = '@004', Conta = '@005', CPFCNPJ = '@006', ContaPadrao = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.Beneficiario.ValorBD);
                sql.Replace("@003", this.Banco.ValorBD);
                sql.Replace("@004", this.Agencia.ValorBD);
                sql.Replace("@005", this.Conta.ValorBD);
                sql.Replace("@006", this.CPFCNPJ.ValorBD);
                sql.Replace("@007", this.ContaPadrao.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }        

    }

    public class EmpresaContaLista : EmpresaContaLista_B
    {

        public EmpresaContaLista() { }

        public EmpresaContaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
