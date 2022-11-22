using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Text;
using WebApiFingertec3._0.Entity;
using WebApiFingertec3._0.Models;
using WebApiFingertec3._0.Repositories;
using WebApiFingertec3._0.Services;

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

        //https://localhost:5050/enroll/1
        [HttpGet]
        [Route("enroll/{id:int:min(1)}")]
        [Authorize]
        public string Enroll(int id)
        {
            try
            {
                return EntityFinger.EnrollCapture(id);
            }
          catch (Exception e)
            {

                return "Erro " + e;
            }
;
        }
        //https://localhost:5050/capture
        [HttpGet]
        //[Route("capture/{id:int:min(1)}")]
        [Route("capture")]
        [Authorize]
        public string Capture()
        {
            try
            {
                int id = 1;
                return EntityFinger.Capture(id);
            }
            catch (Exception e)
            {

                return "Erro " + e;
            }
;
        }
        //https://localhost:5050/verify-match
        [HttpGet]
        [Route("verify-match")]
        [Authorize]
        public string VerifyMatch([FromBody] Template Templates)
        {

            try
            {
                return EntityFinger.VerifyMatch(Templates.template);
            }
            catch (Exception e)
            {

                return "Erro " + e;
            }

        }
         //https://localhost:5050/identify
        [HttpPost]
        [Route("identify")]
        [Authorize]
        public int Identify([FromBody] Object template)
        {

            try
            {
                return EntityFinger.IdentifyData(template);
            }
            catch ( Exception)
            {
                return 0;
            }  
    
          
        }



        //[HttpGet]
        ////[Route("capture/{id:int:min(1)}")]
        //[Route("enrollrdp")]
        //[Authorize]
        //public string Capture2()
        //{
        //    SocketClient socket = new();
        //    return socket.getDigitalString(0);
            
        //}
      
    }
}
