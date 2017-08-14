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
    public class TractoBd
    {
        private static readonly object _mutex = new object();
        private static TractoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private TractoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static TractoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TractoBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoTractos(string esAvisoEntrada, int transportistaID, Sam3_Usuario usuario, int paginaID, string idioma)
        {
            try
            {
                List<TractoAV> lstCamiones = new List<TractoAV>();

                if (int.Parse(esAvisoEntrada) == 1 && (bool)PerfilBd.Instance.VerificarPermisoCreacion(usuario.PerfilID, "Tracto", paginaID))
                {
                    if (idioma == "en-US")
                    {
                        lstCamiones.Add(new TractoAV { VehiculoID = "0", Placas = "Add new" });
                    }
                    else
                    {
                        lstCamiones.Add(new TractoAV { VehiculoID = "0", Placas = "Agregar nuevo" });
                    }
                }

                using (SamContext ctx = new SamContext())
                {
                    List<TractoAV> result = (from r in ctx.Sam3_Vehiculo
                                             join rvt in ctx.Sam3_Rel_Vehiculo_Transportista on r.VehiculoID equals rvt.VehiculoID
                                             where r.Activo && r.TipoVehiculoID == 1
                                             && rvt.TransportistaID == transportistaID
                                             select new TractoAV
                                             {
                                                VehiculoID = r.VehiculoID.ToString(),
                                                Placas = r.Placas
                                             }).AsParallel().ToList();

                    lstCamiones.AddRange(result);

                    return lstCamiones;
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

        public object InsertarTracto(VehiculoJson cambios, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (!ctx.Sam3_Vehiculo.Where(c => c.Placas == cambios.Placas && c.TipoVehiculoID == 1 && c.Activo).AsParallel().Any())
                    {
                        Sam3_Vehiculo nuevoCamion = new Sam3_Vehiculo();
                        nuevoCamion.Activo = true;
                        nuevoCamion.FechaModificacion = DateTime.Now;
                        nuevoCamion.Placas = cambios.Placas;
                        nuevoCamion.PolizaSeguro = cambios.PolizaSeguro;
                        nuevoCamion.TarjetaCirculacion = cambios.TarjetaCirculacion;
                        nuevoCamion.UsuarioModificacion = usuario.UsuarioID;
                        nuevoCamion.TipoVehiculoID = Convert.ToInt32(cambios.TipoVehiculoID);

                        ctx.Sam3_Vehiculo.Add(nuevoCamion);
                        ctx.SaveChanges();

                        Sam3_Rel_Vehiculo_Chofer nuevoRegistroChofer = new Sam3_Rel_Vehiculo_Chofer();
                        nuevoRegistroChofer.VehiculoID = nuevoCamion.VehiculoID;
                        nuevoRegistroChofer.Activo = true;
                        nuevoRegistroChofer.ChoferID = Convert.ToInt32(cambios.ChoferID);
                        nuevoRegistroChofer.FechaModificacion = DateTime.Now;
                        nuevoRegistroChofer.UsuarioModificacion = usuario.UsuarioID;

                        ctx.Sam3_Rel_Vehiculo_Chofer.Add(nuevoRegistroChofer);

                        Sam3_Rel_Vehiculo_Transportista transportista = new Sam3_Rel_Vehiculo_Transportista();
                        transportista.Activo = true;
                        transportista.FechaModificacion = DateTime.Now;
                        transportista.TransportistaID = Convert.ToInt32(cambios.TransportistaID);
                        transportista.VehiculoID = nuevoCamion.VehiculoID;
                        transportista.UsuarioModificacion = usuario.UsuarioID;

                        ctx.Sam3_Rel_Vehiculo_Transportista.Add(transportista);

                        ctx.SaveChanges();

                        return new CatalogoTracto
                        {
                            VehiculoID = nuevoCamion.VehiculoID.ToString(),
                            Placas = nuevoCamion.Placas,
                            TarjetaCirculacion = nuevoCamion.TarjetaCirculacion,
                            PolizaSeguro = nuevoCamion.PolizaSeguro,
                            choferNombre = cambios.choferNombre,
                            choferID = cambios.ChoferID.ToString(),
                            transportistaNombre = cambios.transportistaNombre,
                            transportistaID = cambios.TransportistaID.ToString(),
                            relVehiculoChoferID = nuevoRegistroChofer.Rel_Vehiculo_Chofer_ID.ToString(),
                            relVehiculoTransportistaID = transportista.Rel_Vehiculo_Transportista_ID.ToString()
                        };
                        //return new Camion { Placas = nuevoCamion.Placas, CamionID = nuevoCamion.VehiculoID.ToString() };
                    }
                    else
                    {
                        throw new Exception("Tracto existente");
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

        public object ActualizarTracto(VehiculoJson cambios, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    return null;
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

        public object EliminarTracto(int vehiculoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Vehiculo camion = ctx.Sam3_Vehiculo.Where(x => x.VehiculoID == vehiculoID).AsParallel().SingleOrDefault();
                    camion.Activo = false;
                    camion.FechaModificacion = DateTime.Now;
                    camion.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
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

    }//Fin clase
}