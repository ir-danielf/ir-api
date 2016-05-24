using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    public class SangriaCaixa_B : BaseBD
    {
        public caixaid CaixaID = new caixaid();
        public contalojaid ContaLojaID = new contalojaid();
        public supervisorid SupervisorID = new supervisorid();
        public formapagamentoid FormaPagamentoID = new formapagamentoid();
        public datahora DataHora = new datahora();
        public valor Valor = new valor();
        public tiposangria TipoSangria = new tiposangria();

        #region Classes de dados

        public class caixaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CaixaID";
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
        public class contalojaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContaLojaID";
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
        public class supervisorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SupervisorID";
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
        public class formapagamentoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "FormaPagamentoID";
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
        public class datahora : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataHora";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
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
                    return 12;
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
        public class tiposangria : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoSangria";
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
                return base.Valor;
            }

        }
        public class sangriacontaprodutorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SangriaContaProdutorID";
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
        #endregion

        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM SangriaCaixa WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    //TODO: carregar retorno do select para as propriedades
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

        public override void Limpar()
        {
            this.CaixaID.Limpar();
            this.ContaLojaID.Limpar();
            this.SupervisorID.Limpar();
            this.FormaPagamentoID.Limpar();
            this.DataHora.Limpar();
            this.Valor.Limpar();
            this.TipoSangria.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {
            this.Control.Desfazer();

            this.CaixaID.Desfazer();
            this.ContaLojaID.Desfazer();
            this.SupervisorID.Desfazer();
            this.FormaPagamentoID.Desfazer();
            this.DataHora.Desfazer();
            this.Valor.Desfazer();
            this.TipoSangria.Desfazer();
        }

        public override bool Inserir()
        {
            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                //TODO: Montar sql de insert

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

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
                //TODO: Montar sql de update

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

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

                string sqlDelete = "DELETE FROM SangriaCaixa WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

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
    }
}
