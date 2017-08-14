using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Web.Script.Serialization;
using System.Transactions;

namespace BackEndSAM.DataAcces
{
    public class CortadorBd
    {
        private static readonly object _mutex = new object();
        private static CortadorBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private CortadorBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static CortadorBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CortadorBd();
                    }
                }
                return _instance;
            }
        }

        public object ListadoCortadores(Sam3_Usuario usuario)
        {
            try
            {
                
                List<int> proyectos = new List<int>();
                List<int> patios = new List<int>();
                List<Operador> listado = new List<Operador>();

                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {

                        proyectos = (from p in ctx.Sam3_Rel_Usuario_Proyecto
                                     join eqp in ctx.Sam3_EquivalenciaProyecto on p.ProyectoID equals eqp.Sam3_ProyectoID
                                     where p.Activo && eqp.Activo
                                     && p.UsuarioID == usuario.UsuarioID
                                     select eqp.Sam2_ProyectoID).Distinct().AsParallel().ToList();

                        proyectos = proyectos.Where(x => x > 0).ToList();


                        patios = (from p in ctx.Sam3_Proyecto
                                  join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                  join eq in ctx.Sam3_EquivalenciaPatio on pa.PatioID equals eq.Sam3_PatioID
                                  join up in ctx.Sam3_Rel_Usuario_Proyecto on p.ProyectoID equals up.ProyectoID
                                  where p.Activo && pa.Activo && eq.Activo
                                  && up.UsuarioID == usuario.UsuarioID
                                  select eq.Sam2_PatioID).Distinct().AsParallel().ToList();

                        patios = patios.Where(x => x > 0).ToList();

                        listado = (from co in ctx2.Cortador
                                   where patios.Contains(co.PatioID)
                                   select new Operador
                                   {
                                       OperadorID = co.CortadorID.ToString(),
                                       Nombre = co.Nombre
                                   }).AsParallel().Distinct().ToList();
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