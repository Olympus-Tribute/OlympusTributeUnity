using UnityEngine;


namespace BuildingsFolder
{
    public class OwnersMaterial
    {
        private static readonly string[] KingdomNames = new []{"Athens", "Thebes" , "Sparta", "Corinth", "Troy", "Delphi","Argos", "Rhodes", "Cretan","Tripoli", "Pylos"};

        private static readonly Color[] KingdomColors = new[]
        {
            
            new Color(169F/255F, 141F/255F, 0),
            new Color (179F/255F, 0,0),
            new Color (0, 0,0),
            new Color (99F/255F, 0, 181F/255F),
        };

        public static string GetName(uint owner)
        {
            if (owner >= KingdomNames.Length)
            {
                return "Player " + owner;
            }
            return KingdomNames[owner];
        }
        public static Color GetColor(uint owner)
        {
            if (owner < KingdomColors.Length) return KingdomColors[owner];
            var random = new System.Random((int) owner);
            return new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
        }
    }
}