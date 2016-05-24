using CTLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRLib.Paralela
{
    public class EstruturaCartoesPorCliente
    {
        private const string VISA = "Images/bandeira_visa.gif";
        private const string MASTER = "Images/bandeira_master.gif";
        private const string AMEX = "Images/bandeira_amex.gif";
        private const string HIPER = "Images/hipercard_40.jpg";
        private const string AURA = "Images/aura_40.jpg";
        private const string DINERS = "Images/bandeira_diners.gif";
        private const string REDESHOP = "Images/bandeira_visa.gif";
        private const string OUTROS = "Images/bandeira_outros.jpg";

        public string Bandeira { get; set; }
        public string BandeiraNome { get; set; }

        public string Cartao { get; set; }
        public int CartaoID { get; set; }
        public string Cliente { get; set; }
        public int Registros { get; set; }
        public int IndexAtual { get; set; }
        public string Ativo { get; set; }
        public bool Visible { get; set; }
        public int QuantidadePorPagina { get; set; }
        public int Ordenacao1 { get; set; }
        public string Ordenacao { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }

        public Cliente.enumTipoBusca TipoBusca { get; set; }
        public string Filtro { get; set; }

        public bool SemPaginacao { get; set; }

        BD bd;

        public List<EstruturaCartoesPorCliente> CarregarLista()
        {
            bd = new BD();
            StringBuilder stbSQL = new StringBuilder();

            List<EstruturaCartoesPorCliente> lstRetorno = new List<EstruturaCartoesPorCliente>();
            try
            {

                stbSQL.Append("WITH tbGeral AS ( SELECT DISTINCT CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' ");
                stbSQL.Append("THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("END AS Cliente, CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' ");
	            stbSQL.Append("THEN  tCliente.CPF COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("ELSE tCliente.CNPJ COLLATE Latin1_General_CI_AI   ");
                stbSQL.Append("END AS CPF, ");
                stbSQL.Append("tCliente.Email, tCartao.NroCartao, ");
                stbSQL.Append("tCartao.ID AS CartaoID, tCartao.Ativo, tCartao.BandeiraID ");
                stbSQL.Append("FROM tCliente (NOLOCK) ");
                stbSQL.Append("INNER JOIN tCartao (NOLOCK) ON tCliente.ID = tCartao.ClienteID ");
                switch (TipoBusca)
                {
                    case IRLib.Paralela.Cliente.enumTipoBusca.Nome:
                        stbSQL.Append("WHERE tCliente.Nome LIKE '" + this.Filtro + "%'), ");
                        break;
                    case IRLib.Paralela.Cliente.enumTipoBusca.Email:
                        stbSQL.Append("WHERE tCliente.Email = '" + this.Filtro + "'), ");
                        break;
                    case IRLib.Paralela.Cliente.enumTipoBusca.CPF:
                        stbSQL.Append("WHERE tCliente.CPF = '" + this.Filtro + "'  AND (tCliente.CNPJ IS NULL or tCliente.CNPJ = '')), ");
                        break;
                    case IRLib.Paralela.Cliente.enumTipoBusca.CNPJ:
                        stbSQL.Append("WHERE tCliente.CNPJ = '" + this.Filtro + "'), ");
                        break;
                    default:
                        throw new Exception("Tipo da busca inválida.");
                }

                stbSQL.Append("tbCount AS (SELECT Count(CartaoID) AS Registros FROM tbGeral), ");
                stbSQL.Append("tbOrdenada AS (SELECT  Cliente, CPF, Email, NroCartao, CartaoID, ");
                stbSQL.Append("Ativo, BandeiraID, ROW_NUMBER() OVER (ORDER BY " + this.Ordenacao + " ) AS 'RowNumber' ");
                stbSQL.Append("FROM tbGeral) ");
                stbSQL.Append("SELECT Cliente, CPF, Email, NroCartao, CartaoID, ");
                stbSQL.Append("Ativo, BandeiraID, RowNumber, Registros ");
                stbSQL.Append("FROM tbOrdenada, tbCount ");
                if (!SemPaginacao)
                    stbSQL.Append("WHERE RowNumber BETWEEN " + IndexAtual + " AND " + (IndexAtual + QuantidadePorPagina - 1));

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    lstRetorno.Add(new EstruturaCartoesPorCliente
                    {
                        Cartao = bd.LerString("NroCartao"),
                        CartaoID = bd.LerInt("CartaoID"),
                        Cliente = bd.LerString("Cliente"),
                        Visible = bd.LerBoolean("Ativo"),
                        Ativo = bd.LerBoolean("Ativo") ? "Sim" : "Não",
                        Bandeira = this.Bandeiras(bd.LerInt("BandeiraID"))[0],
                        BandeiraNome = this.Bandeiras(bd.LerInt("BandeiraID"))[1],
                        Email = bd.LerString("Email"),
                        CPF = bd.LerString("CPF"),

                    });
                    if (this.QuantidadePorPagina != 0)
                        this.Registros = bd.LerInt("Registros");

                }
                return lstRetorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public void MontarOrdenacao()
        {
            switch (Ordenacao1)
            {
                case 2:
                    this.Ordenacao = " Cliente ASC ";
                    break;
                default:
                    this.Ordenacao = " Cliente DESC ";
                    break;
            }
        }

        public string[] Bandeiras(int bandeiraID)
        {
            string[] retorno = new string[2];
            string imagem = string.Empty;
            string nome = string.Empty;

            switch (bandeiraID)
            {
                case 2: //VISA
                    imagem = VISA;
                    nome = "Visa";
                    break;
                case 3: //MASTER
                    imagem = MASTER;
                    nome = "Master";
                    break;
                case 5: //AMEX
                    imagem = AMEX;
                    nome = "Amex";
                    break;
                case 9: //REDESHOP
                    imagem = REDESHOP;
                    nome = "Redeshop";
                    break;
                case 12: //HIPER
                    imagem = HIPER;
                    nome = "Hipercard";
                    break;
                case 13: //AURA
                    imagem = AURA;
                    nome = "Aura";
                    break;
                case 4: //DINERS
                    imagem = DINERS;
                    nome = "Diners";
                    break;
                default: //Outros
                    imagem = OUTROS;
                    nome = "Outros";
                    break;
            }
            retorno[0] = imagem;
            retorno[1] = nome;
            return retorno;
        }


    }
}
