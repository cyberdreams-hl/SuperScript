using UnityEditor;
using UnityEngine;
using System.Collections.Generic; // Nécessaire pour utiliser List<>

[CustomEditor(typeof(SuperCraftRecipe))]
public class SuperCraftRecipeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SuperCraftRecipe recipe = (SuperCraftRecipe)target;

        // Nom de la recette
        recipe.recipeName = EditorGUILayout.TextField("Nom de la recette", recipe.recipeName);

        // Résultat du craft
        recipe.resultItem = (SuperItem)EditorGUILayout.ObjectField("Résultat (Item)", recipe.resultItem, typeof(SuperItem), false);
        recipe.resultQuantity = EditorGUILayout.IntField("Quantité résultante", recipe.resultQuantity);

        // Liste des ingrédients
        EditorGUILayout.LabelField("Ingrédients", EditorStyles.boldLabel);

        if (recipe.ingredients == null)
        {
            recipe.ingredients = new List<SuperCraftRecipe.Ingredient>();
        }

        for (int i = 0; i < recipe.ingredients.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            recipe.ingredients[i].item = (SuperItem)EditorGUILayout.ObjectField("Item", recipe.ingredients[i].item, typeof(SuperItem), false);
            recipe.ingredients[i].quantity = EditorGUILayout.IntField("Quantité", recipe.ingredients[i].quantity);

            // Bouton pour supprimer un ingrédient
            if (GUILayout.Button("Supprimer"))
            {
                recipe.ingredients.RemoveAt(i);
                i--; // Ajuste l'index après la suppression
            }

            EditorGUILayout.EndHorizontal();
        }

        // Bouton pour ajouter un nouvel ingrédient
        if (GUILayout.Button("Ajouter Ingrédient"))
        {
            recipe.ingredients.Add(new SuperCraftRecipe.Ingredient());
        }

        // Appliquer les modifications
        if (GUI.changed)
        {
            EditorUtility.SetDirty(recipe);
        }
    }
}
