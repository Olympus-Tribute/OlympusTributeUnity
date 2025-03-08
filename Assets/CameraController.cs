using System;
using ForServer;
using Resources;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /*
    public float moveSpeed = 10f;        // Vitesse de déplacement
    public float zoomSpeed = 10f;       // Vitesse du zoom
    public float minZoom = 5f;         // Zoom minimal (distance maximale)
    public float maxZoom = 1000f;         // Zoom maximal (distance minimale)
    public float rotationSpeed = 1000f;  // Vitesse de rotation avec la souris (augmentez cette valeur)
    public float rotationMultiplier = 50f; // Facteur d'amplification de la rotation
    
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        // Configurez la caméra pour une vue plongeante
        if (transform.eulerAngles.x != 45f) // Si l'angle n'est pas déjà défini
        {
            transform.rotation = Quaternion.Euler(45f, 0f, 0f); // Vue plongeante à 45°
        }
    }

    void Update()
    {
        MoveCamera();
        RotateCameraWithMouse();
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
    */
    
    private Camera _mainCamera;
    
    public readonly VectorSmoothDynamics TargetPosition = new VectorSmoothDynamics();
    protected readonly SmoothFloat Zoom = new SmoothFloat(20, 20);
    protected readonly SmoothFloat HorizontalAngle = new SmoothFloat(20, 0);
    protected readonly SmoothFloat VerticalAngle = new SmoothFloat(20, Math.PI/2f);
    
    private const float GetZoomSensitivity = 30;
    private const float HorizontalSensitivity = 10;
    private const float VerticalSensitivity = 10;
    
    private const float MinAngle = -(float)Math.PI;
    private const float MaxAngle = (float)Math.PI;
    private const float MinimumZoom = 1;
    private const float MaxZoom = 100;
  
    
    public void Start()
    {
        _mainCamera = Camera.main;
    }

    public void Update()
    {
        Zoom.targetValue += -Input.mouseScrollDelta.y * Time.deltaTime * GetZoomSensitivity;
        if (Input.GetMouseButton(1))
        {
            HorizontalAngle.targetValue += Input.GetAxis("Mouse X") * Time.deltaTime * HorizontalSensitivity;
            VerticalAngle.targetValue -= Input.GetAxis("Mouse Y") * Time.deltaTime * VerticalSensitivity;
        }
        
        bool forward = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        float moveDirY =  forward == backward ? 0 : forward ? 1 : -1;
        float moveDirX = left==right ? 0 : right ? -1 : 1;


        float horizontalAngle = this.HorizontalAngle.currentValue;
        float cameraForwardX = (float) Math.Cos(horizontalAngle);
        float cameraForwardY = (float) -Math.Sin(horizontalAngle);

        float dirX = moveDirX;
        float dirY = -moveDirY;

        float x = dirX * cameraForwardY + dirY * cameraForwardX;
        float z = dirY * cameraForwardY - dirX * cameraForwardX;

        TargetPosition.x.targetValue += x;
        TargetPosition.z.targetValue += z;
        
        UpdateSmoothDynamics();

        float horizontalAngleCurrent = this.HorizontalAngle.currentValue;
        float verticalAngleCurrent = this.VerticalAngle.currentValue;
        float zoom = this.Zoom.currentValue;
        
        Vector3 target = TargetPosition.Get();
        
        ClampInputs();
        SetCameraPosition(horizontalAngleCurrent, verticalAngleCurrent, zoom, target);
        SetCameraRotation(horizontalAngleCurrent, verticalAngleCurrent);
    }

    
    private void SetCameraPosition(float horizontalAngleCurrent, float verticalAngleCurrent, float zoom, Vector3 target) {
        Vector3 crossed = GetAheadVector(horizontalAngleCurrent, verticalAngleCurrent);
        Vector3 ahead = crossed * (-zoom *0.4f);
        ahead.y *= 0.5f;
        
        ahead += target;

        ahead += crossed * zoom;

        _mainCamera.transform.position = ahead;
    }
    
    private Vector3 GetAheadVector(float horizontalAngleCurrent, float verticalAngleCurrent) {
        float x = (float) Math.Cos(horizontalAngleCurrent);
        float y = (float) Math.Sin(verticalAngleCurrent);
        float z = (float) -Math.Sin(horizontalAngleCurrent);

        float lengthOther = (float) Math.Sqrt(1 - y*y);

        return new Vector3(x * lengthOther, y, z * lengthOther);
    }

    private void SetCameraRotation(float horizontalAngleCurrent, float verticalAngleCurrent) {
        Quaternion rotationZ = Quaternion.AngleAxis((-90 + (float)(horizontalAngleCurrent * 180 / Math.PI)), new Vector3(0, 1, 0));
        Quaternion rotationY = Quaternion.AngleAxis(((float)(verticalAngleCurrent * 180 / Math.PI)), new Vector3(1, 0, 0));
        Quaternion res = rotationZ * rotationY;
        
        _mainCamera.transform.rotation = res;
    }
    
    private void ClampInputs() {
        
        float verticalCurrent = VerticalAngle.currentValue;

        if (verticalCurrent > MaxAngle)
        {
            VerticalAngle.currentValue = MaxAngle;
        }

        if (verticalCurrent< MinAngle)
            VerticalAngle.currentValue = MinAngle;

        float zoomCurrent = Zoom.currentValue;

        if (zoomCurrent < MinimumZoom)
        {
            Zoom.currentValue = MinimumZoom;
        }
            
        if (zoomCurrent > MaxZoom)
        {
            Zoom.currentValue = MaxZoom;
        }
    }

    private void UpdateSmoothDynamics()
    {
        Zoom.Update(Time.deltaTime);
        HorizontalAngle.Update(Time.deltaTime);
        VerticalAngle.Update(Time.deltaTime);
        TargetPosition.Update(Time.deltaTime);
    }
}

public class VectorSmoothDynamics
{
    public SmoothFloat x = new SmoothFloat(20, 0);
    public SmoothFloat z = new SmoothFloat(20, 0);

    public void Set(Vector3 vector)
    {
        x.targetValue = vector.x;
        z.targetValue = vector.z;
    }

    public void Update(float deltaTime)
    {
        x.Update(deltaTime);
        z.Update(deltaTime);
    }
    
    public Vector3 Get()
    {
        return new Vector3(x.CurrentValue, 3, z.CurrentValue);
    }
}

