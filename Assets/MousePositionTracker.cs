using System;
using UnityEngine;

public class MousePositionTracker : MonoBehaviour
{
    private Camera _mainCamera;
    
    public void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        /*
            if (Input.GetMouseButtonDown(0))
            {
                Vector3? worldPosition = GetMouseWorldPosition();
                if (worldPosition.HasValue)
                {
                    Vector3 pos = worldPosition.Value;
                    Debug.Log($"Mouse clicked at world position: X={pos.x}, Y={pos.y}, Z={pos.z}");
                }
            }
        */
    }

    public Vector3? GetMouseWorldPositionCo()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point; // Retourne la position d'impact
        }
        return null; // Retourne null si aucun objet touché
    }

    public (int, int)? GetMouseMapIndexCo()
    {
        Vector3? mouseWorldPositionCo = GetMouseWorldPositionCo();
        if (mouseWorldPositionCo is not null)
        {
            (float x, float y, float z) = StaticGridTools.WorldCoToWorldCenterCo(mouseWorldPositionCo.Value.x, mouseWorldPositionCo.Value.y, mouseWorldPositionCo.Value.z);
            return ((int, int))(x, z);
        }
        return null;
    }
}