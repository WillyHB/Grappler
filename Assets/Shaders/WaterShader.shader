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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 springPoints[2000];
            float springCount;
            float4 size;

            sampler2D _MainTex;
            float4 _MainTex_ST;

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
                //fixed4 col = tex2D(_MainTex, i.uv);

                fixed4 col;

   
                 for (int j = 0; j < springCount-1; j++)
                {
                    if (i.uv.x * size.x >= springPoints[j].x && i.uv.x * size.x <= springPoints[j+1].x)
                    {
                        if (i.uv.y * size.y < springPoints[j].y)
                        {
                            col = fixed4(0.25, 0.5, 0.7, 0.6);

                        }
                    }
                }

                /*
                float value = 0.1*sin(_Time.y + i.uv.x) + 0.9; 


                if (i.uv.y > value){
                    col = fixed4(0, 0, 0, 0);
                    }
                else{
                    col = fixed4(1, 1, 1, 1);
                    }*/

                return col;
                
            }
            ENDCG
        }
    }
}
