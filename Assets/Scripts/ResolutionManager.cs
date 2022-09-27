using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResolutionManager : MonoBehaviour
{
    public Camera MainCamera;
    public Camera RenderCamera;
    public Camera PlayerCamera;

    public GameObject Quad;

    public static RenderTexture RenderTexture { get; set; }

    public bool AutomaticallyConfigureAspectRatio;
    private bool acar;

    public float AspectRatio;
    private float aspectRatio;

    public int VirtualHeight = 180;
    private int virtualHeight;

    [Min(0)]
    public float Zoom;

    private float zoom;

    public static Vector2Int VirtualDimensions { get; private set; } = new();

    public static Vector2Int ClientDimensions { get; private set; } = new();

    public static float ScaleValue { get; private set; }

    private void Awake()
    {
        CameraEffects.Zoom += (v) => zoom = v;
    }

    private void OnDisable()
    {
        CameraEffects.Zoom -= (v) => zoom = v;
    }

    private void Start()
    {
        ClientDimensions = new Vector2Int(Screen.width, Screen.height);

        if (AutomaticallyConfigureAspectRatio)
        {
            AspectRatio = (float)ClientDimensions.x / ClientDimensions.y;
        }

        int virtualWidth = (int)(VirtualHeight * AspectRatio);

        ScaleValue = (float)ClientDimensions.y / VirtualHeight;

        VirtualDimensions = new Vector2Int(virtualWidth, VirtualHeight);

        CreateRenderTexture();

        MainCamera.targetTexture = RenderTexture;

        Material renderMaterial = new(Shader.Find("Universal Render Pipeline/Unlit"))
        {
            mainTexture = RenderTexture
        };

        Quad.GetComponent<MeshRenderer>().material = renderMaterial;


        float orthoSize = (float)VirtualDimensions.x / ((((float)VirtualDimensions.x / VirtualDimensions.y) * 2) * 16);


        Quad.transform.localScale =
            new Vector3((orthoSize * 2) * AspectRatio + 0.2f, (orthoSize * 2) + 0.2f, 1);

        RenderCamera.orthographicSize = orthoSize - Zoom;
        PlayerCamera.orthographicSize = orthoSize - Zoom;

        aspectRatio = AspectRatio;
        virtualHeight = VirtualHeight;
        acar = AutomaticallyConfigureAspectRatio;
        zoom = Zoom;
    }

    private void CreateRenderTexture()
    {
        if (RenderTexture != null)
        {
            RenderTexture.Release();
            RenderTexture = null;
        }

        RenderTexture = new RenderTexture(VirtualDimensions.x, VirtualDimensions.y, 24)
        {
            filterMode = FilterMode.Point
        };

        RenderTexture.Create();
    }

    private void Update()
    {
        if (aspectRatio != AspectRatio
            || virtualHeight != VirtualHeight
            || new Vector2Int(Screen.width, Screen.height) != ClientDimensions
            || acar != AutomaticallyConfigureAspectRatio
            || zoom != Zoom)
        {
            Start();
        }
    }
}
