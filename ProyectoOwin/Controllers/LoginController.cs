using Microsoft.Owin.Security;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using ProyectoOwin.Models;

namespace ProyectoOwin.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Index(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            var authService = new AdAuthenticationService(authenticationManager);
            var authenticationResult = authService.SignIn(model.Username, model.Password);
            if (authenticationResult.IsSuccess)
            {
                return RedirectToAction("Index", "Ejemplo");
            }
            ModelState.AddModelError("errorMsg", authenticationResult.ErrorMessage);
            return View(model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Logoff()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(MyAuthentication.ApplicationCookie);

            return RedirectToAction("Index", "Login");
        }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Usuario Requerido")]
        [AllowHtml]
        public string Username { get; set; }

        [Required(ErrorMessage = "Contraseña Requerida")]
        [AllowHtml]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}