using System.Collections.Generic;
using Resources;

namespace BuildingsFolder
{
    public static class AllPrices
    {
        public const float ZeusParalyzeDuration = 30f;
        public const float DionysusParalyzeDuration = 120f;
        public const float PoseidonParalyzeDuration = 15f;
        
        // --- Ressources de départ ---
        public const int StartingWood = 0;
        public const int StartingStone = 0;
        public const int StartingGold = 0;
        public const int StartingWater = 0;
        public const int StartingVine = 0;
        public const int StartingDiamond = 0;
        public const int StartingObsidian = 0;
        public const int StartingPopulation = 0;

        // --- Maison ---
        public const int HouseWoodPrice = 50;
        public const int HouseStonePrice = 50;

        // --- Extracteurs ---
        public const int WoodExtractorWoodPrice = 15;
        public const int WoodExtractorPopulationPrice = 10;

        public const int StoneExtractorStonePrice = 15;
        public const int StoneExtractorPopulationPrice = 10;

        public const int GoldExtractorWoodPrice = 15;
        public const int GoldExtractorStonePrice = 15;
        public const int GoldExtractorPopulationPrice = 10;

        public const int WaterExtractorWoodPrice = 15;
        public const int WaterExtractorStonePrice = 15;
        public const int WaterExtractorGoldPrice = 20;
        public const int WaterExtractorPopulationPrice = 10;

        public const int VineExtractorWoodPrice = 15;
        public const int VineExtractorStonePrice = 15;
        public const int VineExtractorGoldPrice = 20;
        public const int VineExtractorPopulationPrice = 10;

        public const int DiamondExtractorWoodPrice = 15;
        public const int DiamondExtractorStonePrice = 15;
        public const int DiamondExtractorGoldPrice = 20;
        public const int DiamondExtractorPopulationPrice = 10;

        public const int ObsidianExtractorWoodPrice = 15;
        public const int ObsidianExtractorStonePrice = 15;
        public const int ObsidianExtractorGoldPrice = 20;
        public const int ObsidianExtractorPopulationPrice = 10;

        // --- Temples ---
        public const int ZeusTempleWoodPrice = 50;
        public const int ZeusTempleStonePrice = 50;
        public const int ZeusTempleGoldPrice = 125;
        public const int ZeusTemplePopulationPrice = 25;

        public const int PoseidonTempleWoodPrice = 50;
        public const int PoseidonTempleStonePrice = 50;
        public const int PoseidonTempleGoldPrice = 50;
        public const int PoseidonTempleWaterPrice = 75;
        public const int PoseidonTemplePopulationPrice = 25;

        public const int DionysusTempleWoodPrice = 50;
        public const int DionysusTempleStonePrice = 50;
        public const int DionysusTempleGoldPrice = 50;
        public const int DionysusTempleVinePrice = 75;
        public const int DionysusTemplePopulationPrice = 25;

        public const int AthenaTempleWoodPrice = 50;
        public const int AthenaTempleStonePrice = 50;
        public const int AthenaTempleGoldPrice = 50;
        public const int AthenaTempleDiamondPrice = 75;
        public const int AthenaTemplePopulationPrice = 25;

        public const int HadesTempleWoodPrice = 50;
        public const int HadesTempleStonePrice = 50;
        public const int HadesTempleGoldPrice = 50;
        public const int HadesTempleObsidianPrice = 75;
        public const int HadesTemplePopulationPrice = 25;

        // --- Attaques ---

        // Zeus : Bois=25, Pierre=25, Or=50
        public const int ZeusAttackWoodPrice = 25;
        public const int ZeusAttackStonePrice = 25;
        public const int ZeusAttackGoldPrice = 50;

        // Poseidon : Bois=75, Pierre=75, Eau=50
        public const int PoseidonAttackWoodPrice = 75;
        public const int PoseidonAttackStonePrice = 75;
        public const int PoseidonAttackWaterPrice = 50;

        // Dyonisos : Bois=20, Pierre=20, Vigne=50
        public const int DyonisosAttackWoodPrice = 20;
        public const int DyonisosAttackStonePrice = 20;
        public const int DyonisosAttackVinePrice = 50;

        // Athena : Bois=30, Pierre=30, Diamant=50
        public const int AthenaAttackWoodPrice = 30;
        public const int AthenaAttackStonePrice = 30;
        public const int AthenaAttackDiamondPrice = 50;

        // Hades : Bois=50, Pierre=50, Obsidienne=50, Population=5
        public const int HadesAttackWoodPrice = 50;
        public const int HadesAttackStonePrice = 50;
        public const int HadesAttackObsidianPrice = 50;
        public const int HadesAttackPopulationPrice = 5;


        // --- Dictionnaires ---

