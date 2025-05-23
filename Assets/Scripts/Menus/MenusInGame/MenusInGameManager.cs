using UnityEngine;

namespace Menus.MenusInGame
{
    public class MenusInGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject menuUISelectTypeOfBuilding;
        [SerializeField] private GameObject menuUISelectExtractor;
        [SerializeField] private GameObject menuUISelectTemple;
        [SerializeField] private GameObject menuUIAttack;
        [SerializeField] private GameObject menuUISettings;

        private GameObject _lastOpenedMenu = null;
        public bool isMenuOpen = false;
        
        private CameraController _cameraController;

        private void OnEnable()
        {
            SetAllMenusInactive();
            _cameraController = FindFirstObjectByType<CameraController>();
        }

        private void Update()
        {
            CheckIfMenuIsOpen();
        }

        private void SetAllMenusInactive()
        {
            menuUISelectTypeOfBuilding?.SetActive(false);
            menuUISelectExtractor?.SetActive(false);
            menuUISelectTemple?.SetActive(false);
            menuUIAttack?.SetActive(false);
            menuUISettings?.SetActive(false);
            _lastOpenedMenu = null;
            isMenuOpen = false;
        }

        private void CheckIfMenuIsOpen()
        {
            isMenuOpen = menuUISelectTypeOfBuilding.activeSelf || 
                         menuUISelectExtractor.activeSelf || 
                         menuUISelectTemple.activeSelf || 
                         menuUIAttack.activeSelf ||
                         menuUISettings;
        }

        public void ShowMenu(GameObject menu)
        {
            if (menu == null)
            {
                return;
            }
            
            
            if (isMenuOpen)
            {
                return;
            }
            
            
            menu.SetActive(true);
            _lastOpenedMenu = menu;
            isMenuOpen = true;

            _cameraController.isActive = false;
        }

        public void CloseCurrentMenu()
        {
            if (_lastOpenedMenu != null)
            {
                _lastOpenedMenu.SetActive(false);
                _lastOpenedMenu = null;
                isMenuOpen = false;
                
                _cameraController.isActive = true;
            }
        }
    }
}
