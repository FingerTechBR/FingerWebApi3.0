using NITGEN.SDK.NBioBSP;  
using Microsoft.AspNetCore.Mvc;
using WebApiFingertec3._0.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApiFingertec3._0.Abstraction;
using System.Data;
using WebApiFingertec3._0.Services;
using WebApiFingertec3._0.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace WebApiFingertec3._0.Controllers
{
    [ApiController]
    public class FingerController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user,
                token
            };
        }

        [HttpGet]
        [Route("enroll/{id:int:min(1)}")]
        [Authorize]
        public string Enroll(int id)
        {
            return Entity.EntityFinger.EnrrowCapture(id)
;
        }

        [HttpGet]
        //[Route("capture/{id:int:min(1)}")]
        [Route("capture")]
        [Authorize]
        public string Capture()
        {
            int id = 1;
            return Entity.EntityFinger.Capture(id)
;
        }

        [HttpGet]
        [Route("verify-match")]
        [Authorize]
        public string VerifyMatch([FromBody] Template Templates)
        {

            return Entity.EntityFinger.VerifyMatch(Templates.Templates);

        }

        [HttpGet]
        [Route("identify")]
        [Authorize]
        public string Identify([FromBody] Object template)
        {

            return Entity.EntityFinger.Identify(template);

        }
        
    }
}
