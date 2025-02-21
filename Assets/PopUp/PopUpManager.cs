using TMPro;
using UnityEngine;

namespace PopUp
{
    public class PopUpManager : MonoBehaviour
    {
        public GameObject PopUpUi;
        public TMP_Text PopUpText;


        private void Start()
        {
            PopUpText = gameObject.AddComponent<TextMeshPro>();
            PopUpUi = new GameObject();
            SetPopUpInactive();
        }

        public void SetPopUpInactive()
        {
            PopUpUi.SetActive(false);
        }

        public void ShowPopUp(string message)
        {
            PopUpText.SetText(message);
            PopUpUi.SetActive(true);
        }
    }
}