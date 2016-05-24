using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    public partial class AssinaturaTexto : AssinaturaTexto_B
    {
        public int ID { get; set; }
        public int AssinaturaAnoID { get; set; }
        public int AssinaturaFaseID { get; set; }
        public string PaginaPrincipal { get; set; }
        public string PaginaLogin { get; set; }
        public string PaginaRodape { get; set; }
        public string Termos { get; set; }

        public override void Ler(int id)
        {
            try
            {
                string sql = "SELECT * FROM tAssinaturaTexto (NOLOCK) WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.ID = id;

                    this.AssinaturaAnoID = bd.LerInt("AssinaturaAnoID");

                    this.AssinaturaFaseID = bd.LerInt("AssinaturaFaseID");

                    this.PaginaLogin = bd.LerString("PaginaLogin");

                    this.PaginaPrincipal = bd.LerString("PaginaPrincipal");

                    this.PaginaRodape = bd.LerString("PaginaRodape");

                    this.Termos = bd.LerString("Termos");
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

        public override bool Inserir()
        {
            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tAssinaturaTexto(NOLOCK)");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaTexto(ID, AssinaturaAnoID, AssinaturaFaseID, PaginaPrincipal, PaginaLogin, PaginaRodape, Termos) ");
                sql.Append("VALUES (@ID, @001, '@002', '@003', '@004', '@005', '@006')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.AssinaturaAnoID.ToString());
                sql.Replace("@002", this.AssinaturaFaseID.ToString());
                sql.Replace("@003", this.PaginaPrincipal);
                sql.Replace("@004", this.PaginaLogin);
                sql.Replace("@005", this.PaginaRodape);
                sql.Replace("@006", this.Termos);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

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
                sql.Append("UPDATE tAssinaturaTexto SET PaginaPrincipal = '@001', PaginaLogin = '@002', PaginaRodape = '@003', Termos = '@004' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.ID.ToString());
                sql.Replace("@001", this.PaginaPrincipal);
                sql.Replace("@002", this.PaginaLogin);
                sql.Replace("@003", this.PaginaRodape);
                sql.Replace("@004", this.Termos);

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

        private void InserirControle(string p)
        {
            //throw new NotImplementedException();
        }
    }
}