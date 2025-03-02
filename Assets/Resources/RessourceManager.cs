using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ForNetwork;
using Networking.Common.Server;
using System.Collections.Generic;
using OlympusDedicatedServer.Components.Resources;

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
    
    public static RessourceManager Instance;

    public Dictionary<ResourceType, Ressource> resources = new Dictionary<ResourceType, Ressource>()
    {
        { ResourceType.Population, new Ressource("Population") },
        { ResourceType.Stone, new Ressource("Stone") },
        { ResourceType.Wood, new Ressource("Wood") },
        { ResourceType.Gold, new Ressource("Gold") },
        { ResourceType.Water, new Ressource("Water") },
        { ResourceType.Vine, new Ressource("Grapes") },
        { ResourceType.Obsidian, new Ressource("Obsidian") },
        { ResourceType.Diamond, new Ressource("Diamond") }
    };

    private Dictionary<ResourceType, TMP_Text> resourceTexts;

    void Start()
    {
        Instance = this;
        
        resourceTexts = new Dictionary<ResourceType, TMP_Text>
        {
            { ResourceType.Population, populationText },
            { ResourceType.Stone, stoneText },
            { ResourceType.Wood, woodText },
            { ResourceType.Gold, goldText },
            { ResourceType.Water, waterText },
            { ResourceType.Vine, wineText },
            { ResourceType.Obsidian, obsidianText },
            { ResourceType.Diamond, diamondText }
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
                    
                    for (int i = 0; i < action.Resources.Length; i++)
                    {
                        SetResource((ResourceType)i, action.Resources[i]);
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
    
    public void SetResource(ResourceType nom, uint valeur)
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
            resourceTexts[ressource.Key].text = $"{ressource.Value.Nom}: {(int)(Math.Round(ressource.Value.Quantite.currentValue))}";
        }
    }
}
