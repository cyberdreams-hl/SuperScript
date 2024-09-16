using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Nécessaire pour la navigation de l'IA

public class SuperEnemyController : MonoBehaviour
{
    // Attributs de l'ennemi
    public float hp = 100f;
    public float maxHp = 100f;
    public float detectionRadius = 10f; // Radius de détection pour repérer le joueur
    public float attackRange = 2f;      // Distance à laquelle l'ennemi peut attaquer
    public float attackCooldown = 2f;   // Temps entre deux attaques

    private float nextAttackTime = 0f;  // Timer pour gérer le cooldown des attaques
    private Transform player;           // Référence au joueur
    private NavMeshAgent agent;         // L'agent NavMesh pour le déplacement
    private Animator animator;          // Référence à l'Animator pour les animations

    // Références pour les attaques
    [System.Serializable]
    public class Attack
    {
        public string attackName;        // Nom de l'attaque
        public AnimationClip animation;  // Animation de l'attaque
        public AudioClip sound;          // Son joué lors de l'attaque
        public ParticleSystem particle;  // Particules générées lors de l'attaque
        public float minDistance;        // Distance minimum pour utiliser cette attaque
        public float maxDistance;        // Distance maximum pour utiliser cette attaque
        public float damage;             // Dégâts infligés par l'attaque
    }
    public List<Attack> attacks = new List<Attack>(); // Liste des attaques configurables

    // Paramètres pour le comportement
    public bool canChasePlayer = true;   // L'ennemi peut-il poursuivre le joueur ?
    public bool canAttack = true;        // L'ennemi peut-il attaquer le joueur ?

    void Start()
    {
        // Initialisation
        player = GameObject.FindGameObjectWithTag("Player").transform; // Recherche du joueur
        agent = GetComponent<NavMeshAgent>();  // Récupération du NavMeshAgent
        animator = GetComponent<Animator>();   // Récupération de l'Animator
        hp = maxHp;
    }

    void Update()
    {
        // Gestion de la détection du joueur
        DetectPlayer();

        // Si l'ennemi est en mode poursuite et qu'il peut attaquer
        if (canChasePlayer && canAttack && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                // Lancer une attaque appropriée selon la distance et d'autres circonstances
                PerformAttack();
                nextAttackTime = Time.time + attackCooldown; // Gestion du cooldown
            }
        }
    }

    void DetectPlayer()
    {
        // Détection du joueur dans le radius
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            if (canChasePlayer)
            {
                agent.SetDestination(player.position); // Poursuit le joueur
                animator.SetBool("isWalking", true);   // Active l'animation de marche
            }
        }
        else
        {
            // Si le joueur est hors du radius, l'ennemi arrête de bouger
            agent.SetDestination(transform.position);
            animator.SetBool("isWalking", false);      // Désactive l'animation de marche
        }
    }

    void PerformAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Choisir l'attaque en fonction de la distance
        foreach (Attack attack in attacks)
        {
            if (distanceToPlayer >= attack.minDistance && distanceToPlayer <= attack.maxDistance)
            {
                // Lancer l'animation de l'attaque
                animator.Play(attack.animation.name);

                // Jouer le son associé
                if (attack.sound != null)
                {
                    AudioSource.PlayClipAtPoint(attack.sound, transform.position);
                }

                // Activer les particules
                if (attack.particle != null)
                {
                    attack.particle.Play();
                }

                // Appliquer les dégâts au joueur (logique à compléter)
                ApplyDamageToPlayer(attack.damage);

                break; // Une attaque par cycle
            }
        }
    }

    void ApplyDamageToPlayer(float damage)
    {
        // Logique pour infliger des dégâts au joueur (peut être connecté au script du joueur)
        Debug.Log("Inflige " + damage + " dégâts au joueur.");
    }

    // Fonction appelée quand l'ennemi reçoit des dégâts
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die(); // Lancer la mort de l'ennemi
        }
    }

    void Die()
    {
        // Animation de mort
        animator.SetTrigger("Die");
        agent.isStopped = true; // Arrête le déplacement de l'ennemi
        // Autres logiques pour la mort, comme la désactivation des collisions ou la destruction de l'objet
        Destroy(gameObject, 3f); // Délai avant la destruction de l'ennemi
    }
}
