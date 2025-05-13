using UnityEngine;
using System.Collections.Generic;  

public class PlacementHexagones : MonoBehaviour
{
    public GameObject hexagonePrefab; // Le prefab de l'hexagone importé depuis Blender
    public Transform centre; // Le centre autour duquel les hexagones seront placés

    // Liste pour contenir tous les hexagones générés
    public List<GameObject> hexagonesGeneres = new List<GameObject>();

    void Start()
    {
        PlacerHexagones();
    }

    void PlacerHexagones()
    {
        // Positionner l'hexagone central à (0, 0, 0)
        GameObject centralHexagone = Instantiate(hexagonePrefab, centre.position, Quaternion.identity);
        hexagonesGeneres.Add(centralHexagone);  // Ajouter l'hexagone central à la liste

        // Positions du premier cercle de 6 hexagones
        Vector3[] premierCerclePositions = new Vector3[]
        {
            new Vector3(17.3f, 0, 0),
            new Vector3(-17.3f, 0, 0),
            new Vector3(8.65f, 0, 15f),
            new Vector3(-8.65f, 0, 15f),
            new Vector3(8.65f, 0, -15f),
            new Vector3(-8.65f, 0, -15f)
        };

        // Instancier les hexagones du premier cercle et les ajouter à la liste
        foreach (var position in premierCerclePositions)
        {
            GameObject hex = Instantiate(hexagonePrefab, position, Quaternion.identity);
            hexagonesGeneres.Add(hex);  // Ajouter chaque hexagone à la liste
        }

        // Positions du deuxième cercle de 12 hexagones
        Vector3[] deuxiemeCerclePositions = new Vector3[]
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

        // Instancier les hexagones du deuxième cercle et les ajouter à la liste
        foreach (var position in deuxiemeCerclePositions)
        {
            GameObject hex = Instantiate(hexagonePrefab, position, Quaternion.identity);
            hexagonesGeneres.Add(hex);  // Ajouter chaque hexagone à la liste
        }
    }
}