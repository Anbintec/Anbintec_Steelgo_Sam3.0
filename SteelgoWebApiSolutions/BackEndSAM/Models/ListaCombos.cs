using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListaCombos
    {
        public string id { get; set; }
        public string value { get; set; }
    }

    public class ListaCombosICC
    {
        public string id { get; set; }
        public string value { get; set; }
        public int tipoGrupo { get; set; }
    }

    public class ListaEnteros
    {
        public int ID { get; set; }
        public int RelFCID { get; set; }
        public int RelBID { get; set; }
        public int Cantidad { get; set; }
    }

    public class FoliosItems
    {
        public List<ListaEnteros> Folios { get; set; }
        public List<ListaEnteros> Items { get; set; }

        public FoliosItems()
        {
            Folios = new List<ListaEnteros>();
            Items = new List<ListaEnteros>();
        }
    }

    public class ListaDiametros
    {
        public int id { get; set; }
        public decimal value { get; set; }
    }

    public class ListaComboItemCodeEdicion
    {
        public string id { get; set; }
        public string value { get; set; }
        public string Descripcion { get; set; }
    }
}