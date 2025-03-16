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
        
        //______________________________________________//
        //_____________________ICON_____________________//
        //______________________________________________//

        
        /*
        public Image IconPopulation;
        public Image IconWood;
        public Image IconStone;
        public Image IconGold;
        public Image IconDiamond;
        public Image IconObsidian;
        public Image IconWater;
        public Image IconVine;

        

        // Variables publiques pour assigner les sprites dans l'éditeur Unity
        public Sprite IconPopulationSprite;
        public Sprite IconWoodSprite;
        public Sprite IconStoneSprite;
        public Sprite IconGoldSprite;
        public Sprite IconDiamondSprite;
        public Sprite IconObsidianSprite;
        public Sprite IconWaterSprite;
        public Sprite IconVineSprite;
        
        */
        
        //______________________________________________//
        //______________________________________________//
        //______________________________________________//
        
        private AttacksManager _attacksManager;
        private BuildingsManager _buildingsManager;
        
        private Dictionary<ResourceType, string> _resourceSpriteNames;
        
        private uint _compteurMouse;
        
        public void Start()
        {
            _attacksManager = FindFirstObjectByType<AttacksManager>();
            _buildingsManager = FindFirstObjectByType<BuildingsManager>();
            menuUIAttack.SetActive(false);
            _compteurMouse = 0;
            
            _resourceSpriteNames = new Dictionary<ResourceType, string>
            {
                { ResourceType.Population, "icon_population" },
                { ResourceType.Wood, "icon_wood" },
                { ResourceType.Stone, "icon_stone" },
                { ResourceType.Gold, "icon_gold" },
                { ResourceType.Diamond, "icon_diamond" },
                { ResourceType.Obsidian, "icon_obsidian" },
                { ResourceType.Water, "icon_water" },
                { ResourceType.Vine, "icon_vine" },
            };

            // Assigner les icônes
            //AssignResourceIcons();
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

        
        /*
        private void AssignResourceIcons()
        {
            // Assigner les icônes aux images de l'UI
            IconPopulation.sprite = IconPopulationSprite;
            IconWood.sprite = IconWoodSprite;
            IconStone.sprite = IconStoneSprite;
            IconGold.sprite = IconGoldSprite;
            IconDiamond.sprite = IconDiamondSprite;
            IconObsidian.sprite = IconObsidianSprite;
            IconWater.sprite = IconWaterSprite;
            IconVine.sprite = IconVineSprite;
        }

        private string CreateTextePrice(Temple temple)
        {
            string res = string.Empty;
            foreach (KeyValuePair<ResourceType, int> couple in temple.AttackPrice)
            {
                res += $"{couple.Key}: {couple.Value}\n";
            }
            return res;
        }
        */
        
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
