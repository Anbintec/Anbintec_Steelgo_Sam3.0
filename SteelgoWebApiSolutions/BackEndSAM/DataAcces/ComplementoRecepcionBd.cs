using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseManager.Sam3;
using DatabaseManager.Sam2;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Transactions;
using System.Web.Script.Serialization;
using System.Configuration;

namespace BackEndSAM.DataAcces
{
    public class ComplementoRecepcionBd
    {
        private static readonly object _mutex = new object();
        private static ComplementoRecepcionBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ComplementoRecepcionBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ComplementoRecepcionBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ComplementoRecepcionBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListado(int folioCuantificacionID, Sam3_Usuario usuario, bool soloListado = false)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ItemCodeComplemento> listado = new List<ItemCodeComplemento>();
                    //Agregamos items con relacion con Folio Cuantificacion
                    listado.AddRange((from fc in ctx.Sam3_FolioCuantificacion
                                      join rfi in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on fc.FolioCuantificacionID equals rfi.FolioCuantificacionID
                                      join rel in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB on rfi.Rel_FolioCuantificacion_ItemCode_ID equals rel.Rel_FolioCuantificacion_ItemCode_ID
                                      join nu in ctx.Sam3_NumeroUnico on rel.NumeroUnicoID equals nu.NumeroUnicoID
                                      join it in ctx.Sam3_ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                      join nui in ctx.Sam3_NumeroUnicoInventario  on nu.NumeroUnicoID equals  nui.NumeroUnicoID 
                                      where nui.Activo && fc.Activo && rfi.Activo && it.Activo && nu.Activo && rel.Activo //&& nu.NumeroUnicoID == 10945
                                      && fc.FolioCuantificacionID == folioCuantificacionID
                                      select new ItemCodeComplemento
                                      {
                                          NumeroUnicoID = nu.NumeroUnicoID.ToString(),
                                          NumeroUnico = nu.Prefijo + "-" + nu.Consecutivo,
                                          ItemCode = it.Codigo,
                                          NumeroUnicoCliente = nu.NumeroUnicoCliente,
                                          Descripcion = it.DescripcionEspanol,
                                          TipoAcero = (from rdi in ctx.Sam3_Rel_ItemCode_Diametro
                                                       join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rdi.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                       join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                       join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                       join fa in ctx.Sam3_FamiliaAcero on ics.FamiliaAceroID equals fa.FamiliaAceroID
                                                       where riit.Activo && rids.Activo && ics.Activo && fa.Activo
                                                       && rdi.Diametro1ID == (from d5 in ctx.Sam3_Diametro
                                                                              where d5.Activo && d5.Valor == nu.Diametro1
                                                                              select d5.DiametroID).FirstOrDefault()
                                                       && rdi.Diametro2ID == (from d4 in ctx.Sam3_Diametro
                                                                              where d4.Activo && d4.Valor == nu.Diametro2
                                                                              select d4.DiametroID).FirstOrDefault()
                                                       && rdi.ItemCodeID == it.ItemCodeID
                                                       select fa.Nombre).FirstOrDefault(),
                                          ItemCodeSteelgoID = (from rdi in ctx.Sam3_Rel_ItemCode_Diametro
                                                               join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rdi.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                               join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                               join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                               join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                                               where riit.Activo && rids.Activo && ics.Activo && cat.Activo
                                                               && rdi.ItemCodeID == it.ItemCodeID
                                                               && rdi.Diametro1ID == (from d in ctx.Sam3_Diametro
                                                                                      where d.Activo && d.Valor == nu.Diametro1
                                                                                      select d.DiametroID).FirstOrDefault()
                                                               && rdi.Diametro2ID == (from d1 in ctx.Sam3_Diametro
                                                                                      where d1.Activo && d1.Valor == nu.Diametro2
                                                                                      select d1.DiametroID).FirstOrDefault()
                                                               select ics.ItemCodeSteelgoID).FirstOrDefault(),
                                          D1 = nu.Diametro1.ToString(),
                                          D2 = nu.Diametro2.ToString(),
                                          ItemCodeID = it.ItemCodeID,
                                          ProyectoID = it.ProyectoID,
                                          Cantidad = (from n in ctx.Sam3_NumeroUnico
                                                      where n.NumeroUnicoID == nu.NumeroUnicoID
                                                      select n).Count(),
                                          MM = nui.CantidadRecibida.ToString(),//rel.MM.ToString(),
                                          Colada =(from c in ctx.Sam3_Colada where c.Activo && c.ColadaID== nu.ColadaID select c.NumeroColada ).FirstOrDefault(),  //nu.Sam3_Colada.NumeroColada,
                                          EstatusDocumental = nu.EstatusDocumental,
                                          EstatusFisico = nu.EstatusFisico,
                                          TipoUso = (from c in ctx.Sam3_TipoUso where c.Activo && c.TipoUsoID == nu.TipoUsoID select c.Nombre).FirstOrDefault(),  //nu.Sam3_TipoUso.Nombre,
                                          RelFCID = rel.Rel_FolioCuantificacion_ItemCode_ID.ToString(),
                                          RelNUFCBID = rel.Rel_NumeroUnico_RelFC_RelB_ID.ToString(),
                                          ColadaOriginal = (from c in ctx.Sam3_Colada where c.Activo && c.ColadaID == nu.ColadaID select c.NumeroColada).FirstOrDefault(),//nu.Sam3_Colada.NumeroColada,
                                          TieneComplementoRecepcion = it.TieneComplementoRecepcion ? "Si" : "No",
                                          MTRID = nu.MTRID.ToString(),
                                          Diametro1ID = (from d2 in ctx.Sam3_Diametro
                                                         where d2.Activo && d2.Valor == nu.Diametro1
                                                         select d2.DiametroID).FirstOrDefault(),
                                          Diametro2ID = (from d3 in ctx.Sam3_Diametro
                                                         where d3.Activo && d3.Valor == nu.Diametro2
                                                         select d3.DiametroID).FirstOrDefault()
                                      }).AsParallel().Distinct().ToList());

                    foreach (var item in listado)
                    {
                        item.MTR = (from mtr in ctx.Sam3_MTR
                                    where mtr.Activo && mtr.MTRID.ToString() == item.MTRID
                                    select mtr.NumeroMTR.ToString()).AsParallel().SingleOrDefault();
                        item.CantidadPiezasMTR = (from mtr in ctx.Sam3_MTR
                                                  where mtr.Activo && mtr.MTRID.ToString() == item.MTRID
                                                  select mtr.CantidadPiezas.ToString()).AsParallel().SingleOrDefault();
                    }

                    //agregar items en bulto
                    listado.AddRange((from fc in ctx.Sam3_FolioCuantificacion
                                      join b in ctx.Sam3_Bulto on fc.FolioCuantificacionID equals b.FolioCuantificacionID
                                      join rbi in ctx.Sam3_Rel_Bulto_ItemCode on b.BultoID equals rbi.BultoID
                                      join rel in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB on rbi.Rel_Bulto_ItemCode_ID equals rel.Rel_Bulto_ItemCode_ID
                                      join nu in ctx.Sam3_NumeroUnico on rel.NumeroUnicoID equals nu.NumeroUnicoID
                                      join it in ctx.Sam3_ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                      where fc.Activo && b.Activo && rbi.Activo && it.Activo && nu.Activo && rel.Activo
                                      && fc.FolioCuantificacionID == folioCuantificacionID
                                      select new ItemCodeComplemento
                                      {
                                          NumeroUnicoID = nu.NumeroUnicoID.ToString(),
                                          NumeroUnico = nu.Prefijo + "-" + nu.Consecutivo,
                                          ItemCode = it.Codigo,
                                          NumeroUnicoCliente = nu.NumeroUnicoCliente,
                                          Descripcion = it.DescripcionEspanol,
                                          TipoAcero = (from rdi in ctx.Sam3_Rel_ItemCode_Diametro
                                                       join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rdi.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                       join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                       join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                       join fa in ctx.Sam3_FamiliaAcero on ics.FamiliaAceroID equals fa.FamiliaAceroID
                                                       where riit.Activo && rids.Activo && ics.Activo && fa.Activo
                                                       && rdi.Diametro1ID == (from d5 in ctx.Sam3_Diametro
                                                                              where d5.Activo && d5.Valor == nu.Diametro1
                                                                              select d5.DiametroID).FirstOrDefault()
                                                       && rdi.Diametro2ID == (from d4 in ctx.Sam3_Diametro
                                                                              where d4.Activo && d4.Valor == nu.Diametro2
                                                                              select d4.DiametroID).FirstOrDefault()
                                                       && rdi.ItemCodeID == it.ItemCodeID
                                                       select fa.Nombre).FirstOrDefault(),
                                          ItemCodeSteelgoID = (from rdi in ctx.Sam3_Rel_ItemCode_Diametro
                                                               join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rdi.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                               join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                               join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                               join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                                               where riit.Activo && rids.Activo && ics.Activo && cat.Activo
                                                               && rdi.ItemCodeID == it.ItemCodeID
                                                               && rdi.Diametro1ID == (from d in ctx.Sam3_Diametro
                                                                                      where d.Activo && d.Valor == nu.Diametro1
                                                                                      select d.DiametroID).FirstOrDefault()
                                                               && rdi.Diametro2ID == (from d1 in ctx.Sam3_Diametro
                                                                                      where d1.Activo && d1.Valor == nu.Diametro2
                                                                                      select d1.DiametroID).FirstOrDefault()
                                                               select ics.ItemCodeSteelgoID).FirstOrDefault(),
                                          D1 = nu.Diametro1.ToString(),
                                          D2 = nu.Diametro2.ToString(),
                                          ItemCodeID = it.ItemCodeID,
                                          ProyectoID = it.ProyectoID,
                                          Cantidad = (from n in ctx.Sam3_NumeroUnico
                                                      where n.NumeroUnicoID == nu.NumeroUnicoID
                                                      select n).Count(),
                                          MM = rel.MM.ToString(),
                                          Colada = (from c in ctx.Sam3_Colada where c.Activo && c.ColadaID == nu.ColadaID select c.NumeroColada).FirstOrDefault(), //nu.Sam3_Colada.NumeroColada,
                                          EstatusDocumental = nu.EstatusDocumental,
                                          EstatusFisico = nu.EstatusFisico,
                                          TipoUso = (from c in ctx.Sam3_TipoUso where c.Activo && c.TipoUsoID == nu.TipoUsoID select c.Nombre).FirstOrDefault(),//nu.Sam3_TipoUso.Nombre,
                                          RelNUFCBID = rel.Rel_NumeroUnico_RelFC_RelB_ID.ToString(),
                                          RelBID = rel.Rel_Bulto_ItemCode_ID.ToString(),
                                          ColadaOriginal = (from c in ctx.Sam3_Colada where c.Activo && c.ColadaID == nu.ColadaID select c.NumeroColada).FirstOrDefault(), //nu.Sam3_Colada.NumeroColada,
                                          TieneComplementoRecepcion = it.TieneComplementoRecepcion ? "Si" : "No",
                                          Diametro1ID = (from d2 in ctx.Sam3_Diametro
                                                         where d2.Activo && d2.Valor == nu.Diametro1
                                                         select d2.DiametroID).FirstOrDefault(),
                                          Diametro2ID = (from d3 in ctx.Sam3_Diametro
                                                         where d3.Activo && d3.Valor == nu.Diametro2
                                                         select d3.DiametroID).FirstOrDefault()
                                      }
                        ).AsParallel().Distinct().ToList());

                    foreach (ItemCodeComplemento item in listado)
                    {
                        int numeroDigitos = ctx.Sam3_ProyectoConfiguracion.Where(x => x.ProyectoID == item.ProyectoID)
                            .Select(x => x.DigitosNumeroUnico).AsParallel().SingleOrDefault();

                        string formato = "D" + numeroDigitos.ToString();

                        string[] elementos = item.NumeroUnico.Split('-').ToArray();

                        int temp = Convert.ToInt32(elementos[1]);

                        item.NumeroUnico = elementos[0] + "-" + temp.ToString(formato);


                        string diametro = (from ics in ctx.Sam3_ItemCodeSteelgo
                                               //join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                           join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                           join d in ctx.Sam3_Diametro on cat.DiametroID equals d.DiametroID
                                           where ics.ItemCodeSteelgoID == item.ItemCodeSteelgoID
                                           && ics.Activo && cat.Activo && d.Activo
                                           select d.Valor.ToString()).AsParallel().SingleOrDefault();

                        string cedulaA = (from ics in ctx.Sam3_ItemCodeSteelgo
                                              //join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                          join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                          join ced in ctx.Sam3_Cedula on cat.CedulaA equals ced.CedulaID
                                          where ics.ItemCodeSteelgoID == item.ItemCodeSteelgoID
                                          && ics.Activo && cat.Activo && ced.Activo
                                          select ced.Codigo).AsParallel().SingleOrDefault();

                        string cedulaB = (from ics in ctx.Sam3_ItemCodeSteelgo
                                              //join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                          join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                          join ced in ctx.Sam3_Cedula on cat.CedulaB equals ced.CedulaID
                                          where ics.ItemCodeSteelgoID == item.ItemCodeSteelgoID
                                          && ics.Activo && cat.Activo && ced.Activo
                                          select ced.Codigo).AsParallel().SingleOrDefault();

                        string cedulaC = (from ics in ctx.Sam3_ItemCodeSteelgo
                                              //join ics in ctx.Sam3_ItemCodeSteelgo on ricsd.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                          join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                          join ced in ctx.Sam3_Cedula on cat.CedulaC equals ced.CedulaID
                                          where ics.ItemCodeSteelgoID == item.ItemCodeSteelgoID
                                          && ics.Activo && cat.Activo && ced.Activo
                                          select ced.Codigo).AsParallel().SingleOrDefault();

                        item.Cedula = item.ItemCodeSteelgoID == 1 || item.ItemCodeSteelgoID == 0 ? "" : diametro + " - " + cedulaA + " - " + cedulaB + " - " + cedulaC;

                        //item.Cedula = item.ItemCodeSteelgoID == 1 ? "" : item.Cedula;

                        item.TipoAcero = item.ItemCodeSteelgoID == 1 ? "" : item.TipoAcero;
                    }

