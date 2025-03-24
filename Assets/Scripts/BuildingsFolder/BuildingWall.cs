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
        private readonly OpenBitSet _processed = new OpenBitSet();
        private OwnerManager _ownerManager;
        private readonly uint _mapWidth;
        private readonly uint _mapHeight;
        
        private readonly GameObject [,][] _walls;
        [SerializeField]private GameObject _wallPrefab;
        private BuildingFlag _buildingFlag;
        private readonly (uint , GameObject)?[,] _flags;
        private uint[,] wallCount;

        public BuildingWall()
        {
            _mapHeight = ServerManager.MapHeight;
            _mapWidth = ServerManager.MapWidth;
            _walls = new GameObject[_mapHeight, _mapWidth][];
            _flags = new (uint ,GameObject)? [_mapHeight, _mapWidth];
            wallCount = new uint[_mapHeight, _mapWidth];
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

        public bool IsProcessed(int x, int y)
        {
            return _processed.Get(x + y * (int)_mapWidth);
        }

        public void Update()
        {
            OpenBitSet needUpdate = _ownerManager.NewOwner;
            var i = 0;
            OpenBitSet needFlagUpdate = new OpenBitSet();
            
            while ( (i = needUpdate.NextSetBit(i)) != -1)
            {
                int x =  (int)(i % _mapWidth);
                int y = (int)(i / _mapWidth);
                var sideIndex = 0;
                uint? owner = Owner(x, y);
                _processed.Set(i);
                
                foreach (var neighbourPos in WorldCoordinates.FindNeighboringTilesOrdered(x, y, _mapWidth, _mapHeight))
                {
                    
                    if (neighbourPos.HasValue)
                    {
                        var (nx, ny) = neighbourPos.Value;
                        ConsiderBuildWall(sideIndex, nx, ny , x , y , owner );
                        needFlagUpdate.Set(WorldCoordinates.ToIndex(nx, ny, _mapWidth));  
                    }
                    
                    sideIndex++;
                }
                needFlagUpdate.Set(WorldCoordinates.ToIndex(x, y, _mapWidth));

                
                needUpdate.Clear(i);
                i++;
            }
            NeedFlagUpdate(needFlagUpdate);
            
            _processed.Clear(0,i);
        }

        public void NeedFlagUpdate(OpenBitSet needFlagUpdate)
        {
            int i = 0;
            while ( (i = needFlagUpdate.NextSetBit(i)) != -1)
            {
                int x =  (int)(i % _mapWidth);
                int y = (int)(i / _mapWidth);
                uint? owner = Owner(x, y);

                if (wallCount[x,y] >= 3)
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
                    wallCount[x,y]--;
                    wallCount[nx,ny]--;
                }
                DestroyWall(sideIndex, x, y);
                DestroyWall(oppositeSideIndex, nx, ny);
                
                return;
            }
            
            
            if ( hasWall )
            {
                return;
            }

            wallCount[x,y]++;
            wallCount[nx,ny]++;
            var (wx, wy) = StaticGridTools.MapIndexToWorldCenterCo(x, y);
            var newWall = GameObject.Instantiate(_wallPrefab , new Vector3(wx, 0, wy), Quaternion.Euler(0, SideIndexToDegree(sideIndex), 0) );
            _walls[x , y ][sideIndex] = newWall; 
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
                DestroyFlagTupple(x, y, flagTupple);
                return;
            }

            if (flagTupple.HasValue)
            {
                if (flagTupple.Value.Item1 == owner)
                {
                    return;
                }
                DestroyFlagTupple(x,y, flagTupple);
            }
            (uint, GameObject)? RealFlag = (owner.Value , _buildingFlag.InstantiateFlag(x, y, owner.Value));
            _flags[x,y] = RealFlag;
        }

        private void DestroyFlagTupple(int x, int y, (uint, GameObject)? flagTupple)
        {
            Destroy(flagTupple.Value.Item2);
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