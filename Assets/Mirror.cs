using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera MirrorCam;
    public Shader MirrorShader;

    public Texture2D CrackTexture;

    private RenderTexture renderTex;
    private Material renderMaterial;
    private MeshRenderer meshRenderer;

    public float DistanceFromMirror = 0.5f;

    private float aspectRatio;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        GenerateRenderSurfaces();
    }

    public void GenerateRenderSurfaces()
    {
        aspectRatio = meshRenderer.bounds.size.x / meshRenderer.bounds.size.y;

        renderTex = new RenderTexture((int)(1080 * aspectRatio), 1080, 24);
        renderMaterial = new Material(MirrorShader);

        renderMaterial.mainTexture = renderTex;

        meshRenderer.material = renderMaterial;

        MirrorCam.targetTexture = renderTex;
        MirrorCam.orthographicSize = meshRenderer.bounds.size.y / 2;
    }
    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(meshRenderer.bounds.size.x / meshRenderer.bounds.size.y - aspectRatio) < Mathf.Epsilon)
        {
            GenerateRenderSurfaces();
        }

        renderMaterial.SetTexture("_CrackTex", CrackTexture);
        renderMaterial.SetVector("size", new Vector4(meshRenderer.bounds.size.x, meshRenderer.bounds.size.y, 0, 0));
        MirrorCam.transform.localPosition = new Vector3(DistanceFromMirror, 0, -5);
    }
}
