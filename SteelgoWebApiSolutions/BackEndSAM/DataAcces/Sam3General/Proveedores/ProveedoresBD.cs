using BackEndSAM.Models.Sam3General.Proveedores;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Sam3General.Proveedores
{
    public class ProveedoresBD
    {
        private static readonly object _mutex = new object();
        private static ProveedoresBD _instance;

        public static ProveedoresBD Instance
        {
            get
            {
                lock(_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProveedoresBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerProveedores(int ProyectoID,int tipoProveedor)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_CG_Get_ListadoProveedores_Result> result = ctx.Sam3_Embarque_CG_Get_ListadoProveedores(ProyectoID,tipoProveedor).ToList();
                    List<DetalleProveedor> listaDetalle = new List<DetalleProveedor>();
                    listaDetalle.Add(new DetalleProveedor());

                    foreach (Sam3_Embarque_CG_Get_ListadoProveedores_Result item in result)
                    {
                        listaDetalle.Add(new DetalleProveedor {
                            ProveedorID = item.ProveedorID,
                            Nombre = item.Nombre
                        });
                    }
                    return listaDetalle;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object GuardarNuevoProveedor(string NombreProveedor, int UsuarioID, int ProyectoID, string Descripcion, string Direccion, string Telefono, int tipoProveedor)
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {

                    ObjectResult<int?> resultSp = ctx.Sam3_Embarque_CG_CreateProveedor(NombreProveedor, UsuarioID, ProyectoID, Descripcion, Direccion, Telefono,tipoProveedor);
                    var valor = resultSp.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];

                    TransactionalInformation result = new TransactionalInformation();

                    if (valor > 0)
                    {
                        result.ReturnMessage.Add("Ok");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }
                    else
                    {
                        result.ReturnMessage.Add("La plana para ese proveedor ya existe");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }
    }
}