using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f; // Santé maximale
    public float currentHealth;    // Santé actuelle

    void Start()
    {
        // Initialisation de la santé actuelle à la santé maximale au démarrage
        currentHealth = maxHealth;
    }

    // Fonction pour infliger des dégâts
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Réduire la santé par la quantité de dégâts reçus
        Debug.Log(gameObject.name + " a pris " + damage + " de dégâts.");

        if (currentHealth <= 0)
        {
            Die(); // Si la santé est à zéro ou moins, on détruit l'objet ou on lance la fonction de mort
        }
    }

    // Fonction pour gérer la mort de l'objet
    void Die()
    {
        Debug.Log(gameObject.name + " est mort.");
        Destroy(gameObject); // Détruire l'objet dans la scène
    }
}
