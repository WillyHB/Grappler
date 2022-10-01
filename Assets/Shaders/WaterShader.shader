Shader "Unlit/WaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                uint vertexID : SV_VertexID;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            float points[2000];
            float4 size;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                
                if (v.vertexID % 2 == 0)
                {
                    // BOTTOM VERTEX
                }

                else
                {  
                    // TOP VERTEX

     
                    //springs[i].Height += 0.3f * WaveAmplitude * Mathf.Sin((i * 0.15f - Time.time*WaveSpeed));

                   
                    v.vertex.y = ((points[v.vertexID/2] + size.y) / size.y - 0.5);

                    v.vertex.y += 0.01 * sin(v.vertexID/2 * 0.2f + _Time*60);
                    v.vertex.y += 0.025 * sin(v.vertexID/2 * 0.1f - _Time*50);
                   



                }

                
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (i.uv.y > 0.48){
                     return fixed4(0.7, 0.7, 0.8, 0.8); 
                    }
                
                return fixed4(0.5, 0.5, 0.8, 0.4);
                
            }

            ENDCG
        }
    }
}
