Shader "Unlit/MirrorShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CrackTex("Texture", 2D) = ""{}
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque"
            "Queue" = "Transparent"
        }
     
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 size;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _CrackTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                if (tex2D(_CrackTex, i.uv).a != 0)
                {
  
                     col = tex2D(_MainTex, i.uv - 0.05) + fixed4(0.3, 0.3, 0.3, 1);

                     if (tex2D(_CrackTex, i.uv).r == 1) col += + fixed4(0.3, 0.3, 0.3, 1);
                }


                if (col.a < 0.9) col = fixed4(0.5, 0.5, 0.75, 1);
                else col += fixed4(0.3, 0.3, 0.5, 0.5);

                if (i.uv.x * size.x < 0.1 || i.uv.y * size.y < 0.1 || i.uv.y * size.y > size.y * 0.99 || i.uv.x * size.x > size.x * 0.99)
                {
                   return fixed4(0.7, 0.7, 0.85, 1);
                }



                return col;
            }
            ENDCG
        }
    }
}
