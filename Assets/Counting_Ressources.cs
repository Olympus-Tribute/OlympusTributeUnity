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
    public TMP_Text diamand;

    private int score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        habitants.text = "Habitants: " + score.ToString();
        pierre.text = "Pierre: " + score.ToString();
        bois.text = "Bois: " + score.ToString();
        or.text = "Or: " + score.ToString();
        eau.text = "Eau: " + score.ToString();
        vin.text = "Vin: " + score.ToString();
        obsidienne.text = "Obsidienne: " + score.ToString();
        diamand.text = "Diamand: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
