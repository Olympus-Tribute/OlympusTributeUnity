using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Counting_Ressources : MonoBehaviour
{
    public TMP_Text habitants;
    public TMP_Text pierre;
    public TMP_Text bois;
    public TMP_Text or;
    public TMP_Text eau;
    public TMP_Text vin;
    public TMP_Text obsidienne;
    public TMP_Text diamant;

    private int score_habitant = 0;
    private int score_pierre = 0;
    private int score_bois = 0;
    private int score_or = 0;
    private int score_eau = 0;
    private int score_vin = 0;
    private int score_obsidienne = 0;
    private int score_diamant = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        habitants.text = "Habitants: " + score_habitant.ToString();
        pierre.text = "Pierre: " + score_pierre.ToString();
        bois.text = "Bois: " + score_bois.ToString();
        or.text = "Or: " + score_or.ToString();
        eau.text = "Eau: " + score_eau.ToString();
        vin.text = "Vin: " + score_vin.ToString();
        obsidienne.text = "Obsidienne: " + score_obsidienne.ToString();
        diamant.text = "Diamant: " + score_diamant.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
