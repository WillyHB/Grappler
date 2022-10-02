using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResolutionManager : MonoBehaviour
{
    public RenderCamera[] Cameras;

    public Camera RenderCamera;

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
        float orthoSize = (float)VirtualDimensions.x / ((((float)VirtualDimensions.x / VirtualDimensions.y) * 2) * 16);

        CreateRenderTextures();


        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].Camera.targetTexture = Cameras[i].RenderTexture;
            Cameras[i].RenderMaterial.mainTexture = Cameras[i].RenderTexture;
            Cameras[i].RenderSurface.GetComponent<MeshRenderer>().material = Cameras[i].RenderMaterial;

            Cameras[i].RenderSurface.transform.localScale = new Vector3((orthoSize * 2) * AspectRatio + 0.2f, (orthoSize * 2) + 0.2f, 1);
        }

        RenderCamera.orthographicSize = orthoSize - Zoom;

        aspectRatio = AspectRatio;
        virtualHeight = VirtualHeight;
        acar = AutomaticallyConfigureAspectRatio;
        zoom = Zoom;
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
