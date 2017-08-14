using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using BackEndSAM.Models;
using SecurityManager.Api.Models;

namespace BackEndSAM.DataAcces
{
    public class OrdenTrabajoBd
    {
        private static readonly object _mutex = new object();
        private static OrdenTrabajoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private OrdenTrabajoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static OrdenTrabajoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new OrdenTrabajoBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerOrdenTrabajoPorProyecto(string proyecto, string busqueda, Sam3_Usuario usuario)
        {
            try
            {
                List<ListaCombos> listado = new List<ListaCombos>();
                int sam2_proyectoID = 0;
                List<int> proyectos = new List<int>();
                List<int> patios = new List<int>();
                int proyectoID = string.IsNullOrEmpty(proyecto) ? 0 : Convert.ToInt32(proyecto);

                using (Sam2Context ctx2 = new Sam2Context())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        proyectos = (from p in ctx.Sam3_Rel_Usuario_Proyecto
                                     join eqp in ctx.Sam3_EquivalenciaProyecto on p.ProyectoID equals eqp.Sam3_ProyectoID
                                     where p.Activo && eqp.Activo
                                     && p.UsuarioID == usuario.UsuarioID
                                     && eqp.Sam3_ProyectoID == proyectoID
                                     select eqp.Sam2_ProyectoID).Distinct().AsParallel().ToList();

                        proyectos = proyectos.Where(x => x > 0).ToList();



                        patios = (from p in ctx.Sam3_Proyecto
                                  join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                  join eq in ctx.Sam3_EquivalenciaPatio on pa.PatioID equals eq.Sam2_PatioID
                                  where p.Activo && pa.Activo && eq.Activo
                                  && proyectos.Contains(p.ProyectoID)
                                  select eq.Sam2_PatioID).Distinct().AsParallel().ToList();

                        patios = patios.Where(x => x > 0).ToList();

                        List<string> elementos = new List<string>();
                        if (busqueda != "" && busqueda != null)
                        {
                            char[] lstElementoNumeroControl = busqueda.ToCharArray();
                            foreach (char i in lstElementoNumeroControl)
                            {
                                elementos.Add(i.ToString());
                            }
                        }


                        sam2_proyectoID = (from eq in ctx.Sam3_EquivalenciaProyecto
                                           where eq.Activo && eq.Sam3_ProyectoID == proyectoID
                                           select eq.Sam2_ProyectoID).AsParallel().SingleOrDefault();

                        listado = (from odt in ctx2.OrdenTrabajo
                                   join p in ctx2.Proyecto on odt.ProyectoID equals p.ProyectoID
                                   join pa in ctx2.Patio on p.PatioID equals pa.PatioID
                                   where proyectos.Contains(p.ProyectoID)
                                   && patios.Contains(pa.PatioID)
                                   && odt.ProyectoID == sam2_proyectoID
                                   && odt.EstatusOrdenID == (from es in ctx2.EstatusOrden
                                                             where es.Nombre == "Activa"
                                                             select es.EstatusOrdenID).FirstOrDefault()
                                   && elementos.Any(x => odt.NumeroOrden.Contains(x))
                                   select new ListaCombos
                                   {
                                       id = odt.OrdenTrabajoID.ToString(),
                                       value = odt.NumeroOrden
                                   }).AsParallel().Distinct().OrderBy(x => x.value).ToList();
                    }
                }

                return listado;
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