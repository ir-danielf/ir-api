using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ObjectExtensions
    {
        public static T CopyFrom<T>(this T o, object from, string[] propriedades = null)
        {
            if(o == null)
                o = (T)Activator.CreateInstance(typeof(T));
            if(from == null)
                return o;
            var fromType = from.GetType();
            PropertyInfo[] props;
            if (propriedades == null)
            {
                 props = o.GetType().GetProperties();
            }
            else
            {
                var toType = o.GetType(); 
                int tamanho = propriedades.Count();
                props = new PropertyInfo[tamanho];
                for(int i = 0; i< tamanho; i++)
                {
                    props[i] = toType.GetProperty(propriedades[i]);
                }
            }
            
            
            foreach (var prop in props)
            {
                if(prop != null)
                {
                    var fromProp = fromType.GetProperty(prop.Name);
                    if (fromProp != null)
                    {
                        try
                        {
                            prop.SetValue(o, fromProp.GetValue(from));
                        }
                        catch { }
                        
                    }
                }
            }
            return o;
            
        }

        public static T CopyTo<T>(this object o, T to, string[] propriedades = null)
        {
            if(to == null)
                to = (T)Activator.CreateInstance(typeof(T));
            return to.CopyFrom(o, propriedades);

        }

        public static T GetAsType<T>(this object o)
        {
            return (T)o;

        }

        public static bool PropsIsNullOrEmpty(this object o, string[] propriedades = null)
        {
            bool retorno = true;
            foreach(var propriedade in propriedades)
            {
                if(!o.PropIsNullOrEmpty(propriedade))
                {
                    retorno = false;
                }
            }
            return retorno;

        }

        public static bool PropIsNullOrEmpty(this object o, string propriedade)
        {
            var valor = o.GetPropByNameAsString(propriedade);
            return (string.IsNullOrEmpty(valor) || (valor == "0"));
        }

        public static object GetPropByName(this object o, string propriedade)
        {
            var tipo = o.GetType();
            var prop = tipo.GetProperty(propriedade);
            return prop.GetValue(o);
        }

        public static void SetPropByName(this object o, string propriedade, object value)
        {
            var tipo = o.GetType();
            var prop = tipo.GetProperty(propriedade);
            prop.SetValue(o, value);
        }

        public static T GetPropByName<T>(this object o, string propriedade)
        {
            return (T)o.GetPropByName(propriedade);
        }

        public static string GetPropByNameAsString(this object o, string propriedade)
        {
            var valor = o.GetPropByName(propriedade);
            if (valor == null)
            {
                return null;
            }
            else
            {
                return valor.ToString();
            }
            
        }

        public static int GetPropByNameAsInt(this object o, string propriedade)
        {
            return Convert.ToInt32(o.GetPropByNameAsString(propriedade));

        }

    }
}
