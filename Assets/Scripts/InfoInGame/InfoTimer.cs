using System;
using System.Collections.Generic;
using BuildingsFolder;
using TMPro;
using UnityEngine;

namespace InfoInGame
{
    public class InfoTimer : MonoBehaviour
    {
        public GameObject infoTimerGameObject;
        public TMP_Text infoTimerText;

        private float _timing;
        private bool _timerEnded = false;

        public void OnEnable()
        {
            if (GameConstants.TimerModeIsActive)
            {
                infoTimerGameObject.SetActive(true);
                if (GameConstants.TimerSetByHostInMin <= 0)
                {
                    GameConstants.TimerSetByHostInMin = 60;
                }
                _timing = GameConstants.TimerSetByHostInMin * 60f;
            }
            else
            {
                infoTimerGameObject.SetActive(false);
                _timing = 0f;
                Destroy(this.gameObject);
            }
        }

        private void Update()
        {
            if (_timerEnded) return;

            _timing -= Time.deltaTime;

            if (_timing <= 0f)
            {
                _timing = 0f;
                _timerEnded = true;
                TimerEnded();
            }
            
            int hours = Mathf.FloorToInt(_timing / 3600);
            int minutes = Mathf.FloorToInt((_timing % 3600) / 60);
            int seconds = Mathf.FloorToInt(_timing % 60);

            if (hours > 0)
            {
                infoTimerText.text = $"Time left : {hours:D2}h {minutes:D2}m {seconds:D2}s";
            }
            else
            {
                if (minutes > 0)
                {
                    infoTimerText.text = $"Time left : {minutes:D2}m {seconds:D2}s";
                }
                else
                {
                    infoTimerText.text = $"Time left : {seconds:D2}s";
                }
                
            }
            
        }

        private void TimerEnded()
        {
            Debug.Log("Timer finished!");
        }
    }
}