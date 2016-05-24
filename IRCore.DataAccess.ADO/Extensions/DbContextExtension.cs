using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using IRCore.DataAccess.Model;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using IRCore.Util;
using IRCore.DataAccess.ADO.Exceptions;

namespace IRCore.DataAccess.Model
{
    public static class DbContextExtension
    {
        public static T ResolveAttach<T>(this DbContext db, T obj, params string[] pkNames) where T : class
        {
            if ((pkNames == null) || (pkNames.Length == 0))
            {
                pkNames = new string[] { "ID" };
            }
            // Verifica se o obejto esta vinculado no Local
            if (!db.Set<T>().Local.Contains(obj))
            {
                T objLocal = null;
                if (!obj.PropsIsNullOrEmpty(pkNames))
                {
                    string where = "";
                    int tamanho = pkNames.Count();
                    object[] valores = new object[tamanho];
                    for (int i = 0; i < tamanho; i++)
                    {
                        where += string.IsNullOrEmpty(where) ? "" : " And ";
                        where += pkNames[i] + " = @" + i;
                        valores[i] = obj.GetPropByName(pkNames[i]);
                    }
                    objLocal = db.Set<T>().Local.AsQueryable<T>().Where<T>(where, valores).FirstOrDefault();
                }
                // Verifica se o obejto com mesmo ID já esta vinculado no Local
                if (objLocal != null)
                {
                    db.Entry<T>(objLocal).CurrentValues.SetValues(obj);
                    obj = objLocal;
                }
                else
                {
                    try
                    {
                        db.Set<T>().Attach(obj);
                    }
                    catch
                    {
                        obj = obj.CopyTo<T>(null);
                        db.Set<T>().Attach(obj);
                    }
                }

            }
            return obj;
        }

        public static bool Remove<T>(this DbContext db, T obj, bool updateDataBase = true, bool withException = false, params string[] pkNames) where T : class
        {
            if ((pkNames == null) || (pkNames.Length == 0))
            {
                pkNames = new string[] { "ID" };
            }
            if (obj != null)
            {
                obj = db.ResolveAttach<T>(obj, pkNames);
                db.Entry<T>(obj).State = EntityState.Deleted;
                if (updateDataBase)
                {
                    return db.Save(withException);
                }
                return true;
            }
            return false;
        }
        
        public static bool Save<T>(this DbContext db, T obj, bool updateDataBase = true, bool withException = false, params string[] pkNames) where T : class
        {
            if ((pkNames == null) || (pkNames.Length == 0))
            {
                pkNames = new string[] { "ID" };
            }
            if (obj != null)
            {
                if ((db.Entry<T>(obj).State != EntityState.Modified) && ((db.Entry<T>(obj).State != EntityState.Added)))
                {
                    obj = db.ResolveAttach<T>(obj, pkNames);
                    db.Entry<T>(obj).State = ((obj.PropsIsNullOrEmpty(pkNames))?EntityState.Added:EntityState.Modified);                   
                }
                
                if (updateDataBase)
                {
                    return db.Save(withException);
                }
                return true;
            }
            return false;
        }

        public static bool Save(this DbContext db, bool withException = false)
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                LogUtil.Error(dbEx);
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {

                        LogUtil.ErrorTitulo("Erro na coluna " + validationErrors.Entry.Entity.GetType().Name + "." + validationError.PropertyName, "Mensagem: " + validationError.ErrorMessage);
                        if(withException)
                        {
                            throw new ADOException("Erro na coluna " + validationErrors.Entry.Entity.GetType().Name + "." + validationError.PropertyName + " com Mensagem: " + validationError.ErrorMessage, dbEx);
                        }
                    }
                }
                return false;
            }
        }

        public static bool Remove<T>(this IRCoreEntitiesSite db, T obj, bool updateDataBase = true, bool withException = false, params string[] pkNames) where T : class
        {
            return ((DbContext)db).Remove(obj, updateDataBase, withException, pkNames);
        }

        public static bool Save<T>(this IRCoreEntitiesSite db, T obj, bool updateDataBase = true, bool withException = false, params string[] pkNames) where T : class
        {
            return ((DbContext)db).Save(obj, updateDataBase, withException, pkNames);
        }

        public static bool Save(this IRCoreEntitiesSite db)
        {
            return ((DbContext)db).Save();
        }


        public static bool Remove<T>(this IRCoreEntitiesIngresso db, T obj, bool updateDataBase = true, bool withException = false, params string[] pkNames) where T : class
        {
            return ((DbContext)db).Remove(obj, updateDataBase, withException, pkNames);
        }
        public static bool Save<T>(this IRCoreEntitiesIngresso db, T obj, bool updateDataBase = true, bool withException = false, params string[] pkNames) where T : class
        {
            return ((DbContext)db).Save(obj, updateDataBase, withException, pkNames);
        }

        public static bool Save(this IRCoreEntitiesIngresso db)
        {
            return ((DbContext)db).Save();
        }
    }
}