        public static readonly Dictionary<ResourceType, int> PriceHouseDict = new()
        {
            { ResourceType.Wood, HouseWoodPrice },
            { ResourceType.Stone, HouseStonePrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceWoodExtractorDict = new()
        {
            { ResourceType.Wood, WoodExtractorWoodPrice },
            { ResourceType.Population, WoodExtractorPopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceStoneExtractorDict = new()
        {
            { ResourceType.Stone, StoneExtractorStonePrice },
            { ResourceType.Population, StoneExtractorPopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceGoldExtractorDict = new()
        {
            { ResourceType.Wood, GoldExtractorWoodPrice },
            { ResourceType.Stone, GoldExtractorStonePrice },
            { ResourceType.Population, GoldExtractorPopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceWaterExtractorDict = new()
        {
            { ResourceType.Wood, WaterExtractorWoodPrice },
            { ResourceType.Stone, WaterExtractorStonePrice },
            { ResourceType.Gold, WaterExtractorGoldPrice },
            { ResourceType.Population, WaterExtractorPopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceVineExtractorDict = new()
        {
            { ResourceType.Wood, VineExtractorWoodPrice },
            { ResourceType.Stone, VineExtractorStonePrice },
            { ResourceType.Gold, VineExtractorGoldPrice },
            { ResourceType.Population, VineExtractorPopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceDiamondExtractorDict = new()
        {
            { ResourceType.Wood, DiamondExtractorWoodPrice },
            { ResourceType.Stone, DiamondExtractorStonePrice },
            { ResourceType.Gold, DiamondExtractorGoldPrice },
            { ResourceType.Population, DiamondExtractorPopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceObsidianExtractorDict = new()
        {
            { ResourceType.Wood, ObsidianExtractorWoodPrice },
            { ResourceType.Stone, ObsidianExtractorStonePrice },
            { ResourceType.Gold, ObsidianExtractorGoldPrice },
            { ResourceType.Population, ObsidianExtractorPopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceZeusTempleDict = new()
        {
            { ResourceType.Wood, ZeusTempleWoodPrice },
            { ResourceType.Stone, ZeusTempleStonePrice },
            { ResourceType.Gold, ZeusTempleGoldPrice },
            { ResourceType.Population, ZeusTemplePopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PricePoseidonTempleDict = new()
        {
            { ResourceType.Wood, PoseidonTempleWoodPrice },
            { ResourceType.Stone, PoseidonTempleStonePrice },
            { ResourceType.Gold, PoseidonTempleGoldPrice },
            { ResourceType.Water, PoseidonTempleWaterPrice },
            { ResourceType.Population, PoseidonTemplePopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceDionysusTempleDict = new()
        {
            { ResourceType.Wood, DionysusTempleWoodPrice },
            { ResourceType.Stone, DionysusTempleStonePrice },
            { ResourceType.Gold, DionysusTempleGoldPrice },
            { ResourceType.Vine, DionysusTempleVinePrice },
            { ResourceType.Population, DionysusTemplePopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceAthenaTempleDict = new()
        {
            { ResourceType.Wood, AthenaTempleWoodPrice },
            { ResourceType.Stone, AthenaTempleStonePrice },
            { ResourceType.Gold, AthenaTempleGoldPrice },
            { ResourceType.Diamond, AthenaTempleDiamondPrice },
            { ResourceType.Population, AthenaTemplePopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceHadesTempleDict = new()
        {
            { ResourceType.Wood, HadesTempleWoodPrice },
            { ResourceType.Stone, HadesTempleStonePrice },
            { ResourceType.Gold, HadesTempleGoldPrice },
            { ResourceType.Obsidian, HadesTempleObsidianPrice },
            { ResourceType.Population, HadesTemplePopulationPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceZeusAttackDict = new()
        {
            { ResourceType.Wood, ZeusAttackWoodPrice },
            { ResourceType.Stone, ZeusAttackStonePrice },
            { ResourceType.Gold, ZeusAttackGoldPrice }
        };

        public static readonly Dictionary<ResourceType, int> PricePoseidonAttackDict = new()
        {
            { ResourceType.Wood, PoseidonAttackWoodPrice },
            { ResourceType.Stone, PoseidonAttackStonePrice },
            { ResourceType.Water, PoseidonAttackWaterPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceDyonisosAttackDict = new()
        {
            { ResourceType.Wood, DyonisosAttackWoodPrice },
            { ResourceType.Stone, DyonisosAttackStonePrice },
            { ResourceType.Vine, DyonisosAttackVinePrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceAthenaAttackDict = new()
        {
            { ResourceType.Wood, AthenaAttackWoodPrice },
            { ResourceType.Stone, AthenaAttackStonePrice },
            { ResourceType.Diamond, AthenaAttackDiamondPrice }
        };

        public static readonly Dictionary<ResourceType, int> PriceHadesAttackDict = new()
        {
            { ResourceType.Wood, HadesAttackWoodPrice },
            { ResourceType.Stone, HadesAttackStonePrice },
            { ResourceType.Obsidian, HadesAttackObsidianPrice },
            { ResourceType.Population, HadesAttackPopulationPrice }
        };
    }
}

   

