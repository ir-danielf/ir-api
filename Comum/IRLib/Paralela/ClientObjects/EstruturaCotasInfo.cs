using System;
using System.Data;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]

    public class EstruturaCotasInfo
    {
        /* Mudou Apresentacao - Verifica se Tem CotaID ( Vem do BUFFER )
         * Mudou Setor        - Verifica se tem CotaID ( Vem do BUFFER )
         * Reservar Lugar Marcado - Verifica se ja carregou as informacoes daquela cota no buffer, 
         * caso positivo atribui true ao hint informando que encontrou os itens 
         *      da tabela CotaItem assim nao indo até o banco novamente para retornar os itens daquela cota
         * caso negativo carrega os itens apartir do CotaID e percorre um for para verificar os itens, se encontrar o Item daquele preço
         *      atribui as informacoes na estrutura e busca a quantidade ja vendida no banco (tCotaItemControle)
         *      
         * 
         */



        #region Properties
        public int DonoID { get; set; }
        public int IngressoID { get; set; }
        public int ReservaID { get; set; }

        public int CotaID_ApresentacaoSetor { get; set; }
        public int CotaID_Apresentacao { get; set; }

        public int QuantidadeCota { get; set; }
        public int QuantidadeCotaAPS { get; set; }
        public int QuantidadeApresentacao { get; set; }
        public int QuantidadeApresentacaoSetor { get; set; }


        public int QuantidadePorClienteCota { get; set; }
        public int QuantidadePorClienteCotaAPS { get; set; }
        public int QuantidadePorClienteApresentacao { get; set; }
        public int QuantidadePorClienteApresentacaoSetor { get; set; }

        public bool naoTemItem { get; set; }
        public bool naoTemItemAPS { get; set; }

        public bool TemCotaItem { get; set; }
        public bool TemCotaItemAPS { get; set; }

        public bool EncontrouCotaItem { get; set; }
        public bool EncontrouCotaItemAPS { get; set; }

        private int VendidaAP { get; set; }
        private int VendidaAPS { get; set; }


        public int ApresentacaoID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public int SetorID { get; set; }


        public string CodigoPromocional { get; set; }

        public int MaximaCotaItem { get; set; }
        public int MaximaCotaItemAPS { get; set; }

        public int MaximaPorClienteCotaItem { get; set; }
        public int MaximaPorClienteCotaItemAPS { get; set; }
        public int MaximaCodigo { get; set; }

        public int MaximaApresentacao { get; set; }
        public int MaximaPorClienteApresentacao { get; set; }

        public int MaximaApresentacaoSetor { get; set; }
        public int MaximaPorClienteApresentacaoSetor { get; set; }

        public int CotaItemID { get; set; }
        public int CotaItemID_APS { get; set; }

        public bool ValidaBin { get; set; }
        public bool ValidaBinAPS { get; set; }

        public int ParceiroID { get; set; }
        public int ParceiroIDAPS { get; set; }
        public string Pais { get; set; }
        public int[] QuantidadeJaVendida { get; set; }
        public int QuantidadeReservada { get; set; }

        public bool BuscaFormaPagamento { get; set; }
        public bool BuscaFormaPagamentoAPS { get; set; }

        public bool BuscaObrigatoriedade { get; set; }
        public bool BuscaObrigatoriedadeAPS { get; set; }

        public bool CodigoPromoVisivel { get; set; }
        public string CodigoPromo { get; set; }
        public string PrecoNome { get; set; }
        public string CPF { get; set; }
        public string Identificacao { get; set; }
        public string Evento { get; set; }

        public bool CPFResponsavel { get; set; }
        public bool TemTermo { get; set; }
        public bool Nominal { get; set; }
        #endregion

        public void setQuantidadeApresentacaoZero()
        {
            this.QuantidadeApresentacao = 0;
            this.QuantidadePorClienteApresentacao = 0;
        }
        public void setQuantidadeCotaZero()
        {
            this.QuantidadeCota = 0;
            this.QuantidadePorClienteCota = 0;
            this.QuantidadeCotaAPS = 0;
            this.QuantidadePorClienteCotaAPS = 0;
        }
        public void setQuantidadeApresentacaoSetorZero()
        {
            this.QuantidadeApresentacaoSetor = 0;
            this.QuantidadePorClienteApresentacaoSetor = 0;
        }
        public void setValidaBinFalse()
        {
            this.ValidaBin = false;
            this.ValidaBinAPS = false;
        }
        public void setCotaItemZero()
        {
            this.CotaItemID = 0;
            this.CotaItemID_APS = 0;
        }
        public void setEncontrouFalse()
        {
            this.EncontrouCotaItem = false;
            this.EncontrouCotaItemAPS = false;
        }
        public void setTemItemFalse()
        {
            this.TemCotaItem = false;
            this.TemCotaItemAPS = false;
        }

        public void sumQuantidadeVendidaReservar(int QuantidadeAP, int QuantidadeCotaItem)
        {
            this.QuantidadeJaVendida[0] += QuantidadeAP;
            this.QuantidadeJaVendida[1] += QuantidadeAP;
            this.QuantidadeJaVendida[2] += QuantidadeCotaItem;
            this.QuantidadeJaVendida[3] += QuantidadeCotaItem;
        }



        public void LimparEstrutura()
        {
            this.setValidaBinFalse();
            this.setTemItemFalse();
            this.setCotaItemZero();
            this.setQuantidadeCotaZero();
            this.BuscaFormaPagamento = true;
            this.BuscaFormaPagamentoAPS = true;

            this.BuscaObrigatoriedade = true;
            this.BuscaObrigatoriedadeAPS = true;
            this.EncontrouCotaItem = false;
            this.EncontrouCotaItemAPS = false;
            this.ParceiroID = 0;
            this.ParceiroIDAPS = 0;
        }



        public bool temCota()
        {
            if (this.CotaID_Apresentacao > 0 || this.CotaID_ApresentacaoSetor > 0)
                return true;
            else
                return false;
        }

        [Obsolete("Remover este metodo")]
        public int getCotaID()
        {
            return this.CotaID_ApresentacaoSetor > 0 ? this.CotaID_ApresentacaoSetor : this.CotaID_Apresentacao;
        }

        public int getQuantidadeMaxima()
        {
            int[] Quantidades = new int[3] { this.QuantidadeApresentacao, this.QuantidadeApresentacaoSetor, this.QuantidadeCota };

            Array.Sort(Quantidades);

            for (int i = 0; i <= Quantidades.Length; i++)
            {
                if (i == 3)
                    return 0;
                if (Quantidades[i] > 0)
                    return Quantidades[i];
            }
            return 0;
        }

        public int getQuantidadeMaximaCliente()
        {
            int[] Quantidades = new int[3] { this.QuantidadePorClienteCota, this.QuantidadePorClienteApresentacaoSetor, this.QuantidadePorClienteApresentacao };

            Array.Sort(Quantidades);

            for (int i = 0; i <= Quantidades.Length; i++)
            {
                if (i == 3)
                    return 0;
                if (Quantidades[i] > 0)
                    return Quantidades[i];
            }
            return 0;

        }

        public int getQuantidadeMaximaVender()
        {
            int[] Quantidades = new int[2] { this.MaximaApresentacao, this.MaximaCotaItem };

            Array.Sort(Quantidades);

            for (int i = 0; i <= 2; i++)
            {
                if (i == 2)
                    return 0;
                if (Quantidades[i] > 0)
                    return Quantidades[i];
            }
            return 0;
        }

        public int getQuantidadeMaximaVenderAPS()
        {
            int[] Quantidades = new int[3] { this.MaximaApresentacao, this.MaximaApresentacaoSetor, this.MaximaCotaItemAPS };

            Array.Sort(Quantidades);

            for (int i = 0; i <= 3; i++)
            {
                if (i == 3)
                    return 0;
                if (Quantidades[i] > 0)
                    return Quantidades[i];
            }
            return 0;
        }

        public bool ValidaReserva(int QuantidadeReservar)
        {
            if (QuantidadeJaVendida[0] + QuantidadeReservar > this.QuantidadeApresentacao && this.QuantidadeApresentacao > 0)
                return false;
            else if (QuantidadeJaVendida[1] + QuantidadeReservar > this.QuantidadeApresentacaoSetor && this.QuantidadeApresentacaoSetor > 0)
                return false;
            else if (QuantidadeJaVendida[2] + QuantidadeReservar > this.QuantidadeCota && this.QuantidadeCota > 0)
                return false;
            else if (QuantidadeJaVendida[3] + QuantidadeReservar > this.QuantidadeCotaAPS && this.QuantidadeCotaAPS > 0)
                return false;
            else
                return true;
        }

        public bool ValidaQuantidade(int qtdAP, int qtdAPS, int qtdAPCotaItem, int qtdAPSCotaItem)
        {
            if (qtdAP + QuantidadeApresentacao > MaximaApresentacao && MaximaApresentacao > 0)
                return false;
            else if (qtdAPS + QuantidadeApresentacaoSetor > MaximaApresentacaoSetor && MaximaApresentacaoSetor > 0)
                return false;
            else if ((qtdAPCotaItem + QuantidadeCota > MaximaCotaItem || qtdAPSCotaItem + QuantidadeCota > MaximaCotaItem) &&
                MaximaCotaItem > 0)
                return false;
            else if ((qtdAPCotaItem + QuantidadeCotaAPS > MaximaCotaItemAPS || qtdAPSCotaItem + QuantidadeCotaAPS > MaximaCotaItemAPS) &&
                MaximaCotaItemAPS > 0)
                return false;
            else
                return true;
        }

        public string ValidaQuantidadeComMSG(int qtdAP, int qtdAPS, int qtdAPCotaItem, int qtdAPSCotaItem)
        {
            if (qtdAP + QuantidadeApresentacao > MaximaApresentacao && MaximaApresentacao > 0)
                return "A Quantidade de ingressos com preço especial excedeu o limite para a Apresentação.";
            else if (qtdAPS + QuantidadeApresentacaoSetor > MaximaApresentacaoSetor && MaximaApresentacaoSetor > 0)
                return "A Quantidade de ingressos com preço especial excedeu o limite para o Setor.";
            else if ((qtdAPCotaItem + QuantidadeCota > MaximaCotaItem || qtdAPSCotaItem + QuantidadeCota > MaximaCotaItem) &&
                MaximaCotaItem > 0)
                return "A Quantidade de ingressos com preço especial excedeu o limite de venda para a apresentacao.";
            else if ((qtdAPCotaItem + QuantidadeCotaAPS > MaximaCotaItemAPS || qtdAPSCotaItem + QuantidadeCotaAPS > MaximaCotaItemAPS) &&
                MaximaCotaItemAPS > 0)
                return "A Quantidade de ingressos com preço especial excedeu o limite de venda para o setor.";
            else
                return string.Empty;
        }

        public bool ValidaQuantidadeCliente(int qtdAP, int qtdAPS, int qtdClienteAP, int qtdClienteAPS)
        {

            if (qtdAP + QuantidadePorClienteApresentacao > MaximaPorClienteApresentacao && MaximaPorClienteApresentacao > 0)
                return false;

            else if (qtdAPS + QuantidadePorClienteApresentacaoSetor > MaximaPorClienteApresentacaoSetor && MaximaPorClienteApresentacaoSetor > 0)
                return false;

            else if ((qtdClienteAP + QuantidadePorClienteCota > MaximaPorClienteCotaItem ||
                qtdClienteAPS + QuantidadePorClienteCota > MaximaPorClienteCotaItem) && MaximaPorClienteCotaItem > 0)
                return false;
            else if ((qtdClienteAP + QuantidadePorClienteCotaAPS > MaximaPorClienteCotaItemAPS ||
                qtdClienteAPS + QuantidadePorClienteCotaAPS > MaximaPorClienteCotaItemAPS) && MaximaPorClienteCotaItemAPS > 0)
                return false;
            else
                return true;

        }

        public void Preencher(bool ApresentacaoSetor, DataRow linha)
        {
            if (!ApresentacaoSetor)
            {
                this.CotaItemID = Convert.ToInt32(linha["ID"]);
                this.QuantidadeCota = Convert.ToInt32(linha["Quantidade"]);
                this.QuantidadePorClienteCota = Convert.ToInt32(linha["QuantidadePorCliente"]);
                this.ValidaBin = Convert.ToBoolean(linha["ValidaBin"]);
                this.ParceiroID = Convert.ToInt32(linha["ParceiroID"]);
                this.Nominal = Convert.ToBoolean(linha["Nominal"]);
            }
            else
            {
                this.CotaItemID_APS = Convert.ToInt32(linha["ID"]);
                this.QuantidadeCotaAPS = Convert.ToInt32(linha["Quantidade"]);
                this.QuantidadePorClienteCotaAPS = Convert.ToInt32(linha["QuantidadePorCliente"]);
                this.ValidaBinAPS = Convert.ToBoolean(linha["ValidaBin"]);
                this.ParceiroIDAPS = Convert.ToInt32(linha["ParceiroID"]);
                this.Nominal = Convert.ToBoolean(linha["Nominal"]);
            }
        }
    }
}
