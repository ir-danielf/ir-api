using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
namespace IRLib.Paralela
{
    public class ContatoTipo : ContatoTipo_B
    {
        public List<EstruturaIDNome> ValoresComboBoxContatoTipo()
        {
            List<EstruturaIDNome> listaCombo = new List<EstruturaIDNome>();

            try
            {
                string sql = "Select ID, Nome FROM tContatoTipo (NOLOCK)";

                bd.Consulta(sql);
                listaCombo.Add(new EstruturaIDNome() { ID = 0, Nome = string.Empty, });

                while (bd.Consulta().Read())
                {
                    listaCombo.Add(new EstruturaIDNome() { ID =  bd.LerInt("ID"), Nome = bd.LerString("Nome") });
                }
                return listaCombo;
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

        public string TipoContato(int ContatoID)
        {
            string contato = "";

            if (ContatoID == 0)
            {
                return contato = "-";
            }

            string sql = "SELECT nome FROM tContatoTipo WHERE ID = " + ContatoID;
            bd.Consulta(sql);

            if (bd.Consulta().Read())
            {
                contato = bd.LerString("nome");
            }

            return contato;

        }
    }
}
