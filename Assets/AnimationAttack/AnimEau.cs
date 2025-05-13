using UnityEngine;
using System.Collections;
public class WaveDeformation : MonoBehaviour
{
    public float amplitude = 1.0f; // Hauteur de la vague
    public float vitesseVague = 2.0f; // Vitesse de la vague
    public float longueurOnde = 10.0f; // Longueur d'onde de la vague
    public float delaiPropagation = 0.5f; // Délai entre le centre et l'extérieur

    private Mesh mesh;
    private Vector3[] verticesOrigine;
    private bool vagueEnCours = false;

    void Start()
    {
        // Récupérer le mesh du GameObject
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        // Créer une copie unique du mesh
        mesh = Instantiate(meshFilter.sharedMesh) as Mesh;
        meshFilter.mesh = mesh; // Assigner la copie unique au MeshFilter

        // Stocker les vertices d'origine
        verticesOrigine = mesh.vertices;
    }

    void Update()
    {
        // Démarrer la vague si elle n'est pas déjà en cours
        if (!vagueEnCours)
        {
            StartCoroutine(AnimerVague());
        }
    }

    IEnumerator AnimerVague()
    {
        vagueEnCours = true;

        // Animer le centre (premier vertex)
        yield return DeformerVertices(0, 1); // Le centre est supposé être le premier vertex

        // Animer les hexagones intérieurs
        yield return DeformerVertices(1, 6); // Hexagones intérieurs (indices 1 à 6)

        // Animer les hexagones extérieurs
        yield return DeformerVertices(7, verticesOrigine.Length - 1); // Hexagones extérieurs (indices 7 à N)

        // Réinitialiser les positions après l'animation
        ReinitialiserPositions();

        vagueEnCours = false;
    }

    IEnumerator DeformerVertices(int debut, int fin)
    {
        for (float t = 0; t < 1f; t += Time.deltaTime / vitesseVague)
        {
            Vector3[] vertices = mesh.vertices;

            for (int i = debut; i <= fin; i++)
            {
                // Calculer la distance par rapport au centre
                float distance = Vector3.Distance(verticesOrigine[i], Vector3.zero);

                // Appliquer une fonction sinusoïdale pour créer la vague
                float deplacement = Mathf.Sin((distance - t * vitesseVague) * Mathf.PI * 2f / longueurOnde) * amplitude;

                // Déplacer le vertex verticalement
                vertices[i].y = verticesOrigine[i].y + deplacement;
            }

            // Mettre à jour les vertices du mesh
            mesh.vertices = vertices;
            mesh.RecalculateNormals(); // Recalculer les normales pour l'éclairage

            yield return null; // Attendre la frame suivante
        }
    }

    void ReinitialiserPositions()
    {
        // Réinitialiser les vertices à leur position d'origine
        mesh.vertices = verticesOrigine;
        mesh.RecalculateNormals(); // Recalculer les normales
    }
}