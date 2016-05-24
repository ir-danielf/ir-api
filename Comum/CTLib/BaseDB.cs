// Versao 1.0

// MSB: Migrando código de VB para C#.

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting.Lifetime;
using System.Threading;


namespace CTLib
{

    // CÓDIGO COMUM A TODAS AS TABELAS
    // O código de definição das Interfaces está comentado.
    #region "Interfaces"

    public interface IBaseBD
    {
        void Ler(int id);
        void Limpar();
        void Desfazer();
        bool Inserir();
        bool Inserir(BD bd);
        bool Atualizar();
        bool Atualizar(BD bd);
        bool Excluir(int id);
        bool Excluir(BD bd, int id);
        bool Excluir();

        //		string NomeClasse{
        //			get;
        //		}
    }

    public interface IBaseProperty
    {
        string Nome
        {
            get;
        }
        int ID
        {
            get;
        }
        int Tamanho
        {
            get;
        }
        string Rotulo
        {
            get;
        }
        string ValorBD
        {
            get;
        }
        bool Alterado();
        void Desfazer();
        void Limpar();
    }

    interface ITextProperty : IBaseProperty
    {
        string Valor
        {
            get;
            set;
        }
        string ValorAntigo
        {
            get;
        }
    }

    interface INumberProperty : IBaseProperty
    {
        decimal Valor
        {
            get;
            set;
        }
        decimal ValorAntigo
        {
            get;
        }
    }

    interface IIntegerProperty : IBaseProperty
    {
        int Valor
        {
            get;
            set;
        }
        int ValorAntigo
        {
            get;
        }
    }

    interface IDateProperty : IBaseProperty
    {
        DateTime Valor
        {
            get;
            set;
        }

        DateTime ValorAntigo
        {
            get;
            set;
        }
    }

    interface IDateTimeProperty : IBaseProperty
    {
        DateTime Valor
        {
            get;
            set;
        }

        DateTime ValorAntigo
        {
            get;
            set;
        }
    }

    interface IBooleanProperty : IBaseProperty
    {
        bool Valor
        {
            get;
            set;
        }
        bool ValorAntigo
        {
            get;
        }
    }

    public interface IBaseLista
    {
        //bool Ler(int id);
        bool Excluir();
        bool ExcluirTudo();
        bool Inserir();
        void Carregar();
        DataTable Tabela();
        DataTable Relatorio();
        IDataReader ListaPropriedade(string campo);
        int[] ToArray();

        IBaseBD this[int indice]
        {
            get;
        }
        int TamanhoMax
        {
            set;
        }
        int Tamanho
        {
            get;
        }
        int Indice
        {
            get;
        }
        string FiltroSQL
        {
            get;
            set;
        }
        string OrdemSQL
        {
            get;
            set;
        }
        bool Primeiro();
        bool Ultimo();
        bool Proximo();
        bool Anterior();
        bool LocalizarID(int ID);
    }

    #endregion

    // CÓDIGO COMUM A TODAS AS CLASSES DE INTERFACES COM BANCO DE DADOS
    // Retirados os códigos das classes referentes à implementação das interfaces.
    #region "Interfaces com Banco de Dados"

    [ObjectType(ObjectType.RemotingType.CAO)]
    public abstract class BaseBD : CrossAppDomainObject, IBaseBD, ISponsoredObject
    {

        public BDControl Control = new BDControl();

        protected BD bd = new BD();
        protected bool nolock = false;

        public abstract void Ler(int id);
        public abstract void Limpar();
        public abstract void Desfazer();
        public abstract bool Inserir();
        public virtual bool Inserir(BD bd)
        {
            return this.Inserir();
        }
        public abstract bool Atualizar();
        public virtual bool Atualizar(BD bd)
        {
            return this.Atualizar();
        }

        public abstract bool Excluir(int id);
        public virtual bool Excluir(BD bd, int id)
        {
            return this.Excluir(id);
        }

        //		public virtual string NomeClasse{
        //			get{ return NomeClasse; }
        //		}

