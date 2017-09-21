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
using System.Transactions;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;
using System.Data;
using DatabaseManager.Constantes;

namespace BackEndSAM.DataAcces
{
    public class OrdenRecepcionBd
    {
        private static readonly object _mutex = new object();
        private static OrdenRecepcionBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private OrdenRecepcionBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static OrdenRecepcionBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new OrdenRecepcionBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoGenerarOrdenRecepcion(FiltrosJson filtros, Sam3_Usuario usuario, bool conteo = false)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    DateTime fechaInicial = new DateTime();
                    DateTime fechaFinal = new DateTime();
                    DateTime.TryParse(filtros.FechaInicial, out fechaInicial);
                    DateTime.TryParse(filtros.FechaFinal, out fechaFinal);

                    int proyectoID = filtros.ProyectoID != "" ? Convert.ToInt32(filtros.ProyectoID) : 0;
                    int folioAvisoLlegadaID = filtros.FolioAvisoEntradaID != "" ? Convert.ToInt32(filtros.FolioAvisoEntradaID) : 0;
                    int itemCodeID = filtros.ItemCodeID != "" ? Convert.ToInt32(filtros.ItemCodeID) : 0;
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"])
                        ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;
                    int tipoMaterialID = 0;
                    int.TryParse(filtros.TipoMaterialID, out tipoMaterialID);
                    int packingListID = filtros.PackingListID != "" ? Convert.ToInt32(filtros.PackingListID) : 0;
                    int clienteID = filtros.ClienteID != "" ? Convert.ToInt32(filtros.ClienteID) : 0;


                    //Patios y proyectos del usuario
                    List<int> proyectos;
                    List<int> patios;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    List<Sam3_FolioAvisoEntrada> registros;

