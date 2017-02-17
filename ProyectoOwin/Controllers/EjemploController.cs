using ProyectoOwin.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ProyectoOwin.Controllers
{
    public class EjemploController : Controller
    {
        private readonly List<Ejemplo> ejemplos = new List<Ejemplo>()
    {
        new Ejemplo { Propiedad1 = 1, Propiedad2 = "Nicolás Avellaneda", Propiedad3 = 1874 },
        new Ejemplo { Propiedad1 = 2, Propiedad2 = "Julio Argentino Roca", Propiedad3 = 1880 },
        new Ejemplo { Propiedad1 = 3, Propiedad2 = "Carlos Pellegrini", Propiedad3 = 1890 }        
    };

        [Authorize(Roles = "Users")]
        public ActionResult Index()
        {
            return View(ejemplos);
        }
    }
}