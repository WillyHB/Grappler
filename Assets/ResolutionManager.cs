using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public Camera MainCamera;
    public Camera RenderCamera;
    public Camera PlayerCamera;

    public GameObject Quad;

    public RenderTexture RenderTexture { get; set; }

    public bool AutomaticallyConfigureAspectRatio;
    private bool acar;

    public float AspectRatio;
    private float aspectRatio;

    public int VirtualHeight = 180;
    private int virtualHeight;

    public Vector2Int VirtualDimensions { get; private set; } = new();

    public Vector2Int ClientDimensions { get; private set; } = new();

    public float ScaleValue { get; private set; }

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
            new Vector3((RenderCamera.orthographicSize * 2) * AspectRatio, (RenderCamera.orthographicSize * 2), 1);

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
