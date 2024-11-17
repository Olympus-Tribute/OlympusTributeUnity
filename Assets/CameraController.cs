using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    public float moveSpeed = 10f;        // Vitesse de déplacement
    public float zoomSpeed = 10f;       // Vitesse du zoom
    public float minZoom = 10f;         // Zoom minimal (distance maximale)
    public float maxZoom = 50f;         // Zoom maximal (distance minimale)
    public float rotationSpeed = 1000f;  // Vitesse de rotation avec la souris (augmentez cette valeur)
    public float rotationMultiplier = 50f; // Facteur d'amplification de la rotation
    
    
    private Camera cam;

    void Start()
    {
        // Obtenez la caméra attachée à cet objet
        cam = Camera.main;

        // Configurez la caméra pour une vue plongeante
        if (transform.eulerAngles.x != 45f) // Si l'angle n'est pas déjà défini
        {
            transform.rotation = Quaternion.Euler(45f, 0f, 0f); // Vue plongeante à 45°
        }
    }

    void Update()
    {
        // Déplacement
        MoveCamera();

        // Rotation
        RotateCameraWithMouse();

        // Zoom
        ZoomCamera();
    }

    void MoveCamera()
    {
        // Utilisez les axes "Horizontal" et "Vertical" pour déplacer la caméra (WASD ou flèches)
        float horizontal = Input.GetAxis("Horizontal"); // A/D ou flèches gauche/droite
        float vertical = Input.GetAxis("Vertical");     // W/S ou flèches haut/bas

        // Déplacement dans le plan XZ
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    void RotateCameraWithMouse()
    {
        // Vérifiez si le clic droit de la souris est enfoncé
        if (Input.GetMouseButton(0)) // 1 correspond au bouton droit de la souris
        {
            // Obtenez les mouvements de la souris
            float mouseX = Input.GetAxis("Mouse X"); // Déplacement horizontal de la souris
            float mouseY = Input.GetAxis("Mouse Y"); // Déplacement vertical de la souris

            // Appliquez une rotation autour de l'axe Y pour la rotation horizontale (inversée)
            transform.Rotate(Vector3.up, -mouseX * rotationSpeed * rotationMultiplier * Time.deltaTime, Space.World);

            // Appliquez une rotation autour de l'axe local X pour incliner la caméra verticalement (inversée)
            float tiltAngle = mouseY * rotationSpeed * rotationMultiplier * Time.deltaTime; // Inversion de l'angle
            Vector3 currentEulerAngles = transform.eulerAngles;
            float newTilt = Mathf.Clamp(currentEulerAngles.x + tiltAngle, 20f, 80f); // Limitez l'inclinaison entre 20° et 80°
            transform.eulerAngles = new Vector3(newTilt, transform.eulerAngles.y, 0f);
        }
    }

    void ZoomCamera()
    {
        if (cam == null) return;

        // Détection de la molette de la souris
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            if (cam.orthographic)
            {
                // Zoom pour une caméra orthographique
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            }
            else
            {
                // Zoom pour une caméra en perspective
                Vector3 newPosition = transform.position + transform.forward * scroll * zoomSpeed;
                float distance = Vector3.Distance(newPosition, Vector3.zero); // Distance par rapport à l'origine
                if (distance >= minZoom && distance <= maxZoom)
                {
                    transform.position = newPosition;
                }
            }
        }
    }
}

