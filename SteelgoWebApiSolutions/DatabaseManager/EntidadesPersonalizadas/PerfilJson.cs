using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.EntidadesPersonalizadas
{
    public class PerfilJson
    {
        public Layout layout { get; set; }
        public List<Entidad> entidades { get; set; }
        public string token { get; set; }
        public PerfilJson()
        {
            layout = new Layout();
            entidades = new List<Entidad>();
        }
    }

    public class Layout
    {
        public bool create { get; set; }
        public bool list { get; set; }
        public bool detail { get; set; }
        public bool destroy { get; set; }
        public bool createIncidence { get; set; }
        public bool solutionincidence { get; set; }

        public List<object> properties { get; set; }
        public List<object> navigation { get; set; }
        public Layout()
        {
            properties = new List<object>();
            navigation = new List<object>();
        }
    }

    public class Properties 
    {
        public string propertyName { get; set; }
        public bool visible { get; set; }
        public bool editable { get; set; }
        public bool required { get; set; }
    }

    public class Entidad
    {
        public string entityName { get; set; }
        public bool create { get; set; }
        public bool list { get; set; }
        public bool detail { get; set; }
        public bool destroy { get; set; }
        public bool createIncidence { get; set; }
        public bool solutionincidence { get; set; }

        public List<Properties> properties { get; set; }

        public Entidad()
        {
            properties = new List<Properties>();
        }
    }

    public class sideMenu
    {
        public string type { get; set; }
        public bool visible { get; set; }
        public bool editable { get; set; }
        public List<SideMenuElement> elements { get; set; }
        public sideMenu()
        {
            elements = new List<SideMenuElement>();
        }
    }

    public class contextMenu
    {
        public string type { get; set; }
        public bool visible { get; set; }
        public bool editable { get; set; }
        public List<ContextMenuElement> elements { get; set; }
        public contextMenu()
        {
            elements = new List<ContextMenuElement>();
        }
    }

    public class quicklinks
    {
        public string type { get; set; }
        public bool visible { get; set; }
        public bool editable { get; set; }
        public List<QuickLinksElement> elements { get; set; }
        public quicklinks()
        {
            elements = new List<QuickLinksElement>();
        }
    }

    public class QuickLinksElement
    {
        public string texto { get; set; }
        public string liga { get; set; }
    }

    public class SideMenuElement
    {
        public int elemetId { get; set; }
        public int idPadre { get; set; }
        public string texto { get; set; }
        public string liga { get; set; }
        public string icono { get; set; }
        public int nivel { get; set; }
        public int acomodo { get; set; }
    }

    public class ContextMenuElement
    {
        public string texto { get; set; }
        public string liga { get; set; }
    }

}
