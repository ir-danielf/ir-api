/**************************************************
* Arquivo: DonoIngresso.cs
* Gerado: 05/07/2012
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IRLib
{

    public class DonoIngresso : DonoIngresso_B
    {

        public DonoIngresso() { }

        public EstruturaDonoIngressoSite Buscar(int DonoID)
        {
            this.Ler(DonoID);
            return new EstruturaDonoIngressoSite()
            {
                Nome = this.Nome.Valor,
                RG = this.RG.Valor,
                CPF = this.CPF.Valor,
                Email = this.Email.Valor,
                DataNascimento = string.Compare(this.DataNascimento.Valor.ToString("ddMMyyyy"), "01011753", 0) == 0 ? string.Empty : this.DataNascimento.Valor.ToString("dd/MM/yyyy"),
                Telefone = this.Telefone.Valor,
                NomeResponsavel = this.NomeResponsavel.Valor,
                CPFResponsavel = this.CPFResponsavel.Valor
            };
        }

        public EstruturaDonoIngressoSite BuscarPorIngressoId(int IngressoId)
        {
            string sql = @"SELECT TOP 1 tDonoIngresso.ID, tDonoIngresso.Nome, tDonoIngresso.RG, tDonoIngresso.CPF, tDonoIngresso.Email, tDonoIngresso.Telefone,
                            tDonoIngresso.DataNascimento, tDonoIngresso.CPFResponsavel, tDonoIngresso.NomeResponsavel
                            FROM tIngresso (NOLOCK)
                            LEFT JOIN tingressoCliente (NOLOCK) on tIngresso.ID = tingressoCliente.IngressoID
                            LEFT JOIN tDonoIngresso (NOLOCK) on tingressoCliente.DonoID = tDonoIngresso.ID
                            WHERE tIngresso.ID = @IngressoId";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@IngressoId", Value = IngressoId, DbType = System.Data.DbType.Int32 });

            bd.Consulta(sql, parametros);

            EstruturaDonoIngressoSite _EstruturaDonoIngressoSite = null;
            while (bd.Consulta().Read())
            {
                _EstruturaDonoIngressoSite = new EstruturaDonoIngressoSite();
                _EstruturaDonoIngressoSite.Id = bd.LerInt(("ID"));
                _EstruturaDonoIngressoSite.Nome = bd.LerString("Nome");
                _EstruturaDonoIngressoSite.RG = bd.LerString("RG");
                _EstruturaDonoIngressoSite.CPF = bd.LerString("CPF");
                _EstruturaDonoIngressoSite.Email = bd.LerString("Email");
                _EstruturaDonoIngressoSite.Telefone = bd.LerString("Telefone");
                _EstruturaDonoIngressoSite.DataNascimento = bd.LerString("DataNascimento");
                _EstruturaDonoIngressoSite.CPFResponsavel = bd.LerString("CPFResponsavel");
                _EstruturaDonoIngressoSite.NomeResponsavel = bd.LerString("NomeResponsavel");
                break;
            }
            return _EstruturaDonoIngressoSite;
        }
    }

    public class DonoIngressoLista : DonoIngressoLista_B
    {

        public DonoIngressoLista() { }

    }

}
