using BackEndSAM.Models.Materiales.UbicacionNumeroUnico;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Materiales.UbicacionNumeroUnico
{
    public class UbicacionNumeroUnicoBd
    {
        private static readonly object _mutex = new object();
        private static UbicacionNumeroUnicoBd _instance;


        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private UbicacionNumeroUnicoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static UbicacionNumeroUnicoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new UbicacionNumeroUnicoBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoRack(int proyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Rack> listaRacks = (from r in ctx.Sam3_Proyecto
                                             join c in ctx.Sam3_Rack on r.PatioID equals c.PatioID
                                             where r.Activo && c.Activo == true && r.ProyectoID == proyectoID
                                             select new Rack
                                             {
                                                 Nombre = c.Rack,
                                                 RackID = c.RackID
                                             }).AsParallel().ToList();
                    return listaRacks;                                       
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

        public object ObtenerUbicacionRack(int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {                    
                    List<RackUbicacion> ListaUbicacion = (
                        from P in ctx.Sam3_Proyecto
                        join R in ctx.Sam3_Rack on P.PatioID equals R.PatioID
                        where P.Activo && R.Activo == true && P.ProyectoID == ProyectoID                       
                        group R by R.Ubicacion into data
                        select new RackUbicacion
                        {                            
                            Ubicacion = data.Key
                        }
                    ).AsParallel().ToList();
                    return ListaUbicacion;
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

        public object ObtenerPasilloRack(string Ubicacion)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<RackPasillo> ListaPasillo = (
                        from R in ctx.Sam3_Rack
                        where R.Activo == true && R.Ubicacion == Ubicacion
                        group R by R.Pasillo into data
                        select new RackPasillo
                        {
                            Pasillo = data.Key
                        }
                    ).AsParallel().ToList();
                    return ListaPasillo;
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

        public object ObtenerNivelRack(string Ubicacion, string Pasillo)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {                    
                    List<RackNivel> ListaNivel = (
                        from R in ctx.Sam3_Rack
                        where R.Activo == true && (R.Ubicacion == Ubicacion && R.Pasillo == Pasillo)
                        select new RackNivel
                        {                            
                            RackID = R.RackID,
                            Nivel = R.Nivel
                        }
                    ).AsParallel().ToList();
                    return ListaNivel;
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
        /// Actualizar la ubicacion de un cojunto numero unico.
        /// </summary>
        /// <param name="arrayNumeroUnico">arreglo de numeros unicos</param>
        /// <param name="usuario">usuario actual</param>
        /// <param name="proyectoID">proyecto para los numeros unicos.</param>
        ///  <param name="rackID">rack que se le asigna a los numeros unicos.</param>
        /// <returns>numeros unicos que no se han actualizado.</returns>
        public object ActualizarRack(int[] arrayNumeroUnico, int proyectoID, int usuario, int rackID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        List<Sam3_NumeroUnico> listaNumerosUnicos = ctx.Sam3_NumeroUnico
                            .Where(x => arrayNumeroUnico.Contains(x.Consecutivo) && x.ProyectoID==proyectoID).ToList();

                        listaNumerosUnicos.ForEach(x => x.RackID = rackID);

                        
                        ctx.SaveChanges();
                        ctx_tran.Commit();
                       return ObtenerRack(arrayNumeroUnico, proyectoID, usuario);
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
        /// Obtiene la ubicacion de un cojunto numero unico.
        /// </summary>
        /// <param name="arrayNumeroUnico">arreglo de numeros unicos</param>
        /// <param name="usuario">usuario actual</param>
        /// <param name="proyectoID">proyecto para los numeros unicos.</param>
        ///  <param name="rackID">rack que se le asigna a los numeros unicos.</param>
        /// <returns>numeros unicos que no se han actualizado.</returns>
        public object ObtenerRack(int[] arrayNumeroUnico, int proyectoID, int usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {

                        List<DetalleGrid> listaNumerosUnicosCorrectos = (from r in ctx.Sam3_NumeroUnico
                                                                         join c in ctx.Sam3_ItemCode on r.ItemCodeID equals c.ItemCodeID
                                                                         join d in ctx.Sam3_Rack on r.RackID equals d.RackID into ODs
                                                                         from d in ODs.DefaultIfEmpty()
                                                                         where r.Activo && c.Activo && r.ProyectoID == proyectoID && arrayNumeroUnico.Contains(r.Consecutivo)
                                                                         select new DetalleGrid
                                                                         {
                                                                             Consecutivo=r.Consecutivo,
                                                                             NumeroUnicoID=r.NumeroUnicoID,
                                                                             NombreNU = r.NumeroUnicoCliente,
                                                                             ItemCode = c.Codigo,
                                                                             DescipcionItemCode = c.DescripcionEspanol,
                                                                             D1 = r.Diametro1,
                                                                             D2 = r.Diametro2,
                                                                             Rack = d.Rack
                                                                         }).AsParallel().ToList();

                        List<DetalleGrid> listaNumerosUnicosPorProyecto = (from r in ctx.Sam3_NumeroUnico.DefaultIfEmpty()
                                                                           where r.Activo && r.ProyectoID == proyectoID
                                                                           select new DetalleGrid
                                                                           {
                                                                               Consecutivo = r.Consecutivo
                                                                           }).AsParallel().ToList();

                        List<DetalleGrid> listaNumerosUnicosIncorrectos = new List<DetalleGrid>();
                       bool existe = false;
                        for (int i = 0; i < arrayNumeroUnico.Length; i++)
                        {
                            for (int j = 0; j < listaNumerosUnicosPorProyecto.Count; j++)
                            {
                                if (arrayNumeroUnico[i] == listaNumerosUnicosPorProyecto[j].Consecutivo)
                                {
                                    existe = true;
                                    
                                }
                            }
                            if (!existe)
                            {
                                listaNumerosUnicosIncorrectos.Add(new DetalleGrid { NumeroUnicoID = arrayNumeroUnico[i] });
                            }
                            existe = false;
                        }


                        List<object> listasNU = new List<object>();
                        listasNU.Add(listaNumerosUnicosCorrectos);
                        listasNU.Add(listaNumerosUnicosIncorrectos);

                        return listasNU;
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