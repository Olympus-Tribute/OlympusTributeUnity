using System.Collections.Generic;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using ForServer;
using Networking.Common.Server;
using UnityEngine;

namespace BuildingsFolder
{
    public class BuildingsManager : MonoBehaviour
    {
        // Dictionnaire pour stocker les bâtiments (clé = (int, int), valeur = Building)
        public Dictionary<(int, int), Building> buildings = new Dictionary<(int, int), Building>();

        // Références aux GameObjects des bâtiments
        public GameObject prefabAgora;
        public GameObject prefabExtractorWood;
        public GameObject prefabExtractorStone;
        public GameObject prefabExtractorGold;
        public GameObject prefabExtractorDiamond;

        private void Awake()
        {
            if (Network.Instance == null)
            {
                Debug.LogWarning("Network.Instance is not initialized!");
                return;
            }
        }

        //___________________________________________________________//
        //_________________________For Multi_________________________//
        //___________________________________________________________//
    
        void OnEnable()
        {
            Debug.Log("Starting BuildingsManager...");
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerPlaceBuildingGameAction>(
                    (connection, action) => { PlaceBuilding(action.X * 5, action.Y * 5, action.BuildingId, action.OwnerId); });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerRemoveBuildingGameAction>(
                (connection, action) => { DeleteBuilding(action.X * 5, action.Y * 5); });
        }
    
        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//
    

        public void PlaceBuilding(int x, int z, int buildingType, uint ownerId)
        {
            Vector3 positionKey = new Vector3(x, 0, z);
            
            if (buildings.ContainsKey((x, z)))
            {
                DeleteBuilding(x, z);
            }
            
            Building newBuilding = CreateBuilding(x, z, buildingType, positionKey, ownerId);
            
            // Ajoute le bâtiment au dictionnaire
            buildings[(x, z)] = newBuilding;
            Debug.Log($"Bâtiment ajouté à la position ({x}, {z}).");
        }
    
        public void DeleteBuilding(int x, int z)
        {
            if (buildings.TryGetValue((x, z), out Building building))
            {
                GameObject buildingGameObject = building.GameObject;
                Destroy(buildingGameObject);
                
                buildings.Remove((x, z));
                Debug.Log($"Bâtiment supprimé à la position ({x}, {z}).");
            }
            else
            {
                Debug.LogWarning($"Aucun bâtiment trouvé à la position ({x}, {z}).");
            }
        }
    
        public Building CreateBuilding(int x, int z, int buildingType, Vector3 positionKey, uint ownerId)
        {
            GameObject prefab = GetBuildingPrefab(buildingType);
            GameObject instantiate = Instantiate(prefab, positionKey, Quaternion.identity);
            switch (buildingType)
            {
                case 0: // Extracteur Wood
                    return new Extractor("extracteur wood", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Wood);
                case 1: // Extracteur Stone
                    return new Extractor("Agora", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Stone);
                case 2: // Extracteur Gold
                    return new Extractor("Agora", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Gold);
                case 3: // Extracteur Diamond
                    return new Extractor("Agora", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Diamond);
                case 4: // Agora
                    return new Agora("Agora", "", instantiate, (x, z), ownerId);
                default:
                    return null;
            }
        }
        
        public GameObject GetBuildingPrefab(int buildingType)
        {
            switch (buildingType)
            {
                case 0:
                    return prefabExtractorWood;
                case 1:
                    return prefabExtractorStone;
                case 2:
                    return prefabExtractorGold;
                case 3:
                    return prefabExtractorDiamond;
                case 4: // Agora
                    return prefabAgora;
                default:
                    return null;
            }
        }
    }
}
