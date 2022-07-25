using NITGEN.SDK.NBioBSP;  
using Microsoft.AspNetCore.Mvc;
using WebApiFingertec3._0.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApiFingertec3._0.Abstraction;
using System.Data;

namespace WebApiFingertec3._0.Controllers
{
    [ApiController]
    public class FingerController : ControllerBase
    {

        [HttpGet]
        [Route("enroll/{id:int:min(1)}")]
        public string Enroll(int id)
        {
            return Entity.EntityFinger.EnrrowCapture(id);
        }

        [HttpGet]
        //[Route("capture/{id:int:min(1)}")]
        [Route("capture")]
        public string Capture()
        {
            int id = 1;
            return Entity.EntityFinger.Capture(id);   
        }

        [HttpGet]
        [Route("verify-match")]
        public string VerifyMatch([FromBody] Template Templates)
        {

            return Entity.EntityFinger.VerifyMatch(Templates.Templates);

        }

        [HttpGet]
        [Route("identify")]
        public string Identify([FromBody] Object template)
        {

            return Entity.EntityFinger.Identify(template);

            return "";
        }

 

    }
}
