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
    public class ItemCodeSteelgoBd
    {
        private static readonly object _mutex = new object();
        private static ItemCodeSteelgoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ItemCodeSteelgoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ItemCodeSteelgoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ItemCodeSteelgoBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtener los item code steelgo para el grid de materiales en cuantificacion
        /// </summary>
        /// <returns>Lista de ICS</returns>
        public object ObtenerListadoItemCodeSteelgo()
        {
            try
            {
                //List<ListaCombos> ics = new List<ListaCombos>();
                using (SamContext ctx = new SamContext())
                {
                    //ics.Add(new ListaCombos { id = "0", value = "Agregar Nuevo" });
                    List<ListaCombos> listado = (from r in ctx.Sam3_ItemCodeSteelgo
                                                 join rid in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on r.ItemCodeSteelgoID equals rid.ItemCodeSteelgoID
                                                 join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                                 join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                                 where r.Activo && rid.Activo
                                                 select new ListaCombos
                                                 {
                                                     id = rid.Rel_ItemCodeSteelgo_Diametro_ID.ToString(),
                                                     value = r.Codigo + "(" + d1.Valor.ToString() + ", " + d2.Valor.ToString() + ")"
                                                 }).AsParallel().ToList();
                    //ics.AddRange(listado);
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


        /// <summary>
        /// Obtener la relacion de un itemcode con un steelgo
        /// </summary>
        /// <returns>Lista de ICS</returns>
        public object ObtenerListadoRelacionItemCodeSteelgo(string itemcode)
        {
            try
            {
                //List<ListaCombos> ics = new List<ListaCombos>();
                using (SamContext ctx = new SamContext())
                {
                    //ics.Add(new ListaCombos { id = "0", value = "Agregar Nuevo" });
                    List<ListaCombos> listado = (from r in ctx.Sam3_ItemCodeSteelgo
                                                 join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on r.ItemCodeSteelgoID equals rids.ItemCodeSteelgoID
                                                 join rel in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rids.Rel_ItemCodeSteelgo_Diametro_ID equals rel.Rel_ItemCodeSteelgo_Diametro_ID
                                                 join rid in ctx.Sam3_Rel_ItemCode_Diametro on rel.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                 join item in ctx.Sam3_ItemCode on rid.ItemCodeID equals item.ItemCodeID
                                                 join d1 in ctx.Sam3_Diametro on rids.Diametro1ID equals d1.DiametroID
                                                 join d2 in ctx.Sam3_Diametro on rids.Diametro2ID equals d2.DiametroID
                                                 where r.Activo && rel.Activo && item.Activo 
                                                 && rid.Rel_ItemCode_Diametro_ID.ToString() == itemcode
                                                 select new ListaCombos
                                                 {
                                                     id = rids.Rel_ItemCodeSteelgo_Diametro_ID.ToString(),
                                                     value = r.Codigo + " (" + d1.Valor.ToString() + ", " + d2.Valor.ToString() + ")"
                                                 }).AsParallel().ToList();



                    //if (listado.Count <= 0)
                    //{

                    //    listado = (from r in ctx.Sam3_ItemCodeSteelgo
                    //               where r.Activo
                    //               select new ListaCombos
                    //               {
                    //                   id = r.ItemCodeSteelgoID.ToString(),
                    //                   value = r.Codigo
                    //               }).AsParallel().ToList();
                    //}

                    //ics.AddRange(listado);
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

        /// <summary>
        /// Obtener el detalle de un item code steelgo
        /// </summary>
        /// <param name="itemCodeSteelgoID">item code steelgo seleccionado</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns>objeto con la informacion del item code steelgo</returns>
        public object ObtenerDetalleitemCodeSteelgo(string itemCodeSteelgo, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int idSteelgo = Convert.ToInt32(itemCodeSteelgo);

                    ItemCodeSteelgoJson detalle = (from r in ctx.Sam3_ItemCodeSteelgo
                                                   join rid in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on r.ItemCodeSteelgoID equals rid.ItemCodeSteelgoID
                                                   join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                                   join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                                   join c in ctx.Sam3_CatalogoCedulas on r.CedulaID equals c.CatalogoCedulasID
                                                   join d in ctx.Sam3_Diametro on c.DiametroID equals d.DiametroID
                                                   where r.Activo && c.Activo && rid.Activo && d1.Activo && d2.Activo && d.Activo
                                                   && rid.Rel_ItemCodeSteelgo_Diametro_ID == idSteelgo
                                                   select new ItemCodeSteelgoJson
                                                   {
                                                       Area = r.Area,
                                                       DescripcionEspanol = r.DescripcionEspanol,
                                                       DescripcionIngles = r.DescripcionIngles,
                                                       Diametro1 = d1.Valor,
                                                       Diametro2 = d2.Valor,
                                                       Familia = (from fa in ctx.Sam3_FamiliaAcero
                                                                  where fa.FamiliaAceroID == r.FamiliaAceroID && fa.Activo && r.Activo
                                                                  select fa.Nombre).FirstOrDefault(),
                                                       ItemCodeSteelgoID = rid.Rel_ItemCodeSteelgo_Diametro_ID,
                                                       Peso = r.Peso,
                                                       Codigo = r.Codigo,
                                                       TipoAcero = (from fa in ctx.Sam3_FamiliaAcero
                                                                    join fm in ctx.Sam3_FamiliaMaterial on fa.FamiliaMaterialID equals fm.FamiliaMaterialID
                                                                    where fa.FamiliaAceroID == r.FamiliaAceroID && fa.Activo && fm.Activo
                                                                    select fm.Nombre).FirstOrDefault(),
                                                   }).AsParallel().SingleOrDefault();

                    if (detalle != null)
                    {
                        string diametro = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                           join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                           join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                           join d in ctx.Sam3_Diametro on cat.DiametroID equals d.DiametroID
                                           where ricsd.Rel_ItemCodeSteelgo_Diametro_ID == detalle.ItemCodeSteelgoID
                                           && ricsd.Activo && ics.Activo && cat.Activo && d.Activo
                                           select d.Valor.ToString()).AsParallel().SingleOrDefault();

                        string cedulaA = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                          join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                          join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                          join ced in ctx.Sam3_Cedula on cat.CedulaA equals ced.CedulaID
                                          where ricsd.Rel_ItemCodeSteelgo_Diametro_ID == detalle.ItemCodeSteelgoID
                                          && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                          select ced.Codigo).AsParallel().SingleOrDefault();

                        string cedulaB = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                          join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                          join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                          join ced in ctx.Sam3_Cedula on cat.CedulaB equals ced.CedulaID
                                          where ricsd.Rel_ItemCodeSteelgo_Diametro_ID == detalle.ItemCodeSteelgoID
                                          && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                          select ced.Codigo).AsParallel().SingleOrDefault();

                        string cedulaC = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                          join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                          join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                          join ced in ctx.Sam3_Cedula on cat.CedulaC equals ced.CedulaID
                                          where ricsd.Rel_ItemCodeSteelgo_Diametro_ID == detalle.ItemCodeSteelgoID
                                          && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                          select ced.Codigo).AsParallel().SingleOrDefault();

                        detalle.Cedula = diametro + " - " + cedulaA + " - " + cedulaB + " - " + cedulaC;
                    }
                    return detalle;
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
        /// Obtener el detalle de un item code steelgo
        /// </summary>
        /// <param name="itemCodeSteelgoID">item code steelgo seleccionado</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns>objeto con la informacion del item code steelgo</returns>
        public object ObtenerDetalleRelacionitemCodeSteelgo(string ItemCode, string diam1, string diam2, int mm, Sam3_Usuario usuario,int proyectoID,int cantidad)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_ItemCode item = ctx.Sam3_ItemCode.Where(x => x.Codigo == ItemCode && x.Activo && x.ProyectoID==proyectoID).FirstOrDefault();
                     decimal diametro1 = diam1 != "" ? Convert.ToDecimal(diam1) : 0;
                     decimal diametro2 = diam2 != "" ? Convert.ToDecimal(diam2) : 0;

                     int diametro1IID = (from d in ctx.Sam3_Diametro
                                         where d.Activo && d.Valor == diametro1
                                         select d.DiametroID).AsParallel().SingleOrDefault();

                     int diametro2ID = (from d in ctx.Sam3_Diametro
                                        where d.Activo && d.Valor == diametro2
                                        select d.DiametroID).AsParallel().SingleOrDefault();

                    ItemCodeClienteRetorno detalle = new ItemCodeClienteRetorno();

                    detalle = item!=null ? ((from r in ctx.Sam3_ItemCode
                               join rid in ctx.Sam3_Rel_ItemCode_Diametro on r.ItemCodeID equals rid.ItemCodeID
                               join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                               join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                               join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                               join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                               join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                               where r.Activo && riit.Activo && ics.Activo && rid.Activo && d1.Activo && d2.Activo
                               && r.ItemCodeID == item.ItemCodeID && d1.DiametroID == diametro1IID && d2.DiametroID == diametro2ID
                               select new ItemCodeClienteRetorno
                               {
                                   ItemCode = r.Codigo,
                                   Descripcion = r.DescripcionEspanol,
                                   D1 = d1.Valor,
                                   D2 = d2.Valor,
                                   ItemCodeSteelgo = ics.Codigo,
                                   Familia = (from f in ctx.Sam3_FamiliaAcero where f.FamiliaAceroID == ics.FamiliaAceroID && f.Activo select f.Nombre).FirstOrDefault(),

                                   TipoAcero = (from rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo
                                                join rdis in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on rics.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                join itcs in ctx.Sam3_ItemCodeSteelgo on rdis.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                join fa in ctx.Sam3_FamiliaAcero on itcs.FamiliaAceroID equals fa.FamiliaAceroID
                                                join fm in ctx.Sam3_FamiliaMaterial on fa.FamiliaMaterialID equals fm.FamiliaMaterialID
                                                where rics.Activo && itcs.Activo
                                                && rics.Rel_ItemCode_ItemCodeSteelgo == riit.Rel_ItemCode_ItemCodeSteelgo
                                                select fm.Nombre).FirstOrDefault(),
                                   Colada="",
                                   Cantidad = r.Cantidad,
                                   MM = mm,
                                   Detallar= "No",
                                   TieneNU= "No",
                                   TieneError= false,
                                   BultoID= "",
                                   RelFCId= "",
                                   RelBID= "",
                                   ItemCodeID = rid.Rel_ItemCode_Diametro_ID,
                                   ItemCodeSteelgoID = rids.Rel_ItemCodeSteelgo_Diametro_ID.ToString(),
                                   ItemCodeOrigenID = r.ItemCodeID,
                                   TipoMaterial=  r.TipoMaterialID,
                                   TextoTipoMaterial= (from tm in ctx.Sam3_TipoMaterial
                                                       where tm.TipoMaterialID == r.TipoMaterialID
                                                       select tm.Nombre).FirstOrDefault()
                                   //ColadaNombre = (from c in ctx.Sam3_Colada where c.ColadaID == r.ColadaID && c.Activo select c.NumeroColada).FirstOrDefault(),

                                   //ColadaID = r.ColadaID
                               }).AsParallel().SingleOrDefault()) : null;

                    if (detalle != null)
                    {
                        string diametro = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                           join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                           join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                           join d in ctx.Sam3_Diametro on cat.DiametroID equals d.DiametroID
                                           where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                           && ricsd.Activo && ics.Activo && cat.Activo && d.Activo
                                           select d.Valor.ToString()).AsParallel().SingleOrDefault();

                        string cedulaA = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                          join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                          join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                          join ced in ctx.Sam3_Cedula on cat.CedulaA equals ced.CedulaID
                                          where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                          && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                          select ced.Codigo).AsParallel().SingleOrDefault();

                        string cedulaB = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                          join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                          join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                          join ced in ctx.Sam3_Cedula on cat.CedulaB equals ced.CedulaID
                                          where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                          && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                          select ced.Codigo).AsParallel().SingleOrDefault();

                        string cedulaC = (from ricsd in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro
                                          join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                          join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                          join ced in ctx.Sam3_Cedula on cat.CedulaC equals ced.CedulaID
                                          where ricsd.Rel_ItemCodeSteelgo_Diametro_ID.ToString() == detalle.ItemCodeSteelgoID
                                          && ricsd.Activo && ics.Activo && cat.Activo && ced.Activo
                                          select ced.Codigo).AsParallel().SingleOrDefault();

                        detalle.Cedula = diametro + " - " + cedulaA + " - " + cedulaB + " - " + cedulaC;

                        return detalle;
                    }
                    else {

                        if (item != null)
                        {
                            detalle = (from r in ctx.Sam3_ItemCode
                                       join rid in ctx.Sam3_Rel_ItemCode_Diametro on r.ItemCodeID equals rid.ItemCodeID
                                       join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                       join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                       where r.Activo && rid.Activo
                                         && r.ItemCodeID == item.ItemCodeID
                                         && d1.DiametroID == diametro1IID
                                         && d2.DiametroID == diametro2ID
                                       select new ItemCodeClienteRetorno
                                       {
                                           ItemCodeID = rid.Rel_ItemCode_Diametro_ID,
                                           ItemCode = r.Codigo + "(" + d1.Valor + ", " + d2.Valor + ")",
                                           D1 = d1.Valor,
                                           D2 = d2.Valor,
                                           //ColadaNombre = (from c in ctx.Sam3_Colada where c.ColadaID == r.ColadaID && c.Activo select c.NumeroColada).FirstOrDefault(),
                                           Cantidad = r.Cantidad,
                                           MM = mm,
                                           Descripcion = r.DescripcionEspanol,
                                           ItemCodeOrigenID = r.ItemCodeID,
                                           TipoMaterial = r.TipoMaterialID,
                                           TextoTipoMaterial = (from tm in ctx.Sam3_TipoMaterial
                                                                where tm.TipoMaterialID == r.TipoMaterialID
                                                                select tm.Nombre).FirstOrDefault()
                                       }).AsParallel().SingleOrDefault();
                        }
                        else
                        {
                            
                            ItemCodeClienteRetorno itemRetorno = new ItemCodeClienteRetorno
                            {
                                ItemCode= ItemCode,
                                BultoID= "",
                                Descripcion= "", 
                                D1=decimal.Parse( diam1), 
                                D2=decimal.Parse( diam2), 
                                ItemCodeSteelgo= "",
                                Familia= "", 
                                Cedula= "", 
                                TipoAcero= "", 
                                Colada= "", 
                                Cantidad= cantidad, 
                                MM= mm, 
                                Detallar= "No", 
                                TieneNU= "No", 
                                TieneError= true, 
                                RelFCId= "", 
                                RelBID= "", 
                                ItemCodeID= null, 
                                ItemCodeSteelgoID= "", 
                                ItemCodeOrigenID= null, 
                                TipoMaterial= null, 
                                TextoTipoMaterial= null
                            };
                            detalle = itemRetorno;
                        }
                        return detalle;
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

        public object InsertarRelacionItemCodes(List<AsociacionItemCodeSteelgo> asociaciones, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Rel_ItemCode_ItemCodeSteelgo nuevoRegistro;
                    foreach (AsociacionItemCodeSteelgo asc in asociaciones)
                    {
                        foreach (int i in asc.Itemcodes)
                        {
                            nuevoRegistro = new Sam3_Rel_ItemCode_ItemCodeSteelgo();
                            nuevoRegistro.Activo = true;
                            nuevoRegistro.FechaModificacion = DateTime.Now;
                            nuevoRegistro.ItemCodeID = i;
                            nuevoRegistro.ItemCodeSteelgoID = asc.ItemCodeSteelgo;
                            nuevoRegistro.UsuarioModificacion = usuario.UsuarioID;

                            ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo.Add(nuevoRegistro);
                        }

                    }
                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
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

        public object InsertarItemCodeSteelgo(ItemCodeSteelgoJson json, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_ItemCodeSteelgo nuevoItem = new Sam3_ItemCodeSteelgo();
                    nuevoItem.Activo = true;
                    nuevoItem.Area = json.Area;
                    nuevoItem.CedulaID = json.CedulaID;
                    nuevoItem.DescripcionEspanol = json.DescripcionEspanol;
                    nuevoItem.DescripcionIngles = json.DescripcionIngles;
                    //nuevoItem.Diametro1 = json.Diametro1;
                    //nuevoItem.Diametro2 = json.Diametro2;
                    nuevoItem.FamiliaAceroID = json.FamiliaAceroID;
                    nuevoItem.FechaModificacion = DateTime.Now;
                    nuevoItem.Peso = json.Peso;
                    nuevoItem.UsuarioModificacion = usuario.UsuarioID;
                    nuevoItem.Codigo = json.Codigo;

                    ctx.Sam3_ItemCodeSteelgo.Add(nuevoItem);

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add(nuevoItem.ItemCodeSteelgoID.ToString());
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

        public object ActualizarItemCodeSteelgo(ItemCodeSteelgoJson json, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_ItemCodeSteelgo itemBd = ctx.Sam3_ItemCodeSteelgo.Where(x => x.ItemCodeSteelgoID == json.ItemCodeSteelgoID)
                        .AsParallel().SingleOrDefault();

                    itemBd.Area = json.Area;
                    itemBd.CedulaID = json.CedulaID;
                    itemBd.DescripcionEspanol = json.DescripcionEspanol;
                    itemBd.DescripcionIngles = json.DescripcionIngles;
                    //itemBd.Diametro1 = json.Diametro1;
                    //itemBd.Diametro2 = json.Diametro2;
                    itemBd.FamiliaAceroID = json.FamiliaAceroID;
                    itemBd.FechaModificacion = DateTime.Now;
                    itemBd.Peso = json.Peso;
                    itemBd.UsuarioModificacion = usuario.UsuarioID;
                    itemBd.Codigo = json.Codigo;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
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

        public object EliminarItemCodeSteelgo(int itemCodeSteelgoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_ItemCodeSteelgo itemBd = ctx.Sam3_ItemCodeSteelgo.Where(x => x.ItemCodeSteelgoID == itemCodeSteelgoID)
                        .AsParallel().SingleOrDefault();

                    itemBd.Activo = false;
                    itemBd.FechaModificacion = DateTime.Now;
                    itemBd.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
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
        /// Obtener los item code steelgo por diametros y proyecto
        /// </summary>
        /// <returns>Lista de ICS</returns>
        public object ObtenerListadoItemCodeSteelgo(int proyectoID, Sam3_Usuario usuario)
        {
            try
            {
                //List<ListaCombos> ics = new List<ListaCombos>();
                using (SamContext ctx = new SamContext())
                {
                    //ics.Add(new ListaCombos { id = "0", value = "Agregar Nuevo" });
                    List<ListaComboItemCodeEdicion> listado = (from it in ctx.Sam3_ItemCode
                                                 //join rid in ctx.Sam3_Rel_ItemCode_Diametro on it.ItemCodeID equals rid.ItemCodeID
                                                 //join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                 //join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                 //join its in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals its.ItemCodeSteelgoID
                                                 //join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                                 //join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                                 where it.Activo //&& rid.Activo && riit.Activo && rids.Activo && its.Activo && d1.Activo && d2.Activo
                                                 && it.ProyectoID == proyectoID
                                                 select new ListaComboItemCodeEdicion
                                                 {
                                                     id = it.ItemCodeID.ToString(),
                                                     value = it.Codigo,// + "(" + d1.Valor.ToString() + ", " + d2.Valor.ToString() + ")"
                                                     Descripcion = it.DescripcionEspanol
                                                 }).AsParallel().ToList();
                    //ics.AddRange(listado);
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