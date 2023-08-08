using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Server.Web.Api.Controllers
{
    public class ExceptionController : Controller
    {
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        
        public IActionResult HandleError() =>
        Problem("Please Contact your Admin.");
    }
}
