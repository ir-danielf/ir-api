/**************************************************
* Arquivo: CodigoBarraEvento.cs
* Gerado: 04/08/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{

    public class CodigoBarraEvento : CodigoBarraEvento_B
    {

        public CodigoBarraEvento() { }


        public void GerarCodigos(int apresentacaoSetorID, List<string> codigos, DateTime dataInclusao, int EventoID, BD pBD)
        {
            if (codigos.Count == 0)
                return;

            this.ApresentacaoSetorID.Valor = apresentacaoSetorID;
            this.DataInclusao.Valor = dataInclusao;
            this.Utilizado.Valor = false;
            this.EventoID.Valor = EventoID;

            foreach (string codigo in codigos)
            {
                this.Codigo.Valor = codigo;
                this.Inserir(pBD);
            }
        }

        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tCodigoBarraEvento(ApresentacaoSetorID, Codigo, DataInclusao, Utilizado, EventoID) ");
            sql.Append("VALUES (@001,'@002','@003', '@004', '@005'); SELECT SCOPE_IDENTITY();");
            sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
            sql.Replace("@002", this.Codigo.ValorBD);
            sql.Replace("@003", this.DataInclusao.ValorBD);
            sql.Replace("@004", this.Utilizado.ValorBD);
            sql.Replace("@005", this.EventoID.ValorBD);

            int x = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
            this.Control.ID = x;

            return x > 0;
        }

        public void VerificarCodigos()
        {
            List<EstruturaQuantidadeCodigosListaBranca> lista = this.BuscarInformacoes();
            if (lista == null || lista.Count == 0)
                return;

            if (Temporizador.Instancia.CodigoBarra.CriarCodigos.Valor)
                this.CriarCodigos(lista);

            this.EnviarAlerta(lista, Temporizador.Instancia.CodigoBarra.CriarCodigos.Valor);
        }

        private void CriarCodigos(List<EstruturaQuantidadeCodigosListaBranca> lista)
        {

            try
            {
                bd.IniciarTransacao();

                foreach (var item in lista)
                {
                    List<string> codigos = new CodigoBarra().BuscarListaBranca(bd, Temporizador.Instancia.CodigoBarra.Minimo.Valor);
                    this.GerarCodigos(item.ApresentacaoSetorID, codigos, DateTime.Now, item.EventoID, bd);
                }

                bd.FinalizarTransacao();
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
        }

        private List<EstruturaQuantidadeCodigosListaBranca> BuscarInformacoes()
        {
            try
            {
                List<EstruturaQuantidadeCodigosListaBranca> lista = new List<EstruturaQuantidadeCodigosListaBranca>();

                string sql =
                    string.Format(@"
                                    SELECT
	                                    r.Nome AS Regional, r.EmailAlertaRetirada AS Email, 
                                        em.Nome AS Empresa, l.Nome AS Local, e.Nome AS Evento, e.ID as EventoID,
                                        ap.Horario, aps.ID AS ApresentacaoSetorID, s.Nome AS Setor, 
                                        SUM(CASE WHEN cbe.Utilizado = 'F'
													THEN 1
													ELSE 0
												END) AS Quantidade
	                                    FROM tRegional r (NOLOCK)
	                                    INNER JOIN tEmpresa em (NOLOCK) ON r.ID = em.RegionalID
	                                    INNER JOIN tLocal l (NOLOCK) ON em.ID = l.EmpresaID
	                                    INNER JOIN tEvento e (NOLOCK) ON l.ID = e.LocalID
	                                    INNER JOIN tApresentacao ap (NOLOCK) ON ap.EventoID = e.ID
	                                    INNER JOIN tApresentacaoSetor aps (NOLOCK) ON aps.ApresentacaoID = ap.ID
	                                    INNER JOIN tSetor s (NOLOCK) ON s.ID = aps.SetorID	
	                                    INNER JOIN tCodigoBarraEvento cbe (NOLOCK) ON cbe.ApresentacaoSetorID = aps.ID
	                                    WHERE e.TipoCodigoBarra = 'B' AND ap.DisponivelVenda = 'T' AND ap.Horario > '{0}' --AND cbe.Utilizado = 'F'
	                                    GROUP BY r.Nome, r.EmailAlertaRetirada, em.Nome, l.Nome, e.Nome, aps.ID, ap.Horario, s.Nome, e.ID
                                        HAVING (SUM(CASE WHEN cbe.Utilizado = 'F'
													THEN 1
													ELSE 0
												END)) < {1}
                                    ", DateTime.Now.ToString("yyyyMMddHHmmss"), Temporizador.Instancia.CodigoBarra.Minimo.Valor);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaQuantidadeCodigosListaBranca()
                    {
                        Regional = bd.LerString("Regional"),
                        Email = bd.LerString("Email"),
                        Empresa = bd.LerString("Empresa"),
                        Local = bd.LerString("Local"),
                        Evento = bd.LerString("Evento"),
                        Horario = bd.LerDateTime("Horario"),
                        ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID"),
                        Setor = bd.LerString("Setor"),
                        Quantidade = bd.LerInt("Quantidade"),
                        EventoID = bd.LerInt("EventoID")
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private void EnviarAlerta(List<EstruturaQuantidadeCodigosListaBranca> lista, bool codigosCriados)
        {
            EnviaSMS enviar = new EnviaSMS();

            foreach (var item in lista)
            {
                if (item.Email.Length > 0)
                    ServicoEmailParalela.EnviarAlertaCodigoBarra(item.Email, item.Regional, item.Empresa, item.Local, item.Evento, item.Horario.ToString(), item.Setor, item.Quantidade, codigosCriados);

                enviar.EnviarSMS_AlertaCodigoBarra(Temporizador.Instancia.CodigoBarra.Telefones.Valor.Split(';').ToList(), item, codigosCriados);
            }
        }
    }

    public class CodigoBarraEventoLista : CodigoBarraEventoLista_B
    {
        public CodigoBarraEventoLista() { }
    }
}
