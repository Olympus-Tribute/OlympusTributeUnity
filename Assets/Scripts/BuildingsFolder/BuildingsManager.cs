using System;
using System.Collections;
using System.Collections.Generic;
using Attacks;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using Grid;
using Networking.Common.Server;
using OlympusDedicatedServer.Components.Attack;
using OlympusDedicatedServer.Components.WorldComp;
using PopUp;
using Sound;
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
        
        // Audio
        AudioManager audioManager;
        
        private AttacksManager _attacksManager;
        
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
            _map = FindFirstObjectByType<GridGenerator>().MapGenerator;
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }

        //___________________________________________________________//
        //_________________________For Multi_________________________//
        //___________________________________________________________//
    
        void OnEnable()
        {
            _attacksManager = FindFirstObjectByType<AttacksManager>();
            
            Debug.Log("Starting BuildingsManager...");
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerPlaceBuildingGameAction>(
                (connection, action) =>
                {
                    Debug.Log("[CLIENT]     : Receive 'ServerPlaceBuildingGameAction'");

                    if (action.BuildingId == 0 && action.OwnerId == GameConstants.PlayerId)
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

            //OwnerManager = new OwnerManager(GameConstants.MapWidth, GameConstants.MapHeight, _map);
            OwnerManager = FindFirstObjectByType<OwnerManager>();
        }
    
        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//

        private void AddBuildingOwner(Building building)
        {
            SetOwners(building.Position.Item1, building.Position.Item2, building.Range, building.OwnerId);
        }

        public void SetOwners(int tx, int ty, int range, uint ownerId)
        {
            foreach (var (x, y) in WorldCoordinates.FindTilesOfNRadiusIncludingMe(tx, ty, GameConstants.MapWidth, GameConstants.MapHeight, range))
            {
                OwnerManager.AddOwner(x, y, ownerId);
            }
        }

        private void RemoveBuildingOwner(Building building)
        {
            if (_attacksManager.dictAthena.TryGetValue((building.Position.Item1, building.Position.Item2), out AttacksManager.AthenaInfo info))
            {
                UnsetOwners(building.Position.Item1, building.Position.Item2, building.Range, info.AttackerId);
                _attacksManager.dictAthena.Remove((building.Position.Item1, building.Position.Item2));
            }
            else
            {
                UnsetOwners(building.Position.Item1, building.Position.Item2, building.Range, building.OwnerId);
            }
        }

        public void UnsetOwners(int tx, int ty, int range, uint ownerId)
        {
            foreach (var (x, y) in WorldCoordinates.FindTilesOfNRadiusIncludingMe(tx, ty, GameConstants.MapWidth, GameConstants.MapHeight, range))
            {
                OwnerManager.RemoveOwner(x, y, ownerId);
            }
        }

        public void PlaceBuilding(int xMapIndex, int zMapIndex, int buildingType, uint ownerId)
        {
            if (Buildings.ContainsKey((xMapIndex, zMapIndex)))
            {
                DeleteBuilding(xMapIndex, zMapIndex);
            }
            
            Building newBuilding = InstantiateBuilding(xMapIndex, zMapIndex, buildingType, ownerId);
            
            Buildings[(xMapIndex, zMapIndex)] = newBuilding;

            AddBuildingOwner(newBuilding);

            ShowPopUpPlaceBuilding(newBuilding);
            audioManager.PlayConstruction();
        }
        
        public void DeleteBuilding(int x, int z)
        {
            if (Buildings.TryGetValue((x, z), out Building building))
            {
                GameObject buildingGameObject = building.GameObject;
                Destroy(buildingGameObject);
                
                Buildings.Remove((x, z));
                RemoveBuildingOwner(building);
                
                Debug.Log($"Bâtiment supprimé à la position ({x}, {z}).");
                
            }
        }
        
        public GameObject FakeDeleteBuilding(int x, int z)
        {
            if (!Buildings.TryGetValue((x, z), out Building building)) 
                return null;
            Buildings.Remove((x, z));
            RemoveBuildingOwner(building);
            Debug.Log($"Bâtiment fake supprimé à la position ({x}, {z}).");
            return building.GameObject;
        }
     
        
        public Building InstantiateBuilding(int x, int z, int buildingType, uint ownerId)
        {
            GameObject prefab = GetBuildingPrefab(buildingType);
            
            (float xWorldCenterCo, float zWorldCenterCo)  = StaticGridTools.MapIndexToWorldCenterCo(x, z);
            Vector3 positionKey = new Vector3(xWorldCenterCo, 0, zWorldCenterCo);
            
            int rotationAngle = StaticGridTools.MapIndexToRotation(x, z, GameConstants.Seed, (int)GameConstants.MapWidth);
            GameObject instantiate = Instantiate(prefab, positionKey, Quaternion.Euler(0, rotationAngle, 0));
            
            switch (buildingType)
            {
                case 0: // Agora
                    return new Agora("Agora", "", instantiate, (x, z), ownerId);
                case 1: // House
                    return new House("House", "", instantiate, (x, z), ownerId);
                case 2: // Extractor Wood
                    return new Extractor("Extractor Wood", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Wood);
                case 3: // Extractor Stone
                    return new Extractor("Extractor Stone", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Stone);
                case 4: // Extractor Gold
                    return new Extractor("Extractor Gold", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Gold);
                case 5: // Extractor Diamond
                    return new Extractor("Extractor Diamond", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Diamond);
                case 6: // Extractor Obsidian
                    return new Extractor("Extractor Diamond", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Obsidian);
                case 7: // Extractor Water
                    return new Extractor("Extractor Water", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Water);
                case 8: // Extractor Vine
                    return new Extractor("Extractor Vine", "", instantiate, (x, z), ownerId, Extractor.ResourceType.Vine);
                case 9: // Temple Gold
                    return new Temple("Temple Gold", "...", instantiate, (x, z), ownerId,
                        AttackType.Zeus,
                        attackPrice: AllPrices.PriceZeusAttackDict,
                        descriptionAttack: $"____Type____ :\n- Paralysis \n \n__Duration__ :\n- 30 \n \n____Cost____ :\n- 25 Wood \n- 25 Stone \n- 50 Gold");
                case 10: // Temple Diamond
                    return new Temple("Temple Diamond", "...", instantiate, (x, z), ownerId,
                        AttackType.Athena,
                        attackPrice: AllPrices.PriceAthenaAttackDict,
                        $"____Type____ :\n- Stealing \n \n__Duration__ :\n- 60 \n \n____Cost____ :\n- 30 Wood \n- 30 Stone \n- 25 Diamond");
                case 11: // Temple Obsidian
                    return new Temple("Temple Obsidian", "...", instantiate, (x, z), ownerId,
                        AttackType.Hades,
                        attackPrice: AllPrices.PriceHadesAttackDict,
                        $"____Type____ :\n- Destruction \n \n____Cost____ :\n- 50 Wood \n- 50 Stone \n- 50 Obsidian");
                case 12: // Temple Water
                    return new Temple("Temple Water", "...", instantiate, (x, z), ownerId,
                        AttackType.Poseidon,
                        attackPrice: AllPrices.PricePoseidonAttackDict,
                        $"____Type____ :\n- Paralysis \n \n__Duration__ :\n- 15 \n \n____Cost____ :\n- 75 Wood \n- 75 Stone \n- 50 Water");
                case 13: // Temple Vine
                    return new Temple("Temple Vine", "...", instantiate, (x, z), ownerId,
                        AttackType.Dionysos,
                        attackPrice: AllPrices.PriceDyonisosAttackDict,
                        $"____Type____ :\n- Paralysis \n \n__Duration__ :\n- 120 \n \n____Cost____ :\n- 20 Wood \n- 20 Stone \n- 50 Wine \n- 5 Population");
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
        
        private void ShowPopUpPlaceBuilding(Building newBuilding)
        {
            
            string article;
            if (newBuilding is Extractor)
            {
                article = "an";
            }
            else
            {
                article = "a";
            }
            
            PopUpManager.Instance.ShowPopUp($"{OwnersMaterial.GetName(newBuilding.OwnerId)} has placed {article} {newBuilding.Name}.", 3);
        }
        
        private readonly Dictionary<(int x, int z), float> _disabledUntil = new();

        public void DisableBuilding(int x, int z, float duration, GameObject waterPrefab = null)
        {
            if (!Buildings.TryGetValue((x, z), out var building)) return;
            var go = building.GameObject;
            if (go == null) return;

            float currentTime = Time.time;
            float disableUntil = currentTime + duration;

            if (_disabledUntil.TryGetValue((x, z), out float existingUntil))
            {
                if (disableUntil <= existingUntil)
                    return; 

                _disabledUntil[(x, z)] = disableUntil;
            }
            else
            {
                _disabledUntil.Add((x, z), disableUntil);
                go.SetActive(false);
            }

            GameObject waterInstance = null;
            if (waterPrefab != null)
            {
                waterInstance = Instantiate(waterPrefab, go.transform.position, Quaternion.identity);
            }

            StartCoroutine(ReenableBuildingCoroutine(x, z, go, waterInstance, disableUntil - currentTime));
        }

        private IEnumerator ReenableBuildingCoroutine(int x, int z, GameObject go, GameObject waterGO, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (_disabledUntil.TryGetValue((x, z), out float until) && Time.time >= until)
            {
                _disabledUntil.Remove((x, z));
                if (go != null) go.SetActive(true);
                if (waterGO != null) Destroy(waterGO);
            }
        }


    }
    
}
