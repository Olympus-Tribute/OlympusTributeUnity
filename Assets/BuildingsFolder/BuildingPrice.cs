using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using OlympusDedicatedServer.Components.Resources;
using UnityEngine.Rendering;

namespace BuildingsFolder
{
    public class BuildingPrice  : MonoBehaviour
    {
        public GameObject Panel;
        private RessourceManager manager;

        public string Nom = "";

        public void Start()
        {
            manager = new RessourceManager();
        }
        
        public bool Ressource(Dictionary<ResourceType, Ressource> ressources)
        {
            int population = (int)ressources[ResourceType.Population].RealQuantite;
            int wood = (int)ressources[ResourceType.Wood].RealQuantite;
            int stone = (int)ressources[ResourceType.Stone].RealQuantite;
            int gold = (int)ressources[ResourceType.Gold].RealQuantite;
            int water = (int)ressources[ResourceType.Water].RealQuantite;
            int wine = (int)ressources[ResourceType.Vine].RealQuantite;
            int obsidian = (int)ressources[ResourceType.Obsidian].RealQuantite;
            int diamond = (int)ressources[ResourceType.Diamond].RealQuantite;
            
            switch (Nom)
            {
                case "Maison":
                    if (population - 2 > 0 && wood - 10 > 0 && stone - 2 > 10)
                    {
                        return true;
                    }
                    return false;
                case "Extracteur":
                    /*
                    if (population - 5 > 0 && wood - 15 > 0 && stone - 15 > 0)
                    {
                        return true;
                    }
                    return false;
                    */
                    return true;
                case "ExtracteurPlusOr":
                    /*
                    if (population - 5 > 0 && wood - 15 > 0 && stone - 15 > 0 && gold - 20 > 0)
                    {
                        return true;
                    }
                    return false;
                    */
                return true;
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
        
        void Update()
        {
            Dictionary<ResourceType, Ressource> ressources= manager.resources;
            
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