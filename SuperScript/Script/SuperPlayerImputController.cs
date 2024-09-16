using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPlayerInputController : MonoBehaviour
{
    // Références au contrôleur de joueur et autres composants
    public SuperPlayerController playerController;
    public GameObject actionBarPanel;  // Référence au panneau de la barre d'action
    public GameObject inventoryPanel;  // Référence au panneau de l'inventaire
    public GameObject settingsPanel;   // Référence au panneau des paramètres

    // Touche pour ouvrir/fermer l'inventaire
    public KeyCode inventoryKey = KeyCode.I;

    // Touche pour s'accroupir
    public KeyCode crouchKey = KeyCode.C;

    // Touche pour escalader (si près d'une surface d'escalade)
    public KeyCode climbKey = KeyCode.Space;

    // Touche pour voler (si l'objet pour voler est équipé)
    public KeyCode flyKey = KeyCode.F;

    // Touche pour activer/désactiver la souris pour interagir avec l'interface
    public KeyCode toggleMouseKey = KeyCode.LeftAlt;

    // Raccourcis clavier pour la barre d'action (1 à 5)
    public KeyCode[] actionBarKeys = new KeyCode[5] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    private bool isCrouched = false;
    private bool isFlying = false;
    private bool isMouseEnabled = false;

    void Update()
    {
        HandleMovement();
        HandleActionBar();
        HandleInterface();
    }

    void HandleMovement()
    {
        // Gestion du s'accroupir
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouched = !isCrouched;
            playerController.SetCrouch(isCrouched); // Méthode à implémenter dans SuperPlayerController
        }

        // Gestion de l'escalade (si possible)
        if (Input.GetKey(climbKey) && playerController.IsNearClimbableSurface())
        {
            playerController.StartClimbing(); // Méthode à implémenter dans SuperPlayerController
        }

        // Gestion du vol (si un item qui permet de voler est équipé)
        if (Input.GetKeyDown(flyKey) && playerController.HasFlightItem())
        {
            isFlying = !isFlying;
            playerController.SetFlying(isFlying); // Méthode à implémenter dans SuperPlayerController
        }
    }

    void HandleActionBar()
    {
        // Gestion des raccourcis clavier pour la barre d'action
        for (int i = 0; i < actionBarKeys.Length; i++)
        {
            if (Input.GetKeyDown(actionBarKeys[i]))
            {
                playerController.UseAction(i); // Méthode à implémenter dans SuperPlayerController pour utiliser l'action
            }
        }
    }

    void HandleInterface()
    {
        // Ouvrir/fermer l'inventaire
        if (Input.GetKeyDown(inventoryKey))
        {
            TogglePanel(inventoryPanel);
        }

        // Activer/désactiver la souris pour interagir avec l'UI
        if (Input.GetKeyDown(toggleMouseKey))
        {
            isMouseEnabled = !isMouseEnabled;
            Cursor.lockState = isMouseEnabled ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isMouseEnabled;
        }
    }

    // Fonction pour ouvrir/fermer un panneau
    void TogglePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }
}

