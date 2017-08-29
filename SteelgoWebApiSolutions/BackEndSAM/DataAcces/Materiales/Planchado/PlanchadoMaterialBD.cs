using BackEndSAM.Utilities.ConvertirDataTable;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace BackEndSAM.DataAcces.Materiales.Planchado
{
    public class PlanchadoMaterialBD
    {
        private static readonly object _mutex = new object();
        private static PlanchadoMaterialBD _Instance;
        public static PlanchadoMaterialBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if(_Instance == null)
                    {
                        _Instance = new PlanchadoMaterialBD();
                    }
                }
                return _Instance;
            }
        }

        
        public object PlanchadoMateriales(DataTable Datos, int ProyectoID, int UsuarioID)
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {                    
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros =
                    {
                        { "@ProyectoID", ProyectoID.ToString() },
                        { "@UsuarioID", UsuarioID.ToString() }
                    };
                    DataTable Result = _SQL.EjecutaDataAdapter(Stords.PLANCHADOMATERIALES, Datos, "@Datos", Parametros);
                    return ToDataTable.table_to_csv(Result);
                }
            }
            catch (Exception ex)
            {                
                LoggerBd.Instance.EscribirLog(ex);                
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result; ;
            }
        }
    }
}