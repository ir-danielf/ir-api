/**************************************************
* Arquivo: MusicaCabecaAgregado.cs
* Gerado: 13/12/2011
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Text;

namespace IRLib
{

    public class MusicaCabecaAgregado : MusicaCabecaAgregado_B
    {

        public MusicaCabecaAgregado() { }

        public bool Inserir(bool assinante, int eventoID)
        {

            try
            {

                MusicaCabeca oMusicaCabeca = new MusicaCabeca();

                if (!oMusicaCabeca.VerificaVaga(eventoID, assinante))
                {
                    throw new EventFullException("Não foi possível efetuar a inscrição, vagas esgotadas");
                }

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tMusicaCabecaAgregado(MusicaCabecaInscritoID, AgregadoID, Presente, DataInscricao) ");
                sql.Append("VALUES (@001,@002,'@003','@004'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.MusicaCabecaInscritoID.ValorBD);
                sql.Replace("@002", this.AgregadoID.ValorBD);
                sql.Replace("@003", this.Presente.ValorBD);
                sql.Replace("@004", this.DataInscricao.ValorBD);

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

        public void Ler(int Evento, int AgregadoID)
        {
            try
            {

                string sql = "SELECT * FROM tMusicaCabecaAgregado WHERE AgregadoID = " + AgregadoID;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.MusicaCabecaInscritoID.ValorBD = bd.LerInt("MusicaCabecaInscritoID").ToString();
                    this.AgregadoID.ValorBD = bd.LerInt("AgregadoID").ToString();
                    this.Presente.ValorBD = bd.LerString("Presente");
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

        public Assinaturas.Models.MusicaCabecaComprovantes CarregarComprovante(int MusicaCabecaAgregadoID)
        {
            try
            {

                IRLib.Assinaturas.Models.MusicaCabecaComprovantes retorno = new Assinaturas.Models.MusicaCabecaComprovantes();

                string Sql = string.Format(@"
                                SELECT  Convert(nvarchar,mci.ID) + '-' +  Convert(nvarchar,mca.ID) as ID, mci.DataInscricao, mc.Nome as Evento, mc.Data as DataEvento,a.Nome, mc.Detalhes
                                FROM tMusicaCabeca mc
                                INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                INNER JOIN tMusicaCabecaAgregado mca ON mca.MusicaCabecaInscritoID = mci.ID
                                INNER JOIN tAgregados a ON mca.AgregadoID = a.ID
                                WHERE mca.ID = {0}", MusicaCabecaAgregadoID);

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
                                SELECT mca.ID 
                                FROM tMusicaCabeca mc
                                INNER JOIN tMusicaCabecaInscrito mci ON mci.MusicaCabecaID = mc.ID
                                INNER JOIN tMusicaCabecaAgregado mca ON mca.MusicaCabecaInscritoID = mci.ID
                                INNER JOIN tAgregados a ON mca.AgregadoID = a.ID
                                WHERE mca.ID = {0} AND mci.ClienteID = {1}",comprovanteID,clienteID);

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

    public class MusicaCabecaAgregadoLista : MusicaCabecaAgregadoLista_B
    {

        public MusicaCabecaAgregadoLista() { }

    }

}
