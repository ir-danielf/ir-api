using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaMensagens
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Usuario { get; set; }
        public int Prioridade { get; set; }

        public List<int> Tipos { get; set; }
        public List<int> Regionais { get; set; }
        public string Mensagem { get; set; }
        public DateTime PermanecerAte { get; set; }
        public DateTime EnviadoEm { get; set; }
        public DateTime InseridoEm { get; set; }
        public DateTime AlteradoEm { get; set; }
        public DateTime IniciarEm { get; set; }
        public bool Ativo { get; set; }

        /// <summary>
        /// Referente a ATUALIZACAO do Robo e Não da mensagem no sistema!!!!!!!
        /// </summary>
        public DateTime AtualizacaoEm { get; set; }

        public string RetornarPrioridade
        {
            get
            {
                switch (this.Prioridade)
                {
                    case 1:
                        return "Baixa";
                    case 2:
                        return "Média";
                    case 3:
                        return "Alta";
                    default:
                        return string.Empty;

                }
            }
        }


    }

    [Serializable]
    public class EstruturaMensagensSeparadas
    {
        public int RegionalID { get; set; }
        public int CanalTipoID { get; set; }
        public List<EstruturaMensagens> Mensagens { get; set; }
    }

    [Serializable]
    public class ListaMensgensSeparadas : List<EstruturaMensagensSeparadas>
    {
        public void Inserir(EstruturaMensagens mensagem, int regionalID, int canalTipoID)
        {
            EstruturaMensagensSeparadas item = this.Where(c => c.RegionalID == regionalID && c.CanalTipoID == canalTipoID).FirstOrDefault();
            if (item != null)
                item.Mensagens.Add(mensagem);
            else
                this.Add(new EstruturaMensagensSeparadas()
                {
                    CanalTipoID = canalTipoID,
                    RegionalID = regionalID,
                    Mensagens = new List<EstruturaMensagens>() { mensagem },
                });
        }

        public void Alterar(EstruturaMensagens mensagem, int regionalID, int canalTipoID)
        {
            EstruturaMensagensSeparadas item = this.Where(c => c.RegionalID == regionalID && c.CanalTipoID == canalTipoID).FirstOrDefault();

            var mensagemEncontrada = item.Mensagens.Where(c => c.ID == mensagem.ID).FirstOrDefault();
            if (mensagemEncontrada == null)
                return;

            if (mensagemEncontrada.AlteradoEm >= mensagem.AlteradoEm)
                return;

            if (!mensagem.Ativo)
                item.Mensagens.Remove(mensagemEncontrada);

            mensagemEncontrada.Titulo = mensagem.Titulo;
            mensagemEncontrada.Mensagem = mensagem.Mensagem;
            mensagemEncontrada.Prioridade = mensagem.Prioridade;
            mensagemEncontrada.Usuario = mensagem.Usuario;
            mensagemEncontrada.PermanecerAte = mensagem.PermanecerAte;
            mensagemEncontrada.AlteradoEm = mensagem.AlteradoEm;
            mensagemEncontrada.AtualizacaoEm = DateTime.Now;
        }

        /// <summary>
        /// Com a atualização de Canais e Regionais, deve ser possível remover uma mensagem da lista
        /// ela permanece no Client, mas os proximos q abrirem Não irão buscar essa mensagem
        /// </summary>
        /// <param name="ultimaAtualizacaoRobo"></param>
        public void RemoverInvalidas(DateTime ultimaAtualizacaoRobo)
        {
            foreach (var agrupamento in this)
                agrupamento.Mensagens.RemoveAll(c => ultimaAtualizacaoRobo.Subtract(c.AtualizacaoEm).TotalSeconds > 30);

            this.RemoveAll(c => c.Mensagens.Count == 0);
        }
    }
}
