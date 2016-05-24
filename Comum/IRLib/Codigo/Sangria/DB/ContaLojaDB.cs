using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    public class ContaLoja_B : BaseBD
    {
        public lojaid LojaID = new lojaid();
        public datahora DataHora = new datahora();
        public valor Valor = new valor();
        public contatipomovimentacaoid ContaTipoMovimentacaoID = new contatipomovimentacaoid();

        #region Classes de dados
        public class lojaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LojaID";
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
        public class contatipomovimentacaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContaTipoMovimentacaoID";
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
                string sql = "SELECT * FROM ContaLoja(NOLOCK) WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.DataHora.ValorBD = bd.LerDateTime("DataHora").ToString();
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
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
            this.LojaID.Limpar();
            this.DataHora.Limpar();
            this.Valor.Limpar();
            this.ContaTipoMovimentacaoID.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {
            this.Control.Desfazer();

            this.LojaID.Desfazer();
            this.DataHora.Desfazer();
            this.Valor.Desfazer();
            this.ContaTipoMovimentacaoID.Desfazer();
        }

        public override bool Inserir()
        {
            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO ContaLoja (LojaID,DataHora,Valor,ContaTipoMovimentacaoID)");
                sql.Append("VALUES (@001, '@002', @003, @004); SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.LojaID.ValorBD);
                sql.Replace("@002", this.DataHora.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.ContaTipoMovimentacaoID.ValorBD);
                
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

        public override bool Atualizar()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE ContaLoja SET LojaID = @001, DataHora = @002,Valor = @003,ContaTipoMovimentacaoID = @004 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LojaID.ValorBD);
                sql.Replace("@002", this.DataHora.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.ContaTipoMovimentacaoID.ValorBD);
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

        public override bool Excluir(int id)
        {
            try
            {
                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlDelete = "DELETE FROM ContaLoja WHERE ID=" + id;

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
