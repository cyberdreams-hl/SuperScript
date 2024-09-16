using System.Collections;
using System.Collections.Generic; // Nécessaire pour utiliser List<>
using UnityEngine;

[CreateAssetMenu(fileName = "New Craft Recipe", menuName = "SuperCraft/CraftRecipe")]
public class SuperCraftRecipe : ScriptableObject
{
    public string recipeName; // Nom de la recette de craft
    public SuperItem resultItem; // Item résultant de la recette
    public int resultQuantity = 1; // Quantité de l'item résultant

    // Classe interne pour représenter un ingrédient et sa quantité
    [System.Serializable]
    public class Ingredient
    {
        public SuperItem item; // L'item utilisé comme ingrédient
        public int quantity;   // Quantité de l'ingrédient nécessaire
    }

    // Liste des ingrédients nécessaires pour crafter l'item
    public List<Ingredient> ingredients = new List<Ingredient>(); 
}

