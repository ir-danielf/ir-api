using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    public abstract class EstornoDadosDepositoBancario_B : BaseBD
    {
        public canceldevolucaopendenteid CancelDevolucaoPendenteID = new canceldevolucaopendenteid();
        public vendabilheteriaidcancel VendaBilheteriaIDCancel = new vendabilheteriaidcancel();
        public vendabilheteriaidvenda VendaBilheteriaIDVenda = new vendabilheteriaidvenda();
        public datadeposito DataDeposito = new datadeposito();
        public banco Banco = new banco();
        public agencia Agencia = new agencia();
        public conta Conta = new conta();
        public digito Digito = new digito();
        public valor Valor = new valor();
        public cliente Cliente = new cliente();
        public cpfcliente CPFCliente = new cpfcliente();
        public nomecorrentista NomeCorrentista = new nomecorrentista();
        public cpfcorrentista CPFCorrentista = new cpfcorrentista();
        public cancelamentopor CancelamentoPor = new cancelamentopor();
        public status Status = new status();
        public motivolancamentomanual MotivoLancamentoManual = new motivolancamentomanual();
        public contapoupanca ContaPoupanca = new contapoupanca();
        public email Email = new email();

        public EstornoDadosDepositoBancario_B() { }

        // passar o Usuario logado no sistema
        public EstornoDadosDepositoBancario_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        public override void Ler(int id)
        {
            try
            {
                string sql = "SELECT * FROM EstornoDadosDepositoBancario WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CancelDevolucaoPendenteID.ValorBD = bd.LerString("CancelDevolucaoPendenteID");
                    this.VendaBilheteriaIDCancel.ValorBD = bd.LerString("VendaBilheteriaIDCancel");
                    this.VendaBilheteriaIDVenda.ValorBD = bd.LerString("VendaBilheteriaIDVenda");
                    //this.DataDeposito.ValorBD = bd.LerDateTime("DataDeposito").ToString();
                    this.Banco.ValorBD = bd.LerString("Banco");
                    this.Agencia.ValorBD = bd.LerString("Agencia");
                    this.Conta.ValorBD = bd.LerString("Conta");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.Cliente.ValorBD = bd.LerString("Cliente");
                    this.CPFCliente.ValorBD = bd.LerString("CPFCliente");
                    this.NomeCorrentista.ValorBD = bd.LerString("NomeCorrentista");
                    this.CPFCorrentista.ValorBD = bd.LerString("CPFCorrentista");
                    this.CancelamentoPor.ValorBD = bd.LerString("CancelamentoPor");
                    this.Status.ValorBD = bd.LerString("Status");
                    this.MotivoLancamentoManual.ValorBD = bd.LerString("MotivoLancamentoManual");
                    this.ContaPoupanca.ValorBD = bd.LerBoolean("ContaPoupanca").ToString();
                    this.Email.ValorBD = bd.LerString("Email");
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
            finally
            {
                bd.Fechar();
            }
        }

        internal void LerPorCancelDevolucaoPendenteID(int id)
        {
            try
            {
                string sql = "SELECT * FROM EstornoDadosDepositoBancario WHERE CancelDevolucaoPendenteID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("id");
                    this.CancelDevolucaoPendenteID.ValorBD = bd.LerString("CancelDevolucaoPendenteID");
                    this.VendaBilheteriaIDCancel.ValorBD = bd.LerString("VendaBilheteriaIDCancel");
                    this.VendaBilheteriaIDVenda.ValorBD = bd.LerString("VendaBilheteriaIDVenda");
                    //this.DataDeposito.ValorBD = bd.LerDateTime("DataDeposito").ToString();
                    this.Banco.ValorBD = bd.LerString("Banco");
                    this.Agencia.ValorBD = bd.LerString("Agencia");
                    this.Conta.ValorBD = bd.LerString("Conta");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.Cliente.ValorBD = bd.LerString("Cliente");
                    this.CPFCliente.ValorBD = bd.LerString("CPFCliente");
                    this.NomeCorrentista.ValorBD = bd.LerString("NomeCorrentista");
                    this.CPFCorrentista.ValorBD = bd.LerString("CPFCorrentista");
                    this.CancelamentoPor.ValorBD = bd.LerString("CancelamentoPor");
                    this.Status.ValorBD = bd.LerString("Status");
                    this.MotivoLancamentoManual.ValorBD = bd.LerString("MotivoLancamentoManual");
                    this.ContaPoupanca.ValorBD = bd.LerBoolean("ContaPoupanca").ToString();
                    this.Email.ValorBD = bd.LerString("Email");
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
            finally
            {
                bd.Fechar();
            }
        }
        public override void Limpar()
        {
            this.CancelDevolucaoPendenteID.Limpar();
            this.VendaBilheteriaIDCancel.Limpar();
            this.VendaBilheteriaIDVenda.Limpar();
            this.DataDeposito.Limpar();
            this.Banco.Limpar();
            this.Agencia.Limpar();
            this.Conta.Limpar();
            this.Valor.Limpar();
            this.Cliente.Limpar();
            this.CPFCliente.Limpar();
            this.NomeCorrentista.Limpar();
            this.CPFCorrentista.Limpar();
            this.CancelamentoPor.Limpar();
            this.Status.Limpar();
            this.MotivoLancamentoManual.Limpar();
            this.ContaPoupanca.Limpar();
            this.Email.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {
            this.CancelDevolucaoPendenteID.Desfazer();
            this.VendaBilheteriaIDCancel.Desfazer();
            this.VendaBilheteriaIDVenda.Desfazer();
            this.Banco.Desfazer();
            this.Agencia.Desfazer();
            this.Conta.Desfazer();
            this.Valor.Desfazer();
            this.Cliente.Desfazer();
            this.CPFCliente.Desfazer();
            this.NomeCorrentista.Desfazer();
            this.CPFCorrentista.Desfazer();
            this.CancelamentoPor.Desfazer();
            this.Status.Desfazer();
            this.MotivoLancamentoManual.Desfazer();
            this.ContaPoupanca.Desfazer();
            this.Email.Desfazer();
        }

        public override bool Inserir()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO EstornoDadosDepositoBancario(CancelDevolucaoPendenteID, VendaBilheteriaIDCancel, VendaBilheteriaIDVenda, DataDeposito, Banco, Agencia, Conta, Valor, Cliente, CPFCliente, NomeCorrentista, CPFCorrentista, CancelamentoPor, Status, MotivoLancamentoManual, ContaPoupanca, Email, DataInsert) ");
                sql.Append("VALUES (@000, @001, @002, GETDATE() + 4, '@004', '@005', '@006', @007, '@008', '@009' , '@010' , '@011' , '@012' , '@013', '@014' , @015 , '@016', GETDATE()); SELECT SCOPE_IDENTITY();");

                sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
                sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
                sql.Replace("@004", this.Banco.ValorBD);
                sql.Replace("@005", this.Agencia.ValorBD);
                sql.Replace("@006", this.Conta.ValorBD);
                sql.Replace("@007", this.Valor.ValorBD);
                sql.Replace("@008", this.Cliente.ValorBD);
                sql.Replace("@009", this.CPFCliente.ValorBD);
                sql.Replace("@010", this.NomeCorrentista.ValorBD);
                sql.Replace("@011", this.CPFCorrentista.ValorBD);
                sql.Replace("@012", this.CancelamentoPor.ValorBD);
                sql.Replace("@013", this.Status.ValorBD);
                sql.Replace("@014", this.MotivoLancamentoManual.ValorBD);
                sql.Replace("@015", this.ContaPoupanca.ValorBD);
                sql.Replace("@016", this.Email.ValorBD);

                this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                bd.Fechar();

                return this.Control.ID > 0;

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

        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO EstornoDadosDepositoBancario(CancelDevolucaoPendenteID, VendaBilheteriaIDCancel, VendaBilheteriaIDVenda, DataDeposito, Banco, Agencia, Conta, Valor, Cliente, CPFCliente, NomeCorrentista, CPFCorrentista, CancelamentoPor, Status, MotivoLancamentoManual, ContaPoupanca, Email, DataInsert) ");
            sql.Append("VALUES (@000, @001, @002,  GETDATE() + 4, '@004', '@005', '@006', @007, '@008', '@009' , '@010' , '@011' , '@012' , '@013', '@014' , @015 , '@016', GETDATE()); SELECT SCOPE_IDENTITY();");

            sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
            sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
            sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
            sql.Replace("@004", this.Banco.ValorBD);
            sql.Replace("@005", this.Agencia.ValorBD);
            sql.Replace("@006", this.Conta.ValorBD);
            sql.Replace("@007", this.Valor.ValorBD);
            sql.Replace("@008", this.Cliente.ValorBD);
            sql.Replace("@009", this.CPFCliente.ValorBD);
            sql.Replace("@010", this.NomeCorrentista.ValorBD);
            sql.Replace("@011", this.CPFCorrentista.ValorBD);
            sql.Replace("@012", this.CancelamentoPor.ValorBD);
            sql.Replace("@013", this.Status.ValorBD);
            sql.Replace("@014", this.MotivoLancamentoManual.ValorBD);
            sql.Replace("@015", this.ContaPoupanca.ValorBD);
            sql.Replace("@016", this.Email.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
            bd.Fechar();

            return this.Control.ID > 0;
        }

        public override bool Atualizar()
        {
            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE EstornoDadosDepositoBancario SET CancelDevolucaoPendenteID = @000, VendaBilheteriaIDCancel = @001, VendaBilheteriaIDVenda = @002, DataDeposito = GETDATE() + 4, Banco = '@004', Agencia = '@005', Conta = '@006', Valor = @007, Cliente = '@008', CPFCliente = '@009', NomeCorrentista = '@010', CPFCorrentista = '@011', CancelamentoPor = '@012', Status = '@013', MotivoLancamentoManual = '@014', ContaPoupanca = @015, Email = '@016' ");
                sql.Append("WHERE ID = @ID");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
                sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
                sql.Replace("@004", this.Banco.ValorBD);
                sql.Replace("@005", this.Agencia.ValorBD);
                sql.Replace("@006", this.Conta.ValorBD);
                sql.Replace("@007", this.Valor.ValorBD);
                sql.Replace("@008", this.Cliente.ValorBD);
                sql.Replace("@009", this.CPFCliente.ValorBD);
                sql.Replace("@010", this.NomeCorrentista.ValorBD);
                sql.Replace("@011", this.CPFCorrentista.ValorBD);
                sql.Replace("@012", this.CancelamentoPor.ValorBD);
                sql.Replace("@013", this.Status.ValorBD);
                sql.Replace("@014", this.MotivoLancamentoManual.ValorBD);
                sql.Replace("@015", this.ContaPoupanca.ValorBD);
                sql.Replace("@016", this.Email.ValorBD);



                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

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

        public override bool Atualizar(BD bd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE EstornoDadosDepositoBancario SET CancelDevolucaoPendenteID = @000, VendaBilheteriaIDCancel = @001, VendaBilheteriaIDVenda = @002, DataDeposito = GETDATE() + 4, Banco = '@004', Agencia = '@005', Conta = '@006', Valor = @007, Cliente = '@008', CPFCliente = '@009', NomeCorrentista = '@010', CPFCorrentista = '@011', CancelamentoPor = '@012', Status = '@013', MotivoLancamentoManual = '@014', ContaPoupanca = @015, Email = '@016' ");
            sql.Append("WHERE ID = @ID");

            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
            sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
            sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
            sql.Replace("@004", this.Banco.ValorBD);
            sql.Replace("@005", this.Agencia.ValorBD);
            sql.Replace("@006", this.Conta.ValorBD);
            sql.Replace("@007", this.Valor.ValorBD);
            sql.Replace("@008", this.Cliente.ValorBD);
            sql.Replace("@009", this.CPFCliente.ValorBD);
            sql.Replace("@010", this.NomeCorrentista.ValorBD);
            sql.Replace("@011", this.CPFCorrentista.ValorBD);
            sql.Replace("@012", this.CancelamentoPor.ValorBD);
            sql.Replace("@013", this.Status.ValorBD);
            sql.Replace("@014", this.MotivoLancamentoManual.ValorBD);
            sql.Replace("@015", this.ContaPoupanca.Valor ? "1" : "0");
            sql.Replace("@016", this.Email.ValorBD);
            
            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;
        }

        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM EstornoDadosDepositoBancario WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);
                bd.Fechar();

                bool result = Convert.ToBoolean(x);
                return result;

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

        public override bool Excluir(BD bd, int id)
        {
            string sqlDelete = "DELETE FROM EstornoDadosDepositoBancario WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);
            bd.Fechar();

            bool result = Convert.ToBoolean(x);
            return result;
        }

        public override bool Excluir()
        {

            try
            {
                return this.Excluir(this.Control.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public class canceldevolucaopendenteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CancelDevolucaoPendenteID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class vendabilheteriaidcancel : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaIDCancel";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class vendabilheteriaidvenda : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaIDVenda";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class datadeposito : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataDeposito";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override DateTime Valor
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
                return base.Valor.ToString("yyyy-MM-dd HH:MM:ss");
            }

        }

        public class banco : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Banco";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class agencia : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Agencia";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class conta : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Conta";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class digito : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Digito";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class valor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "Valor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override decimal Valor
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
                return base.Valor.ToString("###,##0.00");
            }

        }

        public class cliente : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Cliente";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class cpfcliente : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CPFCliente";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class cpfcorrentista : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CPFCorrentista";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class nomecorrentista : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeCorrentista";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class cancelamentopor : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CancelamentoPor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class status : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Status";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class motivolancamentomanual : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "MotivoLancamentoManual";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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

        public class contapoupanca : BooleanProperty
        {
            public override string Nome
            {
                get
                {
                    return "ContaPoupanca";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override bool Valor
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

        public class email : TextProperty
        {
            public override string Nome
            {
                get
                {
                    return "Email";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
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
    }
}
