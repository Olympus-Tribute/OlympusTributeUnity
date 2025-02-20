using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ForNetwork;
using Networking.Common.Server;
using System.Collections.Generic;

public class RessourceManager : MonoBehaviour
{
    public TMP_Text populationText;
    public TMP_Text stoneText;
    public TMP_Text woodText;
    public TMP_Text goldText;
    public TMP_Text waterText;
    public TMP_Text wineText;
    public TMP_Text obsidianText;
    public TMP_Text diamondText;

    public Dictionary<string, Ressource> resources = new Dictionary<string, Ressource>()
    {
        { "Population", new Ressource("Population") },
        { "Stone", new Ressource("Stone") },
        { "Wood", new Ressource("Wood") },
        { "Gold", new Ressource("Gold") },
        { "Water", new Ressource("Water") },
        { "Wine", new Ressource("Wine") },
        { "Obsidian", new Ressource("Obsidian") },
        { "Diamond", new Ressource("Diamond") }
    };

    private Dictionary<string, TMP_Text> resourceTexts;

    void Start()
    {
        resourceTexts = new Dictionary<string, TMP_Text>
        {
            { "Population", populationText },
            { "Stone", stoneText },
            { "Wood", woodText },
            { "Gold", goldText },
            { "Water", waterText },
            { "Wine", wineText },
            { "Obsidian", obsidianText },
            { "Diamond", diamondText }
        };

        UpdateDisplay();
    }

    void OnEnable()
    {
        Debug.Log("Starting ResourceManager...");
        if (Network.Instance.Proxy != null)
        {
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerSetResourcesGameAction>(
                (connection, action) =>
                {
                    string[] names = new []{"Population", "Wood", "Stone", "Gold", "Diamond", "Obsidian", "Water", "Wine"};
                    for (int i = 0; i < action.Resources.Length; i++)
                    {
                        SetResource(names[i], action.Resources[i]);
                    }
                });
        }
    }

    
    //_________________________________//
    //_________________________________//
    //_________________________________//

    
    // pour Smooth
    public void Update()
    {
        foreach (var ressource in resources)
        {
            ressource.Value.Quantite.Update(Time.deltaTime);
        }

        UpdateDisplay();
    }
    
    //_________________________________//
    //_________________________________//
    //_________________________________//
    
    public void SetResource(string nom, uint valeur)
    {
        if (resources.ContainsKey(nom))
        {
            resources[nom].SetQuantite((int)valeur);
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        foreach (var ressource in resources)
        {
            if (resourceTexts.ContainsKey(ressource.Key))
            {
                resourceTexts[ressource.Key].text = $"{ressource.Key}: {(int)(Math.Round(ressource.Value.Quantite.currentValue))}";
            }
        }
    }
}
