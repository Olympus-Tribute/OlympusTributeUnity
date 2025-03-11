using Attacks.Animation;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using Networking.Common.Server;
using OlympusDedicatedServer.Components.Attack;
using PopUp;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Attacks
{
    public class AttacksManager : MonoBehaviour
    {
        private BuildingsManager _buildingsManager;
        private Temple _temple;
        [SerializeField] private GameObject _poseidonAnimation;
        [SerializeField] private GameObject _hadesAnimation;
        
        public GameObject menuUIAttack;
        public TMP_Text TitleInfoAttack;
        public TMP_Text InfoAttack;
    
        // Référence à l'image dans le panel
        //public Image panelImage;

        // Références aux images de remplacement
        //public Sprite imageAttackPoseidon;
        //public Sprite imageAttackHades;
        
        private uint compteurMouse;
 
        private void Awake()
        {
            _buildingsManager = FindObjectOfType<BuildingsManager>();
            if (_buildingsManager == null)
            {
                Debug.LogError("BuildingsManager was not found in the scene !");
            }

            //___________________________________________________________//
            //_________________________For Multi_________________________//
            //___________________________________________________________//
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackGameAction>((proxy, action) =>
            {
                (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                var temple = _buildingsManager.Buildings[(action.TempleX, action.TempleY)] as Temple;
                switch (temple?.Type)
                {
                    case (AttackType.Poseidon):
                        Instantiate(_poseidonAnimation, new Vector3(x,0,z),quaternion.identity);
                        break;
                    
                    case (AttackType.Hades):
                        var fakeDeleteBuilding = _buildingsManager.FakeDeleteBuilding(action.TargetX, action.TargetY);
                        var instantiate = Instantiate(_hadesAnimation, new Vector3(x,0,z),quaternion.identity);
                        instantiate.GetComponent<AnimHades>().buildingDestroy = fakeDeleteBuilding;
                        break;
                }
            });
            
            //___________________________________________________________//
            //___________________________________________________________//
            //___________________________________________________________//
        }


        public void Start()
        {
            _temple = null;
            menuUIAttack.SetActive(false);
            compteurMouse = 0;
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
            if (_temple is null)
            {
                if (_buildingsManager.Buildings.TryGetValue((x,y),out var targetbBuilding) &&
                    targetbBuilding is Temple targetTemple)
                {
                    compteurMouse += 1;
                    menuUIAttack.SetActive(true);
                    
                    //Un temple a été target
                    _temple = targetTemple;
                    TitleInfoAttack.text = $"Attack : {targetTemple.Name}";
                    InfoAttack.text = $"Description :\n{targetTemple.DescriptionAttack}";
                    //SelectImageAttack();
                }
            }
            else if (compteurMouse >= 2)
            {
                Debug.Log("Une attaque a été envoyé");
                _temple.SendAttack(x,y);
                _temple = null;
                compteurMouse = 0;
            }
        }

        public void ButtonAttack()
        {
            menuUIAttack.SetActive(false);
            PopUpManager.Instance.ShowPopUp("Select a building to attack", 5);
            compteurMouse += 1;
        }
        
        public void ButtonQuit()
        {
            menuUIAttack.SetActive(false);
            _temple = null;
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
    }
}
