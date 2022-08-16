using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public RenderTexture RenderTexture;

    public Vector2 AspectRatio = new(16, 9);
    private Vector2 aspectRatio;

    public int VirtualHeight = 180;
    private int virtualHeight;

    public Vector2Int VirtualDimensions { get; private set; }

    public Vector2Int ClientDimensions { get; private set; }
    private Vector2Int clientDimensions;

    public float ScaleValue { get; private set; }

    private void Start()
    {
        ClientDimensions = new Vector2Int(Screen.width, Screen.height);

        int virtualWidth = (int)(VirtualHeight * (AspectRatio.x / AspectRatio.y));

        ScaleValue = ClientDimensions.y / VirtualHeight;

        VirtualDimensions = new Vector2Int(virtualWidth, VirtualHeight);

        RenderTexture.width = VirtualDimensions.x;
        RenderTexture.height = VirtualDimensions.y;

        aspectRatio = AspectRatio;
        virtualHeight = VirtualHeight;
        clientDimensions = ClientDimensions;
    }

    private void Update()
    {
        if (aspectRatio != AspectRatio
            || virtualHeight != VirtualHeight
            || clientDimensions != ClientDimensions)
        {
            Start();
        }
    }
}
