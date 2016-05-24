using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaUsuario : ICloneable
    {

        public int ID { get; set; }
        public int EmpresaID { get; set; }
        public int RegionalID { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Status { get; set; }
        public bool Validade { get; set; }
        public string StatusVisualizar
        {
            get
            {
                switch (Status)
                {
                    case "L":
                        return "Liberado";
                    case "T":
                        return "Temporário";
                    case "B":
                        return "Bloqueado";
                    default:
                        return "";
                }

            }
        }
        public string ValidadeVisualizar
        {
            get
            {
                return Validade ? "Sim" : "Não";
            }
        }
        public DateTime ValidoDe { get; set; }
        public DateTime ValidoAte { get; set; }
        public string CodigoTerminal { get; set; }





        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
