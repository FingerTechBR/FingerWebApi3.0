﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("enroll/{id:int:min(1)}")]
        [Authorize]
        public string Enroll(int id)
        {
            return Entity.EntityFinger.EnrollCapture(id)
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

        [HttpPost]
        [Route("identify")]
        [Authorize]
        public int Identify([FromBody] Object template)
        {

            try
            {
                return Entity.EntityFinger.Identify(template);
            }
            catch ( Exception)
            {
                throw;
            }  
    
          
        }

    }
}
