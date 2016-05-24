using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    public abstract class ListaBancos_B : BaseBD
    {
        public codigo Codigo = new codigo();
        public nomebanco NomeBanco = new nomebanco();
        public irdeposita IRDeposita = new irdeposita();

        public ListaBancos_B() { }

        // passar o Usuario logado no sistema
        public ListaBancos_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        public override void Ler(int id)
        {
            throw new NotImplementedException();
        }

        public override void Limpar()
        {
            throw new NotImplementedException();
        }

        public override void Desfazer()
        {
            throw new NotImplementedException();
        }

        public override bool Inserir()
        {
            throw new NotImplementedException();
        }

        public override bool Atualizar()
        {
            throw new NotImplementedException();
        }

        public override bool Excluir(int id)
        {
            throw new NotImplementedException();
        }



        public class codigo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Codigo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
                return base.Valor.ToString();
            }

        }

        public class nomebanco : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeBanco";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class irdeposita : BooleanProperty
        {
            public override string Nome
            {
                get
                {
                    return "IRDeposita";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override bool Valor
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
                return base.Valor.ToString();
            }

        }

    }
}
