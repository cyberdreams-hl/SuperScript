using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPlayerController : MonoBehaviour
{
    // Attributs du joueur
    public float hp = 100f;
    public float maxHp = 100f;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float hunger = 100f;
    public float maxHunger = 100f;
    public float thirst = 100f;
    public float maxThirst = 100f;
    public float oxygen = 100f;
    public float maxOxygen = 100f;
    public int currentAmmo;
    public int skillPoints; // Points de compétence
    public float currentTemperature;

    // Gestion du Poid
    public float currentPoid = 0f;  // Poid actuel porté par le joueur
    public float maxPoid = 100f;    // Poid maximum que le joueur peut porter

    // Gestion de l'inventaire
    public List<SuperItem> inventory = new List<SuperItem>();

    // Nouvelle barre d'actions (max 5 actions/items dans cet exemple)
    public SuperItem[] actionBar = new SuperItem[5];

    // Paramètres de déplacement et animations
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float sprintSpeed = 6f;
    public float crouchSpeed = 1.5f;
    public float climbSpeed = 2f;

    // Gestion des animations
    public Animator animator;
    public Transform cameraTransform;
    
    // Vecteurs pour la gestion des directions
    private Vector3 moveDirection;
    private CharacterController controller;

    // Animation pour chaque type de déplacement et direction
    public AnimationClip walkForward, walkBackward, walkLeft, walkRight;
    public AnimationClip runForward, runBackward, runLeft, runRight;
    public AnimationClip sprintForward, sprintLeft, sprintRight;
    public AnimationClip crouchForward, crouchBackward, crouchLeft, crouchRight;
    public AnimationClip climb;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hp = maxHp;
        stamina = maxStamina;
        hunger = maxHunger;
        thirst = maxThirst;
        oxygen = maxOxygen;
    }

    void Update()
    {
        // Appel à la gestion des mouvements et animations
        HandleMovement();
    }

    void HandleMovement()
    {
        // Input pour les directions
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        
        // Calcul de la direction du déplacement
        moveDirection = transform.right * moveX + transform.forward * moveZ;

        // Gestion des types de déplacement et de la vitesse
        float speed = 0f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                speed = sprintSpeed; // Sprint
                PlayDirectionalAnimation(sprintForward, sprintForward, sprintLeft, sprintRight, "Sprint");
            }
            else
            {
                speed = runSpeed; // Course
                PlayDirectionalAnimation(runForward, runBackward, runLeft, runRight, "Run");
            }
        }
        else if (Input.GetKey(KeyCode.C))
        {
            speed = crouchSpeed; // Accroupi
            PlayDirectionalAnimation(crouchForward, crouchBackward, crouchLeft, crouchRight, "Crouch");
        }
        else
        {
            speed = walkSpeed; // Marche normale
            PlayDirectionalAnimation(walkForward, walkBackward, walkLeft, walkRight, "Walk");
        }

        // Appliquer la vitesse de déplacement
        controller.Move(moveDirection * speed * Time.deltaTime);

        // Vérifie si le joueur est en train d'escalader (logique simplifiée)
        if (IsClimbing())
        {
            PlayClimbAnimation();
        }
    }

    void PlayDirectionalAnimation(AnimationClip forwardClip, AnimationClip backwardClip, AnimationClip leftClip, AnimationClip rightClip, string actionType)
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveZ > 0)
        {
            animator.Play(forwardClip.name); // Avant
        }
        else if (moveZ < 0)
        {
            animator.Play(backwardClip.name); // Arrière
        }
        else if (moveX > 0)
        {
            animator.Play(rightClip.name); // Droite
        }
        else if (moveX < 0)
        {
            animator.Play(leftClip.name); // Gauche
        }
    }

    void PlayClimbAnimation()
    {
        animator.Play(climb.name); // Joue l'animation d'escalade
        controller.Move(Vector3.up * climbSpeed * Time.deltaTime); // Mouvement simplifié vers le haut
    }

    bool IsClimbing()
    {
        // Logique simplifiée pour détecter si le joueur est en train d'escalader
        return Input.GetKey(KeyCode.Space) && IsNearClimbableSurface();
    }

    bool IsNearClimbableSurface()
    {
        // Implémentation personnalisée selon la logique du jeu pour savoir si le joueur peut escalader
        return true; // Ex. vérification de la distance avec un mur ou une surface
    }

    // Gestion de l'inventaire

    // Fonction pour obtenir la quantité d'un item dans l'inventaire
    public int GetItemQuantity(SuperItem item)
    {
        int totalQuantity = 0;
        foreach (var invItem in inventory)
        {
            if (invItem == item)
            {
                totalQuantity += invItem.itemQuantity;
            }
        }
        return totalQuantity;
    }

    // Fonction pour retirer une certaine quantité d'un item
    public void RemoveItem(SuperItem item, int quantity)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == item)
            {
                if (inventory[i].itemQuantity >= quantity)
                {
                    inventory[i].itemQuantity -= quantity;
                    if (inventory[i].itemQuantity == 0)
                    {
                        inventory.RemoveAt(i);
                    }
                    return;
                }
                else
                {
                    quantity -= inventory[i].itemQuantity;
                    inventory.RemoveAt(i);
                }
            }
        }
    }

    // Fonction pour ajouter un item dans l'inventaire et mettre à jour le Poid
    public void AddItem(SuperItem item, int quantity)
    {
        float totalPoidToAdd = item.itemWeight * quantity;

        // Vérifie si le joueur peut encore porter plus de poid
        if (currentPoid + totalPoidToAdd <= maxPoid)
        {
            bool itemFound = false;
            foreach (var invItem in inventory)
            {
                if (invItem == item && item.isStackable)
                {
                    invItem.itemQuantity += quantity;
                    itemFound = true;
                    break;
                }
            }

            // Si l'item n'a pas été trouvé dans l'inventaire, on l'ajoute
            if (!itemFound)
            {
                SuperItem newItem = Instantiate(item); // Crée une nouvelle instance de l'item
                newItem.itemQuantity = quantity;
                inventory.Add(newItem);
            }

            currentPoid += totalPoidToAdd; // Mise à jour du Poid
        }
        else
        {
            Debug.Log("Poid maximum atteint !");
        }
    }
}

