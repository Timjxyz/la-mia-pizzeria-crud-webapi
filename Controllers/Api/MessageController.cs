using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers.Api
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        
        [HttpPost]
        public IActionResult Post([FromBody] Message message)
        {
          
            using (PizzaContext ctx = new PizzaContext())
            {
                ctx.Messages.Add(message);
                ctx.SaveChanges();
                return Ok();
            }
           
        }

    }
}