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
            var h = ressources["Population"];
            int hint = (int)h.Quantite.targetValue;
            var b = ressources["Wood"];
            int bint = (int)b.Quantite.targetValue;
            var p = ressources["Stone"];
            int pint = (int)p.Quantite.targetValue;
            var or = ressources["Gold"];
            int orint = (int)or.Quantite.targetValue;
            var e = ressources["Water"];
            int eint = (int)e.Quantite.targetValue;
            var v = ressources["Wine"];
            int vint = (int)v.Quantite.targetValue;
            var o= ressources["Obsidian"];
            int oint = (int)o.Quantite.targetValue;
            var d = ressources["Diamond"];
            int dint = (int)d.Quantite.targetValue;
            switch (Nom)
            {
                case "Maison":
                    if (hint - 2 > 0 && bint - 10 > 0 && pint - 2 > 10)
                    {
                        return true;
                    }
                    return false;
                case "Extracteur":
                    if (hint - 5 > 0 && bint - 15 > 0 && pint - 15 > 0)
                    {
                        return true;
                    }
                    return false;
                case "ExtracteurPlusOr":
                    if (hint - 5 > 0 && bint - 15 > 0 && pint - 15 > 0 && orint - 20 > 0)
                    {
                        return true;
                    }
                    return false;
                case "Temple" :
                    if (hint - 20 > 0 && bint - 50 > 0 && pint - 50 > 0 && orint - 50 > 0)
                    {
                        return true;
                    }
                    return false;
                
                case "TempleOr" :
                    if (hint - 20 > 0 && bint - 50 > 0 && pint - 50 > 0 && orint - 75 > 0)
                    {
                        return true;
                    }
                    return false;
                case "TempleEau" :
                    if (hint - 20 > 0 && bint - 50 > 0 && pint - 50 > 0 && orint - 50 > 0 && eint - 75 > 0)
                    {
                        return true;
                    }
                    return false;
                case "TempleVigne" :
                    if (hint - 20 > 0 && bint - 50 > 0 && pint - 50 > 0 && orint - 50 > 0 && vint - 75 > 0)
                    {
                        return true;
                    }
                    return false;
                case "TempleDiamand" :
                    if (hint - 20 > 0 && bint - 50 > 0 && pint - 50 > 0 && orint - 50 > 0 && dint - 75 > 0)
                    {
                        return true;
                    }
                    return false;
                case "TempleObsidienne" :
                    if (hint - 20 > 0 && bint - 50 > 0 && pint - 50 > 0 && orint - 50 > 0 && oint - 75 > 0)
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