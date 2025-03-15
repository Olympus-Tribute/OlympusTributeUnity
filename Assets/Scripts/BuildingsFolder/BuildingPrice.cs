using UnityEngine;
using Resources;
using static BuildingsFolder.AllPrices;
using ResourceType = Resources.ResourceType;

namespace BuildingsFolder
{
    public class BuildingPrice  : MonoBehaviour
    {
        public GameObject Panel;

        public string Nom = "";

        public bool Ressource()
        {
            var ressources = RessourceManager.Instance.resources;
            
            
            int population = (int)ressources[ResourceType.Population].RealQuantite;
            int wood = (int)ressources[ResourceType.Wood].RealQuantite;
            int stone = (int)ressources[ResourceType.Stone].RealQuantite;
            int gold = (int)ressources[ResourceType.Gold].RealQuantite;
            int water = (int)ressources[ResourceType.Water].RealQuantite;
            int vine = (int)ressources[ResourceType.Vine].RealQuantite;
            int obsidian = (int)ressources[ResourceType.Obsidian].RealQuantite;
            int diamond = (int)ressources[ResourceType.Diamond].RealQuantite;
            
            switch (Nom)
            {
                case "House":
                    return wood >= HouseWoodPrice && stone >= HouseStonePrice;
                case "WoodExtractor":
                    return wood >= WoodExtractorWoodPrice && population >= WoodExtractorPopulationPrice;
                case "StoneExtractor":
                    return stone >= StoneExtractorStonePrice && population >= StoneExtractorPopulationPrice;
                case "GoldExtractor":
                    return wood >= GoldExtractorWoodPrice && stone >= GoldExtractorStonePrice && population >= GoldExtractorPopulationPrice;
                case "WaterExtractor":
                    return wood >= WaterExtractorWoodPrice && stone >= WaterExtractorStonePrice && gold >= WaterExtractorGoldPrice && population >= WaterExtractorPopulationPrice;
                case "VineExtractor":
                    return wood >= VineExtractorWoodPrice && stone >= VineExtractorStonePrice && gold >= VineExtractorGoldPrice && population >= VineExtractorPopulationPrice;
                case "DiamondExtractor":
                    return wood >= DiamondExtractorWoodPrice && stone >= DiamondExtractorStonePrice && gold >= DiamondExtractorGoldPrice && population >= DiamondExtractorPopulationPrice;
                case "ObsidianExtractor":
                    return wood >= ObsidianExtractorWoodPrice && stone >= ObsidianExtractorStonePrice && gold >= ObsidianExtractorGoldPrice && population >= ObsidianExtractorPopulationPrice;
                case "Zeus" :
                    return population >= ZeusTemplePopulationPrice && wood >= ZeusTempleWoodPrice && stone >= ZeusTempleStonePrice && gold >= ZeusTempleGoldPrice;
                case "Poseidon" :
                    return population >= PoseidonTemplePopulationPrice && wood >= PoseidonTempleWoodPrice && stone >= PoseidonTempleStonePrice && gold >= PoseidonTempleGoldPrice && water >= PoseidonTempleWaterPrice;
                case "Dionysus" :
                    return population >= DionysusTemplePopulationPrice && wood >= DionysusTempleWoodPrice && stone >= DionysusTempleStonePrice && gold >= DionysusTempleGoldPrice && vine >= DionysusTempleVinePrice;
                case "Athena" :
                    return population >= AthenaTemplePopulationPrice && wood >= AthenaTempleWoodPrice && stone >= AthenaTempleStonePrice && gold >= AthenaTempleGoldPrice && diamond >= AthenaTempleDiamondPrice;
                case "Hades" :
                    return population >= HadesTemplePopulationPrice && wood >= HadesTempleWoodPrice && stone >= HadesTempleStonePrice && gold >= HadesTempleGoldPrice && obsidian >= HadesTempleObsidianPrice;
                default:
                    return true;
            }
        }
        
        void Update()
        {
            if (Ressource())
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