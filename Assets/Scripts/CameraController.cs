using System;
using Resources;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _mainCamera;
    
    public readonly VectorSmoothDynamics TargetPosition = new VectorSmoothDynamics();
    public readonly SmoothFloat Zoom = new SmoothFloat(5, 20);
    protected readonly SmoothFloat HorizontalAngle = new SmoothFloat(5, 0);
    public readonly SmoothFloat VerticalAngle = new SmoothFloat(8, (float)Math.PI/2);
    
    [SerializeField] private const float GetZoomSensitivity = 500;
    [SerializeField] private const float HorizontalSensitivity = 3;
    [SerializeField] private const float VerticalSensitivity = 5;
    [SerializeField] private const float Speed = 0.8f;
    
    [SerializeField] private const float MinAngle = (float)Math.PI/12;
    [SerializeField] private const float MaxAngle = (float)Math.PI/2;
    [SerializeField] private const float MinimumZoom = 10;
    [SerializeField] private const float MaxZoom = 300;

    [SerializeField] private const float DragSpeed = 1.8f;

    
    private bool below = false;
    
    
    public void Start()
    {
        _mainCamera = Camera.main;
    }

    public void Update()
    {
        Zoom.targetValue += -Input.mouseScrollDelta.y * Time.deltaTime * GetZoomSensitivity;
        if (Input.GetMouseButton(1))
        {
            HorizontalAngle.targetValue += (below ? 1 : -1) * Input.GetAxis("Mouse X") * Time.deltaTime * HorizontalSensitivity;
            VerticalAngle.targetValue -= Input.GetAxis("Mouse Y") * Time.deltaTime * VerticalSensitivity;
        }
        else
        {
            Vector3 pos = Input.mousePosition;
            below = pos.y < Screen.height/2f;
        }

        float zoomCurrent = Zoom.currentValue;
        
        bool forward = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        float moveDirY = (forward == backward ? 0 : forward ? 1 : -1)*zoomCurrent*Speed;
        float moveDirX = (left == right ? 0 : right ? -1 : 1)*zoomCurrent*Speed;

        if (Input.GetMouseButton(0))
        {
            moveDirY -= Input.GetAxis("Mouse Y") * DragSpeed *zoomCurrent;
            moveDirX += Input.GetAxis("Mouse X") * DragSpeed *zoomCurrent;
        }
        

        float horizontalAngle = this.HorizontalAngle.currentValue;
        float cameraForwardX = (float) Math.Cos(horizontalAngle);
        float cameraForwardY = (float) -Math.Sin(horizontalAngle);

        float dirX = moveDirX;
        float dirY = -moveDirY;

        float x = dirX * cameraForwardY + dirY * cameraForwardX;
        float z = dirY * cameraForwardY - dirX * cameraForwardX;

        TargetPosition.x.targetValue += x * Time.deltaTime;
        TargetPosition.z.targetValue += z * Time.deltaTime;
        
        UpdateSmoothDynamics();
        ClampInputs();

        float horizontalAngleCurrent = this.HorizontalAngle.currentValue;
        float verticalAngleCurrent = this.VerticalAngle.currentValue;
        float zoom = this.Zoom.currentValue;
        
        Vector3 target = TargetPosition.Get();
        
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
        
        float verticalCurrent = VerticalAngle.targetValue;

        if (verticalCurrent > MaxAngle)
        {
            VerticalAngle.targetValue = MaxAngle;
        }

        if (verticalCurrent< MinAngle)
            VerticalAngle.targetValue = MinAngle;

        float zoomCurrent = Zoom.targetValue;

        if (zoomCurrent < MinimumZoom)
        {
            Zoom.targetValue = MinimumZoom;
        }
            
        if (zoomCurrent > MaxZoom)
        {
            Zoom.targetValue = MaxZoom;
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
    public SmoothFloat x = new SmoothFloat(5, 0);
    public SmoothFloat z = new SmoothFloat(5, 0);

    public void SetHard(Vector3 vector)
    {
        x.targetValue = x.currentValue = vector.x;
        z.targetValue = z.currentValue = vector.z;
    }

    public void SetSmooth(Vector3 vector)
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
        return new Vector3(x.currentValue, 3, z.currentValue);
    }
}

