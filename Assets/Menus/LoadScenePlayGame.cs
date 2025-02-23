using System;
using ForServer;
using Grid;
using UnityEngine;

namespace Menus
{
    public class LoadScenePlayGame : MonoBehaviour
    {
        public void OnEnable()
        {
            GridGenerator gridGenerator = gameObject.GetComponent<GridGenerator>();
            gridGenerator.Generate(ServerManager.Seed, ServerManager.MapWidth, ServerManager.MapHeight);
        }
    }
}