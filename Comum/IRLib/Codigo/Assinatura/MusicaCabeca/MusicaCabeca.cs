/**************************************************
* Arquivo: MusicaCabeca.cs
* Gerado: 13/12/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib
{
    public class EventFullException : Exception
    {
        private String msg;
        public EventFullException(String msg)
            : base(msg)
        {
            this.msg = msg;

        }
        public String getMessage()
        {
            return msg;
        }
    }


    public class MusicaCabeca : MusicaCabeca_B
    {

        const int HorasLimite = 2;
        const int DiasExibicao = 15;

        public MusicaCabeca() { }


        public List<Assinaturas.Models.MusicaCabeca> ListaEventos(int clienteID)
        {
            try
            {
                List<IRLib.Assinaturas.Models.MusicaCabeca> lista = new List<Assinaturas.Models.MusicaCabeca>();
                List<IRLib.ClientObjects.Assinaturas.EstruturaMusicaCabeca> lstInscritos = new List<ClientObjects.Assinaturas.EstruturaMusicaCabeca>();
                List<int> lstInscritosCliente = new List<int>();

                Cliente oCliente = new Cliente();
                bool assinante = true;

                string Sql = @" SELECT mc.ID,mc.Nome,mc.Local,mc.Data,mc.QuantidadeCota,mc.QuantidadeNormal,mc.DataLimiteCota,mc.Detalhes
                                    FROM tMusicaCabeca mc (NOLOCK) 

                                SELECT mc.ID, COUNT(DISTINCT mci.ID) + COUNT(DISTINCT mca.id) as QtdInscritos, mci.Assinante
                                    FROM tMusicaCabeca mc (NOLOCK) 
                                    LEFT JOIN tMusicaCabecaInscrito mci (NOLOCK) ON mci.MusicaCabecaID = mc.ID 
                                    LEFT JOIN tMusicaCabecaAgregado mca (NOLOCK) ON mca.MusicaCabecaInscritoID = mci.ID 
                                    GROUP BY mc.ID,mc.Nome,mc.Local,mc.Data,mc.QuantidadeCota,mc.QuantidadeNormal,mc.DataLimiteCota,mc.Detalhes, mci.Assinante 

                                SELECT MusicaCabecaID 
                                    FROM tMusicaCabecaInscrito 
                                    WHERE ClienteID =" + clienteID;
                bd.Consulta(Sql);

                while (bd.Consulta().Read())
                {
                    DateTime dataEvento = bd.LerDateTime("Data");
                    lista.Add(new IRLib.Assinaturas.Models.MusicaCabeca()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome").ToUpper(),
                        Local = bd.LerString("Local"),
                        Data = dataEvento.ToString("dd/MM/yyyy"),
                        Hora = dataEvento.ToString("HH:mm"),
                        QuantidadeCota = bd.LerInt("QuantidadeCota"),
                        QuantidadeNormal = bd.LerInt("QuantidadeNormal"),
                        DataLimiteCota = bd.LerDateTime("DataLimiteCota").ToString("dd/MM/yyyy"),
                        Detalhes = bd.LerString("Detalhes"),
                    });
                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                    lstInscritos.Add(new IRLib.ClientObjects.Assinaturas.EstruturaMusicaCabeca()
                    {
                        MusicaCabecaID = bd.LerInt("ID"),
                        Qtd = bd.LerInt("QtdInscritos"),
                        Assinante = bd.LerBoolean("Assinante")
                    });

                foreach (var inscritos in lstInscritos)
                    if (inscritos.Assinante)
                        lista.SingleOrDefault(c => c.ID == inscritos.MusicaCabecaID).QuantidadeInscritosCota = inscritos.Qtd;
                    else
                        lista.SingleOrDefault(c => c.ID == inscritos.MusicaCabecaID).QuantidadeInscritosNormal = inscritos.Qtd;

                if (clienteID > 0)
                {
                    bd.Consulta().NextResult();

                    while (bd.Consulta().Read())
                        lstInscritosCliente.Add(bd.LerInt("MusicaCabecaID"));

                    foreach (var musicaCabecaID in lstInscritosCliente)
                    {
                        lista.SingleOrDefault(c => c.ID == musicaCabecaID).Inscrito = true;
                    }

                    bd.Fechar();

                    assinante = oCliente.VerificaAssinante(clienteID);
                }



                foreach (var eventos in lista)
                {
                    eventos.Disponivel = !(DateTime.ParseExact(this.corrigirData(eventos.Data, eventos.Hora), "yyyyMMddHHmm", null) <= DateTime.Now.AddHours(HorasLimite));

                    if (eventos.Disponivel)
                        eventos.Disponivel = (DateTime.Now >= DateTime.ParseExact(this.corrigirData(eventos.Data, eventos.Hora), "yyyyMMddHHmm", null).AddDays(-DiasExibicao));

                    if (eventos.Disponivel)
                    {
                        eventos.Disponivel = (eventos.QuantidadeInscritos < eventos.QuantidadeVagas);
                        if (eventos.Disponivel)
                            if (assinante)
                                eventos.Disponivel = (eventos.QuantidadeInscritosCota < eventos.QuantidadeCota);
                            else
                                if (DateTime.Now.Date < DateTime.ParseExact(this.corrigirData(eventos.DataLimiteCota, ""), "yyyyMMdd", null))
                                    eventos.Disponivel = (eventos.QuantidadeInscritosNormal < eventos.QuantidadeNormal);
                    }
                }



                return lista;
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

        public bool Inserir(Assinaturas.Models.MusicaCabeca novoMusicaCabeca)
        {
            this.AtribuirModel(novoMusicaCabeca);
            return this.Inserir();
        }

        public Assinaturas.Models.MusicaCabeca VerificaEvento(int id)
        {
            try
            {
                IRLib.Assinaturas.Models.MusicaCabeca mMusicaCabeca = new Assinaturas.Models.MusicaCabeca();

                string Sql = @"SELECT * FROM tMusicaCabeca WHERE ID = " + id;

                bd.Consulta(Sql);

                if (bd.Consulta().Read())
                {
                    DateTime dataEvento = bd.LerDateTime("Data");
                    mMusicaCabeca.ID = bd.LerInt("ID");
                    mMusicaCabeca.Nome = bd.LerString("Nome");
                    mMusicaCabeca.Local = bd.LerString("Local");
                    mMusicaCabeca.Data = dataEvento.Date.ToString("dd/MM/yyyy");
                    mMusicaCabeca.Hora = dataEvento.ToString("HH:mm");
                    mMusicaCabeca.QuantidadeCota = bd.LerInt("QuantidadeCota");
                    mMusicaCabeca.QuantidadeNormal = bd.LerInt("QuantidadeNormal");
                    mMusicaCabeca.DataLimiteCota = bd.LerDateTime("DataLimiteCota").ToString("dd/MM/yyyy");
                    mMusicaCabeca.Detalhes = bd.LerString("Detalhes");
                }

                return mMusicaCabeca;
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

        public bool Alterar(Assinaturas.Models.MusicaCabeca mMusicaCabeca)
        {
            this.AtribuirModel(mMusicaCabeca);
            return this.Atualizar();
        }

        private void AtribuirModel(Assinaturas.Models.MusicaCabeca mMusicaCabeca)
        {
            this.Control.ID = mMusicaCabeca.ID;
            this.Nome.Valor = mMusicaCabeca.Nome;
            this.Local.Valor = mMusicaCabeca.Local;

            string datafull = this.corrigirData(mMusicaCabeca.Data, mMusicaCabeca.Hora);
            string datalimite = this.corrigirData(mMusicaCabeca.DataLimiteCota, "");

            this.Data.Valor = DateTime.ParseExact(datafull, "yyyyMMddHHmm", null);
            this.QuantidadeCota.Valor = mMusicaCabeca.QuantidadeCota;
            this.QuantidadeNormal.Valor = mMusicaCabeca.QuantidadeNormal;
            this.DataLimiteCota.Valor = DateTime.ParseExact(datalimite, "yyyyMMdd", null);
            this.Detalhes.Valor = mMusicaCabeca.Detalhes;
        }

        private string corrigirData(string data, string hora)
        {
            string retorno = "";

            string[] dataCorreta = data.Split('/');

            Array.Reverse(dataCorreta);

            foreach (var item in dataCorreta)
            {
                retorno += item;
            }
            retorno += hora.Replace(":", "");

            return retorno;
        }

        public List<Assinaturas.Models.MusicaCabecaComprovantes> ListaEventosInscritos(int clienteID)
        {
            try
            {

                List<IRLib.Assinaturas.Models.MusicaCabecaComprovantes> lista = new List<Assinaturas.Models.MusicaCabecaComprovantes>();

                string Sql = string.Format(@"SELECT mci.ID, mci.DataInscricao, mc.Nome as Evento, mc.Data as DataEvento, c.Nome
                                FROM tMusicaCabeca mc
                                INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                INNER JOIN tCliente c on mci.ClienteID = c.ID
                                where mci.ClienteID = {0} 

                                SELECT  Convert(nvarchar,mci.ID) + '-' +  Convert(nvarchar,mca.ID) as ID, mci.DataInscricao, mc.Nome as Evento, mc.Data as DataEvento,a.Nome
                                FROM tMusicaCabeca mc
                                INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                INNER JOIN tMusicaCabecaAgregado mca ON mca.MusicaCabecaInscritoID = mci.ID
                                INNER JOIN tAgregados a ON mca.AgregadoID = a.ID
                                WHERE mci.ClienteID = {0}", clienteID);

                bd.Consulta(Sql);

                while (bd.Consulta().Read())
                {
                    DateTime dataEvento = bd.LerDateTime("DataEvento");
                    DateTime dataInscricao = bd.LerDateTime("DataInscricao");
                    lista.Add(new IRLib.Assinaturas.Models.MusicaCabecaComprovantes()
                    {
                        ID = bd.LerString("ID"),
                        Nome = bd.LerString("Nome").ToUpper(),
                        DataEvento = dataEvento.ToString("dd/MM/yyyy"),
                        HoraEvento = dataEvento.ToString("HH:mm"),
                        DataInscricao = dataInscricao.ToString("dd/MM/yyyy"),
                        HoraInscricao = dataInscricao.ToString("HH:mm"),
                        Evento = bd.LerString("Evento")
                    });
                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                {
                    DateTime dataEvento = bd.LerDateTime("DataEvento");
                    DateTime dataInscricao = bd.LerDateTime("DataInscricao");
                    lista.Add(new IRLib.Assinaturas.Models.MusicaCabecaComprovantes()
                    {
                        ID = bd.LerString("ID"),
                        Nome = bd.LerString("Nome").ToUpper(),
                        DataEvento = dataEvento.ToString("dd/MM/yyyy"),
                        HoraEvento = dataEvento.ToString("HH:mm"),
                        DataInscricao = dataInscricao.ToString("dd/MM/yyyy"),
                        HoraInscricao = dataInscricao.ToString("HH:mm"),
                        Evento = bd.LerString("Evento")
                    });
                }

                return lista;
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

        public List<Assinaturas.Models.MusicaCabecaInscito> ListaInscritos(int MusicaCabecaID)
        {
            try
            {

                List<IRLib.Assinaturas.Models.MusicaCabecaInscito> lista = new List<Assinaturas.Models.MusicaCabecaInscito>();

                string Sql = string.Format(@"SELECT mci.ID, c.Nome, c.CPF,c.LoginOSESP as Login, mci.Presente
                                                FROM tMusicaCabeca mc
                                                INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                                INNER JOIN tCliente c on mci.ClienteID = c.ID
                                                WHERE mc.ID = {0} 

                               SELECT  Convert(nvarchar,mci.ID) + '-' +  Convert(nvarchar,mca.ID) as ID, a.Nome , '--' as CPF,'--' as Login,mca.Presente
                                FROM tMusicaCabeca mc
                                INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                INNER JOIN tMusicaCabecaAgregado mca ON mca.MusicaCabecaInscritoID = mci.ID
                                INNER JOIN tAgregados a ON mca.AgregadoID = a.ID
                                WHERE mc.ID = {0}", MusicaCabecaID);

                bd.Consulta(Sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.Assinaturas.Models.MusicaCabecaInscito()
                    {
                        ID = bd.LerString("ID"),
                        Nome = bd.LerString("Nome").ToUpper(),
                        Login = bd.LerString("Login"),
                        CPF = bd.LerString("CPF"),
                        Presente = bd.LerBoolean("Presente"),

                    });
                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.Assinaturas.Models.MusicaCabecaInscito()
                    {
                        ID = bd.LerString("ID"),
                        Nome = bd.LerString("Nome").ToUpper(),
                        Login = bd.LerString("Login"),
                        CPF = bd.LerString("CPF"),
                        Presente = bd.LerBoolean("Presente"),
                    });
                }

                return lista;
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

        public List<Assinaturas.Models.MusicaCabecaAgregado> ListaAgregados(int MusicaCabecaID, int clienteID)
        {
            try
            {

                List<IRLib.Assinaturas.Models.MusicaCabecaAgregado> lista = new List<Assinaturas.Models.MusicaCabecaAgregado>();
                List<int> lstInscritosAgregado = new List<int>();
                Cliente oCliente = new Cliente();

                string Sql = string.Format(@"SELECT a.ID, a.Nome , '--' as CPF,'--' as RG
                                                FROM tAgregados a
                                                WHERE a.ClienteID =  {0} 

                                            SELECT mca.AgregadoID AS AgregadoID
                                                FROM tMusicaCabecaInscrito mci 
                                                LEFT JOIN tMusicaCabecaAgregado mca ON mci.ID = mca.MusicaCabecaInscritoID
                                                WHERE mci.ClienteID = {0} AND mci.MusicaCabecaID = {1}  ", clienteID, MusicaCabecaID);

                bd.Consulta(Sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.Assinaturas.Models.MusicaCabecaAgregado()
                    {
                        AgregadoID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome").ToUpper(),
                        RG = bd.LerString("RG"),
                        CPF = bd.LerString("CPF"),
                    });
                }


                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                    lstInscritosAgregado.Add(bd.LerInt("AgregadoID"));

                foreach (var agregadoID in lstInscritosAgregado)
                {
                    if (agregadoID > 0)
                        lista.SingleOrDefault(c => c.AgregadoID == agregadoID).Inscrito = true;
                }

                bd.Fechar();

                return lista;
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

        public string AlterarPresenca(string Chave)
        {

            try
            {
                string retorno = "Sim";
                var id = Chave.Split('-');
                MusicaCabecaAgregado oAgregado = new MusicaCabecaAgregado();
                MusicaCabecaInscrito oInscrito = new MusicaCabecaInscrito();

                switch (id.Length)
                {
                    case 2:
                        oAgregado.Ler(Convert.ToInt32(id[1]));
                        oAgregado.Presente.Valor = !oAgregado.Presente.Valor;
                        oAgregado.Atualizar();
                        retorno = oAgregado.Presente.Valor ? "Sim" : "Não";

                        break;
                    default:
                        oInscrito.Ler(Convert.ToInt32(id[0]));
                        oInscrito.Presente.Valor = !oInscrito.Presente.Valor;
                        oInscrito.Atualizar();
                        retorno = oInscrito.Presente.Valor ? "Sim" : "Não";

                        break;
                }

                return retorno;
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

        public object AlterarInscricao(int Evento, int clienteID)
        {
            try
            {
                string retorno = "Cancelar";

                MusicaCabecaInscrito oInscrito = new MusicaCabecaInscrito();
                oInscrito.Ler(Evento, clienteID);

                if (oInscrito.Control.ID > 0)
                {
                    oInscrito.CancelarAgregados();
                    oInscrito.Excluir();

                    retorno = "Inscrever";
                }
                else
                {
                    oInscrito.Assinante.Valor = new IRLib.Cliente().VerificaAssinante(clienteID);
                    oInscrito.MusicaCabecaID.Valor = Evento;
                    oInscrito.DataInscricao.Valor = DateTime.Now;
                    oInscrito.ClienteID.Valor = clienteID;
                    oInscrito.Inserir();
                }

                return retorno;
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

        public object AlterarInscricaoAgregado(int Evento, int clienteID, int AgregadoID)
        {
            try
            {
                string retorno = "Cancelar";

                MusicaCabecaAgregado oAgregado = new MusicaCabecaAgregado();
                oAgregado.Ler(Evento, AgregadoID);

                MusicaCabecaInscrito oInscrito = new MusicaCabecaInscrito();
                oInscrito.Ler(Evento, clienteID);

                if (oAgregado.Control.ID > 0)
                {
                    oAgregado.Excluir();

                    retorno = "Efetuar";
                }
                else
                {
                    oAgregado.AgregadoID.Valor = AgregadoID;
                    oAgregado.MusicaCabecaInscritoID.Valor = oInscrito.Control.ID;
                    oAgregado.DataInscricao.Valor = DateTime.Now;
                    oAgregado.Inserir(oInscrito.Assinante.Valor, Evento);
                }

                return retorno;
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

        public bool VerificaVaga(int musicaCabecaID, bool assinante)
        {
            try
            {
                IRLib.Assinaturas.Models.MusicaCabeca Evento = new Assinaturas.Models.MusicaCabeca();
                List<IRLib.ClientObjects.Assinaturas.EstruturaMusicaCabeca> lstInscritos = new List<ClientObjects.Assinaturas.EstruturaMusicaCabeca>();

                string Sql = string.Format(@"SELECT mc.ID,mc.Nome,mc.Local,mc.Data,mc.QuantidadeCota,mc.QuantidadeNormal,mc.DataLimiteCota,mc.Detalhes
                                    FROM tMusicaCabeca mc (NOLOCK)
                                    WHERE mc.ID = {0} 

                                SELECT mc.ID, COUNT(DISTINCT mci.ID) + COUNT(DISTINCT mca.id) as QtdInscritos, mci.Assinante
                                    FROM tMusicaCabeca mc (NOLOCK) 
                                    LEFT JOIN tMusicaCabecaInscrito mci (NOLOCK) ON mci.MusicaCabecaID = mc.ID 
                                    LEFT JOIN tMusicaCabecaAgregado mca (NOLOCK) ON mca.MusicaCabecaInscritoID = mci.ID 
                                    WHERE mc.ID = {0} 
                                    GROUP BY mc.ID,mc.Nome,mc.Local,mc.Data,mc.QuantidadeCota,mc.QuantidadeNormal,mc.DataLimiteCota,mc.Detalhes, mci.Assinante ", musicaCabecaID);

                bd.Consulta(Sql);

                while (bd.Consulta().Read())
                {
                    DateTime dataEvento = bd.LerDateTime("Data");
                    Evento = new IRLib.Assinaturas.Models.MusicaCabeca()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome").ToUpper(),
                        Local = bd.LerString("Local"),
                        Data = dataEvento.ToString("dd/MM/yyyy"),
                        Hora = dataEvento.ToString("HH:mm"),
                        QuantidadeCota = bd.LerInt("QuantidadeCota"),
                        QuantidadeNormal = bd.LerInt("QuantidadeNormal"),
                        DataLimiteCota = bd.LerDateTime("DataLimiteCota").ToString("dd/MM/yyyy"),
                        Detalhes = bd.LerString("Detalhes"),
                    };
                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                    lstInscritos.Add(new IRLib.ClientObjects.Assinaturas.EstruturaMusicaCabeca()
                    {
                        MusicaCabecaID = bd.LerInt("ID"),
                        Qtd = bd.LerInt("QtdInscritos"),
                        Assinante = bd.LerBoolean("Assinante")
                    });


                foreach (var inscritos in lstInscritos)
                    if (inscritos.Assinante)
                        Evento.QuantidadeInscritosCota = inscritos.Qtd;
                    else
                        Evento.QuantidadeInscritosNormal = inscritos.Qtd;


                Evento.Disponivel = !(DateTime.ParseExact(this.corrigirData(Evento.Data, Evento.Hora), "yyyyMMddHHmm", null) <= DateTime.Now.AddHours(HorasLimite));
                if (Evento.Disponivel)
                {
                    Evento.Disponivel = (Evento.QuantidadeInscritos < Evento.QuantidadeVagas);
                    if (Evento.Disponivel)
                        if (assinante)
                            Evento.Disponivel = (Evento.QuantidadeInscritosCota < Evento.QuantidadeCota);
                        else
                            if (DateTime.Now.Date < DateTime.ParseExact(this.corrigirData(Evento.DataLimiteCota, ""), "yyyyMMdd", null))
                                Evento.Disponivel = (Evento.QuantidadeInscritosNormal < Evento.QuantidadeNormal);
                }

                return Evento.Disponivel;
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

        public List<Assinaturas.Models.MusicaCabecaInscito> RelatorioInscritos(string Evento, string Chave, string CPF, string NomeInscrito, string NomeTitular, string DataInicial, string DataFinal)
        {
            try
            {
                bool buscaTitular = true;
                bool buscaAgregado = true;

                string filtroAgregado = "";
                string filtroTitular = "";


                if (NomeInscrito.Length > 0)
                {
                    filtroTitular += " AND c.Nome like  '%" + NomeInscrito + "%'";
                    filtroAgregado += " AND a.Nome like '%" + NomeInscrito + "%'";
                }

                if (DataInicial.Length > 0)
                {
                    filtroTitular += " AND mci.DataInscricao >= " + this.corrigirData(DataInicial, "000000");
                    filtroAgregado += " AND mca.DataInscricao >= " + this.corrigirData(DataInicial, "000000");
                }

                if (DataFinal.Length > 0)
                {
                    filtroTitular += " AND mci.DataInscricao < " + this.corrigirData(DataFinal, "999999");
                    filtroAgregado += " AND  mca.DataInscricao < " + this.corrigirData(DataFinal, "999999");
                }

                if (NomeTitular.Length > 0)
                {
                    filtroTitular += " AND c.Nome like '%" + NomeInscrito + "%'";
                    filtroAgregado += " AND c.Nome like '%" + NomeInscrito + "%'";
                }


                if (Chave.Length > 0)
                {
                    var id = Chave.Split('-');

                    switch (id.Length)
                    {
                        case 2:
                            filtroAgregado += " AND mca.ID = " + id[1];
                            buscaTitular = false;
                            break;
                        default:
                            filtroTitular += " AND mci.ID = " + id[0];
                            buscaAgregado = false;
                            break;
                    }

                }

                if (CPF.Length > 0)
                {
                    filtroTitular += " AND c.CPF =  '" + CPF + "' ";
                    buscaAgregado = false;
                }




                List<IRLib.Assinaturas.Models.MusicaCabecaInscito> lista = new List<Assinaturas.Models.MusicaCabecaInscito>();

                if (buscaTitular)
                {

                    string Sql = string.Format(@"SELECT Distinct mci.ID, c.Nome, c.CPF,c.Email,mci.DataInscricao,mci.Assinante,mci.Presente
                                                FROM tMusicaCabeca mc
                                                INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                                INNER JOIN tCliente c on mci.ClienteID = c.ID
                                                WHERE mc.ID = {0} {1}", Evento, filtroTitular);

                    bd.Consulta(Sql);

                    while (bd.Consulta().Read())
                    {
                        lista.Add(new IRLib.Assinaturas.Models.MusicaCabecaInscito()
                        {
                            ID = bd.LerString("ID"),
                            Nome = bd.LerString("Nome").ToUpper(),
                            Email = bd.LerString("Email"),
                            CPF = bd.LerString("CPF"),
                            Titular = true,
                            Assinante = bd.LerBoolean("Assinante"),
                            DataInscricao = bd.LerDateTime("DataInscricao"),
                            Presente = bd.LerBoolean("Presente")
                        });
                    }
                    bd.FecharConsulta();
                }

                if (buscaAgregado)
                {
                    string Sql2 = string.Format(@" SELECT  Convert(nvarchar,mci.ID) + '-' +  Convert(nvarchar,mca.ID) as ID, a.Nome , mci.Assinante, mca.DataInscricao,mca.Presente
                                                    FROM tMusicaCabeca mc
                                                    INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                                    INNER JOIN tMusicaCabecaAgregado mca ON mca.MusicaCabecaInscritoID = mci.ID
                                                    INNER JOIN tAgregados a ON mca.AgregadoID = a.ID
                                                    INNER JOIN tCliente c ON c.ID = mci.ClienteID
                                                    WHERE mc.ID = {0} {1}", Evento, filtroAgregado);

                    bd.Consulta(Sql2);

                    while (bd.Consulta().Read())
                    {
                        lista.Add(new IRLib.Assinaturas.Models.MusicaCabecaInscito()
                        {
                            ID = bd.LerString("ID"),
                            Nome = bd.LerString("Nome").ToUpper(),
                            Email = "--",
                            CPF = "--",
                            Assinante = bd.LerBoolean("Assinante"),
                            Titular = false,
                            DataInscricao = bd.LerDateTime("DataInscricao"),
                            Presente = bd.LerBoolean("Presente")
                        });
                    }
                }
                return lista;
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

        public List<EstruturaIDNome> BuscaEventos()
        {
            try
            {
                string sql = @"SELECT mc.ID, mc.Nome
                                FROM tMusicaCabeca mc (NOLOCK)";

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existe eventos cadastrados");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());


                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool PodeExcluir(int id)
        {
            try
            {
                bool retorno = true;

                string Sql = @"SELECT TOP 1 * FROM tMusicaCabeca mc
                                INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                WHERE mc.ID = " + id;

                bd.Consulta(Sql);

                if (bd.Consulta().Read())
                {
                    retorno = false;
                }

                return retorno;
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
    }

    public class MusicaCabecaLista : MusicaCabecaLista_B
    {

        public MusicaCabecaLista() { }

    }

}
