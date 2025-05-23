using System;
using System.Collections;
using System.Collections.Generic;
using Animation.Attacks;
using Attacks.Animation;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using Networking.Common.Server.Attacks;
using PopUp;
using Unity.Mathematics;
using UnityEngine;
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
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    Instantiate(_poseidonAnimation, new Vector3(x, 0, z), Quaternion.identity);
                    
                    foreach (var (x2, y2) in action.Targets)
                    {
                        _buildingsManager.Buildings[(x2,y2)].Paralyze(action.Duration);
                    }
                    ShowPopUpAttack();
                });

            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackHadesGameAction>(
                (proxy, action) =>
                {
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    var deleteBuilding = _buildingsManager.FakeDeleteBuilding(action.TargetX, action.TargetY);
                    var instantiate = Instantiate(_hadesAnimation, new Vector3(x, 0, z), Quaternion.identity);
                    instantiate.GetComponent<AnimHades>().buildingDestroy = deleteBuilding;
                    ShowPopUpAttack();
                });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackAthenaGameAction>(
                (proxy, action) =>
                {
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    Instantiate(_athenaAnimation, new Vector3(x, 0, z), Quaternion.identity);
                    ShowPopUpAttack();
                });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackDionysusGameAction>(
                (proxy, action) =>
                {
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    Instantiate(_dionysosAnimation, new Vector3(x, 0, z), Quaternion.identity);
                    _buildingsManager.Buildings[(action.TargetX,action.TargetY)].Paralyze(action.Duration);
                    
                    ShowPopUpAttack();
                });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackZeusGameAction>(
                (proxy, action) =>
                {
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);

                    // Foudre vertical
                    Instantiate(_zeus1Animation, new Vector3(x, 0, z), Quaternion.identity);
                    //Destroy(verticalzeus, 0.3f);
                    
                    // Propagation foudre horizontal
                    if (action.Targets.Length > 1)
                    {
                        try
                        {
                            var paralyzeList = ParalyzeList(action.Targets, action.TargetX, action.TargetY);
                            StartCoroutine(PropagateLightning(paralyzeList));
                        }
                        catch (Exception)
                        {
                            Debug.Log("crash ParlyzeList ou Coroutine");
                        }
                        
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

        /*private static List<((int, int), int)> ParalyzeList((int,int)[] targets, int targetX, int targetY)
        {
            List<(int, int)> paralyze = new List<(int, int)>();
            List<(int, int)> nonparalyze = new List<(int, int)>(targets);
            List<((int, int), int)> res = new List<((int, int), int)>();
            paralyze.Add((targetX,targetY));
            nonparalyze.Remove((targetX, targetY));
            
            while (nonparalyze.Count != 0)
            {
                int nbrtentative = 0;
                while (nbrtentative < 100)
                {
                    int randomindex = Random.Range(0,nonparalyze.Count);
                    (int neighborX, int neighborY) = nonparalyze[randomindex];
                    if (paralyze.Contains((neighborX, neighborY + 1)))
                    {
                        res.Add(((neighborX, neighborY + 1),180));
                        paralyze.Add((neighborX,neighborY));
                        nonparalyze.Remove((neighborX, neighborY));
                        break;
                    }
                    if (paralyze.Contains((neighborX, neighborY - 1)))
                    {
                        res.Add(((neighborX, neighborY - 1),0));
                        paralyze.Add((neighborX,neighborY));
                        nonparalyze.Remove((neighborX, neighborY));
                        break;
                    }
                    if (neighborY % 2 == 1)
                    {
                        if (paralyze.Contains((neighborX + 1, neighborY)))
                        {
                            res.Add(((neighborX + 1, neighborY),60));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX - 1, neighborY)))
                        {
                            res.Add(((neighborX - 1, neighborY),300));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                       
                        if (paralyze.Contains((neighborX + 1, neighborY + 1)))
                        {
                            res.Add(((neighborX + 1, neighborY + 1),120));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        
                        if (paralyze.Contains((neighborX - 1, neighborY + 1)))
                        {
                            res.Add(((neighborX - 1, neighborY + 1),240));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                    }
                    else
                    {
                        if (paralyze.Contains((neighborX + 1, neighborY)))
                        {
                            res.Add(((neighborX + 1, neighborY),120));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX - 1, neighborY)))
                        {
                            res.Add(((neighborX - 1, neighborY),240));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX + 1, neighborY - 1)))
                        {
                            res.Add(((neighborX + 1, neighborY - 1),60));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX - 1, neighborY - 1)))
                        {
                            res.Add(((neighborX - 1, neighborY - 1),300));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                    }
                    nbrtentative++;
                }
            }
            return res;
        }*/
        
        private static List<((int, int), int)> ParalyzeList((int, int)[] targets, int targetX, int targetY)
        {
            List<(int, int)> paralyze = new List<(int, int)> { (targetX, targetY) };
            List<(int, int)> nonparalyze = new List<(int, int)>(targets);
            List<((int, int), int)> res = new List<((int, int), int)>();

            nonparalyze.Remove((targetX, targetY));

            // Définit les directions avec leurs angles associés (pour hexagones pointy-top)
            (int dx, int dy, int angle)[] evenDirections = new (int, int, int)[]
            {
                (0, 1, 0),       // N
                (1, 0, 60),      // NE
                (1, -1, 120),    // SE
                (0, -1, 180),    // S
                (-1, -1, 240),   // SW
                (-1, 0, 300)     // NW
            };

            (int dx, int dy, int angle)[] oddDirections = new (int, int, int)[]
            {
                (0, 1, 0),       // N
                (1, 1, 60),      // NE
                (1, 0, 120),     // SE
                (0, -1, 180),    // S
                (-1, 0, 240),    // SW
                (-1, 1, 300)     // NW
            };

            System.Random rng = new System.Random();

            while (nonparalyze.Count > 0)
            {
                int randomIndex = rng.Next(nonparalyze.Count);
                (int x, int y) = nonparalyze[randomIndex];

                var directions = (x % 2 == 0) ? evenDirections : oddDirections;

                bool connected = false;
                foreach (var (dx, dy, angle) in directions)
                {
                    (int nx, int ny) = (x + dx, y + dy);
                    if (paralyze.Contains((nx, ny)))
                    {
                        res.Add(((nx, ny), angle));
                        paralyze.Add((x, y));
                        nonparalyze.RemoveAt(randomIndex);
                        connected = true;
                        break;
                    }
                }

                if (!connected)
                {
                    // Aucun voisin connecté, on le retente plus tard : le remettre à la fin de la liste
                    nonparalyze.Add(nonparalyze[randomIndex]);
                    nonparalyze.RemoveAt(randomIndex);
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
        
        
        
        
        private IEnumerator PropagateLightning(List<((int x, int y), int angle)> paralyzeList)
        {
            foreach (var ((fromX, fromY), angle) in paralyzeList)
            {
                (float fromXWorld, float fromZWorld) = StaticGridTools.MapIndexToWorldCenterCo(fromX, fromY);
                Vector3 fromPosition = new Vector3(fromXWorld, 0, fromZWorld);

                if (_zeus2Animation is null)
                {
                    Debug.LogWarning("_zeus2Animation est nul");
                }
                else
                {
                    Instantiate(_zeus2Animation, fromPosition, Quaternion.Euler(0, angle - 90f, 0));
                }
                
                
                //Destroy(lightning, 1f);

                yield return new WaitForSeconds(0.3f); // Pause d'une seconde avant la prochaine instantiation
            }
        }
        
        
        


    }
    
}