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

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        
        private void Start()
        {
            //PopUpText = PopUpUi.AddComponent<TextMeshPro>();
            SetPopUpInactive();
        }

        public void SetPopUpInactive()
        {
            PopUpUi.SetActive(false);
        }

        public void ShowPopUp(string message, int timeToShowInSeconds = 1)
        {
            PopUpText.SetText(message);
            PopUpUi.SetActive(true);
            Thread.Sleep(timeToShowInSeconds*1000);
            PopUpUi.SetActive(false);
        }
    }
}