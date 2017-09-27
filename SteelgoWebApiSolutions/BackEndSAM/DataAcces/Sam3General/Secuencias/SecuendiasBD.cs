using DatabaseManager.Sam3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General.Secuencias
{
    public class SecuenciasBD
    {
        private static readonly object _mutex = new object();
        private static SecuenciasBD _instance;

        public static SecuenciasBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SecuenciasBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerFolio( int proyectoID, string modulo, string campoExtra)
        {
            try
            {
                String result = "";
                using (SamContext ctx = new SamContext())
                {
                    
                     result = ctx.Sam3_Steelgo_Get_Secuencia(proyectoID,modulo,campoExtra).ToString();
                 
                }
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}