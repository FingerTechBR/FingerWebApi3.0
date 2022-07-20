using NITGEN.SDK.NBioBSP;  
using Microsoft.AspNetCore.Mvc;
using WebApiFingertec3._0.Models;
using System.Text.Json;

namespace WebApiFingertec3._0.Controllers
{
    [ApiController]
    public class FingerController : ControllerBase
    {

        [HttpGet]
        [Route("Enroll/{id:int:min(1)}")]
        public string Enroll(int id)
        {
            return Entity.EntityFinger.EnrrowCapture(id);
        }

        [HttpGet]
        [Route("Capture/{id:int:min(1)}")]
        public string Capture(int id)
        {
            return Entity.EntityFinger.Capture(id);   
        }

        [HttpGet]
        [Route("Identify")]
        public string Identify([FromBody] Template Templates)
        {

            return Entity.EntityFinger.Identify(Templates.Templates);

        }

        [HttpGet]
        [Route("Compare")]
        public string Compare([FromBody]string textFir)
        {
            string er5r = textFir;
            return Entity.EntityFinger.Compare(textFir);
        }




    }
}
