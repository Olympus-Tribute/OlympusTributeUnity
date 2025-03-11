using BuildingsFolder;
using TMPro;
using UnityEngine;

namespace InfoInGame
{
    public class InfoTilesInGame : MonoBehaviour
    {
        public GameObject InfoTileUi;
        public TMP_Text InfoTileText;
        private BuildingsManager _buildingsManager;
        
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
            if (_buildingsManager.Buildings.TryGetValue((x,z),out var building))
            {
                ShowInfoTile($"{building.Name}");
            }
        }

        private void Awake()
        {
            _buildingsManager = FindObjectOfType<BuildingsManager>();
            InfoTileUi.SetActive(true);
        }
        
        private void OnEnable()
        {
            SetPopUpInactive();
            _timing = 0;
        }

        private float _timing;
        
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