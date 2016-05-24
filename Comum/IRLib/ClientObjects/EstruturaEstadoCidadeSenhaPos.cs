using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEstadoCidadeSenhaPos
    {
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Senha { get; set; }
        public string pg { get; set; }
        public int CanalID { get; set; }
        public int LojaID { get; set; }
        public int EmpresaID { get; set; }
        public int UsuarioID { get; set; }
        public string UsuarioNome { get; set; }
    }
}
