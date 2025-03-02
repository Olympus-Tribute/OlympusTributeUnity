using Attacks.Animation;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using JetBrains.Annotations;
using Networking.Common.Server;
using OlympusDedicatedServer.Components.Attack;
using Unity.Mathematics;
using UnityEngine;

namespace Attacks
{
    public class AttacksManager : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera; 
        private BuildingsManager _buildingsManager;
        [CanBeNull] private Temple _temple;
        [SerializeField] private GameObject _poseidonAnimation;
        [SerializeField] private GameObject _hadesAnimation;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
    
        private void Awake()
        {
            _buildingsManager = FindObjectOfType<BuildingsManager>();
            if (_buildingsManager == null)
            {
                Debug.LogError("BuildingsManager was not found in the scene !");
            }

            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackGameAction>((proxy, action) =>
            {
                (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                var temple = _buildingsManager.buildings[(action.TempleX, action.TempleY)] as Temple;
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
        }
        

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("FollowMouse est appellé");
                FollowMouse();
            }
            //TODO si placement ne pas rentrer dans follow mouse et si on rentre dans placement stop la follow mouse
        }
    
        private void FollowMouse()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                (int x, int y) = StaticGridTools.WorldCoToMapIndex(hit.point.x, hit.point.y, hit.point.z);

                if (_temple is null)
                {
                    if (_buildingsManager.buildings.TryGetValue((x,y),out var targetbBuilding) &&
                        targetbBuilding is Temple targetTemple)
                    {
                        Debug.Log("Un temple a été target");
                        _temple = targetTemple;
                    }
                }
                else
                {
                    Debug.Log("Une attaque a été envoyé");
                    _temple.SendAttack(x,y);
                    _temple = null;
                }
            
            }
        }
    }
}
