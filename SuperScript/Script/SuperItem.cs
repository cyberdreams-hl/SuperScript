using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Ammunition,
    CraftingComponent,
    QuestItem,
    Clothing
}

public enum WeaponType
{
    Melee,
    Ranged,
    Magic
}

public enum ConsumableEffect
{
    RestoreHealth,
    RestoreStamina,
    RestoreMana,
    TemporaryMaxHealthBoost,
    TemporaryMaxStaminaBoost
}

[CreateAssetMenu(fileName = "New Super Item", menuName = "SuperItem/Item")]
public class SuperItem : ScriptableObject
{
    // Propriétés de base de l'objet
    public string itemName;
    public ItemType itemType; // Type de l'objet (munitions, nourriture, arme, etc.)
    public Sprite itemIcon; // Icône de l'objet
    public float itemWeight; // Poids de l'objet
    public bool isStackable; // Peut-il être empilé dans l'inventaire ?
    public int maxStack = 1; // Nombre maximum par pile si l'objet est empilable

    // Quantité d'item (Ajout nécessaire pour gérer la quantité dans l'inventaire)
    [HideInInspector] public int itemQuantity = 1; // Quantité par défaut, masqué dans l'éditeur si nécessaire 



    // Pour les objets consommables
    public bool isConsumable; // L'objet est-il consommable ?
    public ConsumableEffect[] consumableEffects; // Liste des effets du consommable
    public float restoreAmount; // Valeur restaurée pour les effets (ex. santé, endurance)

    // Pour les armes
    public bool isWeapon; // L'objet est-il une arme ?
    public WeaponType weaponType; // Type d'arme (corps à corps, à distance, magique)
    public float damage; // Dégâts de l'arme
    public string requiredAmmunition; // Type de munitions requises (si c'est une arme à distance)

    // Pour les objets équipables (armure, vêtements)
    public bool isEquipable; // L'objet peut-il être équipé ?
    public float defenseValue; // Valeur de défense pour l'armure

    // Pour les objets de quête
    public bool isQuestItem; // Objet lié à une quête

    // Fonction pour afficher une description de l'item
    public string GetDescription()
    {
        string description = $"Nom: {itemName}\nType: {itemType.ToString()}\nPoids: {itemWeight}";

        if (isStackable)
            description += $"\nEmpilable jusqu'à: {maxStack}";

        if (isConsumable)
        {
            description += $"\nEffet Consommable: ";
            foreach (var effect in consumableEffects)
            {
                description += $"{effect.ToString()} ({restoreAmount})\n";
            }
        }

        if (isWeapon)
        {
            description += $"\nDégâts: {damage}\nType d'arme: {weaponType.ToString()}";
            if (weaponType == WeaponType.Ranged && !string.IsNullOrEmpty(requiredAmmunition))
                description += $"\nMunition requise: {requiredAmmunition}";
        }

        if (isEquipable)
            description += $"\nValeur de défense: {defenseValue}";

        if (isQuestItem)
            description += "\nObjet de quête";

        return description;
    }
}

