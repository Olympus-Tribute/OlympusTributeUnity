using System;
using System.Data.Common;
using BuildingsFolder;
using Cecs.Util;
using ForServer;
using OlympusDedicatedServer.Components.WorldComp;
using OlympusWorldGenerator;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace BuildingsFolder
{
    public class BuildingWall : MonoBehaviour
    {
        private OwnerManager _ownerManager;
        private readonly uint _mapWidth;
        private readonly uint _mapHeight;
        
        private readonly GameObject [,][] _walls;
        private readonly (uint , GameObject)?[,] _flags;
        private readonly uint[,] _wallCount;

        
        [SerializeField]private GameObject wallPrefab;
        private BuildingFlag _buildingFlag;
        
        
        public BuildingWall()
        {
            _mapHeight = ServerManager.MapHeight;
            _mapWidth = ServerManager.MapWidth;
            _walls = new GameObject[_mapHeight, _mapWidth][];
            _flags = new (uint ,GameObject)? [_mapHeight, _mapWidth];
            _wallCount = new uint[_mapHeight, _mapWidth];
            for (var i = 0; i < _mapWidth; i++)
            {
                for (int j = 0; j < _mapHeight; j++)
                {
                     _walls[i,j] = new GameObject[6];
                }
               
            }
        }

        private void Start()
        {
            _ownerManager = FindFirstObjectByType<OwnerManager>();
            _buildingFlag = this.GetComponent<BuildingFlag>();
        }

        public uint? Owner(int x, int y)
        {
            return _ownerManager.GetOwner(x, y);
        }

        public void Update()
        {
            OpenBitSet needUpdate = _ownerManager.NewOwner;
            OpenBitSet needFlagUpdate = new OpenBitSet();
            
            var i = 0; 
            
            while ( (i = needUpdate.NextSetBit(i)) != -1)
            {
                int x =  (int)(i % _mapWidth);
                int y = (int)(i / _mapWidth);
                uint? owner = Owner(x, y);

                var poses = WorldCoordinates.FindNeighboringTilesOrdered(x, y, _mapWidth, _mapHeight);
                
                for (var sideIndex = 0; sideIndex < poses.Length; sideIndex++)
                {
                    var neighbourPos = poses[sideIndex];
                    
                    if (neighbourPos.HasValue)
                    {
                        var (nx, ny) = neighbourPos.Value;
                        ConsiderBuildWall(sideIndex, nx, ny, x, y, owner);
                        needFlagUpdate.Set(WorldCoordinates.ToIndex(nx, ny, _mapWidth));
                    }
                }

                needFlagUpdate.Set(WorldCoordinates.ToIndex(x, y, _mapWidth));

                
                needUpdate.Clear(i);
                i++;
            }
            NeedFlagUpdate(needFlagUpdate);
        }

        private void NeedFlagUpdate(OpenBitSet needFlagUpdate)
        {
            int i = 0;
            while ( (i = needFlagUpdate.NextSetBit(i)) != -1)
            {
                int x =  (int)(i % _mapWidth);
                int y = (int)(i / _mapWidth);
                uint? owner = Owner(x, y);

                if (_wallCount[x,y] == 3)
                {
                    PlaceFlagAtPos(x, y, owner);
                }
                else
                {  
                    PlaceFlagAtPos(x, y, null);
                }
                i++;
            }
        }

        private void ConsiderBuildWall(int sideIndex, int nx, int ny , int x , int  y , uint? owner)
        {
            var oppositeSideIndex = (sideIndex + 3 ) % 6;
            uint? nOwner = Owner(nx, ny);
            
            var myWall = _walls[x , y ] [sideIndex];
            var nWall = _walls[nx , ny ] [oppositeSideIndex];
            bool hasWall = nWall is not null || myWall is not null;
            if (owner == nOwner)
            {
                if (hasWall)
                {
                    _wallCount[x,y]--;
                    _wallCount[nx,ny]--;
                }
                DestroyWall(sideIndex, x, y);
                DestroyWall(oppositeSideIndex, nx, ny);
                
                return;
            }
            
            
            if ( hasWall )
            {
                return;
            }

            _wallCount[x,y]++;
            _wallCount[nx,ny]++;
            var (wx, wy) = StaticGridTools.MapIndexToWorldCenterCo(x, y);
            _walls[x , y ][sideIndex] = Instantiate(wallPrefab , new Vector3(wx, 0, wy), Quaternion.Euler(0, SideIndexToDegree(sideIndex), 0) ); 
        }

        private void PlaceFlagAtPos(int x, int y, uint? owner)
        {
            var flagTupple = _flags[x , y ];
            if (!owner.HasValue)
            {
                if (!flagTupple.HasValue)
                {
                    return;
                }
                DestroyFlagTupple(x, y, flagTupple.Value);
                return;
            }

            if (flagTupple.HasValue)
            {
                if (flagTupple.Value.Item1 == owner)
                {
                    return;
                }
                DestroyFlagTupple(x,y, flagTupple.Value);
            }
            
            _flags[x,y] = (owner.Value , _buildingFlag.InstantiateFlag(x, y, owner.Value));
        }

        private void DestroyFlagTupple(int x, int y, (uint, GameObject) flagTupple)
        {
            Destroy(flagTupple.Item2);
            _flags[x,y] = null;
        }

        private float SideIndexToDegree(int sideIndex)
        {
            return sideIndex / 6f * 360;
        }

        private void DestroyWall(int sideIndex, int x, int y)
        {
            var wall = _walls[x, y][sideIndex];
            
            Object.Destroy(wall);
            _walls[x, y][sideIndex] = null;
        }
        
    }
}