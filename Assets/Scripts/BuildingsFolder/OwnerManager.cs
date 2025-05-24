using System;
using System.Collections.Generic;
using Cecs.Util;
using Grid;
using OlympusDedicatedServer.Components.WorldComp;
using OlympusWorldGenerator;
using UnityEngine;

namespace BuildingsFolder
{
    public class OwnerManager : MonoBehaviour
    {
        private uint _mapWidth;
        private uint _mapHeight;
        private Dictionary<uint, uint[,]> _mapOfPlayer;
        private List<uint>[,] _globalMap;
        private HexMapGenerator _map;
        private int _numberTilesPlayable;
        public readonly OpenBitSet NewOwner = new OpenBitSet();
        public Dictionary<uint, float> PercentagePerPlayer;


        public void Start()
        {
            _mapWidth = GameConstants.MapWidth;
            _mapHeight = GameConstants.MapHeight;
            _globalMap = new List<uint>[_mapWidth, _mapHeight];
            _mapOfPlayer = new Dictionary<uint, uint[,]>();
            PercentagePerPlayer = new Dictionary<uint, float>();
            _map = FindFirstObjectByType<GridGenerator>().MapGenerator;
            
            _numberTilesPlayable = (int)((_mapWidth * _mapHeight) - CountTilesOfOcean());
            for (int i = 0; i < _mapWidth; i++)
            {
                for (int j = 0; j < _mapHeight; j++)
                {
                    _globalMap[i, j] = new List<uint>();
                }
            }
        }
        
        public void AddOwner(int x, int y, uint owner)
        {
            if (!_mapOfPlayer.TryGetValue(owner, out uint[,] map))
            {
                _mapOfPlayer[owner] = map = new uint[_mapWidth, _mapHeight];
            }
            if (map[x, y] == 0)
            {
                if (_globalMap[x, y].Count == 0)
                {
                    NewOwner.Set(WorldCoordinates.ToIndex(x, y, _mapWidth));
                }
                
                _globalMap[x, y].Add(owner);
                
            }
            map[x, y] ++;
            
            UpdatePercentageOfTileWithoutOcean();
        }

        public void RemoveOwner(int x, int y, uint owner)
        {
            if (!_mapOfPlayer.TryGetValue(owner, out uint[,] map))
            {
                _mapOfPlayer[owner] = map = new uint[_mapWidth, _mapHeight];
            }
            map[x, y] --;
            if (map[x, y] == 0)
            {
                uint ? currentOwner= GetOwner(x, y);
                if (currentOwner == owner)
                {
                    NewOwner.Set(WorldCoordinates.ToIndex(x, y, _mapWidth));
                }
                _globalMap[x, y].Remove(owner);
            }

            UpdatePercentageOfTileWithoutOcean();
        }

        public uint? GetOwner(int x, int y)
        {
            List<uint> tileOwner = _globalMap[x, y];
            if (tileOwner.Count == 0)
            {
                return null;
            }
            return tileOwner[0];
        }
        
        private void UpdatePercentageOfTileWithoutOcean()
        {
            Dictionary<uint, uint> tileCountPerPlayer = new Dictionary<uint, uint>();
            
            for (int i = 0; i < _mapWidth; i++)
            {
                for (int j = 0; j < _mapHeight; j++)
                {
                    if (!(_map[i, j] is FloorTile.Ocean))
                    {
                        uint? owner = GetOwner(i, j);
                        
                        if (owner.HasValue)
                        {
                            tileCountPerPlayer.TryAdd(owner.Value, 0);
                            tileCountPerPlayer[owner.Value] ++;
                        }
                    }
                }
            }
                
            foreach (var entry in tileCountPerPlayer)
            {
                PercentagePerPlayer[entry.Key] = (entry.Value / (_numberTilesPlayable + 0f)) * 100f;
            }
        }
        
        private int CountTilesOfOcean()
        {
            int count = 0;
            for (int i = 0; i < _mapWidth; i++)
            {
                for (int j = 0; j < _mapHeight; j++)
                {
                    if (_map[i, j] is FloorTile.Ocean)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}