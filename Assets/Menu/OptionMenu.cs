using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public void Option1()
    {
        Debug.Log("Option1");
    }
    public void Option2()
    {
        Debug.Log("Option2");
    }

    public void Option3()
    {
        Debug.Log("Option3");
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }
}