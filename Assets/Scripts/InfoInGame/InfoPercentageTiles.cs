using System;
using System.Collections.Generic;
using BuildingsFolder;
using ForServer;
using Grid;
using TMPro;
using UnityEngine;

namespace InfoInGame
{
    public class InfoPercentageTiles : MonoBehaviour
    {
        public GameObject infoPercentageTileUi;
        public TMP_Text infoPercentageTileText;
        
        private OwnerManager _ownerManager;
        
        
        public void Start()
        {
            infoPercentageTileUi.SetActive(true);
            _ownerManager = FindFirstObjectByType<OwnerManager>();
        }
        
        public void Update()
        {
            ShowInfoTile();
        }

        private void ShowInfoTile()
        {
            if (_ownerManager is null)
            {
                return;
            }
            
            string res = String.Empty;

            foreach (KeyValuePair<uint,float> percentagePlayer in _ownerManager.PercentagePerPlayer)
            {
                res += $"{OwnersMaterial.GetName(percentagePlayer.Key)} : {percentagePlayer.Value:F2} %\n" ;
            }
            
            infoPercentageTileText.SetText(res);
            infoPercentageTileUi.SetActive(true);
        }
        
    }
}