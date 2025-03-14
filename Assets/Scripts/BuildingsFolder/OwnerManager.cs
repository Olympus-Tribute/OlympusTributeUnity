using System.Collections.Generic;


namespace BuildingsFolder
{
    public class OwnerManager
    {
        private readonly uint _mapWidth;
        private readonly uint _mapHeight;
        private readonly List<uint>[,] _globalMap;
        private readonly Dictionary<uint, uint[,]> _mapOfPlayer = new Dictionary<uint, uint[,]>();


        public OwnerManager(uint mapWidth, uint mapHeight)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _globalMap = new List<uint>[mapWidth, mapHeight];
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
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
                _globalMap[x, y].Add(owner);
            }
            map[x, y] ++;
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
                _globalMap[x, y].Remove(owner);
            }
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
        
        
        
        
    }
}