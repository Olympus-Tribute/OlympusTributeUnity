using System;
using Resources;

public class Ressource
{
    public string Nom { get; private set; }
    public SmoothFloat Quantite { get; private set; }
    public int RealQuantite {get; private set;}
    public Ressource(string nom, int quantiteInitiale = 0)
    {
        Nom = nom;
        Quantite = new SmoothFloat(1, quantiteInitiale);
    }

    public void SetQuantite(int valeur)
    {
        //Quantite = Math.Max(0, valeur);
        Quantite.targetValue = valeur;
        RealQuantite = valeur;
    }
}