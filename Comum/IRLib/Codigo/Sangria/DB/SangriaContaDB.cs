using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    public class SangriaConta_B : BaseBD
    {
        public contalojaid ContaLojaID = new contalojaid();
        public usuarioid UsuarioID = new usuarioid();
        public formapagamentoid FormaPagamentoID = new formapagamentoid();
        public bancoid BancoID = new bancoid();
        public agencia Agencia = new agencia();
        public conta Conta = new conta();
        public datahora DataHora = new datahora();
        public valor Valor = new valor();
        public locaid LocalID = new locaid();
        public eventoid EventoID = new eventoid();
        public sangriacontatipoid SangriaContaTipoID = new sangriacontatipoid();
        public sangriatipotipooutros SangriaTipoTipoOutros = new sangriatipotipooutros();
        public sangriacontaprodutorid SangriaContaProdutorID = new sangriacontaprodutorid();

        #region Classes de dados

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
        public class usuarioid : IntegerProperty
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
        public class bancoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "BancoID";
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
                    return 5;
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
                    return 10;
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
        public class locaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalID";
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
        public class eventoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EventoID";
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
        public class sangriacontatipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SangriaContaTipoID";
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
        public class sangriatipotipooutros : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "SangriaTipoTipoOutros";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 100;
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

                string sql = "SELECT * FROM SangriaConta WHERE ID = " + id;
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
            this.ContaLojaID.Limpar();
            this.UsuarioID.Limpar();
            this.FormaPagamentoID.Limpar();
            this.BancoID.Limpar();
            this.Agencia.Limpar();
            this.Conta.Limpar();
            this.DataHora.Limpar();
            this.Valor.Limpar();
            this.LocalID.Limpar();
            this.EventoID.Limpar();
            this.SangriaContaTipoID.Limpar();
            this.SangriaTipoTipoOutros.Limpar();
            this.SangriaContaProdutorID.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {
            this.Control.Desfazer();

            this.ContaLojaID.Desfazer();
            this.UsuarioID.Desfazer();
            this.FormaPagamentoID.Desfazer();
            this.BancoID.Desfazer();
            this.Agencia.Desfazer();
            this.Conta.Desfazer();
            this.DataHora.Desfazer();
            this.Valor.Desfazer();
            this.LocalID.Desfazer();
            this.EventoID.Desfazer();
            this.SangriaContaTipoID.Desfazer();
            this.SangriaTipoTipoOutros.Desfazer();
            this.SangriaContaProdutorID.Desfazer();
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

                string sqlDelete = "DELETE FROM SangriaConta WHERE ID=" + id;

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
