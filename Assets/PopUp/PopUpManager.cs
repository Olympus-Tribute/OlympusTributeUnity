using System;
using System.Threading;
using TMPro;
using UnityEngine;

namespace PopUp
{
    public class PopUpManager : MonoBehaviour
    {
        public GameObject PopUpUi;
        public TMP_Text PopUpText;

        public static PopUpManager Instance;

        private float _timing;

        private void Update()
        {
            if (PopUpUi.activeSelf)
            {
                _timing -= Time.deltaTime;
                if (_timing <= 0)
                {
                    SetPopUpInactive();
                }
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        
        private void Start()
        {
            SetPopUpInactive();
            _timing = 0;
        }

        public void SetPopUpInactive()
        {
            PopUpUi.SetActive(false);
        }

        public void ShowPopUp(string message, float timeToShowInSeconds = 1)
        {
            PopUpText.SetText(message);
            PopUpUi.SetActive(true);
            _timing = timeToShowInSeconds;
        }
    }
}