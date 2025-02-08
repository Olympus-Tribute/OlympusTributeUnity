using System;

public class Ressource
{
    public string Nom { get; private set; }
    public int Quantite { get; private set; }

    public Ressource(string nom, int quantiteInitiale = 0)
    {
        Nom = nom;
        Quantite = Math.Max(0, quantiteInitiale);
    }

    public void Ajouter(int valeur)
    {
        Quantite += valeur;
    }

    public void Retirer(int valeur)
    {
        Quantite = Math.Max(0, Quantite - valeur);
    }

    public void SetQuantite(int valeur)
    {
        Quantite = Math.Max(0, valeur);
    }
}