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
using UnityEngine.UI;

namespace Attacks
{
    public class NewAttacksManager : MonoBehaviour
    {
        private BuildingsManager _buildingsManager;
        
        [SerializeField] private GameObject _poseidonAnimation;
        [SerializeField] private GameObject _hadesAnimation;
        [SerializeField] private GameObject _athenaAnimation;
        [SerializeField] private GameObject _dionysosAnimation;
        [SerializeField] private GameObject _zeusAnimation;
        
        public Temple Temple;
        
        private void Awake()
        {
            _buildingsManager = FindFirstObjectByType<BuildingsManager>();
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
                switch (temple?.AttackType)
                {
                    case (AttackType.Poseidon):
                        Instantiate(_poseidonAnimation, new Vector3(x,0,z),quaternion.identity);
                        break;
                    
                    case (AttackType.Hades):
                        var fakeDeleteBuilding = _buildingsManager.FakeDeleteBuilding(action.TargetX, action.TargetY);
                        var instantiate = Instantiate(_hadesAnimation, new Vector3(x,0,z),quaternion.identity);
                        instantiate.GetComponent<AnimHades>().buildingDestroy = fakeDeleteBuilding;
                        break;
                    
                    case (AttackType.Athena) :
                        
                        break;
                    
                    case (AttackType.Dionysos) :
                        break;
                    
                    case(AttackType.Zeus) :
                        break;
                }
                ShowPopUpAttack();
            });
            
            //___________________________________________________________//
            //___________________________________________________________//
            //___________________________________________________________//
        }
        
        public void Start()
        {
            Temple = null;
        }
        
        private void ShowPopUpAttack()
        {
            PopUpManager.Instance.ShowPopUp($"{OwnersMaterial.GetName(ServerManager.PlayerId)} has attacked.", 3);
        }
    }
}