/**************************************************
* Arquivo: EntregaArea.cs
* Gerado: 06/01/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela
{

    public class EntregaArea : EntregaArea_B
    {

        public EntregaArea() { }

        public EntregaArea(int usuarioIDLogado) : base(usuarioIDLogado) { }


        public List<EstruturaEntregaArea> CarregarDados()
        {
            try
            {

                List<EstruturaEntregaArea> lista = new List<EstruturaEntregaArea>();
                bd.Consulta("SELECT ID, Nome, CepInicial, CepFinal FROM tEntregaArea (NOLOCK) ORDER BY Nome");
                while (bd.Consulta().Read())
                    lista.Add(new EstruturaEntregaArea()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        CEPInicial = bd.LerString("CepInicial"),
                        CEPFinal = bd.LerString("CepFinal"),

                    });


                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaEntregaArea> ListarNovo()
        {
            try
            {

                List<EstruturaEntregaArea> lista = new List<EstruturaEntregaArea>();
                EstruturaEntregaArea area = new EstruturaEntregaArea();
                List<EstruturaEntregaAreaCep> ListaCep = new List<EstruturaEntregaAreaCep>();
                EstruturaEntregaAreaCep cep = new EstruturaEntregaAreaCep();
                List<int> Regionais = new List<int>();

                string sql = @"SELECT 
                                a.ID as AreaID, 
                                a.Nome, c.ID AS CEPID, c.CEPInicial, c.CEPFinal, ra.RegionalID
                                FROM tEntregaArea a (NOLOCK)
                                INNER JOIN tEntregaAreaCep c (NOLOCK) on c.EntregaAreaId = a.id
                                INNER JOIN tRegionalArea ra (NOLOCK) ON ra.AreaID = a.ID
                                ORDER BY a.Nome";

                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    if (lista.Where(c => c.ID == bd.LerInt("AreaID")).Count() == 0)
                    {
                        ListaCep = new List<EstruturaEntregaAreaCep>();
                        Regionais = new List<int>();

                        area = new EstruturaEntregaArea()
                        {
                            ID = bd.LerInt("AreaID"),
                            Nome = bd.LerString("Nome"),
                            ListaCEP = ListaCep,
                            Regionais = Regionais,
                        };

                        lista.Add(area);
                    }

                    if (ListaCep.Where(c => c.EntregaAreaCepID == bd.LerInt("CEPID")).Count() == 0)
                    {
                        cep = new EstruturaEntregaAreaCep()
                        {
                            EntregaAreaCepID = bd.LerInt("CEPID"),
                            CepInicio = bd.LerString("CEPInicial"),
                            CepFim = bd.LerString("CEPFinal"),
                            EntregaAreaID = bd.LerInt("AreaID"),
                        };

                        ListaCep.Add(cep);
                    }

                    Regionais.Add(bd.LerInt("RegionalID"));
                }
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaEntregaArea> Listar()
        {

            try
            {

                List<EstruturaEntregaArea> lista = new List<EstruturaEntregaArea>();
                string sql = @"SELECT 
                                a.ID as EntregaAreaID, 
                                a.Nome, 
                                min(c.id) as EntregaAreaCepID,
                                min(c.CepInicial) as CepInicial,
                                min(c.CepFinal) as CepFinal, 
                                'Visualizar Cep' as Ceps
                                FROM tEntregaArea a (nolock)
                                right join tEntregaAreaCep c (nolock)
                                on (c.EntregaAreaId = a.id)
                                group by a.ID,a.Nome";

                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaEntregaArea
                    {
                        ID = bd.LerInt("EntregaAreaID"),
                        Nome = bd.LerString("Nome"),
                        EntregaAreaCepID = bd.LerInt("EntregaAreaCepID"),
                        CEPInicial = bd.LerString("CepInicial"),
                        CEPFinal = bd.LerString("CepFinal")
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

        public string LerNome(int ID)
        {

            try
            {

                string nome = "-";
                string sql = @"SELECT  
                                a.Nome
                                FROM tEntregaArea a (nolock)
                                where id = " + ID;

                bd.Consulta(sql);


                if (bd.Consulta().Read())
                {
                    nome = bd.LerString("Nome");
                }

                return nome;

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

    public class EntregaAreaLista : EntregaAreaLista_B
    {

        public EntregaAreaLista() { }

        public EntregaAreaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
