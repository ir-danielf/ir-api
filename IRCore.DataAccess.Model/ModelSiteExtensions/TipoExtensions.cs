namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using IRCore.Util;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public partial class Tipo
    {

        public string SiteCor
        {
            get
            {
                try
                {
                    var cor = ConfiguracaoAppUtil.GetAsDictionary(enumConfiguracaoModel.siteTiposCor);
                    if (cor.ContainsKey(IR_TipoID.ToString()))
                    {
                        return cor[IR_TipoID.ToString()];
                    }
                    else
                    {
                        return cor["0"];
                    }
                }
                catch
                {
                    return "";
                }
                
            }
        }

        public int SiteOrderm
        {
            get
            {
                try
                {
                    var ordem = ConfiguracaoAppUtil.GetAsListInt(enumConfiguracaoModel.siteTiposOrdem);
                    var item = ordem.FirstOrDefault(t => t == ID);
                    if (ordem.Contains(IR_TipoID))
                    {
                        return ordem.IndexOf(IR_TipoID);
                    }
                    else
                    {
                        return ordem.Count;
                    }
                }
                catch
                {
                    return 0;
                }
                
            }
            
        }

    }
}
