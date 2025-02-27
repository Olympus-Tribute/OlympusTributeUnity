using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void Start()
    {
        CreateSteamAppIdFile();
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    private static void CreateSteamAppIdFile()
    {
        string filePath = "steam_appid.txt";
        
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "480");
            
        }
    }
}
