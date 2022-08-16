using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResolutionManager : MonoBehaviour
{
    public Camera MainCamera;
    public Camera RenderCamera;

    public GameObject Quad;

    public static RenderTexture RenderTexture { get; set; }

    public bool AutomaticallyConfigureAspectRatio;
    private bool acar;

    public float AspectRatio;
    private float aspectRatio;

    public int VirtualHeight = 180;
    private int virtualHeight;

    public static Vector2Int VirtualDimensions { get; private set; } = new();

    public static Vector2Int ClientDimensions { get; private set; } = new();

    public static float ScaleValue { get; private set; }

    private void Start()
    {
        ClientDimensions = new Vector2Int(Screen.width, Screen.height);

        if (AutomaticallyConfigureAspectRatio)
        {
            AspectRatio = (float)ClientDimensions.x / ClientDimensions.y;
        }

        int virtualWidth = (int)(VirtualHeight * AspectRatio);

        ScaleValue = ClientDimensions.y / VirtualHeight;

        VirtualDimensions = new Vector2Int(virtualWidth, VirtualHeight);

        CreateRenderTexture();

        MainCamera.targetTexture = RenderTexture;

        Material renderMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        renderMaterial.mainTexture = RenderTexture;

        Quad.GetComponent<MeshRenderer>().material = renderMaterial;

        Quad.transform.localScale =
            new Vector3((RenderCamera.orthographicSize * 2) * AspectRatio + 0.2f, (RenderCamera.orthographicSize * 2) + 0.2f, 1);

        aspectRatio = AspectRatio;
        virtualHeight = VirtualHeight;
        acar = AutomaticallyConfigureAspectRatio;
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
            || acar != AutomaticallyConfigureAspectRatio)
        {
            Start();
        }
    }
}
