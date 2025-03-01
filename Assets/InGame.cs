using ForNetwork;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class InGame : MonoBehaviour
    {
        public GameObject menuInGame;

        private void Start()
        {
            menuInGame.SetActive(false);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuInGame.SetActive(!menuInGame.activeSelf);
            }
        }
        
        public void CloseMenuInGame()
        {
            menuInGame.SetActive(false);
        }
        
        public void QuitGame()
        {
            menuInGame.SetActive(false);
            SceneManager.LoadScene("MainMenu");
            Network.Instance.Proxy.Connection.Disconnect();
        }
    }
}