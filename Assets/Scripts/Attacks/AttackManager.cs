using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animation.Attacks;
using Animation.Attacks.Hades;
using BuildingsFolder;
using BuildingsFolder.BuildingsClasses;
using ForNetwork;
using Networking.Common.Server.Attacks;
using PopUp;
using Sound;
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


        public Dictionary<(int, int), AthenaInfo> dictAthena = new();
        
        private OwnerManager _ownerManager;
        public AudioManager audioManager;
        public Temple Temple;
                                    
        public class AthenaInfo
        {
            public int Range;
            public float Duration;
            public uint PlayerId;
            public uint AttackerId;

            public AthenaInfo(int range, float duration, uint playerId, uint attackerId)
            {
                Range = range;
                Duration = duration;
                PlayerId = playerId;
                AttackerId = attackerId;
            }

        }
        
        private void Awake()
        {
            _ownerManager = FindFirstObjectByType<OwnerManager>();
            _buildingsManager = FindFirstObjectByType<BuildingsManager>();
            
            audioManager = GameObject.FindFirstObjectByType<AudioManager>();
            
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
                    audioManager.PlayAttackPoseidon();
                });

            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackHadesGameAction>(
                (proxy, action) =>
                {
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    var deleteBuilding = _buildingsManager.FakeDeleteBuilding(action.TargetX, action.TargetY);
                    var instantiate = Instantiate(_hadesAnimation, new Vector3(x, 0, z), Quaternion.identity);
                    instantiate.GetComponent<AnimHades>().buildingDestroy = deleteBuilding;
                    ShowPopUpAttack();
                    audioManager.PlayAttackHades();
                });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackAthenaGameAction>(
                (proxy, action) =>
                {
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    Instantiate(_athenaAnimation, new Vector3(x, 0, z), Quaternion.identity);
                    
                    Building buildingplaced = _buildingsManager.Buildings[(action.TargetX, action.TargetY)];
                    
                    uint oldOwner = buildingplaced.OwnerId;
                    uint oldAttacker = dictAthena.TryGetValue((action.TargetX, action.TargetY), out AthenaInfo info) ? info.AttackerId : oldOwner;
                    
                    dictAthena[(action.TargetX, action.TargetY)] = new AthenaInfo(buildingplaced.Range, action.Duration, oldOwner, action.AttackerId);
                    
                    _buildingsManager.UnsetOwners(action.TargetX, action.TargetY, buildingplaced.Range, oldAttacker);
                    _buildingsManager.SetOwners(action.TargetX, action.TargetY, buildingplaced.Range, action.AttackerId);
                    
                    ShowPopUpAttack();
                    audioManager.PlayAttackAthena();
                });
            
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerAttackDionysusGameAction>(
                (proxy, action) =>
                {
                    (float x, float z) = StaticGridTools.MapIndexToWorldCenterCo(action.TargetX, action.TargetY);
                    Instantiate(_dionysosAnimation, new Vector3(x, 0, z), Quaternion.identity);
                    _buildingsManager.Buildings[(action.TargetX,action.TargetY)].Paralyze(action.Duration);
                    
                    ShowPopUpAttack();
                    audioManager.PlayAttackDionysos();
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
                    audioManager.PlayAttackZeus();
                });




            
            //___________________________________________________________//
            //___________________________________________________________//
            //___________________________________________________________//
            
        }

        public void Update()
        {
            List<(int, int)> toRemove = new List<(int, int)>();

            foreach (var (key, value) in dictAthena)
            {
                (int x, int y) = key;
                
                value.Duration -= Time.deltaTime;
                
                if (value.Duration <= 0f)
                {
                    toRemove.Add((x, y));
                    
                    _buildingsManager.UnsetOwners(x, y, value.Range, value.AttackerId);
                    _buildingsManager.SetOwners(x, y, value.Range, value.PlayerId);
                }
            }

            foreach (var cpl in toRemove)
            {
                dictAthena.Remove(cpl);
            }
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
                int nbrtentative = 0;
                while (nbrtentative < 100)
                {
                    int randomindex = Random.Range(0,nonparalyze.Count);
                    (int neighborX, int neighborY) = nonparalyze[randomindex];
                    if (paralyze.Contains((neighborX + 1, neighborY)))
                    {
                        res.Add(((neighborX + 1, neighborY),180));
                        paralyze.Add((neighborX,neighborY));
                        nonparalyze.Remove((neighborX, neighborY));
                        break;
                    }
                    if (paralyze.Contains((neighborX - 1, neighborY)))
                    {
                        res.Add(((neighborX - 1, neighborY),0));
                        paralyze.Add((neighborX,neighborY));
                        nonparalyze.Remove((neighborX, neighborY));
                        break;
                    }
                    if (neighborY % 2 == 1)
                    {
                        if (paralyze.Contains((neighborX, neighborY + 1)))
                        {
                            res.Add(((neighborX, neighborY + 1),60));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX, neighborY - 1)))
                        {
                            res.Add(((neighborX, neighborY - 1),300));
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
                        
                        if (paralyze.Contains((neighborX + 1, neighborY - 1)))
                        {
                            res.Add(((neighborX + 1, neighborY - 1),240));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                    }
                    else
                    {
                        if (paralyze.Contains((neighborX, neighborY + 1)))
                        {
                            res.Add(((neighborX, neighborY + 1),120));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                        if (paralyze.Contains((neighborX, neighborY - 1)))
                        {
                            res.Add(((neighborX, neighborY - 1),240));
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
                        if (paralyze.Contains((neighborX - 1, neighborY + 1)))
                        {
                            res.Add(((neighborX - 1, neighborY + 1),60));
                            paralyze.Add((neighborX,neighborY));
                            nonparalyze.Remove((neighborX, neighborY));
                            break;
                        }
                    }
                    nbrtentative++;
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
                    Instantiate(_zeus2Animation, fromPosition, Quaternion.Euler(0, angle + 90f, 0));
                }
                
                
                //Destroy(lightning, 1f);

                yield return new WaitForSeconds(0.05f); // Pause d'une seconde avant la prochaine instantiation
            }
        }
        
        
        


    }
    
}