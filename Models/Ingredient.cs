using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

