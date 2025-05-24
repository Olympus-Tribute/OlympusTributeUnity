using System.Collections;
using UnityEngine;

namespace Animation.Attacks.Athena
{

    public class AnimAthena : MonoBehaviour
    {
        public GameObject spearPrefab;
        private const float spawnHeight = -5f;
        private const float targetHeight = 9f;
        private const float moveSpeed = 5f;
        private const float spawnInterval = 0.15f;
        private const float attackDuration = 6f;

        private float timer = 0f;
        private float attackTimer = 0f;
        private bool onAttack = true;
        private const float hexagonRadius = 10f;

        void Update()
        {
            if (!onAttack) return;

            timer += Time.deltaTime;
            attackTimer += Time.deltaTime;
            
            
            if (attackTimer >= attackDuration + 5f)
            {
                onAttack = false;
                Destroy(gameObject);
                return;
            }

            if (timer >= spawnInterval && attackTimer< attackDuration)
            {
                SpawnSpear();
                timer = 0f;
            }
        }

        void SpawnSpear()
        {
            Vector3 randomPosition = GetRandomPositionInsideHexagon();
            randomPosition.y = spawnHeight;

            GameObject spear = Instantiate(spearPrefab, randomPosition, Quaternion.identity);

            StartCoroutine(MoveSpear(spear));
        }

        Vector3 GetRandomPositionInsideHexagon()
        {
            float angle = Random.Range(0f, 360f);
            float randomRadius = Random.Range(0f, hexagonRadius);

            float x = randomRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = randomRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 hexagonCenter = this.transform.position;
            return new Vector3(hexagonCenter.x + x, hexagonCenter.y, hexagonCenter.z + z);
        }


        private IEnumerator MoveSpear(GameObject spear)
        {
            while (spear.transform.position.y < targetHeight)
            {
                spear.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                yield return null;
            }

            Destroy(spear);
        }
    }
}