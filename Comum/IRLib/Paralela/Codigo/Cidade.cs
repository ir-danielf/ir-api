/**************************************************
* Arquivo: Cidade.cs
* Gerado: 14/11/2008
* Autor: Celeritas Ltda
***************************************************/

using System.Collections.Generic;
using System.Data;

namespace IRLib.Paralela
{

    public class Cidade : Cidade_B
    {

        public Cidade() { }

        public Cidade(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public DataTable EstruturaTabela()
        {
            DataTable tabela = new DataTable("Cidade");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("EstadoID", typeof(int));
            tabela.Columns.Add("EstadoSigla", typeof(string));
            return tabela;
        }

        public DataTable TabelaPorEstado(int estadoID)
        {
            try
            {
                DataTable dtt = this.EstruturaTabela();

                bd.Consulta("SELECT ID, Nome FROM tCidade (NOLOCK) WHERE EstadoID = " + estadoID + " ORDER BY Nome");

                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EstadoID"] = estadoID;
                    dtt.Rows.Add(linha);
                }

                return dtt;
            }
            catch
            {
                throw;
            }
            finally
            { bd.Fechar(); }
        }

        public DataTable TabelaPorEstado(string estado)
        {
            try
            {
                DataTable dtt = this.EstruturaTabela();

                bd.Consulta(@"SELECT tCidade.ID, tCidade.Nome 
                                FROM tCidade (NOLOCK)
                                WHERE tCidade.EstadoSigla = '" + estado + "' ORDER BY Nome");

                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    dtt.Rows.Add(linha);
                }

                return dtt;
            }
            catch
            {
                throw;
            }
            finally
            { bd.Fechar(); }
        }

        public DataTable TabelaPorEstadocomSigla(int estadoID)
        {
            try
            {
                DataTable dtt = this.EstruturaTabela();

                bd.Consulta("SELECT ID, Nome,EstadoSigla FROM tCidade (NOLOCK) WHERE EstadoID = " + estadoID + " ORDER BY Nome");

                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EstadoID"] = estadoID;
                    linha["EstadoSigla"] = bd.LerString("EstadoSigla"); ;
                    dtt.Rows.Add(linha);
                }

                return dtt;
            }
            catch
            {
                throw;
            }
            finally
            { bd.Fechar(); }
        }

        public DataTable TabelaPorEstadocomSigla(int estadoID, bool selecione)
        {
            try
            {
                DataTable dtt = this.EstruturaTabela();

                bd.Consulta("SELECT ID, Nome,EstadoSigla FROM tCidade (NOLOCK) WHERE EstadoID = " + estadoID + " ORDER BY Nome");

                if (selecione) {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = 0;
                    linha["Nome"] = "Selecione.....";
                    linha["EstadoID"] = 0;
                    linha["EstadoSigla"] = ""; 
                    dtt.Rows.Add(linha);
                
                }


                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EstadoID"] = estadoID;
                    linha["EstadoSigla"] = bd.LerString("EstadoSigla"); 
                    dtt.Rows.Add(linha);
                }

                return dtt;
            }
            catch
            {
                throw;
            }
            finally
            { bd.Fechar(); }
        }

        public DataTable TabelaPorEstadocomSigla(string EstadoSigla)
        {
            try
            {
                DataTable dtt = this.EstruturaTabela();

                bd.Consulta("SELECT * FROM tCidade (NOLOCK) WHERE EstadoSigla = '" + EstadoSigla + "' ORDER BY Nome");

                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EstadoID"] = bd.LerInt("EstadoID");
                    linha["EstadoSigla"] = bd.LerString("EstadoSigla"); ;
                    dtt.Rows.Add(linha);
                }

                return dtt;
            }
            catch
            {
                throw;
            }
            finally
            { bd.Fechar(); }
        }


        public DataTable TabelaPorEstadocomSigla(string EstadoSigla, bool selecione)
        {
            try
            {
                DataTable dtt = this.EstruturaTabela();

                bd.Consulta("SELECT * FROM tCidade (NOLOCK) WHERE EstadoSigla = '" + EstadoSigla + "' ORDER BY Nome");

                if (selecione) {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = 0;
                    linha["Nome"] = "Selecione....";
                    linha["EstadoID"] = 0;
                    linha["EstadoSigla"] = "" ;
                    dtt.Rows.Add(linha);
                
                }

                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EstadoID"] = bd.LerInt("EstadoID");
                    linha["EstadoSigla"] = bd.LerString("EstadoSigla"); ;
                    dtt.Rows.Add(linha);
                }

                return dtt;
            }
            catch
            {
                throw;
            }
            finally
            { bd.Fechar(); }
        }
        public List<Cidade> ListaPorEstado(int estadoID)
        {
            try
            {
                List<Cidade> objListaCidade = new List<Cidade>();

                bd.Consulta("SELECT TOP 10 ID, Nome FROM tCidade(NOLOCK) WHERE EstadoID = " + estadoID);
                Cidade objCidade = null;
                while (bd.Consulta().Read())
                {
                    objCidade = new Cidade();
                    objCidade.Control.ID = bd.LerInt("ID");
                    objCidade.Nome.Valor = bd.LerString("Nome");
                    objListaCidade.Add(objCidade);
                }

                return objListaCidade;
            }
            catch
            {
                throw;
            }
            finally
            { bd.Fechar(); }
        }

        public int GetCidadeID(string cidade, string estado)
        {

            object obj = bd.ConsultaValor("SELECT ID FROM tCidade(NOLOCK) WHERE Nome like '" + cidade + "' AND EstadoSigla = '" + estado + "'");
            return (obj != null) ? (int)obj : 0;
        }

        public int BuscaCidadeID(string cidade, string estado)
        {

            object obj = bd.ConsultaValor("SELECT ID FROM tCidade(NOLOCK) WHERE Nome like '" + cidade.ToLower().Replace("a", "_").Replace("e", "_").Replace("i", "_").Replace("o", "_").Replace("u", "_") + "' AND EstadoSigla = '" + estado + "'");
            return (obj != null) ? (int)obj : 0;
        }






    }

    public class CidadeLista : CidadeLista_B
    {

        public CidadeLista() { }

        public CidadeLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
