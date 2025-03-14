using System;
using System.Collections.Generic;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using ForServer;
using Grid;
using Networking.Common.Server;
using OlympusDedicatedServer.Components.Attack;
using OlympusDedicatedServer.Components.WorldComp;
using PopUp;
using UnityEngine;

namespace BuildingsFolder
{
    public class BuildingsManager : MonoBehaviour
    {
        // Dictionnaire pour stocker les bâtiments (clé = (int, int), valeur = Building)
        public readonly Dictionary<(int, int), Building> Buildings = new Dictionary<(int, int), Building>();
        
        public OwnerManager OwnerManager;
        private HexMapGenerator _map;
        
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
            _map = FindObjectOfType<GridGenerator>().MapGenerator;
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
            
            OwnerManager = new OwnerManager(ServerManager.MapWidth, ServerManager.MapHeight);
        }
    
        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//

        private void AddBuildingOwner(Building building)
        {
            foreach (var (x, y) in WorldCoordinates.FindTilesOfNRadiusIncludingMe(building.Position.Item1, building.Position.Item2, ServerManager.MapWidth, ServerManager.MapHeight, building.Range))
            {
                OwnerManager.AddOwner(x, y, building.OwnerId);
            }
        }
        
        private void RemoveBuildingOwner(Building building)
        {
            foreach (var (x, y) in WorldCoordinates.FindTilesOfNRadiusIncludingMe(building.Position.Item1, building.Position.Item2, ServerManager.MapWidth, ServerManager.MapHeight, building.Range))
            {
                OwnerManager.RemoveOwner(x, y, building.OwnerId);
            }
        }
        
        public void PlaceBuilding(int xMapIndex, int zMapIndex, int buildingType, uint ownerId)
        {
            if (Buildings.ContainsKey((xMapIndex, zMapIndex)))
            {
                DeleteBuilding(xMapIndex, zMapIndex);
            }
            
            GameObject flag = InstantiateFlag(xMapIndex, zMapIndex, ownerId);
            Building newBuilding = InstantiateBuilding(xMapIndex, zMapIndex, buildingType, ownerId, flag);
            
            Buildings[(xMapIndex, zMapIndex)] = newBuilding;

            AddBuildingOwner(newBuilding);
            
            ShowPopUpPlaceBuilding(newBuilding);
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
                RemoveBuildingOwner(building);
                
                Debug.Log($"Bâtiment supprimé à la position ({x}, {z}).");
                
            }
        }
        
        public GameObject FakeDeleteBuilding(int x, int z)
        {
            if (!Buildings.TryGetValue((x, z), out Building building)) 
                return null;
            GameObject flag = building.Flag;
            Buildings.Remove((x, z));
            Destroy(flag);
            RemoveBuildingOwner(building);
            Debug.Log($"Bâtiment fake supprimé à la position ({x}, {z}).");
            return building.GameObject;
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
                        $"____Type____ :\n- Paralysis \n \n__Duration__ :\n- 30 \n \n____Cost____ :\n- 25 Wood \n- 25 Stone \n- 50 Gold",
                        flag);
                case 10: // Temple Diamond
                    return new Temple("Temple Diamond", "...", instantiate, (x, z), ownerId,
                        AttackType.Athena,
                        $"____Type____ :\n- Stealing \n \n__Duration__ :\n- 60 \n \n____Cost____ :\n- 30 Wood \n- 30 Stone \n- 25 Diamond",
                        flag);
                case 11: // Temple Obsidian
                    return new Temple("Temple Obsidian", "...", instantiate, (x, z), ownerId,
                        AttackType.Hades,
                        $"____Type____ :\n- Destruction \n \n____Cost____ :\n- 50 Wood \n- 50 Stone \n- 50 Obsidian",
                        flag);
                case 12: // Temple Water
                    return new Temple("Temple Water", "...", instantiate, (x, z), ownerId,
                        AttackType.Poseidon,
                        $"____Type____ :\n- Paralysis \n \n__Duration__ :\n- 15 \n \n____Cost____ :\n- 75 Wood \n- 75 Stone \n- 50 Water",
                        flag);
                case 13: // Temple Vine
                    return new Temple("Temple Vine", "...", instantiate, (x, z), ownerId,
                        AttackType.Dionysos, 
                        $"____Type____ :\n- Paralysis \n \n__Duration__ :\n- 120 \n \n____Cost____ :\n- 20 Wood \n- 20 Stone \n- 50 Wine \n- 5 Population",
                        flag);
                default:
                    return null;
            }
        }
        
        private GameObject InstantiateFlag(int xMapIndex, int zMapIndex, uint ownerId)
        {
            (float xWorldCenterCo, float zWorldCenterCo)  = StaticGridTools.MapIndexToWorldCenterCo(xMapIndex, zMapIndex);
            GameObject prefabFlag;
            switch (ownerId)
            {
                case 0:
                    prefabFlag = prefabFlag1;
                    break;
                case 1:
                    prefabFlag = prefabFlag2;
                    break;
                case 2:
                    prefabFlag = prefabFlag3;
                    break;
                case 3:
                    prefabFlag = prefabFlag4;
                    break;
                default:
                    prefabFlag = prefabFlag1;
                    break;
            }

            GameObject flag = Instantiate(prefabFlag, new Vector3(xWorldCenterCo, 0, zWorldCenterCo), Quaternion.identity);
            return flag;
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
        
        private void ShowPopUpPlaceBuilding(Building newBuilding)
        {
            string city;

            if (ServerManager.City.Length <= ServerManager.PlayerId )
            {
                city = $"Player number {ServerManager.PlayerId}";
            }
            else
            {
                city = ServerManager.City[ServerManager.PlayerId];
            }

            string article;
            if (newBuilding is Extractor)
            {
                article = "an";
            }
            else
            {
                article = "a";
            }
            
            PopUpManager.Instance.ShowPopUp($"{city} has placed {article} {newBuilding.Name}.", 3);
        }

        
        
        
    }
    
}
