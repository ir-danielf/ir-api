using CTLib;
using System;
using System.Data;
using System.Text;

namespace IRLib
{

    public class IRMensagem : IRMensagem_B
    {

        public IRMensagem() { }

        public IRMensagem(int usuarioIDLogado) : base(usuarioIDLogado) { }

        #region Métodos de Manipulação do Pacote Mensagem

        public void LerChave(string MsgKey)
        {
            try
            {
                string sql = String.Format("SELECT * FROM IRMessages WHERE MsgKey = '{0}'", MsgKey);
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = int.Parse(bd.LerInt("ID").ToString());
                    this.MsgKey.ValorBD = bd.LerString("MsgKey").ToString();
                    this.MsgContent.ValorBD = bd.LerString("MsgContent").ToString();
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

}
