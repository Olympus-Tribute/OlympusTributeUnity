using System.Collections;
using System.Collections.Generic;
using Animation.Attacks;
using Attacks.Animation;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using Networking.Common.Server.Attacks;
using PopUp;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Attacks
{
    public class AttacksManager : MonoBehaviour
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
                    var anim = Instantiate(_poseidonAnimation, new Vector3(x, 0, z), quaternion.identity);
                    anim.GetComponent<AttackPoseidon>().AnimationDuration = action.Duration;
                    /*
                     déactiver tous les batiments pendant action.Duration sachant que tous les batiments correspondent à action.Targets
                     pendant la période de déactivation, mettre des prefab eau pour former un lac. 
                     */
                    /*foreach (var (x1, y1) in action.Targets)
                    {
                        Instantiate(_waterPrefab, new Vector3(x1, 0, y1), quaternion.identity);
                        StartCoroutine(DestroyLater())
                    }*/
                    
                    //Paralise tous les batiments 
                    foreach (var (x2, y2) in action.Targets)
                    {
                        _buildingsManager.Buildings[(x2,y2)].Paralyze(action.Duration);
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
                    
                    /*var ownerManager = FindFirstObjectByType<OwnerManager>();
                    uint? originalOwner = ownerManager.GetOwner(action.TargetX, action.TargetY);
                    uint newOwner = ServerManager.PlayerId;
                    
                    if (originalOwner.HasValue)
                    {
                        ownerManager.RemoveOwner(action.TargetX, action.TargetY, originalOwner.Value);
                    }
                    ownerManager.AddOwner(action.TargetX, action.TargetY, newOwner);*/
                    Instantiate(_athenaAnimation, new Vector3(x, 0, z), quaternion.identity);
                    //StartCoroutine(RestoreOriginalOwnerAfterDelay(action.TargetX, action.TargetY, originalOwner, action.Duration));
                    
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
                    Instantiate(_dionysosAnimation, new Vector3(x, 0, z), Quaternion.identity);
                    
                    //Paralyse le batiment
                    _buildingsManager.Buildings[(action.TargetX,action.TargetY)].Paralyze(action.Duration);
                    
                    ShowPopUpAttack();
                });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackZeusGameAction>(
                (proxy, action) =>
                {
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);

                    // Foudre vertical
                    var verticalzeus = Instantiate(_zeus1Animation, new Vector3(x, 0, z), Quaternion.identity);
                    StartCoroutine(DestroyLater(verticalzeus));
                    // Propagation foudre horizontal
                    if (action.Targets.Length > 1)
                    {
                        var paralyzeList = ParalyzeList(action.Targets, action.TargetX, action.TargetY);
                        PropagateLightning(paralyzeList, action.Duration);
                    }

                    foreach (var (x2, y2) in action.Targets)
                    {
                        _buildingsManager.Buildings[(x2, y2)].Paralyze(action.Duration);
                    }

                    
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
            PopUpManager.Instance.ShowPopUp($"{OwnersMaterial.GetName(GameConstants.PlayerId)} has attacked.", 3);
        }
        
        private System.Collections.IEnumerator RestoreOriginalOwnerAfterDelay(int x, int y, uint? originalOwner, float delay)
        {
            yield return new WaitForSeconds(delay);

            var ownerManager = FindFirstObjectByType<OwnerManager>();
            if (ownerManager == null) yield break;

            ownerManager.RemoveOwner(x, y, GameConstants.PlayerId);
            
            if (originalOwner.HasValue)
            {
                ownerManager.AddOwner(x, y, originalOwner.Value);
            }
        }
        
        private IEnumerator DestroyAllAfterDelay(List<GameObject> lightningList, float delay)
        {
            yield return new WaitForSeconds(delay);
            foreach (var obj in lightningList)
            {
                if (obj != null)
                    GameObject.Destroy(obj);
            }
        }
        
        /*private static (int, int) GetParalysedFrom(int fromX, int fromY, int angle)
        {
            bool isOddRow = fromY % 2 == 1;

            return angle switch
            {
                270 => (fromX - 1, fromY), // Gauche
                90  => (fromX + 1, fromY), // Droite

                330 => isOddRow ? (fromX, fromY - 1) : (fromX + 1, fromY - 1),
                30  => isOddRow ? (fromX + 1, fromY + 1) : (fromX, fromY + 1),

                210 => isOddRow ? (fromX, fromY + 1) : (fromX - 1, fromY + 1),
                150 => isOddRow ? (fromX - 1, fromY - 1) : (fromX, fromY - 1),

                _ => (fromX, fromY)
            };
        }*/
        
        private void PropagateLightning(List<((int x, int y), int angle)> paralyzeList, float duration)
        {
            foreach (var ((fromX, fromY), angle) in paralyzeList)
            {
                (float fromXWorld, float fromZWorld) = StaticGridTools.MapIndexToWorldCenterCo(fromX, fromY);
                Vector3 fromPosition = new Vector3(fromXWorld, 0, fromZWorld);

                GameObject lightning = Instantiate(_zeus2Animation, fromPosition, Quaternion.Euler(90,angle,0));

                /*var (toX, toY) = GetParalysedFrom(fromX, fromY, angle);
                (float toXWorld, float toZWorld) = StaticGridTools.MapIndexToWorldCenterCo(toX, toY);
                Vector3 toPosition = new Vector3(toXWorld, 0, toZWorld);*/

                var anim = lightning.GetComponent<Anim_ZeusHorizontal>();
                /*if (anim != null)
                {
                    anim.targetPosition = toPosition;
                }*/

                StartCoroutine(DestroyLater(lightning));

                //yield return new WaitForSeconds(0.3f); // délai de propagation visuelle
            }
        }

        private IEnumerator DestroyLater(GameObject gameObject)
        {
            yield return new WaitForSeconds(0.3f);
            Destroy(gameObject);

        }


    }
    
}