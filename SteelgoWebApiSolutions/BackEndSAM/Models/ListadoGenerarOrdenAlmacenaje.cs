using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadoDetalleOrdenAlmacenaje
    {
        public int ProyectoID { get; set; }
        public bool Activo { get; set; }
        public List<ListadoGenerarOrdenAlmacenaje> ListadoGenerarOrdenAlmacenaje { get; set; }
        public string FolioConfiguracionOrdenAlmacenaje { get; set; }
    }
    public class ListadoGenerarOrdenAlmacenaje
    {
            public string FolioCuantificacion { get; set; }
            public string FolioConfiguracionCuantificacion { get; set; }
            public List<ElementoCuantificacionItemCode> ItemCodes { get; set; }

            public ListadoGenerarOrdenAlmacenaje()
            {
                ItemCodes = new List<ElementoCuantificacionItemCode>();
            }
    }

        public class ElementoCuantificacionItemCode
        {
            public string FolioCuantificacion { get; set; }
            public string ItemCodeID { get; set; }
            public string Codigo { get; set; }
            public string Descripcion { get; set; }
            public string D1 { get; set; }
            public string D2 { get; set; }
            public string Cantidad { get; set; }
            public string ProyectoID { get; set; }
            public string Proyecto { get; set; }
            public List<ElementoNumeroUnico> NumerosUnicos { get; set; }

            public ElementoCuantificacionItemCode()
            {
                NumerosUnicos = new List<ElementoNumeroUnico>();
            }
        }

        public class ElementoNumeroUnico
        {
            public string FolioCuantificacion { get; set; }
            public string ItemCodeID { get; set; }
            public string NumeroUnicoID { get; set; }
            public string NumeroUnico { get; set; }
        }

    public class ListadosFolios
    {
        public List<ListaIDS> listaFoliosCuantificacion { get; set; }
        public List<ListaIDS> listaItemCodes { get; set; }
        public List<ListaIDS> listaNumerosUnicos { get; set; }

        public ListadosFolios()
        {
            listaFoliosCuantificacion = new List<ListaIDS>();
            listaItemCodes = new List<ListaIDS>();
            listaNumerosUnicos = new List<ListaIDS>();
        }
    }

    public class ListaIDS
    {
        public int ID { get; set; }
    }
}