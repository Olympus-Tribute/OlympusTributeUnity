using System;
using System.Collections.Generic;
using ForNetwork;
using Networking.Common.Server;
using UnityEngine;

public class BuildingsManager : MonoBehaviour
{
    // Dictionnaire pour stocker les bâtiments (clé = position sous forme de Vector3, valeur = GameObject)
    private Dictionary<Vector3, GameObject> buildings = new Dictionary<Vector3, GameObject>();

    // Références aux GameObjects des bâtiments
    public GameObject buildingPrefab1;
    public GameObject buildingPrefab2;
    public GameObject buildingPrefab3;
    public GameObject buildingPrefab4;

    public Network Network = Network.Instance;
    public bool networkActive;
    //___________________________________________________________//
    //_________________________For Multi_________________________//
    //___________________________________________________________//

    private void Awake()
    {
        /*
        if (Network.Instance == null)
        {
            Debug.LogError("Network.Instance n'est pas initialisé !");
            return;
        }
        */
        //networkActive = Network.Instance.networkActive;
        networkActive = false;
    }

    void OnEnable()
    {
        if (networkActive)
        {
            Debug.Log("Starting by BuildingsManager");
            if (Network.Instance.Proxy != null)
            {
                Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerPlaceBuildingGameAction>(
                    (connection, action) => { PlaceBuilding(action.X * 5, action.Y * 5, action.NumBuildings); });
            }
        }
    }
    
    //___________________________________________________________//
    //___________________________________________________________//
    //___________________________________________________________//

    public bool PlaceBuilding(float x, float z, int buildingType)
    {
        Vector3 positionKey = new Vector3(x, 0, z);

        // Vérifie si un bâtiment existe déjà à cette position
        if (buildings.ContainsKey(positionKey))
        {
            Debug.LogWarning("Un bâtiment existe déjà à cette position !");
            return false;
        }

        // Récupère le prefab correspondant
        GameObject buildingToInstantiate = GetBuildingPrefab(buildingType);
        if (buildingToInstantiate == null)
        {
            Debug.LogWarning("Type de bâtiment invalide !");
            return false;
        }

        // Instancie le bâtiment
        GameObject newBuilding = Instantiate(buildingToInstantiate, positionKey, Quaternion.identity);

        // Ajoute le bâtiment au dictionnaire
        buildings[positionKey] = newBuilding;
        Debug.Log($"Bâtiment ajouté à la position ({x}, {z}).");
        return true;
    }
    
    public void DeleteBuilding(float x, float z)
    {
        Vector3 positionKey = new Vector3(x, 0, z);

        // Vérifie si un bâtiment existe à cette position
        if (buildings.TryGetValue(positionKey, out GameObject building))
        {
            // Supprime le bâtiment de la scène et du dictionnaire
            Destroy(building);
            buildings.Remove(positionKey);
            Debug.Log($"Bâtiment supprimé à la position ({x}, {z}).");
        }
        else
        {
            Debug.LogWarning($"Aucun bâtiment trouvé à la position ({x}, {z}).");
        }
    }
    
    public GameObject GetBuildingPrefab(int buildingType)
    {
        switch (buildingType)
        {
            case 1:
                return buildingPrefab1;
            case 2:
                return buildingPrefab2;
            case 3:
                return buildingPrefab3;
            case 4:
                return buildingPrefab4;
            default:
                return null;
        }
    }
}
