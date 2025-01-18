using Networking.Common.Client;
using UnityEngine;

namespace Buildings
{
    public class BuildingsMenu : MonoBehaviour
    {
        private Camera _mainCamera;
        private GameObject _ghostBuilding;
        private GameObject _selectedBuildingPrefab;
        private int _selectedBuildingType;
    
        public GameObject menuUI;
        private BuildingsManager _buildingsManager;
        private bool _networkActive;
    
        public float gridWidth = 20f;  // Largeur d'un hexagone
        public float gridHeight = 20f; // Hauteur d'un hexagone
    
        private void Awake()
        {
            _buildingsManager = FindObjectOfType<BuildingsManager>();
            if (_buildingsManager == null)
            {
                Debug.LogError("BuildingsManager n'a pas été trouvé dans la scène !");
                return;
            }

            // Récupérer l'état du réseau depuis BuildingsManager si nécessaire
            _networkActive = _buildingsManager.networkActive;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            menuUI.SetActive(false);
        }

        private void Update()
        {
            // Ouvrir/fermer le menu avec la touche "B"
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
    
        private void SelectBuilding(int buildingType)
        {
            menuUI.SetActive(false);

            // Réinitialise le bâtiment "fantôme" si un bâtiment est déjà en cours
            if (_ghostBuilding != null)
            {
                Destroy(_ghostBuilding);
            }
        
            _selectedBuildingType = buildingType;
            _selectedBuildingPrefab = _buildingsManager.GetBuildingPrefab(buildingType);
        
            if (_selectedBuildingPrefab != null)
            {
                // Crée un bâtiment "fantôme" pour le placement
                _ghostBuilding = Instantiate(_selectedBuildingPrefab, Vector3.zero, Quaternion.identity);
                MakePreviewTransparent();  // Rendre le bâtiment "fantôme" transparent
            }
        }

        // Fonction pour déplacer le bâtiment "fantôme" avec la souris
        private void FollowMouse()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Calculer la position arrondie sur la grille hexagonale
                Vector3 snappedPosition = CalculateHexagonalSnappedPosition(hit.point);

                // Déplace le bâtiment "fantôme" à la position calculée
                _ghostBuilding.transform.position = snappedPosition;
            
                //__________WithoutGrid_________
                //_ghostBuilding.transform.position = hit.point
            }
        }


        // Fonction pour placer définitivement le bâtiment à la position de la souris
        private void PlaceBuilding()
        {
            if (_ghostBuilding != null)
            {
                Vector3 position = _ghostBuilding.transform.position;

                if (_networkActive)
                {
                    //___________________________________________//
                    //____________Pour le Multi__________________//
                    //___________________________________________//
                    var action = new ClientPlaceBuildingGameAction((int)position.x/5, (int)position.z/5, (ushort) _selectedBuildingType);
                    if (_buildingsManager.Network.Proxy != null)
                    {
                        _buildingsManager.Network.Proxy.Connection.Send(action);
                    }
                    //___________________________________________//
                    //___________________________________________//
                    //___________________________________________//
                }
                else
                {
                    _buildingsManager.PlaceBuilding((int)position.x, (int)position.z, _selectedBuildingType);
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
    
        // Fonction pour calculer une position sur une grille hexagonale
        private Vector3 CalculateHexagonalSnappedPosition(Vector3 hitPoint)
        {
            float q = hitPoint.x / gridWidth;  // Coordonnée "q" (colonne)
            float r = hitPoint.z / (gridHeight * 0.75f); // Coordonnée "r" (ligne), ajustée par l'espacement vertical (75% de la hauteur)

            // Arrondir les coordonnées pour trouver l'hexagone le plus proche
            float roundedQ = Mathf.Round(q);
            float roundedR = Mathf.Round(r);

            // Convertir les coordonnées hexagonales arrondies en coordonnées mondiales
            float snappedX = roundedQ * gridWidth;
            float snappedZ = roundedR * gridHeight * 0.75f;

            // Décaler les lignes impaires horizontalement
            if (Mathf.Abs(roundedR % 2) > 0.1f) // Ligne impaire
            {
                snappedX += gridWidth / 2f;
            }

            return new Vector3(snappedX, 0, snappedZ); // Garder `y` à 0 ou ajuster en fonction de ton terrain
        }
    }
}