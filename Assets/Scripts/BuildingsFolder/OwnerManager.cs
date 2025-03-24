using System;
using System.Collections.Generic;
using Cecs.Util;
using ForServer;
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
            _mapWidth = ServerManager.MapWidth;
            _mapHeight = ServerManager.MapHeight;
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
        /*
        public OwnerManager(uint mapWidth, uint mapHeight, HexMapGenerator map)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _globalMap = new List<uint>[mapWidth, mapHeight];
            _numberTilesPlayable = (int)((_mapWidth * _mapHeight) - CountTilesOfOcean());
            _mapOfPlayer = new Dictionary<uint, uint[,]>();
            PercentagePerPlayer = new Dictionary<uint, float>();
            _map = map;
            
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    _globalMap[i, j] = new List<uint>();
                }
            }
        }
        
        */
        
        
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

            UpdatePercentageOfTileWithOcean();
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

            UpdatePercentageOfTileWithOcean();
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

        private void UpdatePercentageOfTileWithOcean()
        {
            Dictionary<uint, uint> tileCountPerPlayer = new Dictionary<uint, uint>();
            uint totalTiles = _mapWidth * _mapHeight;

       
            foreach (var player in _mapOfPlayer)
            {
                uint playerId = player.Key;
                uint[,] playerMap = player.Value;
                uint count = 0;
                
                for (int i = 0; i < _mapWidth; i++)
                {
                    for (int j = 0; j < _mapHeight; j++)
                    {
                        if (playerMap[i, j] > 0)
                        {
                            count++;
                        }
                    }
                }
                
                if (count > 0)
                {
                    tileCountPerPlayer[playerId] = count;
                }
            }
            
            foreach (var entry in tileCountPerPlayer)
            {
                PercentagePerPlayer[entry.Key] = (entry.Value / (float)totalTiles) * 100f;
            }
        }
        
        private void UpdatePercentageOfTileWithoutOcean()
        {
            Dictionary<uint, uint> tileCountPerPlayer = new Dictionary<uint, uint>();
            uint totalTiles = _mapWidth * _mapHeight;

       
            foreach (var player in _mapOfPlayer)
            {
                uint playerId = player.Key;
                uint[,] playerMap = player.Value;
                uint count = 0;
                
                for (int i = 0; i < _mapWidth; i++)
                {
                    for (int j = 0; j < _mapHeight; j++)
                    {
                        if (!(_map[i, j] is FloorTile.Ocean) && playerMap[i, j] > 0)
                        {
                            count++;
                        }
                    }
                }
                
                if (count > 0)
                {
                    tileCountPerPlayer[playerId] = count;
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