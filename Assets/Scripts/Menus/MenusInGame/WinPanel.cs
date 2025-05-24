using System;
using System.Collections.Generic;
using System.Linq;
using BuildingsFolder;
using ForNetwork;
using Networking.API.Listeners;
using Networking.Common.Server;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus.MenusInGame
{
    public class WinPanel : MonoBehaviour
    {
        [SerializeField] public GameObject winPanelGameObject;
        [SerializeField] public TMP_Text winnerText;
        [SerializeField] public TMP_Text statsGame;
        
        private double[] playerPercentage;
        private GameActionListener<ServerGameStopGameAction> listener;


        void OnEnable()
        {
            listener = Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerGameStopGameAction>(
                (connection, action) =>
                {
                    Debug.Log("[CLIENT]     : Receive 'ServerGameStop'");
                    playerPercentage = action.Leaderboards;
                    FinishGame();
                });


            winPanelGameObject.SetActive(false);
        }

        void OnDisable()
        {
            Network.Instance.Proxy.GameActionListenerManager.RemoveListener(listener);
        }

        private void FinishGame()
        {
            winPanelGameObject.SetActive(true);
            statsGame.text = GetStatsGame();
        }
        
        public void QuitGame()
        {
            winPanelGameObject.SetActive(false);
            SceneManager.LoadScene("Scenes/Menus/MainMenu");
            Network.Instance.Stop();
        }
        
        public void RestartANewGame()
        {
            winPanelGameObject.SetActive(false);
            SceneManager.LoadScene("WaitingScene");
        }

        private string GetStatsGame()
        {
            string getStatsGame = String.Empty;
            double max = playerPercentage.Max();
            int i = 0;
            foreach (var percentageOfPlayer in playerPercentage)
            {
                if (Math.Abs(percentageOfPlayer - max) < 0.01)
                {
                    winnerText.text = "You Win!";
                }
                else
                {
                    winnerText.text = "You Lose!";
                }
                getStatsGame += $"{OwnersMaterial.GetName((uint)i)} : {percentageOfPlayer*100:F2}%\n";
                i += 1;
            }
            return getStatsGame;
        }
        
        
    }
}