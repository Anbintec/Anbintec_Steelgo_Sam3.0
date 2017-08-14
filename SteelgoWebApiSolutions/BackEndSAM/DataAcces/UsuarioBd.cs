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
using BackEndSAM.Utilities;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Net.Http.Headers;

namespace BackEndSAM.DataAcces
{
    public class UsuarioBd
    {
        private static readonly object _mutex = new object();
        private static UsuarioBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private UsuarioBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static UsuarioBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new UsuarioBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerPatiosYProyectosDeUsuario(int usuarioID, out List<int> proyectos, out List<int> patios)
        {
            try 
            {
                proyectos = new List<int>();
                patios = new List<int>();
                using (SamContext ctx = new SamContext())
                {
                    proyectos = (from r in ctx.Sam3_Rel_Usuario_Proyecto
                                 where r.Activo
                                 && r.UsuarioID == usuarioID
                                 select r.ProyectoID).AsParallel().Distinct().ToList();
                    //Proyecto default
                    proyectos.Add((from p in ctx.Sam3_Proyecto
                                   where p.Activo
                                   && p.Nombre == ""
                                   select p.ProyectoID).FirstOrDefault());

                    List<int> temp = proyectos;
                    patios = (from p in ctx.Sam3_Proyecto
                              join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                              where p.Activo && pa.Activo
                              && temp.Contains(p.ProyectoID)
                              select pa.PatioID).Distinct().AsParallel().ToList();
                    
                    return true;

                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                proyectos = null;
                patios = null;
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object listadoUsuarios(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ListaCombos> listado = (from us in ctx.Sam3_Usuario
                                                 where us.Activo
                                                 select new ListaCombos
                                                 {
                                                     id = us.UsuarioID.ToString(),
                                                     value = us.Nombre + " " + us.ApellidoPaterno
                                                 }).AsParallel().Distinct().ToList();

                    return listado;
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