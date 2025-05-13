using System.Collections;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;  // Référence au prefab de la bulle
    public float spawnInterval = 0.5f;  // Intervalle entre chaque bulle
    public int numberOfBubbles = 10;  // Nombre de bulles à générer
    public float lifetime = 2f;  // Durée de vie d'une bulle avant qu'elle disparaisse

    public float hexRadius = 10f;  // Rayon de la tuile hexagonale

    void Start()
    {
        StartCoroutine(SpawnBubbles());
    }

    // Coroutine pour générer les bulles
    private IEnumerator SpawnBubbles()
    {
        for (int i = 0; i < numberOfBubbles; i++)
        {
            // Calculer une position aléatoire sur la tuile hexagonale
            Vector3 spawnPosition = GetRandomHexPosition();
            
            // Créer une bulle à la position calculée
            GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);
            
            // Détruire la bulle après un certain temps
            Destroy(bubble, lifetime);
            
            // Attendre avant de générer la prochaine bulle
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Fonction pour obtenir une position aléatoire sur la tuile hexagonale
    private Vector3 GetRandomHexPosition()
    {
        // Générer une position aléatoire dans le cadre d'une tuile hexagonale
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(0f, hexRadius);

        // Convertir en coordonnées x, y (polaires vers cartésiennes)
        float x = distance * Mathf.Cos(angle);
        float z = distance * Mathf.Sin(angle);

        // Retourner la position
        return new Vector3(x, 0f, z);
    }
}