using System;

namespace IRLib.Paralela.Codigo.Brainiac
{
    [CTLib.ObjectType(CTLib.ObjectType.RemotingType.SingleCall)]
    public class Gerenciador : MarshalByRefObject
    {
        #region ############# Propriedades ######################
        private string NomeCorreto { get; set; }
        private string NomeDigitado { get; set; }
        private NomeLista NomeLista { get; set; }
        private string Mensagem { get; set; }
        private bool Erro { get; set; }
        private float PorcentagemSobra { get; set; }
        private float PorcentagemAceitos { get; set; }
        private float PorcentagemGeral { get; set; }
        private float PorcentagemNomeRelevante { get; set; }

        private const string Validos = "##############################EAAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCcnN##";
        private const string Invalidos = "/'.+=´`~^º°;:,!@#$%¨*()-?:{}][&ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇçñÑ \"";
        #endregion

        /// <summary>
        /// Gerencia os Metodos de Chamada e Informa o Tipo de Retorno
        /// </summary>
        /// <param name="NomeCorreto"></param>
        /// <param name="NomeDigitado"></param>
        /// <returns></returns>
        public Retorno IniciarNomes(string NomeCorreto, string NomeDigitado)
        {
            try
            {
                this.NomeCorreto = NomeCorreto.Trim();
                this.NomeDigitado = NomeDigitado.Trim();
                this.NomeLista = new NomeLista();

                this.InicarFluxo();

                return new Retorno()
                {

                    Mensagem = this.Mensagem,
                    Porcentagem = this.PorcentagemGeral,
                    PorcentagemSobra = this.PorcentagemSobra,
                    TipoRetorno = (this.Erro ? Enumeradores.EnumTipoRetorno.ImplicarErro : Enumeradores.EnumTipoRetorno.Ok),
                };
            }
            catch (Exception ex)
            {
                return new Retorno()
                {
                    Mensagem = "Não foi possível Validar este Nome.\nErro: " + ex.Message,
                    TipoRetorno = Enumeradores.EnumTipoRetorno.ImplicarErro,
                };
            }
        }

        /// <summary>
        /// Inicia o Fluxo Dando Return em Erros já associados se necessario
        /// </summary>
        private void InicarFluxo()
        {
            this.IniciarNomeCorreto();
            this.IniciarNomeDigitado();

            //Nomes são IDENTICOS?
            if (this.CompararNomesIniciais() == Enumeradores.EnumTipoRetorno.Ok)
                return;

            //Iniciar o ShiftAnd e Reposicionamento
            this.NomeLista.Iniciar();

            //Nomes OK??
            if (this.EncontrarMargem() == Enumeradores.EnumTipoRetorno.ImplicarErro)
                return;

            //Primeiro NOME OK??
            if (this.EncontrarMargemNomeRelevante() == Enumeradores.EnumTipoRetorno.ImplicarErro)
                return;

            //Existe Sobra??
            if (this.EncontrarSobra() == Enumeradores.EnumTipoRetorno.ImplicarErro)
                return;
        }

        //Iniciar a Listagem
        private void IniciarNomeCorreto()
        {

            string tmpNome = string.Empty;
            int indice = 0;
            foreach (string nome in this.NomeCorreto.Split(' '))
            {
                tmpNome = nome.Trim().ToLower();
                if (string.IsNullOrEmpty(tmpNome.Trim()) && tmpNome.Length > 2)
                    continue;

                this.NomeLista.Add(new Nome() { Indice = indice++, StrNome = this.SomenteCaractersValidos(tmpNome), });
            }
        }

        //Iniciar a Listagem
        private void IniciarNomeDigitado()
        {
            string tmpNome = string.Empty;

            foreach (string nome in this.NomeDigitado.Split(' '))
            {
                tmpNome = nome.Trim().ToLower();
                if (string.IsNullOrEmpty(tmpNome.Trim()) && tmpNome.Length > 2)
                    continue;

                NomeLista.InserirItem(this.SomenteCaractersValidos(tmpNome));
            }
        }

        /// <summary>
        /// Faz a comparação dos Nomes
        /// </summary>
        /// <returns></returns>
        private Enumeradores.EnumTipoRetorno CompararNomesIniciais()
        {
            bool Iguais = NomeLista.Comparar();

            this.PorcentagemAceitos = Iguais ? 100f : 0f;

            return
                Iguais ? Enumeradores.EnumTipoRetorno.Ok : Enumeradores.EnumTipoRetorno.ImplicarErro;
        }

        /// <summary>
        /// Encontrar a "Margem" de aceitação ou seja a Porcentagem de Nomes
        /// </summary>
        /// <returns></returns>
        private Enumeradores.EnumTipoRetorno EncontrarMargem()
        {
            this.PorcentagemAceitos =
                 this.NomeLista.EncontrarQuantidadeAceitos();

            //Mais de X% foram aceitos?
            if (PorcentagemAceitos >= Porcentagens.Instancia.Elementos.PorcentagemNomes)
                return Enumeradores.EnumTipoRetorno.Ok;

            this.Mensagem = "A porcentagem Mínima de Nomes aceitos não foi atingida.";
            this.Erro = true;
            return Enumeradores.EnumTipoRetorno.ImplicarErro;
        }

        /// <summary>
        /// Encontrar as Sobras
        /// Correto: João Toscano
        /// Digitado: João Toscano Da Silva
        /// Resultado: > W% de Sobra
        /// </summary>
        /// <returns></returns>
        private Enumeradores.EnumTipoRetorno EncontrarSobra()
        {
            this.PorcentagemSobra =
                this.NomeLista.EncontrarSobras();

            // Sobra de nomes é Inferior a Z%?
            if (PorcentagemSobra <= Porcentagens.Instancia.Elementos.PorcentagemLimiteSobra)
                return Enumeradores.EnumTipoRetorno.Ok;

            this.Mensagem = "A porcentagem Máxima de Sobras foi excedida.";
            this.Erro = true;
            return Enumeradores.EnumTipoRetorno.ImplicarErro;
        }

        /// <summary>
        /// Encontrar a "Margem" de aceitação do Nome Relevante (Primeiro)
        /// Correto: Caolho
        /// Digitado: Kaulhus
        /// Resultado: > Y%
        /// </summary>
        /// <returns></returns>
        private Enumeradores.EnumTipoRetorno EncontrarMargemNomeRelevante()
        {
            this.PorcentagemNomeRelevante =
                this.NomeLista.EncontrarNomeRelevanteAceito();

            if (this.PorcentagemNomeRelevante >= Porcentagens.Instancia.Elementos.PorcentagemAcertosPorNomeRelevante)
                return Enumeradores.EnumTipoRetorno.Ok;

            this.Mensagem = "A porcentagem Mínima de Acertos no Nome Inicial não foi atingida.";
            this.Erro = true;
            return Enumeradores.EnumTipoRetorno.ImplicarErro;
        }

        //Da replace nos caracteres invalidos, evita problemas do tipo
        //D'Angelo 
        private string SomenteCaractersValidos(string nome)
        {
            //Substitui caracteres inválidos pelos válidos.
            for (int i = 0; i < Invalidos.Length; i++)
                nome = nome.Replace(Invalidos[i].ToString(), Validos[i].ToString()).Trim();

            return nome;
        }
    }
}
