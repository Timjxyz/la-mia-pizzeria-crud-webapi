using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Ingredient
    {
        public Ingredient()
        {


        }

        [Key]
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public List<Pizza> Pizzas { get; set; }


    }
}
       


