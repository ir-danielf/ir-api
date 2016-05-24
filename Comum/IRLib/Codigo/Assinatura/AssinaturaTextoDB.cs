using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    public partial class AssinaturaTexto_B : BaseBD
    {
        public override bool Excluir(int id)
        { return false; }

        public override void Ler(int id)
        {  }

        public override bool Inserir()
        { return false; }

        public override bool Atualizar()
        { return false; }

        public override void Desfazer()
        { }
        
        public override void Limpar()
        { }
    }
}