        // Apaga o objeto atual do banco de dados
        public virtual bool Excluir()
        {
            return this.Excluir(this.Control.ID);
        }

        /// <summary>
        /// Buscar registros com hint nolock?!
        /// </summary>
        public bool NoLock
        {
            set { this.nolock = true; }
            get { return this.nolock; }
        }





    }


    #endregion

    // CÓDIGO COMUM A TODOS OS CAMPOS, INDEPENDENTE DO TIPO
    // Retirados os códigos das classes referentes à implementação das interfaces.
    #region "Campos"

    public abstract class BaseProperty : MarshalByRefObject, IBaseProperty
    {
        public const string GLOBAL_CULTURE = "pt-BR";

        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }

        public virtual string Nome
        {
            get { return Nome; }
        }

        public virtual string AtivaBancoIngresso
        {
            get { return AtivaBancoIngresso; }
        }
        public virtual int ID
        {
            get { return ID; }
        }
        public virtual int Tamanho
        {
            get { return Tamanho; }
        }
        public virtual string ValorBD
        {
            get { return ValorBD; }
            set { }
        }

        public string Rotulo
        {
            get
            {
                //if (label == null){
                // MSB: VERIFICAR! O que será feito aqui?
                // '_Label = cInter01.Message(Name, "Rotulo")
                //}
                return null;
            }
        }

