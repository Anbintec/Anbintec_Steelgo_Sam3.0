using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Sam3;
using DatabaseManager.EntidadesPersonalizadas;
using BackEndSAM.Utilities;
using System.Web.Script.Serialization;
using BackEndSAM.Models;
using SecurityManager.Api.Models;

namespace BackEndSAM.DataAcces
{
    public class PlanaBd
    {
        private static readonly object _mutex = new object();
        private static PlanaBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private PlanaBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static PlanaBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PlanaBd();
                    }
                }
                return _instance;
            }
        }

        public List<Plana> ObtenerListadoPlanas(Sam3_Usuario usuario, int transportistaID, int paginaID, string idioma)
        {
            List<Plana> results = new List<Plana>();
            using (SamContext ctx = new SamContext())
            {
                if ((bool)PerfilBd.Instance.VerificarPermisoCreacion(usuario.PerfilID, "Plana", paginaID))
                {
                    if (idioma == "en-US")
                    {
                        results.Add(new Plana { Nombre = "Add new", PlanaID = "0" });
                    }
                    else
                    {
                        results.Add(new Plana { Nombre = "Agregar nuevo", PlanaID = "0" });
                    }
                }

                List<Plana> encontrados = (from p in ctx.Sam3_Vehiculo
                                           join t in ctx.Sam3_TipoVehiculo on p.TipoVehiculoID equals t.TipoVehiculoID
                                           join rvt in ctx.Sam3_Rel_Vehiculo_Transportista on p.VehiculoID equals rvt.VehiculoID
                                           where p.Activo && t.Nombre == "Plana"
                                           && rvt.TransportistaID == transportistaID
                                           select new Plana
                                           {
                                               PlanaID = p.VehiculoID.ToString(),
                                               Nombre = p.Placas
                                           }).AsParallel().ToList();

                results.AddRange(encontrados);

            }
            return results;
        }

        public object InsertarPlana(VehiculoJson plana, Sam3_Usuario usuario)
        {
            try
            {
                Sam3_Vehiculo nuevaPlana;
                using (SamContext ctx = new SamContext())
                {
                    if (!ctx.Sam3_Vehiculo.Where(c => c.Placas == plana.Placas && c.TipoVehiculoID == 2 && c.Activo).AsParallel().Any())
                    {
                        nuevaPlana = new Sam3_Vehiculo
                        {
                            TipoVehiculoID = Convert.ToInt32(plana.TipoVehiculoID),
                            Activo = true,
                            TractoID = Convert.ToInt32(plana.TractoID),
                            Placas = plana.Placas,
                            Unidad = plana.Unidad,
                            Modelo = plana.Modelo,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = usuario.UsuarioID
                        };
                        ctx.Sam3_Vehiculo.Add(nuevaPlana);
                        ctx.SaveChanges();


                        Sam3_Rel_Vehiculo_Chofer nuevoRegistroChofer = new Sam3_Rel_Vehiculo_Chofer();
                        nuevoRegistroChofer.VehiculoID = nuevaPlana.VehiculoID;
                        nuevoRegistroChofer.Activo = true;
                        nuevoRegistroChofer.ChoferID = Convert.ToInt32(plana.ChoferID);
                        nuevoRegistroChofer.FechaModificacion = DateTime.Now;
                        nuevoRegistroChofer.UsuarioModificacion = usuario.UsuarioID;

                        ctx.Sam3_Rel_Vehiculo_Chofer.Add(nuevoRegistroChofer);

                        Sam3_Rel_Vehiculo_Transportista transportista = new Sam3_Rel_Vehiculo_Transportista();
                        transportista.Activo = true;
                        transportista.FechaModificacion = DateTime.Now;
                        transportista.TransportistaID = Convert.ToInt32(plana.TransportistaID);
                        transportista.VehiculoID = nuevaPlana.VehiculoID;
                        transportista.UsuarioModificacion = usuario.UsuarioID;

                        ctx.Sam3_Rel_Vehiculo_Transportista.Add(transportista);

                        ctx.SaveChanges();

                        return new CatalogoPlana
                        {
                            VehiculoID = nuevaPlana.VehiculoID.ToString(),
                            Placas = nuevaPlana.Placas,
                            Unidad = nuevaPlana.Unidad,
                            Modelo = nuevaPlana.Modelo,
                            choferNombre = plana.choferNombre,
                            choferID = plana.ChoferID.ToString(),
                            transportistaNombre = plana.transportistaNombre,
                            transportistaID = plana.TransportistaID.ToString(),
                            relVehiculoChoferID = nuevoRegistroChofer.Rel_Vehiculo_Chofer_ID.ToString(),
                            relVehiculoTransportistaID = transportista.Rel_Vehiculo_Transportista_ID.ToString()
                        };
                        //return new Plana { Nombre = nuevaPlana.Placas, PlanaID = Convert.ToString(nuevaPlana.VehiculoID) };
                    }
                    else
                    {
                        throw new Exception("Plana existente");
                    }
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ActualizarPlana(VehiculoJson cambios, Sam3_Usuario usuario)
        {
            try
            {
                TransactionalInformation result;
                using (SamContext ctx = new SamContext())
                {
                    if (!ctx.Sam3_Vehiculo.Where(c => c.Placas == cambios.Placas && c.TipoVehiculoID == 2 && c.Activo).AsParallel().Any())
                    {
                        int vehiculoID = Convert.ToInt32(cambios.VehiculoID);
                        Sam3_Vehiculo planaEnBd = ctx.Sam3_Vehiculo.Where(x => x.VehiculoID == vehiculoID).AsParallel().SingleOrDefault();
                        planaEnBd.Activo = true;
                        planaEnBd.TractoID = Convert.ToInt32(cambios.TractoID);
                        planaEnBd.Placas = cambios.Placas;
                        planaEnBd.Unidad = cambios.Unidad;
                        planaEnBd.Modelo = cambios.Modelo;
                        planaEnBd.UsuarioModificacion = usuario.UsuarioID;
                        planaEnBd.FechaModificacion = DateTime.Now;

                        ctx.SaveChanges();

                        result = new TransactionalInformation();
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.ReturnMessage.Add("OK");
                        result.IsAuthenicated = true;

                        return result;
                    }
                    else
                    {
                        throw new Exception("Plana existente");
                    }
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object EliminarPlana(int vehiculoID, Sam3_Usuario usuario)
        {
            try
            {
                TransactionalInformation result;
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Vehiculo plana = ctx.Sam3_Vehiculo.Where(x => x.VehiculoID == vehiculoID).AsParallel().SingleOrDefault();
                    plana.Activo = false;
                    plana.UsuarioModificacion = usuario.UsuarioID;
                    plana.FechaModificacion = DateTime.Now;

                    ctx.SaveChanges();

                    result = new TransactionalInformation();
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.ReturnMessage.Add("OK");
                    result.IsAuthenicated = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
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