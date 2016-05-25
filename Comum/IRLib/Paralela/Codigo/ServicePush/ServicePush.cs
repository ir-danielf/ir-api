/**************************************************
* Arquivo: ServicePush.cs
* Gerado: 30/01/2014
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;
using CTLib;
using System.Linq;
using System.Collections.Generic;

namespace IRLib.Paralela
{
    public class ServicePush : ServicePush_B
    {
        public ServicePush() { }

        public ServicePush(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public bool VerificarSeExisteAtivo()
        {
            try
            {
                string sql = string.Format(@"SELECT tsp.ID FROM tServicePush tsp WHERE tsp.ClienteID = {0} AND tsp.Plataforma = '{1}' AND tsp.Token = '{2}' AND tsp.Ativo = 'T'", this.ClienteID.Valor, this.Plataforma.Valor, this.Token.Valor);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");

                    return true;
                }
                else
                    return false;
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
        public bool VerificarSeExiste()
        {
            try
            {
                string sql = string.Format(@"SELECT tsp.ID FROM tServicePush tsp WHERE tsp.ClienteID = {0} AND tsp.Plataforma = '{1}' AND tsp.Token = '{2}'", this.ClienteID.Valor, this.Plataforma.Valor, this.Token.Valor);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");

                    return true;
                }
                else
                    return false;
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

    public class ServicePushLista : ServicePushLista_B
    {
        public ServicePushLista() { }

        public ServicePushLista(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }
}
