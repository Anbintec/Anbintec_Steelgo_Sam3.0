using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using SecurityManager.TokenHandler;
using SecurityManager.Api.Models;
using BackEndSAM.Models;
using BackEndSAM.DataAcces;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ListadoController : ApiController
    {
        /// <summary>
        /// Metodo comun para listados simples, para combos, o grids
        /// </summary>
        /// <param name="tipoListado">Tipo de listado a devolver</param>
        /// <param name="token">credenciales del usuario</param>
        /// <param name="parametroBusqueda">cadena de busqueda</param>
        /// <returns>Devuelve el listado aporpiado de acuerdo al tipo solicitado</returns>
        public object Get(int tipoListado, string token, string idioma = "", int paginaID = 0, string parametroBusqueda = "")
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                switch (tipoListado)
                {
                    //case 1: //Folios aviso llegada
                    //    return AvisoLlegadaBd.Instance.ObtenerListadoFoliosParaFiltro(new Sam3_Usuario());
                    case 2: // Folios de aviso de llegada con permiso de aduana autorizados
                        return AvisoLlegadaBd.Instance.ObtenerListadoFoliosRequierePermiso(new Sam3_Usuario());
                    case 3: // listado de choferes por transportista
                        return ChoferBd.Instance.ObtenerChoferesProTransportista(Convert.ToInt32(parametroBusqueda), paginaID, idioma, usuario);
                    case 4: //Obtener cantidades para dashboard
                        TransactionalInformation rest = new TransactionalInformation();
                        rest.ReturnMessage.Add("El listado de cantidades de Dashboard requiere de parametros de filtrado");
                        rest.ReturnCode = 500;
                        rest.ReturnStatus = false;
                        rest.IsAuthenicated = false;
                        return rest;
                    //case 5:
                    //    return AvisoLlegadaBd.Instance.ObtenerFoliosAvisoLlegadaSinEntrada();
                    //case 6: //Obtener listado de folios que ya tienen llegada de material
                    //    return AvisoLlegadaBd.Instance.ObtenerListadoSinPaseSalida();
                    case 18: // Listado para combo de packing list
                        return ListadoBd.Instance.PackingListsParaComboFiltros(usuario);
                    default:
                        TransactionalInformation result = new TransactionalInformation();
                        result.ReturnMessage.Add("Listado no encontrado");
                        result.ReturnCode = 500;
                        result.ReturnStatus = false;
                        result.IsAuthenicated = false;
                        return result;
                }

            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }

        /// <summary>
        /// Metodo comun de listados simples o datos para grids
        /// </summary>
        /// <param name="data">objeto json en formato de cadena con las propiedades de filtrado</param>
        /// <returns>Devuelve el listado de acuerdo a la propiedad TipoListadoID enviada en el objeto json de entrada</returns>
        public object Get(string data)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            FiltrosJson filtros = serializer.Deserialize<FiltrosJson>(data);
            int tipoListado = Convert.ToInt32(filtros.TipoListado);
            string parametroBusqueda = filtros.ParametroBusqueda;
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(filtros.token, out payload, out newToken);
            if (tokenValido)
            {
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                switch (tipoListado)
                {
                    case 1: //Folios aviso llegada
                        return AvisoLlegadaBd.Instance.ObtenerListadoFoliosParaFiltro(usuario);
                    case 2: // Folios de aviso de llegada con permiso de aduana autorizados
                        return AvisoLlegadaBd.Instance.ObtenerListadoFoliosRequierePermiso(usuario);
                    case 3: // listado de choferes por transportista
                        int temp = filtros.PaginaID != null && filtros.PaginaID != "" ? Convert.ToInt32(filtros.PaginaID) : 0;
                        return ChoferBd.Instance.ObtenerChoferesProTransportista(Convert.ToInt32(parametroBusqueda), temp, filtros.Idioma, usuario);
                    case 4: //Obtener cantidades para dashboard
                        return ListadoBd.Instance.ObtenerCantidadesDashboard(filtros, usuario);
                    case 5: //Obtener cantidades de Dashboard para aviso de entrada
                        return ListadoBd.Instance.ObtenerCantidadesDashboardAvisoEntrada(filtros, usuario);
                    case 7: //Obtener Cantidades dashboard de cuantificacion
                        return ListadoBd.Instance.ObtenerCantidadesDashboardCuantificacion(filtros, usuario);
                    case 8: //Obtener listado para generar ordenes de recepcion
                        return OrdenRecepcionBd.Instance.ObtenerListadoGenerarOrdenRecepcion(filtros, usuario);
                    case 9: //Obtener lista combo de folios de entrada por proyecto, que ya tengan cuantificacion
                        int proyectoID = filtros.ProyectoID != "" ? Convert.ToInt32(filtros.ProyectoID) : 0;
                        return ListadoBd.Instance.ObtenerFoliosEntradaPorProyecto(proyectoID, usuario);
                    case 10: //Obtener lista de combo de itemCodes por folio aviso de llegada
                        int folioAvisoLlegada = filtros.FolioAvisoLlegadaID != "" ? Convert.ToInt32(filtros.FolioAvisoLlegadaID) : 0;
                        return ListadoBd.Instance.ObtenerItemCodesPorFolioLlegada(folioAvisoLlegada, usuario);
                    case 11: //Entradas por cuantificar
                        return ListadoBd.Instance.ListadoMaterialesSinCuantificar(filtros, usuario);
                    case 12: // Packing list por cuantificar
                        return ListadoBd.Instance.ListadoPacknglistPorCuantificar(filtros, usuario);
                    case 13: // Materiales sin itemcode steelgo
                        return ListadoBd.Instance.ListadoMTLSinICS(filtros, usuario);
                    case 14: // Numeros unicos sin complemento de recepcion
                        return ListadoBd.Instance.ListadoNUConRecepcionSinComplemento(filtros, usuario);
                    case 15: // numeros unicos sin rack
                        return ListadoBd.Instance.ListadoNUSinAlmacenaje(filtros, usuario);
                    case 16:// Listado de incidencias activas para el dashboard de cuantificacion
                        return ListadoBd.Instance.ListadoIncidenciasActivas(filtros, usuario);
                    case 17: // Listado de packing list
                        return ListadoBd.Instance.ListadoPackingList(filtros, usuario);
                    case 18: // Listado para combo de packing list
                        return ListadoBd.Instance.PackingListsParaComboFiltros(usuario);
                        //Orden Almacenaje
                    case 19: //Obtener lista de folios cuantificacion
                         int proyectoAlmacenajeID = filtros.ProyectoID != "" ? Convert.ToInt32(filtros.ProyectoID) : 0;
                         return OrdenAlmacenajeBd.Instance.ObtenerFoliosCuantificacionOrdenAlmacenaje(proyectoAlmacenajeID, usuario);
                    case 20: //Obtener ItemCodes con Orden de Recepcion
                         int folioCuantificacionID = filtros.FolioCuantificacionID != "" ? Convert.ToInt32(filtros.FolioCuantificacionID) : 0;
                         return OrdenAlmacenajeBd.Instance.ObtenerItemCodesOrdenAlmacenaje(folioCuantificacionID, usuario);
                    case 21: //Obtener Numeros Unicos sin Orden de Almacenaje
                         int itemCodeID = filtros.ItemCodeID != "" ? Convert.ToInt32(filtros.ItemCodeID) : 0;
                         int folioCID = filtros.FolioCuantificacionID != "" ? Convert.ToInt32(filtros.FolioCuantificacionID) : 0;
                         return OrdenAlmacenajeBd.Instance.ObtenerNumerosUnicosOrdenAlmacenaje(itemCodeID, usuario, folioCID);
                    case 22: //Obtener los patios del usuario
                         return ListadoMaterialesBd.Instance.obtenerPatioListadoMateriales(usuario);
                    case 23: //Obtener los proyectos segun el patio seleccionado
                         return ListadoMaterialesBd.Instance.obtenerProyectoListadoMateriales(filtros.PatioID, usuario);
                    case 24: //Obtener los folios cuantificacion segun el folio de llegada seleccionado
                         return ListadoMaterialesBd.Instance.obtenerFolioCuantificacionListadoMateriales(filtros.FolioAvisoLlegadaID, filtros.ProyectoID);
                    case 25: //listado ODTS y ODTS Activas
                         return ListadoBd.Instance.ListadoOrdenesDeTrabajo(filtros, usuario); 
                    case 26: // Conteos de dashboard de Despachos
                         return ListadoBd.Instance.ConteoDashBoardDespachos(filtros, usuario);
                    case 27: // listado Incidencias
                         return ListadoBd.Instance.ListadoIncidencias(filtros, usuario);
                    case 28://Pre-Despachar por unidad de medida
                         return ListadoBd.Instance.ListadoPreDespacho(filtros, usuario);
                    case 29://Por Despachar por unidad de medida
                         return ListadoBd.Instance.ListadoPorDespachar(filtros, usuario);
                    case 30://Entregar por unidad de medida
                         return ListadoBd.Instance.ListadoPorEntregar(filtros, usuario);
                    case 31://Traveler Pendiente por unidad de medida
                         return ListadoBd.Instance.ListadoTravelerPendiente(filtros, usuario);
                    case 32: //Carga inicial de complemento de recepcion desde dashboard
                         return ComplementoRecepcionBd.Instance.ObtenerTodoPorOrdenRecepcionID(filtros.OrdenRecepcionID, usuario);
                    case 33:
                         return AvisoLlegadaBd.Instance.ObtenerFoliosAvisoLlegadaSinEntrada(usuario);
                    case 34:
                         return AvisoLlegadaBd.Instance.ObtenerListadoSinPaseSalida(usuario);
                    default:
                        TransactionalInformation result = new TransactionalInformation();
                        result.ReturnMessage.Add("Listado no encontrado");
                        result.ReturnCode = 500;
                        result.ReturnStatus = false;
                        result.IsAuthenicated = false;
                        return result;
                }
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }
    }
}