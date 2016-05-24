/**************************************************
* Arquivo: EventoTipoDestaque.cs
* Gerado: 12/03/2012
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class EventoTipoDestaque : EventoTipoDestaque_B
    {

        public EventoTipoDestaque() { }

        public EventoTipoDestaque(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaEventoDestaque> BuscarLista(int EventoTipoID)
        {
            try
            {
                List<EstruturaEventoDestaque> lista = new List<EstruturaEventoDestaque>();

                string sql = string.Format(@"SELECT DISTINCT tEvento.ID as EventoID, tEvento.Nome, tLocal.Nome as Local, tEvento.ImagemDestaque from tEvento
                INNER JOIN tLocal (NOLOCK) ON tLocal.ID = tEvento.LocalID
                INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
                INNER JOIN tEventoSubtipo (NOLOCK) ON tEvento.EventoSubTipoID = tEventoSubtipo.ID
                Where (tEvento.FilmeID IS NULL OR tEvento.FilmeID = 0)
                AND tEventoSubtipo.EventoTipoID = {0} AND tApresentacao.Horario > '{1}%' AND tEvento.Publicar = '" + (char)Evento.PublicarTipo.PublicadoParaVenda + @"' 
                ORDER BY tEvento.Nome ", EventoTipoID, DateTime.Now.ToString("yyyyMMdd"));

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.ClientObjects.EstruturaEventoDestaque()
                    {
                        ID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Nome"),
                        Local = bd.LerString("Local"),
                        Imagem = bd.LerString("ImagemDestaque"),
                    });
                }

                return lista;
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

        public List<EstruturaEventoDestaque> BuscarLista(int EventoTipoID, string EventoNome)
        {
            try
            {
                List<EstruturaEventoDestaque> lista = new List<EstruturaEventoDestaque>();

                string sql = string.Format(@"SELECT DISTINCT tEvento.ID as EventoID, tEvento.Nome, tLocal.Nome as Local, tEvento.ImagemDestaque from tEvento
                INNER JOIN tLocal (NOLOCK) ON tLocal.ID = tEvento.LocalID
                INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
                INNER JOIN tEventoSubtipo (NOLOCK) ON tEvento.EventoSubTipoID = tEventoSubtipo.ID
                Where (tEvento.FilmeID IS NULL OR tEvento.FilmeID = 0)
                AND tEventoSubtipo.EventoTipoID = {0} AND tEvento.Nome LIKE '%{1}%' AND tApresentacao.Horario > '{2}%' AND tEvento.Publicar = '" + (char)Evento.PublicarTipo.PublicadoParaVenda + @"'
                ORDER BY tEvento.Nome ", EventoTipoID, EventoNome, DateTime.Now.ToString("yyyyMMdd"));

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.ClientObjects.EstruturaEventoDestaque()
                    {
                        ID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Nome"),
                        Local = bd.LerString("Local"),
                        Imagem = bd.LerString("ImagemDestaque"),
                    });
                }

                return lista;
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

        public bool VerificaExiste(int EventoID)
        {
            try
            {
                string sql = string.Format("select ID From tEventoTipoDestaque Where EventoID = {0}", EventoID);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    return true;
                else
                    return false;
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

        public bool Excluir(int EventoID)
        {
            try
            {
                bd.IniciarTransacao();

                string sql = string.Format("SELECT ID FROM tEventoTipoDestaque (NOLOCK) WHERE EventoID = {0}", EventoID);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    this.Control.ID = bd.LerInt("ID");

                bd.Fechar();

                if (this.Control.ID > 0)
                    if (this.Excluir())
                    {
                        int retorno = new Evento().AtualizaImagemDestaque(EventoID);

                        if (retorno > 0)
                            return true;
                        else
                            throw new Exception("Erro");
                    }

                return false;
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.FinalizarTransacao();
                bd.Fechar();
            }
        }

        public List<EstruturaEventoDestaque> BuscarListaSalvos(int EventoTipoID)
        {
            try
            {
                List<EstruturaEventoDestaque> lista = new List<EstruturaEventoDestaque>();

                string sql = string.Format(@"SELECT tEvento.ID as EventoID, tEvento.Nome, tLocal.Nome as Local
                FROM tEventoTipoDestaque (NOLOCK)
                INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tEventoTipoDestaque.EventoID
                INNER JOIN tLocal (NOLOCK) ON tLocal.ID = tEvento.LocalID
                INNER JOIN tEventoSubtipo (NOLOCK) ON tEvento.EventoSubTipoID = tEventoSubtipo.ID
                Where (tEvento.FilmeID IS NULL OR tEvento.FilmeID = 0)
                AND tEventoSubtipo.EventoTipoID = {0} ORDER BY tEvento.Nome ", EventoTipoID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.ClientObjects.EstruturaEventoDestaque()
                    {
                        ID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Nome"),
                        Local = bd.LerString("Local"),
                    });
                }

                return lista;
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

        public List<EstruturaEventoDestaque> BuscarListaSalvos(int EventoTipoID, string EventoNome)
        {
            try
            {
                List<EstruturaEventoDestaque> lista = new List<EstruturaEventoDestaque>();

                string sql = string.Format(@"SELECT tEvento.ID as EventoID, tEvento.Nome, tLocal.Nome as Local
                FROM tEventoTipoDestaque (NOLOCK)
                INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tEventoTipoDestaque.EventoID
                INNER JOIN tLocal (NOLOCK) ON tLocal.ID = tEvento.LocalID
                INNER JOIN tEventoSubtipo (NOLOCK) ON tEvento.EventoSubTipoID = tEventoSubtipo.ID
                Where (tEvento.FilmeID IS NULL OR tEvento.FilmeID = 0)
                AND tEventoSubtipo.EventoTipoID = {0} AND tEvento.Nome LIKE '%{1}%' ORDER BY tEvento.Nome ", EventoTipoID, EventoNome);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.ClientObjects.EstruturaEventoDestaque()
                    {
                        ID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Nome"),
                        Local = bd.LerString("Local"),
                    });
                }

                return lista;
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
    }

    public class EventoTipoDestaqueLista : EventoTipoDestaqueLista_B
    {

        public EventoTipoDestaqueLista() { }

        public EventoTipoDestaqueLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}