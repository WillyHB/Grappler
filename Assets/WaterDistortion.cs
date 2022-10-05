using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDistortion : MonoBehaviour
{
    public Material BackRenderMaterial;
    public Material WaterRenderMaterial;
    public MeshRenderer MeshRenderer;

    // Update is called once per frame
    void Update()
    {
        MeshRenderer.material.SetTexture("backgroundTex", BackRenderMaterial.mainTexture);
        MeshRenderer.material.SetTexture("waterTex", WaterRenderMaterial.mainTexture);
    }
}
