Shader "Unlit/WaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaterColour("Water Colour", Color) = (0.5, 0.5, 0.8, 0.45)
        _CrestColour("Water Crest Colour", Color) = (0.7, 0.7, 0.8, 0.8)
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

            float4 _WaterColour;
            float4 _CrestColour;

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
                    v.vertex.y = ((points[v.vertexID/2] + size.y) / size.y - 0.55);           

                    v.vertex.y += (0.03 * sin(v.vertexID/2 * 0.2f + _Time*60)) / size.y;
                    v.vertex.y += (0.05 * sin(v.vertexID/2 * 0.1f - _Time*50)) / size.y;   
                    

                    v.vertex.y += (0.15 * sin(0.1f - _Time*20)) / size.y;             
                }

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (i.uv.y > 0.5 - (0.07 / (size.y))){
                     return _CrestColour; 
                    }
                
                return _WaterColour;
                
            }

            ENDCG
        }
    }
}
