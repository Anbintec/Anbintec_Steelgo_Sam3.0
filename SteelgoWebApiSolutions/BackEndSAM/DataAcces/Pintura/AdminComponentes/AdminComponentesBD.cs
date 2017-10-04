using BackEndSAM.Models.Pintura.AdminComponentes;
using BackEndSAM.Models.Pintura.SistemaPintura;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Pintura.AdminComponentes
{
    public class AdminComponentesBD
    {

        private static readonly object _mutex = new object();

        private static AdminComponentesBD _instance;

        public static AdminComponentesBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new AdminComponentesBD();
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
                    List<Sam3_Pintura_AdminComponentes_Get_Detalle_Result> lista = ctx.Sam3_Pintura_AdminComponentes_Get_Detalle(lenguaje).ToList();
                    List<DetalleGrid> detalleGrid = new List<DetalleGrid>();
                    foreach (Sam3_Pintura_AdminComponentes_Get_Detalle_Result item in lista)
                    {
                        DetalleGrid detalle = new DetalleGrid
                        {
                            Cantidad = item.Cantidad,
                            Componente = item.Componente,
                            ComponenteID = item.ComponenteID,
                            Lote = item.Lote,
                            RowOk = false,
                            Unidad = item.Unidad,
                            Accion = 2,
                            AdminComponentesID = item.AdminComponentesID
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

        public object ObtenerCatalogoComponentes(string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Get_Componentes_Result> listaComponentes = ctx.Sam3_Pintura_Get_Componentes(lenguaje).ToList();

                    List<Componentes> listaComponentesRender = new List<Componentes>();
                    if (listaComponentes.Count > 0)
                        listaComponentesRender.Add(new Componentes());

                    foreach (Sam3_Pintura_Get_Componentes_Result item in listaComponentes)
                    {
                        Componentes componentes = new Componentes
                        {
                            Componente = item.Componente,
                            ComponenteID = item.ComponenteID,
                            Unidad = item.Unidad
                        };
                        listaComponentesRender.Add(componentes);
                    }
                    return listaComponentesRender;
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

        public object ObtenerCatalogoComponentesAdministrados(string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Get_ComponentesAdministrados_Result> listaComponentes = ctx.Sam3_Pintura_Get_ComponentesAdministrados(lenguaje).ToList();

                    List<Componentes> listaComponentesRender = new List<Componentes>();
                    if (listaComponentes.Count > 0)
                        listaComponentesRender.Add(new Componentes());

                    foreach (Sam3_Pintura_Get_ComponentesAdministrados_Result item in listaComponentes)
                    {
                        Componentes componentes = new Componentes
                        {
                            Componente = item.Componente,
                            ComponenteID = item.ComponenteID,
                            Unidad = item.Unidad
                        };
                        listaComponentesRender.Add(componentes);
                    }
                    return listaComponentesRender;
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
                    _SQL.Ejecuta(Stords.GUARDARADMINISTRACIONCOMPONENTES, dtDetalle, "@TablaAdminComponentes", parametro);

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

        public object ObtenerCatalogoComponentesAgregados(int SistemaPinturaProyectoProcesoID, string lenguaje, List<Componentes> listadoComponentes)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_Pintura_Get_ComponentesAgregados_Result> listaComponentes = ctx.Sam3_Pintura_Get_ComponentesAgregados(lenguaje, SistemaPinturaProyectoProcesoID).ToList();

                    List<ComponenteAgregado> listaComponentesAgregados = new List<ComponenteAgregado>();
                    int numeroComponente = 0;

                    foreach (Sam3_Pintura_Get_ComponentesAgregados_Result item in listaComponentes)
                    {
                        ComponenteAgregado componentesAgregados = new ComponenteAgregado
                        {
                            ProyectoProcesoComponenteID = item.ProyectoProcesoComponenteID,
                            Accion = 2,
                            ComponenteAgregadoID = numeroComponente + 1,
                            ComponenteID = item.ComponenteID,
                            Nombre = item.Nombre,
                            ListadoComponentes = listadoComponentes
                        };
                        numeroComponente++;
                        listaComponentesAgregados.Add(componentesAgregados);
                    };

                    return listaComponentesAgregados;
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

        public object ValidarComponente(int ComponenteID, string Lote, int Cantidad)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    ObjectResult<int?> resultSp = ctx.Sam3_Pintura_AdminComponentes_ValidaAsignacion(ComponenteID, Lote, Cantidad);
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