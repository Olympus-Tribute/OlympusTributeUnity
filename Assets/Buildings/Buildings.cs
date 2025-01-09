using UnityEngine;

public class Buildings : MonoBehaviour
{
    public BuildingsManager buildings; // Référence au BuildingsManager

    public PlaceBuildingWithMouse placeBuildingWithMouse;

    void Update()
    {

        //Place un bâtiment aléatoirement lorsqu'on appuie sur la barre d'espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (buildings != null)
            {
                float x = Mathf.Round(Random.Range(10f, 100f)); // Coordonnée arrondie pour éviter des problèmes de précision
                float z = Mathf.Round(Random.Range(10f, 100f));
                int type = Random.Range(1, 5); // Random entre 1 et 4 inclus
                buildings.PlaceBuilding(x, z, type); // Place un bâtiment de type 1
                StartCoroutine(DeleteBuildingAfterDelay(x, z, 5f)); // Supprime après 5 secondes
            }
            else
            {
                Debug.LogError("Le BuildingsManager n'est pas assigné !");
            }
        }
    }

    private System.Collections.IEnumerator DeleteBuildingAfterDelay(float x, float z, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (buildings != null)
        {
            buildings.DeleteBuilding(x, z); // Supprime le bâtiment après le délai
        }
        else
        {
            Debug.LogError("Impossible de supprimer le bâtiment car BuildingsManager n'est pas assigné.");
        }
    }
}
