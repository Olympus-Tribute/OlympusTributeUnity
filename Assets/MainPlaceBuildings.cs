using UnityEngine;
using Buildings;
using Unity.VisualScripting; // Importation du namespace pour utiliser BuildingSpawner

public class MainPlaceBuildings : MonoBehaviour
{
    // Référence au script BuildingSpawner, assigné dans l'inspecteur
    public BuildingSpawner buildingSpawner;

    void Update()
    {
        // Si on appuie sur la touche espace, un bâtiment est placé à une position aléatoire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("hi");
            float x = Random.Range(10f, 100f);  // Coordonnée aléatoire en X
            float y = Random.Range(10f, 100f);  // Coordonnée aléatoire en Y

            // Appel de la méthode pour placer un bâtiment à la position (x, y)
            buildingSpawner.PlaceBuilding(x, y, 1);
            //buildingSpawner.PlaceBuilding(y, x, 2);
            
        }
    }
}