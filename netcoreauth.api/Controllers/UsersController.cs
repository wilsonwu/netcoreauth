using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using netcoreauth.model;

namespace netcoreauth.api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserRepository userRepository;

        public UsersController()
        {
            userRepository = new UserRepository(Startup.connString);
        }

        [Route("checkemail")]
        [HttpGet]
        public dynamic CheckEmail(string email)
        {
            dynamic result;
            var user = userRepository.GetByEmail(email);
            if (user != null)
            {
                result = new
                {
                    code = ReturnCodes.DataGetSucceeded,
                    data = new
                    {
                        isemailavailable = false,
                        activation = user.Is_Activated,
                    },
                };
            }
            else
            {
                result = new
                {
                    code = ReturnCodes.DataGetFailedWithNoData,
                    data = new
                    {
                        isemailavailable = true,
                        activation = false,
                    },
                };
            }
            return result;
        }

        [Route("active/{token}")]
        [HttpGet]
        public dynamic ActiveUser(string token)
        {
			dynamic result;
            string email = Utilities.GetSubFromToken("active", token);
            if (string.IsNullOrEmpty(email))
            {
                result = new
                {
                    code = ReturnCodes.DataUpdateFailed,
                };
			}
            else
            {
				bool active = userRepository.SetActive(email, token);
				result = new
				{
					code = active ? ReturnCodes.DataRemoveSucceeded : ReturnCodes.DataUpdateFailed,
				};
			}
            return result;
        }

		[Route("reset/{token}")]
        [HttpPost]
        public dynamic ResetPassword(string token, [FromBody]User user)
		{
            dynamic result;

			string tokenEmail = Utilities.GetSubFromToken("reset", token);
            if (!string.IsNullOrEmpty(tokenEmail) && tokenEmail == user.Email)
            {
                bool reset = userRepository.UpdatePasswordByEmail(user.Email, user.Password, token);
                result = new
                {
                    code = reset ? ReturnCodes.DataUpdateSucceeded : ReturnCodes.DataUpdateFailed,
                };
            }
            else
            {
                result = new
                {
                    code = ReturnCodes.DataUpdateFailed,
                };
			}
			return result;
		}

        [Route("sendactivemail/{email}")]
        [HttpGet]
        public dynamic SendActiveMail(string email)
        {
            dynamic result;
            var getUser = userRepository.GetByEmail(email);
            if (getUser != null)
            {
                string sent = Utilities.ActiveMailAsync(email);
                result = new
                {
                    code = ReturnCodes.DataGetSucceeded,
                    data = sent,
                };
            }
            else
            {
				result = new
				{
                    code = ReturnCodes.DataGetFailed,
					data = "Fail",
				};
			}
			return result;
		}

		[Route("sendresetmail/{email}")]
		[HttpGet]
		public dynamic SendResetMail(string email)
		{
			dynamic result;
			var getUser = userRepository.GetByEmail(email);
			if (getUser != null)
			{
				string sent = Utilities.ResetMailAsync(email);
				result = new
				{
					code = ReturnCodes.DataGetSucceeded,
					data = sent,
				};
			}
			else
			{
				result = new
				{
					code = ReturnCodes.DataGetFailed,
					data = "Fail",
				};
			}
			return result;
		}

		// POST api/values
		[HttpPost]
        public dynamic Post([FromBody]User user)
        {
			string sent = string.Empty;

			int createdId = userRepository.Add(user);
            if (createdId > 0)
            {
				user.Id = createdId;
                sent = Utilities.ActiveMailAsync(user.Email);
            }
            dynamic result = new
            {
                code = createdId > 0 ? ReturnCodes.DataCreateSucceeded : ReturnCodes.DataCreateFailed,
                data = new
                {
                    id = createdId,
                    email = createdId > 0 ? user.Email : null,
                    activation = false,
                    activemailsend = sent,
                },
            };
            return result;
        }
    }
}
