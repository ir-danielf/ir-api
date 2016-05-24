/**************************************************
* Arquivo: BlackList.cs
* Gerado: 29/01/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRLib
{

    public class BlackList : BlackList_B
    {

        public BlackList() { }

        public BlackList(int usuarioIDLogado) : base() { }

        public enum enumBuscaTipo
        {
            CPF = 1,
            Nome = 2,
            Email = 3,
            BlackList = 4,
            BomCliente = 5,
            CNPJ = 6
        }

        public int Registros { get; set; }
        public int BuscaTipo { get; set; }
        public string BuscaTexto { get; set; }
        public bool Reiniciar { get; set; }
        public int IndexAtual { get; set; }
        public int QuantidadePorPagina { get; set; }
        public bool SemPaginacao { get; set; }
        public string txtIP { get; set; }
        public string Ordem { get; set; }
        public int blackList { get; set; }
        public int bomCliente { get; set; }

        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tBlackList");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tBlackList(ID, Tipo, Identificador, TimeStamp) ");
                sql.Append("VALUES ('@ID','@001','@002','@003')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Tipo.ValorBD);
                sql.Replace("@002", this.Identificador.ValorBD);
                sql.Replace("@003", this.TimeStamp.ValorBD);

                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

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

        public int Excluir(BD bd)
        {
            int retorno;

            string sql = "Delete tBlackList where ID = " + this.Control.ID + " AND Tipo = " + this.Tipo;

            retorno = bd.Executar(sql.ToString());

            return retorno;
        }

        public int BuscaIP()
        {
            int retorno = 0;

            string sql = "Select ID from  tBlackList where Identificador = '" + this.Identificador + "'";

            retorno = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return retorno;
        }

        public List<EstruturaBlackList2> ListaIP()
        {
            try
            {
                List<EstruturaBlackList2> lista = new List<EstruturaBlackList2>();
                EstruturaBlackList2 item;

                StringBuilder sql = new StringBuilder();

                if (string.IsNullOrEmpty(this.Ordem))
                    this.Ordem = "DESC";

                sql.Append(" WITH tbGeral AS( 	 ");

                sql.Append(" SELECT  ");
                sql.Append("        ID, ");
                sql.Append("        TimeStamp ,");
                sql.Append("        Identificador ,");
                sql.Append("        Tipo ");
                sql.Append("   FROM tBlackList ");

                if (!string.IsNullOrEmpty(this.txtIP))
                    sql.Append("   where Identificador like '%" + this.txtIP + "%' ");

                sql.Append(" ) ");
                sql.Append(" 	,tbCount AS( ");
                sql.Append(" 				 	SELECT Count(ID) as Registros FROM tbGeral ");
                sql.Append(" 							) ");
                sql.Append(" 	, tbOrdenada AS ");
                sql.Append(" 				( 	 ");
                sql.Append(" 					SELECT ID,	TimeStamp, Identificador, Tipo  ");
                sql.Append(" 					 ,ROW_NUMBER() OVER (ORDER BY TimeStamp " + this.Ordem + ") AS 'RowNumber' FROM tbGeral ");
                sql.Append(" 				)  ");
                sql.Append("  ");
                sql.Append(" SELECT ID,	TimeStamp, Identificador, Tipo , ");
                sql.Append(" 		RowNumber,  ");
                sql.Append(" 		Registros  ");
                sql.Append(" FROM tbOrdenada, tbCount  ");

                if (!SemPaginacao)
                    sql.Append(" WHERE RowNumber between " + this.IndexAtual + " and " + (this.IndexAtual + this.QuantidadePorPagina - 1) + "  ");

                sql.Append(" ORDER BY TimeStamp " + this.Ordem + "");

                while (bd.Consulta(sql.ToString()).Read())
                {
                    item = new EstruturaBlackList2();
                    item.ID = bd.LerInt("ID");
                    item.Tipo = bd.LerString("Tipo");
                    item.TimeStamp = bd.LerDateTime("TimeStamp").ToString();

                    string identificador = bd.LerString("Identificador");

                    if (!string.IsNullOrEmpty(this.txtIP))
                        identificador.Replace(this.txtIP, "<span style='background-color:yellow'>" + this.txtIP + "</span>");

                    item.Identificador = identificador;

                    if (this.QuantidadePorPagina != 0)
                        this.Registros = bd.LerInt("Registros");

                    lista.Add(item);
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

        public List<EstruturaBandeira> ListaBandeiras()
        {
            try
            {
                List<EstruturaBandeira> lista = new List<EstruturaBandeira>();
                EstruturaBandeira item;

                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT  ");
                sql.Append("        ID, ");
                sql.Append("        TEFID ,");
                sql.Append("        Nome ");
                sql.Append("   FROM tBandeira ");
                sql.Append("  WHERE ID in (1,2,3,4,8,12,13)  ");

                while (bd.Consulta(sql.ToString()).Read())
                {
                    item = new EstruturaBandeira();
                    item.ID = bd.LerInt("ID");
                    item.TEFID = bd.LerInt("TEFID");
                    item.Nome = bd.LerString("Nome");
                    lista.Add(item);
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

        public List<EstruturaBlackListCliente> ListaCliente()
        {
            try
            {
                List<EstruturaBlackListCliente> lista = new List<EstruturaBlackListCliente>();
                EstruturaBlackListCliente item;

                StringBuilder sql = new StringBuilder();
                string campo = string.Empty;

                if (this.bomCliente > 0 || this.blackList > 0 || this.BuscaTipo > 0 || !string.IsNullOrEmpty(this.BuscaTexto))
                {
                    sql.Append(" WITH tbGeral AS( ");
                    if (this.BuscaTipo > 0)
                    {
                        sql.Append("SELECT ID, CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' ");
                        sql.Append("THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                        sql.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NOME, ");
                        sql.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' ");
                        sql.Append("THEN  tCliente.CPF COLLATE Latin1_General_CI_AI ");
                        sql.Append("ELSE tCliente.CNPJ COLLATE Latin1_General_CI_AI   ");
                        sql.Append("END AS CPF , StatusAtual, NivelCliente FROM tCliente WHERE ");
                    }
                    else
                    {
                        sql.Append("SELECT ID, CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' ");
                        sql.Append("THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                        sql.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NOME, ");
                        sql.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' ");
                        sql.Append("THEN  tCliente.CPF COLLATE Latin1_General_CI_AI ");
                        sql.Append("ELSE tCliente.CNPJ COLLATE Latin1_General_CI_AI   ");
                        sql.Append("END AS CPF , StatusAtual, NivelCliente FROM tCliente ");
                    }

                    switch (this.blackList)
                    {
                        case 1:
                            if (this.bomCliente > 0)
                            {
                                sql.Append("StatusAtual = '" + (char)IRLib.Cartao.enumStatusCartao.Bloqueado + "' AND ");
                            }
                            else
                            {
                                sql.Append("StatusAtual = '" + (char)IRLib.Cartao.enumStatusCartao.Bloqueado + "' ");
                            }
                            break;
                        case 2:
                            if (this.bomCliente > 0)
                            {
                                sql.Append("StatusAtual = '" + (char)IRLib.Cartao.enumStatusCartao.Liberado + "' AND ");
                            }
                            else
                            {
                                sql.Append("StatusAtual = '" + (char)IRLib.Cartao.enumStatusCartao.Liberado + "' ");
                            }

                            break;
                    }

                    switch (this.bomCliente)
                    {
                        case 1:
                            sql.Append("NivelCliente  = 1");
                            break;
                        case 2:
                            sql.Append("NivelCliente  = 0 ");
                            break;
                    }

                    if (!string.IsNullOrEmpty(this.BuscaTexto))
                    {
                        switch (this.BuscaTipo)
                        {
                            case (int)enumBuscaTipo.CPF:
                                campo = "CPF";
                                break;
                            case (int)enumBuscaTipo.CNPJ:
                                campo = "CNPJ";
                                break;
                            case (int)enumBuscaTipo.Nome:
                                campo = "Nome";
                                break;
                            case (int)enumBuscaTipo.Email:
                                campo = "Email";
                                break;
                            case 0:
                                campo = "Todos";
                                break;
                            default:
                                campo = "Todos";
                                break;
                        }
                        if (this.BuscaTipo > 0)
                        {
                            if (this.bomCliente > 0 || this.blackList > 0)
                            {
                                sql.Append(" AND @Campo like '%@P_1%'");
                            }
                            else
                            {
                                if (this.BuscaTipo == (int)enumBuscaTipo.CPF)
                                    sql.Append(" @Campo like '%@P_1%' AND (tCliente.CNPJ IS NULL or tCliente.CNPJ = '')");
                                else
                                    sql.Append(" @Campo like '%@P_1%'");
    
                            }
                            sql.Replace("@P_1", this.BuscaTexto);
                            sql.Replace("@Campo", campo);
                        }
                    }
                }
                else
                {
                    sql.Append(" WITH tbGeral AS( ");
                    sql.Append("SELECT ID, CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' ");
                    sql.Append("THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                    sql.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NOME, ");
                    sql.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' ");
                    sql.Append("THEN  tCliente.CPF COLLATE Latin1_General_CI_AI ");
                    sql.Append("ELSE tCliente.CNPJ COLLATE Latin1_General_CI_AI   ");
                    sql.Append("END AS CPF , StatusAtual, NivelCliente FROM tCliente ");
                }

                sql.Append(" ) ");
                sql.Append("  ,tbCount AS( ");
                sql.Append(" SELECT Count(ID) as Registros FROM tbGeral ");
                sql.Append(" ) ");
                sql.Append(" , tbOrdenada AS ");
                sql.Append(" ( ");
                sql.Append(" SELECT	ID, Nome ,CPF , StatusAtual, NivelCliente ");
                sql.Append(" ,ROW_NUMBER() OVER (ORDER BY Nome) AS 'RowNumber' FROM tbGeral ");
                sql.Append(" ) ");
                sql.Append(" ");
                sql.Append(" SELECT ID, Nome ,CPF , StatusAtual , NivelCliente, ");
                sql.Append(" RowNumber,  ");
                sql.Append(" Registros  ");
                sql.Append(" FROM tbOrdenada, tbCount  ");

                if (!SemPaginacao)
                    sql.Append(" WHERE RowNumber between " + this.IndexAtual + " and " + (this.IndexAtual + this.QuantidadePorPagina - 1) + "  ");

                sql.Append(" ORDER BY Nome  ");

                while (bd.Consulta(sql.ToString()).Read())
                {
                    item = new EstruturaBlackListCliente();
                    item.ID = bd.LerInt("ID");
                    item.Nome = bd.LerString("Nome");
                    item.CPF = bd.LerString("CPF");
                    item.Status = bd.LerString("StatusAtual");

                    if (item.Status.Equals("L"))
                    {
                        item.StatusLiberado = true;
                        item.StatusBloqueado = false;
                    }
                    else if (item.Status.Equals("B"))
                    {
                        item.StatusLiberado = false;
                        item.StatusBloqueado = true;
                    }

                    item.NivelCliente = bd.LerInt("NivelCliente");

                    if (item.NivelCliente == 0)
                    {
                        item.BomCliente = false;
                        item.MalCliente = true;
                    }
                    else if (item.NivelCliente == 1)
                    {
                        item.BomCliente = true;
                        item.MalCliente = false;
                    }


                    if (this.QuantidadePorPagina != 0)
                        this.Registros = bd.LerInt("Registros");

                    lista.Add(item);
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

        public static bool BlackListIpIsBlock(string IP)
        {
            BD bd = new BD();

            bool retorno = false;
            string identificador = string.Empty;
            string sql = "SELECT Identificador FROM tBlackList (NOLOCK) WHERE Identificador = '" + IP + "'";

            bd.ConsultaValor(sql);

            while (bd.Consulta().Read())
            {
                identificador = bd.LerString("Identificador");
            }

            if (IP == identificador)
                retorno = true;

            return retorno;
        }
    }

    public class BlackListLista : BlackListLista_B
    {

        public BlackListLista() { }

        public BlackListLista(int usuarioIDLogado) : base() { }
    }

}
