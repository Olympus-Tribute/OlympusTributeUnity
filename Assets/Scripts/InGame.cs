using ForNetwork;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("Scenes/Menus/MainMenu");
        Stop();
    }

    public static void Stop()
    {
        Network.Instance.Stop();
    }
}