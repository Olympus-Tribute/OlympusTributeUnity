using UnityEngine;

namespace Buildings
{
    public class BuildingSpawner : MonoBehaviour
    {
        // Références aux deux prefabs de bâtiments
        public GameObject buildingPrefab1;
        public GameObject buildingPrefab2;
        
        //

        // Méthode pour placer un bâtiment à la position (x, y) avec le type de bâtiment choisi
        public void PlaceBuilding(float x, float y, int buildingType)
        {
            // Crée un vecteur de position avec X, Y, et Z fixé à 0
            Vector3 position = new Vector3(x, 0, y);

            if (buildingType == 1)
            {
                GameObject buildingToInstantiate = buildingPrefab1;
                // Crée une nouvelle instance du prefab sélectionné à la position donnée
                Instantiate(buildingToInstantiate, position, Quaternion.identity);
            }
            if (buildingType == 2)
            {
                GameObject buildingToInstantiate = buildingPrefab2;
                // Crée une nouvelle instance du prefab sélectionné à la position donnée
                Instantiate(buildingToInstantiate, position, Quaternion.identity);
            }
        }
        
        
    }
}