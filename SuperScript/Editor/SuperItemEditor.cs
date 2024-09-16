using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SuperItem))]
public class SuperItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SuperItem item = (SuperItem)target;

        // Affiche les propriétés de base dans l'inspecteur
        EditorGUILayout.LabelField("Propriétés de base", EditorStyles.boldLabel);
        item.itemName = EditorGUILayout.TextField("Nom de l'item", item.itemName);
        item.itemIcon = (Sprite)EditorGUILayout.ObjectField("Icône de l'item", item.itemIcon, typeof(Sprite), false);
        item.itemType = (ItemType)EditorGUILayout.EnumPopup("Type de l'item", item.itemType);
        item.itemWeight = EditorGUILayout.FloatField("Poids", item.itemWeight);
        item.isStackable = EditorGUILayout.Toggle("Empilable ?", item.isStackable);

        if (item.isStackable)
        {
            item.maxStack = EditorGUILayout.IntField("Quantité maximale par pile", item.maxStack);
        }

        // Si l'item est consommable
        item.isConsumable = EditorGUILayout.Toggle("Consommable ?", item.isConsumable);
        if (item.isConsumable)
        {
            EditorGUILayout.LabelField("Effets Consommables", EditorStyles.boldLabel);
            SerializedProperty effects = serializedObject.FindProperty("consumableEffects");
            EditorGUILayout.PropertyField(effects, true);
            item.restoreAmount = EditorGUILayout.FloatField("Valeur restaurée", item.restoreAmount);
        }

        // Si l'item est une arme
        item.isWeapon = EditorGUILayout.Toggle("Arme ?", item.isWeapon);
        if (item.isWeapon)
        {
            item.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Type d'arme", item.weaponType);
            item.damage = EditorGUILayout.FloatField("Dégâts", item.damage);

            if (item.weaponType == WeaponType.Ranged)
            {
                item.requiredAmmunition = EditorGUILayout.TextField("Type de munitions requises", item.requiredAmmunition);
            }
        }

        // Si l'item est équipable
        item.isEquipable = EditorGUILayout.Toggle("Équipable ?", item.isEquipable);
        if (item.isEquipable)
        {
            item.defenseValue = EditorGUILayout.FloatField("Valeur de défense", item.defenseValue);
        }

        // Si l'item est un objet de quête
        item.isQuestItem = EditorGUILayout.Toggle("Objet de quête ?", item.isQuestItem);

        // Appliquer les modifications
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(item);
        }
    }
}

