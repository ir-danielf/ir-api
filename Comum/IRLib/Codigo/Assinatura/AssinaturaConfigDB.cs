/******************************************************
* Arquivo AssinaturaAnoDB.cs
* Gerado em: 20/09/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "AssinaturaConfig_B"

    public abstract class AssinaturaConfig_B : BaseBD
    {

        public nome Nome = new nome();
        public assinaturaID Ano = new assinaturaID();
        public anoAtivoAssinatura AnoAtivoAssinatura = new anoAtivoAssinatura();
        public anoAtivoBancoIngressos AnoAtivoBancoIngressos = new anoAtivoBancoIngressos();
        public anoAtivo AnoAtivo = new anoAtivo();
        public AssinaturaTipoIDBancoIngresso AssinaturaTipoBancoIngresso = new AssinaturaTipoIDBancoIngresso();
        
        public AssinaturaConfig_B() { }

        // passar o Usuario logado no sistema
        public AssinaturaConfig_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        public class nome : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Nome";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
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

        public class assinaturaID : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
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

        public class anoAtivoAssinatura : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AnoAtivoAssinatura";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
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

        public class anoAtivoBancoIngressos : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AnoAtivoBancoIngressos";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
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

        public class anoAtivo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AnoAtivo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
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

        public class AssinaturaTipoIDBancoIngresso : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaTipoIDBancoIngresso";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
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

        public override void Ler(int id)
        {
           
        }

        public override void Limpar()
        {
            
        }

        public override void Desfazer()
        {
            
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


    }
    #endregion

    

    #region "AssinaturaConfigException"

    [Serializable]
    public class AssinaturaConfigException : Exception
    {

        public AssinaturaConfigException() : base() { }

        public AssinaturaConfigException(string msg) : base(msg) { }

    }

    #endregion

}