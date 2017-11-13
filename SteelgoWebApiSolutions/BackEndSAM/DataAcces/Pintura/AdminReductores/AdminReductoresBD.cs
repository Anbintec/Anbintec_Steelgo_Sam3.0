using BackEndSAM.Models.Pintura.AdminReductores;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Pintura.AdminReductores
{
    public class AdminReductoresBD
    {
        private static readonly object _mutex = new object();

        private static AdminReductoresBD _instance;

        public static AdminReductoresBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new AdminReductoresBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerDetalleGrid(string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_AdminReductores_Get_Detalle_Result> lista = ctx.Sam3_Pintura_AdminReductores_Get_Detalle(lenguaje).ToList();
                    List<DetalleGrid> detalleGrid = new List<DetalleGrid>();
                    foreach (Sam3_Pintura_AdminReductores_Get_Detalle_Result item in lista)
                    {
                        DetalleGrid detalle = new DetalleGrid
                        {
                            Cantidad = item.Cantidad,
                            Reductor = item.Reductor,
                            ReductorID = item.ReductorID,
                            Lote = item.Lote,
                            RowOk = false,
                            Unidad = item.Unidad,
                            Accion = 2,
                            AdminReductoresID = item.AdminReductoresID
                        };
                        detalleGrid.Add(detalle);
                    }


                    return detalleGrid;
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

        public object ObtenerCatalogoReductores(string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Get_Reductores_Result> listaReductores = ctx.Sam3_Pintura_Get_Reductores(lenguaje).ToList();

                    List<Reductores> listaReductoresRender = new List<Reductores>();
                    if (listaReductores.Count > 0)
                        listaReductoresRender.Add(new Reductores());

                    foreach (Sam3_Pintura_Get_Reductores_Result item in listaReductores)
                    {
                        Reductores reductores = new Reductores
                        {
                            Reductor = item.Reductor,
                            ReductorID = item.ReductorID,
                            Unidad = item.Unidad
                        };
                        listaReductoresRender.Add(reductores);
                    }
                    return listaReductoresRender;
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

        public object ObtenerCatalogoReductoresAdministrados(string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Get_Reductores_Administrados_Result> listaReductores = ctx.Sam3_Pintura_Get_Reductores_Administrados(lenguaje).ToList();

                    List<Reductores> listaReductoresRender = new List<Reductores>();
                    if (listaReductores.Count > 0)
                        listaReductoresRender.Add(new Reductores());

                    foreach (Sam3_Pintura_Get_Reductores_Administrados_Result item in listaReductores)
                    {
                        Reductores reductores = new Reductores
                        {
                            Reductor = item.Reductor,
                            ReductorID = item.ReductorID,
                            Unidad = item.Unidad
                        };
                        listaReductoresRender.Add(reductores);
                    }
                    return listaReductoresRender;
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

        public object Guardar(DataTable dtDetalle, Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@Lenguaje", lenguaje } };
                    _SQL.Ejecuta(Stords.GUARDARADMINISTRACIONREDUCTORES, dtDetalle, "@TablaAdminReductores", parametro);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

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

        public object ValidarComponente(int ReductorID,string lote,int cantidad)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    ObjectResult<int?> resultSp = ctx.Sam3_Pintura_AdminReductores_ValidaAsignacion(ReductorID, lote, cantidad);
                    var valor = resultSp.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];


                    TransactionalInformation result = new TransactionalInformation();

                    if (valor > 0)
                    {
                        result.ReturnMessage.Add("Ok");
                        result.ReturnMessage.Add("Si");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }
                    else
                    {
                        result.ReturnMessage.Add("No");
                        result.ReturnMessage.Add("No");
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