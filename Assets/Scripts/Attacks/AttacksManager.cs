using Attacks.Animation;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using Networking.Common.Server;
using Networking.Common.Server.Attacks;
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
        
        [SerializeField] private GameObject _poseidonAnimation;
        [SerializeField] private GameObject _hadesAnimation;
        
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
            /*
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
                }
                ShowPopUpAttack();
            });
            */
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
            PopUpManager.Instance.ShowPopUp($"{OwnersMaterial.GetName(GameConstants.PlayerId)} has attacked.", 3);
        }
    }
}
