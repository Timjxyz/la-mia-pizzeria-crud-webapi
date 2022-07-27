using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace la_mia_pizzeria_static.Models
{
    public class Ingredient
    {
        public Ingredient()
        {


        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Pizza> Pizzas { get; set; }


    }
}
       


