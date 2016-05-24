/**************************************************
* Arquivo: ClienteResposta.cs
* Gerado: 01/09/2011
* Autor: Celeritas Ltda
***************************************************/

using System;

namespace IRLib.Paralela
{

    public class ClienteResposta : ClienteResposta_B
    {

        public ClienteResposta() { }

        public void Inserir(int IDPergunta, int IDCamping, string Resposta)
        {
            try
            {
                string sql = string.Empty;

                sql = @"SELECT ID From tClienteResposta (NOLOCK) Where CampingID = " + IDCamping + "AND PerguntaID = " + IDPergunta;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                }

                if (this.Control.ID == 0)
                {
                    this.PerguntaID.Valor = IDPergunta;
                    this.CampingID.Valor = IDCamping;
                    this.Resposta.Valor = Resposta;
                    this.Inserir();
                }
                else
                {
                    this.Ler(this.Control.ID);

                    this.Resposta.Valor = Resposta;

                    this.Atualizar();
                }

                this.Limpar();

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

        public void Ler(int IDPergunta, int IDCamping)
        {
            try
            {
                string sql = string.Empty;

                sql = @"SELECT ID From tClienteResposta (NOLOCK) Where CampingID = " + IDCamping + " AND PerguntaID = " + IDPergunta;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                }

                if (this.Control.ID > 0)
                {
                    this.Ler(this.Control.ID);
                }
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

        public void ExcluirRespostas(int IDCamping)
        {
            try
            {
                string sql = string.Empty;
                int[] id = new int[4];
                int cont = 0;

                sql = @"SELECT ID From tClienteResposta (NOLOCK) Where CampingID = " + IDCamping;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    id[cont++] = bd.LerInt("ID");
                }

                if (id.Length > 0)
                    ExcluirItem(id);

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

        public void ExcluirItem(int[] id)
        {
            try
            {
                foreach (int item in id)
                {
                    string sqlDelete = "DELETE FROM tClienteResposta WHERE ID = " + item;

                    int x = bd.Executar(sqlDelete);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class ClienteRespostaLista : ClienteRespostaLista_B
    {

        public ClienteRespostaLista() { }


    }

}
