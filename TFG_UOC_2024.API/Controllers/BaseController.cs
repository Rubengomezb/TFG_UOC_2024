using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static TFG_UOC_2024.DB.Components.Enums;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;

namespace TFG_UOC_2024.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public const string AdministratorUser = "ADMINISTRATOR";

        // define db context
        protected UserManager<ApplicationUser> userManager;
        protected ApplicationContext db { get; set; }
        protected IMapper mapper { get; set; }
        protected Microsoft.Extensions.Logging.ILogger log;
        protected IConfiguration config;

        public BaseController(UserManager<ApplicationUser> u, ApplicationContext dbContext, IMapper m, Microsoft.Extensions.Logging.ILogger logger = null, IConfiguration configuration = null)
        {
            this.userManager = u;
            this.db = dbContext;
            this.mapper = m;
            this.log = logger;
            this.config = configuration;
        }

        /// <summary>
        /// Create the ActionResult response based on the underlying response from the service layer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        /// <returns></returns>
        protected ActionResult Respond<T>(ServiceResponse<T> r)
        {
            if (r.Status == ServiceStatus.Ok)
                return Ok(r.Data);

            if (r.Status == ServiceStatus.BadRequest)
                return BadRequest(r.Message);

            return NotFound(r.Message);
        }

        /// <summary>
        /// Create the ActionResult response based on the underlying response from the service layer
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        protected ActionResult Respond(GenericResponse r)
        {
            if (r.Status == ServiceStatus.Ok)
                return Ok();

            if (r.Status == ServiceStatus.BadRequest)
                return BadRequest(r.Message);

            return NotFound(r.Message);
        }
    }
}
