using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Nécessaire pour utiliser TextMeshPro

public class SuperUIController : MonoBehaviour
{
    // Références au script SuperPlayerController pour récupérer les attributs du joueur
    public SuperPlayerController superPlayerController;
    public SuperCraftManager craftManager; // Gestionnaire de craft

    // Références UI pour les attributs du joueur avec TextMeshPro
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI thirstText;
    public TextMeshProUGUI oxygenText;
    public TextMeshProUGUI poidText; 

    // Références pour les barres de progression (santé, endurance)
    public Image healthBar;
    public Image staminaBar;
    public Image hungerBar;
    public Image thirstBar;
    public Image oxygenBar;
    public Image poidBar;

    // Inventaire (affichage simplifié avec des textes ou des images)
    public Transform inventoryGrid;
    public TextMeshProUGUI[] inventorySlots;

    // Référence à l'image du réticule
    public Image reticle;

    // Références pour la fenêtre de craft
    public GameObject craftWindow;
    public Transform craftListContent;
    public GameObject craftItemPrefab;
    public Button unlockCraftButton;
    public Button craftButton;
    private SuperCraftRecipe selectedRecipe;

    // Références pour la fenêtre des points de compétence
    public GameObject skillWindow;
    public Button increaseHealthButton;
    public Button increaseStaminaButton;
    public Button increaseHungerButton;
    public Button increaseThirstButton;
    public Button increaseOxygenButton;
    public Button increasePoidButton;

    void Start()
    {
        // Initialisation des valeurs UI au démarrage
        UpdateUI();
        InitializeCraftWindow(); // Initialiser la fenêtre de craft
        InitializeSkillWindow(); // Initialiser la fenêtre des points de compétence
    }

    void Update()
    {
        // Mise à jour continue des informations du joueur sur l'interface utilisateur
        UpdateUI();
    }

    void UpdateUI()
    {
        // Mise à jour des attributs du joueur avec TextMeshPro
        healthText.text = "Health: " + superPlayerController.hp + " / " + superPlayerController.maxHp;
        staminaText.text = "Stamina: " + superPlayerController.stamina + " / " + superPlayerController.maxStamina;
        hungerText.text = "Hunger: " + superPlayerController.hunger + " / " + superPlayerController.maxHunger;
        thirstText.text = "Thirst: " + superPlayerController.thirst + " / " + superPlayerController.maxThirst;
        oxygenText.text = "Oxygen: " + superPlayerController.oxygen + " / " + superPlayerController.maxOxygen;
        poidText.text = "Poid: " + superPlayerController.currentPoid + " / " + superPlayerController.maxPoid;

        // Mise à jour des barres de santé et d'endurance
        healthBar.fillAmount = superPlayerController.hp / superPlayerController.maxHp;
        staminaBar.fillAmount = superPlayerController.stamina / superPlayerController.maxStamina;
        hungerBar.fillAmount = superPlayerController.hunger / superPlayerController.maxHunger;
        oxygenBar.fillAmount = superPlayerController.oxygen/ superPlayerController.maxOxygen;
        thirstBar.fillAmount = superPlayerController.thirst / superPlayerController.maxThirst;
        poidBar.fillAmount = superPlayerController.currentPoid / superPlayerController.maxPoid;

        // Mise à jour de l'inventaire (affichage simple)
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < superPlayerController.inventory.Count)
            {
                SuperItem item = superPlayerController.inventory[i];
                inventorySlots[i].text = item.itemName + " x" + item.itemQuantity;
            }
            else
            {
                inventorySlots[i].text = "";
            }
        }
    }

    // Initialisation de la fenêtre de craft
    void InitializeCraftWindow()
    {
        // Vide la liste des crafts actuels
        foreach (Transform child in craftListContent)
        {
            Destroy(child.gameObject);
        }

        // Remplis la liste des crafts possibles
        foreach (SuperCraftRecipe recipe in craftManager.allRecipes)
        {
            GameObject craftItem = Instantiate(craftItemPrefab, craftListContent);
            craftItem.GetComponentInChildren<TextMeshProUGUI>().text = recipe.recipeName;
            craftItem.GetComponent<Button>().onClick.AddListener(() => SelectRecipe(recipe));
        }

        craftButton.interactable = false;
        unlockCraftButton.interactable = false;
    }

    // Sélectionner une recette de craft dans la fenêtre
    public void SelectRecipe(SuperCraftRecipe recipe)
    {
        selectedRecipe = recipe;

        // Si la recette est débloquée, permet de crafter
        if (craftManager.IsRecipeUnlocked(recipe))
        {
            craftButton.interactable = true;
            unlockCraftButton.interactable = false;
        }
        else
        {
            unlockCraftButton.interactable = true;
            craftButton.interactable = false;
        }
    }

    // Débloquer la recette avec des points de compétence
    public void UnlockRecipe()
    {
        if (selectedRecipe != null && !craftManager.IsRecipeUnlocked(selectedRecipe))
        {
            // Vérifie si le joueur a assez de points de compétence pour débloquer la recette
            if (superPlayerController.skillPoints > 0)
            {
                craftManager.UnlockRecipe(selectedRecipe);
                superPlayerController.skillPoints--; // Déduit un point de compétence

                unlockCraftButton.interactable = false;
                craftButton.interactable = true;
            }
            else
            {
                Debug.Log("Pas assez de points de compétence pour débloquer cette recette.");
            }
        }
    }

    // Crafter l'item de la recette sélectionnée
    public void CraftItem()
    {
        if (selectedRecipe != null && craftManager.CanCraft(selectedRecipe))
        {
            craftManager.Craft(selectedRecipe);
        }
    }

    // Initialisation de la fenêtre des points de compétence
    void InitializeSkillWindow()
    {
        increaseHealthButton.onClick.AddListener(() => IncreaseAttribute("health"));
        increaseStaminaButton.onClick.AddListener(() => IncreaseAttribute("stamina"));
    }

    // Augmenter un attribut avec les points de compétence
    public void IncreaseAttribute(string attribute)
    {
        if (superPlayerController.skillPoints > 0)
        {
            switch (attribute)
            {
                case "health":
                    superPlayerController.maxHp += 10;
                    break;
                case "stamina":
                    superPlayerController.maxStamina += 10;
                    break;
                case "hunger":
                    superPlayerController.maxHunger += 10;
                    break;
                case "thirst":
                    superPlayerController.maxThirst += 10;
                    break;
                case "oxygen":
                    superPlayerController.maxOxygen += 10;
                    break;
                case "poid":
                    superPlayerController.maxPoid+= 10;
                    break;
                    
                    
            }

            superPlayerController.skillPoints--;
            UpdateUI();
        }
    }
}
