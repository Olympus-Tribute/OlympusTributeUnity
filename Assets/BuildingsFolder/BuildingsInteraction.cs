using ForNetwork;
using Networking.Common.Client;
using TMPro;
using UnityEngine;

namespace BuildingsFolder
{
    public class BuildingsInteraction : MonoBehaviour
    {
        public GameObject menuUI;
        public TMP_Text buildingInfoText;

        private Camera _mainCamera;
        private GameObject _selectedBuilding;
        
        private BuildingsManager _buildingsManager;

        private void Start()
        {
            _mainCamera = Camera.main;
            _buildingsManager = FindObjectOfType<BuildingsManager>();
            if (_buildingsManager == null)
            {
                Debug.LogError("BuildingsManager n'a pas été trouvé dans la scène !");
                return;
            }
            menuUI.SetActive(false);
        }

        private void Update()
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

        private void CheckForBuildingClick()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Vérifie si l'objet cliqué est un bâtiment
                if (hit.collider.CompareTag("Building"))
                {
                    _selectedBuilding = hit.collider.gameObject;
                    ShowMenu(hit.point, _selectedBuilding);
                }
            }
        }

        private void ShowMenu(Vector3 position, GameObject building)
        {
            if (menuUI.activeSelf) 
            {
                menuUI.SetActive(false); // Close any existing menu
            }
    
            menuUI.transform.position = position + new Vector3(0, 2, 0);
            buildingInfoText.text = $"Bâtiment : {building.name}";

            menuUI.SetActive(true);
        }

        public void DeleteSelectedBuilding()
        {
            if (_selectedBuilding != null)
            {
                // Obtenir la position du bâtiment sélectionné
                Vector3 positionKey = _selectedBuilding.transform.position;

                // Arrondir les coordonnées pour éviter les imprécisions
                int roundedX = (int)Mathf.Round(positionKey.x) / 5;
                int roundedZ = (int)Mathf.Round(positionKey.z) / 5;
                //Vector3 roundedPositionKey = new Vector3(roundedX, 0, roundedZ);
                
                //___________________________________________//
                //____________Pour le Multi__________________//
                //___________________________________________//
                
                var action = new ClientRemoveBuildingGameAction(roundedX, roundedZ);
                Network.Instance.Proxy.Connection.Send(action);
                
                Debug.Log("Delete selected building");
                //___________________________________________//
                //___________________________________________//
                //___________________________________________//
                
                _selectedBuilding = null;
                menuUI.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Aucun bâtiment sélectionné pour suppression.");
            }
        }
    }
}