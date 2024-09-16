using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCamera : MonoBehaviour
{
    public Transform player; // Référence au joueur à suivre
    public Vector3 offset; // Décalage de la caméra par rapport au joueur
    public float cameraDistance = 5f; // Distance de la caméra par rapport au joueur
    public float cameraHeight = 2f; // Hauteur de la caméra par rapport au joueur
    public float sensitivity = 3f; // Sensibilité de la souris pour le mouvement de la caméra

    public Transform cameraPivot; // Le pivot de la caméra, généralement à la tête du joueur
    public Transform reticle; // Le réticule pour viser au centre de l'écran

    private float yaw = 0f; // Rotation horizontale de la caméra
    private float pitch = 0f; // Rotation verticale de la caméra

    void Start()
    {
        // Cache le curseur pour une expérience plus immersive
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialisation du décalage de la caméra si nécessaire
        if (cameraPivot == null)
        {
            cameraPivot = player;
        }
    }

    void LateUpdate()
    {
        // Déplacer la caméra autour du joueur avec la souris
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -35f, 60f); // Limite l'angle vertical pour éviter de retourner la caméra

        // Calculer la position et la rotation de la caméra
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 positionOffset = rotation * new Vector3(0, cameraHeight, -cameraDistance);
        transform.position = player.position + positionOffset;

        // La caméra regarde toujours le joueur
        transform.LookAt(cameraPivot.position + new Vector3(0, cameraHeight, 0));

        // Mécanique de tir
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Rayon partant du centre de l'écran
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Tir effectué sur : " + hit.collider.name);
            // Applique des dommages à l'objet touché s'il a un script de santé (Health)
            Health targetHealth = hit.collider.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(10f); // Applique 10 points de dégâts
            }

            // Ajoutez ici d'autres effets comme des particules ou des sons à l'impact
        }
    }
}
