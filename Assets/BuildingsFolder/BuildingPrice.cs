using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace BuildingsFolder
{
    public class BuildingPrice  : MonoBehaviour
    {
        public GameObject Panel;
        private RessourceManager manager;

        public string Nom = "";

        
        public bool Ressource(Dictionary<string, Ressource> ressources)
        {
            int population = (int)ressources["Population"].Quantite.targetValue;
            int wood = (int)ressources["Wood"].Quantite.targetValue;
            int stone = (int)ressources["Stone"].Quantite.targetValue;
            int gold = (int)ressources["Gold"].Quantite.targetValue;
            int water = (int)ressources["Water"].Quantite.targetValue;
            int wine = (int)ressources["Wine"].Quantite.targetValue;
            int obsidian = (int)ressources["Obsidian"].Quantite.targetValue;
            int diamond = (int)ressources["Diamond"].Quantite.targetValue;
            
            switch (Nom)
            {
                case "Maison":
                    if (population - 2 > 0 && wood - 10 > 0 && stone - 2 > 10)
                    {
                        return true;
                    }
                    return false;
                case "Extracteur":
                    if (population - 5 > 0 && wood - 15 > 0 && stone - 15 > 0)
                    {
                        return true;
                    }
                    return false;
                case "ExtracteurPlusOr":
                    if (population - 5 > 0 && wood - 15 > 0 && stone - 15 > 0 && gold - 20 > 0)
                    {
                        return true;
                    }
                    return false;
                case "Temple" :
                    if (population - 20 > 0 && wood - 50 > 0 && stone - 50 > 0 && gold - 50 > 0)
                    {
                        return true;
                    }
                    return false;
                
                case "TempleOr" :
                    if (population - 20 > 0 && wood - 50 > 0 && stone - 50 > 0 && gold - 125 > 0)
                    {
                        return true;
                    }
                    return false;
                case "TempleEau" :
                    if (population - 20 > 0 && wood - 50 > 0 && stone - 50 > 0 && gold - 50 > 0 && water - 75 > 0)
                    {
                        return true;
                    }
                    return false;
                case "TempleVigne" :
                    if (population - 20 > 0 && wood - 50 > 0 && stone - 50 > 0 && gold - 50 > 0 && wine - 75 > 0)
                    {
                        return true;
                    }
                    return false;
                case "TempleDiamand" :
                    if (population - 20 > 0 && wood - 50 > 0 && stone - 50 > 0 && gold - 50 > 0 && diamond - 75 > 0)
                    {
                        return true;
                    }
                    return false;
                case "TempleObsidienne" :
                    if (population - 20 > 0 && wood - 50 > 0 && stone - 50 > 0 && gold - 50 > 0 && obsidian - 75 > 0)
                    {
                        return true;
                    }
                    return false;
            }
            
            
            return false;
        }
        
        private void Start()
        {
        }

        void Update()
        {
            Dictionary<string, Ressource> ressources= manager.resources;
            
            if (Ressource(ressources))
            {
                Panel.SetActive(false);
            }
            else
            {
                Panel.SetActive(true);
            }
        }
    }

}