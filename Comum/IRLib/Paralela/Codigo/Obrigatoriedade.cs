/**************************************************
* Arquivo: Obrigatoriedade.cs
* Gerado: 10/12/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class Obrigatoriedade : Obrigatoriedade_B
    {

        public Obrigatoriedade() { }

        public Obrigatoriedade(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public DataTable getInformacoes(int id)
        {
            try
            {

                string sql = "SELECT * FROM tObrigatoriedade(NOLOCK) WHERE ID = " + id;
                bd.Consulta(sql);
                DataTable dtt = this.EstruturaDataTable();
                DataRow dtr;
                if (bd.Consulta().Read())
                {
                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Nome do Cliente";
                    dtr["Adicionar"] = bd.LerBoolean("Nome");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "RG";
                    dtr["Adicionar"] = bd.LerBoolean("RG");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CPF";
                    dtr["Adicionar"] = bd.LerBoolean("CPF");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Telefone";
                    dtr["Adicionar"] = bd.LerBoolean("Telefone");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Telefone - Comercial";
                    dtr["Adicionar"] = bd.LerBoolean("TelefoneComercial");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Celular";
                    dtr["Adicionar"] = bd.LerBoolean("Celular");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Data de Nascimento";
                    dtr["Adicionar"] = bd.LerBoolean("DataNascimento");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Email";
                    dtr["Adicionar"] = bd.LerBoolean("Email");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CEP";
                    dtr["Adicionar"] = bd.LerBoolean("CEPCliente");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Endereço";
                    dtr["Adicionar"] = bd.LerBoolean("EnderecoCliente");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Número";
                    dtr["Adicionar"] = bd.LerBoolean("NumeroCliente");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Complemento";
                    dtr["Adicionar"] = bd.LerBoolean("ComplementoCliente");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Bairro";
                    dtr["Adicionar"] = bd.LerBoolean("BairroCliente");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Cidade";
                    dtr["Adicionar"] = bd.LerBoolean("CidadeCliente");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Estado";
                    dtr["Adicionar"] = bd.LerBoolean("EstadoCliente");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Nome de Entrega";
                    dtr["Adicionar"] = bd.LerBoolean("NomeEntrega");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CPF de Entrega";
                    dtr["Adicionar"] = bd.LerBoolean("CPFEntrega");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "RG de Entrega";
                    dtr["Adicionar"] = bd.LerBoolean("RGEntrega");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CEP de Entrega";
                    dtr["Adicionar"] = bd.LerBoolean("CEPEntrega");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Endereço de Entrega";
                    dtr["Adicionar"] = bd.LerBoolean("EnderecoEntrega");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Número de Entrega";
                    dtr["Adicionar"] = bd.LerBoolean("NumeroEntrega");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Complemento de Entrega";
                    dtr["Adicionar"] = bd.LerBoolean("ComplementoEntrega");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Bairro de Entrega";
                    dtr["Adicionar"] = bd.LerBoolean("BairroEntrega");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Cidade de Entrega";
                    dtr["Adicionar"] = bd.LerBoolean("CidadeEntrega");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Estado de Entrega";
                    dtr["Adicionar"] = bd.LerBoolean("EstadoEntrega");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CPF do Responsável";
                    dtr["Adicionar"] = bd.LerBoolean("CPFResponsavel");
                    dtt.Rows.Add(dtr);
                }
                else
                {
                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Nome";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "RG";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CPF";
                    dtt.Rows.Add(dtr);


                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Telefone";
                    dtt.Rows.Add(dtr);


                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Telefone - Comercial";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Celular";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Data de Nascimento";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Email";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CEP";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Endereço";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Número";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Complemento";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Bairro";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Cidade";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Estado Cliente";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Nome Entrega";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CPF Entrega";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "RG Entrega";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CEP Entrega";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Endereço de Entrega";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Número de Entrega";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Complemento de Entrega";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Bairro de Entrega";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Cidade de Entrega";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Estado de Entrega";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CPF do Responsável";
                    dtt.Rows.Add(dtr);
                }
                return dtt;
            }
            finally
            {
                bd.Fechar();
            }
        }


        public DataTable getInformacoesDono(int id)
        {
            try
            {

                string sql = "SELECT * FROM tObrigatoriedade(NOLOCK) WHERE ID = " + id;
                bd.Consulta(sql);
                DataTable dtt = this.EstruturaDataTable();
                DataRow dtr;
                if (bd.Consulta().Read())
                {
                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Nome do Cliente";
                    dtr["Adicionar"] = bd.LerBoolean("Nome");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "RG";
                    dtr["Adicionar"] = bd.LerBoolean("RG");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CPF";
                    dtr["Adicionar"] = bd.LerBoolean("CPF");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Telefone";
                    dtr["Adicionar"] = bd.LerBoolean("Telefone");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Data de Nascimento";
                    dtr["Adicionar"] = bd.LerBoolean("DataNascimento");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Email";
                    dtr["Adicionar"] = bd.LerBoolean("Email");
                    dtt.Rows.Add(dtr);


                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Nome do Responsável";
                    dtr["Adicionar"] = bd.LerBoolean("NomeResponsavel");
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CPF do Responsável";
                    dtr["Adicionar"] = bd.LerBoolean("CPFResponsavel");
                    dtt.Rows.Add(dtr);
                }
                else
                {
                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Nome do Cliente";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "RG";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CPF";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Telefone";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Data de Nascimento";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Email";
                    dtt.Rows.Add(dtr);


                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "Nome do Responsável";
                    dtt.Rows.Add(dtr);

                    dtr = dtt.NewRow();
                    dtr["Descricao"] = "CPF do Responsável";
                    dtt.Rows.Add(dtr);
                }
                return dtt;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable estrutura()
        {
            DataTable tObrigatoriedade = new DataTable();
            tObrigatoriedade.Columns.Add("ID", typeof(int));
            tObrigatoriedade.Columns.Add("Nome", typeof(bool));
            tObrigatoriedade.Columns.Add("RG", typeof(bool));
            tObrigatoriedade.Columns.Add("CPF", typeof(bool));
            tObrigatoriedade.Columns.Add("Telefone", typeof(bool));
            tObrigatoriedade.Columns.Add("TelefoneComercial", typeof(bool));
            tObrigatoriedade.Columns.Add("Celular", typeof(bool));
            tObrigatoriedade.Columns.Add("DataNascimento", typeof(bool));
            tObrigatoriedade.Columns.Add("Email", typeof(bool));
            tObrigatoriedade.Columns.Add("CEPCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("EnderecoCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("NumeroCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("ComplementoCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("BairroCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("CidadeCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("EstadoCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("NomeEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("CPFEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("RGEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("CEPEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("EnderecoEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("NumeroEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("ComplementoEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("BairroEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("CidadeEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("EstadoEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("NomeResponsavel", typeof(bool));
            tObrigatoriedade.Columns.Add("CPFResponsavel", typeof(bool));
            return tObrigatoriedade;
        }

        public DataTable getInformacoesPorCotaItem(int id)
        {
            try
            {


                DataTable dtt = this.estrutura();

                bd.Consulta("SELECT tObrigatoriedade.* FROM tObrigatoriedade (NOLOCK) INNER JOIN tCotaItem (NOLOCK) " +
                    "ON tObrigatoriedade.ID = tCotaItem.ObrigatoriedadeID WHERE tCotaItem.ID = " + id);

                if (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerBoolean("Nome");
                    linha["RG"] = bd.LerBoolean("RG");
                    linha["CPF"] = bd.LerBoolean("CPF");
                    linha["Telefone"] = bd.LerBoolean("Telefone");
                    linha["TelefoneComercial"] = bd.LerBoolean("TelefoneComercial");
                    linha["Celular"] = bd.LerBoolean("Celular");
                    linha["DataNascimento"] = bd.LerBoolean("DataNascimento");
                    linha["Email"] = bd.LerBoolean("Email");
                    linha["CEPCliente"] = bd.LerBoolean("CEPCliente");
                    linha["EnderecoCliente"] = bd.LerBoolean("EnderecoCliente");
                    linha["NumeroCliente"] = bd.LerBoolean("NumeroCliente");
                    linha["ComplementoCliente"] = bd.LerBoolean("ComplementoCliente");
                    linha["BairroCliente"] = bd.LerBoolean("BairroCliente");
                    linha["EstadoCliente"] = bd.LerBoolean("EstadoCliente");
                    linha["CidadeCliente"] = bd.LerBoolean("CidadeCliente");
                    linha["NomeEntrega"] = bd.LerBoolean("NomeEntrega");
                    linha["CPFEntrega"] = bd.LerBoolean("CPFEntrega");
                    linha["RGEntrega"] = bd.LerBoolean("RGEntrega");
                    linha["CEPEntrega"] = bd.LerBoolean("CEPEntrega");
                    linha["EnderecoEntrega"] = bd.LerBoolean("EnderecoEntrega");
                    linha["NumeroEntrega"] = bd.LerBoolean("NumeroEntrega");
                    linha["ComplementoEntrega"] = bd.LerBoolean("ComplementoEntrega");
                    linha["BairroEntrega"] = bd.LerBoolean("BairroEntrega");
                    linha["CidadeEntrega"] = bd.LerBoolean("CidadeEntrega");
                    linha["EstadoEntrega"] = bd.LerBoolean("EstadoEntrega");
                    linha["NomeResponsavel"] = bd.LerBoolean("NomeResponsavel");
                    linha["CPFResponsavel"] = bd.LerBoolean("CPFResponsavel");
                    dtt.Rows.Add(linha);
                }
                return dtt;
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

        public DataTable getInformacoesPorCotaItem(int[] IDs)
        {
            try
            {


                DataTable dtt = this.estrutura();
                bd.Consulta("SELECT tObrigatoriedade.* FROM tObrigatoriedade (NOLOCK) INNER JOIN tCotaItem (NOLOCK) " +
                     "ON tObrigatoriedade.ID = tCotaItem.ObrigatoriedadeID WHERE tCotaItem.ID IN( " + Utilitario.ArrayToString(IDs) + ")");

                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerBoolean("Nome");
                    linha["RG"] = bd.LerBoolean("RG");
                    linha["CPF"] = bd.LerBoolean("CPF");
                    linha["Telefone"] = bd.LerBoolean("Telefone");
                    linha["TelefoneComercial"] = bd.LerBoolean("TelefoneComercial");
                    linha["Celular"] = bd.LerBoolean("Celular");
                    linha["DataNascimento"] = bd.LerBoolean("DataNascimento");
                    linha["Email"] = bd.LerBoolean("Email");
                    linha["CEPCliente"] = bd.LerBoolean("CEPCliente");
                    linha["EnderecoCliente"] = bd.LerBoolean("EnderecoCliente");
                    linha["NumeroCliente"] = bd.LerBoolean("NumeroCliente");
                    linha["ComplementoCliente"] = bd.LerBoolean("ComplementoCliente");
                    linha["BairroCliente"] = bd.LerBoolean("BairroCliente");
                    linha["EstadoCliente"] = bd.LerBoolean("EstadoCliente");
                    linha["CidadeCliente"] = bd.LerBoolean("CidadeCliente");
                    linha["NomeEntrega"] = bd.LerBoolean("NomeEntrega");
                    linha["CPFEntrega"] = bd.LerBoolean("CPFEntrega");
                    linha["RGEntrega"] = bd.LerBoolean("RGEntrega");
                    linha["CEPEntrega"] = bd.LerBoolean("CEPEntrega");
                    linha["EnderecoEntrega"] = bd.LerBoolean("EnderecoEntrega");
                    linha["NumeroEntrega"] = bd.LerBoolean("NumeroEntrega");
                    linha["ComplementoEntrega"] = bd.LerBoolean("ComplementoEntrega");
                    linha["BairroEntrega"] = bd.LerBoolean("BairroEntrega");
                    linha["CidadeEntrega"] = bd.LerBoolean("CidadeEntrega");
                    linha["EstadoEntrega"] = bd.LerBoolean("EstadoEntrega");
                    linha["NomeResponsavel"] = bd.LerBoolean("NomeResponsavel");
                    linha["CPFResponsavel"] = bd.LerBoolean("CPFResponsavel");
                    dtt.Rows.Add(linha);
                }

                return dtt;
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

        public DataTable getInformacoesAllFalse()
        {
            DataTable dtt = this.EstruturaDataTable();
            DataRow dtr;
            dtr = dtt.NewRow();
            dtr["Descricao"] = "Nome";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "RG";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "CPF";
            dtt.Rows.Add(dtr);


            dtr = dtt.NewRow();
            dtr["Descricao"] = "Telefone";
            dtt.Rows.Add(dtr);


            dtr = dtt.NewRow();
            dtr["Descricao"] = "Telefone - Comercial";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Celular";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Data de Nascimento";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Email";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "CEP";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Endereço";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Número";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Complemento";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Bairro";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Cidade";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Estado Cliente";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Nome Entrega";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "CPF Entrega";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "RG Entrega";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "CEP Entrega";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Endereço de Entrega";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Número de Entrega";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Complemento de Entrega";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Bairro de Entrega";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Cidade de Entrega";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Estado de Entrega";
            dtt.Rows.Add(dtr);

            return dtt;
        }

        public DataTable getInformacoesAllFalseDono()
        {
            DataTable dtt = this.EstruturaDataTable();
            DataRow dtr;
            dtr = dtt.NewRow();
            dtr["Descricao"] = "Nome";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "RG";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "CPF";
            dtt.Rows.Add(dtr);


            dtr = dtt.NewRow();
            dtr["Descricao"] = "Telefone";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Data de Nascimento";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Email";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "Nome do Responsável";
            dtt.Rows.Add(dtr);

            dtr = dtt.NewRow();
            dtr["Descricao"] = "CPF do Responsável";
            dtt.Rows.Add(dtr);

            return dtt;
        }

        public DataTable EstruturaDataTable()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("ID", typeof(int));
            dtt.Columns.Add("Descricao", typeof(string));
            dtt.Columns.Add("Adicionar", typeof(bool));

            dtt.Columns["Adicionar"].DefaultValue = false;


            return dtt;
        }

        public static DataTable EstruturaObrigatoriedade()
        {
            DataTable dtt = new DataTable("Obrigatoriedade");
            dtt.Columns.Add("ID", typeof(int));
            dtt.Columns.Add("Descricao", typeof(string));
            dtt.Columns.Add("Adicionar", typeof(bool));
            return dtt;
        }

        public EstruturaObrigatoriedade getInformacoesUC(int id)
        {
            try
            {
                string sql = "SELECT * FROM tObrigatoriedade WHERE ID = " + id;
                bd.Consulta(sql);
                EstruturaObrigatoriedade retorno = new EstruturaObrigatoriedade();
                if (bd.Consulta().Read())
                {
                    retorno.Nome = bd.LerBoolean("Nome");
                    retorno.RG = bd.LerBoolean("RG");
                    retorno.CPF = bd.LerBoolean("CPF");
                    retorno.Telefone = bd.LerBoolean("Telefone");
                    retorno.TelefoneComercial = bd.LerBoolean("TelefoneComercial");
                    retorno.Celular = bd.LerBoolean("Celular");
                    retorno.DataNascimento = bd.LerBoolean("DataNascimento");
                    retorno.Email = bd.LerBoolean("Email");
                    retorno.CEPCliente = bd.LerBoolean("CEPCliente");
                    retorno.EnderecoCliente = bd.LerBoolean("EnderecoCliente");
                    retorno.NumeroCliente = bd.LerBoolean("NumeroCliente");
                    retorno.ComplementoCliente = bd.LerBoolean("ComplementoCliente");
                    retorno.BairroCliente = bd.LerBoolean("BairroCliente");
                    retorno.CidadeCliente = bd.LerBoolean("CidadeCliente");
                    retorno.EstadoCliente = bd.LerBoolean("EstadoCliente");
                    retorno.NomeEntrega = bd.LerBoolean("NomeEntrega");
                    retorno.CPFEntrega = bd.LerBoolean("CPFEntrega");
                    retorno.RGEntrega = bd.LerBoolean("RGEntrega");
                    retorno.CEPEntrega = bd.LerBoolean("CEPEntrega");
                    retorno.EnderecoEntrega = bd.LerBoolean("EnderecoEntrega");
                    retorno.NumeroEntrega = bd.LerBoolean("NumeroEntrega");
                    retorno.ComplementoEntrega = bd.LerBoolean("ComplementoEntrega");
                    retorno.BairroEntrega = bd.LerBoolean("BairroEntrega");
                    retorno.CidadeEntrega = bd.LerBoolean("CidadeEntrega");
                    retorno.EstadoEntrega = bd.LerBoolean("EstadoEntrega");
                    retorno.CPFResponsavel = bd.LerBoolean("CPFResponsavel");
                }
                else
                {
                    retorno.Nome = false;
                    retorno.RG = false;
                    retorno.CPF = false;
                    retorno.Telefone = false;
                    retorno.TelefoneComercial = false;
                    retorno.Celular = false;
                    retorno.DataNascimento = false;
                    retorno.Email = false;
                    retorno.CEPCliente = false;
                    retorno.EnderecoCliente = false;
                    retorno.NumeroCliente = false;
                    retorno.ComplementoCliente = false;
                    retorno.BairroCliente = false;
                    retorno.CidadeCliente = false;
                    retorno.EstadoCliente = false;
                    retorno.NomeEntrega = false;
                    retorno.CPFEntrega = false;
                    retorno.RGEntrega = false;
                    retorno.CEPEntrega = false;
                    retorno.EnderecoEntrega = false;
                    retorno.NumeroEntrega = false;
                    retorno.ComplementoEntrega = false;
                    retorno.BairroEntrega = false;
                    retorno.CidadeEntrega = false;
                    retorno.EstadoEntrega = false;
                    retorno.CPFResponsavel = false;
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

        /// <summary>
        /// Preenche todos os atributos de Obrigatoriedade
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerNoLOCK(int id)
        {

            try
            {

                string sql = "SELECT * FROM tObrigatoriedade (NOLOCK) WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.RG.ValorBD = bd.LerString("RG");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.TelefoneComercial.ValorBD = bd.LerString("TelefoneComercial");
                    this.Celular.ValorBD = bd.LerString("Celular");
                    this.DataNascimento.ValorBD = bd.LerString("DataNascimento");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.CEPEntrega.ValorBD = bd.LerString("CEPEntrega");
                    this.EnderecoEntrega.ValorBD = bd.LerString("EnderecoEntrega");
                    this.NumeroEntrega.ValorBD = bd.LerString("NumeroEntrega");
                    this.ComplementoEntrega.ValorBD = bd.LerString("ComplementoEntrega");
                    this.BairroEntrega.ValorBD = bd.LerString("BairroEntrega");
                    this.CidadeEntrega.ValorBD = bd.LerString("CidadeEntrega");
                    this.EstadoEntrega.ValorBD = bd.LerString("EstadoEntrega");
                    this.CEPCliente.ValorBD = bd.LerString("CEPCliente");
                    this.EnderecoCliente.ValorBD = bd.LerString("EnderecoCliente");
                    this.NumeroCliente.ValorBD = bd.LerString("NumeroCliente");
                    this.ComplementoCliente.ValorBD = bd.LerString("ComplementoCliente");
                    this.BairroCliente.ValorBD = bd.LerString("BairroCliente");
                    this.CidadeCliente.ValorBD = bd.LerString("CidadeCliente");
                    this.EstadoCliente.ValorBD = bd.LerString("EstadoCliente");
                    this.NomeEntrega.ValorBD = bd.LerString("NomeEntrega");
                    this.CPFEntrega.ValorBD = bd.LerString("CPFEntrega");
                    this.RGEntrega.ValorBD = bd.LerString("RGEntrega");
                    this.CPFResponsavel.ValorBD = bd.LerString("CPFResponsavel");
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

        }

        public EstruturaObrigatoriedade getObrigatoriedadePorCotaItemID(int cotaItemID, int cotaItemID_APS)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT tObrigatoriedade.* FROM tObrigatoriedade (NOLOCK) ");
                stbSQL.Append("INNER JOIN tCotaItem (NOLOCK) ON tObrigatoriedade.ID = tCotaItem.ObrigatoriedadeID ");
                stbSQL.Append("WHERE tCotaItem.ID = " + (cotaItemID_APS > 0 ? cotaItemID_APS : cotaItemID));
                bd.Consulta(stbSQL.ToString());
                EstruturaObrigatoriedade retorno = new EstruturaObrigatoriedade();
                if (bd.Consulta().Read())
                {
                    retorno.Nome = bd.LerBoolean("Nome");
                    retorno.RG = bd.LerBoolean("RG");
                    retorno.CPF = bd.LerBoolean("CPF");
                    retorno.Telefone = bd.LerBoolean("Telefone");
                    retorno.TelefoneComercial = bd.LerBoolean("TelefoneComercial");
                    retorno.Celular = bd.LerBoolean("Celular");
                    retorno.DataNascimento = bd.LerBoolean("DataNascimento");
                    retorno.Email = bd.LerBoolean("Email");
                    retorno.CEPCliente = bd.LerBoolean("CEPCliente");
                    retorno.EnderecoCliente = bd.LerBoolean("EnderecoCliente");
                    retorno.NumeroCliente = bd.LerBoolean("NumeroCliente");
                    retorno.ComplementoCliente = bd.LerBoolean("ComplementoCliente");
                    retorno.BairroCliente = bd.LerBoolean("BairroCliente");
                    retorno.CidadeCliente = bd.LerBoolean("CidadeCliente");
                    retorno.EstadoCliente = bd.LerBoolean("EstadoCliente");
                    retorno.NomeEntrega = bd.LerBoolean("NomeEntrega");
                    retorno.CPFEntrega = bd.LerBoolean("CPFEntrega");
                    retorno.RGEntrega = bd.LerBoolean("RGEntrega");
                    retorno.CEPEntrega = bd.LerBoolean("CEPEntrega");
                    retorno.EnderecoEntrega = bd.LerBoolean("EnderecoEntrega");
                    retorno.NumeroEntrega = bd.LerBoolean("NumeroEntrega");
                    retorno.ComplementoEntrega = bd.LerBoolean("ComplementoEntrega");
                    retorno.BairroEntrega = bd.LerBoolean("BairroEntrega");
                    retorno.CidadeEntrega = bd.LerBoolean("CidadeEntrega");
                    retorno.EstadoEntrega = bd.LerBoolean("EstadoEntrega");
                    retorno.CPFResponsavel = bd.LerBoolean("CPFResponsavel");
                }
                else
                {
                    retorno.Nome = true;
                    retorno.RG = true;
                    retorno.CPF = true;
                    retorno.Telefone = false;
                    retorno.TelefoneComercial = false;
                    retorno.Celular = false;
                    retorno.DataNascimento = false;
                    retorno.Email = true;
                    retorno.CEPCliente = false;
                    retorno.EnderecoCliente = false;
                    retorno.NumeroCliente = false;
                    retorno.ComplementoCliente = false;
                    retorno.BairroCliente = false;
                    retorno.CidadeCliente = false;
                    retorno.EstadoCliente = false;
                    retorno.NomeEntrega = false;
                    retorno.CPFEntrega = false;
                    retorno.RGEntrega = false;
                    retorno.CEPEntrega = false;
                    retorno.EnderecoEntrega = false;
                    retorno.NumeroEntrega = false;
                    retorno.ComplementoEntrega = false;
                    retorno.BairroEntrega = false;
                    retorno.CidadeEntrega = false;
                    retorno.EstadoEntrega = false;
                    retorno.CPFResponsavel = false;
                }
                return retorno;
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

        public static EstruturaObrigatoriedade getObrigatoriedadeAllFalse()
        {

            EstruturaObrigatoriedade retorno = new EstruturaObrigatoriedade();
            retorno.ID = 0;
            retorno.Nome = false;
            retorno.RG = false;
            retorno.CPF = false;
            retorno.Telefone = false;
            retorno.TelefoneComercial = false;
            retorno.Celular = false;
            retorno.DataNascimento = false;
            retorno.Email = false;
            retorno.CEPCliente = false;
            retorno.EnderecoCliente = false;
            retorno.NumeroCliente = false;
            retorno.ComplementoCliente = false;
            retorno.BairroCliente = false;
            retorno.CidadeCliente = false;
            retorno.EstadoCliente = false;
            retorno.NomeEntrega = false;
            retorno.CPFEntrega = false;
            retorno.RGEntrega = false;
            retorno.CEPEntrega = false;
            retorno.EnderecoEntrega = false;
            retorno.NumeroEntrega = false;
            retorno.ComplementoEntrega = false;
            retorno.BairroEntrega = false;
            retorno.CidadeEntrega = false;
            retorno.EstadoEntrega = false;

            return retorno;
        }

        public static EstruturaObrigatoriedade getObrigatoriedadeAllTrue()
        {

            EstruturaObrigatoriedade retorno = new EstruturaObrigatoriedade();
            retorno.ID = 0;
            retorno.Nome = true;
            retorno.RG = true;
            retorno.CPF = true;
            retorno.Telefone = true;
            retorno.TelefoneComercial = true;
            retorno.Celular = true;
            retorno.DataNascimento = true;
            retorno.Email = true;
            retorno.CEPCliente = true;
            retorno.EnderecoCliente = true;
            retorno.NumeroCliente = true;
            retorno.ComplementoCliente = true;
            retorno.BairroCliente = true;
            retorno.CidadeCliente = true;
            retorno.EstadoCliente = true;

            return retorno;
        }


        //Retorna a estrutura preenchida com base nos enumeradores de Evento / Canal - "U" , "F" etc..
        public static EstruturaObrigatoriedade getCamposObrigatoriosPorTipo(string tipoCadastro)
        {

            EstruturaObrigatoriedade retorno = new EstruturaObrigatoriedade();
            retorno.ID = 0;
            retorno.Nome = false;
            retorno.RG = false;
            retorno.CPF = false;
            retorno.Telefone = false;
            retorno.TelefoneComercial = false;
            retorno.Celular = false;
            retorno.DataNascimento = false;
            retorno.Email = false;
            retorno.CEPCliente = false;
            retorno.EnderecoCliente = false;
            retorno.NumeroCliente = false;
            retorno.ComplementoCliente = false;
            retorno.BairroCliente = false;
            retorno.CidadeCliente = false;
            retorno.EstadoCliente = false;
            retorno.NomeEntrega = false;
            retorno.CPFEntrega = false;
            retorno.RGEntrega = false;
            retorno.CEPEntrega = false;
            retorno.EnderecoEntrega = false;
            retorno.NumeroEntrega = false;
            retorno.ComplementoEntrega = false;
            retorno.BairroEntrega = false;
            retorno.CidadeEntrega = false;
            retorno.EstadoEntrega = false;
            switch (tipoCadastro)
            {
                case "U":
                    {
                        retorno.Nome = true;
                        retorno.RG = true;
                        retorno.Email = true;
                        break;
                    }
                case "E":
                    {
                        retorno.NomeEntrega = true;
                        retorno.CPFEntrega = true;
                        retorno.RGEntrega = true;
                        retorno.CEPEntrega = true;
                        retorno.EnderecoEntrega = true;
                        retorno.NumeroEntrega = true;
                        retorno.BairroEntrega = true;
                        retorno.CidadeEntrega = true;
                        retorno.EstadoEntrega = true;
                        break;
                    }
                case "B":
                    {
                        retorno.Nome = true;
                        retorno.RG = true;
                        break;
                    }
            }

            return retorno;
        }

        public EstruturaObrigatoriedadeSite Buscar(int ObrigatoriedadeID)
        {
            try
            {
                this.Ler(ObrigatoriedadeID);
                return new EstruturaObrigatoriedadeSite()
                {
                    Nome = this.Nome.Valor,
                    RG = this.RG.Valor,
                    CPF = this.CPF.Valor,
                    Email = this.Email.Valor,
                    Telefone = this.Telefone.Valor,
                    DataNascimento = this.DataNascimento.Valor,
                    NomeResponsavel = this.NomeResponsavel.Valor,
                    CPFResponsavel = this.CPFResponsavel.Valor
                };
            }
            finally
            {
                bd.Fechar();
            }
        }


        /// <summary>
        /// Inserir novo(a) Obrigatoriedade
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {
            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cObrigatoriedade");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tObrigatoriedade(ID, Nome, RG, CPF, Telefone, TelefoneComercial, Celular, DataNascimento, Email, CEPEntrega, EnderecoEntrega, NumeroEntrega, ComplementoEntrega, BairroEntrega, CidadeEntrega, EstadoEntrega, CEPCliente, EnderecoCliente, NumeroCliente, ComplementoCliente, BairroCliente, CidadeCliente, EstadoCliente, NomeEntrega, CPFEntrega, RGEntrega, CPFResponsavel, NomeResponsavel) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005','@006','@007','@008','@009','@010','@011','@012','@013','@014','@015','@016','@017','@018','@019','@020','@021','@022','@023','@024','@025','@026','@027')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.RG.ValorBD);
                sql.Replace("@003", this.CPF.ValorBD);
                sql.Replace("@004", this.Telefone.ValorBD);
                sql.Replace("@005", this.TelefoneComercial.ValorBD);
                sql.Replace("@006", this.Celular.ValorBD);
                sql.Replace("@007", this.DataNascimento.ValorBD);
                sql.Replace("@008", this.Email.ValorBD);
                sql.Replace("@009", this.CEPEntrega.ValorBD);
                sql.Replace("@010", this.EnderecoEntrega.ValorBD);
                sql.Replace("@011", this.NumeroEntrega.ValorBD);
                sql.Replace("@012", this.ComplementoEntrega.ValorBD);
                sql.Replace("@013", this.BairroEntrega.ValorBD);
                sql.Replace("@014", this.CidadeEntrega.ValorBD);
                sql.Replace("@015", this.EstadoEntrega.ValorBD);
                sql.Replace("@016", this.CEPCliente.ValorBD);
                sql.Replace("@017", this.EnderecoCliente.ValorBD);
                sql.Replace("@018", this.NumeroCliente.ValorBD);
                sql.Replace("@019", this.ComplementoCliente.ValorBD);
                sql.Replace("@020", this.BairroCliente.ValorBD);
                sql.Replace("@021", this.CidadeCliente.ValorBD);
                sql.Replace("@022", this.EstadoCliente.ValorBD);
                sql.Replace("@023", this.NomeEntrega.ValorBD);
                sql.Replace("@024", this.CPFEntrega.ValorBD);
                sql.Replace("@025", this.RGEntrega.ValorBD);
                sql.Replace("@026", this.CPFResponsavel.ValorBD);
                sql.Replace("@027", this.NomeResponsavel.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I" , bd);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        protected void InserirControle(string acao , BD bd)
        {
            try
            {
                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cObrigatoriedade (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }

    public class ObrigatoriedadeLista : ObrigatoriedadeLista_B
    {

        public ObrigatoriedadeLista() { }

        public ObrigatoriedadeLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
