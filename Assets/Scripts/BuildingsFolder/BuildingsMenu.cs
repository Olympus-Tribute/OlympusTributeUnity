using ForNetwork;
using Grid;
using Menus.MenusInGame;
using Networking.Common.Client;
using OlympusWorldGenerator;
using UnityEngine;

namespace BuildingsFolder
{
    public class BuildingsMenu : MonoBehaviour
    {
        private Camera _mainCamera;
        private GameObject _ghostBuilding;
        private GameObject _selectedBuildingPrefab;
        private int _selectedBuildingType;
    
        public GameObject menuUISelectTypeOfBuilding;
        public GameObject menuUISelectExtractor;
        public GameObject menuUISelectTemple;
        private BuildingsManager _buildingsManager;
        private HexMapGenerator _map;
        
        private void Start()
        {
            _mainCamera = Camera.main;
            SetAllMenusInactive();
            
            _buildingsManager = FindFirstObjectByType<BuildingsManager>();
            _map = FindFirstObjectByType<GridGenerator>().MapGenerator;
            if (_buildingsManager == null)
            {
                Debug.LogError("BuildingsManager n'a pas été trouvé dans la scène ! Désactivation du script.");
                enabled = false;
                return;
            }
        }
        
        private void SetAllMenusInactive()
        {
            menuUISelectTypeOfBuilding.SetActive(false);
            menuUISelectExtractor.SetActive(false);
            menuUISelectTemple.SetActive(false);
            
            if (_ghostBuilding != null)
            {
                Destroy(_ghostBuilding); // Détruit le bâtiment "fantôme"
                _ghostBuilding = null;
            }
            _selectedBuildingPrefab = null;  // Réinitialise la sélection
        }
        

        public void Update()
        {
            // Ouvrir/fermer le menu avec la touche "B"
            if (Input.GetKeyDown(KeyCode.B))
            {
                SetAllMenusInactive();

                menuUISelectTypeOfBuilding.SetActive(!menuUISelectTypeOfBuilding.activeSelf);
            }
        
            // Si un bâtiment est sélectionné et que le menu est ouvert, suivre la souris
            if (_selectedBuildingPrefab != null)
            {
                FollowMouse();
                UpdateBuildingPreviewColor();
            }

            // Clic gauche pour placer définitivement le bâtiment
            if (Input.GetMouseButtonDown(0) && _ghostBuilding != null)
            {
                PlaceBuilding();
            }
        }

        public void QuitMenu()
        {
            SetAllMenusInactive();
        }
    
        public void SelectBuilding(int buildingType)
        {
            SetAllMenusInactive();
            
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
            }
        }
        
        public void SelectTypeOfBuilding(string typeOfBuilding)
        {
            switch (typeOfBuilding)
            {
                case "House":
                    SelectBuilding(1);
                    return;
                case "Extractor":
                    menuUISelectTypeOfBuilding.SetActive(false);
                    menuUISelectExtractor.SetActive(true);
                    return;
                case "Temple":
                    menuUISelectTypeOfBuilding.SetActive(false);
                    menuUISelectTemple.SetActive(true);
                    return;
                default:
                    return;
            }
        }

        // Fonction pour déplacer le bâtiment "fantôme" avec la souris
        private void FollowMouse()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                (float x, float y, float z) = StaticGridTools.WorldCoToWorldCenterCo(hit.point.x, hit.point.y, hit.point.z);
                _ghostBuilding.transform.position = new Vector3(x, y, z);
    
                // pour l'orientation
                int rotationAngle = StaticGridTools.MapIndexToRotation((int)x, (int)z, GameConstants.Seed, (int)GameConstants.MapWidth);
                _ghostBuilding.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
            }

        }

        // Fonction pour placer définitivement le bâtiment à la position de la souris
        private void PlaceBuilding()
        {
            if (_ghostBuilding != null && CanPlaceBuilding(_ghostBuilding.transform.position))
            {
                Vector3 position = _ghostBuilding.transform.position;

                //___________________________________________//
                //____________Pour le Multi__________________//
                //___________________________________________//
                
                (int posX, int posZ) = StaticGridTools.WorldCenterCoToMapIndex(position.x, position.z);
                var action = new ClientPlaceBuildingGameAction(posX, posZ, (ushort) _selectedBuildingType);
                Network.Instance.Proxy.Connection.Send(action);
                
                //___________________________________________//
                //___________________________________________//
                //___________________________________________//
                
                Destroy(_ghostBuilding); // Détruit le bâtiment "fantôme"
                _selectedBuildingPrefab = null;  // Réinitialise la sélection
            }
            else
            {
                Debug.LogWarning("Impossible de placer le bâtiment ici !");
            }
        }

        private bool CanPlaceBuilding(Vector3 position)
        {
            (int posX, int posZ) = StaticGridTools.WorldCenterCoToMapIndex(position.x, position.z);
            if (posX >= 0 && posZ >= 0 && posX < GameConstants.MapHeight && posZ < GameConstants.MapWidth)
            {
                uint? owner = _buildingsManager.OwnerManager.GetOwner(posX, posZ);
                if (owner.HasValue && owner == GameConstants.PlayerId)
                {
                    switch (_selectedBuildingType)
                    {
                        case 1 or >= 9 when _map[posX, posZ] is FloorTile.Grass:
                        case 2 when _map[posX, posZ] is FloorTile.Wood:
                        case 3 when _map[posX, posZ] is FloorTile.StoneMountain:
                        case 4 when _map[posX, posZ] is FloorTile.GoldMountain:
                        case 5 when _map[posX, posZ] is FloorTile.DiamondMountain:
                        case 6 when _map[posX, posZ] is FloorTile.ObsidianMountain:
                        case 7 when _map[posX, posZ] is FloorTile.Lake:
                        case 8 when _map[posX, posZ] is FloorTile.Vine:
                            return true;
                        default:
                            return false;
                    }
                }
            }
            return false;
        }

        private void UpdateBuildingPreviewColor()
        {
            
            if (CanPlaceBuilding(_ghostBuilding.transform.position))
            {
                MakePreviewTransparent(new Color(0, 1, 0, 0.5f)); // Vert
            }
            else
            {
                MakePreviewTransparent(new Color(1, 0, 0, 0.5f)); // Rouge
            }
            
        }

        private void MakePreviewTransparent(Color color)
        {
            foreach (var renderer in _ghostBuilding.GetComponentsInChildren<Renderer>())
            {
                foreach (var rendererMaterial in renderer.materials)
                {
                    rendererMaterial.color = color;
                }

            }
        }
    }
}