                    listado = listado.OrderBy(x => x.NumeroUnico).ToList();

                    List<object> lstReturn = new List<object>();
                    string Estatus = ctx.Sam3_FolioCuantificacion.Where(x => x.FolioCuantificacionID == folioCuantificacionID)
                        .Select(x => x.Estatus).SingleOrDefault();

                    lstReturn.Add(Estatus);
                    lstReturn.Add(listado);

#if DEBUG
                    //JavaScriptSerializer serializer = new JavaScriptSerializer();
                    //string json = serializer.Serialize(lstReturn);
#endif
                    if (soloListado)
                    {
                        return listado;
                    }
                    else
                    {
                        return lstReturn;
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

        private ItemCodeComplemento ObtenerPropiedadesJson(int relFCID = 0, int RelBID = 0, int RelNUFCBID = 0, double diam1 = 0, double diam2 = 0)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ItemCodeComplemento item = new ItemCodeComplemento();
                    //Agregamos items con relacion con Folio Cuantificacion
                    if (relFCID > 0)
                    {
                        item = (from rel in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                join nu in ctx.Sam3_NumeroUnico on rel.NumeroUnicoID equals nu.NumeroUnicoID
                                join rfi in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on rel.Rel_FolioCuantificacion_ItemCode_ID equals rfi.Rel_FolioCuantificacion_ItemCode_ID
                                join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                where it.Activo && nu.Activo && rel.Activo && rfi.Activo && rid.Activo
                                && rel.Rel_NumeroUnico_RelFC_RelB_ID == RelNUFCBID
                                select new ItemCodeComplemento
                                {
                                    NumeroUnico = nu.Prefijo + "-" + nu.Consecutivo,
                                    NumeroUnicoID = nu.NumeroUnicoID.ToString(),
                                    ItemCode = it.Codigo,
                                    NumeroUnicoCliente = nu.NumeroUnicoCliente,
                                    Descripcion = it.DescripcionEspanol,

                                    TipoAcero = (from rfii in ctx.Sam3_Rel_FolioCuantificacion_ItemCode
                                                 join rdi in ctx.Sam3_Rel_ItemCode_Diametro on rfii.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                 join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rdi.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                 join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                 join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                 join fa in ctx.Sam3_FamiliaAcero on ics.FamiliaAceroID equals fa.FamiliaAceroID
                                                 where riit.Activo && rids.Activo && ics.Activo && fa.Activo
                                                 && rfii.Rel_FolioCuantificacion_ItemCode_ID == rfi.Rel_FolioCuantificacion_ItemCode_ID
                                                 select fa.Nombre).FirstOrDefault(),
                                    D1 = d1.Valor.ToString(),
                                    D2 = d2.Valor.ToString(),
                                    ItemCodeID = it.ItemCodeID,
                                    ProyectoID = it.ProyectoID,
                                    Cantidad = (from n in ctx.Sam3_NumeroUnico
                                                where n.NumeroUnicoID == nu.NumeroUnicoID
                                                select n).Count(),
                                    MM = rel.MM.ToString(),
                                    Colada = (from c in ctx.Sam3_Colada
                                              where c.ColadaID == rfi.ColadaID
                                              select c.NumeroColada).FirstOrDefault(),
                                    EstatusDocumental = nu.EstatusDocumental,
                                    EstatusFisico = nu.EstatusFisico,
                                    TipoUso = (from tu in ctx.Sam3_TipoUso
                                               where tu.Activo && tu.TipoUsoID == nu.TipoUsoID
                                               select tu.Nombre).FirstOrDefault(),
                                    ColadaID = rfi.ColadaID,
                                    RelFCID = rfi.Rel_FolioCuantificacion_ItemCode_ID.ToString(),
                                    RelNUFCBID = rel.Rel_NumeroUnico_RelFC_RelB_ID.ToString(),
                                    Titulo = "",
                                    DescripcionIncidencia = "",
                                    ColadaOriginal = (from c in ctx.Sam3_Colada
                                                      where c.ColadaID == rfi.ColadaID
                                                      select c.NumeroColada).FirstOrDefault(),
                                    Diametro1ID = d1.DiametroID,
                                    Diametro2ID = d2.DiametroID
                                }).AsParallel().SingleOrDefault();

                        if (item != null)
                        {
                            string diametro = (from ic in ctx.Sam3_ItemCode
                                               join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                               join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                               join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                               join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                               join d in ctx.Sam3_Diametro on cat.DiametroID equals d.DiametroID
                                               where ic.ItemCodeID == item.ItemCodeID && icd.Diametro1ID == diam1 && icd.Diametro2ID == diam1
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && d.Activo
                                               select d.Valor.ToString()).AsParallel().SingleOrDefault();

                            string cedulaA = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaA equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID && icd.Diametro1ID == diam1 && icd.Diametro2ID == diam1
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaB = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaB equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID && icd.Diametro1ID == diam1 && icd.Diametro2ID == diam1
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaC = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaC equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID && icd.Diametro1ID == diam1 && icd.Diametro2ID == diam1
                                              && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            item.Cedula = diametro + " - " + cedulaA + " - " + cedulaB + " - " + cedulaC;
                        }
                    }

                    if (RelBID > 0)
                    {
                        item = (from rel in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                join nu in ctx.Sam3_NumeroUnico on rel.NumeroUnicoID equals nu.NumeroUnicoID
                                join rbi in ctx.Sam3_Rel_Bulto_ItemCode on rel.Rel_Bulto_ItemCode_ID equals rbi.Rel_Bulto_ItemCode_ID
                                join rid in ctx.Sam3_Rel_ItemCode_Diametro on rbi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                where it.Activo && nu.Activo && rel.Activo && rbi.Activo && rid.Activo
                                && rel.Rel_NumeroUnico_RelFC_RelB_ID == RelNUFCBID
                                select new ItemCodeComplemento
                                {
                                    NumeroUnico = nu.Prefijo + "-" + nu.Consecutivo,
                                    NumeroUnicoID = nu.NumeroUnicoID.ToString(),
                                    ItemCode = it.Codigo,
                                    NumeroUnicoCliente = nu.NumeroUnicoCliente,
                                    Descripcion = it.DescripcionEspanol,
                                    TipoAcero = (from rbii in ctx.Sam3_Rel_Bulto_ItemCode
                                                 join rdi in ctx.Sam3_Rel_ItemCode_Diametro on rbii.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                 join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rdi.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                 join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                 join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                 join fa in ctx.Sam3_FamiliaAcero on ics.FamiliaAceroID equals fa.FamiliaAceroID
                                                 where riit.Activo && rids.Activo && ics.Activo && fa.Activo
                                                 && rbii.Rel_Bulto_ItemCode_ID == rbi.Rel_Bulto_ItemCode_ID
                                                 select fa.Nombre).FirstOrDefault(),
                                    D1 = d1.Valor.ToString(),
                                    D2 = d2.Valor.ToString(),
                                    ItemCodeID = it.ItemCodeID,
                                    ProyectoID = it.ProyectoID,
                                    Cantidad = (from n in ctx.Sam3_NumeroUnico
                                                where n.NumeroUnicoID == nu.NumeroUnicoID
                                                select n).Count(),
                                    MM = rel.MM.ToString(),
                                    Colada = (from c in ctx.Sam3_Colada
                                              where c.ColadaID == rbi.ColadaID
                                              select c.NumeroColada).FirstOrDefault(),
                                    EstatusDocumental = nu.EstatusDocumental,
                                    EstatusFisico = nu.EstatusFisico,
                                    TipoUso = (from tu in ctx.Sam3_TipoUso
                                               where tu.Activo && tu.TipoUsoID == nu.TipoUsoID
                                               select tu.Nombre).FirstOrDefault(),
                                    ColadaID = rbi.ColadaID,
                                    RelBID = rbi.Rel_Bulto_ItemCode_ID.ToString(),
                                    RelNUFCBID = rel.Rel_NumeroUnico_RelFC_RelB_ID.ToString(),
                                    Titulo = "",
                                    DescripcionIncidencia = "",
                                    ColadaOriginal = (from c in ctx.Sam3_Colada
                                                      where c.ColadaID == rbi.ColadaID
                                                      select c.NumeroColada).FirstOrDefault(),
                                    Diametro1ID = d1.DiametroID,
                                    Diametro2ID = d2.DiametroID
                                }).AsParallel().SingleOrDefault();

                        if (item != null)
                        {
                            string diametro = (from ic in ctx.Sam3_ItemCode
                                               join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                               join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                               join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                               join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                               join d in ctx.Sam3_Diametro on cat.DiametroID equals d.DiametroID
                                               where ic.ItemCodeID == item.ItemCodeID && icd.Diametro1ID == diam1 && icd.Diametro2ID == diam1
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && d.Activo
                                               select d.Valor.ToString()).AsParallel().SingleOrDefault();

                            string cedulaA = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaA equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID && icd.Diametro1ID == diam1 && icd.Diametro2ID == diam1
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaB = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaB equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID && icd.Diametro1ID == diam1 && icd.Diametro2ID == diam1
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaC = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaC equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID && icd.Diametro1ID == diam1 && icd.Diametro2ID == diam1
                                              && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            item.Cedula = diametro + " - " + cedulaA + " - " + cedulaB + " - " + cedulaC;
                        }
                    }

                    if (item == null)
                    {
                        throw new Exception("Error al obtener las propiedades del Número único");
                    }


                    int numeroDigitos = ctx.Sam3_ProyectoConfiguracion.Where(x => x.ProyectoID == item.ProyectoID)
                        .Select(x => x.DigitosNumeroUnico).AsParallel().SingleOrDefault();

                    string formato = "D" + numeroDigitos.ToString();

                    string[] elementos = item.NumeroUnico.Split('-').ToArray();

                    int temp = Convert.ToInt32(elementos[1]);

                    item.NumeroUnico = elementos[0] + "-" + temp.ToString(formato);

#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(item);
#endif

                    return item;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return null;
            }
        }

        private ItemCodeComplemento ObtenerPropiedadesJson(int relFCID = 0, int RelBID = 0, int RelNUFCBID = 0)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ItemCodeComplemento item = new ItemCodeComplemento();
                    //Agregamos items con relacion con Folio Cuantificacion
                    if (relFCID > 0)
                    {
                        item = (from rel in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                join nu in ctx.Sam3_NumeroUnico on rel.NumeroUnicoID equals nu.NumeroUnicoID
                                join rfi in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on rel.Rel_FolioCuantificacion_ItemCode_ID equals rfi.Rel_FolioCuantificacion_ItemCode_ID
                                join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                where it.Activo && nu.Activo && rel.Activo && rfi.Activo && rid.Activo
                                && rel.Rel_NumeroUnico_RelFC_RelB_ID == RelNUFCBID
                                select new ItemCodeComplemento
                                {
                                    NumeroUnico = nu.Prefijo + "-" + nu.Consecutivo,
                                    NumeroUnicoID = nu.NumeroUnicoID.ToString(),
                                    ItemCode = it.Codigo,
                                    NumeroUnicoCliente = nu.NumeroUnicoCliente,
                                    Descripcion = it.DescripcionEspanol,

                                    TipoAcero = (from rfii in ctx.Sam3_Rel_FolioCuantificacion_ItemCode
                                                 join rdi in ctx.Sam3_Rel_ItemCode_Diametro on rfii.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                 join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rdi.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                 join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                 join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                 join fa in ctx.Sam3_FamiliaAcero on ics.FamiliaAceroID equals fa.FamiliaAceroID
                                                 where riit.Activo && rids.Activo && ics.Activo && fa.Activo
                                                 && rfii.Rel_FolioCuantificacion_ItemCode_ID == rfi.Rel_FolioCuantificacion_ItemCode_ID
                                                 select fa.Nombre).FirstOrDefault(),
                                    D1 = d1.Valor.ToString(),
                                    D2 = d2.Valor.ToString(),
                                    ItemCodeID = it.ItemCodeID,
                                    ProyectoID = it.ProyectoID,
                                    Cantidad = (from n in ctx.Sam3_NumeroUnico
                                                where n.NumeroUnicoID == nu.NumeroUnicoID
                                                select n).Count(),
                                    MM = rel.MM.ToString(),
                                    Colada = (from c in ctx.Sam3_Colada
                                              where c.ColadaID == rfi.ColadaID
                                              select c.NumeroColada).FirstOrDefault(),
                                    EstatusDocumental = nu.EstatusDocumental,
                                    EstatusFisico = nu.EstatusFisico,
                                    TipoUso = (from tu in ctx.Sam3_TipoUso
                                               where tu.Activo && tu.TipoUsoID == nu.TipoUsoID
                                               select tu.Nombre).FirstOrDefault(),
                                    ColadaID = rfi.ColadaID,
                                    RelFCID = rfi.Rel_FolioCuantificacion_ItemCode_ID.ToString(),
                                    RelNUFCBID = rel.Rel_NumeroUnico_RelFC_RelB_ID.ToString(),
                                    Titulo = "",
                                    DescripcionIncidencia = "",
                                    ColadaOriginal = (from c in ctx.Sam3_Colada
                                                      where c.ColadaID == rfi.ColadaID
                                                      select c.NumeroColada).FirstOrDefault(),
                                    Diametro1ID = d1.DiametroID,
                                    Diametro2ID = d2.DiametroID
                                }).AsParallel().SingleOrDefault();

                        if (item != null)
                        {
                            string diametro = (from ic in ctx.Sam3_ItemCode
                                               join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                               join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                               join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                               join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                               join d in ctx.Sam3_Diametro on cat.DiametroID equals d.DiametroID
                                               where ic.ItemCodeID == item.ItemCodeID
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && d.Activo
                                               select d.Valor.ToString()).AsParallel().SingleOrDefault();

                            string cedulaA = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaA equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaB = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaB equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaC = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaC equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID
                                              && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            item.Cedula = diametro + " - " + cedulaA + " - " + cedulaB + " - " + cedulaC;
                        }
                    }

