using System;
using UnityEngine;

public class MousePositionTracker : MonoBehaviour
{
    public static MousePositionTracker Instance {get; private set;}
    
    private Camera _mainCamera;
    
    public void Awake()
    {
       Instance = this;
    }
    
    public void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        // Exemple pour obtenir la position de la souris au clic
        if (Input.GetMouseButtonDown(0))
        {
            Vector3? worldPosition = GetMouseWorldPosition();
            if (worldPosition.HasValue)
            {
                Vector3 pos = worldPosition.Value;
                Debug.Log($"Mouse clicked at world position: X={pos.x}, Y={pos.y}, Z={pos.z}");
            }
        }
    }

    public Vector3? GetMouseWorldPosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point; // Retourne la position d'impact
        }
        return null; // Retourne null si aucun objet n'est touché
    }

    
    // Obtient les indices de la carte correspondant à la position de la souris
    public (int, int)? GetMouseMapIndexCo()
    {
        Vector3? mouseWorldPositionCo = GetMouseWorldPosition();

        // Vérifie si la position est valide
        if (mouseWorldPositionCo.HasValue)
        {
            Vector3 worldPos = mouseWorldPositionCo.Value;
            return  StaticGridTools.WorldCoToMapIndex(worldPos.x, worldPos.y, worldPos.z);
        }

        return null; // Retourne une position invalide
    }

}