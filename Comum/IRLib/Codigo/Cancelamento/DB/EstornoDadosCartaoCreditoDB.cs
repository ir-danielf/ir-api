using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    public abstract class EstornoDadosCartaoCredito_B : BaseBD
    {
        public canceldevolucaopendenteid CancelDevolucaoPendenteID = new canceldevolucaopendenteid();
        public vendabilheteriaidcancel VendaBilheteriaIDCancel = new vendabilheteriaidcancel();
        public vendabilheteriaidvenda VendaBilheteriaIDVenda = new vendabilheteriaidvenda();
        public bandeira Bandeira = new bandeira();
        public cartao Cartao = new cartao();
        public valor Valor = new valor();
        public cliente Cliente = new cliente();
        public cpfcliente CPFCliente = new cpfcliente();
        public cancelamentopor CancelamentoPor = new cancelamentopor();
        public email Email = new email();
        public status Status = new status();

        public EstornoDadosCartaoCredito_B() { }

        // passar o Usuario logado no sistema
        public EstornoDadosCartaoCredito_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        public override void Ler(int id)
        {
            try
            {
                string sql = "SELECT * FROM EstornoDadosCartaoCredito WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CancelDevolucaoPendenteID.ValorBD = bd.LerString("CancelDevolucaoPendenteID");
                    this.VendaBilheteriaIDCancel.ValorBD = bd.LerString("VendaBilheteriaIDCancel");
                    this.VendaBilheteriaIDVenda.ValorBD = bd.LerString("VendaBilheteriaIDVenda");
                    this.Bandeira.ValorBD = bd.LerString("Bandeira");
                    this.Cartao.ValorBD = bd.LerString("Cartao");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.Cliente.ValorBD = bd.LerString("Cliente");
                    this.CPFCliente.ValorBD = bd.LerString("CPFCliente");
                    this.CancelamentoPor.ValorBD = bd.LerString("CancelamentoPor");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.Status.ValorBD = bd.LerString("Status");
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
                string sql = "SELECT * FROM EstornoDadosCartaoCredito WHERE CancelDevolucaoPendenteID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("id");
                    this.CancelDevolucaoPendenteID.ValorBD = bd.LerString("CancelDevolucaoPendenteID");
                    this.VendaBilheteriaIDCancel.ValorBD = bd.LerString("VendaBilheteriaIDCancel");
                    this.VendaBilheteriaIDVenda.ValorBD = bd.LerString("VendaBilheteriaIDVenda");
                    this.Bandeira.ValorBD = bd.LerString("Bandeira");
                    this.Cartao.ValorBD = bd.LerString("Cartao");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.Cliente.ValorBD = bd.LerString("Cliente");
                    this.CPFCliente.ValorBD = bd.LerString("CPFCliente");
                    this.CancelamentoPor.ValorBD = bd.LerString("CancelamentoPor");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.Status.ValorBD = bd.LerString("Status");
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
            this.Bandeira.Limpar();
            this.Cartao.Limpar();
            this.Valor.Limpar();
            this.Cliente.Limpar();
            this.CPFCliente.Limpar();
            this.CancelamentoPor.Limpar();
            this.Email.Limpar();
            this.Status.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {
            this.CancelDevolucaoPendenteID.Desfazer();
            this.VendaBilheteriaIDCancel.Desfazer();
            this.VendaBilheteriaIDVenda.Desfazer();
            this.Bandeira.Desfazer();
            this.Cartao.Desfazer();
            this.Valor.Desfazer();
            this.Cliente.Desfazer();
            this.CPFCliente.Desfazer();
            this.CancelamentoPor.Desfazer();
            this.Email.Desfazer();
            this.Status.Desfazer();
        }

        public override bool Inserir()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO EstornoDadosCartaoCredito(CancelDevolucaoPendenteID, VendaBilheteriaIDCancel, VendaBilheteriaIDVenda, Bandeira, Cartao, Valor, Cliente, CPFCliente, CancelamentoPor, Email, Status, DataInsert) ");
                sql.Append("VALUES (@001, @002 , '@003' , '@004', @005, '@006', '@007', '@008', '@009', '@010' , GETDATE()); SELECT SCOPE_IDENTITY();");

                sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
                sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
                sql.Replace("@003", this.Bandeira.ValorBD);
                sql.Replace("@004", this.Cartao.ValorBD);
                sql.Replace("@005", this.Valor.ValorBD);
                sql.Replace("@006", this.Cliente.ValorBD);
                sql.Replace("@007", this.CPFCliente.ValorBD);
                sql.Replace("@008", this.CancelamentoPor.ValorBD);
                sql.Replace("@009", this.Email.ValorBD);
                sql.Replace("@010", this.Status.ValorBD);

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
            sql.Append("INSERT INTO EstornoDadosCartaoCredito(CancelDevolucaoPendenteID, VendaBilheteriaIDCancel, VendaBilheteriaIDVenda, Bandeira, Cartao, Valor, Cliente, CPFCliente, CancelamentoPor, Email, Status, DataInsert) ");
            sql.Append("VALUES (@001, @002 , '@003' , '@004', @005, '@006', '@007', '@008', '@009', '@010' , GETDATE()); SELECT SCOPE_IDENTITY();");

            sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
            sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
            sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
            sql.Replace("@003", this.Bandeira.ValorBD);
            sql.Replace("@004", this.Cartao.ValorBD);
            sql.Replace("@005", this.Valor.ValorBD);
            sql.Replace("@006", this.Cliente.ValorBD);
            sql.Replace("@007", this.CPFCliente.ValorBD);
            sql.Replace("@008", this.CancelamentoPor.ValorBD);
            sql.Replace("@009", this.Email.ValorBD);
            sql.Replace("@010", this.Status.ValorBD);


            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
            bd.Fechar();

            return this.Control.ID > 0;
        }

        public override bool Atualizar()
        {
            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE EstornoDadosCartaoCredito SET CancelDevolucaoPendenteID = @000, VendaBilheteriaIDCancel = @001, VendaBilheteriaIDVenda = @002, Bandeira = '@003', Cartao = '@004', Valor = @005, Cliente = '@006', CPFCliente = '@007', CancelamentoPor = '@008', Email = '@009' ");
                sql.Append("WHERE ID = @ID");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
                sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
                sql.Replace("@003", this.Bandeira.ValorBD);
                sql.Replace("@004", this.Cartao.ValorBD);
                sql.Replace("@005", this.Valor.ValorBD);
                sql.Replace("@006", this.Cliente.ValorBD);
                sql.Replace("@007", this.CPFCliente.ValorBD);
                sql.Replace("@008", this.CancelamentoPor.ValorBD);
                sql.Replace("@009", this.Email.ValorBD);

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
            sql.Append("UPDATE EstornoDadosCartaoCredito SET CancelDevolucaoPendenteID = @000, VendaBilheteriaIDCancel = @001, VendaBilheteriaIDVenda = @002, Bandeira = '@003', Cartao = '@004', Valor = @005, Cliente = '@006', CPFCliente = '@007', CancelamentoPor = '@008', Email = '@009', Status = '@010' ");
            sql.Append("WHERE ID = @ID");

            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
            sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
            sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
            sql.Replace("@003", this.Bandeira.ValorBD);
            sql.Replace("@004", this.Cartao.ValorBD);
            sql.Replace("@005", this.Valor.ValorBD);
            sql.Replace("@006", this.Cliente.ValorBD);
            sql.Replace("@007", this.CPFCliente.ValorBD);
            sql.Replace("@008", this.CancelamentoPor.ValorBD);
            sql.Replace("@009", this.Email.ValorBD);
            sql.Replace("@010", this.Status.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;
        }

        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM EstornoDadosCartaoCredito WHERE ID=" + id;

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
            string sqlDelete = "DELETE FROM EstornoDadosCartaoCredito WHERE ID=" + id;

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
                    return "SenhaCancelamentoID";
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

        public class bandeira : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Bandeira";
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

        public class cartao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Cartao";
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

    }
}
