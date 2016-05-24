using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "IRMensagem_B"

    public abstract class IRMensagem_B : BaseBD
    {

        public msgKey MsgKey = new msgKey();
        public msgContent MsgContent = new msgContent();

        public IRMensagem_B() { }

        // passar o Usuario logado no sistema
        public IRMensagem_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de PacoteItem
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {
            try
            {
                string sql = "SELECT * FROM IRMessages WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.MsgKey.ValorBD = bd.LerString("msgKey").ToString();
                    this.MsgContent.ValorBD = bd.LerString("msgKey").ToString();
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

        public override bool Inserir()
        {
            return false;
        }

        public override bool Atualizar()
        {
            return false;
        }

        public override bool Excluir(int id)
        {
            return false;
        }

        public override void Limpar()
        {

            this.MsgKey.Limpar();
            this.MsgContent.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {
            this.Control.Desfazer();
            this.MsgKey.Desfazer();
            this.MsgContent.Desfazer();
        }

        public class msgKey : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "MsgKey";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }
        }

        public class msgContent : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "MsgContent";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 255;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        /// <summary>
        /// Obtem uma tabela estruturada com todos os campos dessa classe.
        /// </summary>
        /// <returns></returns>
        public static DataTable Estrutura()
        {

            //Isso eh util para desacoplamento.
            //A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.

            try
            {
                DataTable tabela = new DataTable("IRMensagem");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("MsgKey", typeof(string));
                tabela.Columns.Add("MsgContent", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "IRMensagemException"

    [Serializable]
    public class IRMensagemException : Exception
    {

        public IRMensagemException() : base() { }

        public IRMensagemException(string msg) : base(msg) { }

        public IRMensagemException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}