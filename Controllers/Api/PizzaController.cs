using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers.Api
{
    [Route("api/pizzas")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string? search)
        {
            using(PizzaContext context = new PizzaContext())
            {
                IQueryable<Pizza> pizzas = context.Pizzas;
                if(search != null && search!= "")
                {
                    pizzas = pizzas.Where(p => p.Name.Contains(search));
                }
                return Ok(pizzas.ToList());

            }

        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizza=context.Pizzas.Where(p => p.PizzaId == id).FirstOrDefault();
                if (pizza == null)
                {
                    return NotFound();  
                }
                return Ok(pizza);

            }
        }

        
    }
}
