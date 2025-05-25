using System.Collections.Generic;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using Menus.MenusInGame;
using OlympusDedicatedServer.Components.Attack;
using PopUp;
using Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Attacks
{
    public class AttacksMenu : MonoBehaviour
    {
        public GameObject menuUIAttack;
        public TMP_Text titleInfoAttack;
        public TMP_Text infoAttackPrice;
        
        public GridLayoutGroup gridLayoutGroup;
        public GameObject prefabLine;
        public List<GameObject> listPrefabs;

        public Image imageAttackType;
        
        //______________________________________________//
        //_____________________SPRITE___________________//
        //______________________________________________//
        
        // ICON
        public Sprite iconPopulationSprite;
        public Sprite iconWoodSprite;
        public Sprite iconStoneSprite;
        public Sprite iconGoldSprite;
        public Sprite iconDiamondSprite;
        public Sprite iconObsidianSprite;
        public Sprite iconWaterSprite;
        public Sprite iconVineSprite;
        
        // Image Description
        public Sprite imageAttackZeus;
        public Sprite imageAttackAthena;
        public Sprite imageAttackHades;
        public Sprite imageAttackPoseidon;
        public Sprite imageAttackDionysos;
        
        //______________________________________________//
        //______________________________________________//
        //______________________________________________//
        
        private AttacksManager _attacksManager;
        private BuildingsManager _buildingsManager;
        private Dictionary<ResourceType, Sprite> _resourceSprite;
        private Dictionary<AttackType, Sprite> _attackTypeSprite;
        private uint _compteurMouse;


        public void Start()
        {
            _attacksManager = FindFirstObjectByType<AttacksManager>();
            _buildingsManager = FindFirstObjectByType<BuildingsManager>();
            
            menuUIAttack.SetActive(false);
            _compteurMouse = 0;
            
            _resourceSprite = new Dictionary<ResourceType, Sprite>
            {
                { ResourceType.Population, iconPopulationSprite},
                { ResourceType.Wood, iconWoodSprite },
                { ResourceType.Stone, iconStoneSprite },
                { ResourceType.Gold, iconGoldSprite },
                { ResourceType.Diamond, iconDiamondSprite },
                { ResourceType.Obsidian, iconObsidianSprite },
                { ResourceType.Water, iconWaterSprite },
                { ResourceType.Vine, iconVineSprite },
            };
            
            _attackTypeSprite = new Dictionary<AttackType, Sprite>
            {
                { AttackType.Zeus, imageAttackZeus },
                { AttackType.Athena, imageAttackAthena },
                { AttackType.Hades, imageAttackHades },
                { AttackType.Poseidon, imageAttackPoseidon },
                { AttackType.Dionysos, imageAttackDionysos },
            };

            List<GameObject> prefabs = new List<GameObject>();
            for (int i = 0; i < 4; i++)
            {
                GameObject line = Instantiate(prefabLine, gridLayoutGroup.transform);
                line.SetActive(false);
                prefabs.Add(line);
            }
            listPrefabs = prefabs;
        }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mapIndex = MousePositionTracker.Instance.GetMouseMapIndexCo();
                if (!mapIndex.HasValue)
                {
                    return;
                }
                (int x, int z) = mapIndex.Value;
                Attack(x, z);
            }
        }

        private void Attack(int x, int y)
        {
            
            if (_attacksManager.Temple is null)
            {
                if (_buildingsManager.Buildings.TryGetValue((x, y), out var targetBuilding) && targetBuilding is Temple targetTemple &&
                    (_attacksManager.dictAthena.TryGetValue((x,y),out var info) ? info.AttackerId == GameConstants.PlayerId : targetBuilding.OwnerId == GameConstants.PlayerId))
                {
                    _compteurMouse += 1;
                    
                    menuUIAttack.SetActive(true);
                    
                    // Un temple a été ciblé
                    _attacksManager.Temple = targetTemple;
                    titleInfoAttack.text = $"Attack : {targetTemple.Name}";
                    
                    // Associer le bon Sprite Asset TMP avant d'afficher le texte
                    infoAttackPrice.spriteAsset = TMP_Settings.defaultSpriteAsset;
                    //infoAttackPrice.text = targetTemple.DescriptionAttack;
                    CreateGridLayoutGroup(targetTemple.AttackPrice);
                    CreateImageTypeAttack(targetTemple.AttackType);
                }
            }
            else if (_compteurMouse >= 2)
            {
                _attacksManager.Temple.SendAttack(x, y);
                _attacksManager.Temple = null;
                _compteurMouse = 0;
            }
        }

        public void ButtonAttack()
        {
            menuUIAttack.SetActive(false);
            PopUpManager.Instance.ShowPopUp("Select a building to attack", 5);
            _compteurMouse += 1;
        }
        
        public void ButtonQuit()
        {
            menuUIAttack.SetActive(false);
            _attacksManager.Temple = null;
        }
        

        private void CreateGridLayoutGroup(Dictionary<ResourceType, int> templeAttackPrice)
        {
            GameObject line;
            int i = 0;
         
            foreach (KeyValuePair<ResourceType, int> couple in templeAttackPrice)
            {
                line = listPrefabs[i];
                line.SetActive(true);
                Image image = line.GetComponentInChildren<Image>();
                TMP_Text text = line.GetComponentInChildren<TMP_Text>();
                image.sprite = GetIconSprite(couple.Key);
                text.text = couple.Value.ToString();
                i += 1;
            }
        }

        private Sprite GetIconSprite(ResourceType resourceType)
        {
            return _resourceSprite[resourceType];
        }
        
        private void CreateImageTypeAttack(AttackType attackType)
        {
            imageAttackType.sprite = _attackTypeSprite[attackType];
        }
    }
}
