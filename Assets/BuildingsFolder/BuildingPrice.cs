using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
namespace BuildingsFolder
{
    public class BuildingPrice  : MonoBehaviour
    {

        public TMP_Text house_wood;
        public TMP_Text house_stone;
        public TMP_Text house_people;
        
        public TMP_Text extractor_wood;
        public TMP_Text extractor_stone;
        public TMP_Text extractor_gold;
        public TMP_Text extracteur_people;
        
        public TMP_Text temple_wood;
        public TMP_Text temple_stone;
        public TMP_Text temple_water;
        public TMP_Text temple_obsidian;
        public TMP_Text temple_wine;
        public TMP_Text temple_gold;
        public TMP_Text templegold_gold;
        public TMP_Text temple_diamond;
        public TMP_Text temple_other;
        public TMP_Text temple_people;

        void OnEnable()
        {
            house_wood.text = "x10";
            house_stone.text = "x10";
            house_people.text = "x2";
            
            
            extractor_wood.text = "x15";
            extractor_stone.text = "x15";
            extractor_gold.text = "x20";
            house_people.text = "x5";
            
            temple_wood.text = "x50";
            temple_stone.text = "x50";
            temple_gold.text = "x50";
            temple_people.text = "x20";
            
            templegold_gold.text = "x75";
            temple_water.text = "x75";
            temple_wine.text = "x75";
            temple_diamond.text = "x75";
            temple_obsidian.text = "x75";
            temple_other.text = "...x75";
        }

    }

}