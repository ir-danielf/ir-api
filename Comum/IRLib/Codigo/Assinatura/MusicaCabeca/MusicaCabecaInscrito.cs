/**************************************************
* Arquivo: MusicaCabecaInscrito.cs
* Gerado: 13/12/2011
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace IRLib
{

    public class MusicaCabecaInscrito : MusicaCabecaInscrito_B
    {

        public MusicaCabecaInscrito() { }

        public override bool Inserir()
        {

            try
            {
                MusicaCabeca oMusicaCabeca = new MusicaCabeca();

                if (!oMusicaCabeca.VerificaVaga(this.MusicaCabecaID.Valor, this.Assinante.Valor))
                {
                    throw new EventFullException("Não foi possível efetuar a inscrição, vagas esgotadas");
                }

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tMusicaCabecaInscrito(ClienteID, MusicaCabecaID, Presente, Assinante, DataInscricao) ");
                sql.Append("VALUES (@001,@002,'@003','@004','@005'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.MusicaCabecaID.ValorBD);
                sql.Replace("@003", this.Presente.ValorBD);
                sql.Replace("@004", this.Assinante.ValorBD);
                sql.Replace("@005", this.DataInscricao.ValorBD);

                this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                bd.Fechar();

                return this.Control.ID > 0;

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

        public void Ler(int Evento, int clienteID)
        {
            try
            {

                string sql = "SELECT * FROM tMusicaCabecaInscrito WHERE MusicaCabecaID = " + Evento + " AND ClienteID = " + clienteID;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.MusicaCabecaID.ValorBD = bd.LerInt("MusicaCabecaID").ToString();
                    this.Presente.ValorBD = bd.LerString("Presente");
                    this.Assinante.ValorBD = bd.LerString("Assinante");
                    this.DataInscricao.ValorBD = bd.LerString("DataInscricao");
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
            finally
            {
                bd.Fechar();
            }
        }

        public void CancelarAgregados()
        {
            try
            {
                List<int> lstAgregado = new List<int>();
                string sql = "SELECT ID FROM tMusicaCabecaAgregado WHERE MusicaCabecaInscritoID = " + this.Control.ID;
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lstAgregado.Add(bd.LerInt("ID"));
                }

                MusicaCabecaAgregado oAgregado = new MusicaCabecaAgregado();
                foreach (var id in lstAgregado)
                {
                    oAgregado.Excluir(id);
                }

                bd.Fechar();

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

        public Assinaturas.Models.MusicaCabecaComprovantes CarregarComprovante(int MusicaCabecaInscritoID)
        {
            try
            {

                IRLib.Assinaturas.Models.MusicaCabecaComprovantes retorno = new Assinaturas.Models.MusicaCabecaComprovantes();

                string Sql = string.Format(@"
                                SELECT mci.ID, mci.DataInscricao, mc.Nome as Evento, mc.Data as DataEvento, c.Nome, mc.Detalhes
                                FROM tMusicaCabeca mc
                                INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                INNER JOIN tCliente c on mci.ClienteID = c.ID
                                where mci.ID = {0} ", MusicaCabecaInscritoID);

                bd.Consulta(Sql);

                while (bd.Consulta().Read())
                {
                    DateTime dataEvento = bd.LerDateTime("DataEvento");
                    DateTime dataInscricao = bd.LerDateTime("DataInscricao");

                    retorno = new IRLib.Assinaturas.Models.MusicaCabecaComprovantes()
                    {
                        ID = bd.LerString("ID"),
                        Nome = bd.LerString("Nome").ToUpper(),
                        DataEvento = dataEvento.ToString("dd/MM/yyyy"),
                        HoraEvento = dataEvento.ToString("HH:mm"),
                        DataInscricao = dataInscricao.ToString("dd/MM/yyyy"),
                        HoraInscricao = dataInscricao.ToString("HH:mm"),
                        Evento = bd.LerString("Evento"),
                        DetalhesEvento = bd.LerString("Detalhes")
                    };
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

        public bool VerificaClienteResponsavel(int clienteID, int comprovanteID)
        {
            try
            {

                bool retorno = false;

                string Sql = string.Format(@"
                                SELECT mci.ID
                                FROM tMusicaCabeca mc
                                INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                INNER JOIN tCliente c on mci.ClienteID = c.ID
                                WHERE mci.ID = {0} and mci.ClienteID = {1}", comprovanteID, clienteID);

                bd.Consulta(Sql);

                while (bd.Consulta().Read())
                {
                    retorno = true;
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

    public class MusicaCabecaInscritoLista : MusicaCabecaInscritoLista_B
    {

        public MusicaCabecaInscritoLista() { }

    }

}
