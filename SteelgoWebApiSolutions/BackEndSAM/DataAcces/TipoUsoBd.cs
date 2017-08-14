using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces
{
    public class TipoUsoBd
    {

         private static readonly object _mutex = new object();
         private static TipoUsoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private TipoUsoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static TipoUsoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TipoUsoBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtener los tipos de uso para Cuantificacion
        /// </summary>
        /// <returns>lista de tipos de uso</returns>
        public object ObtenerTipoUso(int agregarOpcion, Sam3_Usuario usuario, int paginaID, string idioma)
        {
            try
            {
                List<TipoUso> listTU = new List<TipoUso>();

                using (SamContext ctx = new SamContext())
                {
                    if (agregarOpcion == 1 && (bool)PerfilBd.Instance.VerificarPermisoCreacion(usuario.PerfilID, "Tipo Uso", paginaID))
                    {
                        if (idioma == "en-US")
                        {
                            listTU.Add(new TipoUso { Nombre = "Add new", id = "0" });
                        }
                        else
                        {
                            listTU.Add(new TipoUso { Nombre = "Agregar Nuevo", id = "0" });
                        }
                    }
                    List<TipoUso> tipoUso = (from t in ctx.Sam3_TipoUso
                              where t.Activo
                              select new TipoUso
                                {
                                    id = t.TipoUsoID.ToString(),
                                    Nombre = t.Nombre
                                }).AsParallel().ToList();

                    listTU.AddRange(tipoUso);
                }
                return listTU;
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

        public object InsertarTipoUso(string nombre, Sam3_Usuario usuario)
        {
            try
            {
                TransactionalInformation result = new TransactionalInformation();
                using (SamContext ctx = new SamContext())
                {
                    if (!ctx.Sam3_TipoUso.Where(x => x.Nombre == nombre).Any())
                    {
                        Sam3_TipoUso nuevoTipo = new Sam3_TipoUso();
                        nuevoTipo.Activo = true;
                        nuevoTipo.FechaModificacion = DateTime.Now;
                        nuevoTipo.Nombre = nombre;
                        nuevoTipo.UsuarioModificacion = usuario.UsuarioID;

                        ctx.Sam3_TipoUso.Add(nuevoTipo);

                        ctx.SaveChanges();

                        result.ReturnMessage.Add(nuevoTipo.TipoUsoID.ToString());
                        result.ReturnMessage.Add(nuevoTipo.Nombre);
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;

                        return result;

                    }
                    else
                    {
                        result.ReturnMessage.Add(string.Format("Ya existe un registro con el nombre {0}", nombre));
                        result.ReturnCode = 500;
                        result.ReturnStatus = false;
                        result.IsAuthenicated = true;

                        return result;
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

    }
}