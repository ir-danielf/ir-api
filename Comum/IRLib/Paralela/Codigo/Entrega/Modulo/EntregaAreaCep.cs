/**************************************************
* Arquivo: EntregaAreaCep.cs
* Gerado: 06/01/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class EntregaAreaCep : EntregaAreaCep_B
    {

        public EntregaAreaCep() { }

        public EntregaAreaCep(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Retorna Lista de EstuturaAreaCep
        /// </summary>
        /// <param name="id">Informe o AreaID</param>
        /// <returns></returns>
        public List<EstruturaEntregaAreaCep> BuscarCep(int AreaId)
        {

            try
            {

                List<EstruturaEntregaAreaCep> lista = new List<EstruturaEntregaAreaCep>();
                string sql = "SELECT * FROM tEntregaAreaCep WHERE EntregaAreaID = " + AreaId;
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaEntregaAreaCep
                    {
                        EntregaAreaCepID = bd.LerInt("ID"),
                        EntregaAreaID = bd.LerInt("EntregaAreaID"),
                        CepInicio = bd.LerString("CepInicial"),
                        CepFim = bd.LerString("CepFinal")
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

        public void Inserir(EstruturaEntregaAreaCep estruturaEntregaAreaCep)
        {
            this.AtribuirEstrutura(estruturaEntregaAreaCep);
            this.Inserir();
        }

        public void Atualizar(EstruturaEntregaAreaCep estruturaEntregaAreaCep)
        {
            this.AtribuirEstrutura(estruturaEntregaAreaCep);
            this.Atualizar();
        }

        public void Inserir(EstruturaEntregaAreaCep estruturaEntregaAreaCep, int EntregaAreaID)
        {
            estruturaEntregaAreaCep.EntregaAreaID = EntregaAreaID;
            this.AtribuirEstrutura(estruturaEntregaAreaCep);
            this.Inserir();
        }

        public void AtribuirEstrutura(EstruturaEntregaAreaCep estruturaEntregaAreaCep)
        {

            this.EntregaAreaID.Valor = estruturaEntregaAreaCep.EntregaAreaID;
            this.Control.ID = estruturaEntregaAreaCep.EntregaAreaCepID;
            this.CepInicial.Valor = estruturaEntregaAreaCep.CepInicio.ToString();
            this.CepFinal.Valor = estruturaEntregaAreaCep.CepFim.ToString();

        }



        public bool VerificaExistente(EstruturaEntregaAreaCep estruturaEntregaAreaCepAdd)
        {
            try
            {
                bool retorno = true;
                string sql = @"select top 1 ID from tEntregaAreaCep
                                where  
                                (CepInicial <= " + estruturaEntregaAreaCepAdd.CepInicio + " and CepFinal >= " + estruturaEntregaAreaCepAdd.CepInicio + @")
                                or
                                (CepInicial <= " + estruturaEntregaAreaCepAdd.CepFim + " and CepFinal >= " + estruturaEntregaAreaCepAdd.CepFim + ")";
                
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno = false;
                }
                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    public class EntregaAreaCepLista : EntregaAreaCepLista_B
    {

        public EntregaAreaCepLista() { }

        public EntregaAreaCepLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
