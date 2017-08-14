using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using DatabaseManager.Sam2;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using SecurityManager.Login;
using System.Web.Security;


namespace SecurityManager.Login
{
    public class LoginSam2
    {
        private static readonly object _mutex = new object();
        private static LoginSam2 _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private LoginSam2()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static LoginSam2 Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new LoginSam2();
                    }
                }
                return _instance;
            }
        }

        public TransactionalInformation Login(string usuario, string password)
        {
            Sam3_Usuario usr = new Sam3_Usuario();
            try
            {
                TransactionalInformation transaction = new TransactionalInformation();
                Sam3_Usuario usuarioSam3;
                
                using (SamContext ctx = new SamContext())
                {
                    MembershipProvider proveedor = Membership.Provider;
                    MembershipUser usuarioRegistrado;

                    if (usuario != "samAdmin" && usuario != "samListados" && usuario != "samCreacion")
                    {
                        if (proveedor.ValidateUser(usuario, password))
                        {
                            usuarioRegistrado = proveedor.GetUser(usuario, false);
                            Guid id = (Guid)usuarioRegistrado.ProviderUserKey;

                            if (ctx.Sam3_Usuario.Where(x => x.Sam2UserID == id).Any())
                            {
                                usuarioSam3 = ctx.Sam3_Usuario.Where(x => x.Sam2UserID == id).AsParallel().SingleOrDefault();
                            }
                            else
                            {
                                using (Sam2Context ctx2 = new Sam2Context())
                                {
                                    
                                    Usuario usrSam2 = ctx2.Usuario.Where(x => x.UserId == id).SingleOrDefault();
                                    usuarioSam3 = new Sam3_Usuario
                                    {
                                        Activo = true,
                                        ApellidoMaterno = usrSam2.ApMaterno,
                                        ApellidoPaterno = usrSam2.ApPaterno,
                                        BloqueadoPorAdministracion = usr.BloqueadoPorAdministracion,
                                        ContrasenaHash = ctx2.aspnet_Membership.Where(x => x.UserId == id).Select(x => x.Password).SingleOrDefault(),
                                        FechaModificacion = DateTime.Now,
                                        Nombre = usr.Nombre,
                                        NombreUsuario = ctx2.aspnet_Users.Where(x => x.UserId == id).Select(x => x.UserName).SingleOrDefault(),
                                        PerfilID = ctx.Sam3_Perfil.Where(x => x.Nombre == "Sin Permisos").Select(x => x.PerfilID).SingleOrDefault(),
                                        Sam2UserID = id,
                                        UsuarioModificacion = 1
                                    };
                                    ctx.Sam3_Usuario.Add(usuarioSam3);
                                    ctx.SaveChanges();
                                }
                            }


                            string token = ManageTokens.Instance.CreateJwtToken(usuarioSam3);

                            transaction.IsAuthenicated = true;
                            transaction.ReturnMessage.Add(usuarioSam3.Nombre + " " + usuarioSam3.ApellidoPaterno);
                            transaction.ReturnMessage.Add(token);
                            transaction.ReturnCode = 200;
                            transaction.ReturnStatus = true;

                            #region registro
                            usr = new Sam3_Usuario
                            {
                                UsuarioID = usuarioSam3.UsuarioID,
                                NombreUsuario = usuarioSam3.NombreUsuario,
                                ContrasenaHash = usuarioSam3.ContrasenaHash,
                                PerfilID = usuarioSam3.PerfilID,
                                BloqueadoPorAdministracion = usuarioSam3.BloqueadoPorAdministracion,
                                Activo = usuarioSam3.Activo,
                                Sam2UserID = id
                            };

                            #endregion
                        }
                        else
                        {
                            string message = "Datos de usuario no valido";
                            transaction.ReturnCode = 400;
                            transaction.ReturnMessage.Add(message);
                            transaction.IsAuthenicated = false;
                            transaction.ReturnStatus = false;
                        }
                    }
                    else
                    {

                        usuarioSam3 = (from us in ctx.Sam3_Usuario
                                       where us.NombreUsuario == usuario && us.ContrasenaHash == password
                                       select us).AsParallel().SingleOrDefault();

                        if (usuarioSam3 != null)
                        {
                            int user = string.Compare(usuarioSam3.NombreUsuario, usuario, false);
                            int pass = string.Compare(usuarioSam3.ContrasenaHash, password, false);
                        }

                        if (usuarioSam3 != null && string.Compare(usuarioSam3.ContrasenaHash, password, false) == 0 && string.Compare(usuarioSam3.NombreUsuario, usuario, false) == 0)
                        {
                            string token = ManageTokens.Instance.CreateJwtToken(usuarioSam3);
                            token = token;
                            transaction.IsAuthenicated = true;
                            transaction.ReturnMessage.Add(usuarioSam3.Nombre + " " + usuarioSam3.ApellidoPaterno);
                            transaction.ReturnMessage.Add(token);
                            transaction.ReturnCode = 200;
                            transaction.ReturnStatus = true;
                        }
                        else
                        {
                            string message = "Datos de usuario no valido";
                            transaction.ReturnCode = 400;
                            transaction.ReturnMessage.Add(message);
                            transaction.IsAuthenicated = false;
                            transaction.ReturnStatus = false;
                        }
                    }
                }

                return transaction;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                Logger.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message + "Stack: "  + ex.StackTrace + "----" + ex.InnerException );
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
            finally
            {
                string json = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                json = serializer.Serialize(usr);
                Logger.Instance.EscribirLog("Usuario:" + usuario + ", pass: " + password);
                Logger.Instance.EscribirLog("GUID devuelto: " + usr.Sam2UserID.ToString());
                Logger.Instance.EscribirLog(json);
            }
        }

    }
}