                    if (RelBID > 0)
                    {
                        item = (from rel in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                join nu in ctx.Sam3_NumeroUnico on rel.NumeroUnicoID equals nu.NumeroUnicoID
                                join rbi in ctx.Sam3_Rel_Bulto_ItemCode on rel.Rel_Bulto_ItemCode_ID equals rbi.Rel_Bulto_ItemCode_ID
                                join rid in ctx.Sam3_Rel_ItemCode_Diametro on rbi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                where it.Activo && nu.Activo && rel.Activo && rbi.Activo && rid.Activo
                                && rel.Rel_NumeroUnico_RelFC_RelB_ID == RelNUFCBID
                                select new ItemCodeComplemento
                                {
                                    NumeroUnico = nu.Prefijo + "-" + nu.Consecutivo,
                                    NumeroUnicoID = nu.NumeroUnicoID.ToString(),
                                    ItemCode = it.Codigo,
                                    NumeroUnicoCliente = nu.NumeroUnicoCliente,
                                    Descripcion = it.DescripcionEspanol,
                                    TipoAcero = (from rbii in ctx.Sam3_Rel_Bulto_ItemCode
                                                 join rdi in ctx.Sam3_Rel_ItemCode_Diametro on rbii.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                 join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rdi.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                 join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                 join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                 join fa in ctx.Sam3_FamiliaAcero on ics.FamiliaAceroID equals fa.FamiliaAceroID
                                                 where riit.Activo && rids.Activo && ics.Activo && fa.Activo
                                                 && rbii.Rel_Bulto_ItemCode_ID == rbi.Rel_Bulto_ItemCode_ID
                                                 select fa.Nombre).FirstOrDefault(),
                                    D1 = d1.Valor.ToString(),
                                    D2 = d2.Valor.ToString(),
                                    ItemCodeID = it.ItemCodeID,
                                    ProyectoID = it.ProyectoID,
                                    Cantidad = (from n in ctx.Sam3_NumeroUnico
                                                where n.NumeroUnicoID == nu.NumeroUnicoID
                                                select n).Count(),
                                    MM = rel.MM.ToString(),
                                    Colada = (from c in ctx.Sam3_Colada
                                              where c.ColadaID == rbi.ColadaID
                                              select c.NumeroColada).FirstOrDefault(),
                                    EstatusDocumental = nu.EstatusDocumental,
                                    EstatusFisico = nu.EstatusFisico,
                                    TipoUso = (from tu in ctx.Sam3_TipoUso
                                               where tu.Activo && tu.TipoUsoID == nu.TipoUsoID
                                               select tu.Nombre).FirstOrDefault(),
                                    ColadaID = rbi.ColadaID,
                                    RelBID = rbi.Rel_Bulto_ItemCode_ID.ToString(),
                                    RelNUFCBID = rel.Rel_NumeroUnico_RelFC_RelB_ID.ToString(),
                                    Titulo = "",
                                    DescripcionIncidencia = "",
                                    ColadaOriginal = (from c in ctx.Sam3_Colada
                                                      where c.ColadaID == rbi.ColadaID
                                                      select c.NumeroColada).FirstOrDefault(),
                                    Diametro1ID = d1.DiametroID,
                                    Diametro2ID = d2.DiametroID
                                }).AsParallel().SingleOrDefault();

                        if (item != null)
                        {
                            string diametro = (from ic in ctx.Sam3_ItemCode
                                               join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                               join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                               join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                               join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                               join d in ctx.Sam3_Diametro on cat.DiametroID equals d.DiametroID
                                               where ic.ItemCodeID == item.ItemCodeID
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && d.Activo
                                               select d.Valor.ToString()).AsParallel().SingleOrDefault();

                            string cedulaA = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaA equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaB = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaB equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID
                                               && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            string cedulaC = (from ic in ctx.Sam3_ItemCode
                                              join icd in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals icd.ItemCodeID
                                              join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on icd.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                              join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                              join cat in ctx.Sam3_CatalogoCedulas on ics.CedulaID equals cat.CatalogoCedulasID
                                              join ced in ctx.Sam3_Cedula on cat.CedulaC equals ced.CedulaID
                                              where ic.ItemCodeID == item.ItemCodeID
                                              && ic.Activo && icd.Activo && rics.Activo && ics.Activo && cat.Activo && ced.Activo
                                              select ced.Codigo).AsParallel().SingleOrDefault();

                            item.Cedula = diametro + " - " + cedulaA + " - " + cedulaB + " - " + cedulaC;
                        }
                    }

                    if (item == null)
                    {
                        throw new Exception("Error al obtener las propiedades del Número único");
                    }


                    int numeroDigitos = ctx.Sam3_ProyectoConfiguracion.Where(x => x.ProyectoID == item.ProyectoID)
                        .Select(x => x.DigitosNumeroUnico).AsParallel().SingleOrDefault();

                    string formato = "D" + numeroDigitos.ToString();

                    string[] elementos = item.NumeroUnico.Split('-').ToArray();

                    int temp = Convert.ToInt32(elementos[1]);

                    item.NumeroUnico = elementos[0] + "-" + temp.ToString(formato);

#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(item);
#endif

