using System;
using System.Collections.Generic;
using Attacks.Animation;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using ForServer;
using Menus.MenusInGame;
using Networking.Common.Server;
using OlympusDedicatedServer.Components.Attack;
using PopUp;
using Resources;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Attacks
{
    public class AttacksMenu : MonoBehaviour
    {
        public GameObject menuUIAttack;
        public TMP_Text titleInfoAttack;
        public TMP_Text infoAttackPrice;
        
        //public GameObject resourceCostContainer; // Panel où afficher les ressources
        //public GameObject resourceCostPrefab; // Prefab avec Image + TMP_Text
    
        
        // Référence à l'image dans le panel
        //public Image panelImage;

        // Références aux images de remplacement
        //public Sprite imageAttackPoseidon;
        //public Sprite imageAttackHades;
        
        
        
        private AttacksManager _attacksManager;
        private BuildingsManager _buildingsManager;
        
        private Dictionary<ResourceType, Sprite> _resourceIcons;
        
        private uint _compteurMouse;
        

        public void Start()
        {
            _attacksManager = FindFirstObjectByType<AttacksManager>();
            _buildingsManager = FindFirstObjectByType<BuildingsManager>();
            menuUIAttack.SetActive(false);
            _compteurMouse = 0;
            
            /*
            _resourceIcons = new Dictionary<ResourceType, Sprite>
            {
                { ResourceType.Population, Resources.Load<Sprite>("Icons/Wood") },
                { ResourceType.Wood, Resources.Load<Sprite>("Icons/Wood") },
                { ResourceType.Stone, Resources.Load<Sprite>("Icons/Stone") },
                { ResourceType.Gold, Resources.Load<Sprite>("Icons/Gold") },
                { ResourceType.Diamond, Resources.Load<Sprite>("Icons/Food") },
                { ResourceType.Obsidian, Resources.Load<Sprite>("Icons/Food") },
                { ResourceType.Water, Resources.Load<Sprite>("Icons/Food") },
                { ResourceType.Vine, Resources.Load<Sprite>("Icons/Food") },
            };
            */
        }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mapIndex  = MousePositionTracker.Instance.GetMouseMapIndexCo();
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
                if (_buildingsManager.Buildings.TryGetValue((x,y),out var targetbBuilding) && targetbBuilding.OwnerId == ServerManager.PlayerId &&
                    targetbBuilding is Temple targetTemple)
                {
                    _compteurMouse += 1;
                    
                    //_menusInGameManager.ShowMenu(menuUIAttack); //
                    menuUIAttack.SetActive(true);
                    
                    //Un temple a été target
                    _attacksManager.Temple = targetTemple;
                    titleInfoAttack.text = $"Attack : {targetTemple.Name}";
                    infoAttackPrice.text = CreateTextePrice(targetTemple);
                    //SelectImageAttack();
                }
            }
            else if (_compteurMouse >= 2)
            {
                _attacksManager.Temple.SendAttack(x,y);
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
        public void SelectImageAttack()
        {
            switch (_temple.Type)
            {
                case AttackType.Poseidon:
                    panelImage.sprite = imageAttackPoseidon;
                    break;
                case AttackType.Hades:
                    panelImage.sprite = imageAttackHades;
                    break;
            }
        }
        */

        private string CreateTextePrice(Temple temple)
        {
            string res = String.Empty;
            foreach (KeyValuePair<ResourceType,int> couple in temple.AttackPrice)
            {
               res += $"{couple.Key.ToString()} : {couple.Value}\n";
            }
            return res;
        }
        
        
        private void ShowPopUpAttack()
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
            
            PopUpManager.Instance.ShowPopUp($"{city} has attack.", 3);
        }
    }
}