using BuildingsFolder;
using ForServer;
using Grid;
using TMPro;
using UnityEngine;

namespace InfoInGame
{
    public class InfoTilesInGame : MonoBehaviour
    {
        public GameObject InfoTileUi;
        public TMP_Text InfoTileText;
        private BuildingsManager _buildingsManager;

        private GridGenerator _map;
        private float _timing;
        
        public void Start()
        {
            InfoTileUi.SetActive(true);
            _buildingsManager = FindFirstObjectByType<BuildingsManager>();
            _map = FindFirstObjectByType<GridGenerator>();
            _timing = 0;
        }
        
        public void Update()
        {
            if (InfoTileUi.activeSelf)
            {
                _timing -= Time.deltaTime;
                if (_timing <= 0)
                {
                    SetPopUpInactive();
                }
            }
            
            var mapIndex  = MousePositionTracker.Instance.GetMouseMapIndexCo();
            if (!mapIndex.HasValue)
            {
                return;
            }
            
            (int x, int z) = mapIndex.Value;

            if (0 <= x && x < ServerManager.MapWidth && 0 <= z && z < ServerManager.MapHeight)
            {
                ShowInfoTile(_map.MapGenerator[x, z].ToString());
            }
            
            if (_buildingsManager.Buildings.TryGetValue((x,z),out var building))
            {
                ShowInfoTile($"{building.Name}");
            }
        }
        
        public void SetPopUpInactive()
        {
            InfoTileUi.SetActive(false);
        }

        public void ShowInfoTile(string message, float timeToShowInSeconds = 0.5f)
        {
            InfoTileText.SetText(message);
            InfoTileUi.SetActive(true);
            _timing = timeToShowInSeconds;
        }
    }
}