        public abstract bool Alterado();
        public abstract void Desfazer();
        public abstract void Limpar();
    }


    // Propriedades texto (string ou memo)
    public abstract class TextProperty : BaseProperty, ITextProperty
    {
        private string valor = "";
        private string valorAntigo = "";

        public virtual string Valor
        {
            get
            {
                return valor;
            }
            set
            {
                valorAntigo = valor;
                valor = value;
            }
        }

        public string ValorAntigo
        {
            get
            {
                return valorAntigo;
            }
        }

        public override string ValorBD
        {
            get
            {
                string s = Valor;
                if (s == null)
                    s = "";
                else
                    s = s.Replace("'", "''");
                return s;
            }
            set
            {
                if (value == null)
                {
                    valor = "";
                    valorAntigo = "";

                }
                else if (value.Trim().Length == 0)
                {
                    valor = "";
                    valorAntigo = "";

                }
                else
                {
                    valor = value;
                    valorAntigo = value;
                }

            }

        }

        public override bool Alterado()
        {
            return (valor != valorAntigo);
        }

        public override void Desfazer()
        {
            valor = valorAntigo;
        }

        public override void Limpar()
        {
            Valor = "";
        }

    }


    // Propriedades numéricas (decimais)
    public abstract class NumberProperty : BaseProperty, INumberProperty
    {
        private decimal valor = 0;
        private decimal valorAntigo = 0;

        public virtual decimal Valor
        {
            get
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                return valor;
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                valorAntigo = valor;
                valor = value;
            }
        }

        public decimal ValorAntigo
        {
            get
            {
                return valorAntigo;
            }
        }

        public override string ValorBD
        {
            get
            {
                return valor.ToString().Replace(",", ".");
            }
            set
            {
                if (value == null)
                {
                    valor = 0M;
                    valorAntigo = 0M;

                }
                else if (value.Trim().Length == 0)
                {
                    valor = 0M;
                    valorAntigo = 0M;

                }
                else
                {
                    try
                    {
                        valor = Convert.ToDecimal(value);
                        valorAntigo = Convert.ToDecimal(value);
                    }
                    catch (Exception erro)
                    {
                        Debug.WriteLine("Erro no ValorBD do NumberProperty (BaseDB.cs) " + erro.Message);
                        Debug.WriteLine("\tValue= " + value + " Valor= " + valor + " Valor Antigo= " + valorAntigo);
                        Debug.WriteLine("\tOs valores foram zerados...");
                        valor = 0M;
                        valorAntigo = 0M;
                    }

                }

            }

        }

        public override bool Alterado()
        {
            return (valor != valorAntigo);
        }

        public override void Desfazer()
        {
            valor = valorAntigo;
        }

        public override void Limpar()
        {
            Valor = 0;
        }
    }

    // Propriedades numéricas (inteiro)
    public abstract class IntegerProperty : BaseProperty, IIntegerProperty
    {
        private int valor = 0;
        private int valorAntigo = 0;
        public virtual int Valor
        {
            get
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                return valor;
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                valorAntigo = valor;
                valor = value;
            }
        }

        public int ValorAntigo
        {
            get
            {
                return valorAntigo;
            }
        }

        public override string ValorBD
        {
            get
            {
                return valor.ToString().Replace(",", ".");
            }
            set
            {
                if (value == null)
                {
                    valor = 0;
                    valorAntigo = 0;

                }
                else if (value.Trim().Length == 0)
                {
                    valor = 0;
                    valorAntigo = 0;

                }
                else
                {
                    // '***aqui - testar o que acontece se recebe string no formato americano
                    try
                    {
                        valor = Convert.ToInt32(value); //*Conserto!
                        valorAntigo = valor;
                    }
                    catch
                    {
                        valor = 0;
                        valorAntigo = 0;
                    }

                }

            }

        }

        public override bool Alterado()
        {
            return (valor != valorAntigo);
        }

        public override void Desfazer()
        {
            valor = valorAntigo;
        }

        public override void Limpar()
        {
            Valor = 0;
        }
    }


    // Propriedades tipo Data
    public abstract class DateProperty : BaseProperty, IDateProperty
    {
        private DateTime valor = System.Windows.Forms.DateTimePicker.MinDateTime.Date;
        private DateTime valorAntigo = System.Windows.Forms.DateTimePicker.MinDateTime.Date;

        public virtual DateTime Valor
        {
            get
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                return valor;
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                valorAntigo = valor;
                valor = value;
            }
        }

        public DateTime ValorAntigo
        {
            get
            {
                return valorAntigo;
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                valorAntigo = valor;
                valor = value;
            }
        }

        // Armazena e recupera a data como yyyyMMdd
        public override string ValorBD
        {
            get
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);

                if (valor == System.Windows.Forms.DateTimePicker.MinDateTime.Date)
                    return "";
                else
                    return (valor.ToString("yyyyMMdd"));
            }

            set
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                try
                {
                    if (value.Trim() != "")
                    {
                        DateTime resp;
                        string ano = value.Substring(0, 4);
                        string mes = value.Substring(4, 2);
                        string dia = value.Substring(6, 2);
                        int a = int.Parse(ano);
                        int m = int.Parse(mes);
                        int d = int.Parse(dia);
                        resp = new DateTime(a, m, d);
                        valor = resp;
                    }
                    else
                    {
                        Limpar();
                    }
                }
                catch (Exception erro)
                {
                    Debug.WriteLine("Erro no ValorBD do DateProperty (BaseDB.cs) " + erro.Message);
                    Debug.WriteLine("\tValue= " + value + " Valor= " + valor + " Valor Antigo= " + valorAntigo);
                    Debug.WriteLine("\tOs valores serão atribuídos com data de Hoje...");
                    valor = System.Windows.Forms.DateTimePicker.MinDateTime.Date;
                    //valorAntigo = DateTime.Today;
                }
                valorAntigo = valor;
            }

        }

        public override bool Alterado()
        {
            return (valor != valorAntigo);
        }

        public override void Desfazer()
        {
            valor = valorAntigo;
        }

        public override void Limpar()
        {
            ///ATENÇÂO:
            Valor = System.Windows.Forms.DateTimePicker.MinDateTime.Date;
        }

    }

    // Propriedades tipo datahora
    public abstract class DateTimeProperty : BaseProperty, IDateTimeProperty
    {
        private DateTime valor = System.Windows.Forms.DateTimePicker.MinDateTime;
        private DateTime valorAntigo = System.Windows.Forms.DateTimePicker.MinDateTime;

        public virtual DateTime Valor
        {
            get
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                return valor;
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                valorAntigo = valor;
                valor = value;
            }
        }

        public DateTime ValorAntigo
        {
            get
            {
                return valorAntigo;
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                valorAntigo = valor;
                valor = value;
            }
        }

        // Armazena e recupera a data como yyyyMMdd
        public override string ValorBD
        {
            get
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);

                if (valor == System.Windows.Forms.DateTimePicker.MinDateTime)
                    return "";
                else
                    return (valor.ToString("yyyyMMddHHmmss"));
            }

            set
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(GLOBAL_CULTURE);
                try
                {
                    if (value.Trim() != "")
                    {
                        DateTime resp;
                        value = value.Replace(" ", "1");
                        string anoBD = value.Substring(0, 4);
                        string mesBD = value.Substring(4, 2);
                        string diaBD = value.Substring(6, 2);
                        string horaBD = value.Substring(8, 2);
                        string minutoBD = value.Substring(10, 2);
                        string segundoBD = value.Substring(12, 2);
                        int ano = int.Parse(anoBD);
                        int mes = int.Parse(mesBD);
                        int dia = int.Parse(diaBD);
                        int hora = int.Parse(horaBD);
                        int minuto = int.Parse(minutoBD);
                        int segundo = int.Parse(segundoBD);
                        resp = new DateTime(ano, mes, dia, hora, minuto, segundo);
                        valor = resp;
                        valorAntigo = valor;
                    }
                    else
                    {
                        Limpar();
                    }
                }
                catch (Exception erro)
                {
                    Debug.WriteLine("Erro no ValorBD do DateTimeProperty (BaseDB.cs) " + erro.Message);
                    Debug.Fail("Erro no ValorBD do DateTimeProperty");
                    valor = System.Windows.Forms.DateTimePicker.MinDateTime;
                }
                valorAntigo = valor;
            }
        }

        public override bool Alterado()
        {
            return (valor != valorAntigo);
        }

        public override void Desfazer()
        {
            valor = valorAntigo;
        }

        public override void Limpar()
        {
            ///ATENÇÂO:
            Valor = System.Windows.Forms.DateTimePicker.MinDateTime;
        }

    }


    // Propriedades lógicas (verdadeiro ou falso)
    public abstract class BooleanProperty : BaseProperty, IBooleanProperty
    {
        private bool valor;
        private bool valorAntigo;

        public virtual bool Valor
        {
            get
            {
                return valor;
            }
            set
            {
                valorAntigo = valor;
                valor = value;
            }
        }

        public bool ValorAntigo
        {
            get
            {
                return valorAntigo;
            }
        }

        public override string ValorBD
        {
            get
            {
                return (valor) ? "T" : "F";
            }
            set
            {
                if (value == null)
                {
                    valor = false;
                    valorAntigo = false;

                }
                else if (value.Trim().Length == 0)
                {
                    valor = false;
                    valorAntigo = false;

                }
                else
                {
                    valor = (value.Trim().ToUpper() == "T");
                    valorAntigo = valor;
                }

            }

        }

        public override bool Alterado()
        {
            return (valor != valorAntigo);
        }

        public override void Desfazer()
        {
            valor = valorAntigo;
        }

        public override void Limpar()
        {
            Valor = false;
        }
    }


    // Controle de atualização do objeto
    public class BDControl : MarshalByRefObject
    {

        private int id;
        private int idAntigo;
        private int versao;
        private string timeStamp;
        private int usuarioID;

        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }

        public int ID
        {
            get { return id; }
            set
            {
                idAntigo = id;
                id = value;
            }
        }

        public int Versao
        {
            get { return versao; }
            set { versao = value; }
        }

        public string TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public int UsuarioID
        {
            get { return usuarioID; }
            set { usuarioID = value; }
        }

        public void Desfazer()
        {
            id = idAntigo;
        }

    }


    // Representa um conjunto de objetos (toda uma tabela ou um subconjunto)
    public abstract class BaseLista : MarshalByRefObject, IBaseLista, ISponsoredObject
    {

        protected BD bd = new BD();
        protected ArrayList lista = new ArrayList();
        private int i;
        protected string filtro = null; //sem filtro
        protected string ordem = "1"; //ordenacao pelo primeiro campo
        protected int tamanhoMax = 0; //zero, nao tem tamanho maximo
        protected bool noLock = false;
        protected ArrayList listaBasePesquisa = new ArrayList();

        protected abstract void Ler(int id);
        public abstract bool Excluir();
        public abstract bool ExcluirTudo();
        public abstract bool Inserir();
        public abstract void Carregar();
        public abstract DataTable Tabela();
        public abstract DataTable Relatorio();
        public abstract IDataReader ListaPropriedade(string campo);
        public abstract int[] ToArray();

        public BaseLista()
        {
            ordem = "1";
        }

        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }

        /// <summary>
        /// Buscar registros com hint NOLOCK
        /// </summary>
        public bool NoLock
        {
            set
            {
                noLock = value;
            }
            get
            {
                return noLock;
            }
        }

        /// <summary>
        /// Tamanho maximo que a lista pode conter ao carregar os itens
        /// </summary>
        public int TamanhoMax
        {
            set
            {
                if (lista.Count > 0) //lista ja esta carregada
                    if (value > lista.Count) //se o tamanho for maior que a lista, ok.
                        tamanhoMax = value;
                    else //se nao, seta para o tamanho da lista
                        tamanhoMax = lista.Count;
                else //se lista nao carregada, ok.
                    tamanhoMax = value;
            }
        }

        /// <summary>
        /// Tamanho da lista
        /// </summary>
        public int Tamanho
        {
            get
            {
                return lista.Count;
            }
        }

        /// <summary>
        /// item da lista
        /// </summary>
        public abstract IBaseBD this[int indice]
        {
            get;
        }

        /// <summary>
        /// indice atual da lista
        /// </summary>
        public int Indice
        {
            get
            {
                return i;
            }
            set
            {
                if (value < lista.Count && value >= 0)
                { //verifica se o indice esta correto.
                    i = value;
                    this.Ler((int)lista[i]);
                }
            }

        }

        /// <summary>
        /// filtro sql acumulativo.
        /// </summary>
        public string FiltroSQL
        {
            get
            {
                return filtro;
            }
            set
            {
                if (value != null)
                {
                    if (filtro == null) //zera o filtro
                        filtro = value;
                    else
                        if (filtro != "") //acumula o filtroSQL usando AND
                            filtro += " AND " + value;
                        else
                            filtro = value;
                }
                else
                {
                    filtro = null;
                }
            }
        }

        /// <summary>
        /// Order by...
        /// </summary>
        public string OrdemSQL
        {
            get
            {
                return ordem;

            }
            set
            {
                ordem = value;
            }
        }

        /// <summary>
        /// Move para o primeiro registro
        /// </summary>
        /// <returns></returns>
        public bool Primeiro()
        {
            bool ok;

            if (lista.Count > 0)
            {
                Indice = 0;
                ok = true;

            }
            else
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Move para o ultimo registro 
        /// </summary>
        /// <returns></returns>
        public bool Ultimo()
        {
            bool ok;

            if (lista.Count > 0)
            {
                Indice = lista.Count - 1;
                ok = true;

            }
            else
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Move para o proximo registro 
        /// </summary>
        /// <returns></returns>
        public bool Proximo()
        {
            bool ok;

            if (Indice < (lista.Count - 1))
            {
                Indice++;
                ok = true;

            }
            else
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Move para o registro anterior
        /// </summary>
        /// <returns></returns>
        public bool Anterior()
        {
            bool ok;
            if (Indice > 0)
            {
                Indice--;
                ok = true;

            }
            else
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Posiciona para a posição da Lista conforme o ID
        /// </summary>
        /// <param name="ID">Valor do ID informado</param>
        public bool LocalizarID(int IDInformado)
        {
            bool ok;
            if (lista.Count > 0)
            {
                Indice = lista.IndexOf(IDInformado);
                ok = true;
            }
            else
            {
                ok = false;
            }
            return ok;
        }


    }

    #endregion
}