using System.Collections.Generic;
using Attacks.Animation;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using ForServer;
using Networking.Common.Server;
using Networking.Common.Server.Attacks;
using OlympusDedicatedServer.Components.Attack;
using PopUp;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Attacks
{
    public class NewAttacksManager : MonoBehaviour
    {
        private BuildingsManager _buildingsManager;
        
        [SerializeField] private GameObject _poseidonAnimation;
        [SerializeField] private GameObject _hadesAnimation;
        [SerializeField] private GameObject _athenaAnimation;
        [SerializeField] private GameObject _dionysosAnimation;
        [SerializeField] private GameObject _zeus1Animation;
        [SerializeField] private GameObject _zeus2Animation;
        [SerializeField] private GameObject _waterPrefab;

        
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
            
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackPoseidonGameAction>(
                (proxy, action) =>
                {
                    /*
                     rend les batiments touchés inactif pendant un instant t
                     */
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    Instantiate(_poseidonAnimation, new Vector3(x, 0, z), quaternion.identity);
                    /*
                     déactiver tous les batiments pendant action.Duration sachant que tous les batiments correspondent à action.Targets
                     pendant la période de déactivation, mettre des prefab eau pour former un lac. 
                     */
                    foreach (var (x1, y) in action.Targets)
                    {
                        _buildingsManager.DisableBuilding(x1, y, action.Duration, _waterPrefab);
                    }
                    
                    ShowPopUpAttack();
                });

            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackHadesGameAction>(
                (proxy, action) =>
                {
                    /*
                     supprime le batiment 
                     */
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    var DeleteBuilding = _buildingsManager.FakeDeleteBuilding(action.TargetX, action.TargetY);
                    var instantiate = Instantiate(_hadesAnimation, new Vector3(x, 0, z), quaternion.identity);
                    instantiate.GetComponent<AnimHades>().buildingDestroy = DeleteBuilding;
                    ShowPopUpAttack();
                });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackAthenaGameAction>(
                (proxy, action) =>
                {
                    /*
                     Vole un batiment pendant un instant t
                     */
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    
                    var ownerManager = FindFirstObjectByType<OwnerManager>();
                    uint? originalOwner = ownerManager.GetOwner(action.TargetX, action.TargetY);
                    uint newOwner = ServerManager.PlayerId;
                    
                    if (originalOwner.HasValue)
                    {
                        ownerManager.RemoveOwner(action.TargetX, action.TargetY, originalOwner.Value);
                    }
                    ownerManager.AddOwner(action.TargetX, action.TargetY, newOwner);
                    Instantiate(_athenaAnimation, new Vector3(x, 0, z), quaternion.identity);
                    StartCoroutine(RestoreOriginalOwnerAfterDelay(action.TargetX, action.TargetY, originalOwner, action.Duration));

                    ShowPopUpAttack();
                });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackDionysusGameAction>(
                (proxy, action) =>
                {
                    /*
                     A revoir concernant le batiment inactif 
                     rend batiment inactif pendant un instant t
                     */
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    _buildingsManager.DisableBuilding(action.TargetX, action.TargetY, action.Duration);
                    var animation = Instantiate(_dionysosAnimation, new Vector3(x, 0, z), Quaternion.identity);
                    
                    ShowPopUpAttack();
                });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackZeusGameAction>(
                (proxy, action) =>
                {
                    /*
                     rend inactif les batiments touchés pendant un instant t
                     */
                    if (action.Targets.Length > 1)
                    {
                        List<((int, int), int)> paralyzelist = ParalyzeList(action.Targets, action.TargetX, action.TargetY);
                    }
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    _buildingsManager.DisableBuilding(action.TargetX, action.TargetY, action.Duration);
                    Instantiate(_zeus1Animation, new Vector3(x, 0, z), quaternion.identity);
                    // premiere animation : eclair vertical pdt dur
                    
                    /*
                     connection de batiments
                     idée : Propagation sous forme d'arbre; gérer les conflits;
                     */
                    var DeleteBuilding = _buildingsManager.FakeDeleteBuilding(action.TargetX, action.TargetY);
                    var instantiate = Instantiate(_hadesAnimation, new Vector3(x, 0, z), quaternion.identity);
                    
                    
                    ShowPopUpAttack();
                });
            
            




            //___________________________________________________________//
            //___________________________________________________________//
            //___________________________________________________________//
            
        }

        private static List<((int, int), int)> ParalyzeList((int,int)[] targets, int targetX, int targetY)
        {
            List<(int, int)> paralyze = new List<(int, int)>();
            List<(int, int)> nonparalyze = new List<(int, int)>(targets);
            List<((int, int), int)> res = new List<((int, int), int)>();
            paralyze.Add((targetX,targetY));
            nonparalyze.Remove((targetX, targetY));
            
            while (nonparalyze.Count != 0)
            {
                while (true)
                {
                    int randomindex = Random.Range(0,nonparalyze.Count);
                    (int neighborX, int neighborY) = nonparalyze[randomindex];
                    if (paralyze.Contains((neighborX + 1, neighborY)))
                    {
                        res.Add(((neighborX + 1, neighborY),270));
                        paralyze.Add((neighborX,neighborY));
                        nonparalyze.Remove((neighborX, neighborY));
                        break;
                    }
                    if (paralyze.Contains((neighborX - 1, neighborY)))
                    {
                        res.Add(((neighborX - 1, neighborY),90));
                        paralyze.Add((neighborX,neighborY));
                        nonparalyze.Remove((neighborX, neighborY));
                        break;
                    }

                    if (neighborY % 2 == 1)
                    {
                        if (paralyze.Contains((neighborX, neighborY + 1)))
                        {
                            res.Add(((neighborX, neighborY + 1),330));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX + 1, neighborY + 1)))
                        {
                            res.Add(((neighborX + 1, neighborY + 1),30));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX, neighborY - 1)))
                        {
                            res.Add(((neighborX, neighborY - 1),210));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX + 1, neighborY - 1)))
                        {
                            res.Add(((neighborX + 1, neighborY - 1),150));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                    }
                    else
                    {
                        if (paralyze.Contains((neighborX - 1, neighborY + 1)))
                        {
                            res.Add(((neighborX - 1, neighborY + 1),330));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX, neighborY + 1)))
                        {
                            res.Add(((neighborX, neighborY + 1),30));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX - 1, neighborY - 1)))
                        {
                            res.Add(((neighborX - 1, neighborY - 1),210));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX, neighborY - 1)))
                        {
                            res.Add(((neighborX, neighborY - 1),150));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                    }
                }
            }
            return res;
        }
        
        public void Start()
        {
            Temple = null;
        }
        
        private void ShowPopUpAttack()
        {
            PopUpManager.Instance.ShowPopUp($"{OwnersMaterial.GetName(ServerManager.PlayerId)} has attacked.", 3);
        }
        
        private System.Collections.IEnumerator RestoreOriginalOwnerAfterDelay(int x, int y, uint? originalOwner, float delay)
        {
            yield return new WaitForSeconds(delay);

            var ownerManager = FindFirstObjectByType<OwnerManager>();
            if (ownerManager == null) yield break;

            ownerManager.RemoveOwner(x, y, ServerManager.PlayerId);
            
            if (originalOwner.HasValue)
            {
                ownerManager.AddOwner(x, y, originalOwner.Value);
            }
        }
    }
}