Shader "Unlit/ClothShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags{
            "RenderType" = "Opaque"
            "Queue" = "Transparent"
            }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
                
                float2 uv : TEXCOORD0;
                uint vertexID : SV_VertexID;

            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 points[2000];
            float4 size;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                 v.vertex = points[v.vertexID];

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    } 
}
