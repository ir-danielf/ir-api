/**************************************************
* Arquivo: Mensagem.cs
* Gerado: 11/04/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace IRLib
{

    public class Mensagem : Mensagem_B
    {

        public Mensagem() { }

        public Mensagem(int usuarioIDLogado) : base(usuarioIDLogado) { }



        public void Inserir(EstruturaMensagens estrutura)
        {
            MensagemRegional oMensagemRegional = new MensagemRegional(this.Control.UsuarioID);
            MensagemCanalTipo oMensagemCanalTipo = new MensagemCanalTipo(this.Control.UsuarioID);

            try
            {
                this.Titulo.Valor = estrutura.Titulo;
                this.PermanecerAte.Valor = estrutura.PermanecerAte;
                this.Mensagem.Valor = estrutura.Mensagem;
                this.Prioriedade.Valor = estrutura.Prioridade;
                this.UsuarioID.Valor = this.Control.UsuarioID;
                this.EnviadoEm.Valor = DateTime.Now;
                this.AlteradoEm.Valor = DateTime.Now;
                this.Ativo.Valor = estrutura.Ativo;
                this.IniciarEm.Valor = estrutura.IniciarEm;
                this.Inserir();

                if(oMensagemRegional!=null)
                foreach (int regionalID in estrutura.Regionais)
                    oMensagemRegional.Inserir(regionalID, this.Control.ID);

                if(oMensagemCanalTipo!=null)
                foreach (int canalTipoID in estrutura.Tipos)
                    oMensagemCanalTipo.Inserir(canalTipoID, this.Control.ID);

            }
            finally
            {
                bd.Fechar();
            }
        }


        public void Atualizar(EstruturaMensagens estrutura)
        {
            MensagemRegional oMensagemRegional = new MensagemRegional(this.Control.UsuarioID);
            MensagemCanalTipo oMensagemCanalTipo = new MensagemCanalTipo(this.Control.UsuarioID);

            try
            {
                bd.IniciarTransacao();

                this.Control.ID = estrutura.ID;
                this.Titulo.Valor = estrutura.Titulo;
                this.PermanecerAte.Valor = estrutura.PermanecerAte;
                this.Mensagem.Valor = estrutura.Mensagem;
                this.Prioriedade.Valor = estrutura.Prioridade;
                this.UsuarioID.Valor = this.Control.UsuarioID;
                this.EnviadoEm.Valor = estrutura.EnviadoEm;
                this.AlteradoEm.Valor = DateTime.Now;
                this.Ativo.Valor = estrutura.Ativo;
                this.IniciarEm.Valor = estrutura.IniciarEm;
                this.Atualizar(bd);

                //Remover Regionais e adicionar dnovo... ou só comprar?? sei não...
                bd.Executar("DELETE FROM tMensagemRegional WHERE MensagemID = " + this.Control.ID);

                if (estrutura.Regionais != null)
                {
                    foreach (int regionalID in estrutura.Regionais)
                        oMensagemRegional.Inserir(regionalID, this.Control.ID);
                }
                //Remover Canais e adicionar dnovo...

                if (estrutura.Regionais != null)
                {
                    bd.Executar("DELETE FROM tMensagemCanalTipo WHERE MensagemID = " + this.Control.ID);
                    foreach (int canalTipoID in estrutura.Tipos)
                        oMensagemCanalTipo.Inserir(canalTipoID, this.Control.ID);
                }
                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }


        public List<EstruturaMensagens> Carregar(
            string titulo, int regionalID, int canalTipoID, DateTime de,
            DateTime ate, bool somenteAtivo, bool somenteDaRegional)
        {
            try
            {
                List<EstruturaMensagens> lista = new List<EstruturaMensagens>();
                string busca = string.Empty;


                if (!string.IsNullOrEmpty(titulo))
                    busca += " Titulo LIKE '%" + titulo + "%' ";

                if (regionalID > 0)
                {
                    if (!string.IsNullOrEmpty(busca))
                        busca += "AND ";

                    busca += "rm.RegionalID = " + regionalID + " ";
                }

                if (canalTipoID > 0)
                {
                    if (!string.IsNullOrEmpty(busca))
                        busca += "AND ";

                    busca += "ct.CanalTipoID = " + canalTipoID + " ";
                }

                if (de > DateTime.MinValue)
                {
                    if (!string.IsNullOrEmpty(busca))
                        busca += "AND ";
                    busca += "m.EnviadoEm >= '" + de.ToString("yyyyMMdd") + "000000' ";
                }

                if (ate > DateTime.MinValue && ate > de)
                {
                    if (!string.IsNullOrEmpty(busca))
                        busca += "AND ";
                    busca += "m.EnviadoEm <= '" + ate.AddDays(1).ToString("yyyyMMdd") + "000000' ";
                }

                if (somenteAtivo)
                {
                    if (!string.IsNullOrEmpty(busca))
                        busca += "AND ";
                    busca += "m.Ativo = 'T' ";
                }

                string sql =
                    string.Format(
                   @"SELECT 
                        m.ID, Titulo, Mensagem, Prioriedade, PermanecerAte, 
                        EnviadoEm, Ativo, AlteradoEm, COUNT(rm.ID) AS Regionais,
                        u.Nome AS Usuario, IniciarEm
                    FROM tMensagem m (NOLOCK)
                    INNER JOIN tMensagemCanalTipo ct (NOLOCK) ON m.ID = ct.MensagemID
                    INNER JOIN tMensagemRegional rm (NOLOCK) ON m.ID = rm.MensagemID
                    LEFT JOIN tUsuario u (NOLOCK) ON u.ID = m.UsuarioID
                    WHERE {0}
                    GROUP BY m.ID, Titulo, Mensagem, Prioriedade, PermanecerAte, EnviadoEm, Ativo, AlteradoEm, u.Nome, IniciarEm
                    {1}
                    ORDER BY m.Titulo, m.EnviadoEm",
                                                   busca,
                                                   somenteDaRegional ? "HAVING COUNT(rm.ID) = 1" : string.Empty);
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaMensagens()
                    {
                        ID = bd.LerInt("ID"),
                        Titulo = bd.LerString("Titulo"),
                        Mensagem = bd.LerString("Mensagem"),
                        Prioridade = bd.LerInt("Prioriedade"),
                        PermanecerAte = bd.LerDateTime("PermanecerAte"),
                        EnviadoEm = bd.LerDateTime("EnviadoEm"),
                        AlteradoEm = bd.LerDateTime("AlteradoEm"),
                        Ativo = bd.LerBoolean("Ativo"),
                        Usuario = bd.LerString("Usuario"),
                        IniciarEm = bd.LerDateTime("IniciarEm"),
                    });
                }

                foreach (EstruturaMensagens ems in lista)
                {
                    int id = ems.ID;
                    ems.Regionais = new List<int>();
                    bd.Consulta("Select RegionalID from tMensagemRegional where MensagemID = " + id);
                    while (bd.Consulta().Read())
                    {
                        ems.Regionais.Add(bd.LerInt("RegionalID"));
                    }
                }

                foreach (EstruturaMensagens ems in lista)
                {
                    int id = ems.ID;
                    ems.Tipos = new List<int>();
                    bd.Consulta("Select CanalTipoID from tMensagemCanalTipo where MensagemID = " + id);
                    while (bd.Consulta().Read())
                    {
                        ems.Tipos.Add(bd.LerInt("CanalTipoID"));
                    }
                }


                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }


        public List<int> CarregarRegionais(int mensagemID)
        {
            try
            {
                List<int> ids = new List<int>();
                bd.Consulta("SELECT RegionalID FROM tMensagemRegional WHERE MensagemID = " + mensagemID);
                while (bd.Consulta().Read())
                    ids.Add(bd.LerInt("RegionalID"));

                return ids;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<int> CarregarTipos(int mensagemID)
        {
            try
            {
                List<int> ids = new List<int>();
                bd.Consulta("SELECT CanalTipoID FROM tMensagemCanalTipo WHERE MensagemID = " + mensagemID);
                while (bd.Consulta().Read())
                    ids.Add(bd.LerInt("CanalTipoID"));

                return ids;
            }
            finally
            {
                bd.Fechar();
            }
        }

    }

    public class MensagemLista : MensagemLista_B
    {

        public MensagemLista() { }

        public MensagemLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
