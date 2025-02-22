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

        public void ShowPopUp(string message)
        {
            PopUpUi.SetActive(true);
            PopUpText.SetText(message);
        }
    }
}