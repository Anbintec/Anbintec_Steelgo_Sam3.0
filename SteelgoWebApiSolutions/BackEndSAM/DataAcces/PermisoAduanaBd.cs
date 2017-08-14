using BackEndSAM.Models;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;

namespace BackEndSAM.DataAcces
{
    public class PermisoAduanaBd
    {
        private static readonly object _mutex = new object();
        private static PermisoAduanaBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private PermisoAduanaBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static PermisoAduanaBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PermisoAduanaBd();
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// Método para obtener los datos del aviso de llegada correspondientes al folio
        /// para enviarlos por correo
        /// </summary>
        /// <param name="folio">Folio seleccionado</param>
        /// <returns>Lista de Formato Permiso aduana</returns>
        public TransactionalInformation ObtenerDatosAvisoLlegada(int folio)
        {
            try
            {
                List<FormatoPermisoAduana> listaAvisoLlegada = new List<FormatoPermisoAduana>();
                List<FormatoPermisoAduana> listaProyectos = new List<FormatoPermisoAduana>();
                List<FormatoPermisoAduana> listaVehiculos = new List<FormatoPermisoAduana>();
                List<FormatoPermisoAduana> listaClientes = new List<FormatoPermisoAduana>();
                int clienteFolioAviso = 0;
                string nombreClienteFolioAviso = "";
               

                using (Sam2Context ctx2 = new Sam2Context())
                {
                    listaClientes = (from cl in ctx2.Cliente
                                     select new FormatoPermisoAduana
                                     {
                                         ClienteID = cl.ClienteID,
                                         NombreCliente = cl.Nombre
                                     }).AsParallel().ToList();
                }

                using (SamContext ctx = new SamContext())
                {

                    var cliente = (from av in ctx.Sam3_FolioAvisoLlegada
                                   where (av.FolioAvisoLlegadaID == folio)
                                   select av.ClienteID).FirstOrDefault();

                    for (int i = 0; i < listaClientes.Count; i++)
                    {
                        if (cliente == listaClientes[i].ClienteID)
                        {
                            clienteFolioAviso = listaClientes[i].ClienteID;
                            nombreClienteFolioAviso = listaClientes[i].NombreCliente;
                            break;
                        }

                    }

                    listaAvisoLlegada = (from av in ctx.Sam3_FolioAvisoLlegada
                                         where (av.FolioAvisoLlegadaID == folio)
                                         select new FormatoPermisoAduana
                                         {
                                             FechaRecepcion = av.FechaRecepcion,
                                             NombreCliente = nombreClienteFolioAviso
                                         }).AsParallel().ToList();

                    listaProyectos = (from avp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                      join p in ctx.Sam3_Proyecto on avp.ProyectoID equals p.ProyectoID
                                      where avp.FolioAvisoLlegadaID == folio
                                      select new FormatoPermisoAduana
                                      {
                                          NombreProyecto = p.Nombre
                                      }).AsParallel().ToList();

                    listaVehiculos = (from av in ctx.Sam3_Rel_FolioAvisoLlegada_Vehiculo
                                      join v in ctx.Sam3_Vehiculo on av.VehiculoID equals v.VehiculoID
                                      join vc in ctx.Sam3_Rel_Vehiculo_Chofer on av.VehiculoID equals vc.VehiculoID
                                      join ch in ctx.Sam3_Chofer on vc.ChoferID equals ch.ChoferID
                                      join vt in ctx.Sam3_Rel_Vehiculo_Transportista on av.VehiculoID equals vt.VehiculoID
                                      join t in ctx.Sam3_Transportista on vt.TransportistaID equals t.TransportistaID
                                      join tv in ctx.Sam3_TipoVehiculo on v.TipoVehiculoID equals tv.TipoVehiculoID
                                      where av.FolioAvisoLlegadaID == folio
                                      select new FormatoPermisoAduana
                                      {
                                          PlacasPlana = v.Placas,
                                          Unidad = v.Unidad,
                                          Modelo = v.Modelo,
                                          TipoVehiculo = tv.Nombre,
                                          NombreChofer = ch.Nombre,
                                          NombreTransportista = t.Nombre
                                      }).AsParallel().ToList();

                    listaAvisoLlegada.AddRange(listaProyectos);
                    listaAvisoLlegada.AddRange(listaVehiculos);
                }
                TransactionalInformation resultEmail = EnviarCorreo(listaAvisoLlegada, listaVehiculos);
                if (resultEmail.ReturnCode == 200)
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
                    result.IsAuthenicated = true;

                    return result;
                }
                else
                {
                    resultEmail.ReturnCode = 500;
                    resultEmail.ReturnStatus = false;
                    resultEmail.IsAuthenicated = true;

                    return resultEmail;
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
        /// Método para enviar el formato de Permiso de Aduana por correo
        /// </summary>
        /// <param name="listaAvisoLlegada">lista con los datos del permiso de aduana seleccionado</param>
        /// <returns></returns>
        public TransactionalInformation EnviarCorreo(object listaAvisoLlegada, List<FormatoPermisoAduana> listaVehiculos)
        {
            try
            {
                List<string> lstEnvio = new List<string>();
                using (SamContext ctx = new SamContext())
                {
                    lstEnvio = (from un in ctx.Sam3_UsuariosNotificaciones
                                 join tn in ctx.Sam3_TipoNotificacion on un.TipoNotificacionID equals tn.TipoNotificacionID
                                 where tn.Nombre == "Permiso Aduana"
                                 select un.Email).ToList();
                }

                foreach (string mailEnvio in lstEnvio)
                {

                    string proyectos = "";
                    string transportista = "";
                    string placas = "";
                    string unidad = "";
                    string modelo = "";
                    string tipoVehiculo = "";
                    string chofer = "";
                    string[] placasArray = { };
                    string[] choferArray = { };
                    string[] unidadArray = { };
                    string[] modeloArray = { };
                    string[] transportistaArray = { };
                    string[] tipoVehiculoArray = { };

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(listaAvisoLlegada);
                    List<FormatoPermisoAduana> listaDatos = new List<FormatoPermisoAduana>();
                    listaDatos = serializer.Deserialize<List<FormatoPermisoAduana>>(json);


                    for (int i = 1; i < listaDatos.Count; i++)
                    {
                        if (listaDatos[i].NombreProyecto != null) { proyectos = proyectos + listaDatos[i].NombreProyecto + " , "; }
                        if (listaDatos[i].NombreTransportista != null) { transportista = transportista + listaDatos[i].NombreTransportista + " , "; }
                        if (listaDatos[i].PlacasPlana != null) { placas = placas + listaDatos[i].PlacasPlana + " , "; }
                        if (listaDatos[i].Unidad != null) { unidad = unidad + listaDatos[i].Unidad + " , "; }
                        if (listaDatos[i].Modelo != null) { modelo = modelo + listaDatos[i].Modelo + " , "; }
                        if (listaDatos[i].TipoVehiculo != null) { tipoVehiculo = tipoVehiculo + listaDatos[i].TipoVehiculo + " , "; }
                        if (listaDatos[i].NombreChofer != null) { chofer = chofer + listaDatos[i].NombreChofer + " , "; }
                    }

                    string fechaRecepcion = listaDatos[0].FechaRecepcion.ToString().Substring(0, 9);
                    DateTime nuevaFecha = DateTime.Parse(fechaRecepcion);
                    StringBuilder body = new StringBuilder();
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("mail.sysgo.com.mx", 25);
                    mail.From = new MailAddress("karen.delacruz@steelgo.com");
                    mail.To.Add(mailEnvio);
                    //Correo Sam2
                    mail.Sender = new MailAddress("automatic@sysgo.com.mx");
                    //Por definir Subject
                    mail.Subject = "Permiso de Aduana";
                    mail.IsBodyHtml = true;
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                    //HTML para las planas
                    if (placas.Length > 0) { placasArray = placas.Split(','); };
                    if (chofer.Length > 0) { choferArray = chofer.Split(','); };
                    if (unidad.Length > 0) { unidadArray = unidad.Split(','); };
                    if (modelo.Length > 0) { modeloArray = modelo.Split(','); };
                    if (transportista.Length > 0) { transportistaArray = transportista.Split(','); };
                    if (tipoVehiculo.Length > 0) { tipoVehiculoArray = tipoVehiculo.Split(','); };

                    for (int i = 0; i < listaVehiculos.Count; i++)
                    {
                        body.AppendFormat("Plataforma: " + placasArray[i] + "<br />");
                        if (choferArray.Length > 0) { body.AppendFormat("Operador: " + choferArray[i] + "<br />"); }
                        if (unidadArray.Length > 0) { body.AppendFormat("Unidad: " + unidadArray[i] + "<br />"); }
                        if (modeloArray.Length > 0) { body.AppendFormat("Modelo: " + modeloArray[i] + "<br />"); }
                        if (transportistaArray.Length > 0) { body.AppendFormat("L&iacute;nea Transportista: " + transportistaArray[i] + "<br />"); }
                        //if (tipoVehiculoArray.Length > 0) { body.AppendFormat("Tipo Vehiculo: " + tipoVehiculoArray[i] + "<br />"); }
                        body.AppendFormat("<br />");
                    }
                    //HTML del mensaje
                    mail.Body = "Buen d&iacute;a Capit&aacute;n<br />"
                                + "Solicito permiso de acceso para la siguiente unidad los d&iacute;as " + String.Format("{0:dd/MM/yyyy}", nuevaFecha.AddDays(-2))
                                + " al " + String.Format("{0:dd/MM/yyyy}", nuevaFecha.AddDays(2))
                                + ", que estar&aacute;  ingresando a las instalaciones de Steelgo.<br /> <br />"
                                + " Cliente: " + listaDatos[0].NombreCliente + "<br />"
                                + "Proyecto: " + proyectos.Substring(0, proyectos.Length - 2) + "<br />"
                                + "<br />"
                                + body;


                    SmtpServer.EnableSsl = false;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("automatic@sysgo.com.mx", "S733lg0H0u*");

                    SmtpServer.Send(mail);
                }

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Ok");
                result.ReturnCode = 200;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500; /*Codigo*/
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        /// <summary>
        /// Insertar permiso de aduana en base de datos, si ya existe un permiso con ese folio de aviso de llegada, se hace update, sino se inserta
        /// </summary>
        /// <param name="folio">Folio seleccionado por el usuario</param>
        /// <param name="usuario">Usuario Actual</param>
        /// <returns></returns>
        public TransactionalInformation InsertarPermisoADuana(int folio, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    Sam3_PermisoAduana permisoAduana = new Sam3_PermisoAduana();
                    var folioAvisoLlegada = (from pa in ctx.Sam3_PermisoAduana
                                             where (pa.FolioAvisoLlegadaID == folio)
                                             select pa).FirstOrDefault();
                    if (folioAvisoLlegada == null)
                    {
                        permisoAduana.FolioAvisoLlegadaID = folio;
                        permisoAduana.Estatus = "Creado";
                        permisoAduana.Activo = true;
                        permisoAduana.UsuarioModificacion = usuario.UsuarioID;
                        permisoAduana.FechaModificacion = DateTime.Now;
                        permisoAduana.PermisoAutorizado = false;
                        permisoAduana.PermisoTramite = true;
                        permisoAduana.NumeroPermiso = null;
                        permisoAduana.FechaGeneracion = DateTime.Now;
                        permisoAduana.FechaAutorización = null;

                        ctx.Sam3_PermisoAduana.Add(permisoAduana);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        permisoAduana = (from pa in ctx.Sam3_PermisoAduana
                                         where (pa.FolioAvisoLlegadaID == folio)
                                         select pa).AsParallel().First();

                        permisoAduana.FolioAvisoLlegadaID = folio;
                        permisoAduana.Estatus = "Creado";
                        permisoAduana.Activo = true;
                        permisoAduana.UsuarioModificacion = usuario.UsuarioID;
                        permisoAduana.FechaModificacion = DateTime.Now;
                        permisoAduana.PermisoAutorizado = false;
                        permisoAduana.PermisoTramite = true;
                        permisoAduana.NumeroPermiso = null;
                        permisoAduana.FechaGeneracion = DateTime.Now;
                        permisoAduana.FechaAutorización = null;

                        ctx.SaveChanges();
                    }

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnMessage.Add(permisoAduana.PermisoAduanaID.ToString());
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

        /// <summary>
        /// Método para guardar los datos del permiso autorizado que capturó el usuario
        /// Inserta el documento que adjuntó el usuario
        /// </summary>
        /// <param name="numeroPermiso">Número del permiso de ADuana autorizado</param>
        /// <param name="nombre">Nombre del documento </param>
        /// <param name="extension">Entension del documento</param>
        /// <param name="folio">Folio del aviso llegada seleccionado</param>
        /// <param name="documentoID">ID del documento</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns></returns>
        public object GuardarDatosPermisoAutorizado(string numeroPermiso, string nombre, string extension, int folio, int documentoID, Sam3_Usuario usuario)
        {
            try
            {
                int PermisoAduanaID = 0;
                //Actualizar numero de permiso en el permiso tramitado anteriormente en PErmisoAduana
                using (SamContext ctx = new SamContext())
                {
                    var nuevoPermiso = (from pa in ctx.Sam3_PermisoAduana
                                        where (pa.FolioAvisoLlegadaID == folio)
                                        select pa).First();

                    nuevoPermiso.PermisoAutorizado = true;
                    nuevoPermiso.PermisoTramite = false;
                    nuevoPermiso.NumeroPermiso = numeroPermiso;
                    nuevoPermiso.UsuarioModificacion = usuario.UsuarioID;
                    nuevoPermiso.FechaModificacion = DateTime.Now;
                    nuevoPermiso.FechaAutorización = DateTime.Now;
                    ctx.SaveChanges();

                    //Guardar Documento en Rel_PermisoAduana_Documento
                    Sam3_Rel_PermisoAduana_Documento permisoAduana_Documento = new Sam3_Rel_PermisoAduana_Documento();
                    PermisoAduanaID = nuevoPermiso.PermisoAduanaID;
                    bool PermisoAduanaExists = ctx.Sam3_Rel_PermisoAduana_Documento.Any(r => r.PermisoAduanaID == PermisoAduanaID);

                    if (PermisoAduanaExists)
                    {
                        permisoAduana_Documento.PermisoAduanaID = PermisoAduanaID;
                        permisoAduana_Documento.DocumentoID = documentoID;
                        permisoAduana_Documento.Nombre = nombre;
                        permisoAduana_Documento.Extencion = extension;
                        permisoAduana_Documento.Activo = true;
                        permisoAduana_Documento.UsuarioModificacion = usuario.UsuarioID;
                        permisoAduana_Documento.FechaModificacion = DateTime.Now;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        permisoAduana_Documento.PermisoAduanaID = PermisoAduanaID;
                        permisoAduana_Documento.DocumentoID = documentoID;
                        permisoAduana_Documento.Nombre = nombre;
                        permisoAduana_Documento.Extencion = extension;
                        permisoAduana_Documento.Activo = true;
                        permisoAduana_Documento.UsuarioModificacion = usuario.UsuarioID;
                        permisoAduana_Documento.FechaModificacion = DateTime.Now;

                        ctx.Sam3_Rel_PermisoAduana_Documento.Add(permisoAduana_Documento);
                        ctx.SaveChanges();
                    }
                }

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Ok");
                result.ReturnMessage.Add(PermisoAduanaID.ToString());
                result.ReturnCode = 200;
                result.ReturnStatus = true;
                result.IsAuthenicated = true;

                return result;
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