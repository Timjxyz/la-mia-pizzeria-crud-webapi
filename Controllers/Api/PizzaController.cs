using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers.Api
{
    [Route("api/pizzas")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        public IActionResult Get()
        {
            PizzaContext pizzaContext = new PizzaContext();
            IQueryable<Pizza> pizzaList = pizzaContext.Pizzas;
            return Ok(pizzaList);
        }
    }
}
