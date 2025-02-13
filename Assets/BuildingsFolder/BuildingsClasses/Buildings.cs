using UnityEngine;

namespace BuildingsFolder.BuildingsClasses
{
    public class Building : MonoBehaviour
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public GameObject GameObject { get; private set; }
        public (int, int) Position { get; private set; }
        public ushort OwnerId { get; private set; }
        

        public Building(string name, string description, GameObject gameObject, (int, int) position, ushort ownerId)
        {
            Name = name;
            Description = description;
            GameObject = gameObject;
            Position = position;
            OwnerId = ownerId;
        }

        /*
        private void InstantiategameObjec()
        {
            if (gameObjec != null)
            {
                GameObject buildingInstance = Instantiate(gameObjec, Position, Quaternion.identity);
                buildingInstance.name = Name;
                Debug.Log($"{Name} a été placé en {Position}");
            }
            else
            {
                Debug.LogError($"gameObjec manquant pour {Name} !");
            }
        }
        
        private void DestroyBuilding()
        {
            Debug.Log($"{Name} a été détruit !");
            Destroy(gameObject);
        }
        */
    }
}