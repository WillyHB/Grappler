Shader "Unlit/FireShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _InnerColour("Inner Colour", Color) = (1, 1, 1, 1)
        _OuterColour("Outer Colour", Color) = (1, 1, 1, 1)
        _TestValue("Test", Float) = 0
    }
    SubShader
    {
        Tags{
            "RenderType" = "Opaque"
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

            float4 _InnerColour;
            float4 _OuterColour;
            float _Test;
            uniform float4 transparent = (0, 0, 0, 0);
            sampler2D _MainTex;
            float4 _MainTex_ST;

            float rand(float2 coord)
            {         
                return frac(sin(dot(coord, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float noise(float2 coord)
            {

                float2 i = floor(coord);
                float2 f = frac(coord);

                float a = rand(i);
                float b = rand(i + float2(1, 0));
                float c = rand(i + float2(0, 1));
                float d = rand(i + float2(1, 1));

                float2 cubic = f * f * (3.0 - 2.0 * f);

                return lerp(a,b, cubic.x) + (c-a) * cubic.y * (1.0 - cubic.x) + (d-b) * cubic.x * cubic.y;
            }

            float fbm(float2 coord){
                //fractal brownian movement
                float value = 0;

                float scale = 0.5;

                for (int i = 0; i < 6; i++){
                    value += noise(coord) * scale;
                    coord *= 2;
                    scale *= 0.5;
                    }

                 return value;
            }



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float overlay(float base, float top)
            {
                if (base < 0.5)
                {
                    return 2 * base * top;
                }

                else
                {
                    return 1-2 * (1-base) * (1-top);
                }
            }

            float eggShape(float2 coord, float radius)
            {
                float2 center = float2(0.5, 0.2);

                float2 diff = abs(coord - center);

                if (coord.y > center.y)
                {
                    diff.y /= 2;
                }
                
                else
                {
                    diff.y *= 2;             
                }

                float dist = sqrt(diff.x * diff.x + diff.y * diff.y) / radius;
                float value = sqrt(1 - dist * dist);
                return clamp(value, 0, 1);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 coord = i.uv * 8;
                float2 fbmCoord = coord / 6;

                float egg = eggShape(i.uv, 0.4);
                float egg2 = eggShape(i.uv, 0.2);


                _Test = 3;


                float noise1 = noise(coord - float2(_Time.y * 0.25, _Time.y * 3.5));
                float noise2 = noise(coord - float2(_Time.y * 0.5, _Time.y * 5.5));
                float combinedNoise = (noise1+noise2) / 2;

                float fbmNoise = fbm(fbmCoord - float2(0, _Time.y * 3));
                fbmNoise = overlay(fbmNoise, 0.9-i.uv.y);

                float combined = combinedNoise * fbmNoise * egg;
                float val = combined;
                fixed4 col = fixed4(val,val, val, 1);

                return col;
            }
            ENDCG
        }
    }
}
