/**************************************************
* Arquivo: CotaItemFormaPagamento.cs
* Gerado: 14/01/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;

namespace IRLib
{

    public class SetorElementoMapa : SetorElementoMapa_B
    {

        public ElementoMapa ElementoMapa { get; set; }

        public int Imagem { get; set; }

        public SetorElementoMapa() { }

        public SetorElementoMapa(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Inserir novo(a) SetorElementoMapa
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO SetorElementoMapa(X, Y, Z, SetorID, ElementoMapaID, Conteudo) ");
                sql.Append("VALUES (@001,@002,@003,@004,@005,@006)");

                sql.Replace("@001", this.X.ValorBD);
                sql.Replace("@002", this.Y.ValorBD);
                sql.Replace("@003", this.Z.ValorBD);
                sql.Replace("@004", this.SetorID.ValorBD);
                sql.Replace("@005", this.ElementoMapaID.ValorBD);
                sql.Replace("@006", this.Conteudo.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = Convert.ToBoolean(x);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SetorElementoMapa> ListarElementos(int SetorID)
        {
            List<SetorElementoMapa> elementos = new List<SetorElementoMapa>();
            try
            {
                string sql = "SELECT sem.* FROM SetorElementoMapa (NOLOCK) sem WHERE sem.SetorID = " + SetorID + " ORDER BY Z";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    SetorElementoMapa sem = new SetorElementoMapa();
                    sem.Ler(bd.LerInt("ID"));
                    elementos.Add(sem);
                }

                foreach(var setorElemento in elementos){
                    var elemento = new ElementoMapa();
                    elemento.Ler(setorElemento.ElementoMapaID.Valor);
                    setorElemento.ElementoMapa = elemento;
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return elementos;

        }

        public void AtualizarImagem(byte[] img)
        {
            var setor = this.SetorID.Valor;
            File.WriteAllBytes(ConfigurationManager.AppSettings["PathElementoMapa"] + "ElementoBackground/bg" + setor.ToString("000000") + ".gif", img);
            //img.Dispose();
        }
    }

    public class SetorElementoMapaLista : SetorElementoMapaLista_B
    {

        public SetorElementoMapaLista() { }

    }

}
