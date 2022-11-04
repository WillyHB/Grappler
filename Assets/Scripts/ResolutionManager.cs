using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public RenderCamera[] Cameras;

    public Camera RenderCamera;

    public bool AutomaticallyConfigureAspectRatio;
    private bool acar;

    public float _AspectRatio;

    public int VirtualHeight = 180;
    private int virtualHeight;

    [Min(0)]
    public float Zoom;

    private float currentZoom;

    public float ZoomSmoothing = 0.075f;
    public static int UnitSize => 16;
    public static Vector2Int VirtualDimensions { get; private set; } = new();

    public static Vector2Int ClientDimensions { get; private set; } = new();

    public static float AspectRatio { get; private set; }
    public static float ScaleValue { get; private set; }

    public static float CameraZoom { get; private set; }

    public CameraEventChannel CamEventChannel;

    private LTDescr zoomCamTween;

    private void Awake()
    {
        CamEventChannel.Zoom += v => Zoom = v;
    }

    private void OnDisable()
    {
        CamEventChannel.Zoom -= v => Zoom = v;
    }

    public static Vector2 ScreenToWorld(Vector2 point)
    {
        float orthoSize = (float)VirtualDimensions.x / ((((float)VirtualDimensions.x / VirtualDimensions.y) * 2) * UnitSize);

        Camera.main.orthographicSize = orthoSize - CameraZoom;
        Vector2 finalPoint = Camera.main.ScreenToWorldPoint(point / ScaleValue);
        Camera.main.orthographicSize = orthoSize;

        return finalPoint;

    }

    private void Start()
    {
        ClientDimensions = new Vector2Int(Screen.width, Screen.height);

        if (AutomaticallyConfigureAspectRatio)
        {
            _AspectRatio = (float)ClientDimensions.x / ClientDimensions.y;
        }

        int virtualWidth = (int)(VirtualHeight * _AspectRatio);

        ScaleValue = (float)ClientDimensions.y / VirtualHeight;

        VirtualDimensions = new Vector2Int(virtualWidth, VirtualHeight);
        float orthoSize = (float)VirtualDimensions.x / ((((float)VirtualDimensions.x / VirtualDimensions.y) * 2) * UnitSize);

        CreateRenderTextures();


        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].Camera.targetTexture = Cameras[i].RenderTexture;
            Cameras[i].RenderMaterial.mainTexture = Cameras[i].RenderTexture;

            Cameras[i].RenderSurface.transform.localScale = new Vector3((orthoSize * 2) * _AspectRatio + 0.2f, (orthoSize * 2) + 0.2f, 1);
        }

        RenderCamera.orthographicSize = GenerateRenderOrthoSize();

        AspectRatio = _AspectRatio;
        virtualHeight = VirtualHeight;
        acar = AutomaticallyConfigureAspectRatio;
        CameraZoom = Zoom;
    } 

    private void CreateRenderTextures()
    {
        for (int i = 0; i < Cameras.Length; i++)
        {
            Vector2Int dimensions = Cameras[i].RenderInVirtualSpace ? VirtualDimensions : ClientDimensions;
            Cameras[i].RenderTexture = new RenderTexture(dimensions.x, dimensions.y, 24)
            {
                filterMode = FilterMode.Point,
                name = Cameras[i].Name,
            };
        }
    }

    private float GenerateRenderOrthoSize()
    {
        float orthoSize = (float)VirtualDimensions.x / ((((float)VirtualDimensions.x / VirtualDimensions.y) * 2) * UnitSize);
        return orthoSize - currentZoom;
    }

    private void Update()
    {
        CameraZoom = Zoom;
        currentZoom = Mathf.Lerp(currentZoom, Zoom, ZoomSmoothing);

        if (AspectRatio != _AspectRatio
            || virtualHeight != VirtualHeight
            || new Vector2Int(Screen.width, Screen.height) != ClientDimensions
            || acar != AutomaticallyConfigureAspectRatio)
        {
            Start();
        }

        RenderCamera.orthographicSize = GenerateRenderOrthoSize();
    }
}

[System.Serializable]
public struct RenderCamera
{
    public string Name;
    public Camera Camera;
    public Material RenderMaterial;
    public RenderTexture RenderTexture { get; set; }
    public GameObject RenderSurface;
    public bool RenderInVirtualSpace;
}