                    if (proyectoID > 0)
                    {
                        registros = (from r in ctx.Sam3_FolioAvisoEntrada
                                     join fa in ctx.Sam3_FolioAvisoLlegada on r.FolioAvisoLlegadaID equals fa.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fa.FolioAvisoLlegadaID equals p.FolioAvisoLlegadaID
                                     join pr in ctx.Sam3_Proyecto on p.ProyectoID equals pr.ProyectoID
                                     join f in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals f.FolioAvisoEntradaID
                                     where r.Activo && fa.Activo && p.Activo && pr.Activo && f.Activo
                                     && proyectos.Contains(pr.ProyectoID)
                                     && patios.Contains(pr.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && patios.Contains(fa.PatioID)
                                     && patios.Contains(r.PatioID)
                                     //&& (fa.FechaModificacion >= fechaInicial && fa.FechaModificacion <= fechaFinal)
                                     select r).AsParallel().ToList();
                    }
                    else
                    {
                        registros = (from r in ctx.Sam3_FolioAvisoEntrada
                                     join fa in ctx.Sam3_FolioAvisoLlegada on r.FolioAvisoLlegadaID equals fa.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fa.FolioAvisoLlegadaID equals p.FolioAvisoLlegadaID
                                     join pr in ctx.Sam3_Proyecto on p.ProyectoID equals pr.ProyectoID
                                     join f in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals f.FolioAvisoEntradaID
                                     where r.Activo && fa.Activo && p.Activo && pr.Activo && f.Activo
                                     && proyectos.Contains(pr.ProyectoID)
                                     && patios.Contains(pr.PatioID)
                                     && patios.Contains(fa.PatioID)
                                     && patios.Contains(r.PatioID)
                                     //&& (fa.FechaModificacion >= fechaInicial && fa.FechaModificacion <= fechaFinal)
                                     select r).AsParallel().ToList();
                    }

                    if (folioAvisoLlegadaID > 0)
                    {
                        registros = registros.Where(x => x.FolioAvisoLlegadaID == folioAvisoLlegadaID).AsParallel().ToList();
                    }

                    if (clienteID > 0)
                    {
                        int sam3_clienteID = (from c in ctx.Sam3_Cliente
                                              where c.Activo && c.Sam2ClienteID == clienteID
                                              select c.ClienteID).AsParallel().SingleOrDefault();

                        registros = registros.Where(x => x.ClienteID == sam3_clienteID).ToList();
                    }

                    if (packingListID > 0)
                    {
                        registros = (from r in registros
                                     join fc in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                     where fc.Activo
                                     && fc.FolioCuantificacionID == packingListID
                                     select r).AsParallel().ToList();
                    }

                    registros = registros.GroupBy(x => x.FolioAvisoEntradaID).Select(x => x.First()).ToList();

                    List<ListadoGenerarOrdenRecepcion> listado = new List<ListadoGenerarOrdenRecepcion>();

                    foreach (Sam3_FolioAvisoEntrada f in registros)
                    {
                        ListadoGenerarOrdenRecepcion elemento = new ListadoGenerarOrdenRecepcion();
                        Sam3_FolioAvisoLlegada FolioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == f.FolioAvisoLlegadaID).FirstOrDefault();

                        elemento.AvisoEntradaID = f.FolioAvisoLlegadaID.ToString();

                        elemento.FolioConfiguracion = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                   where pc.Entidad == FolioAvisoLlegada.Entidad && pc.Proyecto == FolioAvisoLlegada.ProyectoNombrado
                                                                                   select pc.PreFijoFolioAvisoLlegada + ","
                                                                                    + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                                    + FolioAvisoLlegada.Consecutivo.ToString() + ","
                                                                                    + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : f.FolioAvisoLlegadaID.ToString();
                        if (activarFolioConfiguracion)
                        {
                            string[] elemntos = elemento.FolioConfiguracion.Split(',').ToArray();
                            int digitos = Convert.ToInt32(elemntos[1]);
                            int consecutivo = Convert.ToInt32(elemntos[2]);
                            string formato = "D" + digitos.ToString();

                            elemento.FolioConfiguracion = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                        }

                        List<ElementoItemCodeGenerarOrden> tubosRFC = new List<ElementoItemCodeGenerarOrden>();
                        List<ElementoItemCodeGenerarOrden> tubosRB = new List<ElementoItemCodeGenerarOrden>();
                        List<ElementoItemCodeGenerarOrden> AccesoriosRFC = new List<ElementoItemCodeGenerarOrden>();
                        List<ElementoItemCodeGenerarOrden> AccesoriosRB = new List<ElementoItemCodeGenerarOrden>();

                        tubosRFC = (from r in ctx.Sam3_FolioAvisoEntrada
                                    join c in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals c.FolioAvisoEntradaID
                                    join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on r.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                    join rfi in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on c.FolioCuantificacionID equals rfi.FolioCuantificacionID
                                    join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                    join i in ctx.Sam3_ItemCode on rid.ItemCodeID equals i.ItemCodeID
                                    join t in ctx.Sam3_TipoMaterial on i.TipoMaterialID equals t.TipoMaterialID
                                    join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                    join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                    join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                    join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                    where r.Activo && c.Activo && rfp.Activo && rfi.Activo && rid.Activo && i.Activo && t.Activo
                                    && d1.Activo && d2.Activo && rics.Activo && ics.Activo
                                    && i.TipoMaterialID == 1
                                    && r.FolioAvisoLlegadaID == f.FolioAvisoLlegadaID
                                    //&& !rfi.TieneNumerosUnicos
                                    && (from relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                        where relnu.Rel_FolioCuantificacion_ItemCode_ID == rfi.Rel_FolioCuantificacion_ItemCode_ID
                                        && relnu.Activo
                                        select relnu.NumeroUnicoID).Count() < rfi.Cantidad
                                    && rfi.Cantidad > 0
                                    && !(from co in ctx.Sam3_Colada
                                         where co.Activo && co.NumeroColada == ""
                                         && co.ProyectoID == i.ProyectoID
                                         select co.ColadaID).Contains(rfi.ColadaID)
                                    && rfi.ColadaID > 0
                                    && rfi.MM > 0
                                    && ics.Codigo != "ICS-Default"
                                    select new ElementoItemCodeGenerarOrden
                                    {
                                        ItemCodeID = rid.Rel_ItemCode_Diametro_ID.ToString(),
                                        Cantidad = (rfi.Cantidad - (from relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                                                    where relnu.Rel_FolioCuantificacion_ItemCode_ID == rfi.Rel_FolioCuantificacion_ItemCode_ID
                                                                    && relnu.Activo
                                                                    select relnu.NumeroUnicoID).Count()).ToString(),
                                        Codigo = i.Codigo + "(" + d1.Valor.ToString() + ", " + d2.Valor.ToString() + ")",
                                        D1 = d1.Valor.ToString(),
                                        D2 = d2.Valor.ToString(),
                                        Descripcion = i.DescripcionEspanol,
                                        TipoMaterial = t.Nombre,
                                        FolioAvisoLlegadaId = r.FolioAvisoLlegadaID.ToString(),
                                        RelFCId = rfi.Rel_FolioCuantificacion_ItemCode_ID.ToString(),
                                        ItemCodeIDOriginal = i.ItemCodeID
                                    }).AsParallel().Distinct().ToList();



                        tubosRB = (from r in ctx.Sam3_FolioAvisoEntrada
                                   join c in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals c.FolioAvisoEntradaID
                                   join b in ctx.Sam3_Bulto on c.FolioCuantificacionID equals b.FolioCuantificacionID
                                   join rbi in ctx.Sam3_Rel_Bulto_ItemCode on b.BultoID equals rbi.BultoID
                                   join rid in ctx.Sam3_Rel_ItemCode_Diametro on rbi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                   join i in ctx.Sam3_ItemCode on rid.ItemCodeID equals i.ItemCodeID
                                   join t in ctx.Sam3_TipoMaterial on i.TipoMaterialID equals t.TipoMaterialID
                                   join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                   join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                   join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                   join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                   where r.Activo && c.Activo && b.Activo && rbi.Activo && rid.Activo && i.Activo && t.Activo
                                   && d1.Activo && d2.Activo && rics.Activo && ics.Activo
                                   && i.TipoMaterialID == 1
                                   && r.FolioAvisoLlegadaID == f.FolioAvisoLlegadaID
                                   //&& !rbi.TieneNumerosUnicos
                                   && (from relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                       where relnu.Rel_Bulto_ItemCode_ID == rbi.Rel_Bulto_ItemCode_ID
                                       && relnu.Activo
                                       select relnu.NumeroUnicoID).Count() < rbi.Cantidad
                                   && rbi.Cantidad > 0
                                   && !(from co in ctx.Sam3_Colada
                                        where co.Activo && co.NumeroColada == ""
                                        && co.ProyectoID == i.ProyectoID
                                        select co.ColadaID).Contains(rbi.ColadaID)
                                   && rbi.ColadaID > 0
                                   && rbi.MM > 0
                                   && ics.Codigo != "ICS-Default"
                                   select new ElementoItemCodeGenerarOrden
                                   {
                                       ItemCodeID = rid.Rel_ItemCode_Diametro_ID.ToString(),
                                       Cantidad = (rbi.Cantidad - (from relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                                                   where relnu.Rel_Bulto_ItemCode_ID == rbi.Rel_Bulto_ItemCode_ID
                                                                   && relnu.Activo
                                                                   select relnu.NumeroUnicoID).Count()).ToString(),
                                       Codigo = i.Codigo + "(" + d1.Valor.ToString() + ", " + d2.Valor.ToString() + ")",
                                       D1 = d1.Valor.ToString(),
                                       D2 = d2.Valor.ToString(),
                                       Descripcion = i.DescripcionEspanol,
                                       TipoMaterial = t.Nombre,
                                       FolioAvisoLlegadaId = r.FolioAvisoLlegadaID.ToString(),
                                       RelBID = rbi.Rel_Bulto_ItemCode_ID.ToString(),
                                       ItemCodeIDOriginal = i.ItemCodeID
                                   }).AsParallel().Distinct().ToList();

                        AccesoriosRFC = (from r in ctx.Sam3_FolioAvisoEntrada
                                         join c in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals c.FolioAvisoEntradaID
                                         join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on r.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                         join rfi in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on c.FolioCuantificacionID equals rfi.FolioCuantificacionID
                                         join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                         join i in ctx.Sam3_ItemCode on rid.ItemCodeID equals i.ItemCodeID
                                         join t in ctx.Sam3_TipoMaterial on i.TipoMaterialID equals t.TipoMaterialID
                                         join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                         join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                         join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                         join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                         where r.Activo && c.Activo && rfp.Activo && rfi.Activo && rid.Activo && i.Activo && t.Activo
                                         && d1.Activo && d2.Activo && rics.Activo && ics.Activo
                                         && i.TipoMaterialID == 2
                                         && r.FolioAvisoLlegadaID == f.FolioAvisoLlegadaID
                                         //&& !rfi.TieneNumerosUnicos
                                         && (from relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                             where relnu.Rel_FolioCuantificacion_ItemCode_ID == rfi.Rel_FolioCuantificacion_ItemCode_ID
                                             && relnu.Activo
                                             select relnu.NumeroUnicoID).Count() < rfi.Cantidad
                                         && rfi.Cantidad > 0
                                         && !(from co in ctx.Sam3_Colada
                                              where co.Activo && co.NumeroColada == ""
                                              && co.ProyectoID == i.ProyectoID
                                              select co.ColadaID).Contains(rfi.ColadaID)
                                         && rfi.ColadaID > 0
                                         && ics.Codigo != "ICS-Default"
                                         select new ElementoItemCodeGenerarOrden
                                         {
                                             ItemCodeID = rid.Rel_ItemCode_Diametro_ID.ToString(),
                                             Cantidad = (rfi.Cantidad - (from relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                                                         where relnu.Rel_FolioCuantificacion_ItemCode_ID == rfi.Rel_FolioCuantificacion_ItemCode_ID
                                                                         && relnu.Activo
                                                                         select relnu.NumeroUnicoID).Count()).ToString(),
                                             Codigo = i.Codigo + "(" + d1.Valor.ToString() + ", " + d2.Valor.ToString() + ")",
                                             D1 = d1.Valor.ToString(),
                                             D2 = d2.Valor.ToString(),
                                             Descripcion = i.DescripcionEspanol,
                                             TipoMaterial = t.Nombre,
                                             FolioAvisoLlegadaId = r.FolioAvisoLlegadaID.ToString(),
                                             RelFCId = rfi.Rel_FolioCuantificacion_ItemCode_ID.ToString(),
                                             ItemCodeIDOriginal = i.ItemCodeID
                                         }).AsParallel().Distinct().ToList();



                        AccesoriosRB = (from r in ctx.Sam3_FolioAvisoEntrada
                                        join c in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals c.FolioAvisoEntradaID
                                        join b in ctx.Sam3_Bulto on c.FolioCuantificacionID equals b.FolioCuantificacionID
                                        join rbi in ctx.Sam3_Rel_Bulto_ItemCode on b.BultoID equals rbi.BultoID
                                        join rid in ctx.Sam3_Rel_ItemCode_Diametro on rbi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                        join i in ctx.Sam3_ItemCode on rid.ItemCodeID equals i.ItemCodeID
                                        join t in ctx.Sam3_TipoMaterial on i.TipoMaterialID equals t.TipoMaterialID
                                        join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                        join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                        join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                        join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                        where r.Activo && c.Activo && b.Activo && rbi.Activo && rid.Activo && i.Activo && t.Activo
                                        && d1.Activo && d2.Activo && rics.Activo && ics.Activo
                                        && i.TipoMaterialID == 2
                                        && r.FolioAvisoLlegadaID == f.FolioAvisoLlegadaID
                                        //&& !rbi.TieneNumerosUnicos
                                        && (from relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                            where relnu.Rel_Bulto_ItemCode_ID == rbi.Rel_Bulto_ItemCode_ID
                                            && relnu.Activo
                                            select relnu.NumeroUnicoID).Count() < rbi.Cantidad
                                        && rbi.Cantidad > 0
                                        && !(from co in ctx.Sam3_Colada
                                             where co.Activo && co.NumeroColada == ""
                                             && co.ProyectoID == i.ProyectoID
                                             select co.ColadaID).Contains(rbi.ColadaID)
                                        && rbi.ColadaID > 0
                                        && ics.Codigo != "ICS-Default"
                                        select new ElementoItemCodeGenerarOrden
                                        {
                                            ItemCodeID = rid.Rel_ItemCode_Diametro_ID.ToString(),
                                            Cantidad = (rbi.Cantidad - (from relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB
                                                                        where relnu.Rel_Bulto_ItemCode_ID == rbi.Rel_Bulto_ItemCode_ID
                                                                        && relnu.Activo
                                                                        select relnu.NumeroUnicoID).Count()).ToString(),
                                            Codigo = i.Codigo + "(" + d1.Valor.ToString() + ", " + d2.Valor.ToString() + ")",
                                            D1 = d1.Valor.ToString(),
                                            D2 = d2.Valor.ToString(),
                                            Descripcion = i.DescripcionEspanol,
                                            TipoMaterial = t.Nombre,
                                            FolioAvisoLlegadaId = r.FolioAvisoLlegadaID.ToString(),
                                            RelBID = rbi.Rel_Bulto_ItemCode_ID.ToString(),
                                            ItemCodeIDOriginal = i.ItemCodeID
                                        }).AsParallel().Distinct().ToList();


                        tubosRB = tubosRB.GroupBy(x => x.RelBID).Select(x => x.First()).ToList();
                        tubosRFC = tubosRFC.GroupBy(x => x.RelFCId).Select(x => x.First()).ToList();

                        AccesoriosRB = AccesoriosRB.GroupBy(x => x.RelBID).Select(x => x.First()).ToList();
                        AccesoriosRFC = AccesoriosRFC.GroupBy(x => x.RelFCId).Select(x => x.First()).ToList();

                        if (tubosRB.Count > 0)
                        {
                            elemento.Tubos.AddRange(tubosRB);
                        }
                        if (tubosRFC.Count > 0)
                        {
                            elemento.Tubos.AddRange(tubosRFC);
                        }
                        if (AccesoriosRB.Count > 0)
                        {
                            elemento.Accesorios.AddRange(AccesoriosRB);
                        }
                        if (AccesoriosRFC.Count > 0)
                        {
                            elemento.Accesorios.AddRange(AccesoriosRFC);
                        }

                        if (itemCodeID > 0)
                        {
                            elemento.Tubos = elemento.Tubos.Where(x => x.ItemCodeIDOriginal == itemCodeID).AsParallel().ToList();
                            elemento.Accesorios = elemento.Accesorios.Where(x => x.ItemCodeIDOriginal == itemCodeID).AsParallel().ToList();
                        }

                        if (elemento.Tubos.Count > 0 || elemento.Accesorios.Count > 0)
                        {
                            listado.Add(elemento);
                        }

                    }
                    int TotalTubos = 0;
                    int TotalAccesorios = 0;
                    foreach (ListadoGenerarOrdenRecepcion f in listado)
                    {
                        TotalAccesorios += f.Accesorios.Count();
                        TotalTubos += f.Tubos.Count();
                    };
#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(listado);
#endif
                    if (conteo)
                    {
                        switch (tipoMaterialID)
                        {
                            case 1:
                                return TotalTubos;
                                break;
                            case 2:
                                return TotalAccesorios;
                                break;
                            case 3:
                                return TotalAccesorios + TotalTubos;
                                break;
                            default:
                                return listado.Count;
                                break;

                        }


                    }
                    else
                    {
                        return listado.OrderBy(x => x.AvisoEntradaID).ToList();
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

        public object ObtenerListadoOrdenRecepcion(FiltrosJson filtros, Sam3_Usuario usuario, bool conteo = false)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    DateTime fechaInicial = new DateTime();
                    DateTime fechaFinal = new DateTime();
                    DateTime.TryParse(filtros.FechaInicial, out fechaInicial);
                    DateTime.TryParse(filtros.FechaFinal, out fechaFinal);
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;
                    Boolean activarFolioConfiguracionOR = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionOrdenRecepcion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionOrdenRecepcion"].Equals("1") ? true : false) : false;

                    if (fechaFinal.ToShortDateString() == "1/1/0001")
                    {
                        fechaFinal = DateTime.Now;
                    }

                    if (fechaInicial.ToShortDateString() == "1/1/0001")
                    {
                        //int mes = DateTime.Now.Month != 1 ? DateTime.Now.Month - 1 : 12;
                        //int year = DateTime.Now.Month == 1 ? DateTime.Now.Year - 1 : DateTime.Now.Year;
                        //fechaInicial = new DateTime(year, mes, DateTime.Now.Day);
                        fechaInicial = new DateTime(2000, 01, 01);
                    }

                    int clienteID = filtros.ClienteID != "" ? Convert.ToInt32(filtros.ClienteID) : 0;
                    int folioAvisollegadaID = filtros.FolioAvisoLlegadaID != "" ? Convert.ToInt32(filtros.FolioAvisoLlegadaID) : 0;
                    int proyectoID = filtros.ProyectoID != "" ? Convert.ToInt32(filtros.ProyectoID) : 0;

                    //Patios y proyectos del usuario
                    List<int> proyectos = ctx.Sam3_Rel_Usuario_Proyecto.Where(x => x.UsuarioID == usuario.UsuarioID).Select(x => x.ProyectoID).AsParallel().ToList();

                    List<int> patios = (from r in ctx.Sam3_Proyecto
                                        join p in ctx.Sam3_Patio on r.PatioID equals p.PatioID
                                        where r.Activo && proyectos.Contains(r.ProyectoID)
                                        select p.PatioID).AsParallel().ToList();
                    //-----------------------------------------------------------------------------------
                    List<Sam3_OrdenRecepcion> ordenes = new List<Sam3_OrdenRecepcion>();

                    if (proyectoID > 0)
                    {
                        ordenes = (from o in ctx.Sam3_OrdenRecepcion
                                   join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on o.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                   join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                   join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                   join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                   join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                   where o.Activo && rfo.Activo && fe.Activo && rfp.Activo && p.Activo && pa.Activo
                                   && (o.FechaCreacion >= fechaInicial && o.FechaCreacion <= fechaFinal)
                                   && proyectos.Contains(p.ProyectoID)
                                   && p.ProyectoID == proyectoID
                                   select o).ToList();
                    }
                    else
                    {
                        ordenes = (from o in ctx.Sam3_OrdenRecepcion
                                   join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on o.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                   join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                   join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                   join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                   join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                   where o.Activo && rfo.Activo && fe.Activo && rfp.Activo && p.Activo && pa.Activo
                                   && (o.FechaCreacion >= fechaInicial && o.FechaCreacion <= fechaFinal)
                                   && proyectos.Contains(p.ProyectoID)
                                   select o).ToList();
                    }

                    if (clienteID > 0)
                    {
                        int sam3Cliente = (from c in ctx.Sam3_Cliente
                                           where c.Activo && c.Sam2ClienteID == clienteID
                                           select c.ClienteID).AsParallel().SingleOrDefault();
                        ordenes = (from o in ctx.Sam3_OrdenRecepcion
                                   join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on o.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                   join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                   where o.Activo && rfo.Activo && fe.Activo
                                   && fe.ClienteID == sam3Cliente
                                   && (o.FechaCreacion >= fechaInicial && o.FechaCreacion <= fechaFinal)
                                   select o).ToList();
                    }

                    if (folioAvisollegadaID > 0)
                    {
                        ordenes = (from o in ctx.Sam3_OrdenRecepcion
                                   join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on o.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                   join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                   where o.Activo && rfo.Activo && fe.Activo
                                   && fe.FolioAvisoLlegadaID == folioAvisollegadaID
                                   && (o.FechaCreacion >= fechaInicial && o.FechaCreacion <= fechaFinal)
                                   select o).ToList();
                    }

                    ordenes = ordenes.GroupBy(x => x.OrdenRecepcionID).Select(x => x.First()).ToList();

                    List<ListadoOrdeRecepcion> listado = new List<ListadoOrdeRecepcion>();
                    string max = ordenes.Count() > 0 ? ordenes.Max(x => x.Folio).ToString() : "0";
                    int maxpadleft = max.Length;
                    foreach (Sam3_OrdenRecepcion o in ordenes)
                    {
                        ListadoOrdeRecepcion elemento = new ListadoOrdeRecepcion();
                        var fecha = o.FechaCreacion.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US"));

                        //DateTime date = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.CurrentCulture);

                        elemento.FechaOrdenRecepcion = fecha;
                        elemento.OrdenRecepcion = o.Folio.ToString();
                        elemento.OrdenRecepcionID = o.Folio.ToString();

                        if (activarFolioConfiguracionOR)
                        {
                            Sam3_Rel_Proyecto_Entidad_Configuracion rel_proy = (from rel in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                where rel.Rel_Proyecto_Entidad_Configuracion_ID == o.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                select rel).AsParallel().SingleOrDefault();

                            if (rel_proy != null)
                            {
                                string folioOR = rel_proy.PreFijoFolioOrdenRecepcion + ","
                                    + rel_proy.CantidadCerosFolioOrdenRecepcion.ToString() + ","
                                    + o.Consecutivo.ToString() + ","
                                    + rel_proy.PostFijoFolioOrdenRecepcion;

                                string[] elemntos = folioOR.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int cons = Convert.ToInt32(elemntos[2]);
                                string formato_proy = "D" + digitos.ToString();

                                elemento.OrdenRecepcion = elemntos[0].Trim() + cons.ToString(formato_proy).Trim() + elemntos[3].Trim();
                            }
                        }
                        else
                        {
                            //var applypadleft = maxpadleft;
                            elemento.OrdenRecepcion = elemento.OrdenRecepcion.PadLeft(maxpadleft, '0');
                        }


                        List<ElementoItemCode> items = (from roi in ctx.Sam3_Rel_OrdenRecepcion_ItemCode
                                                        join rid in ctx.Sam3_Rel_ItemCode_Diametro on roi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                        join i in ctx.Sam3_ItemCode on rid.ItemCodeID equals i.ItemCodeID
                                                        join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on roi.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                                        join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                                        where roi.OrdenRecepcionID == o.OrdenRecepcionID
                                                        select new ElementoItemCode
                                                        {
                                                            Cantidad = roi.Cantidad != null ? roi.Cantidad.Value : 0,
                                                            Itemcode = i.Codigo,
                                                            AvisoEntrada = fe.FolioAvisoLlegadaID.ToString(),
                                                            FolioConfiguracion = fe.FolioAvisoLlegadaID.ToString()
                                                        }).AsParallel().ToList();


                        if (activarFolioConfiguracion)
                        {
                            foreach (ElementoItemCode row in items)
                            {
                                int folioAvisoLlegadaID = Convert.ToInt32(row.AvisoEntrada);
                                Sam3_FolioAvisoLlegada FolioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == folioAvisoLlegadaID).FirstOrDefault();

                                row.FolioConfiguracion = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                          where pc.Entidad == FolioAvisoLlegada.Entidad && pc.Proyecto == FolioAvisoLlegada.ProyectoNombrado
                                                          select pc.PreFijoFolioAvisoLlegada + ","
                                                           + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                           + FolioAvisoLlegada.Consecutivo.ToString() + ","
                                                           + pc.PostFijoFolioAvisoLlegada).FirstOrDefault();

                                string[] elemntos = row.FolioConfiguracion.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                row.FolioConfiguracion = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }
                        }

