Shader "Unlit/SmokeShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FrontSpeed("Front Speed", Float) = 0.25
        _BackSpeed("Back Speed", Float) = 0.5
        _WindModifier("Wind", Float) = 0
        _CenterY("Center Y", Float) = 0.2
        _Radius("Radius", Float) = 1
        _Pixelation("Pixelation", Range(0.0001, 1)) = 0.3
        _Threshold("Alpha Threshold", Range(0, 1)) = 0.05
        _Colour("Colour", Color) = (1, 1, 1, 1)
        _Height("Height", Float) = 2

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
            #pragma enable_d3d11_debug_symbols

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

            float _FrontSpeed;
            float _BackSpeed;
            float _WindModifier;

            float _CenterY;
            float _Radius;
            float _Height;

            float _Pixelation;
            float _Threshold;

            float4 _Colour;



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

            float eggShape(float2 coord, float radius)
            {
                float2 center = float2(0.5, _CenterY);

                float2 diff = abs(coord - center);

                if (coord.y > _CenterY)
                {
                    diff.y /= _Height;
                }
                
                else
                {
                    diff.y *= 2;             
                }

                float dist = sqrt(diff.x * diff.x + diff.y * diff.y) / radius;
                float value = clamp(1 - dist, 0, 1);
                return clamp(value*value, 0, 1);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 scaledCoords = i.uv * 6;

                float warp = 1-i.uv.y;
                float distFromCenter = abs(i.uv.x - 0.5) * 4;
                if (i.uv.x > 0.5)
                {
                    warp = 1 - warp;
                }

                float2 warpVec = float2(warp, 0) * distFromCenter;

                float motionFbm = fbm(scaledCoords - float2(_Time.y * 0.4, _Time.y * 1.3));
                float smokeFbm = fbm(scaledCoords - float2(_Time.y * _WindModifier, _Time.y * 1) + motionFbm + warpVec);
 
                float egg = eggShape(i.uv, _Radius);
                             

                smokeFbm *= egg;
                smokeFbm = clamp(smokeFbm - _Threshold, 0, 1) / (1 - _Threshold);
                if (smokeFbm < 0.1)
                {
                    smokeFbm *= smokeFbm/0.1;
                }
                smokeFbm /= egg;
                smokeFbm = sqrt(smokeFbm);
                smokeFbm = clamp(smokeFbm, 0, 1);


                return fixed4(smokeFbm * _Colour.r, smokeFbm * _Colour.g, smokeFbm * _Colour.b,  smokeFbm - smokeFbm % _Pixelation);
            }
            ENDCG
        }
    }
}
