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

                if (i.uv.x * size.x < 0.1 || i.uv.y * size.y < 0.1 || i.uv.y * size.y > size.y * 0.98 || i.uv.x * size.x > size.x * 0.98)
                {
                   col = fixed4(0.7, 0.7, 0.85, 1);
                }

                if (tex2D(_CrackTex, i.uv).a == 1)
                {
  
                    col = tex2D(_CrackTex, i.uv);
                }

                else if (tex2D(_CrackTex, i.uv).a != 0)
                {
                    col = tex2D(_MainTex, i.uv - 0.1) - fixed4(tex2D(_CrackTex, i.uv).r,tex2D(_CrackTex, i.uv).g,tex2D(_CrackTex, i.uv).b, 0);
                }

                col += fixed4(0.1, 0.1, 0.2, 0);   

                return col;
            }
            ENDCG
        }
    }
}
