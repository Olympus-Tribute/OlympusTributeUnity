using UnityEngine;

namespace Menus.MenusInGame
{
    public class MenusInGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject menuUISelectTypeOfBuilding;
        [SerializeField] private GameObject menuUISelectExtractor;
        [SerializeField] private GameObject menuUISelectTemple;
        [SerializeField] private GameObject menuUIAttack;

        private GameObject _lastOpenedMenu = null;
        private bool _isMenuOpen = false;

        private void Start()
        {
            SetAllMenusInactive();
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
            _lastOpenedMenu = null;
            _isMenuOpen = false;
        }

        private void CheckIfMenuIsOpen()
        {
            _isMenuOpen = menuUISelectTypeOfBuilding.activeSelf || 
                         menuUISelectExtractor.activeSelf || 
                         menuUISelectTemple.activeSelf || 
                         menuUIAttack.activeSelf;
        }

        public void ShowMenu(GameObject menu)
        {
            if (menu == null)
            {
                return;
            }
            
            /*
            if (_isMenuOpen)
            {
                return;
            }
            */
            
            menu.SetActive(true);
            _lastOpenedMenu = menu;
            _isMenuOpen = true;
        }

        public void CloseCurrentMenu()
        {
            if (_lastOpenedMenu != null)
            {
                _lastOpenedMenu.SetActive(false);
                _lastOpenedMenu = null;
                _isMenuOpen = false;
            }
        }
    }
}
