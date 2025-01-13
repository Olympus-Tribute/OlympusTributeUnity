using UnityEngine;
using UnityEngine;

public class BuildingsMenu2 : MonoBehaviour
{
    private Camera _mainCamera;
    private GameObject _ghostBuilding;
    private GameObject _selectedBuildingPrefab;
    private int _selectedBuildingType;
    
    public GameObject menuUI;
    public BuildingsManager buildingsManager;
    
    void Start()
    {
        _mainCamera = Camera.main;
        menuUI.SetActive(false);
    }

    void Update()
    {
        // Ouvrir/fermer le menu avec touche "B"
        if (Input.GetKeyDown(KeyCode.B))
        {
            menuUI.SetActive(!menuUI.activeSelf);
        }
        
        // Si un bâtiment est sélectionné et que le menu est ouvert, suivre la souris
        if (_selectedBuildingPrefab != null)
        {
            FollowMouse();
        }

        // Clic gauche pour placer définitivement le bâtiment
        if (Input.GetMouseButtonDown(0) && _ghostBuilding != null)
        {
            PlaceBuilding();
        }
    }
    
    public void SelectBuilding(int buildingType)
    {
        menuUI.SetActive(false);

        // Réinitialise le bâtiment "fantôme" si un bâtiment est déjà en cours
        if (_ghostBuilding != null)
        {
            Destroy(_ghostBuilding);
        }
        
        _selectedBuildingType = buildingType;
        _selectedBuildingPrefab = buildingsManager.GetBuildingPrefab(buildingType);
        
        if (_selectedBuildingPrefab != null)
        {
            // Crée un bâtiment "fantôme" pour le placement
            _ghostBuilding = Instantiate(_selectedBuildingPrefab, Vector3.zero, Quaternion.identity);
            //MakePreviewTransparent();
        }
    }


    // Fonction pour déplacer le bâtiment "fantôme" avec la souris
    void FollowMouse()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            _ghostBuilding.transform.position = hit.point;
        }
    }

    // Fonction pour placer définitivement le bâtiment à la position de la souris
    void PlaceBuilding()
    {
        if (_ghostBuilding != null)
        {
            Vector3 position = _ghostBuilding.transform.position;

            // Demande à BuildingsManager de placer le bâtiment
            if (buildingsManager.PlaceBuilding(position.x, position.z, _selectedBuildingType))
            {
                Debug.Log("Bâtiment placé avec succès.");
            }
            else
            {
                Debug.LogWarning("Impossible de placer le bâtiment ici !");
            }

            Destroy(_ghostBuilding); // Détruit le bâtiment "fantôme"
            _selectedBuildingPrefab = null;  // Réinitialise la sélection
        }
    }


    // Applique une transparence au bâtiment "fantôme"
    private void MakePreviewTransparent()
    {
        foreach (var renderer in _ghostBuilding.GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = new Color(1, 1, 1, 0.5f); // Rend le bâtiment semi-transparent
        }
    }
}