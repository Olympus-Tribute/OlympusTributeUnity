using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ForNetwork;
using Networking.Common.Server;
using System.Collections.Generic;

public class RessourceManager : MonoBehaviour
{
    public TMP_Text habitantsText;
    public TMP_Text pierreText;
    public TMP_Text boisText;
    public TMP_Text orText;
    public TMP_Text eauText;
    public TMP_Text vinText;
    public TMP_Text obsidienneText;
    public TMP_Text diamantText;

    private Dictionary<string, Ressource> ressources = new Dictionary<string, Ressource>()
    {
        { "Habitants", new Ressource("Habitants") },
        { "Pierre", new Ressource("Pierre") },
        { "Bois", new Ressource("Bois") },
        { "Or", new Ressource("Or") },
        { "Eau", new Ressource("Eau") },
        { "Vin", new Ressource("Vin") },
        { "Obsidienne", new Ressource("Obsidienne") },
        { "Diamant", new Ressource("Diamant") }
    };

    private Dictionary<string, TMP_Text> ressourceTexts;
    
    private void Awake()
    {

    }

    void Start()
    {
        ressourceTexts = new Dictionary<string, TMP_Text>
        {
            { "Habitants", habitantsText },
            { "Pierre", pierreText },
            { "Bois", boisText },
            { "Or", orText },
            { "Eau", eauText },
            { "Vin", vinText },
            { "Obsidienne", obsidienneText },
            { "Diamant", diamantText }
        };

        MettreAJourAffichage();
    }

    void OnEnable()
    {
        Debug.Log("Starting RessourceManager...");
        if (Network.Instance.Proxy != null)
        {
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerSetResourcesGameAction>(
                (connection, action) =>
                {
                    string[] name = new []{"Bois", "Pierre", "Or", "Diamant"};
                    for (int i = 0; i < action.Resources.Length; i++)
                    {
                        SetRessource(name[i], action.Resources[i]);
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
        foreach (var ressource in ressources)
        {
            ressource.Value.Quantite.Update(Time.deltaTime);
        }
        MettreAJourAffichage();
    }
    
    //_________________________________//
    //_________________________________//
    //_________________________________//
    
    public void SetRessource(string nom, uint valeur)
    {
        if (ressources.ContainsKey(nom))
        {
            ressources[nom].SetQuantite((int)valeur);
            MettreAJourAffichage();
        }
    }

    private void MettreAJourAffichage()
    {
        foreach (var ressource in ressources)
        {
            if (ressourceTexts.ContainsKey(ressource.Key))
            {
                ressourceTexts[ressource.Key].text = $"{ressource.Key}: {(int)(Math.Round(ressource.Value.Quantite.currentValue))}";
            }
        }
    }
}
