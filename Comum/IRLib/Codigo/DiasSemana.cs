/**************************************************
* Arquivo: DiasSemana.cs
* Gerado: 31/03/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class DiasSemana : DiasSemana_B
    {

        public DiasSemana() { }

        public DiasSemana(int usuarioIDLogado) : base(usuarioIDLogado) { }

        internal EstruturaEntregaControleDias Listar(int controleID)
        {
            try
            {
                EstruturaEntregaControleDias diasRetorno = new EstruturaEntregaControleDias();
                string sql = "SELECT DiaDaSemana FROM tDiasSemana WHERE ControleEntregaID = " + controleID;
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    int dia = bd.LerInt("DiaDaSemana");
                    diasRetorno.ListaDiasDaSemana.Add(dia);
                    
                }
                diasRetorno.DiasDaSemana = StringDia(diasRetorno.ListaDiasDaSemana);

                bd.Fechar();

                return diasRetorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string StringDia(List<int> list)
        {
            string retorno = "-";
            string dia;
            foreach (int item in list)
            {
                
                switch (item)
                {
                    case 1:
                        dia = "Segunda-Feira";
                        break;
                    case 2:
                        dia = "Terça-Feira";
                        break;
                    case 3:
                        dia = "Quarta-Feira";
                        break;
                    case 4:
                        dia = "Quinta-Feira";
                        break;
                    case 5:
                        dia = "Sexta-Feira";
                        break;
                    case 6:
                        dia = "Sábado";
                        break;
                    case 7:
                        dia = "Domingo";
                        break;
                    default:
                        dia = "erro";
                        break;
                }


                if (retorno.Length > 1)
                {

                    retorno += ", " + dia;
                }
                else
                {
                    retorno = dia;
                }

            }
            return retorno;
        }


    }

    public class DiasSemanaLista : DiasSemanaLista_B
    {

        public DiasSemanaLista() { }

        public DiasSemanaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
