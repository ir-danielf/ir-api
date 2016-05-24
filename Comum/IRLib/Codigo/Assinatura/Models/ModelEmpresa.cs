using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTLib;

namespace IRLib.Assinaturas.Models
{
    public class ModelEmpresa
    {
        public ModelEmpresa()
        {
            this.NomeFantasia = "-";
            this.Razao = "-";
            this.CNPJ = "-";
            this.Endereco = "-";
            this.CEP = "-";
            this.Bairro = "-";
            this.TEL1 = "-";
            this.TEL2 = "-";
            this.FAX = "-";
            this.Infos = "-";
            this.Politicas = "-";
            this.FaleConosco = "-";
        }

        public void Carregar(int Id)
        {
            IDTipoAssinatura = Id;
            BD bd = new BD();
            try
            {
                string query = @"SELECT NomeFantasia, RazaoSocial, CNPJ, Endereco, CEP, Bairro, Telefone1, Telefone2, Fax, Info, Politicas, FaleConosco FROM assinaturaInfoSite WHERE IDTipoASsinatura = " + Id;

                var reader = bd.Consulta(query);
                if (reader.Read())
                {
                    this.NomeFantasia = reader["NomeFantasia"].ToString();
                    this.Razao = reader["RazaoSocial"].ToString();
                    this.CNPJ = reader["CNPJ"].ToString();
                    this.Endereco = reader["Endereco"].ToString();
                    this.CEP = reader["CEP"].ToString();
                    this.Bairro = reader["Bairro"].ToString();
                    this.TEL1 = reader["Telefone1"].ToString();
                    this.TEL2 = reader["Telefone2"].ToString();
                    this.FAX = reader["Fax"].ToString();
                    this.Infos = reader["Info"].ToString();
                    this.Politicas = reader["Politicas"].ToString();
                    this.FaleConosco = reader["FaleConosco"].ToString();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                bd.Fechar();
            }

        }

        public int IDTipoAssinatura { get; set; }
        public string Infos { get; set; }
        public string NomeFantasia { get; set; }
        public string Razao { get; set; }
        public string CNPJ { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string TEL1 { get; set; }
        public string TEL2 { get; set; }
        public string FAX { get; set; }
        public string Politicas { get; set; }

        public string FaleConosco { get; set; }
    }
}
