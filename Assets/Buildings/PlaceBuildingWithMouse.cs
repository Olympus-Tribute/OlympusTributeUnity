using UnityEngine;

public class PlaceBuildingWithMouse : MonoBehaviour
{
    public GameObject buildingPrefab; // Le prefab du bâtiment à placer
    private GameObject currentBuilding; // Le bâtiment "fantôme" qui suit la souris
    private Camera mainCamera; // La caméra principale

    // Référence au BuildingsManager pour placer le bâtiment
    public BuildingsManager buildingsManager;

    void Start()
    {
        mainCamera = Camera.main; // Initialiser la caméra principale
    }

    void Update()
    {

        // Si la touche espace est appuyée, on crée un bâtiment "fantôme" à la position de la souris
        if (Input.GetKeyDown(KeyCode.M))
        {
            CreateBuildingPreview();
        }

        // Si le bouton droit est cliqué, on place définitivement le bâtiment
        if (Input.GetMouseButtonDown(0)) // 0 = Clic Gauche
        {
            PlaceBuilding();
        }

        // Si un bâtiment "fantôme" existe, on le fait suivre la position du curseur
        if (currentBuilding != null)
        {
            FollowMouse();
        }
    }

    void CreateBuildingPreview()
    {
        // Vérifie si un bâtiment "fantôme" n'existe pas encore
        if (currentBuilding == null)
        {
            currentBuilding = Instantiate(buildingPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    void FollowMouse()
    {
        // Crée un rayon de la position de la souris dans le monde
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Vérifie si la souris touche un objet (comme le sol)
        if (Physics.Raycast(ray, out hit))
        {
            // Déplace le bâtiment "fantôme" à la position du curseur
            currentBuilding.transform.position = hit.point;
        }
    }

    void PlaceBuilding()
    {
        if (currentBuilding != null)
        {
            Debug.Log("Hi");
            // Récupère les coordonnées du bâtiment à placer
            float x = currentBuilding.transform.position.x;
            float z = currentBuilding.transform.position.z;

            // Place le bâtiment via le BuildingsManager (vous pouvez changer le type ici)
            buildingsManager.PlaceBuilding(x, z, 1);

            // Détruit le bâtiment "fantôme" après l'avoir placé
            Destroy(currentBuilding);

            // Réinitialise la référence
            currentBuilding = null;
        }
    }
}
