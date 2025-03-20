using System.Collections.Generic;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using ForServer;
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
        
        public GridLayoutGroup GridLayoutGroup;
        public GameObject PrefabLine;
        public List<GameObject> ListPrefabs;
        
        //______________________________________________//
        //_____________________ICON_____________________//
        //______________________________________________//
        

        // Variables publiques pour assigner les sprites dans l'éditeur Unity
        public Sprite IconPopulationSprite;
        public Sprite IconWoodSprite;
        public Sprite IconStoneSprite;
        public Sprite IconGoldSprite;
        public Sprite IconDiamondSprite;
        public Sprite IconObsidianSprite;
        public Sprite IconWaterSprite;
        public Sprite IconVineSprite;
        
        
        //______________________________________________//
        //______________________________________________//
        //______________________________________________//
        
        private AttacksManager _attacksManager;
        private BuildingsManager _buildingsManager;
        
        private Dictionary<ResourceType, Sprite> _resourceSpriteNames;
        
        private uint _compteurMouse;
        
        public void Start()
        {
            _attacksManager = FindFirstObjectByType<AttacksManager>();
            _buildingsManager = FindFirstObjectByType<BuildingsManager>();
            menuUIAttack.SetActive(false);
            _compteurMouse = 0;
            
            _resourceSpriteNames = new Dictionary<ResourceType, Sprite>
            {
                { ResourceType.Population, IconPopulationSprite},
                { ResourceType.Wood, IconWoodSprite },
                { ResourceType.Stone, IconStoneSprite },
                { ResourceType.Gold, IconGoldSprite },
                { ResourceType.Diamond, IconDiamondSprite },
                { ResourceType.Obsidian, IconObsidianSprite },
                { ResourceType.Water, IconWaterSprite },
                { ResourceType.Vine, IconVineSprite },
            };

            List<GameObject> prefabs = new List<GameObject>();
            for (int i = 0; i < 4; i++)
            {
                GameObject line = Instantiate(PrefabLine, GridLayoutGroup.transform);
                prefabs.Add(line);
            }
            ListPrefabs = prefabs;
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
                if (_buildingsManager.Buildings.TryGetValue((x, y), out var targetBuilding) && targetBuilding.OwnerId == ServerManager.PlayerId &&
                    targetBuilding is Temple targetTemple)
                {
                    _compteurMouse += 1;
                    
                    menuUIAttack.SetActive(true);
                    
                    // Un temple a été ciblé
                    _attacksManager.Temple = targetTemple;
                    titleInfoAttack.text = $"Attack : {targetTemple.Name}";
                    
                    // Associer le bon Sprite Asset TMP avant d'afficher le texte
                    infoAttackPrice.spriteAsset = TMP_Settings.defaultSpriteAsset; 
                    //infoAttackPrice.text = CreateTextePrice(targetTemple);
                    CreateTextePrice(targetTemple);
                }
            }
            else if (_compteurMouse >= 2)
            {
                _attacksManager.Temple.SendAttack(x, y);
                ShowPopUpAttack();
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
        

        private void CreateTextePrice(Temple temple)
        {
            /*
            string res = string.Empty;
            foreach (KeyValuePair<ResourceType, int> couple in temple.AttackPrice)
            {
                res += $"{IconPopulationSprite} {couple.Key}: {couple.Value}\n";
            }
            return res;
            */
            GameObject line;
            int i = 0;
         
            foreach (KeyValuePair<ResourceType, int> couple in temple.AttackPrice)
            {
                line = ListPrefabs[i];
                Image image = line.GetComponentInChildren<Image>();
                TMP_Text text = line.GetComponentInChildren<TMP_Text>();
                image.sprite = GetIconSprite(couple.Key);
                text.text = couple.Value.ToString();
                i += 1;
            }
        }


        public Sprite GetIconSprite(ResourceType resourceType)
        {
            return _resourceSpriteNames[resourceType];
        }
        
        private void ShowPopUpAttack()
        {
            string city;
            if (ServerManager.City.Length <= ServerManager.PlayerId)
            {
                city = $"Player number {ServerManager.PlayerId}";
            }
            else
            {
                city = ServerManager.City[ServerManager.PlayerId];
            }
            
            PopUpManager.Instance.ShowPopUp($"{city} has attacked.", 3);
        }
    }
}
