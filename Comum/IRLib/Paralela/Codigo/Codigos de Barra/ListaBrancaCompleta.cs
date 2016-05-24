/**************************************************
* Arquivo: ListaBrancaCompleta.cs
* Gerado: 03/08/2011
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Text;

namespace IRLib.Paralela
{

    public class ListaBrancaCompleta : ListaBrancaCompleta_B
    {

        public ListaBrancaCompleta() { }

        public void Gerar(int quantidade)
        {
            try
            {
                string max = bd.ConsultaValor("SELECT ISNULL(MAX(CodigoBarra), '0') FROM tListaBrancaCompleta (NOLOCK)").ToString();
                if (string.IsNullOrEmpty(max))
                    max = "0";


                int iniciarEm = Convert.ToInt32(max) + 1;

                int finalizarEm = iniciarEm + quantidade;

                StringBuilder stb = new StringBuilder();
                for (int i = iniciarEm; i <= finalizarEm; i++)
                    bd.Executar(this.StringInserir(i));

                //bd.Executar(stb);

            }
            finally
            {
                bd.Fechar();
            }
        }

        private string StringInserir(int codigo)
        {
            return "INSERT INTO tListaBrancaCompleta (CodigoBarra, Utilizado, DataUtilizado) VALUES ('" + codigo.ToString("00000000000000") + "', 'F', '')";
        }
    }

    public class ListaBrancaCompletaLista : ListaBrancaCompletaLista_B
    {

        public ListaBrancaCompletaLista() { }

    }

}