                        elemento.Detalle = items.OrderBy(x => x.AvisoEntrada).ToList();

                        if (elemento.Detalle.Count > 0)
                        {
                            listado.Add(elemento);
                        }
                    }


#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(listado);
#endif
                    if (conteo)
                    {
                        return listado.Count;
                    }
                    {
                        return listado.OrderBy(x => x.OrdenRecepcion).ToList();
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

        public object ObtenerDetalleOrdeRecepcion(int folio, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ListadoGenerarOrdenRecepcion> listado = new List<ListadoGenerarOrdenRecepcion>();
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;
                    Boolean activarFolioConfiguracionOR = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionOrdenRecepcion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionOrdenRecepcion"].Equals("1") ? true : false) : false;

                    Sam3_OrdenRecepcion orden = ctx.Sam3_OrdenRecepcion.Where(x => x.Folio == folio).AsParallel().SingleOrDefault();

                    List<Sam3_FolioAvisoEntrada> registros = (from fe in ctx.Sam3_FolioAvisoEntrada
                                                              join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on fe.FolioAvisoEntradaID equals rfo.FolioAvisoEntradaID
                                                              join o in ctx.Sam3_OrdenRecepcion on rfo.OrdenRecepcionID equals o.OrdenRecepcionID
                                                              where fe.Activo && rfo.Activo && o.Activo
                                                              && o.OrdenRecepcionID == orden.OrdenRecepcionID
                                                              select fe).AsParallel().ToList();

                    foreach (Sam3_FolioAvisoEntrada r in registros)
                    {
                        ListadoGenerarOrdenRecepcion elemento = new ListadoGenerarOrdenRecepcion();
                        int folioAvisoEntradaID = r.FolioAvisoLlegadaID.Value;
                        elemento.AvisoEntradaID = r.FolioAvisoLlegadaID.ToString();

                        Sam3_FolioAvisoLlegada FolioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == r.FolioAvisoLlegadaID).FirstOrDefault();
                        elemento.FolioConfiguracion = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                   where pc.Entidad == FolioAvisoLlegada.Entidad && pc.Proyecto == FolioAvisoLlegada.ProyectoNombrado
                                                                                   select pc.PreFijoFolioAvisoLlegada + ","
                                                                                    + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                                    + FolioAvisoLlegada.Consecutivo.ToString() + ","
                                                                                    + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : r.FolioAvisoLlegadaID.ToString();


                        if (activarFolioConfiguracion)
                        {
                            string[] elemntos = elemento.FolioConfiguracion.Split(',').ToArray();
                            int digitos = Convert.ToInt32(elemntos[1]);
                            int consecutivo = Convert.ToInt32(elemntos[2]);
                            string formato = "D" + digitos.ToString();

                            elemento.FolioConfiguracion = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                        }

                        elemento.FolioConfiguracionOrdenRecepcion = activarFolioConfiguracionOR ?
                            orden.Rel_Proyecto_Entidad_Configuracion_ID != null ?
                            (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                             where pc.Rel_Proyecto_Entidad_Configuracion_ID == orden.Rel_Proyecto_Entidad_Configuracion_ID
                             select pc.PreFijoFolioOrdenRecepcion + ","
                             + pc.CantidadCerosFolioOrdenRecepcion.ToString() + ","
                             + orden.Consecutivo.ToString() + ","
                             + pc.PostFijoFolioOrdenRecepcion).FirstOrDefault() : orden.Folio.ToString() : orden.Folio.ToString();

                        if (activarFolioConfiguracionOR && orden.Rel_Proyecto_Entidad_Configuracion_ID != null)
                        {
                            string[] elemntos = elemento.FolioConfiguracionOrdenRecepcion.Split(',').ToArray();
                            int digitos = Convert.ToInt32(elemntos[1]);
                            int consecutivo = Convert.ToInt32(elemntos[2]);
                            string formato = "D" + digitos.ToString();

                            elemento.FolioConfiguracionOrdenRecepcion = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                        }

                        elemento.Tubos = (from o in ctx.Sam3_OrdenRecepcion
                                          join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on o.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                          join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                          join rfi in ctx.Sam3_Rel_OrdenRecepcion_ItemCode on rfo.OrdenRecepcionID equals rfi.OrdenRecepcionID
                                          join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                          join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                          join t in ctx.Sam3_TipoMaterial on it.TipoMaterialID equals t.TipoMaterialID
                                          join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                          join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                          where o.Activo && rfo.Activo && rfi.Activo && it.Activo && t.Activo && fe.Activo
                                          && o.OrdenRecepcionID == orden.OrdenRecepcionID
                                          && it.TipoMaterialID == 1
                                          && fe.FolioAvisoLlegadaID == folioAvisoEntradaID
                                          select new ElementoItemCodeGenerarOrden
                                          {
                                              ItemCodeID = rid.Rel_ItemCode_Diametro_ID.ToString(),
                                              Cantidad = rfi.Cantidad != null && rfi.Cantidad.Value > 0 ? rfi.Cantidad.Value.ToString() : "",
                                              Codigo = it.Codigo,
                                              D1 = d1.Valor.ToString(),
                                              D2 = d2.Valor.ToString(),
                                              Descripcion = it.DescripcionEspanol,
                                              TipoMaterial = t.Nombre
                                          }).AsParallel().ToList();

                        elemento.Accesorios = (from o in ctx.Sam3_OrdenRecepcion
                                               join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on o.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                               join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                               join rfi in ctx.Sam3_Rel_OrdenRecepcion_ItemCode on rfo.OrdenRecepcionID equals rfi.OrdenRecepcionID
                                               join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                               join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                               join t in ctx.Sam3_TipoMaterial on it.TipoMaterialID equals t.TipoMaterialID
                                               join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                               join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                               where o.Activo && rfo.Activo && rfi.Activo && it.Activo && t.Activo && fe.Activo
                                               && o.OrdenRecepcionID == orden.OrdenRecepcionID
                                               && it.TipoMaterialID == 2
                                               && fe.FolioAvisoLlegadaID == folioAvisoEntradaID
                                               select new ElementoItemCodeGenerarOrden
                                               {
                                                   ItemCodeID = rid.Rel_ItemCode_Diametro_ID.ToString(),
                                                   Cantidad = rfi.Cantidad != null && rfi.Cantidad.Value > 0 ? rfi.Cantidad.Value.ToString() : "",
                                                   Codigo = it.Codigo,
                                                   D1 = d1.Valor.ToString(),
                                                   D2 = d2.Valor.ToString(),
                                                   Descripcion = it.DescripcionEspanol,
                                                   TipoMaterial = t.Nombre
                                               }).AsParallel().ToList();

                        listado.Add(elemento);
                    }

#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(listado);
#endif

                    return listado.OrderBy(x => x.AvisoEntradaID).ToList();
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

        public object ObtenerInfo(int ordenRecepcion, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {


                    return null;
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


        public object GenerarOrdeRecepcion(List<ListaEnteros> RelItemCodesD, Sam3_Usuario usuario)
        {
            try
            {
                Sam3_OrdenRecepcion nuevaOrden = new Sam3_OrdenRecepcion();
                int consecutivo = 0;
                List<int> foliosEntrada = new List<int>();
                string ordenRecepcionFolio = "";
                Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionOrdenRecepcion"]) ?
                   (ConfigurationManager.AppSettings["ActivarFolioConfiguracionOrdenRecepcion"].Equals("1") ? true : false) : false;

                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {

                        List<int> tempIDs = RelItemCodesD.Where(x => x.ID != 0).Select(x => x.ID).ToList();
                        List<int> tempRFc = RelItemCodesD.Where(x => x.RelFCID != 0).Select(x => x.RelFCID).ToList();
                        List<int> tempRbd = RelItemCodesD.Where(x => x.RelBID != 0).Select(x => x.RelBID).ToList();
                        if (tempRFc.Count > 0)
                        {
                            foliosEntrada = (from i in ctx.Sam3_ItemCode
                                             join rid in ctx.Sam3_Rel_ItemCode_Diametro on i.ItemCodeID equals rid.ItemCodeID
                                             join fci in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on rid.Rel_ItemCode_Diametro_ID equals fci.Rel_ItemCode_Diametro_ID
                                             join fc in ctx.Sam3_FolioCuantificacion on fci.FolioCuantificacionID equals fc.FolioCuantificacionID
                                             join fe in ctx.Sam3_FolioAvisoEntrada on fc.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                             where i.Activo && fci.Activo && fc.Activo && fe.Activo
                                             && tempIDs.Contains(rid.Rel_ItemCode_Diametro_ID)
                                             && tempRFc.Contains(fci.Rel_FolioCuantificacion_ItemCode_ID)
                                             select fe.FolioAvisoEntradaID).AsParallel().Distinct().ToList();

                        }

                        if (tempRbd.Count > 0)
                        {
                            foliosEntrada.AddRange((from i in ctx.Sam3_ItemCode
                                                    join rid in ctx.Sam3_Rel_ItemCode_Diametro on i.ItemCodeID equals rid.ItemCodeID
                                                    join rbi in ctx.Sam3_Rel_Bulto_ItemCode on rid.Rel_ItemCode_Diametro_ID equals rbi.Rel_Bulto_ItemCode_ID
                                                    join b in ctx.Sam3_Bulto on rbi.BultoID equals b.BultoID
                                                    join fc in ctx.Sam3_FolioCuantificacion on b.FolioCuantificacionID equals fc.FolioCuantificacionID
                                                    join fe in ctx.Sam3_FolioAvisoEntrada on fc.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                                    where i.Activo && rbi.Activo && b.Activo && fc.Activo && fe.Activo
                                                    && tempIDs.Contains(rid.Rel_ItemCode_Diametro_ID)
                                                    && tempRbd.Contains(rbi.Rel_Bulto_ItemCode_ID)
                                                    select fe.FolioAvisoEntradaID).AsParallel().Distinct().ToList());
                        }

                        //retiramos los duplicados
                        foliosEntrada = foliosEntrada.GroupBy(x => x).Select(x => x.First()).ToList();

                        if (ctx.Sam3_OrdenRecepcion.Select(x => x.Folio).Any())
                        {
                            consecutivo = ctx.Sam3_OrdenRecepcion.Select(x => x.Folio).Max();
                            consecutivo = consecutivo > 0 ? consecutivo + 1 : 1;
                        }
                        else
                        {
                            consecutivo = 1;
                        }

                        //Generamos un nuevo registro en orden de recepcion
                        nuevaOrden.Activo = true;
                        nuevaOrden.FechaCreacion = DateTime.Now;
                        nuevaOrden.FechaModificacion = DateTime.Now;
                        nuevaOrden.Folio = consecutivo;
                        nuevaOrden.UsuarioModificacion = usuario.UsuarioID;

                        ctx.Sam3_OrdenRecepcion.Add(nuevaOrden);
                        ctx.SaveChanges();

                        List<int> avisos = (from fe in ctx.Sam3_FolioAvisoEntrada
                                            join fll in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals fll.FolioAvisoLlegadaID
                                            where foliosEntrada.Contains(fe.FolioAvisoEntradaID) && fe.Activo && fll.Activo
                                            select fll.ProyectoID).AsParallel().ToList();

                        //Obtenemos el proyecto con el id mas pequeno
                        Sam3_Rel_Proyecto_Entidad_Configuracion rel_proy = (from rel in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                            where rel.Proyecto == avisos.Min() && rel.Activo == 1
                                                                            select rel).AsParallel().SingleOrDefault();

                        int consecutivofolio = rel_proy.ConsecutivoFolioOrdenRecepcion;
                        Sam3_OrdenRecepcion orden = ctx.Sam3_OrdenRecepcion.Where(x => x.OrdenRecepcionID == nuevaOrden.OrdenRecepcionID && x.Activo).AsParallel().SingleOrDefault();
                        orden.Rel_Proyecto_Entidad_Configuracion_ID = rel_proy.Rel_Proyecto_Entidad_Configuracion_ID;
                        orden.Consecutivo = consecutivofolio;
                        ctx.SaveChanges();

                        rel_proy.ConsecutivoFolioOrdenRecepcion = consecutivofolio + 1;
                        ctx.SaveChanges();

                        if (activarFolioConfiguracion)
                        {
                            ordenRecepcionFolio = rel_proy.PreFijoFolioOrdenRecepcion + ","
                                + rel_proy.CantidadCerosFolioOrdenRecepcion.ToString() + ","
                                + rel_proy.ConsecutivoFolioOrdenRecepcion.ToString() + ","
                                + rel_proy.PostFijoFolioOrdenRecepcion;

                            string[] elemntos = ordenRecepcionFolio.Split(',').ToArray();
                            int digitos = Convert.ToInt32(elemntos[1]);
                            int cons = Convert.ToInt32(elemntos[2]);
                            string formato = "D" + digitos.ToString();

                            ordenRecepcionFolio = elemntos[0].Trim() + cons.ToString(formato).Trim() + elemntos[3].Trim();
                        }

                        //generamos la relacion con el folio aviso de entrada
                        foreach (int i in foliosEntrada)
                        {
                            Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion nuevaRelacion = new Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion();
                            nuevaRelacion.Activo = true;
                            nuevaRelacion.FechaModificacion = DateTime.Now;
                            nuevaRelacion.FolioAvisoEntradaID = i;
                            nuevaRelacion.OrdenRecepcionID = nuevaOrden.OrdenRecepcionID;
                            nuevaRelacion.UsuarioModificacion = usuario.UsuarioID;

                            ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion.Add(nuevaRelacion);
                        }

                        //generamos las relaciones para cada uno de los itemcodes
                        foreach (ListaEnteros i in RelItemCodesD)
                        {
                            Sam3_Rel_OrdenRecepcion_ItemCode nuevoOrdenItem = new Sam3_Rel_OrdenRecepcion_ItemCode();
                            nuevoOrdenItem.Activo = true;
                            nuevoOrdenItem.FechaModificacion = DateTime.Now;
                            nuevoOrdenItem.Rel_ItemCode_Diametro_ID = i.ID;
                            nuevoOrdenItem.OrdenRecepcionID = nuevaOrden.OrdenRecepcionID;
                            nuevoOrdenItem.UsuarioModificacion = usuario.UsuarioID;
                            nuevoOrdenItem.Cantidad = i.Cantidad;
                            ctx.Sam3_Rel_OrdenRecepcion_ItemCode.Add(nuevoOrdenItem);
                        }

                        ctx.SaveChanges();

                        ctx_tran.Commit();

                        DataTable dt = new DataTable();
                        dt = BackEndSAM.Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(RelItemCodesD);
                        if (((TransactionalInformation)GenerarNumerosUnicos(dt, orden.OrdenRecepcionID, usuario.UsuarioID)).ReturnMessage[0] != "Ok")
                            throw new Exception("Error al generar los numeros unicos.");
                       
                       
                        if (!(bool)EnviarAvisosBd.Instance.EnviarNotificación(1,
                            string.Format("Se generó una nueva orden de recepcion con folio: {0}",
                            nuevaOrden.OrdenRecepcionID), usuario))
                        {
                            //Agregar error a la bitacora  PENDIENTE
                        }
                       
                    } // fin transaccion
                } // fin samcontext

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Ok");
                result.ReturnMessage.Add(activarFolioConfiguracion ? ordenRecepcionFolio : nuevaOrden.Folio.ToString());
                result.ReturnMessage.Add(nuevaOrden.Folio.ToString());
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

        public object GenerarNumerosUnicos(DataTable Datos, int ordeRecepcionID, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros =
                    {
                        { "@OrdenRecepcionID", ordeRecepcionID.ToString() },
                        { "@usuario", UsuarioID.ToString() }
                    };
                    TransactionalInformation result = new TransactionalInformation();

                    if (_SQL.EjecutaDataAdapter(Stords.GuardarOrdenRecepcion, Datos, "@TablaItemCodes", Parametros).Rows[0][0].ToString() == "ok")
                    {
                        
                        result.ReturnMessage.Add("Ok");
                        result.ReturnCode = 200;
                        result.ReturnStatus = false;
                        result.IsAuthenicated = true;
                    }
                    else
                    {
                       
                        result.ReturnMessage.Add("Error");
                        result.ReturnCode = 500;
                        result.ReturnStatus = false;
                        result.IsAuthenicated = true;
                     
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                LoggerBd.Instance.EscribirLog(ex);
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result; ;
            }
        }
        public List<ListaEnteros> ObtenerItemCodesPorFolioEntrada(List<int> folios)
        {
            try
            {
                List<ListaEnteros> items = new List<ListaEnteros>();
                using (SamContext ctx = new SamContext())
                {
                    foreach (int f in folios)
                    {

                        List<ListaEnteros> tubosRFC = new List<ListaEnteros>();
                        List<ListaEnteros> tubosRB = new List<ListaEnteros>();
                        List<ListaEnteros> AccesoriosRFC = new List<ListaEnteros>();
                        List<ListaEnteros> AccesoriosRB = new List<ListaEnteros>();

                        tubosRFC = (from r in ctx.Sam3_FolioAvisoEntrada
                                    join c in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals c.FolioAvisoEntradaID
                                    join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on r.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                    join rfi in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on c.FolioCuantificacionID equals rfi.FolioCuantificacionID
                                    join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                    join i in ctx.Sam3_ItemCode on rid.ItemCodeID equals i.ItemCodeID
                                    join t in ctx.Sam3_TipoMaterial on i.TipoMaterialID equals t.TipoMaterialID
                                    join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                    join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                    join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                    join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                    where r.Activo && c.Activo && rfi.Activo && i.Activo && t.Activo && rics.Activo && ics.Activo
                                    && i.TipoMaterialID == 1
                                    && r.FolioAvisoLlegadaID == f
                                    && !rfi.TieneNumerosUnicos
                                    && rfi.Cantidad > 0
                                    && !(from co in ctx.Sam3_Colada
                                         where co.Activo && co.NumeroColada == ""
                                         && co.ProyectoID == i.ProyectoID
                                         select co.ColadaID).Contains(rfi.ColadaID)
                                    && rfi.ColadaID > 0
                                    && rfi.MM > 0
                                    select new ListaEnteros
                                    {
                                        ID = rfi.Rel_ItemCode_Diametro_ID.Value,
                                        Cantidad = rfi.Cantidad.Value,
                                        RelFCID = rfi.Rel_FolioCuantificacion_ItemCode_ID
                                    }).AsParallel().Distinct().ToList();



                        tubosRB = (from r in ctx.Sam3_FolioAvisoEntrada
                                   join c in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals c.FolioAvisoEntradaID
                                   join b in ctx.Sam3_Bulto on c.FolioCuantificacionID equals b.FolioCuantificacionID
                                   join rbi in ctx.Sam3_Rel_Bulto_ItemCode on b.BultoID equals rbi.BultoID
                                   join rid in ctx.Sam3_Rel_ItemCode_Diametro on rbi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                   join i in ctx.Sam3_ItemCode on rid.ItemCodeID equals i.ItemCodeID
                                   join t in ctx.Sam3_TipoMaterial on i.TipoMaterialID equals t.TipoMaterialID
                                   join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                   join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                   join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                   join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                   where r.Activo && c.Activo && b.Activo && rbi.Activo && i.Activo && t.Activo && rics.Activo && ics.Activo
                                   && i.TipoMaterialID == 1
                                   && r.FolioAvisoLlegadaID == f
                                   && !rbi.TieneNumerosUnicos
                                   && rbi.Cantidad > 0
                                   && !(from co in ctx.Sam3_Colada
                                        where co.Activo && co.NumeroColada == ""
                                        && co.ProyectoID == i.ProyectoID
                                        select co.ColadaID).Contains(rbi.ColadaID)
                                   && rbi.ColadaID > 0
                                   && rbi.MM > 0
                                   select new ListaEnteros
                                   {
                                       ID = rbi.Rel_ItemCode_Diametro_ID.Value,
                                       Cantidad = rbi.Cantidad.Value,
                                       RelBID = rbi.Rel_Bulto_ItemCode_ID
                                   }).AsParallel().Distinct().ToList();

                        AccesoriosRFC = (from r in ctx.Sam3_FolioAvisoEntrada
                                         join c in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals c.FolioAvisoEntradaID
                                         join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on r.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                         join rfi in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on c.FolioCuantificacionID equals rfi.FolioCuantificacionID
                                         join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                         join i in ctx.Sam3_ItemCode on rid.ItemCodeID equals i.ItemCodeID
                                         join t in ctx.Sam3_TipoMaterial on i.TipoMaterialID equals t.TipoMaterialID
                                         join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                         join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                         join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                         join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                         where r.Activo && c.Activo && rfp.Activo && rfi.Activo && i.Activo && t.Activo && rics.Activo && ics.Activo
                                         && i.TipoMaterialID == 2
                                         && r.FolioAvisoLlegadaID == f
                                         && !rfi.TieneNumerosUnicos
                                         && rfi.Cantidad > 0
                                         && !(from co in ctx.Sam3_Colada
                                              where co.Activo && co.NumeroColada == ""
                                              && co.ProyectoID == i.ProyectoID
                                              select co.ColadaID).Contains(rfi.ColadaID)
                                         && rfi.ColadaID > 0
                                         select new ListaEnteros
                                         {
                                             ID = rfi.Rel_ItemCode_Diametro_ID.Value,
                                             Cantidad = rfi.Cantidad.Value,
                                             RelFCID = rfi.Rel_FolioCuantificacion_ItemCode_ID
                                         }).AsParallel().Distinct().ToList();



                        AccesoriosRB = (from r in ctx.Sam3_FolioAvisoEntrada
                                        join c in ctx.Sam3_FolioCuantificacion on r.FolioAvisoEntradaID equals c.FolioAvisoEntradaID
                                        join b in ctx.Sam3_Bulto on c.FolioCuantificacionID equals b.FolioCuantificacionID
                                        join rbi in ctx.Sam3_Rel_Bulto_ItemCode on b.BultoID equals rbi.BultoID
                                        join rid in ctx.Sam3_Rel_ItemCode_Diametro on rbi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                        join i in ctx.Sam3_ItemCode on rid.ItemCodeID equals i.ItemCodeID
                                        join t in ctx.Sam3_TipoMaterial on i.TipoMaterialID equals t.TipoMaterialID
                                        join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                        join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                        join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                        join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                        where r.Activo && c.Activo && b.Activo && rbi.Activo && i.Activo && t.Activo && rics.Activo && ics.Activo
                                        && i.TipoMaterialID == 2
                                        && r.FolioAvisoLlegadaID == f
                                        && !rbi.TieneNumerosUnicos
                                        && rbi.Cantidad > 0
                                        && !(from co in ctx.Sam3_Colada
                                             where co.Activo && co.NumeroColada == ""
                                             && co.ProyectoID == i.ProyectoID
                                             select co.ColadaID).Contains(rbi.ColadaID)
                                        && rbi.ColadaID > 0
                                        select new ListaEnteros
                                        {
                                            ID = rbi.Rel_ItemCode_Diametro_ID.Value,
                                            Cantidad = rbi.Cantidad.Value,
                                            RelBID = rbi.Rel_Bulto_ItemCode_ID
                                        }).AsParallel().Distinct().ToList();

                        //Eliminar los elementos repetidos
                        tubosRB = tubosRB.GroupBy(x => x.RelBID).Select(x => x.First()).ToList();
                        tubosRFC = tubosRFC.GroupBy(x => x.RelFCID).Select(x => x.First()).ToList();
                        AccesoriosRB = AccesoriosRB.GroupBy(x => x.RelBID).Select(x => x.First()).ToList();
                        AccesoriosRFC = AccesoriosRFC.GroupBy(x => x.RelFCID).Select(x => x.First()).ToList();

                        //Eliminar los que tengan cantidad 0
                        tubosRB = tubosRB.Where(x => x.Cantidad > 0).ToList();
                        tubosRFC = tubosRFC.Where(x => x.Cantidad > 0).ToList();
                        AccesoriosRB = AccesoriosRB.Where(x => x.Cantidad > 0).ToList();
                        AccesoriosRFC = AccesoriosRFC.Where(x => x.Cantidad > 0).ToList();

                        items.AddRange(tubosRB);
                        items.AddRange(tubosRFC);
                        items.AddRange(AccesoriosRB);
                        items.AddRange(AccesoriosRFC);

                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return null;
            }
        }

        public object GenerarOrdeRecepcion(FoliosItems listados, Sam3_Usuario usuario)
        {
            try
            {
                List<ListaEnteros> ids = new List<ListaEnteros>();
                ids.AddRange(listados.Items);
                if (listados.Folios.Count > 0)
                {
                    ids.AddRange(ObtenerItemCodesPorFolioEntrada(listados.Folios.Select(x => x.ID).ToList()));
                }

                if ((bool)ValidaColadas(ids))
                {
                    return GenerarOrdeRecepcion(ids, usuario);
                }
                else
                {
                    throw new Exception("Alguno de los ItemCodes seleccionados no cuenta con colada");
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


        public object EliminarOrdenRecepcion(int OrdenRecepcionID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_OrdenRecepcion orden = ctx.Sam3_OrdenRecepcion.Where(x => x.OrdenRecepcionID == OrdenRecepcionID).AsParallel().SingleOrDefault();
                    orden.Activo = false;
                    orden.FechaModificacion = DateTime.Now;
                    orden.UsuarioModificacion = usuario.UsuarioID;

                    List<Sam3_Rel_OrdenRecepcion_ItemCode> relacion = ctx.Sam3_Rel_OrdenRecepcion_ItemCode.Where(x => x.OrdenRecepcionID == OrdenRecepcionID)
                        .AsParallel().ToList();

                    foreach (Sam3_Rel_OrdenRecepcion_ItemCode r in relacion)
                    {
                        r.Activo = false;
                        r.FechaModificacion = DateTime.Now;
                        r.UsuarioModificacion = usuario.UsuarioID;
                    }

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
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



        public object EliminarItemCodeOrdenRecepcion(string itemcode, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int itemcodeid = Convert.ToInt32(itemcode);

                    Sam3_Rel_OrdenRecepcion_ItemCode RelOrdenRecepcion = ctx.Sam3_Rel_OrdenRecepcion_ItemCode
                        .Where(x => x.Rel_ItemCode_Diametro_ID == itemcodeid)
                        .AsParallel().SingleOrDefault();


                    RelOrdenRecepcion.Activo = false;
                    RelOrdenRecepcion.UsuarioModificacion = usuario.UsuarioID;
                    RelOrdenRecepcion.FechaModificacion = DateTime.Now;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
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

        public List<ListadoIncidencias> ListadoIncidencias(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> Ids)
        {
            try
            {
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_OrdenRecepcion> registros = new List<Sam3_OrdenRecepcion>();
                    Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                    if (proyectoID > 0)
                    {
                        registros = (from o in ctx.Sam3_OrdenRecepcion
                                     join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on o.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                     join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where o.Activo && rfo.Activo && fe.Activo && rfp.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && Ids.Contains(o.OrdenRecepcionID)
                                     select o).AsParallel().Distinct().ToList();
                    }
                    else
                    {
                        registros = (from o in ctx.Sam3_OrdenRecepcion
                                     join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on o.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                     join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where o.Activo && rfo.Activo && fe.Activo && rfp.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && Ids.Contains(o.OrdenRecepcionID)
                                     select o).AsParallel().Distinct().ToList();
                    }

                    if (clienteID > 0)
                    {
                        int sam3Cliente = (from c in ctx.Sam3_Cliente
                                           where c.Activo && c.Sam2ClienteID == clienteID
                                           select c.ClienteID).AsParallel().SingleOrDefault();
                        registros = (from r in registros
                                     join rfo in ctx.Sam3_Rel_FolioAvisoEntrada_OrdenRecepcion on r.OrdenRecepcionID equals rfo.OrdenRecepcionID
                                     join fe in ctx.Sam3_FolioAvisoEntrada on rfo.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                     where rfo.Activo && fe.Activo
                                     && fe.ClienteID == sam3Cliente
                                     select r).AsParallel().Distinct().ToList();
                    }

                    listado = (from r in registros
                               join rio in ctx.Sam3_Rel_Incidencia_OrdenRecepcion on r.OrdenRecepcionID equals rio.OrdenRecepcionID
                               join inc in ctx.Sam3_Incidencia on rio.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where rio.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
                                   Clasificacion = c.Nombre,
                                   Estatus = inc.Estatus,
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo
                                                    && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   TipoIncidencia = tpi.Nombre,
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == inc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + inc.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : inc.IncidenciaID.ToString()
                               }).AsParallel().Distinct().ToList();

                    if (ActivarFolioConfiguracionIncidencias)
                    {
                        foreach (ListadoIncidencias item in listado)
                        {
                            if (!string.IsNullOrEmpty(item.FolioConfiguracionIncidencia))
                            {
                                string[] elemntos = item.FolioConfiguracionIncidencia.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                item.FolioConfiguracionIncidencia = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }
                            else
                            {
                                item.FolioConfiguracionIncidencia = item.FolioIncidenciaID.ToString();
                            }
                        }
                    }


                }
                return listado;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return null;
            }
        }

        private object ValidaColadas(List<ListaEnteros> ids)
        {
            bool coladasFaltantes = false;
            List<int> tempfc = new List<int>();
            List<int> tempBic = new List<int>();

            tempfc = ids.Where(x => x.RelFCID > 0).Select(x => x.RelFCID).ToList();
            tempBic = ids.Where(x => x.RelBID > 0).Select(x => x.RelBID).ToList();
            using (SamContext ctx = new SamContext())
            {
                if (tempfc.Count > 0)
                {
                    coladasFaltantes = (from fc in ctx.Sam3_Rel_FolioCuantificacion_ItemCode
                                        where tempfc.Contains(fc.Rel_FolioCuantificacion_ItemCode_ID)
                                        && (fc.ColadaID == 0 || fc.ColadaID == null)
                                        select fc).Any();
                    if (tempBic.Count > 0 && !coladasFaltantes)
                    {
                        coladasFaltantes = (from bic in ctx.Sam3_Rel_Bulto_ItemCode
                                            where tempBic.Contains(bic.Rel_Bulto_ItemCode_ID)
                                            && (bic.ColadaID == 0 || bic.ColadaID == null)
                                            select bic).Any();
                    }
                }
            }
            return coladasFaltantes ? false : true;
        }
    }
}