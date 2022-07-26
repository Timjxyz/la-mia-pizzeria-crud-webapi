﻿using la_mia_pizzeria_static.Models;
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
                IQueryable<Pizza> pizzas = context.Pizzas.Include(p=>p.Category).Include(pizza => pizza.Ingredients);
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

                List<Ingredient> ingredients = context.Ingredients.ToList();

                PizzaCategories model =new PizzaCategories();
               

                List<SelectListItem> listIngredients = new List<SelectListItem>();

                foreach(Ingredient ingredient in ingredients)
                {
                    listIngredients.Add(new SelectListItem(){Text= ingredient.Name, Value= ingredient.IngredientId.ToString()});
                }
                model.Categories = categories;
                model.Pizza = new Pizza();
                model.Ingredients = listIngredients;
                return View("Create",model);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategories formData)
        {
            using(PizzaContext context = new PizzaContext())
            {
                if (!ModelState.IsValid)
                {
                   formData.Categories = context.Categories.ToList();
                    formData.Ingredients = new List<SelectListItem>();
                    List<Ingredient> ingredients = new PizzaContext().Ingredients.ToList();
                    foreach (Ingredient ingredient in ingredients)
                    {
                        formData.Ingredients.Add(new SelectListItem() { Text = ingredient.Name, Value = ingredient.IngredientId.ToString() });
                    }

                    return View(formData);
                }
                context.Pizzas.Add(formData.Pizza);
                context.SaveChanges();

                return RedirectToAction("Index");

            }
         

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizza = context.Pizzas.Where(p => p.PizzaId == id).Include(p=>p.Category).FirstOrDefault();

                if (pizza == null)
                {
                    return NotFound();
                }
                else
                {
                    PizzaCategories model = new PizzaCategories();
                    model.Pizza = pizza;
                    model.Categories = context.Categories.ToList();
                    return View(model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaCategories data)
        {
            

            using (PizzaContext context = new PizzaContext())
            {

                if (!ModelState.IsValid)
                {
                    data.Categories = context.Categories.ToList();
                    return View(data);
                }


                Pizza pizza=context.Pizzas.Where(p => p.PizzaId == id).FirstOrDefault();    
                if(pizza == null)
                {
                    return NotFound();
                }


                pizza.Name = data.Pizza.Name;
                pizza.Img = data.Pizza.Img;
                pizza.Description= data.Pizza.Description;
                pizza.Price = data.Pizza.Price;
                pizza.CategoryId= data.Pizza.CategoryId;
                context.SaveChanges();
                return RedirectToAction("Index");
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


    }
}
