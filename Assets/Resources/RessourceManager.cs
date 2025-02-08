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
    
    public Network Network = Network.Instance;
    public bool networkActive;

    private void Awake()
    {
        networkActive = Network.Instance != null && Network.Instance.networkActive;
        Debug.Log($"Network active: {networkActive}");
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
        /*
        if (networkActive)
        {
            Debug.Log("Starting RessourceManager...");
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerGetRessourcesGameAction>(
                (connection, action) =>
                {
                    for (int i = 0; i < action.NameRessources.Length; i++)
                    {
                        SetRessource(action.NameRessources[i], action.NbrRessources[i]);
                    }
                });
        }
        else
        {
            Debug.LogWarning("Network is not active or Proxy is null.");
        }
        */
    }
    
    public void SetRessource(string nom, int valeur)
    {
        if (ressources.ContainsKey(nom))
        {
            ressources[nom].SetQuantite(valeur);
            MettreAJourAffichage();
        }
    }

    private void MettreAJourAffichage()
    {
        foreach (var ressource in ressources)
        {
            if (ressourceTexts.ContainsKey(ressource.Key))
            {
                ressourceTexts[ressource.Key].text = $"{ressource.Key}: {ressource.Value.Quantite}";
            }
        }
    }
}
