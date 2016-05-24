using IRCore.DataAccess.ADO.Exceptions;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Estrutura
{
    public abstract class MasterADO<TDB> : MasterADOBase where TDB : MasterADOType
    {

        public MasterADO(MasterADOBase ado, bool hasControle, bool hasControleIndentity = false) : base (ado) 
        {
            HasControle = hasControle;
            HasControleIndentity = hasControleIndentity;
        }

        public MasterADO(bool hasControle, bool hasControleIndentity = false): base(null)
        {
            HasControle = hasControle;
            HasControleIndentity = hasControleIndentity;
        }

        public MasterADO(MasterADOBase ado = null) : base(ado) 
        {
            HasControle = (typeof(TDB) == typeof(dbIngresso));
            HasControleIndentity = false;
        }

        private List<DbContextTransaction> Transacoes = new List<DbContextTransaction>();

        protected bool HasControle { get; set; }
        
        protected bool HasControleIndentity { get; set; }
        
        protected bool NoLock
        {
            get { return (!NoLockInterceptor.SuppressNoLock); }
            set { NoLockInterceptor.SuppressNoLock = (!value); }
        }

        protected DbContext dbPrincipal
        {
            get
            {
                if (typeof(TDB) == typeof(dbIngresso))
                {
                    return dbIngresso; 
                }
                else
                {
                    return dbSite;
                }
            }
        }

        public int IniciarTransacaoDb(bool noLock = true, DbContext db = null)
        {
            int transacaoId = Transacoes.Count;
            if (db == null)
            {
                db = dbPrincipal;
            }
            if (noLock)
            {
                Transacoes.Add(db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted));
            }
            else
            {
                Transacoes.Add(db.Database.BeginTransaction());
            }
            return transacaoId;
        }
        
        public int IniciarTransacao(bool noLock = true)
        {
            return IniciarTransacaoDb(noLock);
        }

        public void FinalizarTransacaoDb(int transacaoId, bool persistir = true, DbContext db = null)
        {
            if (db == null)
            {
                db = dbPrincipal;
            }
            if(persistir)
            {
                Transacoes[transacaoId].Commit();
            }
            else
            {
                Transacoes[transacaoId].Rollback();
            }
            
        }

        public void FinalizarTransacao(int transacaoId, bool persistir = true)
        {
            FinalizarTransacaoDb(transacaoId, persistir);
        }

        public T ExecutarSQLForItem<T>(string sql, DbContext db = null)
        {
            return ExecutarConsultaSQL<T>(sql, db).FirstOrDefault();
        }
        
        public List<T> ExecutarSQLForList<T>(string sql, DbContext db = null)
        {
            var consulta = ExecutarConsultaSQL<T>(sql, db);
            return consulta.ToList();
        }

        public IQueryable<T> ExecutarConsultaSQL<T>(string sql, DbContext db = null)
        {
            if (db == null)
            {
                db = dbPrincipal;
            }
            var dbConsulta = db.Database.SqlQuery<T>(sql);
            return dbConsulta.AsQueryable();
        }

        public int ExecutarComandoSQL<T>(string sql, DbContext db = null)
        {
            if (db == null)
            {
                db = dbPrincipal;
            }
            return db.Database.ExecuteSqlCommand(sql);
        }

        private string[] ExpToStr<T, TKey>(Expression<Func<T, TKey>> exp)
        {
            string[] retorno = null;
            if(exp != null)
            {
                var tipo = typeof(TKey);
                
                if (tipo.Name.StartsWith("<>f__AnonymousType"))
                {
                    retorno = tipo.GetProperties().Select(t => t.Name).ToArray();
                }
                else if (tipo.Name.StartsWith("ICollection"))
                {
                    if((tipo.GenericTypeArguments != null) && (tipo.GenericTypeArguments.Count() > 0))
                    {
                        retorno = new string[] { tipo.GenericTypeArguments[0].Name };
                    }
                    
                }
                else
                {
                    retorno = new string[] { tipo.Name };
                }
            }
            return retorno;
        }

        public IQueryable<T> CarregarReferenciaStr<T>(IQueryable<T> query, params string[] referencias) where T : class
        {
            if ((referencias != null) && (query != null))
            {
                foreach (var load in referencias)
                {
                    query = query.Include(load);
                }
            }
            return query;
        }

        public IQueryable<T> CarregarReferencia<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> referencias) where T : class
        {
            return CarregarReferenciaStr(query, ExpToStr(referencias));
        }

        public T CarregarReferencia<T, TKey>(T obj, Expression<Func<T, TKey>> referencias) where T : class
        {
            return CarregarReferenciaStrDb(obj, null, ExpToStr(referencias));
        }

        public T CarregarReferenciaDb<T, TKey>(T obj, DbContext db, Expression<Func<T, TKey>> referencias) where T : class 
        {
            return CarregarReferenciaStrDb(obj, db, ExpToStr(referencias));
        }
        
        public T CarregarReferenciaStr<T>(T obj, params string[] referencias) where T : class
        {
            return CarregarReferenciaStrDb(obj, null, referencias);
        }

        public T CarregarReferenciaStrDb<T>(T obj, DbContext db, params string[] referencias) where T : class
        {
            if (db == null)
            {
                db = dbPrincipal;
            }
            if ((referencias != null) && (obj != null))
            {
                if (db.Entry<T>(obj).State == EntityState.Detached)
                {
                    db.Set<T>().Attach(obj);
                }
                foreach (var load in referencias)
                {
                    if (obj.GetType().GetProperties().FirstOrDefault(t => t.Name == load).PropertyType.Name.StartsWith("ICollection"))
                    {
                        db.Entry<T>(obj).Collection(load).Load();
                    }
                    else
                    {
                        db.Entry<T>(obj).Reference(load).Load();
                    }
                }
            }
            return obj;
        }

        public bool Salvar<T, TKey>(T obj, Expression<Func<T, TKey>> pk, bool updateDataBase = true, bool withException = false) where T : class
        {

            return Salvar(obj, updateDataBase, withException, ExpToStr(pk));
        }

        public bool Salvar<T>(T obj, bool updateDataBase = true, bool withException = false, params string[] pkNames) where T : class
        {
            if(HasControle)
            {
                throw new ADOException("Para esse objeto é obrigatório passar o parâmetro usuarioID para salvar na tabela de Controle");
                
            }
            return dbPrincipal.Save(obj, updateDataBase, withException, pkNames);
            
        }

        public bool Salvar(bool withException = false)
        {
            return dbPrincipal.Save(withException);
        }

        private int ControleProximoID(Type T, Type C, Type X)
        {
            var ids = new List<int>();

            foreach (dynamic id in dbPrincipal.Set(T).OrderBy("ID desc").Select("ID").Take(1))
            {
                ids.Add(id + 1);
            }
            foreach (dynamic id in dbPrincipal.Set(C).OrderBy("ID desc").Select("ID").Take(1))
            {
                ids.Add(id + 1);
            }
            foreach (dynamic id in dbPrincipal.Set(X).OrderBy("ID desc").Select("ID").Take(1))
            {
                ids.Add(id + 1);
            }

            return ids.Max();
        }

        public int ControleProximoID<T>()
        {
            var tipo = typeof(T);
            Type C = tipo.Assembly.GetType(tipo.Namespace + ".c" + tipo.Name.Substring(1));
            Type X = tipo.Assembly.GetType(tipo.Namespace + ".x" + tipo.Name.Substring(1));

            return ControleProximoID(tipo, C, X);
        }

        public int ControleIncrementaID<T>(T obj)
        {
            var tipo = typeof(T);
            Type C = tipo.Assembly.GetType(tipo.Namespace + ".c" + tipo.Name.Substring(1));
            Type X = tipo.Assembly.GetType(tipo.Namespace + ".x" + tipo.Name.Substring(1));

            int ID = ControleProximoID(tipo, C, X);

            obj.SetPropByName("ID", ID);

            return ID;
        }

        public int ControleMaiorVersao<T>(T obj) where T : class
        {
            var tipo = typeof(T);
            Type C = tipo.Assembly.GetType(tipo.Namespace + ".c" + tipo.Name.Substring(1));
            Type X = tipo.Assembly.GetType(tipo.Namespace + ".x" + tipo.Name.Substring(1));

            return ControleMaiorVersao(obj, C, X);
        }

        private int ControleMaiorVersao<T>(T obj, Type C, Type X) where T : class
        {
            int ID = obj.GetPropByNameAsInt("ID");
            var ct = dbPrincipal.Set(C).Where("ID == " + ID);
            var xt = dbPrincipal.Set(X).Where("ID == " + ID);
            var versoes = new List<int>();
            if (ct.Count() != 0)
            {
                foreach (dynamic versao in ct.OrderBy("Versao desc").Select("Versao").Take(1))
                {
                    versoes.Add(versao);
                }
            }
            else
            {
                versoes.Add(0);
            }
            if (xt.Count() != 0)
            {
                foreach (dynamic versao in xt.OrderBy("Versao desc").Select("Versao").Take(1))
                {
                    versoes.Add(versao + 1);
                }
            }
            else
            {
                versoes.Add(1);
            }
            return versoes.Max();
        }

        public bool Salvar<T>(T obj, int usuarioID, bool withException = false) where T : class
        {
            return Salvar(obj, usuarioID, HasControleIndentity, withException);
        }
        public bool Salvar<T>(T obj, int usuarioID, bool isIndetity, bool withException) where T : class
        {
            var tipo = typeof(T);
            Type C = tipo.Assembly.GetType(tipo.Namespace + ".c" + tipo.Name.Substring(1));
            Type X = tipo.Assembly.GetType(tipo.Namespace + ".x" + tipo.Name.Substring(1));

            return Salvar(obj, usuarioID, isIndetity, withException, C, X);
        }

        private bool Salvar<T>(T obj, int usuarioID, bool isIndetity, bool withException, Type C, Type X) 
            where T : class
        {

            if (!HasControle)
            {
                throw new ADOException("Este método só deve ser utilizado por tabelas que tenham Contole T, C e X");
            }
            var controle = Activator.CreateInstance(C);
            
            int ID = obj.GetPropByNameAsInt("ID");

            if (ID == 0)
            {

                if(isIndetity)
                {
                    dbPrincipal.Set<T>().Add(obj);
                    Salvar(withException);

                    ID = obj.GetPropByNameAsInt("ID");
                }
                else
                {
                    ID = ControleIncrementaID(obj);

                    dbPrincipal.Set<T>().Add(obj);
                }
                

                controle.SetPropByName("Acao", "I");
                controle.SetPropByName("Versao", 0);
            }
            else
            {
                controle.SetPropByName("Acao", "U");

                int Versao = ControleMaiorVersao(obj);
                
                var bkp = Activator.CreateInstance(X);
                T objAtual = (T)Activator.CreateInstance(typeof(T));
                objAtual.CopyFrom(obj);

                dbPrincipal.Set<T>().Attach(obj);
                dbPrincipal.Entry<T>(obj).Reload();

                bkp.CopyFrom(obj);
                bkp.SetPropByName("Versao", Versao);
                dbPrincipal.Set(X).Add(bkp);

                obj.CopyFrom(objAtual);

                controle.SetPropByName("Versao", Versao + 1);


                dbPrincipal.Save(obj, false, withException);

            }

            controle.SetPropByName("UsuarioID", usuarioID);
            controle.SetPropByName("ID", ID);
            controle.SetPropByName("TimeStamp", DateTime.Now.ToString("yyyyMMddHHmmss"));

            dbPrincipal.Set(C).Add(controle);

            return Salvar(withException);
            
        }

        public bool Remover<T>(T obj, bool updateDataBase = true, bool withException = false) where T : class
        {
            if (HasControle)
            {
                throw new ADOException("Para esse objeto é obrigatório passar o parâmetro usuarioID para salvar na tabela de Controle");
            }
            return dbPrincipal.Remove(obj, updateDataBase);

        }

        public bool Remover<T>(T obj, int usuarioID, bool withException = false) where T : class
        {
            var tipo = typeof(T);
            Type C = tipo.Assembly.GetType(tipo.Namespace + ".c" + tipo.Name.Substring(1));
            Type X = tipo.Assembly.GetType(tipo.Namespace + ".x" + tipo.Name.Substring(1));

            return Remover(obj, usuarioID, withException, C, X);

        }

        private bool Remover<T>(T obj, int usuarioID, bool withException, Type C, Type X) where T : class
        {
            if (!HasControle)
            {
                throw new ADOException("Este método só deve ser utilizado por tabelas que tenham Contole T, C e X");
            }
            var controle = Activator.CreateInstance(C);
            var bkp = Activator.CreateInstance(X);

            int ID = obj.GetPropByNameAsInt("ID");
            int Versao = ControleMaiorVersao(obj);
            
            bkp.CopyFrom(obj);
            bkp.SetPropByName("Versao", Versao);
            dbPrincipal.Set(X).Add(bkp);


            controle.SetPropByName("Acao", "D");
            controle.SetPropByName("Versao", Versao + 1);
            controle.SetPropByName("UsuarioID", usuarioID);
            controle.SetPropByName("ID", ID);
            controle.SetPropByName("TimeStamp", DateTime.Now.ToString("yyyyMMddHHmmss"));

            dbPrincipal.Set(C).Add(controle);

            return dbPrincipal.Remove(obj);

        }

        
    
    }

    public class MasterADOType {}
    
    public class dbIngresso : MasterADOType { }

    public class dbSite : MasterADOType { }

}
