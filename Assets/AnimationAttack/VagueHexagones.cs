using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexagonWave : MonoBehaviour
{
    // Placement variables
    public GameObject hexagonePrefab; // Prefab pour l'hexagone
    public Transform centre; // Position centrale pour le placement des hexagones
    public List<GameObject> hexagonesInterieurs = new List<GameObject>(); // Hexagones intérieurs (6)
    public List<GameObject> hexagonesExterieurs = new List<GameObject>(); // Hexagones extérieurs (12)

    // Animation variables
    public float amplitude = 1.0f; // Hauteur de la vague
    public float vitesseVague = 2.0f; // Vitesse de la vague
    public int nombreDeCycles = 3; // Nombre de cycles de la vague
    public float delaiPropagation = 0.5f; // Délai entre les groupes intérieur et extérieur

    // Stockage des positions initiales
    private List<Vector3> positionsInitialesInterieurs = new List<Vector3>();
    private List<Vector3> positionsInitialesExterieurs = new List<Vector3>();

    void Start()
    {
        // Placer les hexagones
        PlacerHexagones();

        // Stocker les positions initiales
        StockerPositionsInitiales();

        // Démarrer l'animation de la vague
        StartCoroutine(AnimerVague());
    }

    void PlacerHexagones()
    {
        // Hexagone central à (0, 0, 0)
        GameObject centralHexagone = Instantiate(hexagonePrefab, centre.position, Quaternion.identity);

        // Positions des hexagones intérieurs (6)
        Vector3[] positionsInterieurs = new Vector3[]
        {
            new Vector3(17.3f, 0, 0),
            new Vector3(-17.3f, 0, 0),
            new Vector3(8.65f, 0, 15f),
            new Vector3(-8.65f, 0, 15f),
            new Vector3(8.65f, 0, -15f),
            new Vector3(-8.65f, 0, -15f)
        };

        // Instancier les hexagones intérieurs
        foreach (Vector3 position in positionsInterieurs)
        {
            GameObject hex = Instantiate(hexagonePrefab, centre.position + position, Quaternion.identity);
            hexagonesInterieurs.Add(hex);
        }

        // Positions des hexagones extérieurs (12)
        Vector3[] positionsExterieurs = new Vector3[]
        {
            new Vector3(-17.3f, 0, 30f),
            new Vector3(-17.3f, 0, -30f),
            new Vector3(0, 0, 30f),
            new Vector3(0, 0, -30f),
            new Vector3(-25.95f, 0, 15f),
            new Vector3(25.95f, 0, 15f),
            new Vector3(-25.95f, 0, -15f),
            new Vector3(25.95f, 0, -15f),
            new Vector3(34.6f, 0, 0),
            new Vector3(-34.6f, 0, 0),
            new Vector3(17.3f, 0, 30f),
            new Vector3(17.3f, 0, -30f)
        };

        // Instancier les hexagones extérieurs
        foreach (Vector3 position in positionsExterieurs)
        {
            GameObject hex = Instantiate(hexagonePrefab, centre.position + position, Quaternion.identity);
            hexagonesExterieurs.Add(hex);
        }
    }

    void StockerPositionsInitiales()
    {
        // Stocker les positions initiales des hexagones intérieurs
        foreach (GameObject hex in hexagonesInterieurs)
        {
            positionsInitialesInterieurs.Add(hex.transform.localPosition);
        }

        // Stocker les positions initiales des hexagones extérieurs
        foreach (GameObject hex in hexagonesExterieurs)
        {
            positionsInitialesExterieurs.Add(hex.transform.localPosition);
        }
    }

    void ReinitialiserPositions()
    {
        // Réinitialiser les positions des hexagones intérieurs
        for (int i = 0; i < hexagonesInterieurs.Count; i++)
        {
            hexagonesInterieurs[i].transform.localPosition = positionsInitialesInterieurs[i];
        }

        // Réinitialiser les positions des hexagones extérieurs
        for (int i = 0; i < hexagonesExterieurs.Count; i++)
        {
            hexagonesExterieurs[i].transform.localPosition = positionsInitialesExterieurs[i];
        }
    }

    IEnumerator AnimerVague()
    {
        for (int cycle = 0; cycle < nombreDeCycles; cycle++)
        {
            // Animer les hexagones intérieurs
            for (float t = 0; t < 1f; t += Time.deltaTime / vitesseVague)
            {
                float deplacement = Mathf.Sin(t * Mathf.PI * 2f) * amplitude;

                // Appliquer le mouvement aux hexagones intérieurs
                for (int i = 0; i < hexagonesInterieurs.Count; i++)
                {
                    hexagonesInterieurs[i].transform.localPosition = new Vector3(
                        hexagonesInterieurs[i].transform.localPosition.x,
                        deplacement,
                        hexagonesInterieurs[i].transform.localPosition.z
                    );
                }
                yield return null; // Attendre la frame suivante
            }

            // Délai avant d'animer les hexagones extérieurs
            yield return new WaitForSeconds(delaiPropagation);

            // Animer les hexagones extérieurs
            for (float t = 0; t < 1f; t += Time.deltaTime / vitesseVague)
            {
                float deplacement = Mathf.Sin(t * Mathf.PI * 2f) * amplitude;

                // Appliquer le mouvement aux hexagones extérieurs
                for (int i = 0; i < hexagonesExterieurs.Count; i++)
                {
                    hexagonesExterieurs[i].transform.localPosition = new Vector3(
                        hexagonesExterieurs[i].transform.localPosition.x,
                        deplacement,
                        hexagonesExterieurs[i].transform.localPosition.z
                    );
                }
                yield return null; // Attendre la frame suivante
            }
        }

        // Réinitialiser les positions après l'animation
        ReinitialiserPositions();
    }
}