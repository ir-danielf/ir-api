using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace IRLib.Paralela
{

    public class BufferMensagens : MarshalByRefObject
    {
        private ListaMensgensSeparadas ListaMensagens { get; set; }
        private List<int> ListaIDs { get; set; }
        private DateTime UltimaExecucao { get; set; }

        private int TempoRequisicaoInicial = Convert.ToInt32(ConfigurationManager.AppSettings["TempoRequisicaoInicial"]);
        private bool Carregando { get; set; }

        public BufferMensagens()
        {
            UltimaExecucao = DateTime.MinValue;
            ListaIDs = new List<int>();
            this.ListaMensagens = new ListaMensgensSeparadas();
        }

        public void Carregar()
        {
            if (Carregando)
                return;

            BD bd = new BD();
            try
            {
                Carregando = true;
                bd.Consulta(
                    string.Format(@"SELECT 
                            tMensagem.ID, tMensagem.Titulo,
                            tMensagem.Mensagem, tMensagem.Prioriedade,
                            tMensagem.PermanecerAte, tUsuario.Nome AS Usuario,
                            tMensagemRegional.RegionalID, tMensagemCanalTipo.CanalTipoID,
                            tMensagem.AlteradoEm, tMensagem.Ativo, tMensagem.EnviadoEm
                        FROM tMensagem (NOLOCK) 
                        INNER JOIN tUsuario (NOLOCK) ON tUsuario.ID = tMensagem.UsuarioID
                        INNER JOIN tMensagemRegional (NOLOCK) ON tMensagemRegional.MensagemID = tMensagem.ID
                        INNER JOIN tMensagemCanalTipo (NOLOCK) ON tMensagemCanalTipo.MensagemID = tMensagem.ID
                        --LEFT JOIN #tmpMensagens tmp ON tmp.ID = tMensagem.ID
                        WHERE IniciarEm >= '{0}' AND PermanecerAte >= '{0}' AND Ativo = 'T'
                        ORDER BY 
                            tMensagem.ID DESC, 
                            tMensagemCanalTipo.CanalTipoID DESC, 
                            tMensagemRegional.RegionalID DESC ", DateTime.Now.ToString("yyyyMMdd") + "000000"));

                while (bd.Consulta().Read())
                {
                    if (this.ListaMensagens
                        .Where(c => c.RegionalID == bd.LerInt("RegionalID") && c.CanalTipoID == bd.LerInt("CanalTipoID") &&
                                c.Mensagens.Where(x => x.ID == bd.LerInt("ID")).Count() > 0)
                        .Count() > 0)
                    {
                        ListaMensagens.Alterar(
                            new EstruturaMensagens()
                            {
                                ID = bd.LerInt("ID"),
                                Titulo = bd.LerString("Titulo"),
                                Mensagem = bd.LerString("Mensagem"),
                                Prioridade = bd.LerInt("Prioriedade"),
                                Usuario = bd.LerString("Usuario"),
                                PermanecerAte = bd.LerDateTime("PermanecerAte"),
                                EnviadoEm = bd.LerDateTime("EnviadoEm"),
                                AlteradoEm = bd.LerDateTime("AlteradoEm"),
                                Ativo = bd.LerBoolean("Ativo"),
                                AtualizacaoEm = DateTime.Now,
                            },
                            bd.LerInt("RegionalID"),
                            bd.LerInt("CanalTipoID"));
                    }
                    else
                    {
                        if (!bd.LerBoolean("Ativo"))
                            continue;

                        if (!ListaIDs.Contains(bd.LerInt("ID")))
                            ListaIDs.Add(bd.LerInt("ID"));

                        ListaMensagens.Inserir(
                            new EstruturaMensagens()
                            {
                                ID = bd.LerInt("ID"),
                                Titulo = bd.LerString("Titulo"),
                                InseridoEm = DateTime.Now,
                                Mensagem = bd.LerString("Mensagem"),
                                Prioridade = bd.LerInt("Prioriedade"),
                                Usuario = bd.LerString("Usuario"),
                                PermanecerAte = bd.LerDateTime("PermanecerAte"),
                                EnviadoEm = bd.LerDateTime("EnviadoEm"),
                                AlteradoEm = bd.LerDateTime("AlteradoEm"),
                                AtualizacaoEm = DateTime.Now,
                                Ativo = bd.LerBoolean("Ativo"),
                            },
                            bd.LerInt("RegionalID"),
                            bd.LerInt("CanalTipoID"));
                    }
                }


                ListaMensagens.RemoverInvalidas(DateTime.Now);

                UltimaExecucao = DateTime.Now;
            }
            finally
            {
                Carregando = false;
                bd.Fechar();
            }
        }


        public List<EstruturaMensagens> BuscarMensagens(int canalTipoID, int regionalID, DateTime ultimaRequisicao)
        {
            var mensagens = this.ListaMensagens.Where(c => c.RegionalID == regionalID && c.CanalTipoID == canalTipoID).FirstOrDefault();

            if (mensagens == null)
                return null;

            //Busca somente as que devem permanecer visiveis
            var m =  mensagens
                .Mensagens
                .Where(c => c.PermanecerAte > DateTime.Now && c.AlteradoEm >= ultimaRequisicao)
                .OrderBy(c => c.AtualizacaoEm).ToList();

            return m;
        }

        public int TempoRequisicao()
        {
            return this.TempoRequisicaoInicial;
        }
    }
}
