using System;
using System.Collections.Generic;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using ForServer;
using Networking.Common.Server;
using OlympusDedicatedServer.Components.Attack;
using PopUp;
using UnityEngine;

namespace BuildingsFolder
{
    public class BuildingsManager : MonoBehaviour
    {
        // Dictionnaire pour stocker les bâtiments (clé = (int, int), valeur = Building)
        public Dictionary<(int, int), Building> buildings = new Dictionary<(int, int), Building>();
        
        private Camera _mainCamera;
        // ____________________________________________________________________//
        // _______Références aux GameObjects des bâtiments_____________________//
        // ____________________________________________________________________//
        
        public GameObject prefabAgora;
        public GameObject prefabHouse;
        
        // Extractor
        public GameObject prefabExtractorWood;
        public GameObject prefabExtractorStone;
        public GameObject prefabExtractorGold;
        public GameObject prefabExtractorDiamond;
        public GameObject prefabExtractorObsidian;
        public GameObject prefabExtractorWater;
        public GameObject prefabExtractorVine;
        
        // Temple
        public GameObject prefabTempleGold;
        public GameObject prefabTempleDiamond;
        public GameObject prefabTempleObsidian;
        public GameObject prefabTempleWater;
        public GameObject prefabTempleVine;
        
        
        // Flag
        public GameObject prefabFlag1;
        public GameObject prefabFlag2;
        public GameObject prefabFlag3;
        public GameObject prefabFlag4;
        // ____________________________________________________________________//
        // ____________________________________________________________________//
        // ____________________________________________________________________//

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
                (connection, action) =>
                {
                    Debug.Log("[CLIENT]     : Receive 'ServerPlaceBuildingGameAction'");

                    if (action.BuildingId == 0 && action.OwnerId == ServerManager.PlayerId)
                    {
                        (float xWorldCenterCo, float zWorldCenterCo)  = StaticGridTools.MapIndexToWorldCenterCo(action.X, action.Y);
                        //_mainCamera.transform.position = new Vector3(xWorldCenterCo, _mainCamera.transform.position.y, zWorldCenterCo);
                        
                        _mainCamera.GetComponent<CameraController>().TargetPosition.Set(new Vector3(xWorldCenterCo, 0, zWorldCenterCo));
                    }
                    PlaceBuilding(action.X, action.Y, action.BuildingId, action.OwnerId);
                });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerRemoveBuildingGameAction>(
                (connection, action) =>
                {
                    Debug.Log("[CLIENT]     : Receive 'ServerRemoveBuildingGameAction'");
                    (float xPlaceBuilding, float zPlaceBuilding)  = StaticGridTools.MapIndexToWorldCenterCo(action.X, action.Y);
                    DeleteBuilding((int)xPlaceBuilding, (int)zPlaceBuilding);
                });
        }
    
        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//

        public void Start()
        {
            _mainCamera = Camera.main;
        }

        public void PlaceBuilding(int x, int z, int buildingType, uint ownerId)
        {
            if (buildings.ContainsKey((x, z)))
            {
                DeleteBuilding(x, z);
            }
            
            (float xWorldCenterCo, float zWorldCenterCo)  = StaticGridTools.MapIndexToWorldCenterCo(x, z);
            Vector3 positionKey = new Vector3(xWorldCenterCo, 0, zWorldCenterCo);

            GameObject flag = null;
            if (ownerId == 0)
            {
                flag= Instantiate(prefabFlag1, new Vector3(xWorldCenterCo, 0, zWorldCenterCo), Quaternion.identity);
                
            }
            if (ownerId == 1)
            {
                flag= Instantiate(prefabFlag2, new Vector3(xWorldCenterCo, 0, zWorldCenterCo), Quaternion.identity);
                
            }
            if (ownerId == 2)
            {
                flag= Instantiate(prefabFlag3, new Vector3(xWorldCenterCo, 0, zWorldCenterCo), Quaternion.identity);
                
            }
            if (ownerId == 3)
            {
                flag= Instantiate(prefabFlag4, new Vector3(xWorldCenterCo, 0, zWorldCenterCo), Quaternion.identity);
                
            }
            
            Building newBuilding = CreateBuilding(x, z, buildingType, positionKey, ownerId, flag);
            buildings[(x, z)] = newBuilding;

            ShowPopUpPlaceBuilding(newBuilding);
        }

        private void ShowPopUpPlaceBuilding(Building newBuilding)
        {
            switch (newBuilding.OwnerId)
            {
                case 0:
                    if (newBuilding is Agora || newBuilding is Extractor)
                    { 
                        PopUpManager.Instance.ShowPopUp($"Athènes has placed an {newBuilding.Name}.", 3);
                    }
                    else
                    {
                        PopUpManager.Instance.ShowPopUp($"Athènes has placed a {newBuilding.Name}.", 3);
                    }

                    break;
                case 1:
                    if (newBuilding is Agora || newBuilding is Extractor)
                    {
                        PopUpManager.Instance.ShowPopUp($"Sparte has placed an {newBuilding.Name}.", 3);
                    }
                    else
                    {
                        PopUpManager.Instance.ShowPopUp($"Sparte has placed a {newBuilding.Name}.", 3);
                    }

                    break;
                case 2:
                    if (newBuilding is Agora || newBuilding is Extractor)
                    {
                        PopUpManager.Instance.ShowPopUp($"Thèbes has placed an {newBuilding.Name}.", 3);
                    }
                    else
                    {
                        PopUpManager.Instance.ShowPopUp($"Thèbes has placed a {newBuilding.Name}.", 3);
                    }

                    break;
                case 3:
                    if (newBuilding is Agora || newBuilding is Extractor)
                    {
                        PopUpManager.Instance.ShowPopUp($"Corinthe has placed an {newBuilding.Name}.", 3);
                    }
                    else
                    {
                        PopUpManager.Instance.ShowPopUp($"Corinthe has placed a {newBuilding.Name}.", 3);
                    }

                    break;
                default:
                    if (newBuilding is Agora || newBuilding is Extractor)
                    {
                        PopUpManager.Instance.ShowPopUp($"Player number {ServerManager.PlayerId} has placed an {newBuilding.Name}.", 3);
                    }
                    else
                    {
                        PopUpManager.Instance.ShowPopUp($"Player number {ServerManager.PlayerId} has placed a {newBuilding.Name}.", 3);
                    }
                    break;
            }
        }
    
        public void DeleteBuilding(int x, int z)
        {
            if (buildings.TryGetValue((x, z), out Building building))
            {
                GameObject buildingGameObject = building.GameObject;
                GameObject flag = building.Flag;
                Destroy(buildingGameObject);
                Destroy(flag);
                
                buildings.Remove((x, z));
                Debug.Log($"Bâtiment supprimé à la position ({x}, {z}).");
                
            }
        }
    
        public Building CreateBuilding(int x, int z, int buildingType, Vector3 positionKey, uint ownerId,
            GameObject flag)
        {
            GameObject prefab = GetBuildingPrefab(buildingType);
            //GameObject instantiate = Instantiate(prefab, positionKey, Quaternion.identity);
            
            int rotationAngle = StaticGridTools.MapIndexToRotation(x, z, ServerManager.Seed, (int)ServerManager.MapWidth);
            GameObject instantiate = Instantiate(prefab, positionKey, Quaternion.Euler(0, rotationAngle, 0));
            
            switch (buildingType)
            {
                case 0: // Agora
                    return new Agora("Agora", "", instantiate, (x, z), ownerId, flag);
                case 1: // House
                    return new House("House", "", instantiate, (x, z), ownerId,flag);
                case 2: // Extractor Wood
                    return new Extractor("Extractor Wood", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Wood,flag);
                case 3: // Extractor Stone
                    return new Extractor("Extractor Stone", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Stone, flag);
                case 4: // Extractor Gold
                    return new Extractor("Extractor Gold", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Gold, flag);
                case 5: // Extractor Diamond
                    return new Extractor("Extractor Diamond", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Diamond, flag);
                case 6: // Extractor Obsidian
                    return new Extractor("Extractor Diamond", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Obsidian, flag);
                case 7: // Extractor Water
                    return new Extractor("Extractor Water", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Water, flag);
                case 8: // Extractor Vine
                    return new Extractor("Extractor Vine", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Vine, flag);
                case 9: // Temple Gold
                    return new Temple("Temple Gold", "", instantiate, (x, z), ownerId,AttackType.Zeus, flag);
                case 10: // Temple Diamond
                    return new Temple("Temple Diamond", "", instantiate, (x, z), ownerId,AttackType.Athena, flag);
                case 11: // Temple Obsidian
                    return new Temple("Temple Obsidian", "", instantiate, (x, z), ownerId,AttackType.Hades, flag);
                case 12: // Temple Water
                    return new Temple("Temple Water", "", instantiate, (x, z), ownerId,AttackType.Poseidon, flag);
                case 13: // Temple Vine
                    return new Temple("Temple Vine", "", instantiate, (x, z), ownerId,AttackType.Dionysos, flag);
                default:
                    return null;
            }
        }
        
        public GameObject GetBuildingPrefab(int buildingType)
        {
            switch (buildingType)
            {
                case 0: // Agora
                    return prefabAgora;
                case 1: // House
                    return prefabHouse;
                case 2: // Extractor Wood
                    return prefabExtractorWood;
                case 3: // Extractor Stone
                    return prefabExtractorStone;
                case 4: // Extractor Gold
                    return prefabExtractorGold;
                case 5: // Extractor Diamond
                    return prefabExtractorDiamond;
                case 6: // Extractor Obsidian
                    return prefabExtractorObsidian;
                case 7: // Extractor Water
                    return prefabExtractorWater;
                case 8: // Extractor Vine
                    return prefabExtractorVine;
                case 9: // Temple Gold
                    return prefabTempleGold;
                case 10: // Temple Diamond
                    return prefabTempleDiamond;
                case 11: // Temple Obsidian
                    return prefabTempleObsidian;
                case 12: // Temple Water
                    return prefabTempleWater;
                case 13: // Temple Vine
                    return prefabTempleVine;
                default:
                    return null;
            }
        }
        
        public GameObject FakeDeleteBuilding(int x, int z)
        {
            if (!buildings.TryGetValue((x, z), out Building building)) 
                return null;
            GameObject flag = building.Flag;
            buildings.Remove((x, z));
            Destroy(flag);
            Debug.Log($"Bâtiment fake supprimé à la position ({x}, {z}).");
            return building.GameObject;
        }
    }
    
}
