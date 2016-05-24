/**************************************************
* Arquivo: TelaManual.cs
* Gerado: 25/11/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRLib.Paralela
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class TelaManualParalela : TelaManual_B
    {
        public TelaManualParalela() { }

        public EstruturaTelaManual verificaTelaAtiva(string nome)
        {
            try
            {
                StringBuilder filter = new StringBuilder();

                if (!String.IsNullOrEmpty(nome))
                {
                    if (filter.Length > 0)
                        filter.Append("AND");

                    filter.AppendFormat(" t.Nome = '{0}' ", nome);
                }

                string sql = string.Empty;
                EstruturaTelaManual tela = new EstruturaTelaManual();

                sql = string.Format(@"SELECT t.Ativa,ISNULL(m.Arquivo,'') as Arquivo,t.Nome
                                    FROM tTela t (NOLOCK)
                                    LEFT JOIN tTelaManual m (NOLOCK)
                                    ON ( t.id = m.idTela )
                                    {0}", filter.Length > 0 ? "WHERE " + filter.ToString() : string.Empty);
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    tela.Ativa = bd.LerBoolean("Ativa");
                    tela.Arquivo = bd.LerString("Arquivo");
                    tela.Nome = bd.LerString("Nome");
                }

                return tela;
            }
            finally
            {
                bd.Fechar();
            }


        }

        public List<EstruturaTela> CarregarTelaFiltrada(string nome)
        {
            try
            {
                StringBuilder filter = new StringBuilder();


                string sql = string.Empty;
                List<EstruturaTela> lista = new List<EstruturaTela>();

                sql = string.Format(@"SELECT t.ID, t.Nome, t.Titulo, m.Id
                                        FROM tTela t (NOLOCK)  
                                        Left JOIN tTelaManual m (NOLOCK) 
                                        on (t.id = m.idTela) 
                                        where Ativa='t' and m.Id is null order by t.Titulo");


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaTela()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Titulo = bd.LerString("Titulo"),
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string SelecionarTela(string id)
        {
            try
            {

                StringBuilder filter = new StringBuilder();


                if (!String.IsNullOrEmpty(id))
                {
                    if (filter.Length > 0)
                        filter.Append("AND");

                    filter.AppendFormat(" t.ID = {0} ", id);
                }

                string sql = string.Empty;
                EstruturaTela lista = new EstruturaTela();

                sql = string.Format(@"SELECT t.ID, t.Nome, t.Titulo
                                    FROM tTela t (NOLOCK)
                                    {0}", filter.Length > 0 ? "WHERE " + filter.ToString() : string.Empty);


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    lista.ID = bd.LerInt("ID");
                    lista.Nome = bd.LerString("Nome");
                    lista.Titulo = bd.LerString("Titulo");

                }

                return lista.Nome;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaTelaManual> CarregarListaFiltrada(string nome)
        {
            try
            {
                StringBuilder filter = new StringBuilder();


                if (!String.IsNullOrEmpty(nome))
                    filter.AppendFormat(" AND t.Titulo like '%{0}%' ", nome);

                string sql = string.Empty;
                List<EstruturaTelaManual> lista = new List<EstruturaTelaManual>();

                sql = string.Format(@"SELECT t.ID as IDTela,m.ID, t.Nome, t.Titulo, m.Manual, m.Arquivo,t.Ativa, t.Nome AS Form
                                    FROM tTela t (NOLOCK)  
                                    INNER JOIN tTelaManual m (NOLOCK) 
                                    on (t.id = m.idTela) 
                                    {0} order by t.Titulo desc", filter.Length > 0 ? "WHERE t.Ativa = 'T' " + filter.ToString() : string.Empty);


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaTelaManual()
                    {
                        IDTela = bd.LerString("IDTela"),
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Titulo = bd.LerString("Titulo"),
                        Manual = bd.LerString("Manual"),
                        Arquivo = bd.LerString("Arquivo"),
                        Ativa = bd.LerBoolean("Ativa"), 
                        Form = bd.LerString("Form")
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaTelaManual LerIdTela(int id)
        {
            EstruturaTelaManual eTM = new EstruturaTelaManual();

            try
            {
                string sql = @"SELECT t.id as IDTela,ISNULL(m.ID,0) as ID, t.Nome, t.Titulo, 
                                ISNULL(m.Manual,'') as Manual, ISNULL(m.Arquivo,'') as Arquivo,t.Ativa
                                FROM tTela t (NOLOCK)  
                                LEFT JOIN tTelaManual m (NOLOCK) 
                                on (t.id = m.idTela) WHERE t.id = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    eTM.ID = bd.LerInt("ID");
                    eTM.IDTela = bd.LerString("IDTela");
                    eTM.Manual = bd.LerString("Manual");
                    eTM.Arquivo = bd.LerString("Arquivo");
                    eTM.Titulo = bd.LerString("Titulo");
                    eTM.Nome = bd.LerString("Nome");
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();

                return eTM;

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

        public void Atualizar(EstruturaTelaManual estruturaManual)
        {
            this.AtribuirEstrutura(estruturaManual);
            this.Atualizar();
        }

        public void AtribuirEstrutura(EstruturaTelaManual estruturaManual)
        {
            this.Control.ID = estruturaManual.ID;
            this.IDTela.Valor = estruturaManual.IDTela;
            this.Manual.Valor = estruturaManual.Manual;
            this.Arquivo.Valor = estruturaManual.Arquivo;


        }

        public void Inserir(EstruturaTelaManual estruturaManual)
        {
            this.AtribuirEstrutura(estruturaManual);
            this.Inserir();
        }
    }

    public class TelaManualLista : TelaManualLista_B
    {

        public TelaManualLista() { }



    }

}
