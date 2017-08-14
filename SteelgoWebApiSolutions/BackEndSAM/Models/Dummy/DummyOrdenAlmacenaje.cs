using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class OrdenAlmacenajeJson
    {
        public string FechaOrdenAlmacenaje { get; set; }
        public string OrdenAlmacenaje { get; set; }
        public string OrdenAlmacenajeID { get; set; }
        public List<PackingListCuantificacion> FolioCuantificacion { get; set; }

        public OrdenAlmacenajeJson()
        {
            FolioCuantificacion = new List<PackingListCuantificacion>();
        }
    }

    public class PackingListCuantificacion
    {
        public string OrdenAlmacenaje { get; set; }
        public string FolioCuantificacion { get; set; }
        public int FolioCuantificacionID { get; set; }
        public List<ElementoItemCodeGenerarOrdenAlmacenaje> ItemCodes { get; set; }

        public PackingListCuantificacion()
        {
            ItemCodes = new List<ElementoItemCodeGenerarOrdenAlmacenaje>();
        }
       
    }
    public class ElementoItemCodeGenerarOrdenAlmacenaje
    {
        public string OrdenAlmacenaje { get; set; }
        public string ItemCodeID { get; set; }
        public string Codigo { get; set; }
        public string NumeroUnico { get; set; }
        public string NumeroUnicoID { get; set; }
    }



    //public class GenerarOrdenAlmacenajeJson 
    //{
    //    public string FolioCuantificacion { get; set; }
    //    public List<ElementoCuantificacionItemCode> ItemCodes { get; set; }

    //    public GenerarOrdenAlmacenajeJson()
    //    {
    //        ItemCodes = new List<ElementoCuantificacionItemCode>();
    //    }
    //}

    //public class ElementoCuantificacionItemCode
    //{
    //    public string FolioCuantificacion { get; set; }
    //    public string ItemCodeID { get; set; }
    //    public string Codigo { get; set; }
    //    public string Descripcion { get; set; }
    //    public string D1 { get; set; }
    //    public string D2 { get; set; }
    //    public string Cantidad { get; set; }
    //    public List<ElementoNumeroUnico> NumerosUnicos { get; set; }

    //    public ElementoCuantificacionItemCode()
    //    {
    //        NumerosUnicos = new List<ElementoNumeroUnico>();
    //    }
    //}

    //public class ElementoNumeroUnico
    //{
    //    public string FolioCuantificacion { get; set; }
    //    public string ItemCodeID { get; set; }
    //    public string NumeroUnicoID { get; set; }
    //    public string NumeroUnico { get; set; }
    //}
}