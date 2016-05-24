using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    public abstract class CancelDevolucaoPendenteIngresso_B : BaseBD
    {
        public canceldevolucaopendenteid CancelDevolucaoPendenteID = new canceldevolucaopendenteid();
        public ingressoid IngressoID = new ingressoid();

        public CancelDevolucaoPendenteIngresso_B() { }

        // passar o Usuario logado no sistema
        public CancelDevolucaoPendenteIngresso_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        public override void Ler(int id)
        {
            try
            {
                string sql = "SELECT * FROM tCancelDevolucaoPendenteIngresso WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CancelDevolucaoPendenteID.ValorBD = bd.LerInt("CancelDevolucaoPendenteID").ToString();
                    this.IngressoID.ValorBD = bd.LerInt("IngressoID").ToString();
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
            this.IngressoID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {
            this.CancelDevolucaoPendenteID.Desfazer();
            this.IngressoID.Desfazer();
        }

        public override bool Inserir()
        {
            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tCancelDevolucaoPendenteIngresso(CancelDevolucaoPendenteID, IngressoID, DataInsert) ");
                sql.Append("VALUES (@001, @002, getdate()); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CancelDevolucaoPendenteID.ValorBD);
                sql.Replace("@002", this.IngressoID.ValorBD);

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
            sql.Append("INSERT INTO tCancelDevolucaoPendenteIngresso(CancelDevolucaoPendenteID, IngressoID, DataInsert) ");
            sql.Append("VALUES (@001, @002, getdate()); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.CancelDevolucaoPendenteID.ValorBD);
            sql.Replace("@002", this.IngressoID.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
            bd.Fechar();

            return this.Control.ID > 0;
        }

        public override bool Atualizar()
        {
            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCancelDevolucaoPendenteIngresso SET CancelDevolucaoPendenteID = @001, IngressoID = @002 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CancelDevolucaoPendenteID.ValorBD);
                sql.Replace("@002", this.IngressoID.ValorBD);


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
            sql.Append("UPDATE tCancelDevolucaoPendenteIngresso SET CancelDevolucaoPendenteID = @001, IngressoID = @002 ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.CancelDevolucaoPendenteID.ValorBD);
            sql.Replace("@002", this.IngressoID.ValorBD);


            int x = bd.Executar(sql.ToString());
            bd.Fechar();

            bool result = Convert.ToBoolean(x);

            return result;
        }

        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tCancelDevolucaoPendenteIngresso WHERE ID=" + id;

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

            string sqlDelete = "DELETE FROM tCancelDevolucaoPendenteIngresso WHERE ID=" + id;

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

        public class ingressoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "IngressoID";
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
    }
}
