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
        public readonly Dictionary<(int, int), Building> Buildings = new Dictionary<(int, int), Building>();
        
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
                        
                        //_____________________________________//
                        //______________Camera_________________//
                        //_____________________________________//
                        
                        CameraController controller = Camera.main!.GetComponent<CameraController>();
                        controller.TargetPosition.SetHard(new Vector3(xWorldCenterCo, 0, zWorldCenterCo));

                        controller.Zoom.targetValue = 100;
                        controller.VerticalAngle.targetValue = (float)(Math.PI/4f);
                        controller.VerticalAngle.currentValue = (float)(Math.PI/2f);
                        controller.Zoom.currentValue = 0;
                        
                        //_____________________________________//
                        //_____________________________________//
                        //_____________________________________//
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

        public void PlaceBuilding(int xMapIndex, int zMapIndex, int buildingType, uint ownerId)
        {
            if (Buildings.ContainsKey((xMapIndex, zMapIndex)))
            {
                DeleteBuilding(xMapIndex, zMapIndex);
            }
            
            GameObject flag = InstantiateFlag(xMapIndex, zMapIndex, ownerId);
            Building newBuilding = InstantiateBuilding(xMapIndex, zMapIndex, buildingType, ownerId, flag);
            
            Buildings[(xMapIndex, zMapIndex)] = newBuilding;

            ShowPopUpPlaceBuilding(newBuilding);
        }

        private void ShowPopUpPlaceBuilding(Building newBuilding)
        {
            switch (newBuilding.OwnerId)
            {
                case 0:
                    if (newBuilding is Extractor)
                    { 
                        PopUpManager.Instance.ShowPopUp($"Athènes has placed an {newBuilding.Name}.", 3);
                    }
                    else
                    {
                        PopUpManager.Instance.ShowPopUp($"Athènes has placed a {newBuilding.Name}.", 3);
                    }

                    break;
                case 1:
                    if (newBuilding is Extractor)
                    {
                        PopUpManager.Instance.ShowPopUp($"Sparte has placed an {newBuilding.Name}.", 3);
                    }
                    else
                    {
                        PopUpManager.Instance.ShowPopUp($"Sparte has placed a {newBuilding.Name}.", 3);
                    }

                    break;
                case 2:
                    if (newBuilding is Extractor)
                    {
                        PopUpManager.Instance.ShowPopUp($"Thèbes has placed an {newBuilding.Name}.", 3);
                    }
                    else
                    {
                        PopUpManager.Instance.ShowPopUp($"Thèbes has placed a {newBuilding.Name}.", 3);
                    }

                    break;
                case 3:
                    if (newBuilding is Extractor)
                    {
                        PopUpManager.Instance.ShowPopUp($"Corinthe has placed an {newBuilding.Name}.", 3);
                    }
                    else
                    {
                        PopUpManager.Instance.ShowPopUp($"Corinthe has placed a {newBuilding.Name}.", 3);
                    }

                    break;
                default:
                    if (newBuilding is Extractor)
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

        public GameObject InstantiateFlag(int xMapIndex, int zMapIndex, uint ownerId)
        {
            (float xWorldCenterCo, float zWorldCenterCo)  = StaticGridTools.MapIndexToWorldCenterCo(xMapIndex, zMapIndex);
            
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
            return flag;
        }
    
        public void DeleteBuilding(int x, int z)
        {
            if (Buildings.TryGetValue((x, z), out Building building))
            {
                GameObject buildingGameObject = building.GameObject;
                GameObject flag = building.Flag;
                Destroy(buildingGameObject);
                Destroy(flag);
                
                Buildings.Remove((x, z));
                Debug.Log($"Bâtiment supprimé à la position ({x}, {z}).");
                
            }
        }
    
        public Building InstantiateBuilding(int x, int z, int buildingType, uint ownerId,
            GameObject flag)
        {
            GameObject prefab = GetBuildingPrefab(buildingType);
            
            (float xWorldCenterCo, float zWorldCenterCo)  = StaticGridTools.MapIndexToWorldCenterCo(x, z);
            Vector3 positionKey = new Vector3(xWorldCenterCo, 0, zWorldCenterCo);
            
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
                    return new Temple("Temple Gold", "...", instantiate, (x, z), ownerId,
                        AttackType.Zeus,
                        $"____Type____ : Paralysis \n__Duration__ : 30 \n- Paralysis \n____Cost____ :\n- 25 Wood \n- 25 Stone \n- 50 Gold",
                        flag);
                case 10: // Temple Diamond
                    return new Temple("Temple Diamond", "...", instantiate, (x, z), ownerId,
                        AttackType.Athena,
                        $"____Type____ : Stealing \n__Duration__ : 60 \n- Paralysis \n____Cost____ ::\n- 30 Wood \n- 30 Stone \n- 25 Diamond",
                        flag);
                case 11: // Temple Obsidian
                    return new Temple("Temple Obsidian", "...", instantiate, (x, z), ownerId,
                        AttackType.Hades,
                        $"____Type____ : Destruction \n- Paralysis \n____Cost____ ::\n- 50 Wood \n- 50 Stone \n- 50 Obsidian",
                        flag);
                case 12: // Temple Water
                    return new Temple("Temple Water", "...", instantiate, (x, z), ownerId,
                        AttackType.Poseidon,
                        $"____Type____ : Paralysis \n__Duration__ : 15 \n- Paralysis \n____Cost____ ::\n- 75 Wood \n- 75 Stone \n- 50 Water",
                        flag);
                case 13: // Temple Vine
                    return new Temple("Temple Vine", "...", instantiate, (x, z), ownerId,
                        AttackType.Dionysos, 
                        $"____Type____ : Paralysis \n__Duration__ : 120 \n- Paralysis \n____Cost____ ::\n- 20 Wood \n- 20 Stone \n- 50 Wine \n- 5 Population",
                        flag);
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
            if (!Buildings.TryGetValue((x, z), out Building building)) 
                return null;
            GameObject flag = building.Flag;
            Buildings.Remove((x, z));
            Destroy(flag);
            Debug.Log($"Bâtiment fake supprimé à la position ({x}, {z}).");
            return building.GameObject;
        }
    }
    
}
