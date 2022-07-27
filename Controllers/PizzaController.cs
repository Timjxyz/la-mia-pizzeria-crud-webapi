using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        private readonly ILogger<PizzaController> _logger;

        public PizzaController(ILogger<PizzaController> logger)
        {
            _logger = logger;
        }
        // GET: MenuPizze
        public IActionResult Index()
        {
            using(PizzaContext context = new PizzaContext())
            {
                IQueryable<Pizza> pizzas = context.Pizzas.Include(p=>p.Category).Include(p => p.Ingredients);
                return View("Index",pizzas.ToList());

            }
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizzaFound = context.Pizzas.Where(pizza => pizza.PizzaId == id).Include(pizza=>pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

                if (pizzaFound == null)
                {
                    return NotFound($"La pizza con id {id} non è stato trovato");
                }
                else
                {
                    return View("Details", pizzaFound);
                }
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            using (PizzaContext context = new PizzaContext())
            {
                List<Category> categories = context.Categories.ToList();


                PizzaCategories model =new PizzaCategories();
               

                
                model.Categories = categories;
                model.Pizza = new Pizza();
                model.Ingredients = RetriveTagListItem();
                return View("Create",model);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategories data)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext context = new PizzaContext())
                {
                    List<Category> categories = context.Categories.ToList();
                    data.Categories = categories;
                    data.Ingredients = RetriveTagListItem();
                    return View("Create",data);
                    

                }
            }

            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizzaToCreate = new Pizza();
                pizzaToCreate.Name=data.Pizza.Name;
                pizzaToCreate.Img=data.Pizza.Img;
                pizzaToCreate.Description=data.Pizza.Description;   
                pizzaToCreate.CategoryId=data.Pizza.CategoryId; 
                pizzaToCreate.Ingredients=new List<Ingredient>();

                if (data.SelectedIngredients != null)
                {
                    foreach(string selectedIngredientId in data.SelectedIngredients)
                    {
                        int selectedIntIngredient=int.Parse(selectedIngredientId);
                        Ingredient ingredient= context.Ingredients.Where(m=>m.Id==selectedIntIngredient).FirstOrDefault();  
                        pizzaToCreate.Ingredients.Add(ingredient);
                    }
                }

                context.Pizzas.Add(pizzaToCreate);
                context.SaveChanges();
                return RedirectToAction("Index");


            }



        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizzaToEdit = context.Pizzas.Where(p => p.PizzaId == id).Include(p=>p.Category).Include(p => p.Ingredients).FirstOrDefault();

                if (pizzaToEdit == null)
                {
                    return NotFound();
                }
                else
                {
                    List<Category> categories = context.Categories.ToList();
                    PizzaCategories model = new PizzaCategories();
                    model.Pizza = pizzaToEdit;
                    model.Categories = context.Categories.ToList();
                    model.SelectedIngredients = new List<string>();

                    foreach(Ingredient t in pizzaToEdit.Ingredients)
                    {
                        model.SelectedIngredients.Add(t.Id.ToString());
                    }

                    model.Ingredients = RetriveTagListItem();

                    return View(model);
                }
            }
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaCategories data)
        {

            if (!ModelState.IsValid)
            {


                using (PizzaContext context = new PizzaContext())
                {
                    List<Category> categories = context.Categories.ToList();
                    data.Categories = categories;
                    data.Ingredients = RetriveTagListItem();
                    return View("Update", data);


                }

            }
            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizzaToEdit = context.Pizzas.Where(p => p.PizzaId == id).Include(p=>p.Ingredients).FirstOrDefault();
                if(pizzaToEdit!= null)
                {
                    pizzaToEdit.Name = data.Pizza.Name;
                    pizzaToEdit.Img = data.Pizza.Img;
                    pizzaToEdit.Description = data.Pizza.Description;
                    pizzaToEdit.Price = data.Pizza.Price;
                    pizzaToEdit.CategoryId = data.Pizza.CategoryId;
                    pizzaToEdit.Ingredients.Clear();

                    if (data.SelectedIngredients != null)
                    {
                        foreach (string selectedIngredientId in data.SelectedIngredients)
                        {
                            int selectedIntIngredient = int.Parse(selectedIngredientId);
                            Ingredient ingredient = context.Ingredients.Where(m => m.Id == selectedIntIngredient).FirstOrDefault();
                            pizzaToEdit.Ingredients.Add(ingredient);
                        }
                    }


                    context.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    return NotFound();
                }
            }
                
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizza = context.Pizzas.Where(pizza => pizza.PizzaId == id).FirstOrDefault();

                if(pizza == null)
                {
                    return NotFound();
                }

                context.Pizzas.Remove(pizza);   
                context.SaveChanges();
                return RedirectToAction("Index");

            }

        }

        private static List<SelectListItem> RetriveTagListItem()
        {
            using (PizzaContext context = new PizzaContext())
            {

                List<SelectListItem> ingredientList = new List<SelectListItem>();
                List<Ingredient> ingredients = context.Ingredients.ToList();
                foreach (Ingredient ingredient in ingredients)
                {
                    ingredientList.Add(new SelectListItem() { Text = ingredient.Name, Value = ingredient.Id.ToString() });
                }
                return ingredientList;
            }
        }


    }
}
