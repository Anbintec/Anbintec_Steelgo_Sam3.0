using BackEndSAM.Models;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace BackEndSAM.DataAcces
{
    public class CatalogosBd
    {
        private static readonly object _mutex = new object();
        private static CatalogosBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private CatalogosBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static CatalogosBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CatalogosBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Funcion con llenar el combo de catalogos
        /// Catalogos ABC
        /// </summary>
        /// <returns></returns>
        public object obtenerCatalogos()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ListaCombos> valoresCombo = new List<ListaCombos>();

                    ListaCombos patios = new ListaCombos();
                    ListaCombos chofer = new ListaCombos();
                    ListaCombos tipoAviso = new ListaCombos();
                    ListaCombos transportista = new ListaCombos();
                    ListaCombos tracto = new ListaCombos();
                    ListaCombos plana = new ListaCombos();
                    ListaCombos proveedor = new ListaCombos();
                    ListaCombos tipoUso = new ListaCombos();
                    ListaCombos camion = new ListaCombos();
                    ListaCombos Acero = new ListaCombos();
                    ListaCombos coladas = new ListaCombos();
                    ListaCombos familiaMaterial = new ListaCombos();
                    ListaCombos familiaAcero = new ListaCombos();
                    ListaCombos fabricante = new ListaCombos();
                    ListaCombos Cedulas = new ListaCombos();
                    ListaCombos MTR = new ListaCombos();

                    patios.id = "1";
                    patios.value = "Patios";
                    valoresCombo.Add(patios);

                    chofer.id = "2";
                    chofer.value = "Chofer";
                    valoresCombo.Add(chofer);

                    tipoAviso.id = "3";
                    tipoAviso.value = "Tipo Aviso";
                    valoresCombo.Add(tipoAviso);

                    transportista.id = "4";
                    transportista.value = "Transportista";
                    valoresCombo.Add(transportista);

                    tracto.id = "5";
                    tracto.value = "Tracto";
                    valoresCombo.Add(tracto);

                    plana.id = "6";
                    plana.value = "Plana";
                    valoresCombo.Add(plana);

                    proveedor.id = "7";
                    proveedor.value = "Proveedor";
                    valoresCombo.Add(proveedor);

                    tipoUso.id = "8";
                    tipoUso.value = "Tipo de Uso";
                    valoresCombo.Add(tipoUso);

                    camion.id = "9";
                    camion.value = "Camion";
                    valoresCombo.Add(camion);

                    fabricante.id = "10";
                    fabricante.value = "Fabricante";
                    valoresCombo.Add(fabricante);

                    Cedulas.id = "11";
                    Cedulas.value = "Cédulas";
                    valoresCombo.Add(Cedulas);

                    MTR.id = "12";
                    MTR.value = "MTR";
                    valoresCombo.Add(MTR);

                    valoresCombo = valoresCombo.OrderBy(x => x.value).ToList();

                    return valoresCombo;
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

        /// <summary>
        /// Muestra la informacion del catalogo seleccionado
        /// </summary>
        /// <param name="catalogoID"></param>
        /// <returns></returns>
        public object obtenerInformacionCatalogos(string catalogoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    switch (Convert.ToInt32(catalogoID))
                    {
                        case 1: //Patios
                            #region
                            List<CatalogoPatio> catPatio = new List<CatalogoPatio>();
                            catPatio = (from p in ctx.Sam3_Patio
                                        where p.Activo
                                        select new CatalogoPatio
                                        {
                                            PatioID = p.PatioID.ToString(),
                                            Nombre = p.Nombre,
                                            Propietario = p.Propietario,
                                            Descripcion = p.Descripcion,
                                            RequierePermisoAduana = p.RequierePermisoAduana == true ? "Si" : "No"
                                        }).AsParallel().ToList();

                            return catPatio;
                            #endregion
                        case 2: //Chofer
                            #region
                            List<CatalogoChofer> catChofer = new List<CatalogoChofer>();
                            catChofer = (from ch in ctx.Sam3_Chofer
                                         join t in ctx.Sam3_Transportista on ch.TransportistaID equals t.TransportistaID
                                         where ch.Activo && t.Activo
                                         select new CatalogoChofer
                                         {
                                             ChoferID = ch.ChoferID.ToString(),
                                             Nombre = ch.Nombre,
                                             TransportistaID = t.TransportistaID.ToString(),
                                             TransportistaNombre = t.Nombre
                                         }).AsParallel().ToList();

                            return catChofer;
                            #endregion
                        case 3: //Tipo Aviso
                            #region
                            List<Catalogos> catTipoAviso = new List<Catalogos>();
                            catTipoAviso = (from ta in ctx.Sam3_TipoAviso
                                            where ta.Activo
                                            select new Catalogos
                                            {
                                                Id = ta.TipoAvisoID.ToString(),
                                                Nombre = ta.Nombre
                                            }).AsParallel().ToList();

                            return catTipoAviso;
                            #endregion
                        case 4: //Transportista
                            #region
                            List<CatalogoTransportista> catTransportista = new List<CatalogoTransportista>();
                            catTransportista = (from t in ctx.Sam3_Transportista
                                                join c in ctx.Sam3_Contacto on t.ContactoID equals c.ContactoID
                                                where t.Activo && c.Activo
                                                select new CatalogoTransportista
                                                {
                                                    ContactoID = c.ContactoID.ToString(),
                                                    Contacto = c.Nombre,
                                                    TransportistaID = t.TransportistaID.ToString(),
                                                    Nombre = t.Nombre,
                                                    Descripcion = t.Descripcion,
                                                    Direccion = t.Direccion,
                                                    Telefono = t.Telefono
                                                }).AsParallel().ToList();

                            return catTransportista;
                            #endregion
                        case 5: //Tracto
                            #region
                            List<CatalogoTracto> catTracto = new List<CatalogoTracto>();
                            catTracto = (from t in ctx.Sam3_Vehiculo
                                         join rvch in ctx.Sam3_Rel_Vehiculo_Chofer on t.VehiculoID equals rvch.VehiculoID
                                         join ch in ctx.Sam3_Chofer on rvch.ChoferID equals ch.ChoferID
                                         join rvt in ctx.Sam3_Rel_Vehiculo_Transportista on t.VehiculoID equals rvt.VehiculoID
                                         join tr in ctx.Sam3_Transportista on rvt.TransportistaID equals tr.TransportistaID
                                         where t.Activo && rvch.Activo && rvt.Activo && ch.Activo && tr.Activo
                                         && t.TipoVehiculoID == 1
                                         select new CatalogoTracto
                                         {
                                             VehiculoID = t.VehiculoID.ToString(),
                                             Placas = t.Placas,
                                             TarjetaCirculacion = t.TarjetaCirculacion,
                                             PolizaSeguro = t.PolizaSeguro,
                                             choferID = rvch.ChoferID.ToString(),
                                             choferNombre = ch.Nombre,
                                             transportistaID = rvt.TransportistaID.ToString(),
                                             transportistaNombre = tr.Nombre,
                                             relVehiculoChoferID = rvch.Rel_Vehiculo_Chofer_ID.ToString(),
                                             relVehiculoTransportistaID = rvt.Rel_Vehiculo_Transportista_ID.ToString()
                                         }).AsParallel().ToList();

                            return catTracto;
                            #endregion
                        case 6: //Plana
                            #region
                            List<CatalogoPlana> catPlana = new List<CatalogoPlana>();
                            catPlana = (from v in ctx.Sam3_Vehiculo
                                        join rvch in ctx.Sam3_Rel_Vehiculo_Chofer on v.VehiculoID equals rvch.VehiculoID
                                        join ch in ctx.Sam3_Chofer on rvch.ChoferID equals ch.ChoferID
                                        join rvt in ctx.Sam3_Rel_Vehiculo_Transportista on v.VehiculoID equals rvt.VehiculoID
                                        join tr in ctx.Sam3_Transportista on rvt.TransportistaID equals tr.TransportistaID
                                        where v.Activo && rvch.Activo && rvt.Activo && ch.Activo && tr.Activo
                                        && v.TipoVehiculoID == 2
                                        select new CatalogoPlana
                                        {
                                            VehiculoID = v.VehiculoID.ToString(),
                                            Placas = v.Placas,
                                            Unidad = v.Unidad,
                                            Modelo = v.Modelo,
                                            TractoID = v.TractoID == -1 ? "" : v.TractoID.ToString(),
                                            choferID = rvch.ChoferID.ToString(),
                                            choferNombre = ch.Nombre,
                                            transportistaID = rvt.TransportistaID.ToString(),
                                            transportistaNombre = tr.Nombre,
                                            relVehiculoChoferID = rvch.Rel_Vehiculo_Chofer_ID.ToString(),
                                            relVehiculoTransportistaID = rvt.Rel_Vehiculo_Transportista_ID.ToString()
                                        }).AsParallel().ToList();

                            return catPlana;
                            #endregion
                        case 7: //Proveedor
                            #region
                            List<CatalogoProveedor> catProveedor = new List<CatalogoProveedor>();
                            catProveedor = (from p in ctx.Sam3_Proveedor
                                            join c in ctx.Sam3_Contacto on p.ContactoID equals c.ContactoID
                                            where p.Activo && c.Activo
                                            select new CatalogoProveedor
                                            {
                                                ProveedorID = p.ProveedorID.ToString(),
                                                ContactoID = c.ContactoID.ToString(),
                                                Contacto = c.Nombre,
                                                Nombre = p.Nombre,
                                                Descripcion = p.Descripcion,
                                                Direccion = p.Direccion,
                                                Telefono = p.Telefono
                                            }).AsParallel().ToList();

                            return catProveedor;
                            #endregion
                        case 8: //Tipo de Uso
                            #region
                            List<Catalogos> catTipoUso = new List<Catalogos>();
                            catTipoUso = (from tu in ctx.Sam3_TipoUso
                                          where tu.Activo
                                          select new Catalogos
                                          {
                                              Id = tu.TipoUsoID.ToString(),
                                              Nombre = tu.Nombre
                                          }).AsParallel().ToList();

                            return catTipoUso;
                            #endregion
                        case 9: //Camion
                            #region
                            List<Catalogos> catCamion = new List<Catalogos>();
                            catCamion = (from v in ctx.Sam3_TipoVehiculo

                                         where v.Activo
                                         select new Catalogos
                                         {
                                             Id = v.TipoVehiculoID.ToString(),
                                             Nombre = v.Nombre
                                         }).AsParallel().ToList();

                            return catCamion;
                            #endregion
                        case 10: //fabricante
                            #region
                            List<CatalogoFabricante> catFabricante = new List<CatalogoFabricante>();
                            catFabricante = (from f in ctx.Sam3_Fabricante
                                             join c in ctx.Sam3_Contacto on f.ContactoID equals c.ContactoID
                                             where f.Activo && c.Activo
                                             select new CatalogoFabricante
                                             {
                                                 FabricanteID = f.FabricanteID.ToString(),
                                                 ContactoID = c.ContactoID.ToString(),
                                                 Contacto = c.Nombre,
                                                 Nombre = f.Nombre,
                                                 Descripcion = f.Descripcion,
                                                 Direccion = f.Direccion,
                                                 Telefono = f.Telefono
                                             }).AsParallel().ToList();
                            return catFabricante;
                            #endregion
                        case 11: //Catalogo Cedulas
                            #region
                            List<CatalogoCedulas> catCedulas = new List<CatalogoCedulas>();

                            catCedulas = (from cat in ctx.Sam3_CatalogoCedulas
                                          where cat.Activo
                                          select new CatalogoCedulas
                                          {
                                              CedulaID = cat.CatalogoCedulasID.ToString(),
                                              Diametro1 = String.IsNullOrEmpty((from di in ctx.Sam3_Diametro where di.Activo && di.DiametroID == cat.DiametroID select di.Valor.ToString()).FirstOrDefault()) ?
                                              null : (from di in ctx.Sam3_Diametro where di.Activo && di.DiametroID == cat.DiametroID select di.Valor.ToString()).FirstOrDefault(),
                                              CedulaA = (from ced in ctx.Sam3_Cedula where ced.Activo && cat.CedulaA == ced.CedulaID select ced.Codigo).FirstOrDefault(),
                                              CedulaB = (from ced in ctx.Sam3_Cedula where ced.Activo && cat.CedulaB == ced.CedulaID select ced.Codigo).FirstOrDefault(),
                                              CedulaC = (from ced in ctx.Sam3_Cedula where ced.Activo && cat.CedulaC == ced.CedulaID select ced.Codigo).FirstOrDefault(),
                                              CedulaIn = cat.EspesorIn.ToString(),
                                              CedulaMM = cat.EspesorMM.ToString(),
                                              Correcta = true,
                                              CatalogoCedulaID = cat.CatalogoCedulasID
                                          }).AsParallel().ToList();
                            catCedulas = catCedulas.OrderBy(x => x.Diametro1).ToList();
                            return catCedulas;
                            #endregion
                        case 12: //MTR
                            #region
                            List<CatalogoMTR> catalogoMTR = new List<CatalogoMTR>();
                            catalogoMTR = (from mtr in ctx.Sam3_MTR
                                           join ic in ctx.Sam3_ItemCode on mtr.ItemCodeID equals ic.ItemCodeID
                                           join rid in ctx.Sam3_Rel_ItemCode_Diametro on mtr.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                           join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                           join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                           join col in ctx.Sam3_Colada on mtr.ColadaID equals col.ColadaID
                                           where mtr.Activo
                                           select new CatalogoMTR
                                           {
                                               MTRID = mtr.MTRID,
                                               Proyecto = ic.Sam3_Proyecto.Nombre,
                                               ItemCodeID = rid.Rel_ItemCode_Diametro_ID.ToString(),
                                               ItemCode = ic.Codigo,
                                               ColadaID = col.ColadaID.ToString(),
                                               Colada = col.NumeroColada,
                                               NumeroMTR = mtr.NumeroMTR,
                                               CantidadPiezas = mtr.CantidadPiezas.ToString(),
                                               D1 = d1.Valor.ToString(), 
                                               D2 = d2.Valor.ToString()
                                           }).AsParallel().Distinct().ToList();


                            return catalogoMTR;
                            #endregion
                        default:
                            #region
                            TransactionalInformation result = new TransactionalInformation();
                            result.ReturnMessage.Add("Listado no encontrado");
                            result.ReturnCode = 500;
                            result.ReturnStatus = false;
                            result.IsAuthenicated = false;
                            return result;
                            #endregion
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


        /// <summary>
        /// Funcion para actualizar la informacion de los catalogos
        /// Segun la informacion capturada en el grid de Administracion de Catalogos
        /// </summary>
        /// <param name="data">datos a actualizar</param>
        /// <param name="catalogoID">id del catalogo seleccionado</param>
        /// <param name="usuario">usuario actua</param>
        /// <returns></returns>
        public object actualizarCatalogo(string data, string catalogoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    object res = new object();
                    TransactionalInformation result = new TransactionalInformation();

                    switch (Convert.ToInt32(catalogoID))
                    {
                        case 1: //Patios
                            #region
                            Sam3_Patio patio = serializer.Deserialize<Sam3_Patio>(data);
                            if (ctx.Sam3_Patio.Where(x => x.PatioID == patio.PatioID && x.Activo).AsParallel().Any())
                            {
                                Sam3_Patio patioEnBd = ctx.Sam3_Patio.Where(x => x.PatioID == patio.PatioID && x.Activo).AsParallel().SingleOrDefault();

                                patioEnBd.Descripcion = patio.Descripcion != null && patio.Descripcion != patioEnBd.Descripcion ?
                            patio.Descripcion : patioEnBd.Descripcion;
                                patioEnBd.Nombre = patio.Nombre != null && patio.Nombre != patioEnBd.Nombre ?
                            patio.Nombre : patioEnBd.Nombre;
                                patioEnBd.Propietario = patio.Propietario != null && patio.Propietario != patioEnBd.Propietario ?
                            patio.Propietario : patioEnBd.Propietario;
                                patioEnBd.RequierePermisoAduana = patio.RequierePermisoAduana != null && patio.RequierePermisoAduana != patioEnBd.RequierePermisoAduana ?
                            patio.RequierePermisoAduana : patioEnBd.RequierePermisoAduana;

                                patioEnBd.UsuarioModificacion = usuario.UsuarioID;
                                patioEnBd.FechaModificacion = DateTime.Now;

                                ctx.SaveChanges();
                                return new CatalogoPatio
                                {
                                    PatioID = patioEnBd.PatioID.ToString(),
                                    Nombre = patioEnBd.Nombre,
                                    Descripcion = patioEnBd.Descripcion,
                                    Propietario = patioEnBd.Propietario,
                                    RequierePermisoAduana = patioEnBd.RequierePermisoAduana == true ? "Si" : "No"
                                };
                            }
                            else
                            {
                                throw new Exception("Patio no existente");
                            }
                            #endregion
                        case 2: //chofer
                            #region
                            CatalogoChofer chofer = serializer.Deserialize<CatalogoChofer>(data);
                            if (ctx.Sam3_Chofer.Where(c => c.ChoferID.ToString() == chofer.ChoferID && c.Activo).AsParallel().Any())
                            {
                                Sam3_Chofer choferEnBd = ctx.Sam3_Chofer.Where(x => x.ChoferID.ToString() == chofer.ChoferID && x.Activo).AsParallel().SingleOrDefault();

                                choferEnBd.Nombre = chofer.Nombre != null && chofer.Nombre != choferEnBd.Nombre ?
                                    chofer.Nombre : choferEnBd.Nombre;

                                choferEnBd.TransportistaID = chofer.TransportistaID != null && chofer.TransportistaID != choferEnBd.TransportistaID.ToString() ?
                                    Convert.ToInt32(chofer.TransportistaID) : choferEnBd.TransportistaID;

                                choferEnBd.UsuarioModificacion = usuario.UsuarioID;

                                choferEnBd.FechaModificacion = DateTime.Now;

                                ctx.SaveChanges();

                                return new CatalogoChofer
                                {
                                    ChoferID = choferEnBd.ChoferID.ToString(),
                                    Nombre = choferEnBd.Nombre,
                                    TransportistaID = choferEnBd.TransportistaID.ToString(),
                                    TransportistaNombre = chofer.TransportistaNombre
                                };
                            }
                            else
                            {
                                throw new Exception("Chofer no existente");
                            }
                            #endregion
                        case 3: //Tipo Aviso
                            #region
                            Catalogos tipoAviso = serializer.Deserialize<Catalogos>(data);

                            if (ctx.Sam3_TipoAviso.Where(x => x.Activo && x.TipoAvisoID.ToString() == tipoAviso.Id).AsParallel().Any())
                            {
                                Sam3_TipoAviso avisoEnBd = ctx.Sam3_TipoAviso.Where(x => x.TipoAvisoID.ToString() == tipoAviso.Id && x.Activo).AsParallel().SingleOrDefault();

                                avisoEnBd.Nombre = tipoAviso.Nombre != null && tipoAviso.Nombre != avisoEnBd.Nombre ?
                                    tipoAviso.Nombre : avisoEnBd.Nombre;

                                avisoEnBd.FechaModificacion = DateTime.Now;

                                avisoEnBd.UsuarioModificacion = usuario.UsuarioID;

                                ctx.SaveChanges();

                                return new Catalogos
                                {
                                    Id = avisoEnBd.TipoAvisoID.ToString(),
                                    Nombre = avisoEnBd.Nombre
                                };
                            }
                            else
                            {
                                throw new Exception("Tipo Aviso no existente");
                            }
                            #endregion
                        case 4: //Transportista
                            #region
                            CatalogoTransportista transportista = serializer.Deserialize<CatalogoTransportista>(data);
                            if (ctx.Sam3_Transportista.Where(x => x.TransportistaID.ToString() == transportista.TransportistaID && x.Activo).AsParallel().Any())
                            {
                                Sam3_Transportista transEnBd = ctx.Sam3_Transportista.Where(x => x.TransportistaID.ToString() == transportista.TransportistaID && x.Activo).AsParallel().SingleOrDefault();
                                transEnBd.ContactoID = transportista.ContactoID != null && transportista.ContactoID != transEnBd.ContactoID.ToString() ?
                                    Convert.ToInt32(transportista.ContactoID) : transEnBd.ContactoID;

                                transEnBd.Nombre = transportista.Nombre != null && transportista.Nombre != transEnBd.Nombre ?
                                    transportista.Nombre : transEnBd.Nombre;

                                transEnBd.Descripcion = transportista.Descripcion != null && transportista.Descripcion != transEnBd.Descripcion ?
                                    transportista.Descripcion : transEnBd.Descripcion;

                                transEnBd.Direccion = transportista.Direccion != null && transportista.Direccion != transEnBd.Direccion ?
                                    transportista.Direccion : transEnBd.Direccion;

                                transEnBd.Telefono = transportista.Telefono != null && transportista.Telefono != transEnBd.Telefono ?
                                    transportista.Telefono : transEnBd.Telefono;

                                transEnBd.UsuarioModificacion = usuario.UsuarioID;

                                transEnBd.FechaModificacion = DateTime.Now;

                                ctx.SaveChanges();

                                return new CatalogoTransportista
                                {
                                    TransportistaID = transEnBd.TransportistaID.ToString(),
                                    ContactoID = transEnBd.ContactoID.ToString(),
                                    Contacto = transportista.Contacto,
                                    Descripcion = transEnBd.Descripcion,
                                    Direccion = transEnBd.Direccion,
                                    Nombre = transEnBd.Nombre,
                                    Telefono = transEnBd.Telefono
                                };
                            }
                            else
                            {
                                throw new Exception("Transportista no existente");
                            }
                            #endregion
                        case 5: //Tracto
                            #region
                            CatalogoTracto vehiculo = serializer.Deserialize<CatalogoTracto>(data);
                            if (ctx.Sam3_Vehiculo.Where(x => x.VehiculoID.ToString() == vehiculo.VehiculoID && x.Activo && x.TipoVehiculoID == 1).AsParallel().Any())
                            {
                                Sam3_Vehiculo vehiculoEnBd = ctx.Sam3_Vehiculo.Where(x => x.VehiculoID.ToString() == vehiculo.VehiculoID && x.Activo).AsParallel().SingleOrDefault();

                                vehiculoEnBd.Placas = vehiculo.Placas != null && vehiculo.Placas != vehiculoEnBd.Placas ?
                                    vehiculo.Placas : vehiculoEnBd.Placas;

                                vehiculoEnBd.TarjetaCirculacion = vehiculo.TarjetaCirculacion != null && vehiculo.TarjetaCirculacion != vehiculoEnBd.TarjetaCirculacion ?
                                    vehiculo.TarjetaCirculacion : vehiculoEnBd.TarjetaCirculacion;

                                vehiculoEnBd.PolizaSeguro = vehiculo.PolizaSeguro != null && vehiculo.PolizaSeguro != vehiculoEnBd.PolizaSeguro ?
                                    vehiculo.PolizaSeguro : vehiculoEnBd.PolizaSeguro;

                                vehiculoEnBd.UsuarioModificacion = usuario.UsuarioID;

                                vehiculoEnBd.FechaModificacion = DateTime.Now;

                                ctx.SaveChanges();

                                Sam3_Rel_Vehiculo_Chofer relVehiculoChofer = ctx.Sam3_Rel_Vehiculo_Chofer
                                    .Where(x => x.Rel_Vehiculo_Chofer_ID.ToString() == vehiculo.relVehiculoChoferID && x.Activo).AsParallel().SingleOrDefault();

                                relVehiculoChofer.ChoferID = Convert.ToInt32(vehiculo.choferID);
                                relVehiculoChofer.Activo = true;
                                relVehiculoChofer.UsuarioModificacion = usuario.UsuarioID;
                                relVehiculoChofer.FechaModificacion = DateTime.Now;
                                ctx.SaveChanges();


                                Sam3_Rel_Vehiculo_Transportista relVehiculotransportista = ctx.Sam3_Rel_Vehiculo_Transportista
                                    .Where(x => x.Rel_Vehiculo_Transportista_ID.ToString() == vehiculo.relVehiculoTransportistaID && x.Activo).AsParallel().SingleOrDefault();

                                relVehiculotransportista.TransportistaID = Convert.ToInt32(vehiculo.transportistaID);
                                relVehiculotransportista.Activo = true;
                                relVehiculotransportista.UsuarioModificacion = usuario.UsuarioID;
                                relVehiculotransportista.FechaModificacion = DateTime.Now;
                                ctx.SaveChanges();

                                return new CatalogoTracto
                                {
                                    VehiculoID = vehiculoEnBd.VehiculoID.ToString(),
                                    Placas = vehiculoEnBd.Placas,
                                    TarjetaCirculacion = vehiculoEnBd.TarjetaCirculacion,
                                    PolizaSeguro = vehiculoEnBd.PolizaSeguro,
                                    choferNombre = vehiculo.choferNombre,
                                    choferID = relVehiculoChofer.ChoferID.ToString(),
                                    transportistaNombre = vehiculo.transportistaNombre,
                                    transportistaID = relVehiculotransportista.TransportistaID.ToString(),
                                    relVehiculoChoferID = relVehiculoChofer.Rel_Vehiculo_Chofer_ID.ToString(),
                                    relVehiculoTransportistaID = relVehiculotransportista.Rel_Vehiculo_Transportista_ID.ToString()
                                };
                            }
                            else
                            {
                                throw new Exception("Tracto no existente");
                            }

                            #endregion
                        case 6: //Plana
                            #region
                            CatalogoPlana plana = serializer.Deserialize<CatalogoPlana>(data);
                            if (ctx.Sam3_Vehiculo.Where(x => x.TipoVehiculoID == 2 && x.Activo && x.VehiculoID.ToString() == plana.VehiculoID).AsParallel().Any())
                            {
                                int vehiculoID = Convert.ToInt32(plana.VehiculoID);
                                Sam3_Vehiculo planaEnBd = ctx.Sam3_Vehiculo.Where(x => x.VehiculoID == vehiculoID).AsParallel().SingleOrDefault();
                                //planaEnBd.Activo = true;
                                planaEnBd.TractoID = Convert.ToInt32(plana.TractoID);
                                planaEnBd.Placas = plana.Placas;
                                planaEnBd.Unidad = plana.Unidad;
                                planaEnBd.Modelo = plana.Modelo;
                                planaEnBd.UsuarioModificacion = usuario.UsuarioID;
                                planaEnBd.FechaModificacion = DateTime.Now;

                                ctx.SaveChanges();

                                Sam3_Rel_Vehiculo_Chofer relVehiculoChoferPlana = ctx.Sam3_Rel_Vehiculo_Chofer
                                    .Where(x => x.Rel_Vehiculo_Chofer_ID.ToString() == plana.relVehiculoChoferID && x.Activo).AsParallel().SingleOrDefault();

                                relVehiculoChoferPlana.ChoferID = Convert.ToInt32(plana.choferID);
                                relVehiculoChoferPlana.Activo = true;
                                relVehiculoChoferPlana.UsuarioModificacion = usuario.UsuarioID;
                                relVehiculoChoferPlana.FechaModificacion = DateTime.Now;
                                ctx.SaveChanges();


                                Sam3_Rel_Vehiculo_Transportista relVehiculotransportistaPlana = ctx.Sam3_Rel_Vehiculo_Transportista
                                    .Where(x => x.Rel_Vehiculo_Transportista_ID.ToString() == plana.relVehiculoTransportistaID && x.Activo).AsParallel().SingleOrDefault();

                                relVehiculotransportistaPlana.TransportistaID = Convert.ToInt32(plana.transportistaID);
                                relVehiculotransportistaPlana.Activo = true;
                                relVehiculotransportistaPlana.UsuarioModificacion = usuario.UsuarioID;
                                relVehiculotransportistaPlana.FechaModificacion = DateTime.Now;
                                ctx.SaveChanges();

                                return new CatalogoPlana
                                {
                                    VehiculoID = planaEnBd.VehiculoID.ToString(),
                                    Placas = planaEnBd.Placas,
                                    Unidad = planaEnBd.Unidad,
                                    Modelo = planaEnBd.Modelo,
                                    choferNombre = plana.choferNombre,
                                    choferID = relVehiculoChoferPlana.ChoferID.ToString(),
                                    transportistaNombre = plana.transportistaNombre,
                                    transportistaID = relVehiculotransportistaPlana.TransportistaID.ToString(),
                                    relVehiculoChoferID = relVehiculoChoferPlana.Rel_Vehiculo_Chofer_ID.ToString(),
                                    relVehiculoTransportistaID = relVehiculotransportistaPlana.Rel_Vehiculo_Transportista_ID.ToString()
                                };
                            }
                            else
                            {
                                throw new Exception("Plana no existente");
                            }
                            #endregion
                        case 7: //Proveedor
                            #region
                            CatalogoProveedor proveedor = serializer.Deserialize<CatalogoProveedor>(data);
                            if (ctx.Sam3_Proveedor.Where(x => x.ProveedorID.ToString() == proveedor.ProveedorID && x.Activo).AsParallel().Any())
                            {
                                Sam3_Proveedor provEnBd = ctx.Sam3_Proveedor.Where(x => x.ProveedorID.ToString() == proveedor.ProveedorID && x.Activo).AsParallel().SingleOrDefault();

                                provEnBd.ContactoID = proveedor.ContactoID != null && proveedor.ContactoID != provEnBd.ContactoID.ToString() ?
                                    Convert.ToInt32(proveedor.ContactoID) : provEnBd.ContactoID;

                                provEnBd.Nombre = proveedor.Nombre != null && proveedor.Nombre != provEnBd.Nombre ?
                                    proveedor.Nombre : provEnBd.Nombre;

                                provEnBd.Descripcion = proveedor.Descripcion != null && proveedor.Descripcion != provEnBd.Descripcion ?
                                    proveedor.Descripcion : provEnBd.Descripcion;

                                provEnBd.Direccion = proveedor.Direccion != null && proveedor.Direccion != provEnBd.Direccion ?
                                    proveedor.Direccion : provEnBd.Direccion;

                                provEnBd.Telefono = proveedor.Telefono != null && proveedor.Telefono != provEnBd.Telefono ?
                                    proveedor.Telefono : provEnBd.Telefono;

                                provEnBd.UsuarioModificacion = usuario.UsuarioID;

                                provEnBd.FechaModificacion = DateTime.Now;

                                ctx.SaveChanges();

                                return new CatalogoProveedor
                                {
                                    Contacto = proveedor.Contacto,
                                    ContactoID = provEnBd.ContactoID.ToString(),
                                    Nombre = provEnBd.Nombre,
                                    Descripcion = provEnBd.Descripcion,
                                    Direccion = provEnBd.Direccion,
                                    Telefono = provEnBd.Telefono
                                };
                            }
                            else
                            {
                                throw new Exception("Proveedor no existente");
                            }
                            #endregion
                        case 8: //Tipo de uso
                            #region
                            Catalogos tipoUso = serializer.Deserialize<Catalogos>(data);
                            if (!ctx.Sam3_TipoUso.Where(x => x.Activo && x.TipoUsoID.ToString() == tipoUso.Id).AsParallel().Any())
                            {
                                Sam3_TipoUso tipoUsoEnBd = ctx.Sam3_TipoUso.Where(x => x.TipoUsoID.ToString() == tipoUso.Id && x.Activo).AsParallel().SingleOrDefault();

                                tipoUsoEnBd.Nombre = tipoUso.Nombre != null && tipoUso.Nombre != tipoUsoEnBd.Nombre ?
                                    tipoUso.Nombre : tipoUsoEnBd.Nombre;

                                tipoUsoEnBd.UsuarioModificacion = usuario.UsuarioID;

                                tipoUsoEnBd.FechaModificacion = DateTime.Now;

                                ctx.SaveChanges();

                                return new Catalogos
                                {
                                    Id = tipoUsoEnBd.TipoUsoID.ToString(),
                                    Nombre = tipoUsoEnBd.Nombre
                                };
                            }
                            else
                            {
                                throw new Exception("Tipo de uso no existente");
                            }
                            #endregion
                        case 9: //Camion
                            #region
                            Catalogos camion = serializer.Deserialize<Catalogos>(data);
                            if (ctx.Sam3_TipoVehiculo.Where(x => x.Activo && x.TipoVehiculoID.ToString() == camion.Id).AsParallel().Any())
                            {
                                Sam3_TipoVehiculo camionEnBd = ctx.Sam3_TipoVehiculo.Where(x => x.TipoVehiculoID.ToString() == camion.Id && x.Activo).AsParallel().SingleOrDefault();
                                camionEnBd.Nombre = camion.Nombre != null && camion.Nombre != camionEnBd.Nombre ?
                                    camion.Nombre : camionEnBd.Nombre;

                                camionEnBd.UsuarioModificacion = usuario.UsuarioID;

                                camionEnBd.FechaModificacion = DateTime.Now;

                                ctx.SaveChanges();

                                return new Catalogos
                                {
                                    Id = camionEnBd.TipoVehiculoID.ToString(),
                                    Nombre = camionEnBd.Nombre
                                };
                            }
                            else
                            {
                                throw new Exception("Camion no existente");
                            }
                            #endregion
                        case 10: //fabricante
                            #region
                            CatalogoFabricante fabricante = serializer.Deserialize<CatalogoFabricante>(data);
                            if (ctx.Sam3_Fabricante.Where(x => x.FabricanteID.ToString() == fabricante.FabricanteID && x.Activo).AsParallel().Any())
                            {
                                Sam3_Fabricante fabricanteEnBd = ctx.Sam3_Fabricante.Where(x => x.FabricanteID.ToString() == fabricante.FabricanteID && x.Activo).AsParallel().SingleOrDefault();

                                fabricanteEnBd.ContactoID = fabricante.ContactoID != null && fabricante.ContactoID != fabricanteEnBd.ContactoID.ToString() ?
                                    Convert.ToInt32(fabricante.ContactoID) : fabricanteEnBd.ContactoID;

                                fabricanteEnBd.Nombre = fabricante.Nombre != null && fabricante.Nombre != fabricanteEnBd.Nombre ?
                                    fabricante.Nombre : fabricanteEnBd.Nombre;

                                fabricanteEnBd.Descripcion = fabricante.Descripcion != null && fabricante.Descripcion != fabricanteEnBd.Descripcion ?
                                    fabricante.Descripcion : fabricanteEnBd.Descripcion;

                                fabricanteEnBd.Direccion = fabricante.Direccion != null && fabricante.Direccion != fabricanteEnBd.Direccion ?
                                    fabricante.Direccion : fabricanteEnBd.Direccion;

                                fabricanteEnBd.Telefono = fabricante.Telefono != null && fabricante.Telefono != fabricanteEnBd.Telefono ?
                                    fabricante.Telefono : fabricanteEnBd.Telefono;

                                fabricanteEnBd.UsuarioModificacion = usuario.UsuarioID;

                                fabricanteEnBd.FechaModificacion = DateTime.Now;

                                ctx.SaveChanges();

                                return new CatalogoFabricante
                                {
                                    Contacto = fabricante.Contacto,
                                    ContactoID = fabricante.ContactoID,
                                    Nombre = fabricanteEnBd.Nombre,
                                    Descripcion = fabricanteEnBd.Descripcion,
                                    Direccion = fabricanteEnBd.Direccion,
                                    Telefono = fabricanteEnBd.Telefono
                                };
                            }
                            else
                            {
                                throw new Exception("Fabricante no existente");
                            }
                            #endregion
                        case 12: //MTR
                            #region
                            CatalogoMTR mtr = serializer.Deserialize<CatalogoMTR>(data);
                            if (ctx.Sam3_MTR.Where(x => x.MTRID == mtr.MTRID && x.Activo).AsParallel().Any())
                            {
                                Sam3_MTR mtrEnBd = ctx.Sam3_MTR.Where(x => x.Activo && x.MTRID == mtr.MTRID).AsParallel().SingleOrDefault();
                                mtrEnBd.NumeroMTR = mtr.NumeroMTR != null && mtr.NumeroMTR != mtrEnBd.NumeroMTR ?
                                    mtr.NumeroMTR : mtrEnBd.NumeroMTR;

                                int valor = Convert.ToInt32(mtr.ItemCodeID);

                                int itemCodeSam3 = (from icd in ctx.Sam3_Rel_ItemCode_Diametro
                                                    where icd.Activo && icd.Rel_ItemCode_Diametro_ID == valor
                                                    select icd.ItemCodeID).AsParallel().SingleOrDefault();

                                mtrEnBd.ItemCodeID = mtr.ItemCodeID != null && Convert.ToInt32(mtr.ItemCodeID) != mtrEnBd.ItemCodeID ?
                                    itemCodeSam3 : mtrEnBd.ItemCodeID;

                                mtrEnBd.ColadaID = mtr.ColadaID != null && mtr.ColadaID != mtrEnBd.ColadaID.ToString() ?
                                    Convert.ToInt32(mtr.ColadaID) : mtrEnBd.ColadaID;

                                mtrEnBd.CantidadPiezas = mtr.CantidadPiezas != null && Convert.ToInt32(mtr.CantidadPiezas) != mtrEnBd.CantidadPiezas ?
                                    Convert.ToInt32(mtr.CantidadPiezas) : mtrEnBd.CantidadPiezas;

                                mtrEnBd.FechaModificacion = DateTime.Now;
                                mtrEnBd.UsuarioModificacion = usuario.UsuarioID;
                                mtrEnBd.Rel_ItemCode_Diametro_ID = valor;

                                ctx.SaveChanges();

                                return new CatalogoMTR
                                {
                                    MTRID = mtr.MTRID,
                                    NumeroMTR = mtr.NumeroMTR,
                                    CantidadPiezas = mtr.CantidadPiezas,
                                    Colada = mtr.Colada,
                                    ColadaID = mtr.ColadaID,
                                    ItemCode = mtr.ItemCode,
                                    ItemCodeID = valor.ToString(),
                                    D1 = mtr.D1,
                                    D2 = mtr.D2
                                };
                            }
                            else
                            {
                                throw new Exception("MTR no existente");
                            }
                            #endregion
                        default:
                            #region
                            TransactionalInformation resultado = new TransactionalInformation();
                            resultado.ReturnMessage.Add("Listado no encontrado");
                            resultado.ReturnCode = 500;
                            resultado.ReturnStatus = false;
                            resultado.IsAuthenicated = false;
                            return resultado;
                            #endregion
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

        /// <summary>
        /// Funcion para eliminar un elemento del catalogo
        /// Catalogos ABC 
        /// </summary>
        /// <param name="id">id del elemento a eliminar</param>
        /// <param name="catalogoID">catalogo seleccionado</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns></returns>
        public object EliminarElementoCatalogo(int id, string catalogoID, Sam3_Usuario usuario)
        {
            try
            {
                object res = new object();
                TransactionalInformation result = new TransactionalInformation();

                using (SamContext ctx = new SamContext())
                {
                    switch (Convert.ToInt32(catalogoID))
                    {
                        case 1: //Patios
                            #region
                            res = PatioBd.Instance.EliminarPatio(id, usuario);
                            return res;
                            #endregion
                        case 2: //Chofer
                            #region

                            res = ChoferBd.Instance.EliminarChofer(id, usuario);
                            return res;

                            #endregion
                        case 3: //Tipo Aviso
                            #region

                            Sam3_TipoAviso tipoAviso = ctx.Sam3_TipoAviso.Where(x => x.TipoAvisoID == id).AsParallel().SingleOrDefault();

                            tipoAviso.Activo = false;
                            tipoAviso.UsuarioModificacion = usuario.UsuarioID;
                            tipoAviso.FechaModificacion = DateTime.Now;

                            ctx.SaveChanges();

                            result = new TransactionalInformation();
                            result.ReturnCode = 200;
                            result.ReturnStatus = true;
                            result.ReturnMessage.Add("OK");
                            result.IsAuthenicated = true;

                            return result;

                            #endregion
                        case 4: //Transportista 
                            #region

                            res = TransportistaBd.Instance.EliminarTransportista(id, usuario);
                            return res;

                            #endregion
                        case 5: //Tracto
                            #region

                            res = TractoBd.Instance.EliminarTracto(id, usuario);
                            return res;

                            #endregion
                        case 6: //Plana
                            #region

                            res = PlanaBd.Instance.EliminarPlana(id, usuario);
                            return res;

                            #endregion
                        case 7: //Proveedor
                            #region

                            res = ProveedorBd.Instance.EliminarProveedor(id, usuario);
                            return res;

                            #endregion
                        case 8: //Tipo de uso
                            #region

                            Sam3_TipoUso tipoUso = ctx.Sam3_TipoUso.Where(x => x.TipoUsoID == id).AsParallel().SingleOrDefault();
                            tipoUso.Activo = false;
                            tipoUso.UsuarioModificacion = usuario.UsuarioID;
                            tipoUso.FechaModificacion = DateTime.Now;

                            ctx.SaveChanges();

                            result = new TransactionalInformation();
                            result.ReturnCode = 200;
                            result.ReturnStatus = true;
                            result.ReturnMessage.Add("OK");
                            result.IsAuthenicated = true;

                            return result;

                            #endregion
                        case 9: //Camion
                            #region

                            Sam3_TipoVehiculo tipoVehiculo = ctx.Sam3_TipoVehiculo.Where(x => x.TipoVehiculoID == id).AsParallel().SingleOrDefault();
                            tipoVehiculo.Activo = false;
                            tipoVehiculo.UsuarioModificacion = usuario.UsuarioID;
                            tipoVehiculo.FechaModificacion = DateTime.Now;

                            ctx.SaveChanges();

                            result = new TransactionalInformation();
                            result.ReturnCode = 200;
                            result.ReturnStatus = true;
                            result.ReturnMessage.Add("OK");
                            result.IsAuthenicated = true;

                            return result;

                            #endregion
                        case 10:  //fabricante
                            #region

                            Sam3_Fabricante fabricante = ctx.Sam3_Fabricante.Where(x => x.FabricanteID == id).AsParallel().SingleOrDefault();
                            fabricante.Activo = false;
                            fabricante.UsuarioModificacion = usuario.UsuarioID;
                            fabricante.FechaModificacion = DateTime.Now;

                            ctx.SaveChanges();

                            result = new TransactionalInformation();
                            result.ReturnCode = 200;
                            result.ReturnStatus = true;
                            result.ReturnMessage.Add("OK");
                            result.IsAuthenicated = true;

                            return result;

                            #endregion
                        case 12://MTR
                            #region
                            Sam3_MTR mtr = ctx.Sam3_MTR.Where(x => x.MTRID == id).AsParallel().SingleOrDefault();
                            mtr.Activo = false;
                            mtr.UsuarioModificacion = usuario.UsuarioID;
                            mtr.FechaModificacion = DateTime.Now;

                            ctx.SaveChanges();

                            result = new TransactionalInformation();
                            result.ReturnCode = 200;
                            result.ReturnStatus = true;
                            result.ReturnMessage.Add("OK");
                            result.IsAuthenicated = true;

                            return result;

                            #endregion
                        default:
                            #region
                            TransactionalInformation resultado = new TransactionalInformation();
                            resultado.ReturnMessage.Add("Listado no encontrado");
                            resultado.ReturnCode = 500;
                            resultado.ReturnStatus = false;
                            resultado.IsAuthenicated = false;
                            return resultado;
                            #endregion
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

        /// <summary>
        /// Funcion para insertar un nuevo elemento del 
        /// catalogo seleccionado a la base de datos
        /// Catalogos ABC
        /// </summary>
        /// <param name="data">datos capturados en el grid</param>
        /// <param name="catalogoID">catalogo seleccionado</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns>datos del elemento insertado</returns>
        public object InsertarElementoAlCatalogo(string data, string catalogoID, Sam3_Usuario usuario)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                object res = new object();
                TransactionalInformation result = new TransactionalInformation();

                using (SamContext ctx = new SamContext())
                {
                    switch (Convert.ToInt32(catalogoID))
                    {
                        case 1: //Patios
                            #region
                            CatalogoPatio catalogoPatio = serializer.Deserialize<CatalogoPatio>(data);
                            if (!ctx.Sam3_Patio.Where(x => x.Nombre == catalogoPatio.Nombre && x.Activo).AsParallel().Any())
                            {
                                Sam3_Patio patio = new Sam3_Patio();
                                patio.Nombre = catalogoPatio.Nombre;
                                patio.Descripcion = catalogoPatio.Descripcion;
                                patio.Propietario = catalogoPatio.Propietario;
                                patio.RequierePermisoAduana = Convert.ToBoolean(catalogoPatio.RequierePermisoAduana);
                                patio.Activo = true;
                                patio.UsuarioModificacion = usuario.UsuarioID;
                                patio.FechaModificacion = DateTime.Now;

                                ctx.Sam3_Patio.Add(patio);
                                ctx.SaveChanges();

                                return new CatalogoPatio
                                {
                                    PatioID = patio.PatioID.ToString(),
                                    Nombre = patio.Nombre,
                                    Descripcion = patio.Descripcion,
                                    Propietario = patio.Propietario,
                                    RequierePermisoAduana = patio.RequierePermisoAduana == true ? "Si" : "No"
                                };
                            }
                            else
                            {
                                throw new Exception("Patio existente");
                            }

                            #endregion
                        case 2: //Chofer
                            #region
                            CatalogoChofer catalogoChofer = serializer.Deserialize<CatalogoChofer>(data);
                            int transportistaID = Convert.ToInt32(catalogoChofer.TransportistaID);
                            if (!ctx.Sam3_Chofer.Where(x => x.Nombre == catalogoChofer.Nombre && x.TransportistaID == transportistaID && x.Activo).AsParallel().Any())
                            {
                                Sam3_Chofer chofer = new Sam3_Chofer();
                                chofer.Nombre = catalogoChofer.Nombre;
                                chofer.TransportistaID = transportistaID;
                                chofer.Activo = true;
                                chofer.FechaModificacion = DateTime.Now;
                                chofer.UsuarioModificacion = usuario.UsuarioID;

                                ctx.Sam3_Chofer.Add(chofer);
                                ctx.SaveChanges();

                                return new CatalogoChofer
                                {
                                    ChoferID = chofer.ChoferID.ToString(),
                                    Nombre = chofer.Nombre,
                                    TransportistaID = chofer.TransportistaID.ToString(),
                                    TransportistaNombre = catalogoChofer.TransportistaNombre
                                };
                            }
                            else
                            {
                                throw new Exception("Chofer existente");
                            }
                            #endregion
                        case 3: //Tipo Aviso
                            #region

                            Catalogos CatalogotipoAviso = serializer.Deserialize<Catalogos>(data);
                            if (!ctx.Sam3_TipoAviso.Where(t => t.Nombre == CatalogotipoAviso.Nombre && t.Activo).AsParallel().Any())
                            {
                                Sam3_TipoAviso tipoAviso = new Sam3_TipoAviso();

                                tipoAviso.Nombre = CatalogotipoAviso.Nombre;
                                tipoAviso.Activo = true;
                                tipoAviso.UsuarioModificacion = usuario.UsuarioID;
                                tipoAviso.FechaModificacion = DateTime.Now;

                                ctx.Sam3_TipoAviso.Add(tipoAviso);
                                ctx.SaveChanges();

                                return new Catalogos
                                {
                                    Id = tipoAviso.TipoAvisoID.ToString(),
                                    Nombre = tipoAviso.Nombre
                                };
                            }
                            else
                            {
                                throw new Exception("Tipo Aviso existente");
                            }
                            #endregion
                        case 4: //Transportista 
                            #region
                            CatalogoTransportista catalogoTransportista = serializer.Deserialize<CatalogoTransportista>(data);
                            if (!ctx.Sam3_Transportista.Where(x => x.Nombre == catalogoTransportista.Nombre && x.Activo).AsParallel().Any())
                            {
                                Sam3_Transportista transportista = new Sam3_Transportista();

                                transportista.ContactoID = Convert.ToInt32(catalogoTransportista.ContactoID);
                                transportista.Nombre = catalogoTransportista.Nombre;
                                transportista.Descripcion = catalogoTransportista.Descripcion;
                                transportista.Direccion = catalogoTransportista.Direccion;
                                transportista.Telefono = catalogoTransportista.Telefono;
                                transportista.Activo = true;
                                transportista.UsuarioModificacion = usuario.UsuarioID;
                                transportista.FechaModificacion = DateTime.Now;

                                ctx.Sam3_Transportista.Add(transportista);

                                ctx.SaveChanges();

                                return new CatalogoTransportista
                                {
                                    TransportistaID = transportista.TransportistaID.ToString(),
                                    ContactoID = transportista.ContactoID.ToString(),
                                    Contacto = catalogoTransportista.Contacto,
                                    Descripcion = transportista.Descripcion,
                                    Direccion = transportista.Direccion,
                                    Nombre = transportista.Nombre,
                                    Telefono = transportista.Telefono
                                };
                            }
                            else
                            {
                                throw new Exception("Transportista existente");
                            }
                            #endregion
                        case 5: //Tracto
                            #region

                            VehiculoJson tracto = serializer.Deserialize<VehiculoJson>(data);
                            if (!ctx.Sam3_Vehiculo.Where(c => c.Placas == tracto.Placas && c.TipoVehiculoID == 1 && c.Activo).AsParallel().Any())
                            {
                                tracto.TipoVehiculoID = "1";

                                Sam3_Vehiculo nuevoCamion = new Sam3_Vehiculo();
                                nuevoCamion.Activo = true;
                                nuevoCamion.FechaModificacion = DateTime.Now;
                                nuevoCamion.Placas = tracto.Placas;
                                nuevoCamion.PolizaSeguro = tracto.PolizaSeguro;
                                nuevoCamion.TarjetaCirculacion = tracto.TarjetaCirculacion;
                                nuevoCamion.UsuarioModificacion = usuario.UsuarioID;
                                nuevoCamion.TipoVehiculoID = Convert.ToInt32(tracto.TipoVehiculoID);

                                ctx.Sam3_Vehiculo.Add(nuevoCamion);
                                ctx.SaveChanges();

                                Sam3_Rel_Vehiculo_Chofer nuevoRegistroChofer = new Sam3_Rel_Vehiculo_Chofer();
                                nuevoRegistroChofer.VehiculoID = nuevoCamion.VehiculoID;
                                nuevoRegistroChofer.Activo = true;
                                nuevoRegistroChofer.ChoferID = Convert.ToInt32(tracto.ChoferID);
                                nuevoRegistroChofer.FechaModificacion = DateTime.Now;
                                nuevoRegistroChofer.UsuarioModificacion = usuario.UsuarioID;

                                ctx.Sam3_Rel_Vehiculo_Chofer.Add(nuevoRegistroChofer);

                                Sam3_Rel_Vehiculo_Transportista transportista = new Sam3_Rel_Vehiculo_Transportista();
                                transportista.Activo = true;
                                transportista.FechaModificacion = DateTime.Now;
                                transportista.TransportistaID = Convert.ToInt32(tracto.TransportistaID);
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
                                    choferNombre = tracto.choferNombre,
                                    choferID = tracto.ChoferID.ToString(),
                                    transportistaNombre = tracto.transportistaNombre,
                                    transportistaID = tracto.TransportistaID.ToString(),
                                    relVehiculoChoferID = nuevoRegistroChofer.Rel_Vehiculo_Chofer_ID.ToString(),
                                    relVehiculoTransportistaID = transportista.Rel_Vehiculo_Transportista_ID.ToString()
                                };
                            }
                            else
                            {
                                throw new Exception("Tracto existente");
                            }
                            #endregion
                        case 6: //Plana
                            #region
                            VehiculoJson plana = serializer.Deserialize<VehiculoJson>(data);
                            plana.TipoVehiculoID = "2";

                            if (!ctx.Sam3_Vehiculo.Where(c => c.Placas == plana.Placas && c.TipoVehiculoID == 2 && c.Activo).AsParallel().Any())
                            {
                                Sam3_Vehiculo nuevaPlana = new Sam3_Vehiculo
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

                            #endregion
                        case 7: //Proveedor
                            #region

                            CatalogoProveedor catalogoProveedor = serializer.Deserialize<CatalogoProveedor>(data);
                            if (!ctx.Sam3_Proveedor.Where(p => p.Nombre == catalogoProveedor.Nombre && p.Activo).AsParallel().Any())
                            {
                                Sam3_Proveedor proveedor = new Sam3_Proveedor();

                                proveedor.ContactoID = Convert.ToInt32(catalogoProveedor.ContactoID);
                                proveedor.Nombre = catalogoProveedor.Nombre;
                                proveedor.Descripcion = catalogoProveedor.Descripcion;
                                proveedor.Direccion = catalogoProveedor.Direccion;
                                proveedor.Telefono = catalogoProveedor.Telefono;

                                proveedor.UsuarioModificacion = usuario.UsuarioID;
                                proveedor.FechaModificacion = DateTime.Now;
                                proveedor.Activo = true;
                                ctx.Sam3_Proveedor.Add(proveedor);
                                ctx.SaveChanges();

                                return new CatalogoProveedor
                                {
                                    Contacto = catalogoProveedor.Contacto,
                                    ContactoID = proveedor.ContactoID.ToString(),
                                    Nombre = proveedor.Nombre,
                                    Descripcion = proveedor.Descripcion,
                                    Direccion = proveedor.Direccion,
                                    Telefono = proveedor.Telefono,
                                    ProveedorID = proveedor.ProveedorID.ToString()
                                };
                            }
                            else
                            {
                                throw new Exception("Proveedor existente");
                            }

                            #endregion
                        case 8: //Tipo de uso
                            #region
                            Catalogos catalogoTipoUso = serializer.Deserialize<Catalogos>(data);

                            Sam3_TipoUso tipoUso = new Sam3_TipoUso();

                            if (!ctx.Sam3_TipoUso.Where(x => x.Nombre == catalogoTipoUso.Nombre && x.Activo).Any())
                            {
                                tipoUso.Activo = true;
                                tipoUso.FechaModificacion = DateTime.Now;
                                tipoUso.Nombre = catalogoTipoUso.Nombre; ;
                                tipoUso.UsuarioModificacion = usuario.UsuarioID;

                                ctx.Sam3_TipoUso.Add(tipoUso);
                                ctx.SaveChanges();

                                return new Catalogos
                                {
                                    Id = tipoUso.TipoUsoID.ToString(),
                                    Nombre = tipoUso.Nombre
                                };
                            }
                            else
                            {
                                throw new Exception("Tipo de Uso existente");
                            }

                            #endregion
                        case 9: //Camion
                            #region

                            Catalogos catalogosVehiculo = serializer.Deserialize<Catalogos>(data);
                            if (!ctx.Sam3_TipoVehiculo.Where(x => x.Nombre == catalogosVehiculo.Nombre && x.Activo).AsParallel().Any())
                            {
                                Sam3_TipoVehiculo vehiculo = new Sam3_TipoVehiculo();

                                vehiculo.Nombre = catalogosVehiculo.Nombre;

                                vehiculo.Activo = true;
                                vehiculo.FechaModificacion = DateTime.Now;
                                vehiculo.UsuarioModificacion = usuario.UsuarioID;

                                ctx.Sam3_TipoVehiculo.Add(vehiculo);
                                ctx.SaveChanges();

                                return new Catalogos
                                {
                                    Id = vehiculo.TipoVehiculoID.ToString(),
                                    Nombre = vehiculo.Nombre
                                };
                            }
                            else
                            {
                                throw new Exception("Camion existente");
                            }
                            #endregion
                        case 10:  //fabricante
                            #region
                            CatalogoFabricante catalogoFabricante = serializer.Deserialize<CatalogoFabricante>(data);
                            if (!ctx.Sam3_Fabricante.Where(x => x.Nombre == catalogoFabricante.Nombre && x.Activo).AsParallel().Any())
                            {
                                Sam3_Fabricante fabricante = new Sam3_Fabricante();

                                fabricante.ContactoID = Convert.ToInt32(catalogoFabricante.ContactoID);
                                fabricante.Nombre = catalogoFabricante.Nombre;
                                fabricante.Descripcion = catalogoFabricante.Descripcion;
                                fabricante.Direccion = catalogoFabricante.Direccion;
                                fabricante.Telefono = catalogoFabricante.Telefono;

                                fabricante.Activo = true;
                                fabricante.UsuarioModificacion = usuario.UsuarioID;
                                fabricante.FechaModificacion = DateTime.Now;

                                ctx.Sam3_Fabricante.Add(fabricante);
                                ctx.SaveChanges();

                                return new CatalogoFabricante
                                {
                                    FabricanteID = fabricante.FabricanteID.ToString(),
                                    Contacto = catalogoFabricante.Contacto,
                                    ContactoID = catalogoFabricante.ContactoID,
                                    Nombre = fabricante.Nombre,
                                    Descripcion = fabricante.Descripcion,
                                    Direccion = fabricante.Direccion,
                                    Telefono = fabricante.Telefono
                                };
                            }
                            else
                            {
                                throw new Exception("Fabricante existente");
                            }
                            #endregion
                        case 12: //MTR
                            #region
                            CatalogoMTR mtr = serializer.Deserialize<CatalogoMTR>(data);
                            int coladaID = Convert.ToInt32(mtr.ColadaID);
                            int valor = Convert.ToInt32(mtr.ItemCodeID);
                            int itemCodeSam3 = (from icd in ctx.Sam3_Rel_ItemCode_Diametro
                                                    where icd.Activo && icd.Rel_ItemCode_Diametro_ID == valor
                                                    select icd.ItemCodeID).AsParallel().SingleOrDefault();

                            if (!ctx.Sam3_MTR.Where(x => x.NumeroMTR == mtr.NumeroMTR && x.ColadaID == coladaID 
                                && x.ItemCodeID == itemCodeSam3 && x.Rel_ItemCode_Diametro_ID == valor && x.Activo).AsParallel().Any())
                            {
                                Sam3_MTR catalogoMTR = new Sam3_MTR();
                                catalogoMTR.NumeroMTR = mtr.NumeroMTR;
                                catalogoMTR.ItemCodeID = itemCodeSam3;
                                catalogoMTR.ColadaID = coladaID;
                                catalogoMTR.CantidadPiezas = Convert.ToInt32(mtr.CantidadPiezas);
                                catalogoMTR.Activo = true;
                                catalogoMTR.FechaModificacion = DateTime.Now;
                                catalogoMTR.UsuarioModificacion = usuario.UsuarioID;
                                catalogoMTR.Rel_ItemCode_Diametro_ID = valor;

                                ctx.Sam3_MTR.Add(catalogoMTR);
                                ctx.SaveChanges();

                                return new CatalogoMTR
                                {
                                    MTRID = catalogoMTR.MTRID,
                                    NumeroMTR = mtr.NumeroMTR,
                                    CantidadPiezas = mtr.CantidadPiezas,
                                    Colada = mtr.Colada,
                                    ColadaID = mtr.ColadaID,
                                    ItemCode = mtr.ItemCode,
                                    ItemCodeID = valor.ToString(),
                                    D1 = mtr.D1, 
                                    D2 = mtr.D2, 
                                    Proyecto = mtr.Proyecto
                                };
                            }
                            else
                            {
                                throw new Exception("MTR existente");
                            }
                            #endregion
                        default:
                            #region
                            TransactionalInformation resultado = new TransactionalInformation();
                            resultado.ReturnMessage.Add("Listado no encontrado");
                            resultado.ReturnCode = 500;
                            resultado.ReturnStatus = false;
                            resultado.IsAuthenicated = false;
                            return resultado;
                            #endregion
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

        /// <summary>
        /// Guardar nueva cedula
        /// </summary>
        /// <param name="data"></param>
        /// <param name="catalogoID"></param>
        /// <param name="factorConversion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object InsertarCedula(string data, string catalogoID, string factorConversion, Sam3_Usuario usuario)
        {
            try
            {
                List<CatalogoCedulas> cedulasCorrectas = new List<CatalogoCedulas>();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<CatalogoCedulas> catalogoCedulas = serializer.Deserialize<List<CatalogoCedulas>>(data);

                //ids de Cedulas
                int idCedulaA = 0;
                int idCedulaB = 0;
                int idCedulaC = 0;
                int idDiametro = 0;

                bool existe = false;

                foreach (CatalogoCedulas item in catalogoCedulas)
                {
                    decimal diam = Convert.ToDecimal(item.Diametro1);
                    ValidarDiametros(diam, usuario);
                    using (SamContext ctx = new SamContext())
                    {
                        using (var sam3_tran = ctx.Database.BeginTransaction())
                        {
                            using (Sam2Context ctx2 = new Sam2Context())
                            {
                                using (var sam2_tran = ctx2.Database.BeginTransaction())
                                {
                                    Sam3_CatalogoCedulas nuevoElemento = new Sam3_CatalogoCedulas();
                                    decimal factor = Convert.ToDecimal(factorConversion);

                                    idDiametro = ctx.Sam3_Diametro.Where(x => x.Valor == diam && x.Activo).Select(x => x.DiametroID).AsParallel().SingleOrDefault();
                                    int? eqDiametro = ctx.Sam3_EquivalenciaDiametro.Where(x => x.Sam3_DiametroID == idDiametro && x.Activo).Select(x => x.Sam2_DiametroID).AsParallel().SingleOrDefault();


                                    if (idDiametro == 0)
                                    {
                                        item.Correcta = false;
                                        item.MensajeError = "El diámetro " + item.Diametro1 + " no existe en el catálogo";
                                    }
                                    else
                                    {
                                        item.Correcta = true;
                                    }

                                    decimal EspesorMM = Convert.ToDecimal(item.CedulaMM);
                                    if (!String.IsNullOrEmpty(item.CedulaA) && item.Correcta)
                                    {
                                        if (!ctx.Sam3_Cedula.Where(x => x.Codigo == item.CedulaA && x.Activo).Any())
                                        {
                                            //Inserto la cedula A
                                            Sam3_Cedula nuevaCedula = new Sam3_Cedula();
                                            nuevaCedula.Activo = true;
                                            nuevaCedula.Codigo = item.CedulaA;
                                            nuevaCedula.FechaModificacion = DateTime.Now;
                                            nuevaCedula.UsuarioModificacion = usuario.UsuarioID;

                                            ctx.Sam3_Cedula.Add(nuevaCedula);
                                            ctx.SaveChanges();

                                            DatabaseManager.Sam2.Cedula nuevaCedulaSAM2 = new DatabaseManager.Sam2.Cedula();

                                            if (!ctx2.Cedula.Where(x => x.Codigo == item.CedulaA).Any())
                                            {
                                                nuevaCedulaSAM2.Codigo = item.CedulaA;
                                                nuevaCedulaSAM2.FechaModificacion = DateTime.Now;
                                                nuevaCedulaSAM2.VerificadoPorCalidad = false;

                                                ctx2.Cedula.Add(nuevaCedulaSAM2);
                                                ctx2.SaveChanges();
                                            }
                                            else
                                            {
                                                nuevaCedulaSAM2 = ctx2.Cedula.Where(x => x.Codigo == item.CedulaA).SingleOrDefault();
                                            }



                                            Sam3_EquivalenciaCedula equivalencia = new Sam3_EquivalenciaCedula();
                                            equivalencia.Activo = true;
                                            equivalencia.FechaModificacion = DateTime.Now;
                                            equivalencia.Sam2_CedulaID = nuevaCedulaSAM2.CedulaID;
                                            equivalencia.Sam3_CedulaID = nuevaCedula.CedulaID;
                                            equivalencia.UsuarioModificacion = usuario.UsuarioID;

                                            ctx.Sam3_EquivalenciaCedula.Add(equivalencia);
                                            ctx.SaveChanges();
                                        }

                                        idCedulaA = ctx.Sam3_Cedula.Where(x => x.Codigo == item.CedulaA && x.Activo).Select(x => x.CedulaID).AsParallel().SingleOrDefault();
                                        int eqCedulaA = ctx.Sam3_EquivalenciaCedula.Where(x => x.Sam3_CedulaID == idCedulaA && x.Activo).Select(x => x.Sam2_CedulaID).AsParallel().SingleOrDefault();

                                        if (!ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro && x.CedulaID == idCedulaA && x.Activo == 1).Any())
                                        {
                                            //Inserto el espesor
                                            Sam3_Espesor nuevoEspesor = new Sam3_Espesor();
                                            nuevoEspesor.Activo = 1;
                                            nuevoEspesor.FechaModificacion = DateTime.Now;
                                            nuevoEspesor.UsuarioModificacion = usuario.UsuarioID;
                                            nuevoEspesor.CedulaID = idCedulaA;
                                            nuevoEspesor.DiametroID = idDiametro;
                                            nuevoEspesor.Valor = Convert.ToDecimal(item.CedulaMM);

                                            ctx.Sam3_Espesor.Add(nuevoEspesor);
                                            ctx.SaveChanges();

                                            Espesor nuevoEspesorSAM2 = new Espesor();
                                            if (!ctx2.Espesor.Where(x => x.DiametroID == eqDiametro.Value && x.CedulaID == eqCedulaA).Any())
                                            {
                                                nuevoEspesorSAM2.DiametroID = (int)eqDiametro;
                                                nuevoEspesorSAM2.CedulaID = eqCedulaA;
                                                nuevoEspesorSAM2.Valor = Convert.ToDecimal(item.CedulaMM);
                                                nuevoEspesorSAM2.FechaModificacion = DateTime.Now;

                                                ctx2.Espesor.Add(nuevoEspesorSAM2);
                                                ctx2.SaveChanges();
                                            }
                                            else
                                            {
                                                nuevoEspesorSAM2 = ctx2.Espesor.Where(x => x.DiametroID == eqDiametro.Value && x.CedulaID == eqCedulaA).SingleOrDefault();
                                            }

                                            Sam3_EquivalenciaEspesor equivalencia = new Sam3_EquivalenciaEspesor();
                                            equivalencia.Activo = true;
                                            equivalencia.FechaModificacion = DateTime.Now;
                                            equivalencia.UsuarioModificacion = usuario.UsuarioID;
                                            equivalencia.Sam2_EspesorID = nuevoEspesorSAM2.EspesorID;
                                            equivalencia.Sam3_EspesorID = nuevoEspesor.EspesorID;

                                            ctx.Sam3_EquivalenciaEspesor.Add(equivalencia);
                                            ctx.SaveChanges();
                                        }
                                        else
                                        {
                                            if (ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro && x.CedulaID == idCedulaA && x.Valor != EspesorMM && x.Activo == 1).Any())
                                            {
                                                //item.Correcta = false;
                                                //item.MensajeError = "Ya existe la cédula " + item.CedulaA + ", diámetro " + item.Diametro1 + " pero con un espesor " 
                                                //+ ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro 
                                                //&& x.CedulaID == idCedulaA && x.Activo == 1).Select(x => x.Valor).AsParallel().SingleOrDefault();
                                                Sam3_Espesor espesorS3 = ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro && x.CedulaID == idCedulaA
                                                    && x.Valor != EspesorMM && x.Activo == 1).AsParallel().SingleOrDefault();
                                                espesorS3.FechaModificacion = DateTime.Now;
                                                espesorS3.Valor = EspesorMM;
                                                ctx.SaveChanges();

                                                int S2espesorID = (from eq in ctx.Sam3_EquivalenciaEspesor
                                                                   where eq.Activo && eq.Sam3_EspesorID == espesorS3.EspesorID
                                                                   select eq.Sam2_EspesorID).AsParallel().SingleOrDefault();

                                                Espesor espesorSam2 = ctx2.Espesor.Where(x => x.EspesorID == S2espesorID).AsParallel().SingleOrDefault();
                                                espesorSam2.Valor = EspesorMM;
                                                espesorSam2.FechaModificacion = DateTime.Now;
                                                ctx2.SaveChanges();
                                            }
                                        }
                                    }

                                    if (!String.IsNullOrEmpty(item.CedulaB) && item.Correcta)
                                    {
                                        if (!ctx.Sam3_Cedula.Where(x => x.Codigo == item.CedulaB && x.Activo).Any())
                                        {
                                            //Inserto Cedula B
                                            Sam3_Cedula nuevaCedula = new Sam3_Cedula();
                                            nuevaCedula.Activo = true;
                                            nuevaCedula.Codigo = item.CedulaB;
                                            nuevaCedula.FechaModificacion = DateTime.Now;
                                            nuevaCedula.UsuarioModificacion = usuario.UsuarioID;

                                            ctx.Sam3_Cedula.Add(nuevaCedula);
                                            ctx.SaveChanges();

                                            DatabaseManager.Sam2.Cedula nuevaCedulaSAM2 = new DatabaseManager.Sam2.Cedula();
                                            if (!ctx2.Cedula.Where(x => x.Codigo == item.CedulaB).Any())
                                            {
                                                nuevaCedulaSAM2.Codigo = item.CedulaB;
                                                nuevaCedulaSAM2.FechaModificacion = DateTime.Now;
                                                nuevaCedulaSAM2.VerificadoPorCalidad = false;

                                                ctx2.Cedula.Add(nuevaCedulaSAM2);
                                                ctx2.SaveChanges();
                                            }
                                            else
                                            {
                                                nuevaCedulaSAM2 = ctx2.Cedula.Where(x => x.Codigo == item.CedulaB).SingleOrDefault();
                                            }

                                            Sam3_EquivalenciaCedula equivalencia = new Sam3_EquivalenciaCedula();
                                            equivalencia.Activo = true;
                                            equivalencia.FechaModificacion = DateTime.Now;
                                            equivalencia.Sam2_CedulaID = nuevaCedulaSAM2.CedulaID;
                                            equivalencia.Sam3_CedulaID = nuevaCedula.CedulaID;
                                            equivalencia.UsuarioModificacion = usuario.UsuarioID;

                                            ctx.Sam3_EquivalenciaCedula.Add(equivalencia);
                                            ctx.SaveChanges();
                                        }

                                        idCedulaB = ctx.Sam3_Cedula.Where(x => x.Codigo == item.CedulaB && x.Activo).Select(x => x.CedulaID).AsParallel().SingleOrDefault();
                                        int eqCedulaB = ctx.Sam3_EquivalenciaCedula.Where(x => x.Sam3_CedulaID == idCedulaB && x.Activo).Select(x => x.Sam2_CedulaID).AsParallel().SingleOrDefault();

                                        if (!ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro && x.CedulaID == idCedulaB && x.Activo == 1).Any())
                                        {
                                            //Inserto el espesor
                                            Sam3_Espesor nuevoEspesor = new Sam3_Espesor();
                                            nuevoEspesor.Activo = 1;
                                            nuevoEspesor.FechaModificacion = DateTime.Now;
                                            nuevoEspesor.UsuarioModificacion = usuario.UsuarioID;
                                            nuevoEspesor.CedulaID = idCedulaB;
                                            nuevoEspesor.DiametroID = idDiametro;
                                            nuevoEspesor.Valor = Convert.ToDecimal(item.CedulaMM);

                                            ctx.Sam3_Espesor.Add(nuevoEspesor);
                                            ctx.SaveChanges();

                                            Espesor nuevoEspesorSAM2 = new Espesor();
                                            if (!ctx2.Espesor.Where(x => x.DiametroID == eqDiametro.Value && x.CedulaID == eqCedulaB).Any())
                                            {
                                                nuevoEspesorSAM2.DiametroID = (int)eqDiametro;
                                                nuevoEspesorSAM2.CedulaID = eqCedulaB;
                                                nuevoEspesorSAM2.Valor = Convert.ToDecimal(item.CedulaMM);
                                                nuevoEspesorSAM2.FechaModificacion = DateTime.Now;

                                                ctx2.Espesor.Add(nuevoEspesorSAM2);
                                                ctx2.SaveChanges();
                                            }
                                            else
                                            {
                                                nuevoEspesorSAM2 = ctx2.Espesor.Where(x => x.DiametroID == eqDiametro.Value && x.CedulaID == eqCedulaB).SingleOrDefault();
                                            }

                                            Sam3_EquivalenciaEspesor equivalencia = new Sam3_EquivalenciaEspesor();
                                            equivalencia.Activo = true;
                                            equivalencia.FechaModificacion = DateTime.Now;
                                            equivalencia.UsuarioModificacion = usuario.UsuarioID;
                                            equivalencia.Sam2_EspesorID = nuevoEspesorSAM2.EspesorID;
                                            equivalencia.Sam3_EspesorID = nuevoEspesor.EspesorID;

                                            ctx.Sam3_EquivalenciaEspesor.Add(equivalencia);
                                            ctx.SaveChanges();
                                        }
                                        else
                                        {
                                            if (ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro && x.CedulaID == idCedulaB && x.Valor != EspesorMM && x.Activo == 1).Any())
                                            {
                                                //item.Correcta = false;
                                                //item.MensajeError = "Ya existe la cédula " + item.CedulaA + ", diámetro " + item.Diametro1 + " pero con un espesor " 
                                                //+ ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro 
                                                //&& x.CedulaID == idCedulaA && x.Activo == 1).Select(x => x.Valor).AsParallel().SingleOrDefault();
                                                Sam3_Espesor espesorS3 = ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro && x.CedulaID == idCedulaB
                                                    && x.Valor != EspesorMM && x.Activo == 1).AsParallel().SingleOrDefault();
                                                espesorS3.FechaModificacion = DateTime.Now;
                                                espesorS3.Valor = EspesorMM;
                                                ctx.SaveChanges();

                                                int S2espesorID = (from eq in ctx.Sam3_EquivalenciaEspesor
                                                                   where eq.Activo && eq.Sam3_EspesorID == espesorS3.EspesorID
                                                                   select eq.Sam2_EspesorID).AsParallel().SingleOrDefault();

                                                Espesor espesorSam2 = ctx2.Espesor.Where(x => x.EspesorID == S2espesorID).AsParallel().SingleOrDefault();
                                                espesorSam2.Valor = EspesorMM;
                                                espesorSam2.FechaModificacion = DateTime.Now;
                                                ctx2.SaveChanges();
                                            }
                                        }
                                    }

                                    if (!String.IsNullOrEmpty(item.CedulaC) && item.Correcta)
                                    {
                                        if (!ctx.Sam3_Cedula.Where(x => x.Codigo == item.CedulaC && x.Activo).Any())
                                        {
                                            //Inserto la cedula C
                                            Sam3_Cedula nuevaCedula = new Sam3_Cedula();
                                            nuevaCedula.Activo = true;
                                            nuevaCedula.Codigo = item.CedulaC;
                                            nuevaCedula.FechaModificacion = DateTime.Now;
                                            nuevaCedula.UsuarioModificacion = usuario.UsuarioID;

                                            ctx.Sam3_Cedula.Add(nuevaCedula);
                                            ctx.SaveChanges();

                                            DatabaseManager.Sam2.Cedula nuevaCedulaSAM2 = new DatabaseManager.Sam2.Cedula();
                                            if (!ctx2.Cedula.Where(x => x.Codigo == item.CedulaC).Any())
                                            {
                                                nuevaCedulaSAM2.Codigo = item.CedulaC;
                                                nuevaCedulaSAM2.FechaModificacion = DateTime.Now;
                                                nuevaCedulaSAM2.VerificadoPorCalidad = false;

                                                ctx2.Cedula.Add(nuevaCedulaSAM2);
                                                ctx2.SaveChanges();
                                            }
                                            else
                                            {
                                                nuevaCedulaSAM2 = ctx2.Cedula.Where(x => x.Codigo == item.CedulaC).SingleOrDefault();
                                            }

                                            Sam3_EquivalenciaCedula equivalencia = new Sam3_EquivalenciaCedula();
                                            equivalencia.Activo = true;
                                            equivalencia.FechaModificacion = DateTime.Now;
                                            equivalencia.Sam2_CedulaID = nuevaCedulaSAM2.CedulaID;
                                            equivalencia.Sam3_CedulaID = nuevaCedula.CedulaID;
                                            equivalencia.UsuarioModificacion = usuario.UsuarioID;

                                            ctx.Sam3_EquivalenciaCedula.Add(equivalencia);
                                            ctx.SaveChanges();
                                        }

                                        idCedulaC = ctx.Sam3_Cedula.Where(x => x.Codigo == item.CedulaC && x.Activo).Select(x => x.CedulaID).AsParallel().SingleOrDefault();
                                        int eqCedulaC = ctx.Sam3_EquivalenciaCedula.Where(x => x.Sam3_CedulaID == idCedulaC && x.Activo).Select(x => x.Sam2_CedulaID).AsParallel().SingleOrDefault();

                                        if (!ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro && x.CedulaID == idCedulaC && x.Activo == 1).Any())
                                        {
                                            //Inserto el espesor
                                            Sam3_Espesor nuevoEspesor = new Sam3_Espesor();
                                            nuevoEspesor.Activo = 1;
                                            nuevoEspesor.FechaModificacion = DateTime.Now;
                                            nuevoEspesor.UsuarioModificacion = usuario.UsuarioID;
                                            nuevoEspesor.CedulaID = idCedulaC;
                                            nuevoEspesor.DiametroID = idDiametro;
                                            nuevoEspesor.Valor = Convert.ToDecimal(item.CedulaMM);

                                            ctx.Sam3_Espesor.Add(nuevoEspesor);
                                            ctx.SaveChanges();

                                            Espesor nuevoEspesorSAM2 = new Espesor();
                                            if (!ctx2.Espesor.Where(x => x.DiametroID == eqDiametro.Value && x.CedulaID == eqCedulaC).Any())
                                            {
                                                nuevoEspesorSAM2.DiametroID = (int)eqDiametro;
                                                nuevoEspesorSAM2.CedulaID = eqCedulaC;
                                                nuevoEspesorSAM2.Valor = Convert.ToDecimal(item.CedulaMM);
                                                nuevoEspesorSAM2.FechaModificacion = DateTime.Now;

                                                ctx2.Espesor.Add(nuevoEspesorSAM2);
                                                ctx2.SaveChanges();
                                            }
                                            else
                                            {
                                                nuevoEspesorSAM2 = ctx2.Espesor.Where(x => x.DiametroID == eqDiametro.Value && x.CedulaID == eqCedulaC).SingleOrDefault();
                                            }

                                            Sam3_EquivalenciaEspesor equivalencia = new Sam3_EquivalenciaEspesor();
                                            equivalencia.Activo = true;
                                            equivalencia.FechaModificacion = DateTime.Now;
                                            equivalencia.UsuarioModificacion = usuario.UsuarioID;
                                            equivalencia.Sam2_EspesorID = nuevoEspesorSAM2.EspesorID;
                                            equivalencia.Sam3_EspesorID = nuevoEspesor.EspesorID;

                                            ctx.Sam3_EquivalenciaEspesor.Add(equivalencia);
                                            ctx.SaveChanges();
                                        }
                                        else
                                        {
                                            if (ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro && x.CedulaID == idCedulaC && x.Valor != EspesorMM && x.Activo == 1).Any())
                                            {
                                                //item.Correcta = false;
                                                //item.MensajeError = "Ya existe la cédula " + item.CedulaA + ", diámetro " + item.Diametro1 + " pero con un espesor " 
                                                //+ ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro 
                                                //&& x.CedulaID == idCedulaA && x.Activo == 1).Select(x => x.Valor).AsParallel().SingleOrDefault();
                                                Sam3_Espesor espesorS3 = ctx.Sam3_Espesor.Where(x => x.DiametroID == idDiametro && x.CedulaID == idCedulaC
                                                    && x.Valor != EspesorMM && x.Activo == 1).AsParallel().SingleOrDefault();
                                                espesorS3.FechaModificacion = DateTime.Now;
                                                espesorS3.Valor = EspesorMM;
                                                ctx.SaveChanges();

                                                int S2espesorID = (from eq in ctx.Sam3_EquivalenciaEspesor
                                                                   where eq.Activo && eq.Sam3_EspesorID == espesorS3.EspesorID
                                                                   select eq.Sam2_EspesorID).AsParallel().SingleOrDefault();

                                                Espesor espesorSam2 = ctx2.Espesor.Where(x => x.EspesorID == S2espesorID).AsParallel().SingleOrDefault();
                                                espesorSam2.Valor = EspesorMM;
                                                espesorSam2.FechaModificacion = DateTime.Now;
                                                ctx2.SaveChanges();
                                            }
                                        }
                                    }

                                    idCedulaA = ctx.Sam3_Cedula.Where(x => x.Codigo == item.CedulaA && x.Activo).Select(x => x.CedulaID).AsParallel().SingleOrDefault();
                                    idCedulaB = ctx.Sam3_Cedula.Where(x => x.Codigo == item.CedulaB && x.Activo).Select(x => x.CedulaID).AsParallel().SingleOrDefault();
                                    idCedulaC = ctx.Sam3_Cedula.Where(x => x.Codigo == item.CedulaC && x.Activo).Select(x => x.CedulaID).AsParallel().SingleOrDefault();
                                    EspesorMM = Convert.ToDecimal(item.CedulaMM);



                                    int cedulaDefault = (from c in ctx.Sam3_Cedula
                                                         where c.Activo && c.Codigo == "-"
                                                         select c.CedulaID).AsParallel().SingleOrDefault();

                                    bool existeEspesor = (from esp in ctx.Sam3_Espesor
                                                          where esp.Activo == 1
                                                          && esp.DiametroID == idDiametro
                                                          && esp.Valor == EspesorMM
                                                          select esp).Any();



                                    if (!existeEspesor)
                                    {
                                        Sam3_Espesor espesorSam3 = new Sam3_Espesor
                                        {
                                            Activo = 1,
                                            Valor = EspesorMM,
                                            DiametroID = idDiametro,
                                            FechaModificacion = DateTime.Now,
                                            UsuarioModificacion = usuario.UsuarioID
                                        };

                                        ctx.Sam3_Espesor.Add(espesorSam3);
                                        ctx.SaveChanges();
                                        int sam2CedulaDefault = (from ced in ctx2.Cedula
                                                                 where ced.Codigo == "-"
                                                                 select ced.CedulaID).AsParallel().Distinct().SingleOrDefault();
                                        Espesor espesorSam2 = new Espesor();
                                        if (!ctx2.Espesor.Where(x => x.DiametroID == eqDiametro && x.Valor == EspesorMM && x.CedulaID == sam2CedulaDefault).Any())
                                        {
                                            espesorSam2 = new Espesor
                                            {
                                                CedulaID = sam2CedulaDefault,
                                                Valor = EspesorMM,
                                                DiametroID = eqDiametro.Value,
                                                FechaModificacion = DateTime.Now
                                            };

                                            ctx2.Espesor.Add(espesorSam2);
                                            ctx2.SaveChanges();
                                        }
                                        else
                                        {
                                            espesorSam2 = ctx2.Espesor.Where(x => x.DiametroID == eqDiametro && x.Valor == EspesorMM && x.CedulaID == sam2CedulaDefault).SingleOrDefault();
                                        }

                                        Sam3_EquivalenciaEspesor equivalencia = new Sam3_EquivalenciaEspesor
                                        {
                                            Activo = true,
                                            FechaModificacion = DateTime.Now,
                                            Sam2_EspesorID = espesorSam2.EspesorID,
                                            Sam3_EspesorID = espesorSam3.EspesorID,
                                            UsuarioModificacion = usuario.UsuarioID
                                        };
                                        ctx.Sam3_EquivalenciaEspesor.Add(equivalencia);
                                        ctx.SaveChanges();
                                    }

                                    int cedulaOriginal = item.CedulaID != "" && item.CedulaID != null ? Convert.ToInt32(item.CedulaID) : 0;
                                    bool existente = cedulaOriginal > 0 ? true : false;

                                    if (!existente)
                                    {
                                        nuevoElemento = new Sam3_CatalogoCedulas();
                                        nuevoElemento.DiametroID = idDiametro;
                                        if (idCedulaA > 0)
                                        {
                                            nuevoElemento.CedulaA = idCedulaA;
                                        }
                                        else
                                        {
                                            nuevoElemento.CedulaA = 0;
                                        }
                                        if (idCedulaB > 0)
                                        {
                                            nuevoElemento.CedulaB = idCedulaB;
                                        }
                                        else
                                        {
                                            nuevoElemento.CedulaB = 0;
                                        }
                                        if (idCedulaC > 0)
                                        {
                                            nuevoElemento.CedulaC = idCedulaC;
                                        }
                                        else
                                        {
                                            nuevoElemento.CedulaC = 0;
                                        }
                                        nuevoElemento.EspesorMM = EspesorMM;
                                        nuevoElemento.EspesorIn = Convert.ToDecimal(item.CedulaIn);
                                        nuevoElemento.Activo = true;
                                        nuevoElemento.FechaModificacion = DateTime.Now;
                                        nuevoElemento.UsuarioModificacion = usuario.UsuarioID;

                                        ctx.Sam3_CatalogoCedulas.Add(nuevoElemento);
                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        nuevoElemento = (from cat in ctx.Sam3_CatalogoCedulas
                                                         where cat.Activo
                                                         && cat.CatalogoCedulasID == cedulaOriginal
                                                         select cat).AsParallel().SingleOrDefault();

                                        nuevoElemento.DiametroID = idDiametro;
                                        if (idCedulaA > 0 || idCedulaA != nuevoElemento.CedulaA)
                                        {
                                            nuevoElemento.CedulaA = idCedulaA;
                                        }
                                        else
                                        {
                                            nuevoElemento.CedulaA = 0;
                                        }
                                        if (idCedulaB > 0 || idCedulaB != nuevoElemento.CedulaB)
                                        {
                                            nuevoElemento.CedulaB = idCedulaB;
                                        }
                                        {
                                            nuevoElemento.CedulaB = 0;
                                        }
                                        if (idCedulaC > 0 || idCedulaC != nuevoElemento.CedulaC)
                                        {
                                            nuevoElemento.CedulaC = idCedulaC;
                                        }
                                        else
                                        {
                                            nuevoElemento.CedulaC = 0;
                                        }
                                        if (EspesorMM != nuevoElemento.EspesorMM)
                                        {
                                            nuevoElemento.EspesorMM = EspesorMM;
                                        }
                                        nuevoElemento.EspesorIn = Convert.ToDecimal(item.CedulaIn);
                                        nuevoElemento.Activo = true;
                                        nuevoElemento.FechaModificacion = DateTime.Now;
                                        nuevoElemento.UsuarioModificacion = usuario.UsuarioID;

                                        ctx.SaveChanges();
                                    }

                                    cedulasCorrectas.Add(new CatalogoCedulas
                                    {
                                        Correcta = item.Correcta,
                                        Diametro1 = item.Diametro1,
                                        CedulaID = nuevoElemento == null ? "0" : nuevoElemento.CatalogoCedulasID.ToString(),
                                        CedulaA = item.CedulaA,
                                        CedulaB = item.CedulaB,
                                        CedulaC = item.CedulaC,
                                        CedulaIn = item.CedulaIn.ToString(),
                                        CedulaMM = item.CedulaMM.ToString(),
                                        MensajeError = item.MensajeError,
                                        CatalogoCedulaID = nuevoElemento.CatalogoCedulasID
                                        //Espesor = cedula.Espesor.ToString()
                                    });

                                    if (item.Correcta)
                                    {

                                        sam2_tran.Commit();
                                        sam3_tran.Commit();
                                    }
                                }
                            }
                        }
                    }
                }//foreach
                return cedulasCorrectas;
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

        /// <summary>
        /// Funcion para validar si una cedula tiene conflictos
        /// Que afecte 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="catalogoID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        //public object validarCedulas(string data, string catalogoID, Sam3_Usuario usuario)
        //{
        //    try
        //    {
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        List<CatalogoCedulas> catalogoCedulas = serializer.Deserialize<List<CatalogoCedulas>>(data);
        //        List<CatalogoCedulas> cedulasEnBD = new List<CatalogoCedulas>();
        //        ValidarCedulas datos = new ValidarCedulas();
        //        datos.HayConflictos = false;

        //        using (SamContext ctx = new SamContext())
        //        {
        //            foreach (CatalogoCedulas item in catalogoCedulas)
        //            {
        //                cedulasEnBD = (from c in ctx.Sam3_Cedula
        //                               where c.Activo && ((c.CedulaA == item.CedulaA ||
        //                          c.CedulaB == item.CedulaB ||
        //                          c.CedulaC == item.CedulaC) &&
        //                          c.DiametroID.ToString() == item.Diametro1) ||
        //                          ((c.CedulaA == item.CedulaA ||
        //                          c.CedulaB == item.CedulaB ||
        //                          c.CedulaC == item.CedulaC) &&
        //                          String.IsNullOrEmpty(c.DiametroID.ToString()))
        //                               select new CatalogoCedulas
        //                               {
        //                                   CedulaID = c.CedulaID.ToString(),
        //                                   Diametro1ID = c.DiametroID.ToString(),
        //                                   CedulaA = c.CedulaA,
        //                                   CedulaB = c.CedulaB,
        //                                   CedulaC = c.CedulaC,
        //                                   CedulaIn = c.CedulaIn.ToString(),
        //                                   CedulaMM = c.CedulaMM.ToString(),
        //                                   //Espesor = c.Espesor.ToString()
        //                               }).AsParallel().ToList();

        //                if (cedulasEnBD.Count > 1)
        //                {
        //                    datos.HayConflictos = true;
        //                    datos.CedulasExistentes.AddRange(cedulasEnBD);
        //                    datos.CedulasNuevas.AddRange(catalogoCedulas);
        //                }
        //            }
        //            return datos;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //-----------------Agregar mensaje al Log -----------------------------------------------
        //        LoggerBd.Instance.EscribirLog(ex);
        //        //-----------------Agregar mensaje al Log -----------------------------------------------
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(ex.Message);
        //        result.ReturnCode = 500;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = true;

        //        return result;
        //    }
        //}


        /// <summary>
        /// Se obtiene el factor de conversion 
        /// para el catalogo de Cedulas
        /// </summary>
        /// <param name="catalogoID">catalogo de cedulas</param>
        /// <returns>string con el dato del factor</returns>
        public object obtenerFactorConversion(string catalogoID)
        {
            try
            {
                string factor = ConfigurationManager.AppSettings["factorConversion"];

                return factor;
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

        /// <summary>
        /// Se obtienen los datos del ICS para el Grid
        /// Pantalla Catalogo ICS
        /// </summary>
        /// <returns></returns>
        public object obtenerDatosCatalogoICS()
        {
            try
            {
                List<ICSDatosAsociacion> lista = new List<ICSDatosAsociacion>();
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        lista = (from ics in ctx.Sam3_ItemCodeSteelgo
                                 join g in ctx.Sam3_Grupo on ics.GrupoID equals g.GrupoID
                                 join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                 //join c in ctx.Sam3_Cedula on ics.CedulaID equals c.CedulaID
                                 join diam in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on ics.ItemCodeSteelgoID equals diam.ItemCodeSteelgoID
                                 join fa in ctx.Sam3_FamiliaAcero on ics.FamiliaAceroID equals fa.FamiliaAceroID
                                 join fm in ctx.Sam3_FamiliaMaterial on fa.FamiliaMaterialID equals fm.FamiliaMaterialID
                                 //join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on ics.ItemCodeSteelgoID equals rics.ItemCodeSteelgoID
                                 join ricd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on ics.ItemCodeSteelgoID equals ricd.ItemCodeSteelgoID

                                 where ics.Activo && g.Activo && cat.Activo && diam.Activo
                                 select new ICSDatosAsociacion
                                 {
                                     ItemCodeSteelgoID = ics.ItemCodeSteelgoID.ToString(),
                                     Codigo = ics.Codigo,
                                     Descripcion = ics.DescripcionEspanol,
                                     DescripcionIngles = ics.DescripcionIngles,
                                     DescripcionLarga = ics.DescripcionLargaEspanol,
                                     DescripcionLargaIngles = ics.DescripcionLargaIngles,
                                     Rel_ICS_DiametroID = diam.Rel_ItemCodeSteelgo_Diametro_ID.ToString(),
                                     Diametro1ID = diam.Diametro1ID.ToString(),
                                     Diametro1 = (from d in ctx.Sam3_Diametro where d.Activo && d.DiametroID == diam.Diametro1ID select d.Valor.ToString()).FirstOrDefault(),
                                     Diametro2ID = g.TieneD2 == null || g.TieneD2 == false // Si no tiene D2 se muestra la opcion de cero
                                                    ? (ctx.Sam3_Diametro.Where(x => x.Activo && x.Valor == 0).FirstOrDefault().DiametroID.ToString()) : diam.Diametro2ID.ToString(),
                                     Diametro2 = g.TieneD2 == null || g.TieneD2 == false // Si no tiene D2 se muestra la opcion de cero
                                                ? (ctx.Sam3_Diametro.Where(x => x.Activo && x.Valor == 0).FirstOrDefault().Valor.ToString()) :
                                                (from d in ctx.Sam3_Diametro where d.Activo && d.DiametroID == diam.Diametro2ID select d.Valor.ToString()).FirstOrDefault(),
                                     Grupo = g.Nombre,
                                     GrupoID = ics.GrupoID.ToString(),
                                     FamiliaMaterialID = fm.FamiliaMaterialID.ToString(),
                                     FamiliaMaterial = fm.Nombre,
                                     AceroID = ics.FamiliaAceroID.ToString(),
                                     CedulaID = ics.CedulaID.ToString(),
                                     CedulaA = (from ced in ctx.Sam3_Cedula
                                                where ced.CedulaID == cat.CedulaA && ced.Activo
                                                select ced.Codigo
                                                    ).FirstOrDefault(),
                                     CedulaB = (from ced in ctx.Sam3_Cedula
                                                where ced.CedulaID == cat.CedulaB && ced.Activo
                                                select ced.Codigo
                                                    ).FirstOrDefault(),
                                     Libra = (from ced in ctx.Sam3_Cedula
                                              where ced.CedulaID == cat.CedulaC && ced.Activo
                                              select ced.Codigo
                                                    ).FirstOrDefault(),
                                     Inch = cat.EspesorIn.ToString(),
                                     MM = cat.EspesorMM.ToString(),
                                     Peso = ics.Peso.ToString(),
                                     Area = ics.Area.ToString(),
                                     TieneD2 = g.TieneD2 == null ? "No" : g.TieneD2 == true ? "Si" : "No",
                                     Asociado = (ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo.Where(x => x.Rel_ItemCodeSteelgo_Diametro_ID == ricd.Rel_ItemCodeSteelgo_Diametro_ID && x.Activo).Any())
                                 }).AsParallel().ToList();

                        lista.ForEach(x =>
                        {
                            int equiv = (from eq in ctx.Sam3_EquivalenciaFamiliaAcero
                                         where eq.Activo && eq.Sam3_FamiliaAceroID.ToString() == x.AceroID
                                         select eq.Sam2_FamiliaAceroID).AsParallel().SingleOrDefault();

                            x.Acero = (from fa in ctx2.FamiliaAcero
                                       where fa.FamiliaAceroID == equiv
                                       select fa.Nombre).AsParallel().SingleOrDefault();
                        });
                    }
                }

                return lista;
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

        /// <summary>
        /// Guarda la informacion del ICS
        /// Pantalla Catalogo ICS
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public object guardarItemCodeSteelgo(ICSDatosAsociacion datos, Sam3_Usuario usuario)
        {
            try
            {
                Sam3_Rel_ItemCodeSteelgo_Diametro nuevo = new Sam3_Rel_ItemCodeSteelgo_Diametro();
                //Sam3_Diametro diametro1 = new Sam3_Diametro();
                //Sam3_Diametro diametro2 = new Sam3_Diametro();
                Sam3_ItemCodeSteelgo ICSteelgo = new Sam3_ItemCodeSteelgo();

                using (SamContext ctx = new SamContext())
                {
                    using (var sam3_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var sam2_tran = ctx2.Database.BeginTransaction())
                            {
                                decimal diametro1 = Convert.ToDecimal(datos.Diametro1);
                                decimal diametro2 = Convert.ToDecimal(datos.Diametro2);
                                //Insertamos diametro 1 en sam 3
                                if (!ctx.Sam3_Diametro.Where(x => x.Valor == diametro1 && x.Activo).Any())
                                {
                                    Sam3_Diametro diam1 = new Sam3_Diametro();
                                    Diametro diam1Sam2 = new Diametro();

                                    diam1.Valor = Convert.ToDecimal(datos.Diametro1);
                                    diam1.VerificadoPorCalidad = true;
                                    diam1.Activo = true;
                                    diam1.UsuarioModificacion = usuario.UsuarioID;
                                    diam1.FechaModificacion = DateTime.Now;
                                    ctx.Sam3_Diametro.Add(diam1);
                                    ctx.SaveChanges();
                                    datos.Diametro1ID = diam1.DiametroID.ToString();

                                    //Se inserta el espesor para el nuevo diametro
                                    decimal espesorMM = Convert.ToDecimal(datos.MM);
                                    Sam3_Espesor espesor = new Sam3_Espesor();
                                    espesor.CedulaID = null;
                                    espesor.DiametroID = Convert.ToInt32(datos.Diametro1ID);
                                    espesor.Valor = Convert.ToDecimal(datos.MM);
                                    espesor.Activo = 1;
                                    espesor.UsuarioModificacion = usuario.UsuarioID;
                                    espesor.FechaModificacion = DateTime.Now;
                                    ctx.Sam3_Espesor.Add(espesor);
                                    ctx.SaveChanges();
                                }

                                //Insertamos diametro 2 en Sam3
                                if (!ctx.Sam3_Diametro.Where(x => x.Valor == diametro2 && x.Activo).Any())
                                {
                                    Sam3_Diametro diam2 = new Sam3_Diametro();
                                    Diametro diam2Sam2 = new Diametro();

                                    diam2.Valor = Convert.ToDecimal(datos.Diametro2);
                                    diam2.VerificadoPorCalidad = true;
                                    diam2.Activo = true;
                                    diam2.UsuarioModificacion = usuario.UsuarioID;
                                    diam2.FechaModificacion = DateTime.Now;
                                    ctx.Sam3_Diametro.Add(diam2);
                                    ctx.SaveChanges();
                                    datos.Diametro2ID = diam2.DiametroID.ToString();
                                }

                                //Insertar cedula en caso de que solo haya capturado el espesor
                                if (String.IsNullOrEmpty(datos.CedulaA) && String.IsNullOrEmpty(datos.CedulaB) && String.IsNullOrEmpty(datos.Libra))
                                {
                                    if (!ctx.Sam3_CatalogoCedulas.Where(x => x.DiametroID.ToString() == datos.Diametro1ID && x.EspesorIn.ToString() == datos.Inch
                                        && x.EspesorMM.ToString() == datos.MM && x.CedulaA == 0 && x.CedulaB == 0 && x.CedulaC == 0 && x.Activo).Any())
                                    {
                                        Sam3_CatalogoCedulas cat = new Sam3_CatalogoCedulas();
                                        cat.DiametroID = Convert.ToInt32(datos.Diametro1ID);
                                        cat.CedulaA = 0;
                                        cat.CedulaB = 0;
                                        cat.CedulaC = 0;
                                        cat.EspesorIn = Convert.ToDecimal(datos.Inch);
                                        cat.EspesorMM = Convert.ToDecimal(datos.MM);
                                        cat.Activo = true;
                                        cat.UsuarioModificacion = usuario.UsuarioID;
                                        cat.FechaModificacion = DateTime.Now;
                                        ctx.Sam3_CatalogoCedulas.Add(cat);
                                        ctx.SaveChanges();

                                        datos.CedulaID = cat.CatalogoCedulasID.ToString();

                                        //Verificamos si existe el espesor
                                        int sam3_diametroID = Convert.ToInt32(datos.Diametro1ID);
                                        decimal mm = Convert.ToDecimal(datos.MM);
                                        int? eqDiametro = (from eq in ctx.Sam3_EquivalenciaDiametro
                                                           where eq.Activo && eq.Sam3_DiametroID == sam3_diametroID
                                                           select eq.Sam2_DiametroID).AsParallel().SingleOrDefault();

                                        bool existeEspesor = (from esp in ctx.Sam3_Espesor
                                                              where esp.Activo == 1
                                                              && esp.DiametroID == sam3_diametroID
                                                              && esp.Valor == mm
                                                              select esp).Any();



                                        if (!existeEspesor)
                                        {
                                            Sam3_Espesor espesorSam3 = new Sam3_Espesor
                                            {
                                                Activo = 1,
                                                Valor = mm,
                                                DiametroID = sam3_diametroID,
                                                FechaModificacion = DateTime.Now,
                                                UsuarioModificacion = usuario.UsuarioID
                                            };

                                            ctx.Sam3_Espesor.Add(espesorSam3);
                                            ctx.SaveChanges();
                                            int sam2CedulaDefault = (from ced in ctx2.Cedula
                                                                     where ced.Codigo == "-"
                                                                     select ced.CedulaID).AsParallel().Distinct().SingleOrDefault();

                                            Espesor espesorSam2 = new Espesor();
                                            if (!ctx2.Espesor.Where(x => x.DiametroID == eqDiametro && x.Valor == mm && x.CedulaID == sam2CedulaDefault).Any())
                                            {
                                                espesorSam2 = new Espesor
                                                {
                                                    CedulaID = sam2CedulaDefault,
                                                    Valor = mm,
                                                    DiametroID = eqDiametro.Value,
                                                    FechaModificacion = DateTime.Now
                                                };

                                                ctx2.Espesor.Add(espesorSam2);
                                                ctx2.SaveChanges();
                                            }
                                            else
                                            {
                                                espesorSam2 = ctx2.Espesor.Where(x => x.DiametroID == eqDiametro && x.Valor == mm && x.CedulaID == sam2CedulaDefault).SingleOrDefault();
                                            }

                                            Sam3_EquivalenciaEspesor equivalencia = new Sam3_EquivalenciaEspesor
                                            {
                                                Activo = true,
                                                FechaModificacion = DateTime.Now,
                                                Sam2_EspesorID = espesorSam2.EspesorID,
                                                Sam3_EspesorID = espesorSam3.EspesorID,
                                                UsuarioModificacion = usuario.UsuarioID
                                            };
                                            ctx.Sam3_EquivalenciaEspesor.Add(equivalencia);
                                            ctx.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        datos.CedulaID = ctx.Sam3_CatalogoCedulas
                                            .Where(x => x.DiametroID.ToString() == datos.Diametro1ID && x.EspesorIn.ToString() == datos.Inch && x.Activo)
                                            .Select(x => x.CatalogoCedulasID.ToString()).AsParallel().FirstOrDefault();
                                    }
                                }

                                //Insertamos item code steelgo
                                if (!ctx.Sam3_ItemCodeSteelgo.Where(x => x.Codigo == datos.Codigo && x.Activo).Any())
                                {
                                    ICSteelgo.Codigo = datos.Codigo;
                                    ICSteelgo.DescripcionEspanol = datos.Descripcion;
                                    ICSteelgo.DescripcionLargaEspanol = datos.DescripcionLarga;
                                    ICSteelgo.DescripcionLargaIngles = datos.DescripcionLargaIngles;
                                    ICSteelgo.DescripcionIngles = datos.DescripcionIngles;
                                    ICSteelgo.GrupoID = Convert.ToInt32(datos.GrupoID);
                                    ICSteelgo.CedulaID = Convert.ToInt32(datos.CedulaID);
                                    ICSteelgo.Peso = Convert.ToDecimal(datos.Peso);
                                    ICSteelgo.Area = Convert.ToDecimal(datos.Area);
                                    ICSteelgo.FamiliaAceroID = Convert.ToInt32(datos.AceroID);
                                    ICSteelgo.Activo = true;
                                    ICSteelgo.UsuarioModificacion = usuario.UsuarioID;
                                    ICSteelgo.FechaModificacion = DateTime.Now;

                                    ctx.Sam3_ItemCodeSteelgo.Add(ICSteelgo);

                                    ctx.SaveChanges();

                                    //Se crea la relacion ics-diametro
                                    if (!ctx.Sam3_Rel_ItemCodeSteelgo_Diametro.Where(x => x.Activo &&
                                        x.Diametro1ID.ToString() == datos.Diametro1ID &&
                                        x.Diametro2ID.ToString() == datos.Diametro2ID &&
                                        x.ItemCodeSteelgoID == ICSteelgo.ItemCodeSteelgoID).Any())
                                    {
                                        nuevo.ItemCodeSteelgoID = ICSteelgo.ItemCodeSteelgoID;
                                        nuevo.Diametro1ID = Convert.ToInt32(datos.Diametro1ID);
                                        nuevo.Diametro2ID = Convert.ToInt32(datos.Diametro2ID);
                                        nuevo.Activo = true;
                                        nuevo.FechaModificacion = DateTime.Now;
                                        nuevo.UsuarioModificacion = usuario.UsuarioID;

                                        ctx.Sam3_Rel_ItemCodeSteelgo_Diametro.Add(nuevo);
                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        nuevo = ctx.Sam3_Rel_ItemCodeSteelgo_Diametro.Where(x => x.Activo &&
                                         x.Diametro1ID.ToString() == datos.Diametro1ID &&
                                         x.Diametro2ID.ToString() == datos.Diametro2ID &&
                                         x.ItemCodeSteelgoID == ICSteelgo.ItemCodeSteelgoID).AsParallel().SingleOrDefault();
                                    }

                                    //diametro1.Valor = ctx.Sam3_Diametro.Where(x => x.DiametroID.ToString() == datos.Diametro1).Select(x => x.Valor).AsParallel().SingleOrDefault();
                                    //diametro2.Valor = ctx.Sam3_Diametro.Where(x => x.DiametroID.ToString() == datos.Diametro2).Select(x => x.Valor).AsParallel().SingleOrDefault();

                                    sam3_tran.Commit();
                                }
                                else
                                {
                                    throw new Exception("El ItemCode Steelgo capturado ya existe en el catálogo");
                                }
                            }
                            return new ICSDatosAsociacion
                            {
                                ItemCodeSteelgoID = ICSteelgo.ItemCodeSteelgoID.ToString(),
                                Codigo = ICSteelgo.Codigo,
                                Descripcion = ICSteelgo.DescripcionEspanol,
                                DescripcionLarga = ICSteelgo.DescripcionLargaEspanol,
                                DescripcionIngles = ICSteelgo.DescripcionIngles,
                                DescripcionLargaIngles = ICSteelgo.DescripcionLargaIngles,
                                Rel_ICS_DiametroID = nuevo.Rel_ItemCodeSteelgo_Diametro_ID.ToString(),
                                Diametro1ID = nuevo.Diametro1ID.ToString(),
                                Diametro2ID = nuevo.Diametro2ID.ToString(),
                                Diametro1 = datos.Diametro1,
                                Diametro2 = datos.Diametro2,
                                Grupo = datos.Grupo,
                                GrupoID = datos.GrupoID,
                                AceroID = datos.AceroID,
                                FamiliaMaterial = datos.FamiliaMaterial,
                                FamiliaMaterialID = "0",
                                Acero = datos.Acero,
                                CedulaID = datos.CedulaID,
                                CedulaA = datos.CedulaA,
                                CedulaB = datos.CedulaB,
                                Libra = datos.Libra,
                                Inch = datos.Inch,
                                MM = datos.MM,
                                Espesor = datos.Espesor,
                                Peso = ICSteelgo.Peso.ToString(),
                                Area = ICSteelgo.Area.ToString(),
                                TieneD2 = datos.TieneD2
                            };
                        }
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

        /// <summary>
        /// Funcion para editar un item Code Steelgo
        /// Catalogo item code steelgo
        /// </summary>
        /// <param name="datos">datos a editar</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns>datos editados</returns>
        public object editarItemCodeSteelgo(ICSDatosAsociacion datos, Sam3_Usuario usuario, int editado)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        //Sam3_Diametro diametro1 = new Sam3_Diametro();
                        //Sam3_Diametro diametro2 = new Sam3_Diametro();
                        Sam3_ItemCodeSteelgo ics = new Sam3_ItemCodeSteelgo();
                        Sam3_Rel_ItemCodeSteelgo_Diametro icsDiam = new Sam3_Rel_ItemCodeSteelgo_Diametro();
                        using (var sam3_tran = ctx.Database.BeginTransaction())
                        {
                            if (editado == 1)
                            {
                                #region Insertar cedula no existente
                                decimal diametro1 = Convert.ToDecimal(datos.Diametro1);
                                decimal diametro2 = Convert.ToDecimal(datos.Diametro2);
                                //Insertamos diametro 1 en sam 3
                                if (!ctx.Sam3_Diametro.Where(x => x.Valor == diametro1 && x.Activo).Any())
                                {
                                    Sam3_Diametro diam1 = new Sam3_Diametro();
                                    Diametro diam1Sam2 = new Diametro();

                                    diam1.Valor = Convert.ToDecimal(datos.Diametro1);
                                    diam1.VerificadoPorCalidad = true;
                                    diam1.Activo = true;
                                    diam1.UsuarioModificacion = usuario.UsuarioID;
                                    diam1.FechaModificacion = DateTime.Now;
                                    ctx.Sam3_Diametro.Add(diam1);
                                    ctx.SaveChanges();
                                    datos.Diametro1ID = diam1.DiametroID.ToString();

                                    //Se inserta el espesor para el nuevo diametro
                                    decimal espesorMM = Convert.ToDecimal(datos.MM);
                                    Sam3_Espesor espesor = new Sam3_Espesor();
                                    espesor.CedulaID = null;
                                    espesor.DiametroID = Convert.ToInt32(datos.Diametro1ID);
                                    espesor.Valor = Convert.ToDecimal(datos.MM);
                                    espesor.Activo = 1;
                                    espesor.UsuarioModificacion = usuario.UsuarioID;
                                    espesor.FechaModificacion = DateTime.Now;
                                    ctx.Sam3_Espesor.Add(espesor);
                                    ctx.SaveChanges();
                                }

                                //Insertamos diametro 2 en Sam3
                                if (!ctx.Sam3_Diametro.Where(x => x.Valor == diametro2 && x.Activo).Any())
                                {
                                    Sam3_Diametro diam2 = new Sam3_Diametro();
                                    Diametro diam2Sam2 = new Diametro();

                                    diam2.Valor = Convert.ToDecimal(datos.Diametro2);
                                    diam2.VerificadoPorCalidad = true;
                                    diam2.Activo = true;
                                    diam2.UsuarioModificacion = usuario.UsuarioID;
                                    diam2.FechaModificacion = DateTime.Now;
                                    ctx.Sam3_Diametro.Add(diam2);
                                    ctx.SaveChanges();
                                    datos.Diametro2ID = diam2.DiametroID.ToString();
                                }

                                //Insertar cedula en caso de que solo haya capturado el espesor
                                if (String.IsNullOrEmpty(datos.CedulaA) && String.IsNullOrEmpty(datos.CedulaB) && String.IsNullOrEmpty(datos.Libra))
                                {
                                    if (!ctx.Sam3_CatalogoCedulas.Where(x => x.DiametroID.ToString() == datos.Diametro1ID && x.EspesorIn.ToString() == datos.Inch
                                        && x.EspesorMM.ToString() == datos.MM && x.CedulaA == 0 && x.CedulaB == 0 && x.CedulaC == 0 && x.Activo).Any())
                                    {
                                        Sam3_CatalogoCedulas cat = new Sam3_CatalogoCedulas();
                                        cat.DiametroID = Convert.ToInt32(datos.Diametro1ID);
                                        cat.CedulaA = 0;
                                        cat.CedulaB = 0;
                                        cat.CedulaC = 0;
                                        cat.EspesorIn = Convert.ToDecimal(datos.Inch);
                                        cat.EspesorMM = Convert.ToDecimal(datos.MM);
                                        cat.Activo = true;
                                        cat.UsuarioModificacion = usuario.UsuarioID;
                                        cat.FechaModificacion = DateTime.Now;
                                        ctx.Sam3_CatalogoCedulas.Add(cat);
                                        ctx.SaveChanges();

                                        datos.CedulaID = cat.CatalogoCedulasID.ToString();

                                        //Verificamos si existe el espesor
                                        int sam3_diametroID = Convert.ToInt32(datos.Diametro1ID);
                                        decimal mm = Convert.ToDecimal(datos.MM);
                                        int? eqDiametro = (from eq in ctx.Sam3_EquivalenciaDiametro
                                                           where eq.Activo && eq.Sam3_DiametroID == sam3_diametroID
                                                           select eq.Sam2_DiametroID).AsParallel().SingleOrDefault();

                                        bool existeEspesor = (from esp in ctx.Sam3_Espesor
                                                              where esp.Activo == 1
                                                              && esp.DiametroID == sam3_diametroID
                                                              && esp.Valor == mm
                                                              select esp).Any();



                                        if (!existeEspesor)
                                        {
                                            Sam3_Espesor espesorSam3 = new Sam3_Espesor
                                            {
                                                Activo = 1,
                                                Valor = mm,
                                                DiametroID = sam3_diametroID,
                                                FechaModificacion = DateTime.Now,
                                                UsuarioModificacion = usuario.UsuarioID
                                            };

                                            ctx.Sam3_Espesor.Add(espesorSam3);
                                            ctx.SaveChanges();
                                            int sam2CedulaDefault = (from ced in ctx2.Cedula
                                                                     where ced.Codigo == "-"
                                                                     select ced.CedulaID).AsParallel().Distinct().SingleOrDefault();

                                            Espesor espesorSam2 = new Espesor();
                                            if (!ctx2.Espesor.Where(x => x.DiametroID == eqDiametro && x.Valor == mm && x.CedulaID == sam2CedulaDefault).Any())
                                            {
                                                espesorSam2 = new Espesor
                                                {
                                                    CedulaID = sam2CedulaDefault,
                                                    Valor = mm,
                                                    DiametroID = eqDiametro.Value,
                                                    FechaModificacion = DateTime.Now
                                                };

                                                ctx2.Espesor.Add(espesorSam2);
                                                ctx2.SaveChanges();
                                            }
                                            else
                                            {
                                                espesorSam2 = ctx2.Espesor.Where(x => x.DiametroID == eqDiametro && x.Valor == mm && x.CedulaID == sam2CedulaDefault).SingleOrDefault();
                                            }

                                            Sam3_EquivalenciaEspesor equivalencia = new Sam3_EquivalenciaEspesor
                                            {
                                                Activo = true,
                                                FechaModificacion = DateTime.Now,
                                                Sam2_EspesorID = espesorSam2.EspesorID,
                                                Sam3_EspesorID = espesorSam3.EspesorID,
                                                UsuarioModificacion = usuario.UsuarioID
                                            };
                                            ctx.Sam3_EquivalenciaEspesor.Add(equivalencia);
                                            ctx.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        datos.CedulaID = ctx.Sam3_CatalogoCedulas
                                            .Where(x => x.DiametroID.ToString() == datos.Diametro1ID && x.EspesorIn.ToString() == datos.Inch && x.Activo)
                                            .Select(x => x.CatalogoCedulasID.ToString()).AsParallel().FirstOrDefault();
                                    }
                                }
                                #endregion

                                if (!ctx.Sam3_ItemCodeSteelgo.Where(x => x.Codigo == datos.Codigo && x.Activo && x.ItemCodeSteelgoID.ToString() != datos.ItemCodeSteelgoID).Any())
                                {
                                    ics = ctx.Sam3_ItemCodeSteelgo.Where(x => x.ItemCodeSteelgoID.ToString() == datos.ItemCodeSteelgoID && x.Activo).AsParallel().SingleOrDefault();
                                    ics.Codigo = datos.Codigo;
                                    ics.DescripcionEspanol = datos.Descripcion;
                                    ics.DescripcionIngles = datos.DescripcionIngles;
                                    ics.DescripcionLargaEspanol = datos.DescripcionLarga;
                                    ics.DescripcionLargaIngles = datos.DescripcionLargaIngles;
                                    ics.GrupoID = Convert.ToInt32(datos.GrupoID);
                                    ics.FamiliaAceroID = Convert.ToInt32(datos.AceroID);
                                    ics.CedulaID = Convert.ToInt32(datos.CedulaID);
                                    ics.Peso = Convert.ToDecimal(datos.Peso);
                                    ics.Area = Convert.ToDecimal(datos.Area);
                                    ics.Activo = true;
                                    ics.UsuarioModificacion = usuario.UsuarioID;
                                    ics.FechaModificacion = DateTime.Now;

                                    ctx.SaveChanges();

                                    icsDiam = ctx.Sam3_Rel_ItemCodeSteelgo_Diametro.Where(x => x.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == datos.Rel_ICS_DiametroID).AsParallel().SingleOrDefault();
                                    icsDiam.Diametro1ID = Convert.ToInt32(datos.Diametro1ID);
                                    icsDiam.Diametro2ID = Convert.ToInt32(datos.Diametro2ID);
                                    icsDiam.Activo = true;
                                    icsDiam.UsuarioModificacion = usuario.UsuarioID;
                                    icsDiam.FechaModificacion = DateTime.Now;

                                    ctx.SaveChanges();


                                    //diametro1.Valor = ctx.Sam3_Diametro.Where(x => x.DiametroID.ToString() == datos.Diametro1).Select(x => x.Valor).AsParallel().SingleOrDefault();
                                    //diametro2.Valor = ctx.Sam3_Diametro.Where(x => x.DiametroID.ToString() == datos.Diametro2).Select(x => x.Valor).AsParallel().SingleOrDefault();

                                    sam3_tran.Commit();
                                }
                                else
                                {
                                    throw new Exception("El ItemCode Steelgo capturado ya existe en el catálogo");
                                }

                            }
                            else
                            {
                                if (!ctx.Sam3_ItemCodeSteelgo.Where(x => x.Codigo == datos.Codigo && x.Activo).Any())
                                {
                                    ics.Codigo = datos.Codigo;
                                    ics.DescripcionEspanol = datos.Descripcion;
                                    ics.DescripcionLargaEspanol = datos.DescripcionLarga;
                                    ics.DescripcionLargaIngles = datos.DescripcionLargaIngles;
                                    ics.DescripcionIngles = datos.DescripcionIngles;
                                    ics.GrupoID = Convert.ToInt32(datos.GrupoID);
                                    ics.CedulaID = Convert.ToInt32(datos.CedulaID);
                                    ics.Peso = Convert.ToDecimal(datos.Peso);
                                    ics.Area = Convert.ToDecimal(datos.Area);
                                    ics.FamiliaAceroID = Convert.ToInt32(datos.AceroID);
                                    ics.Activo = true;
                                    ics.UsuarioModificacion = usuario.UsuarioID;
                                    ics.FechaModificacion = DateTime.Now;

                                    ctx.Sam3_ItemCodeSteelgo.Add(ics);

                                    ctx.SaveChanges();

                                    if (!ctx.Sam3_Rel_ItemCodeSteelgo_Diametro.Where(x => x.Activo &&
                                        x.Diametro1ID.ToString() == datos.Diametro1ID &&
                                        x.Diametro2ID.ToString() == datos.Diametro2ID &&
                                        x.ItemCodeSteelgoID == ics.ItemCodeSteelgoID).Any())
                                    {
                                        icsDiam.ItemCodeSteelgoID = ics.ItemCodeSteelgoID;
                                        icsDiam.Diametro1ID = Convert.ToInt32(datos.Diametro1ID);
                                        icsDiam.Diametro2ID = Convert.ToInt32(datos.Diametro2ID);
                                        icsDiam.Activo = true;
                                        icsDiam.FechaModificacion = DateTime.Now;
                                        icsDiam.UsuarioModificacion = usuario.UsuarioID;

                                        ctx.Sam3_Rel_ItemCodeSteelgo_Diametro.Add(icsDiam);
                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        icsDiam = ctx.Sam3_Rel_ItemCodeSteelgo_Diametro.Where(x => x.Activo &&
                                         x.Diametro1ID.ToString() == datos.Diametro1ID &&
                                         x.Diametro2ID.ToString() == datos.Diametro2ID &&
                                         x.ItemCodeSteelgoID == ics.ItemCodeSteelgoID).AsParallel().SingleOrDefault();
                                    }

                                    //diametro1.Valor = ctx.Sam3_Diametro.Where(x => x.DiametroID.ToString() == datos.Diametro1).Select(x => x.Valor).AsParallel().SingleOrDefault();
                                    //diametro2.Valor = ctx.Sam3_Diametro.Where(x => x.DiametroID.ToString() == datos.Diametro2).Select(x => x.Valor).AsParallel().SingleOrDefault();

                                    sam3_tran.Commit();
                                }
                                else
                                {
                                    throw new Exception("El ItemCode Steelgo capturado ya existe en el catálogo");
                                }
                            }


                        }

                        return new ICSDatosAsociacion
                        {
                            ItemCodeSteelgoID = ics.ItemCodeSteelgoID.ToString(),
                            Codigo = ics.Codigo,
                            Descripcion = ics.DescripcionEspanol,
                            DescripcionIngles = ics.DescripcionIngles,
                            DescripcionLarga = ics.DescripcionLargaEspanol,
                            DescripcionLargaIngles = ics.DescripcionLargaIngles,
                            Diametro1ID = icsDiam.Diametro1ID.ToString(),
                            Diametro2ID = icsDiam.Diametro2ID.ToString(),
                            Diametro1 = (from d in ctx.Sam3_Diametro
                                         where d.Activo
                                         && d.DiametroID.ToString() == datos.Diametro1ID
                                         select d.Valor.ToString()).AsParallel().SingleOrDefault(),
                            Diametro2 = (from d in ctx.Sam3_Diametro
                                         where d.Activo
                                         && d.DiametroID.ToString() == datos.Diametro2ID
                                         select d.Valor.ToString()).AsParallel().SingleOrDefault(),
                            Rel_ICS_DiametroID = icsDiam.Rel_ItemCodeSteelgo_Diametro_ID.ToString(),
                            GrupoID = ics.GrupoID.ToString(),
                            Grupo = datos.Grupo,
                            Acero = datos.Acero,
                            AceroID = ics.FamiliaAceroID.ToString(),
                            CedulaA = datos.CedulaA,
                            CedulaB = datos.CedulaB,
                            CedulaID = ics.CedulaID.ToString(),
                            Libra = datos.Libra,
                            Inch = datos.Inch,
                            MM = datos.MM,
                            Espesor = datos.Espesor,
                            Peso = ics.Peso.ToString(),
                            Area = ics.Area.ToString(),
                            TieneD2 = datos.TieneD2,
                            FamiliaMaterial = datos.FamiliaMaterial,
                            FamiliaMaterialID = datos.FamiliaMaterialID,
                            Asociado = datos.Asociado
                        };
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

        /// <summary>
        /// Funcion para eliminar un item code steelgo
        /// Catalogo item code steelgo
        /// </summary>
        /// <param name="id">id del elemento a eliminar</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns></returns>
        public object EliminarItemCodeSteelgo(int idICS, int diametrosID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var sam3_tran = ctx.Database.BeginTransaction())
                    {
                        //Se elimina la relacion con diametro
                        Sam3_Rel_ItemCodeSteelgo_Diametro relDiam = ctx.Sam3_Rel_ItemCodeSteelgo_Diametro.Where(x => x.Rel_ItemCodeSteelgo_Diametro_ID == diametrosID && x.Activo).AsParallel().SingleOrDefault();
                        relDiam.Activo = false;
                        relDiam.UsuarioModificacion = usuario.UsuarioID;
                        relDiam.FechaModificacion = DateTime.Now;

                        ctx.SaveChanges();

                        //Se eliminan las relaciones con item codes
                        List<Sam3_Rel_ItemCode_ItemCodeSteelgo> lista = ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo.Where(x => x.ItemCodeSteelgoID == idICS && x.Activo).AsParallel().ToList();

                        foreach (Sam3_Rel_ItemCode_ItemCodeSteelgo item in lista)
                        {
                            item.Activo = false;
                            item.FechaModificacion = DateTime.Now;
                            item.UsuarioModificacion = usuario.UsuarioID;

                            ctx.SaveChanges();
                        }

                        //se elimina item code steelgo
                        Sam3_ItemCodeSteelgo ics = ctx.Sam3_ItemCodeSteelgo.Where(x => x.ItemCodeSteelgoID == idICS && x.Activo).AsParallel().SingleOrDefault();
                        ics.Activo = false;
                        ics.UsuarioModificacion = usuario.UsuarioID;
                        ics.FechaModificacion = DateTime.Now;

                        ctx.SaveChanges();

                        sam3_tran.Commit();
                    }

                    TransactionalInformation result = new TransactionalInformation();
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

        /// <summary>
        /// Funcion para obtener los datos de las cedulas para un item code steelgo
        /// catalogo item code steelgo
        /// </summary>
        /// <param name="datosCedulas"> </param>
        /// <returns></returns>
        public object obtenerCedulasICS(CatalogoCedulas datosCedulas)
        {
            string errorInfo = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    CatalogoCedulas cedula = new CatalogoCedulas();

                    int cedulaAID = !String.IsNullOrEmpty(datosCedulas.CedulaA) ? (from c in ctx.Sam3_Cedula where c.Activo && c.Codigo == datosCedulas.CedulaA select c.CedulaID).AsParallel().SingleOrDefault() : 0;
                    int cedulaBID = !String.IsNullOrEmpty(datosCedulas.CedulaB) ? (from c in ctx.Sam3_Cedula where c.Activo && c.Codigo == datosCedulas.CedulaB select c.CedulaID).AsParallel().SingleOrDefault() : 0;
                    int cedulaCID = !String.IsNullOrEmpty(datosCedulas.CedulaC) ? (from c in ctx.Sam3_Cedula where c.Activo && c.Codigo == datosCedulas.CedulaC select c.CedulaID).AsParallel().SingleOrDefault() : 0;
                    decimal diametroSeleccionado = Convert.ToDecimal(datosCedulas.Diametro1);
                    int diametroID = (from d in ctx.Sam3_Diametro
                                      where d.Activo && d.Valor == diametroSeleccionado
                                      select d.DiametroID).AsParallel().SingleOrDefault();
                    string espesorMm = !String.IsNullOrEmpty(datosCedulas.EspesorMM) ? datosCedulas.EspesorMM : "0";
                    string espesorIn = !String.IsNullOrEmpty(datosCedulas.EspesorIn) ? datosCedulas.EspesorIn : "0";

                    errorInfo += "\nCedulaA: " + cedulaAID.ToString() +
                        "\nCedula b: " + cedulaBID.ToString() +
                        "\nCedula c: " + cedulaCID.ToString() +
                        "\nDiametro :" + diametroID.ToString();

                    List<CatalogoCedulas> lista = (from cat in ctx.Sam3_CatalogoCedulas
                                                   where cat.Activo && cat.DiametroID == diametroID
                                                   select new CatalogoCedulas
                                                   {
                                                       CedulaID = cat.CatalogoCedulasID.ToString(),
                                                       Diametro1ID = cat.DiametroID.ToString(),
                                                       Diametro1 = (from d in ctx.Sam3_Diametro
                                                                    where d.Activo && d.DiametroID == cat.DiametroID
                                                                    select d.Valor.ToString()).FirstOrDefault(),
                                                       CedulaA = (from ced in ctx.Sam3_Cedula
                                                                  where ced.Activo && ced.CedulaID == cat.CedulaA
                                                                  select ced.Codigo).FirstOrDefault(),
                                                       CedulaB = (from ced in ctx.Sam3_Cedula
                                                                  where ced.Activo && ced.CedulaID == cat.CedulaB
                                                                  select ced.Codigo).FirstOrDefault(),
                                                       CedulaC = (from ced in ctx.Sam3_Cedula
                                                                  where ced.Activo && ced.CedulaID == cat.CedulaC
                                                                  select ced.Codigo).FirstOrDefault(),
                                                       CedulaIn = cat.EspesorIn.ToString(),
                                                       CedulaMM = cat.EspesorMM.ToString()
                                                   }).AsParallel().Distinct().ToList();

                    lista = lista.GroupBy(x => x.CedulaID).Select(x => x.First()).ToList();

                    errorInfo += "\nLista : " + serializer.Serialize(lista);

                    if (lista.Count <= 0 || lista == null)
                    {
                        errorInfo += "\nLista Vacia: CedulaA : " + cedulaAID + "CedulaB : " + cedulaBID + "CedulaC : " + cedulaCID;
                        TransactionalInformation result = new TransactionalInformation();
                        result.ReturnMessage.Add("No se encontraron coincidencias.");
                        result.ReturnCode = 500;
                        result.ReturnStatus = false;
                        result.IsAuthenicated = true;

                        return result;
                    }

                    if (String.IsNullOrEmpty(datosCedulas.CedulaA))
                    {
                        if (String.IsNullOrEmpty(datosCedulas.CedulaB))
                        {
                            if (String.IsNullOrEmpty(datosCedulas.CedulaC))
                            {
                                //abyc vacias
                                if (lista.Where(x => x.CedulaA == null && x.CedulaB == null && x.CedulaC == null).Any())
                                {
                                    if (espesorIn == "0" && espesorMm == "0")
                                    {
                                        cedula = new CatalogoCedulas();

                                        List<string> tempEspMm = (from lst in lista
                                                                  where lst.CedulaA == null
                                                                  && lst.CedulaB == null
                                                                  && lst.CedulaC == null
                                                                  select lst.CedulaMM).Distinct().ToList();

                                        cedula.EspesoresMm = (from esp in ctx.Sam3_Espesor
                                                              where esp.Activo == 1
                                                              && esp.DiametroID == diametroID
                                                              && tempEspMm.Contains(esp.Valor.ToString())
                                                              select new ListaCombos
                                                              {
                                                                  id = esp.EspesorID.ToString(),
                                                                  value = esp.Valor.ToString()
                                                              }).Distinct().AsParallel().ToList();

                                        cedula.Correcta = true;
                                    }

                                    if (espesorIn != "0" && espesorMm == "0")
                                    {
                                        cedula = lista.Where(x => x.CedulaA == null && x.CedulaB == null && x.CedulaC == null
                                            && x.CedulaIn == espesorIn).AsParallel().FirstOrDefault();
                                        cedula.Correcta = true;
                                    }

                                    if (espesorMm != "0" && espesorIn == "0")
                                    {
                                        cedula = lista.Where(x => x.CedulaA == null && x.CedulaB == null && x.CedulaC == null
                                            && x.CedulaMM == espesorMm).AsParallel().FirstOrDefault();
                                        cedula.Correcta = true;
                                    }

                                    if (espesorIn != "0" && espesorMm != "0")
                                    {
                                        cedula = lista.Where(x => x.CedulaA == null && x.CedulaB == null && x.CedulaC == null
                                            && x.CedulaMM == espesorMm && x.CedulaIn == espesorIn).AsParallel().FirstOrDefault();
                                        cedula.Correcta = true;
                                    }

                                }
                                else
                                {
                                    cedula.Correcta = false;
                                    lista.Clear();
                                }
                            }
                            else
                            {
                                cedula = cedulaCID == 0 ? null : lista.Where(x => x.CedulaC == datosCedulas.CedulaC).AsParallel().SingleOrDefault();
                                cedula.Correcta = true;

                            }
                        }
                        else if (String.IsNullOrEmpty(datosCedulas.CedulaC))
                        {
                            //busco por B
                            cedula = cedulaBID == 0 ? null : lista.Where(x => x.CedulaB == datosCedulas.CedulaB).AsParallel().SingleOrDefault();
                            cedula.Correcta = true;
                        }
                        else
                        {
                            //busco por b y c
                            cedula = cedulaBID == 0 ? cedulaCID == 0 ? null : lista.Where(x => x.CedulaC == datosCedulas.CedulaC).AsParallel().SingleOrDefault() : cedulaCID == 0 ? lista.Where(x => x.CedulaB == datosCedulas.CedulaB).AsParallel().SingleOrDefault() : lista.Where(x => x.CedulaB == datosCedulas.CedulaB && x.CedulaC == datosCedulas.CedulaC).AsParallel().SingleOrDefault();
                            cedula.Correcta = true;
                        }
                    }
                    else if (String.IsNullOrEmpty(datosCedulas.CedulaB))
                    {
                        if (String.IsNullOrEmpty(datosCedulas.CedulaC))
                        {
                            //busco por A
                            cedula = cedulaAID == 0 ? null : lista.Where(x => x.CedulaA == datosCedulas.CedulaA).AsParallel().SingleOrDefault();
                            cedula.Correcta = true;
                        }
                        else
                        {
                            //busco por A y C
                            cedula = cedulaAID == 0 ? cedulaCID == 0 ? null : lista.Where(x => x.CedulaC == datosCedulas.CedulaC).AsParallel().SingleOrDefault() : cedulaCID == 0 ? lista.Where(x => x.CedulaA == datosCedulas.CedulaA).AsParallel().SingleOrDefault() : lista.Where(x => x.CedulaA == datosCedulas.CedulaA && x.CedulaC == datosCedulas.CedulaC).AsParallel().SingleOrDefault();
                            cedula.Correcta = true;
                        }
                    }
                    else if (String.IsNullOrEmpty(datosCedulas.CedulaC))
                    {
                        //busco por A y b
                        cedula = cedulaAID == 0 ? cedulaBID == 0 ? null : lista.Where(x => x.CedulaB == datosCedulas.CedulaB).AsParallel().SingleOrDefault() : cedulaBID == 0 ? lista.Where(c => c.CedulaA == datosCedulas.CedulaA).AsParallel().SingleOrDefault() : lista.Where(x => x.CedulaA == datosCedulas.CedulaA && x.CedulaB == datosCedulas.CedulaB).AsParallel().SingleOrDefault();
                        cedula.Correcta = true;
                    }
                    //else
                    //{
                    //    //busco por todas
                    //    cedula = cedulaAID == 0 
                    //}


                    return cedula;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                LoggerBd.Instance.EscribirLog(errorInfo);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Error en la búsqueda de cédulas.");
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object obtenerFamilia(int familiaAceroID)
        {
            try
            {
                //                select Sam3_FamiliaMaterial.Nombre,Sam3_FamiliaMaterial.FamiliaMaterialID from Sam3_FamiliaMaterial 
                //inner join Sam3_FamiliaAcero on Sam3_FamiliaMaterial.FamiliaMaterialID = Sam3_FamiliaAcero.FamiliaMaterialID
                //where Sam3_FamiliaAcero.FamiliaAceroID=3;
                using (SamContext ctx = new SamContext())
                {

                    return (from x in ctx.Sam3_FamiliaMaterial
                            join t in ctx.Sam3_FamiliaAcero on x.FamiliaMaterialID equals t.FamiliaMaterialID
                            where t.FamiliaAceroID == familiaAceroID && x.Activo && t.Activo
                            select x.Nombre).AsParallel().SingleOrDefault();
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

        public object validarDiametroExistente(string diametro)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    decimal valor = Convert.ToDecimal(diametro);
                    if (ctx.Sam3_Diametro.Where(x => x.Valor == valor && x.Activo).Any())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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

        private void ValidarDiametros(decimal valor, Sam3_Usuario usuario)
        {
            try
            {
                Diametro sam2_diametro;
                Sam3_Diametro sam3_diametro;
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        #region insertar diametro y equivalencia si no existe
                        if (ctx.Sam3_Diametro.Where(x => x.Valor == valor).Any())
                        {
                            sam3_diametro = (from d in ctx.Sam3_Diametro
                                             where d.Activo && d.Valor == valor
                                             select d).AsParallel().SingleOrDefault();
                        }
                        else
                        {
                            sam3_diametro = new Sam3_Diametro
                            {
                                Activo = true,
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = usuario.UsuarioID,
                                Valor = valor,
                                VerificadoPorCalidad = true
                            };

                            ctx.Sam3_Diametro.Add(sam3_diametro);
                            ctx.SaveChanges();
                        }

                        if (ctx2.Diametro.Where(x => x.Valor == sam3_diametro.Valor).AsParallel().Any())
                        {
                            sam2_diametro = ctx2.Diametro.Where(x => x.Valor == sam3_diametro.Valor).AsParallel().SingleOrDefault();
                        }
                        else
                        {
                            //no existe el diametro en sam2
                            sam2_diametro = new Diametro
                            {
                                Valor = sam3_diametro.Valor,
                                FechaModificacion = DateTime.Now,
                                VerificadoPorCalidad = true
                            };
                            ctx2.Diametro.Add(sam2_diametro);
                            ctx2.SaveChanges();
                        }

                        if (!ctx.Sam3_EquivalenciaDiametro.Where(x => x.Sam2_DiametroID == sam2_diametro.DiametroID && x.Sam3_DiametroID == sam3_diametro.DiametroID
                            && x.Activo).AsParallel().Any())
                        {
                            //si no existe la equivalencia la creamos, si existe termina este proceso
                            Sam3_EquivalenciaDiametro equivalencia = new Sam3_EquivalenciaDiametro
                            {
                                Activo = true,
                                Sam3_DiametroID = sam3_diametro.DiametroID,
                                Sam2_DiametroID = sam2_diametro.DiametroID,
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = usuario.UsuarioID
                            };
                            ctx.Sam3_EquivalenciaDiametro.Add(equivalencia);
                            ctx.SaveChanges();
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerBd.Instance.EscribirLog(ex);
            }
        }
    }
}