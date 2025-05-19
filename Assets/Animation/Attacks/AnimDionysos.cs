using UnityEngine;

public class HexagonalBubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab; 
    private float spawnInterval = 0.03f; 
    private float tileRadius = 8.7f; 
    private float bubbleRadius = 2f; 
    private int bubblesPerTile = 20; 
    public float duration = 5f; 

    private void Start()
    {
        // Commence à générer les bulles à intervalle très rapide
        InvokeRepeating("SpawnBubble", 0f, spawnInterval);

        // Arrêter la génération de bulles après totalDuration secondes
        Invoke("StopBubbleGeneration", duration);
    }

    void SpawnBubble()
    {
        // Générer une nouvelle bulle à une position aléatoire en dessous de la tuile hexagonale
        Vector3 spawnPosition = GetRandomPositionOnHexagon(tileRadius);
        spawnPosition.y = -1.2f; // Position initiale sous la tuile (en dessous à y = -1.2)

        GameObject newBubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

        // Ajouter le mouvement de montée à la bulle
        BubbleMovement bubbleMovement = newBubble.AddComponent<BubbleMovement>();
        bubbleMovement.tileRadius = tileRadius;
        bubbleMovement.bubbleRadius = bubbleRadius;
        bubbleMovement.targetY = 1.16f; // Hauteur cible mise à jour à 1.16
        bubbleMovement.animationDuration = 1f; // L'animation doit durer 5 secondes
        bubbleMovement.speedMultiplier = 2f; // Facteur d'accélération de la vitesse (ajuster cette valeur pour changer la vitesse)
    }

    // Fonction pour obtenir une position aléatoire sur une tuile hexagonale de rayon 'tileRadius'
    Vector3 GetRandomPositionOnHexagon(float radius)
    {
        // On va utiliser un système de coordonnées polaires pour créer des positions hexagonales
        float angle = Random.Range(0f, 2f * Mathf.PI); // Angle aléatoire entre 0 et 2π
        float distance = Random.Range(0f, radius); // Distance aléatoire à partir du centre, jusqu'à la surface

        // Conversion des coordonnées polaires en coordonnées cartésiennes
        float x = distance * Mathf.Cos(angle);
        float z = distance * Mathf.Sin(angle);

        // Retourner la position calculée
        return new Vector3(x, 0f, z);
    }

    // Fonction pour arrêter la génération de bulles après un certain temps
    void StopBubbleGeneration()
    {
        // Arrêter l'appel répété de la fonction SpawnBubble
        CancelInvoke("SpawnBubble");

        // Détruire toutes les bulles restantes après 5 secondes
        BubbleMovement[] bubbles = FindObjectsOfType<BubbleMovement>();
        foreach (var bubble in bubbles)
        {
            Destroy(bubble.gameObject); // Détruire la bulle
        }
    }
}

public class BubbleMovement : MonoBehaviour
{
    public float targetY = 1.16f; // Hauteur cible mise à jour à y=1.16
    public float tileRadius = 8.7f; // Rayon de la tuile hexagonale (mise à jour à 8.7)
    public float bubbleRadius = 2f; // Rayon de la bulle
    public float animationDuration = 5f; // Durée totale de l'animation (en secondes)
    public float speedMultiplier = 2f; // Facteur d'accélération de la vitesse

    private float startY;
    private float elapsedTime = 0f; // Temps écoulé depuis le début de l'animation

    void Start()
    {
        // Initialiser la position de départ
        startY = transform.position.y;
    }

    void Update()
    {
        // Incrémenter le temps écoulé
        elapsedTime += Time.deltaTime;

        // Calculer la position Y de la bulle pendant l'animation avec un facteur d'accélération
        float newY = Mathf.Lerp(startY, targetY, (elapsedTime / animationDuration) * speedMultiplier);

        // Appliquer la nouvelle position à la bulle
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Si l'animation est terminée, détruire la bulle
        if (elapsedTime >= animationDuration)
        {
            Destroy(gameObject); // Supprimer la bulle après 5 secondes
        }
    }
}
