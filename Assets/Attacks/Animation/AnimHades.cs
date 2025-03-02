using BuildingsFolder;
using UnityEngine;

namespace Attacks.Animation
{
    public class AnimHades : MonoBehaviour
    {
        public GameObject swordPrefab;
        public GameObject buildingDestroy;
        private float spawnHeight = 50f; 
        private float fallSpeed = 20f; 
        private float spawnInterval = 0.1f; 
        public float rainDuration = 5f; 

        private float timer = 0f;
        private float rainTimer = 0f;
        public bool isRaining = true; // Contrôle si la pluie est active
        private float hexagonRadius = 10f; 

        void Update()
        {
            if (!isRaining) return; // Si la pluie est terminée, ne rien faire

            // Mettre à jour les timers
            timer += Time.deltaTime;
            rainTimer += Time.deltaTime;

            // Vérifier si la durée totale de la pluie est écoulée
            if (rainTimer >= rainDuration)
            {
                isRaining = false; // Arrêter la pluie
                //Debug.Log("Pluie d'épées terminée !");
                Destroy(buildingDestroy);
                return;
            }

            buildingDestroy.transform.position -= new Vector3(0,Time.deltaTime,0);

            // Générer une épée à intervalles réguliers
            if (timer >= spawnInterval)
            {
                SpawnSword();
                timer = 0f;
            }
        }

        void SpawnSword()
        {
            Vector3 randomPosition = GetRandomPositionInsideHexagon();  // pos aléatoire sur l'hexagone
            randomPosition.y = spawnHeight; 

            GameObject sword = Instantiate(swordPrefab, randomPosition, Quaternion.identity); // Instancier l'épée

            StartCoroutine(FallSword(sword));  // Faire tomber l'épée
        }

        Vector3 GetRandomPositionInsideHexagon()
        {
            float angle = Random.Range(0f, 360f); // angle aléatoire

            float randomRadius = Random.Range(0f, hexagonRadius); // nb aléatoire entre 0 et 10 le rayon de l'hexagone

            float x = randomRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = randomRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 hexagonPosition = this.transform.position;
            return new Vector3(hexagonPosition.x + x, hexagonPosition.y, hexagonPosition.z + z);
        }

        System.Collections.IEnumerator FallSword(GameObject sword)
        {
        
            while (sword.transform.position.y > this.transform.position.y)
            {
                sword.transform.Translate(  fallSpeed * Time.deltaTime * Vector3.down);
                yield return null; 
            }

       
            Destroy(sword); // disparait qd arrive au sol
        }
    }
}