                    return item;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return null;
            }
        }

        public object GuardarComplemento(int tipoGuardadoID, ItemCodeComplemento itemCodeJson, Sam3_Usuario usuario)
        {
            try
            {
                bool dañado = false;
                bool aprobado = false;
                int relFcId = itemCodeJson.RelFCID != null && itemCodeJson.RelFCID != "" ? Convert.ToInt32(itemCodeJson.RelFCID) : 0;
                int relBId = itemCodeJson.RelBID != null && itemCodeJson.RelBID != "" ? Convert.ToInt32(itemCodeJson.RelBID) : 0;
                int relNuId = itemCodeJson.RelNUFCBID != null && itemCodeJson.RelNUFCBID != "" ? Convert.ToInt32(itemCodeJson.RelNUFCBID) : 0;
                NumeroUnico actualizaNumSam2 = null;
                Sam3_NumeroUnicoSegmento segmento = null;
                Sam3_NumeroUnicoMovimiento movimiento = null;
                NumeroUnico sam2_numeroUnico = null;
                NumeroUnicoSegmento segmentoSam2 = null;
                Sam3_NumeroUnico actualizaNU = null;
                int cantidadRecibida = 0;
                int cantidadDañada = 0;
                int buenEstado = 0;
                int congelado = 0;
                int disponibleCruce = 0;
                int cantidadDespachada = 0;
                int fisico = 0;

                TransactionalInformation result = new TransactionalInformation();
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var ctx2_tran = ctx2.Database.BeginTransaction())
                            {
                                Sam3_ItemCode actualizaItem = ctx.Sam3_ItemCode
                                            .Where(x => x.ItemCodeID == itemCodeJson.ItemCodeID && x.Activo).SingleOrDefault();



                                string[] elementos = itemCodeJson.NumeroUnico.Split('-').ToArray();
                                int temp = Convert.ToInt32(elementos[1]);
                                string prefijo = elementos[0];

                                actualizaNU = ctx.Sam3_NumeroUnico
                                    .Where(x => x.NumeroUnicoID.ToString() == itemCodeJson.NumeroUnicoID).SingleOrDefault();


                                Sam3_EquivalenciaNumeroUnico ItemNumeroUnicoSam2 = ctx.Sam3_EquivalenciaNumeroUnico
                                            .Where(x => x.Sam3_NumeroUnicoID == actualizaNU.NumeroUnicoID && x.Activo).SingleOrDefault();

                                Sam3_NumeroUnicoInventario sam3_numeroUnicoInventario = (from a in ctx.Sam3_NumeroUnicoInventario
                                                                                         where a.NumeroUnicoID == actualizaNU.NumeroUnicoID && a.Activo
                                                                                         select a).SingleOrDefault();

                                List<Sam3_NumeroUnicoSegmento> lista_Sam3NumeroUnicoSegmento = (from a in ctx.Sam3_NumeroUnicoSegmento
                                                                                                where a.NumeroUnicoID == actualizaNU.NumeroUnicoID && a.Activo
                                                                                                select a).ToList();

                                int coladaID = (from c in ctx.Sam3_Colada
                                                where c.NumeroColada == itemCodeJson.Colada
                                                && c.ProyectoID == actualizaNU.ProyectoID
                                                select c.ColadaID).AsParallel().SingleOrDefault();

                                if (itemCodeJson.Titulo != "" && itemCodeJson.Titulo != null)
                                {
                                    Sam3_Incidencia incidencia = new Sam3_Incidencia();
                                    incidencia.Activo = true;
                                    incidencia.ClasificacionID = (from c in ctx.Sam3_ClasificacionIncidencia
                                                                  where c.Activo && c.Nombre == "Materiales"
                                                                  select c.ClasificacionIncidenciaID).AsParallel().SingleOrDefault();
                                    incidencia.Descripcion = itemCodeJson.DescripcionIncidencia;
                                    incidencia.Estatus = "Abierta";
                                    incidencia.FechaCreacion = DateTime.Now;
                                    incidencia.FechaModificacion = DateTime.Now;
                                    incidencia.TipoIncidenciaID = (from tp in ctx.Sam3_TipoIncidencia
                                                                   where tp.Activo && tp.Nombre == "Número único"
                                                                   select tp.TipoIncidenciaID).AsParallel().SingleOrDefault();
                                    incidencia.Titulo = itemCodeJson.Titulo;
                                    incidencia.UsuarioID = usuario.UsuarioID;
                                    incidencia.Version = 1;

                                    ctx.Sam3_Incidencia.Add(incidencia);
                                    ctx.SaveChanges();


                                    Sam3_Rel_Incidencia_NumeroUnico nuevaRelIncidencia = new Sam3_Rel_Incidencia_NumeroUnico();
                                    nuevaRelIncidencia.Activo = true;
                                    nuevaRelIncidencia.FechaModificacion = DateTime.Now;
                                    nuevaRelIncidencia.IncidenciaID = incidencia.IncidenciaID;
                                    nuevaRelIncidencia.NumeroUnicoID = actualizaNU.NumeroUnicoID;
                                    nuevaRelIncidencia.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_NumeroUnico.Add(nuevaRelIncidencia);
                                    ctx.SaveChanges();
                                }

                                if (itemCodeJson.TituloMTR != "" && itemCodeJson.TituloMTR != null)
                                {
                                    Sam3_Incidencia incidencia = new Sam3_Incidencia();
                                    incidencia.Activo = true;
                                    incidencia.ClasificacionID = (from c in ctx.Sam3_ClasificacionIncidencia
                                                                  where c.Activo && c.Nombre == "Materiales"
                                                                  select c.ClasificacionIncidenciaID).AsParallel().SingleOrDefault();
                                    incidencia.Descripcion = itemCodeJson.DescripcionIncidenciaMTR;
                                    incidencia.Estatus = "Abierta";
                                    incidencia.FechaCreacion = DateTime.Now;
                                    incidencia.FechaModificacion = DateTime.Now;
                                    incidencia.TipoIncidenciaID = (from tp in ctx.Sam3_TipoIncidencia
                                                                   where tp.Activo && tp.Nombre == "Número único"
                                                                   select tp.TipoIncidenciaID).AsParallel().SingleOrDefault();
                                    incidencia.Titulo = itemCodeJson.TituloMTR;
                                    incidencia.UsuarioID = usuario.UsuarioID;
                                    incidencia.Version = 1;

                                    ctx.Sam3_Incidencia.Add(incidencia);
                                    ctx.SaveChanges();


                                    Sam3_Rel_Incidencia_NumeroUnico nuevaRelIncidencia = new Sam3_Rel_Incidencia_NumeroUnico();
                                    nuevaRelIncidencia.Activo = true;
                                    nuevaRelIncidencia.FechaModificacion = DateTime.Now;
                                    nuevaRelIncidencia.IncidenciaID = incidencia.IncidenciaID;
                                    nuevaRelIncidencia.NumeroUnicoID = actualizaNU.NumeroUnicoID;
                                    nuevaRelIncidencia.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_Incidencia_NumeroUnico.Add(nuevaRelIncidencia);
                                    ctx.SaveChanges();
                                }

                                switch (tipoGuardadoID)
                                {
                                    case 1: // Guardado Parcial
                                        #region Guardado parcial
                                        //Actualizo el numero Unico
                                        if (actualizaNU != null)
                                        {
                                            string estatus = "C";
                                            if (itemCodeJson.EstatusFisico == "Aprobado" && itemCodeJson.EstatusDocumental == "Aprobado")
                                            {
                                                aprobado = true;
                                                dañado = false;
                                                estatus = "A";
                                            }

                                            if (itemCodeJson.EstatusFisico == "Aprobado" && itemCodeJson.EstatusDocumental == "Rechazado")
                                            {
                                                aprobado = false;
                                                dañado = false;
                                                estatus = "C";
                                            }
                                            if (itemCodeJson.EstatusFisico == "Aprobado" && itemCodeJson.EstatusDocumental == "")
                                            {
                                                aprobado = false;
                                                dañado = false;
                                                estatus = "C";
                                            }


                                            if (itemCodeJson.EstatusFisico == "Condicionado")
                                            {
                                                aprobado = false;
                                                dañado = false;
                                                estatus = "C";
                                            }

                                            if (itemCodeJson.EstatusFisico == string.Empty || itemCodeJson.EstatusFisico == "" || itemCodeJson.EstatusFisico == null)
                                            {
                                                aprobado = false;
                                                dañado = false;
                                                estatus = "C";
                                            }


                                            if (itemCodeJson.EstatusFisico == "Dañado")
                                            {
                                                aprobado = false;
                                                dañado = true;
                                                estatus = "R";
                                            }

                                            actualizaNU.Estatus = estatus;
                                            actualizaNU.NumeroUnicoCliente = itemCodeJson.NumeroUnicoCliente;
                                            actualizaNU.FechaModificacion = DateTime.Now;
                                            actualizaNU.UsuarioModificacion = usuario.UsuarioID;
                                            actualizaNU.ColadaID = coladaID;
                                            actualizaNU.EstatusFisico = itemCodeJson.EstatusFisico;
                                            actualizaNU.EstatusDocumental = itemCodeJson.EstatusDocumental;

                                            if (itemCodeJson.TipoUso != "" && itemCodeJson.TipoUso != null)
                                            {
                                                int? tipoUsoID = 0;
                                                tipoUsoID = (from tp in ctx.Sam3_TipoUso
                                                             where tp.Activo && tp.Nombre == itemCodeJson.TipoUso
                                                             select tp.TipoUsoID).SingleOrDefault();

                                                if (tipoUsoID > 0 && tipoUsoID != null)
                                                {
                                                    actualizaNU.TipoUsoID = tipoUsoID;
                                                }
                                                else
                                                {
                                                    throw new Exception("El tipo de uso no existe o esta desactivado");
                                                }
                                            }

                                            actualizaNU.MTRID = String.IsNullOrEmpty(itemCodeJson.MTRID) ? (int?)null : Convert.ToInt32(itemCodeJson.MTRID);

                                            #region Actualizar nu sam2
                                            int numSam2 = (from eq in ctx.Sam3_EquivalenciaNumeroUnico
                                                           where eq.Activo && eq.Sam3_NumeroUnicoID == actualizaNU.NumeroUnicoID
                                                           select eq.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();

                                            actualizaNumSam2 = ctx2.NumeroUnico.Where(x => x.NumeroUnicoID == numSam2).AsParallel().SingleOrDefault();

                                            actualizaNumSam2.Estatus = actualizaNU.Estatus;
                                            actualizaNumSam2.FechaModificacion = DateTime.Now;
                                            actualizaNumSam2.ColadaID = (from eq in ctx.Sam3_EquivalenciaColada
                                                                         where eq.Activo && eq.Sam3_ColadaID == actualizaNU.ColadaID
                                                                         select eq.Sam2_ColadaID).AsParallel().SingleOrDefault();
                                            ctx2.SaveChanges();
                                            #endregion



                                            #region Actualizar MM
                                            //Actuaalizar MM
                                            int milimetros = itemCodeJson.MM != null && itemCodeJson.MM != "" ? Convert.ToInt32(itemCodeJson.MM) : 0;
                                            cantidadRecibida = ctx.Sam3_NumeroUnicoInventario
                                                .Where(x => x.NumeroUnicoID == actualizaNU.NumeroUnicoID).Select(x => x.CantidadRecibida).AsParallel().SingleOrDefault();

                                            //bool inventarioCongelado = (from nui in ctx2.NumeroUnicoInventario
                                            //                           where nui.NumeroUnicoID == actualizaNumSam2.NumeroUnicoID
                                            //                           select nui.InventarioCongelado).AsParallel().SingleOrDefault() > 0 ? true : false;

                                            //si los milimetros son mayores a 0 y si son diferentes del inventario recibido en cuantificacion
                                            if (milimetros > 0 && milimetros != cantidadRecibida)
                                            {

                                                int? tempDespachados = ctx2.Despacho.Where(x => x.NumeroUnicoID == ItemNumeroUnicoSam2.Sam2_NumeroUnicoID && !x.Cancelado)
                                                    .Select(x => x.DespachoID).Count() > 0 ?
                                                    ctx2.Despacho.Where(x => x.NumeroUnicoID == ItemNumeroUnicoSam2.Sam2_NumeroUnicoID && !x.Cancelado)
                                                    .Select(x => x.Cantidad).Sum() : 0;

                                                cantidadDespachada = tempDespachados != null ? tempDespachados.Value : 0;

                                                if (sam3_numeroUnicoInventario.InventarioCongelado > 0) // si el numerounico tiene congelado
                                                {
                                                    throw new Exception("El Número Único ya cuenta con congelados, no se puede actualizar el inventario por este medio");
                                                }

                                                bool aumento = cantidadRecibida < milimetros;

                                                if (milimetros < cantidadDespachada)
                                                {
                                                    throw new Exception("El inventario no puede ser menor a la cantidad que actualmente se ha despachado");
                                                }

                                                fisico = milimetros - cantidadDespachada;
                                                cantidadDañada = dañado ? milimetros : 0;
                                                buenEstado = milimetros - cantidadDañada - cantidadDespachada;
                                                disponibleCruce = buenEstado - congelado;

                                                #region actualizar Sam3


                                                sam3_numeroUnicoInventario.CantidadRecibida = milimetros;
                                                if (dañado)
                                                {
                                                    sam3_numeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                }
                                                else
                                                {
                                                    sam3_numeroUnicoInventario.CantidadDanada = 0;
                                                }
                                                sam3_numeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                sam3_numeroUnicoInventario.InventarioFisico = fisico;
                                                sam3_numeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;
                                                sam3_numeroUnicoInventario.UsuarioModificacion = usuario.UsuarioID;
                                                sam3_numeroUnicoInventario.FechaModificacion = DateTime.Now;

                                                if (actualizaNU.Sam3_ItemCode.TipoMaterialID == 1) // tubo
                                                {
                                                    segmento = lista_Sam3NumeroUnicoSegmento.Where(x => x.Segmento == "A").SingleOrDefault();
                                                    segmento.InventarioBuenEstado = buenEstado;
                                                    if (dañado)
                                                    {
                                                        segmento.CantidadDanada = cantidadDañada;
                                                    }
                                                    else
                                                    {
                                                        segmento.CantidadDanada = cantidadDañada;
                                                    }
                                                    segmento.InventarioDisponibleCruce = disponibleCruce;
                                                    segmento.InventarioFisico = fisico;
                                                    segmento.FechaModificacion = DateTime.Now;
                                                    segmento.UsuarioModificacion = usuario.UsuarioID;
                                                }

                                                movimiento = new Sam3_NumeroUnicoMovimiento();
                                                movimiento.Activo = true;
                                                movimiento.Estatus = "A";
                                                movimiento.FechaModificacion = DateTime.Now;
                                                movimiento.FechaMovimiento = DateTime.Now;
                                                movimiento.NumeroUnicoID = actualizaNU.NumeroUnicoID;
                                                movimiento.ProyectoID = actualizaNU.ProyectoID;
                                                movimiento.Referencia = "Complemento de recepcion";
                                                movimiento.Segmento = "A";
                                                movimiento.UsuarioModificacion = usuario.UsuarioID;

                                                if (aumento)
                                                {
                                                    int diferencia = milimetros - cantidadRecibida;
                                                    movimiento.Cantidad = diferencia;
                                                    movimiento.TipoMovimientoID = (from tp in ctx.Sam3_TipoMovimiento
                                                                                   where tp.Activo
                                                                                   && tp.Nombre == "Aumento de Inventario por Actualización MM"
                                                                                   select tp.TipoMovimientoID).AsParallel().SingleOrDefault();
                                                }
                                                else
                                                {
                                                    int diferencia = cantidadRecibida - milimetros;
                                                    movimiento.Cantidad = diferencia;
                                                    movimiento.TipoMovimientoID = (from tp in ctx.Sam3_TipoMovimiento
                                                                                   where tp.Activo
                                                                                   && tp.Nombre == "Reducción de Inventario por Actualización MM"
                                                                                   select tp.TipoMovimientoID).AsParallel().SingleOrDefault();
                                                }

                                                #region validaciones de inventario
                                                if (sam3_numeroUnicoInventario.InventarioBuenEstado < sam3_numeroUnicoInventario.InventarioCongelado)
                                                {
                                                    throw new Exception("El inventario físico no puede ser menor al inventario congelado");
                                                }
                                                if (sam3_numeroUnicoInventario.InventarioDisponibleCruce < 0)
                                                {
                                                    throw new Exception("El inventario disponible para cruce no puede ser menor a  0");
                                                }
                                                #endregion

                                                ctx.Sam3_NumeroUnicoMovimiento.Add(movimiento);
                                                ctx.SaveChanges();
                                                #endregion

                                                #region Actualizar Sam2

                                                actualizaNumSam2.NumeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                actualizaNumSam2.NumeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;
                                                actualizaNumSam2.NumeroUnicoInventario.InventarioFisico = fisico;
                                                actualizaNumSam2.NumeroUnicoInventario.CantidadRecibida = milimetros;
                                                if (dañado)
                                                {
                                                    actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                }
                                                else
                                                {
                                                    actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = 0;
                                                }

                                                actualizaNumSam2.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

                                                if (actualizaNU.Sam3_ItemCode.TipoMaterialID == 1) // tubo
                                                {
                                                    segmentoSam2 = actualizaNumSam2.NumeroUnicoSegmento.Where(x => x.Segmento == "A")
                                                        .SingleOrDefault();
                                                    segmentoSam2.InventarioBuenEstado = buenEstado;
                                                    segmentoSam2.InventarioDisponibleCruce = disponibleCruce;
                                                    segmentoSam2.InventarioFisico = fisico;
                                                    if (dañado)
                                                    {
                                                        segmentoSam2.CantidadDanada = cantidadDañada;
                                                    }
                                                    else
                                                    {
                                                        segmentoSam2.CantidadDanada = 0;
                                                    }

                                                    segmentoSam2.FechaModificacion = DateTime.Now;
                                                }
                                                ctx2.SaveChanges();
                                                #endregion

                                                #region ActualizarRelacion IT

                                                Sam3_Rel_NumeroUnico_RelFC_RelB relNumeos = (from rel in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                                                                             where rel.Activo
                                                                                             && rel.NumeroUnicoID == actualizaNU.NumeroUnicoID
                                                                                             select rel).AsParallel().SingleOrDefault();

                                                relNumeos.MM = milimetros;
                                                relNumeos.FechaModificacion = DateTime.Now;
                                                relNumeos.UsuarioModificacion = usuario.UsuarioID;
                                                ctx.SaveChanges();

                                                #endregion
                                                //} // IF tipo de material
                                            } // if diferencia de inventario
                                            else // no cambia el inventario
                                            {
                                                if (dañado || (actualizaNU.Estatus == "A" && sam3_numeroUnicoInventario.CantidadDanada > 0)
                                                    || (actualizaNU.Estatus == "C" && sam3_numeroUnicoInventario.CantidadDanada > 0))
                                                {
                                                    if (sam3_numeroUnicoInventario.InventarioCongelado > 0 && dañado) // si el numerounico tiene congelado
                                                    {
                                                        throw new Exception("El Número Único ya cuenta con congelados, no se puede actualizar el inventario por este medio");
                                                    }
                                                    cantidadDañada = dañado ? cantidadRecibida : 0;
                                                    buenEstado = dañado ? cantidadRecibida - cantidadDañada : cantidadRecibida;
                                                    disponibleCruce = buenEstado - congelado;
                                                    //no cambio el inventario 
                                                    if (actualizaNU.Sam3_ItemCode.TipoMaterialID == 1) // tubo
                                                    {
                                                        #region Actualizar SAM3

                                                        if (dañado)
                                                        {
                                                            sam3_numeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            sam3_numeroUnicoInventario.CantidadDanada = 0;
                                                        }
                                                        sam3_numeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                        sam3_numeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;

                                                        segmento = lista_Sam3NumeroUnicoSegmento.Where(s => s.Segmento == "A").SingleOrDefault();
                                                        segmento.InventarioFisico = cantidadRecibida;
                                                        segmento.InventarioBuenEstado = buenEstado;
                                                        if (dañado)
                                                        {
                                                            segmento.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            segmento.CantidadDanada = 0;
                                                        }
                                                        segmento.InventarioDisponibleCruce = disponibleCruce;
                                                        #endregion
                                                        #region Actualizar SAM2
                                                        if (dañado)
                                                        {
                                                            actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = 0;
                                                        }
                                                        actualizaNumSam2.NumeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                        actualizaNumSam2.NumeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;

                                                        segmentoSam2 = actualizaNumSam2.NumeroUnicoSegmento.Where(s => s.Segmento == "A").SingleOrDefault();
                                                        segmentoSam2.InventarioFisico = cantidadRecibida;
                                                        segmentoSam2.InventarioBuenEstado = buenEstado;
                                                        segmentoSam2.InventarioDisponibleCruce = disponibleCruce;
                                                        if (dañado)
                                                        {
                                                            segmentoSam2.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            segmentoSam2.CantidadDanada = 0;
                                                        }
                                                        #endregion
                                                    }
                                                    else // accesorio
                                                    {
                                                        #region Actualizar SAM3
                                                        if (dañado)
                                                        {
                                                            sam3_numeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            sam3_numeroUnicoInventario.CantidadDanada = 0;
                                                        }

                                                        sam3_numeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                        sam3_numeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;
                                                        #endregion
                                                        #region Actualizar SAM2
                                                        if (dañado)
                                                        {
                                                            actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = 0;
                                                        }
                                                        actualizaNumSam2.NumeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                        actualizaNumSam2.NumeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;
                                                        #endregion
                                                    }

                                                    ctx.SaveChanges();
                                                    ctx2.SaveChanges();
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            throw new Exception(string.Format("Error al actualizar el número único {}", itemCodeJson.NumeroUnico));
                                        }

                                        if (actualizaItem != null)
                                        {
                                            actualizaItem.TieneComplementoRecepcion = false;
                                            actualizaItem.FechaModificacion = DateTime.Now;
                                            actualizaItem.UsuarioModificacion = usuario.UsuarioID;
                                        }
                                        else
                                        {
                                            throw new Exception(string.Format("Error al actualizar La informacion del ItemCode {}", itemCodeJson.ItemCode));
                                        }

                                        if (!ctx.Sam3_Rel_Itemcode_Colada.Where(x => x.ColadaID == coladaID && x.ItemCodeID == actualizaItem.ItemCodeID).Any())
                                        {
                                            Sam3_Rel_Itemcode_Colada nuevarel = new Sam3_Rel_Itemcode_Colada();
                                            nuevarel.Activo = true;
                                            nuevarel.ColadaID = coladaID;
                                            nuevarel.FechaModificacion = DateTime.Now;
                                            nuevarel.ItemCodeID = actualizaItem.ItemCodeID;
                                            nuevarel.UsuarioModificacion = usuario.UsuarioID;

                                            ctx.Sam3_Rel_Itemcode_Colada.Add(nuevarel);
                                            ctx.SaveChanges();
                                        }

                                        ctx.SaveChanges();
                                        itemCodeJson.TieneError = false;

                                        break;
                                    #endregion
                                    case 2: // Guardar y terminar
                                        #region guardar y terminar
                                        //Actualizo el numero Unico
                                        if (actualizaNU != null)
                                        {
                                            string estatus = "C";
                                            if (itemCodeJson.EstatusFisico == "Aprobado" && itemCodeJson.EstatusDocumental == "Aprobado")
                                            {
                                                dañado = false;
                                                aprobado = true;
                                                estatus = "A";
                                            }

                                            if (itemCodeJson.EstatusFisico == "Aprobado" && itemCodeJson.EstatusDocumental == "Rechazado")
                                            {
                                                dañado = false;
                                                aprobado = false;
                                                estatus = "C";
                                            }
                                            if (itemCodeJson.EstatusFisico == "Aprobado" && itemCodeJson.EstatusDocumental == "")
                                            {
                                                dañado = false;
                                                aprobado = false;
                                                estatus = "C";
                                            }


                                            if (itemCodeJson.EstatusFisico == "Condicionado")
                                            {
                                                aprobado = false;
                                                dañado = false;
                                                estatus = "C";
                                            }

                                            if (itemCodeJson.EstatusFisico == string.Empty || itemCodeJson.EstatusFisico == "" || itemCodeJson.EstatusFisico == null)
                                            {
                                                aprobado = false;
                                                dañado = false;
                                                estatus = "C";
                                            }


                                            if (itemCodeJson.EstatusFisico == "Dañado")
                                            {
                                                aprobado = false;
                                                dañado = true;
                                                estatus = "R";
                                            }

                                            actualizaNU.Estatus = estatus;
                                            actualizaNU.NumeroUnicoCliente = itemCodeJson.NumeroUnicoCliente;
                                            actualizaNU.FechaModificacion = DateTime.Now;
                                            actualizaNU.UsuarioModificacion = usuario.UsuarioID;
                                            actualizaNU.ColadaID = coladaID;
                                            actualizaNU.EstatusFisico = itemCodeJson.EstatusFisico;
                                            actualizaNU.EstatusDocumental = itemCodeJson.EstatusDocumental;
                                            actualizaNU.TipoUsoID = itemCodeJson.TipoUso != "" && itemCodeJson.TipoUso != null ?
                                                (from tp in ctx.Sam3_TipoUso
                                                 where tp.Activo && tp.Nombre == itemCodeJson.TipoUso
                                                 select tp.TipoUsoID).SingleOrDefault() : 1;

                                            int mtr = 0;
                                            int.TryParse(itemCodeJson.MTRID, out mtr);
                                            if (mtr > 0)
                                            {
                                                actualizaNU.MTRID = mtr;
                                            }

                                            #region Actualizar nu sam2
                                            int numSam2 = (from eq in ctx.Sam3_EquivalenciaNumeroUnico
                                                           where eq.Activo && eq.Sam3_NumeroUnicoID == actualizaNU.NumeroUnicoID
                                                           select eq.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();
                                            actualizaNumSam2 = ctx2.NumeroUnico.Where(x => x.NumeroUnicoID == numSam2).AsParallel().SingleOrDefault();
                                            actualizaNumSam2.FechaModificacion = DateTime.Now;
                                            actualizaNumSam2.Estatus = actualizaNU.Estatus;
                                            actualizaNumSam2.ColadaID = (from eq in ctx.Sam3_EquivalenciaColada
                                                                         where eq.Activo && eq.Sam3_ColadaID == actualizaNU.ColadaID
                                                                         select eq.Sam2_ColadaID).AsParallel().SingleOrDefault();
                                            ctx2.SaveChanges();
                                            #endregion

                                            #region Actualizar MM
                                            //Actuaalizar MM
                                            int milimetros = itemCodeJson.MM != null && itemCodeJson.MM != "" ? Convert.ToInt32(itemCodeJson.MM) : 0;
                                            cantidadRecibida = ctx.Sam3_NumeroUnicoInventario
                                                .Where(x => x.NumeroUnicoID == actualizaNU.NumeroUnicoID).Select(x => x.CantidadRecibida).AsParallel().SingleOrDefault();
                                            //bool inventarioCongelado = (from nui in ctx2.NumeroUnicoInventario
                                            //                            where nui.NumeroUnicoID == actualizaNumSam2.NumeroUnicoID
                                            //                            select nui.InventarioCongelado).AsParallel().SingleOrDefault() > 0 ? true : false;

                                            //si los milimetros son mayores a 0 y si son diferentes del inventario recibido en cuantificacion
                                            if (milimetros > 0 && milimetros != cantidadRecibida)
                                            {
                                                int? tempDespachados = ctx2.Despacho.Where(x => x.NumeroUnicoID == ItemNumeroUnicoSam2.Sam2_NumeroUnicoID && !x.Cancelado)
                                                   .Select(x => x.DespachoID).Count() > 0 ?
                                                   ctx2.Despacho.Where(x => x.NumeroUnicoID == ItemNumeroUnicoSam2.Sam2_NumeroUnicoID && !x.Cancelado)
                                                   .Select(x => x.Cantidad).Sum() : 0;

                                                //int? tempDespachados = ctx.Sam3_Despacho.Where(x => x.NumeroUnicoID == actualizaNU.NumeroUnicoID && x.Activo && !x.Cancelado)
                                                //    .Select(x => x.DespachoID).Count() > 0 ?
                                                //    ctx.Sam3_Despacho.Where(x => x.NumeroUnicoID == actualizaNU.NumeroUnicoID && x.Activo && !x.Cancelado)
                                                //    .Select(x => x.Cantidad).Sum() : 0;

                                                cantidadDespachada = tempDespachados != null ? tempDespachados.Value : 0;

                                                if (sam3_numeroUnicoInventario.InventarioCongelado > 0) // si el numerounico tiene congelado
                                                {
                                                    throw new Exception("El Número Único ya cuenta con congelados, no se puede actualizar el inventario por este medio");
                                                }

                                                bool aumento = cantidadRecibida < milimetros;

                                                if (milimetros < cantidadDespachada)
                                                {
                                                    throw new Exception("El inventario no puede ser menor a la cantidad que actualmente se ha despachado");
                                                }

                                                fisico = milimetros - cantidadDespachada;
                                                cantidadDañada = dañado ? milimetros : 0;
                                                buenEstado = milimetros - cantidadDañada - cantidadDespachada;
                                                disponibleCruce = buenEstado - congelado;

                                                #region actualizar Sam3
                                                sam3_numeroUnicoInventario.CantidadRecibida = milimetros;
                                                sam3_numeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                sam3_numeroUnicoInventario.InventarioFisico = fisico;
                                                sam3_numeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;
                                                if (dañado)
                                                {
                                                    sam3_numeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                }
                                                else
                                                {
                                                    sam3_numeroUnicoInventario.CantidadDanada = 0;
                                                }
                                                sam3_numeroUnicoInventario.UsuarioModificacion = usuario.UsuarioID;
                                                sam3_numeroUnicoInventario.FechaModificacion = DateTime.Now;

                                                if (actualizaNU.Sam3_ItemCode.TipoMaterialID == 1) // tubo
                                                {
                                                    segmento = lista_Sam3NumeroUnicoSegmento.Where(x => x.Segmento == "A").SingleOrDefault();
                                                    segmento.InventarioBuenEstado = buenEstado;
                                                    segmento.InventarioDisponibleCruce = disponibleCruce;
                                                    segmento.InventarioFisico = fisico;
                                                    if (dañado)
                                                    {
                                                        segmento.CantidadDanada = cantidadDañada;
                                                    }
                                                    else
                                                    {
                                                        segmento.CantidadDanada = 0;
                                                    }
                                                    segmento.FechaModificacion = DateTime.Now;
                                                    segmento.UsuarioModificacion = usuario.UsuarioID;
                                                }

                                                movimiento = new Sam3_NumeroUnicoMovimiento();
                                                movimiento.Activo = true;
                                                movimiento.Estatus = "A";
                                                movimiento.FechaModificacion = DateTime.Now;
                                                movimiento.FechaMovimiento = DateTime.Now;
                                                movimiento.NumeroUnicoID = actualizaNU.NumeroUnicoID;
                                                movimiento.ProyectoID = actualizaNU.ProyectoID;
                                                movimiento.Referencia = "Complemento de recepcion";
                                                movimiento.Segmento = "A";
                                                movimiento.UsuarioModificacion = usuario.UsuarioID;

                                                if (aumento)
                                                {
                                                    int diferencia = milimetros - cantidadRecibida;
                                                    movimiento.Cantidad = diferencia;
                                                    movimiento.TipoMovimientoID = (from tp in ctx.Sam3_TipoMovimiento
                                                                                   where tp.Activo
                                                                                   && tp.Nombre == "Aumento de Inventario por Actualización MM"
                                                                                   select tp.TipoMovimientoID).AsParallel().SingleOrDefault();
                                                }
                                                else
                                                {
                                                    int diferencia = cantidadRecibida - milimetros;
                                                    movimiento.Cantidad = diferencia;
                                                    movimiento.TipoMovimientoID = (from tp in ctx.Sam3_TipoMovimiento
                                                                                   where tp.Activo
                                                                                   && tp.Nombre == "Reducción de Inventario por Actualización MM"
                                                                                   select tp.TipoMovimientoID).AsParallel().SingleOrDefault();
                                                }

                                                #region validaciones de inventario
                                                if (sam3_numeroUnicoInventario.InventarioBuenEstado < sam3_numeroUnicoInventario.InventarioCongelado)
                                                {
                                                    throw new Exception("El inventario físico no puede ser menor al inventario congelado");
                                                }
                                                if (sam3_numeroUnicoInventario.InventarioDisponibleCruce < 0)
                                                {
                                                    throw new Exception("El inventario disponible para cruce no puede ser menor a  0");
                                                }
                                                #endregion

                                                ctx.Sam3_NumeroUnicoMovimiento.Add(movimiento);
                                                ctx.SaveChanges();
                                                #endregion

                                                #region Actualizar Sam2
                                                actualizaNumSam2.NumeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                actualizaNumSam2.NumeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;
                                                actualizaNumSam2.NumeroUnicoInventario.InventarioFisico = fisico;
                                                actualizaNumSam2.NumeroUnicoInventario.CantidadRecibida = milimetros;
                                                if (dañado)
                                                {
                                                    actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                }
                                                else
                                                {
                                                    actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = 0;
                                                }
                                                actualizaNumSam2.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

                                                if (actualizaNU.Sam3_ItemCode.TipoMaterialID == 1) // tubo
                                                {
                                                    segmentoSam2 = actualizaNumSam2.NumeroUnicoSegmento.Where(x => x.Segmento == "A")
                                                        .SingleOrDefault();
                                                    segmentoSam2.InventarioBuenEstado = buenEstado;
                                                    segmentoSam2.InventarioDisponibleCruce = disponibleCruce;
                                                    segmentoSam2.InventarioFisico = fisico;
                                                    if (dañado)
                                                    {
                                                        segmentoSam2.CantidadDanada = cantidadDañada;
                                                    }
                                                    else
                                                    {
                                                        segmentoSam2.CantidadDanada = 0;
                                                    }
                                                    segmentoSam2.FechaModificacion = DateTime.Now;
                                                }

                                                #region validaciones de inventario sam2
                                                if (sam2_numeroUnico.NumeroUnicoInventario.InventarioBuenEstado < sam2_numeroUnico.NumeroUnicoInventario.InventarioCongelado)
                                                {
                                                    throw new Exception("El inventario Físico no puede ser menor que el inventario congelado");
                                                }
                                                if (sam2_numeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce < 0)
                                                {
                                                    throw new Exception("El inventario disponible de cruce no puede ser menor a 0");
                                                }
                                                #endregion

                                                ctx2.SaveChanges();
                                                #endregion

                                                #region ActualizarRelacion IT
                                                Sam3_Rel_NumeroUnico_RelFC_RelB relNumeos = (from rel in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                                                                             where rel.Activo
                                                                                             && rel.NumeroUnicoID == actualizaNU.NumeroUnicoID
                                                                                             select rel).AsParallel().SingleOrDefault();

                                                relNumeos.MM = milimetros;
                                                relNumeos.FechaModificacion = DateTime.Now;
                                                relNumeos.UsuarioModificacion = usuario.UsuarioID;
                                                ctx.SaveChanges();

                                                #endregion
                                            }
                                            else //no cambia el inventario
                                            {
                                                if (dañado || (actualizaNU.Estatus == "A" && sam3_numeroUnicoInventario.CantidadDanada > 0)
                                                    || (actualizaNU.Estatus == "C" && sam3_numeroUnicoInventario.CantidadDanada > 0))
                                                {
                                                    if (sam3_numeroUnicoInventario.InventarioCongelado > 0 && dañado) // si el numerounico tiene congelado
                                                    {
                                                        throw new Exception("El Número Único ya cuenta con congelados, no se puede actualizar el inventario por este medio");
                                                    }

                                                    cantidadDañada = dañado ? cantidadRecibida : 0;
                                                    buenEstado = cantidadRecibida - cantidadDañada;
                                                    disponibleCruce = cantidadRecibida - cantidadDañada - congelado;
                                                    //no cambio el inventario

                                                    if (actualizaNU.Sam3_ItemCode.TipoMaterialID == 1) // tubo
                                                    {
                                                        #region Actualizar SAM3
                                                        if (dañado)
                                                        {
                                                            sam3_numeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            sam3_numeroUnicoInventario.CantidadDanada = 0;
                                                        }
                                                        sam3_numeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                        sam3_numeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;

                                                        segmento = lista_Sam3NumeroUnicoSegmento.Where(s => s.Segmento == "A").SingleOrDefault();
                                                        segmento.InventarioFisico = cantidadRecibida;
                                                        segmento.InventarioBuenEstado = buenEstado;
                                                        if (dañado)
                                                        {
                                                            segmento.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            segmento.CantidadDanada = 0;
                                                        }
                                                        segmento.InventarioDisponibleCruce = disponibleCruce;
                                                        #endregion
                                                        #region Actualizar SAM2
                                                        if (dañado)
                                                        {
                                                            actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = 0;
                                                        }
                                                        actualizaNumSam2.NumeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                        actualizaNumSam2.NumeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;

                                                        segmentoSam2 = actualizaNumSam2.NumeroUnicoSegmento.Where(s => s.Segmento == "A").SingleOrDefault();
                                                        segmentoSam2.InventarioFisico = cantidadRecibida;
                                                        segmentoSam2.InventarioBuenEstado = buenEstado;
                                                        segmentoSam2.InventarioDisponibleCruce = disponibleCruce;
                                                        if (dañado)
                                                        {
                                                            segmentoSam2.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            segmentoSam2.CantidadDanada = 0;
                                                        }
                                                        #endregion
                                                    }
                                                    else // accesorio
                                                    {
                                                        #region Actualizar SAM3
                                                        if (dañado)
                                                        {
                                                            sam3_numeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            sam3_numeroUnicoInventario.CantidadDanada = 0;
                                                        }
                                                        sam3_numeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                        sam3_numeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;
                                                        #endregion
                                                        #region Actualizar SAM2
                                                        if (dañado)
                                                        {
                                                            actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = cantidadDañada;
                                                        }
                                                        else
                                                        {
                                                            actualizaNumSam2.NumeroUnicoInventario.CantidadDanada = 0;
                                                        }
                                                        actualizaNumSam2.NumeroUnicoInventario.InventarioBuenEstado = buenEstado;
                                                        actualizaNumSam2.NumeroUnicoInventario.InventarioDisponibleCruce = disponibleCruce;
                                                        #endregion
                                                    }

                                                    ctx.SaveChanges();
                                                    ctx2.SaveChanges();
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            throw new Exception(string.Format("Error al actualizar el número único {}", itemCodeJson.NumeroUnico));
                                        }

                                        if (actualizaItem != null)
                                        {
                                            if (itemCodeJson.MM == "" || itemCodeJson.Colada == "" || itemCodeJson.EstatusFisico == ""
                                                || itemCodeJson.EstatusDocumental == "" || itemCodeJson.TipoUso == "")
                                            {
                                                throw new Exception(string.Format("Datos Incompletos"));
                                            }

                                            actualizaItem.TieneComplementoRecepcion = true;
                                            actualizaItem.FechaModificacion = DateTime.Now;
                                            actualizaItem.UsuarioModificacion = usuario.UsuarioID;
                                        }
                                        else
                                        {
                                            throw new Exception(string.Format("Error al actualizar La informacion del ItemCode {}", itemCodeJson.ItemCode));
                                        }

                                        ctx.SaveChanges();

                                        if (!ctx.Sam3_Rel_Itemcode_Colada.Where(x => x.ColadaID == coladaID && x.ItemCodeID == actualizaItem.ItemCodeID).Any())
                                        {
                                            Sam3_Rel_Itemcode_Colada nuevarel = new Sam3_Rel_Itemcode_Colada();
                                            nuevarel.Activo = true;
                                            nuevarel.ColadaID = coladaID;
                                            nuevarel.FechaModificacion = DateTime.Now;
                                            nuevarel.ItemCodeID = actualizaItem.ItemCodeID;
                                            nuevarel.UsuarioModificacion = usuario.UsuarioID;

                                            ctx.Sam3_Rel_Itemcode_Colada.Add(nuevarel);
                                            ctx.SaveChanges();
                                        }

                                        itemCodeJson.TieneError = false;

                                        break;
                                    #endregion
                                    default:

                                        result.ReturnMessage.Add("No se encontro el tipo de guardado");
                                        result.ReturnCode = 500;
                                        result.ReturnStatus = false;
                                        result.IsAuthenicated = true;

                                        return result;
                                } // Fin switch
                                ctx_tran.Commit();
                                ctx2_tran.Commit();
                            } // tran sam2
                        } // sam2
                    } // tran sam3
                }// fin using SAM

                itemCodeJson = ObtenerPropiedadesJson(relFcId, relBId, relNuId, double.Parse(actualizaNU.Diametro1.ToString()), double.Parse(actualizaNU.Diametro2.ToString()));
                return itemCodeJson;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                itemCodeJson.TieneError = true;
                return itemCodeJson;
            }
        }

        public object ObtenerTodoPorOrdenRecepcionID(int ordenRecepxionID, Sam3_Usuario usuario)
        {
            try
            {
                bool activaConfiguracionPackinglist = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"])
                        ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"].Equals("1") ? true : false) : false;
                List<object> result = new List<object>();
                using (SamContext ctx = new SamContext())
                {
                    List<ListaCombos> foliosCuantificacion = (from or in ctx.Sam3_OrdenRecepcion
                                                              join relor in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on or.OrdenRecepcionID equals relor.OrdenRecepcionID
                                                              join fe in ctx.Sam3_FolioAvisoEntrada on relor.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                                              join fc in ctx.Sam3_FolioCuantificacion on relor.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                                              where or.Activo && relor.Activo && fc.Activo
                                                              && or.Folio == ordenRecepxionID
                                                              select new ListaCombos
                                                              {
                                                                  id = fc.FolioCuantificacionID.ToString(),
                                                                  value = activaConfiguracionPackinglist ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                            where pc.Rel_Proyecto_Entidad_Configuracion_ID == fc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                            && pc.Activo == 1
                                                                                                            select pc.PreFijoFolioPackingList + ","
                                                                                                            + pc.CantidadCerosFolioPackingList.ToString() + ","
                                                                                                            + fc.ConsecutivoConfiguracion.ToString() + ","
                                                                                                            + pc.PostFijoFolioPackingList).FirstOrDefault() : fe.FolioAvisoLlegadaID + "-" + fc.Consecutivo
                                                              }).ToList();

                    List<ListaCombos> proyectos = (from or in ctx.Sam3_OrdenRecepcion
                                                   join relor in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on or.OrdenRecepcionID equals relor.OrdenRecepcionID
                                                   join fe in ctx.Sam3_FolioAvisoEntrada on relor.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                                   join relfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals relfp.FolioAvisoLlegadaID
                                                   join p in ctx.Sam3_Proyecto on relfp.ProyectoID equals p.ProyectoID
                                                   where or.Activo && relor.Activo && fe.Activo && relfp.Activo && p.Activo
                                                   && p.Nombre != ""
                                                   && or.Folio == ordenRecepxionID
                                                   select new ListaCombos
                                                   {
                                                       id = p.ProyectoID.ToString(),
                                                       value = p.Nombre
                                                   }).ToList();

                    List<int> lstfoliosIds = (from fc in foliosCuantificacion select Convert.ToInt32(fc.id)).ToList();

                    List<ItemCodeComplemento> listado = new List<ItemCodeComplemento>();
                    foreach (int i in lstfoliosIds)
                    {
                        var lst = ObtenerListado(i, usuario, true);
                        listado.AddRange((List<ItemCodeComplemento>)lst);
                    }

                    result.Add(proyectos);
                    result.Add(foliosCuantificacion);
                    result.Add(listado);

                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string temp = serializer.Serialize(result);
                return result;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------

                return null;
            }
        }

        public object ReemplazarItemCode(EditarItemCode datos, Sam3_Usuario usuario)
        {
            try
            {
                Sam3_Diametro diametro1;
                Sam3_Diametro diametro2;
                Diametro sam2_diametro1;
                Diametro sam2_diametro2;
                Sam3_ItemCode nuevoItemCode = null;
                int sam2_ProyectoID = 0;
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var ctx2_tran = ctx2.Database.BeginTransaction())
                            {
                                #region diametros
                                //Diametro1
                                if (ctx.Sam3_Diametro.Where(x => x.Valor == datos.d1 && x.Activo).Any()) //Verificamos si existe el diametro
                                {
                                    diametro1 = ctx.Sam3_Diametro.Where(x => x.Valor == datos.d1 && x.Activo).AsParallel().SingleOrDefault();
                                }
                                else // el diametro no existe
                                {
                                    diametro1 = new Sam3_Diametro
                                    {
                                        Activo = true,
                                        FechaModificacion = DateTime.Now,
                                        UsuarioModificacion = usuario.UsuarioID,
                                        Valor = datos.d1,
                                        VerificadoPorCalidad = true
                                    };
                                    ctx.Sam3_Diametro.Add(diametro1);
                                    ctx.SaveChanges();
                                }

                                //verificamos si existe equivalencia para el diametro en sam2
                                if (ctx.Sam3_EquivalenciaDiametro.Where(x => x.Sam3_DiametroID == diametro1.DiametroID && x.Activo).Any())
                                {
                                    int sam2_diametroID = (from eq in ctx.Sam3_EquivalenciaDiametro
                                                           where eq.Activo && eq.Sam3_DiametroID == diametro1.DiametroID
                                                           select eq.Sam2_DiametroID.Value).SingleOrDefault();

                                    sam2_diametro1 = ctx2.Diametro.Where(x => x.DiametroID == sam2_diametroID).AsParallel().SingleOrDefault();

                                }
                                else
                                {
                                    //si no existe la equivalencia hay que crear el diametro en sam2
                                    if (!ctx.Sam3_Diametro.Where(x => x.Valor == diametro1.Valor).Any()) //verificamos si realmente no existe el diametro
                                    {
                                        sam2_diametro1 = new Diametro
                                        {
                                            Valor = diametro1.Valor,
                                            FechaModificacion = DateTime.Now,
                                            VerificadoPorCalidad = true
                                        };

                                        ctx2.Diametro.Add(sam2_diametro1);
                                        ctx2.SaveChanges();

                                        //guardamos la equivalencia
                                        Sam3_EquivalenciaDiametro nuevaEquivalencia = new Sam3_EquivalenciaDiametro
                                        {
                                            Activo = true,
                                            FechaModificacion = DateTime.Now,
                                            Sam2_DiametroID = sam2_diametro1.DiametroID,
                                            Sam3_DiametroID = diametro1.DiametroID,
                                            UsuarioModificacion = usuario.UsuarioID
                                        };
                                        ctx.Sam3_EquivalenciaDiametro.Add(nuevaEquivalencia);
                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        //Existe el diametro pero no la equivalencia
                                        sam2_diametro1 = ctx2.Diametro.Where(x => x.Valor == diametro1.Valor).AsParallel().SingleOrDefault();

                                        //guardamos la equivalencia
                                        Sam3_EquivalenciaDiametro nuevaEquivalencia = new Sam3_EquivalenciaDiametro
                                        {
                                            Activo = true,
                                            FechaModificacion = DateTime.Now,
                                            Sam2_DiametroID = sam2_diametro1.DiametroID,
                                            Sam3_DiametroID = diametro1.DiametroID,
                                            UsuarioModificacion = usuario.UsuarioID
                                        };
                                        ctx.Sam3_EquivalenciaDiametro.Add(nuevaEquivalencia);
                                        ctx.SaveChanges();
                                    }
                                }

                                //Diametro 2
                                if (ctx.Sam3_Diametro.Where(x => x.Valor == datos.d2 && x.Activo).Any()) //Verificamos si existe el diametro
                                {
                                    diametro2 = ctx.Sam3_Diametro.Where(x => x.Valor == datos.d2 && x.Activo).AsParallel().SingleOrDefault();
                                }
                                else // el diametro no existe
                                {
                                    diametro2 = new Sam3_Diametro
                                    {
                                        Activo = true,
                                        FechaModificacion = DateTime.Now,
                                        UsuarioModificacion = usuario.UsuarioID,
                                        Valor = datos.d2,
                                        VerificadoPorCalidad = true
                                    };
                                    ctx.Sam3_Diametro.Add(diametro2);
                                    ctx.SaveChanges();
                                }

                                //verificamos si existe equivalencia para el diametro en sam2
                                if (ctx.Sam3_EquivalenciaDiametro.Where(x => x.Sam3_DiametroID == diametro2.DiametroID && x.Activo).Any())
                                {
                                    int sam2_diametroID = (from eq in ctx.Sam3_EquivalenciaDiametro
                                                           where eq.Activo && eq.Sam3_DiametroID == diametro2.DiametroID
                                                           select eq.Sam2_DiametroID.Value).SingleOrDefault();

                                    sam2_diametro2 = ctx2.Diametro.Where(x => x.DiametroID == sam2_diametroID).AsParallel().SingleOrDefault();

                                }
                                else
                                {
                                    //si no existe la equivalencia hay que crear el diametro en sam2
                                    if (!ctx.Sam3_Diametro.Where(x => x.Valor == diametro2.Valor).Any()) //verificamos si realmente no existe el diametro
                                    {
                                        sam2_diametro2 = new Diametro
                                        {
                                            Valor = diametro2.Valor,
                                            FechaModificacion = DateTime.Now,
                                            VerificadoPorCalidad = true
                                        };

                                        ctx2.Diametro.Add(sam2_diametro2);
                                        ctx2.SaveChanges();

                                        //guardamos la equivalencia
                                        Sam3_EquivalenciaDiametro nuevaEquivalencia = new Sam3_EquivalenciaDiametro
                                        {
                                            Activo = true,
                                            FechaModificacion = DateTime.Now,
                                            Sam2_DiametroID = sam2_diametro2.DiametroID,
                                            Sam3_DiametroID = diametro2.DiametroID,
                                            UsuarioModificacion = usuario.UsuarioID
                                        };
                                        ctx.Sam3_EquivalenciaDiametro.Add(nuevaEquivalencia);
                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        //Existe el diametro pero no la equivalencia
                                        sam2_diametro2 = ctx2.Diametro.Where(x => x.Valor == diametro2.Valor).AsParallel().SingleOrDefault();

                                        //guardamos la equivalencia
                                        Sam3_EquivalenciaDiametro nuevaEquivalencia = new Sam3_EquivalenciaDiametro
                                        {
                                            Activo = true,
                                            FechaModificacion = DateTime.Now,
                                            Sam2_DiametroID = sam2_diametro2.DiametroID,
                                            Sam3_DiametroID = diametro2.DiametroID,
                                            UsuarioModificacion = usuario.UsuarioID
                                        };
                                        ctx.Sam3_EquivalenciaDiametro.Add(nuevaEquivalencia);
                                        ctx.SaveChanges();
                                    }
                                }

                                if (diametro1 == null)
                                {
                                    throw new Exception("Error en la búsqueda del diametro 1");
                                }
                                if (diametro2 == null)
                                {
                                    throw new Exception("Error en la búsqueda del diametro 2");
                                }
                                #endregion

                                #region rel ItemCode Diametros
                                //recuperamos la informacion del Itemcode
                                if (datos.ItemCodeID > 0)
                                {
                                    nuevoItemCode = ctx.Sam3_ItemCode.Where(x => x.ItemCodeID == datos.ItemCodeID).AsParallel().SingleOrDefault();
                                }
                                else
                                {
                                    if (datos.NumerosUnicos.Count() == 1)
                                    {
                                        int nuID = datos.NumerosUnicos[0];
                                        nuevoItemCode = (from nu in ctx.Sam3_NumeroUnico
                                                         join it in ctx.Sam3_ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                         where nu.Activo && it.Activo
                                                         && nu.NumeroUnicoID == nuID
                                                         select it).AsParallel().Distinct().SingleOrDefault();
                                    }
                                }

                                if (nuevoItemCode == null)
                                {
                                    throw new Exception("Error en la búsqueda de ItemCode");
                                }

                                //verificamos si existe la relacion de Itemcode con diametros, si no existe se crea una nueva
                                if (!ctx.Sam3_Rel_ItemCode_Diametro.Where(x => x.ItemCodeID == nuevoItemCode.ItemCodeID && x.Diametro1ID == diametro1.DiametroID
                                    && x.Diametro2ID == diametro2.DiametroID && x.Activo).Any())
                                {
                                    Sam3_Rel_ItemCode_Diametro nuevaRelITDiametros = new Sam3_Rel_ItemCode_Diametro
                                    {
                                        Activo = true,
                                        Diametro1ID = diametro1.DiametroID,
                                        Diametro2ID = diametro2.DiametroID,
                                        ItemCodeID = nuevoItemCode.ItemCodeID,
                                        FechaModificacion = DateTime.Now,
                                        UsuarioModificacion = usuario.UsuarioID
                                    };

                                    ctx.Sam3_Rel_ItemCode_Diametro.Add(nuevaRelITDiametros);
                                    ctx.SaveChanges();

                                    //Llegados a este punto sabemos que no existe un Itemcode Steelgo para esta relacioón, pues acaba de ser creada, se envia un error
                                    throw new Exception(string.Format("No existe un ItemCodeSteelgo asociado para el ItemCode: {0}, diametro1: {1}, diametro2: {2}",
                                        nuevoItemCode.ItemCodeID, diametro1.Valor, diametro2.Valor));
                                }
                                else
                                {
                                    // si ya existe la relacion de Itemcode y diametros verificamos que tenga un ItemCodeSteelgo asociado
                                    bool tieneICS = (from it in ctx.Sam3_ItemCode
                                                     join rid in ctx.Sam3_Rel_ItemCode_Diametro on it.ItemCodeID equals rid.ItemCodeID
                                                     join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                     join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                     join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                     join d1 in ctx.Sam3_Diametro on rids.Diametro1ID equals d1.DiametroID
                                                     join d2 in ctx.Sam3_Diametro on rids.Diametro2ID equals d2.DiametroID
                                                     where it.Activo && rid.Activo && riit.Activo && rids.Activo && ics.Activo && d1.Activo && d2.Activo
                                                     && it.ItemCodeID == nuevoItemCode.ItemCodeID
                                                     && d1.DiametroID == diametro1.DiametroID
                                                     && d2.DiametroID == diametro2.DiametroID
                                                     select ics).AsParallel().Any();

                                    if (!tieneICS) // si no hay un itemCodeSteelgo asociado se envia un error
                                    {
                                        throw new Exception(string.Format("No existe un ItemCodeSteelgo asociado para el ItemCode: {0}, diametro1: {1}, diametro2: {2}",
                                        nuevoItemCode.ItemCodeID, diametro1.Valor, diametro2.Valor));
                                    }
                                }
                                #endregion

                                #region Actualizar números únicos
                                List<Sam3_NumeroUnico> lstNumeroUnicos = (from nu in ctx.Sam3_NumeroUnico
                                                                          where nu.Activo
                                                                          && datos.NumerosUnicos.Contains(nu.NumeroUnicoID)
                                                                          select nu).Distinct().AsParallel().ToList();

                                foreach (Sam3_NumeroUnico nu in lstNumeroUnicos)
                                {
                                    nu.NumeroUnicoCliente = datos.NumeroUnicoCliente;
                                    nu.ItemCodeID = nuevoItemCode.ItemCodeID;
                                    nu.Diametro1 = diametro1.Valor;
                                    nu.Diametro2 = diametro2.Valor;
                                    nu.FechaModificacion = DateTime.Now;
                                    nu.UsuarioModificacion = usuario.UsuarioID;
                                }

                                //recuperamos el proyecto id de sam2 
                                sam2_ProyectoID = (from eq in ctx.Sam3_EquivalenciaProyecto
                                                   where eq.Activo && eq.Sam3_ProyectoID == nuevoItemCode.ProyectoID
                                                   select eq.Sam2_ProyectoID).AsParallel().SingleOrDefault();

                                //Buscamos los numeros unicos de sam2
                                DatabaseManager.Sam2.ItemCode sam2_ItemCode;
                                if (ctx.Sam3_EquivalenciaItemCode.Where(x => x.Activo && x.Sam3_ItemCodeID == nuevoItemCode.ItemCodeID).Any())
                                {
                                    int temp = (from eq in ctx.Sam3_EquivalenciaItemCode
                                                where eq.Activo && eq.Sam3_ItemCodeID == nuevoItemCode.ItemCodeID
                                                select eq.Sam2_ItemCodeID).AsParallel().SingleOrDefault();

                                    sam2_ItemCode = ctx2.ItemCode.Where(x => x.Codigo == nuevoItemCode.Codigo && x.ProyectoID == sam2_ProyectoID).AsParallel().SingleOrDefault();
                                }
                                else
                                {
                                    // no existe quivalencia de ItemCode
                                    sam2_ItemCode = ctx2.ItemCode.Where(x => x.Codigo == nuevoItemCode.Codigo && x.ProyectoID == sam2_ProyectoID).AsParallel().SingleOrDefault();
                                    Sam3_EquivalenciaItemCode nuevaEquivalencia = new Sam3_EquivalenciaItemCode
                                    {
                                        Activo = true,
                                        FechaModificacion = DateTime.Now,
                                        Sam2_ItemCodeID = sam2_ItemCode.ItemCodeID,
                                        Sam3_ItemCodeID = nuevoItemCode.ItemCodeID,
                                        UsuarioModificacion = usuario.UsuarioID
                                    };
                                    ctx.Sam3_EquivalenciaItemCode.Add(nuevaEquivalencia);
                                    ctx.SaveChanges();

                                }

                                if (sam2_ItemCode == null)
                                {
                                    throw new Exception("Error: el ItemCode: " + nuevoItemCode.Codigo + ", no se encuentra en SAM 2 o en la equivalencia");
                                }

                                List<int> sam2_NumerosUicos = (from eq in ctx.Sam3_EquivalenciaNumeroUnico
                                                               where eq.Activo
                                                               && datos.NumerosUnicos.Contains(eq.Sam3_NumeroUnicoID)
                                                               select eq.Sam2_NumeroUnicoID).Distinct().AsParallel().ToList();

                                List<NumeroUnico> lstSam2NumeroUnico = (from nu in ctx2.NumeroUnico
                                                                        where sam2_NumerosUicos.Contains(nu.NumeroUnicoID)
                                                                        select nu).Distinct().AsParallel().ToList();

                                foreach (NumeroUnico nu in lstSam2NumeroUnico)
                                {
                                    nu.NumeroUnicoCliente = datos.NumeroUnicoCliente;
                                    if (sam2_ItemCode.ItemCodeID > 0)
                                    {
                                        nu.ItemCodeID = sam2_ItemCode.ItemCodeID;
                                    }
                                    else
                                    {
                                        throw new Exception("No se encontro el ItemCode en SAM 2");
                                    }
                                    nu.Diametro1 = sam2_diametro1.Valor;
                                    nu.Diametro2 = sam2_diametro2.Valor;
                                    nu.FechaModificacion = DateTime.Now;
                                }

                                #endregion

                                ctx.SaveChanges();
                                ctx2.SaveChanges();

                                ctx_tran.Commit();
                                ctx2_tran.Commit();
                            } // fin tran sam 2
                        }// fin sam2
                    } // fin sam3 tran
                } // fin ctx

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("OK");
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
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ReemplazarItemCode(EditarItemCodeMasDetalle datos, Sam3_Usuario usuario)
        {
            try
            {
                Sam3_Diametro diametro1;
                Sam3_Diametro diametro2;
                Diametro sam2_diametro1;
                Diametro sam2_diametro2;
                Sam3_ItemCode nuevoItemCode = null;
                int sam2_ProyectoID = 0;
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var ctx2_tran = ctx2.Database.BeginTransaction())
                            {
                                #region diametros
                                //Diametro1
                                if (ctx.Sam3_Diametro.Where(x => x.Valor == datos.d1 && x.Activo).Any()) //Verificamos si existe el diametro
                                {
                                    diametro1 = ctx.Sam3_Diametro.Where(x => x.Valor == datos.d1 && x.Activo).AsParallel().SingleOrDefault();
                                }
                                else // el diametro no existe
                                {
                                    diametro1 = new Sam3_Diametro
                                    {
                                        Activo = true,
                                        FechaModificacion = DateTime.Now,
                                        UsuarioModificacion = usuario.UsuarioID,
                                        Valor = datos.d1,
                                        VerificadoPorCalidad = true
                                    };
                                    ctx.Sam3_Diametro.Add(diametro1);
                                    ctx.SaveChanges();
                                }

                                //verificamos si existe equivalencia para el diametro en sam2
                                if (ctx.Sam3_EquivalenciaDiametro.Where(x => x.Sam3_DiametroID == diametro1.DiametroID && x.Activo).Any())
                                {
                                    int sam2_diametroID = (from eq in ctx.Sam3_EquivalenciaDiametro
                                                           where eq.Activo && eq.Sam3_DiametroID == diametro1.DiametroID
                                                           select eq.Sam2_DiametroID.Value).SingleOrDefault();

                                    sam2_diametro1 = ctx2.Diametro.Where(x => x.DiametroID == sam2_diametroID).AsParallel().SingleOrDefault();

                                }
                                else
                                {
                                    //si no existe la equivalencia hay que crear el diametro en sam2
                                    if (!ctx.Sam3_Diametro.Where(x => x.Valor == diametro1.Valor).Any()) //verificamos si realmente no existe el diametro
                                    {
                                        sam2_diametro1 = new Diametro
                                        {
                                            Valor = diametro1.Valor,
                                            FechaModificacion = DateTime.Now,
                                            VerificadoPorCalidad = true
                                        };

                                        ctx2.Diametro.Add(sam2_diametro1);
                                        ctx2.SaveChanges();

                                        //guardamos la equivalencia
                                        Sam3_EquivalenciaDiametro nuevaEquivalencia = new Sam3_EquivalenciaDiametro
                                        {
                                            Activo = true,
                                            FechaModificacion = DateTime.Now,
                                            Sam2_DiametroID = sam2_diametro1.DiametroID,
                                            Sam3_DiametroID = diametro1.DiametroID,
                                            UsuarioModificacion = usuario.UsuarioID
                                        };
                                        ctx.Sam3_EquivalenciaDiametro.Add(nuevaEquivalencia);
                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        //Existe el diametro pero no la equivalencia
                                        sam2_diametro1 = ctx2.Diametro.Where(x => x.Valor == diametro1.Valor).AsParallel().SingleOrDefault();

                                        //guardamos la equivalencia
                                        Sam3_EquivalenciaDiametro nuevaEquivalencia = new Sam3_EquivalenciaDiametro
                                        {
                                            Activo = true,
                                            FechaModificacion = DateTime.Now,
                                            Sam2_DiametroID = sam2_diametro1.DiametroID,
                                            Sam3_DiametroID = diametro1.DiametroID,
                                            UsuarioModificacion = usuario.UsuarioID
                                        };
                                        ctx.Sam3_EquivalenciaDiametro.Add(nuevaEquivalencia);
                                        ctx.SaveChanges();
                                    }
                                }

                                //Diametro 2
                                if (ctx.Sam3_Diametro.Where(x => x.Valor == datos.d2 && x.Activo).Any()) //Verificamos si existe el diametro
                                {
                                    diametro2 = ctx.Sam3_Diametro.Where(x => x.Valor == datos.d2 && x.Activo).AsParallel().SingleOrDefault();
                                }
                                else // el diametro no existe
                                {
                                    diametro2 = new Sam3_Diametro
                                    {
                                        Activo = true,
                                        FechaModificacion = DateTime.Now,
                                        UsuarioModificacion = usuario.UsuarioID,
                                        Valor = datos.d2,
                                        VerificadoPorCalidad = true
                                    };
                                    ctx.Sam3_Diametro.Add(diametro2);
                                    ctx.SaveChanges();
                                }

                                //verificamos si existe equivalencia para el diametro en sam2
                                if (ctx.Sam3_EquivalenciaDiametro.Where(x => x.Sam3_DiametroID == diametro2.DiametroID && x.Activo).Any())
                                {
                                    int sam2_diametroID = (from eq in ctx.Sam3_EquivalenciaDiametro
                                                           where eq.Activo && eq.Sam3_DiametroID == diametro2.DiametroID
                                                           select eq.Sam2_DiametroID.Value).SingleOrDefault();

                                    sam2_diametro2 = ctx2.Diametro.Where(x => x.DiametroID == sam2_diametroID).AsParallel().SingleOrDefault();

                                }
                                else
                                {
                                    //si no existe la equivalencia hay que crear el diametro en sam2
                                    if (!ctx.Sam3_Diametro.Where(x => x.Valor == diametro2.Valor).Any()) //verificamos si realmente no existe el diametro
                                    {
                                        sam2_diametro2 = new Diametro
                                        {
                                            Valor = diametro2.Valor,
                                            FechaModificacion = DateTime.Now,
                                            VerificadoPorCalidad = true
                                        };

                                        ctx2.Diametro.Add(sam2_diametro2);
                                        ctx2.SaveChanges();

                                        //guardamos la equivalencia
                                        Sam3_EquivalenciaDiametro nuevaEquivalencia = new Sam3_EquivalenciaDiametro
                                        {
                                            Activo = true,
                                            FechaModificacion = DateTime.Now,
                                            Sam2_DiametroID = sam2_diametro2.DiametroID,
                                            Sam3_DiametroID = diametro2.DiametroID,
                                            UsuarioModificacion = usuario.UsuarioID
                                        };
                                        ctx.Sam3_EquivalenciaDiametro.Add(nuevaEquivalencia);
                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        //Existe el diametro pero no la equivalencia
                                        sam2_diametro2 = ctx2.Diametro.Where(x => x.Valor == diametro2.Valor).AsParallel().SingleOrDefault();

                                        //guardamos la equivalencia
                                        Sam3_EquivalenciaDiametro nuevaEquivalencia = new Sam3_EquivalenciaDiametro
                                        {
                                            Activo = true,
                                            FechaModificacion = DateTime.Now,
                                            Sam2_DiametroID = sam2_diametro2.DiametroID,
                                            Sam3_DiametroID = diametro2.DiametroID,
                                            UsuarioModificacion = usuario.UsuarioID
                                        };
                                        ctx.Sam3_EquivalenciaDiametro.Add(nuevaEquivalencia);
                                        ctx.SaveChanges();
                                    }
                                }

                                if (diametro1 == null)
                                {
                                    throw new Exception("Error en la búsqueda del diametro 1");
                                }
                                if (diametro2 == null)
                                {
                                    throw new Exception("Error en la búsqueda del diametro 2");
                                }
                                #endregion

                                #region rel ItemCode Diametros
                                //recuperamos la informacion del Itemcode
                                if (datos.ItemCodeID > 0)
                                {
                                    nuevoItemCode = ctx.Sam3_ItemCode.Where(x => x.ItemCodeID == datos.ItemCodeID).AsParallel().SingleOrDefault();
                                }
                                else
                                {
                                    if (datos.NumerosUnicos.Count() == 1)
                                    {
                                        int nuID = datos.NumerosUnicos[0];
                                        nuevoItemCode = (from nu in ctx.Sam3_NumeroUnico
                                                         join it in ctx.Sam3_ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                         where nu.Activo && it.Activo
                                                         && nu.NumeroUnicoID == nuID
                                                         select it).AsParallel().Distinct().SingleOrDefault();
                                    }
                                }

                                if (nuevoItemCode == null)
                                {
                                    throw new Exception("Error en la búsqueda de ItemCode");
                                }

                                //verificamos si existe la relacion de Itemcode con diametros, si no existe se crea una nueva
                                if (!ctx.Sam3_Rel_ItemCode_Diametro.Where(x => x.ItemCodeID == nuevoItemCode.ItemCodeID && x.Diametro1ID == diametro1.DiametroID
                                    && x.Diametro2ID == diametro2.DiametroID && x.Activo).Any())
                                {
                                    Sam3_Rel_ItemCode_Diametro nuevaRelITDiametros = new Sam3_Rel_ItemCode_Diametro
                                    {
                                        Activo = true,
                                        Diametro1ID = diametro1.DiametroID,
                                        Diametro2ID = diametro2.DiametroID,
                                        ItemCodeID = nuevoItemCode.ItemCodeID,
                                        FechaModificacion = DateTime.Now,
                                        UsuarioModificacion = usuario.UsuarioID
                                    };

                                    ctx.Sam3_Rel_ItemCode_Diametro.Add(nuevaRelITDiametros);
                                    ctx.SaveChanges();

                                    //Llegados a este punto sabemos que no existe un Itemcode Steelgo para esta relacioón, pues acaba de ser creada, se envia un error
                                    throw new Exception(string.Format("No existe un ItemCodeSteelgo asociado para el ItemCode: {0}, diametro1: {1}, diametro2: {2}",
                                        nuevoItemCode.ItemCodeID, diametro1.Valor, diametro2.Valor));
                                }
                                else
                                {
                                    // si ya existe la relacion de Itemcode y diametros verificamos que tenga un ItemCodeSteelgo asociado
                                    bool tieneICS = (from it in ctx.Sam3_ItemCode
                                                     join rid in ctx.Sam3_Rel_ItemCode_Diametro on it.ItemCodeID equals rid.ItemCodeID
                                                     join riit in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals riit.Rel_ItemCode_Diametro_ID
                                                     join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on riit.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                     join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                     join d1 in ctx.Sam3_Diametro on rids.Diametro1ID equals d1.DiametroID
                                                     join d2 in ctx.Sam3_Diametro on rids.Diametro2ID equals d2.DiametroID
                                                     where it.Activo && rid.Activo && riit.Activo && rids.Activo && ics.Activo && d1.Activo && d2.Activo
                                                     && it.ItemCodeID == nuevoItemCode.ItemCodeID
                                                     && d1.DiametroID == diametro1.DiametroID
                                                     && d2.DiametroID == diametro2.DiametroID
                                                     select ics).AsParallel().Any();

                                    if (!tieneICS) // si no hay un itemCodeSteelgo asociado se envia un error
                                    {
                                        throw new Exception(string.Format("No existe un ItemCodeSteelgo asociado para el ItemCode: {0}, diametro1: {1}, diametro2: {2}",
                                        nuevoItemCode.ItemCodeID, diametro1.Valor, diametro2.Valor));
                                    }
                                }
                                #endregion

                                #region Actualizar números únicos
                                List<Sam3_NumeroUnico> lstNumeroUnicos = (from nu in ctx.Sam3_NumeroUnico
                                                                          where nu.Activo
                                                                          && datos.NumerosUnicos.Contains(nu.NumeroUnicoID)
                                                                          select nu).Distinct().AsParallel().ToList();

                                foreach (Sam3_NumeroUnico nu in lstNumeroUnicos)
                                {
                                    nu.NumeroUnicoCliente = datos.NumeroUnicoCliente;
                                    nu.ItemCodeID = nuevoItemCode.ItemCodeID;
                                    nu.Diametro1 = diametro1.Valor;
                                    nu.Diametro2 = diametro2.Valor;
                                    nu.FechaModificacion = DateTime.Now;
                                    nu.UsuarioModificacion = usuario.UsuarioID;
                                }

                                //recuperamos el proyecto id de sam2 
                                sam2_ProyectoID = (from eq in ctx.Sam3_EquivalenciaProyecto
                                                   where eq.Activo && eq.Sam3_ProyectoID == nuevoItemCode.ProyectoID
                                                   select eq.Sam2_ProyectoID).AsParallel().SingleOrDefault();

                                //Buscamos los numeros unicos de sam2
                                DatabaseManager.Sam2.ItemCode sam2_ItemCode;
                                if (ctx.Sam3_EquivalenciaItemCode.Where(x => x.Activo && x.Sam3_ItemCodeID == nuevoItemCode.ItemCodeID).Any())
                                {
                                    int temp = (from eq in ctx.Sam3_EquivalenciaItemCode
                                                where eq.Activo && eq.Sam3_ItemCodeID == nuevoItemCode.ItemCodeID
                                                select eq.Sam2_ItemCodeID).AsParallel().SingleOrDefault();

                                    sam2_ItemCode = ctx2.ItemCode.Where(x => x.Codigo == nuevoItemCode.Codigo && x.ProyectoID == sam2_ProyectoID).AsParallel().SingleOrDefault();
                                }
                                else
                                {
                                    // no existe quivalencia de ItemCode
                                    sam2_ItemCode = ctx2.ItemCode.Where(x => x.Codigo == nuevoItemCode.Codigo && x.ProyectoID == sam2_ProyectoID).AsParallel().SingleOrDefault();
                                    Sam3_EquivalenciaItemCode nuevaEquivalencia = new Sam3_EquivalenciaItemCode
                                    {
                                        Activo = true,
                                        FechaModificacion = DateTime.Now,
                                        Sam2_ItemCodeID = sam2_ItemCode.ItemCodeID,
                                        Sam3_ItemCodeID = nuevoItemCode.ItemCodeID,
                                        UsuarioModificacion = usuario.UsuarioID
                                    };
                                    ctx.Sam3_EquivalenciaItemCode.Add(nuevaEquivalencia);
                                    ctx.SaveChanges();

                                }

                                if (sam2_ItemCode == null)
                                {
                                    throw new Exception("Error: el ItemCode: " + nuevoItemCode.Codigo + ", no se encuentra en SAM 2 o en la equivalencia");
                                }

                                List<int> sam2_NumerosUicos = (from eq in ctx.Sam3_EquivalenciaNumeroUnico
                                                               where eq.Activo
                                                               && datos.NumerosUnicos.Contains(eq.Sam3_NumeroUnicoID)
                                                               select eq.Sam2_NumeroUnicoID).Distinct().AsParallel().ToList();

                                List<NumeroUnico> lstSam2NumeroUnico = (from nu in ctx2.NumeroUnico
                                                                        where sam2_NumerosUicos.Contains(nu.NumeroUnicoID)
                                                                        select nu).Distinct().AsParallel().ToList();

                                foreach (NumeroUnico nu in lstSam2NumeroUnico)
                                {
                                    nu.NumeroUnicoCliente = datos.NumeroUnicoCliente;
                                    if (sam2_ItemCode.ItemCodeID > 0)
                                    {
                                        nu.ItemCodeID = sam2_ItemCode.ItemCodeID;
                                    }
                                    else
                                    {
                                        throw new Exception("No se encontro el ItemCode en SAM 2");
                                    }
                                    nu.Diametro1 = sam2_diametro1.Valor;
                                    nu.Diametro2 = sam2_diametro2.Valor;
                                    nu.FechaModificacion = DateTime.Now;
                                }

                                #endregion

                                ctx.SaveChanges();
                                ctx2.SaveChanges();

                                ctx_tran.Commit();
                                ctx2_tran.Commit();
                            } // fin tran sam 2
                        }// fin sam2
                    } // fin sam3 tran
                } // fin ctx

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("OK");
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
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }
    }
}
