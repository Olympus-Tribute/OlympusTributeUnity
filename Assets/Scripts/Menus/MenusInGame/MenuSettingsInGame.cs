using ForNetwork;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus.MenusInGame
{
    public class MenuSettingsInGame : MonoBehaviour
    {
        public GameObject menuInGame;
        
        private CameraController mainCamera;
    
        private void Start()
        {
            menuInGame.SetActive(false);
            mainCamera = FindFirstObjectByType<CameraController>();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuInGame.SetActive(!menuInGame.activeSelf);
                mainCamera.isActive = !menuInGame.activeSelf;
            }
        }
        
        public void CloseMenuInGame()
        {
            menuInGame.SetActive(false);
            mainCamera.isActive = true;
        }
        
        public void QuitGame()
        {
            menuInGame.SetActive(false);
            SceneManager.LoadScene("Scenes/Menus/MainMenu");
            Stop();
        }

        public static void Stop()
        {
            Network.Instance.Stop();
        }
    }
}