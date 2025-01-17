using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class BuildingsInteraction : MonoBehaviour
    {
        public GameObject menuUI;
        public Text buildingInfoText;

        private Camera _mainCamera;
        private GameObject _selectedBuilding;

        void Start()
        {
            _mainCamera = Camera.main;
            menuUI.SetActive(false);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckForBuildingClick();
            }
            
            if (Input.GetMouseButtonDown(1) && menuUI.activeSelf)
            {
                menuUI.SetActive(false);
            }
        }

        void CheckForBuildingClick()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Vérifie si l'objet cliqué est un bâtiment
                if (hit.collider.CompareTag("Building"))
                {
                    _selectedBuilding = hit.collider.gameObject;
                    ShowMenu(hit.point, _selectedBuilding);
                }
            }
        }

        void ShowMenu(Vector3 position, GameObject building)
        {
            
            menuUI.transform.position = position + new Vector3(0, 2, 0);
            
            buildingInfoText.text = $"Bâtiment : {building.name}";
            
            menuUI.SetActive(true);
        }

        public void DeleteSelectedBuilding()
        {
            if (_selectedBuilding != null)
            {
                Destroy(_selectedBuilding); // Supprime le bâtiment
                _selectedBuilding = null;
                menuUI.SetActive(false); // Ferme le menu
            }
        }

        public void UpgradeSelectedBuilding()
        {
            if (_selectedBuilding != null)
            {
                // Exemple d'action : mise à niveau
                Debug.Log($"Mise à niveau du bâtiment : {_selectedBuilding.name}");
                menuUI.SetActive(false);
            }
        }
    }
}