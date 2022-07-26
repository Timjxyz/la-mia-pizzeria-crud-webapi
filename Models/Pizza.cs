﻿using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Pizza
{
    [Key]
    public int PizzaId { get; set; }
    [StringLength(50, ErrorMessage = "Il numero massimo di caratteri inseribili è 50 caratteri")]
    [Required(ErrorMessage = "Il campo è obbligatorio")]
    public string Name { get; set; }
    [ValidatorPizzaForm]
    [StringLength(100, ErrorMessage = "Il numero massimo di caratteri inseribili è 100 caratteri")]
    public string Description { get; set; }

    public string Img { get; set; }
    [Range(1, 100, ErrorMessage = "Il prezzo selezionato non è valido, min 1/ max 100")]
    public double Price { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public List<Ingredient>? Ingredients {get;set;}
        public Pizza()
        {

        }
}

