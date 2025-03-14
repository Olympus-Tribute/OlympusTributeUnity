using Attacks.Animation;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using ForServer;
using Networking.Common.Server;
using OlympusDedicatedServer.Components.Attack;
using PopUp;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Attacks
{
    public class AttacksMenu : MonoBehaviour
    {
        public GameObject menuUIAttack;
        public TMP_Text TitleInfoAttack;
        public TMP_Text InfoAttack;
    
        // Référence à l'image dans le panel
        //public Image panelImage;

        // Références aux images de remplacement
        //public Sprite imageAttackPoseidon;
        //public Sprite imageAttackHades;
        
        
        private AttacksManager _attacksManager;
        private BuildingsManager _buildingsManager;
        private uint _compteurMouse;
        
        public void Start()
        {
            _attacksManager = FindFirstObjectByType<AttacksManager>();
            _buildingsManager = FindFirstObjectByType<BuildingsManager>();
            menuUIAttack.SetActive(false);
            _compteurMouse = 0;
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
                if (_buildingsManager.Buildings.TryGetValue((x,y),out var targetbBuilding) &&
                    targetbBuilding is Temple targetTemple)
                {
                    _compteurMouse += 1;
                    menuUIAttack.SetActive(true);
                    
                    //Un temple a été target
                    _attacksManager.Temple = targetTemple;
                    TitleInfoAttack.text = $"Attack : {targetTemple.Name}";
                    InfoAttack.text = $"{targetTemple.DescriptionAttack}";
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