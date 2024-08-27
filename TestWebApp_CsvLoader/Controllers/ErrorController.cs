using Microsoft.AspNetCore.Mvc;

namespace EmployeeImportApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error404()
        {
            return View("~/Views/Shared/404.cshtml");
        }

        public IActionResult Error500()
        {
            return View("~/Views/Shared/500.cshtml");
        }

        public IActionResult Error403()
        {
            return View("~/Views/Shared/403.cshtml");
        }

        public IActionResult Error(int statusCode)
        {
            return View("~/Views/Shared/Error.cshtml", statusCode);
        }
    }
}
