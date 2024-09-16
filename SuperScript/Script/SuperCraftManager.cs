using System.Collections.Generic;
using UnityEngine;

public class SuperCraftManager : MonoBehaviour
{
    public List<SuperCraftRecipe> allRecipes = new List<SuperCraftRecipe>(); // Toutes les recettes de craft disponibles
    private List<SuperCraftRecipe> unlockedRecipes = new List<SuperCraftRecipe>(); // Recettes débloquées

    // Vérifie si une recette est débloquée
    public bool IsRecipeUnlocked(SuperCraftRecipe recipe)
    {
        return unlockedRecipes.Contains(recipe);
    }

    // Débloque une recette
    public void UnlockRecipe(SuperCraftRecipe recipe)
    {
        if (!unlockedRecipes.Contains(recipe))
        {
            unlockedRecipes.Add(recipe);
            Debug.Log(recipe.recipeName + " a été débloquée.");
        }
    }

    // Vérifie si le joueur peut crafter l'item
    public bool CanCraft(SuperCraftRecipe recipe)
    {
        // Logique pour vérifier si les ingrédients sont disponibles dans l'inventaire
        return true;
    }

    // Crafter l'item
    public void Craft(SuperCraftRecipe recipe)
    {
        if (CanCraft(recipe))
        {
            // Logique pour retirer les ingrédients et ajouter l'item dans l'inventaire du joueur
            Debug.Log("Crafting: " + recipe.resultItem.itemName);
        }
    }
}

