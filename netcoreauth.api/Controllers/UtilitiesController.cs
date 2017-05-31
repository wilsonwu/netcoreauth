using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netcoreauth.model;

namespace netcoreauth.api.Controllers
{
    [Route("api/[controller]")]
    public class UtilitiesController : Controller
    {
        private readonly Mail mail;

		public UtilitiesController()
		{
            mail = new Mail();
		}

		// GET: api/values
		//[Authorize]
        [Route("sendmail")]
		[HttpGet]
		public dynamic SendMail()
		{
            var result = mail.SendEmail("whfill@163.com", "admin@imoogoo.com", "test", "activelink");
			return result;
		}

	}
}
