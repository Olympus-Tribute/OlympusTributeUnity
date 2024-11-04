using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void Start()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
