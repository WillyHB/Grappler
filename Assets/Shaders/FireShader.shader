Shader "Unlit/FireShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FirstColour("First Colour", Color) = (1, 1, 1, 1)
        _SecondColour("Second Colour", Color) = (1, 1, 1, 1)
        _ThirdColour("Third Colour", Color) = (1, 1, 1, 1)
        _FourthColour("Fourth Colour", Color) = (1, 1, 1, 1)
        _FirstThreshold("First Threshold", Range(0, 1)) = 0
        _SecondThreshold("Second Threshold",  Range(0, 1)) = 0
        _ThirdThreshold("Third Threshold",  Range(0, 1)) = 0
        _FourthThreshold("Fourth Threshold",  Range(0, 1)) = 0
        _FrontSpeed("Front Speed", Float) = 0.25
        _BackSpeed("Back Speed", Float) = 0.5
        _WindModifier("Wind", Float) = 0
        _CenterY("Center Y", Float) = 0.2
        _Radius("Radius", Float) = 1
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
            #pragma enable_d3d11_debug_symbols
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

            float4 _FirstColour;
            float4 _SecondColour;
            float4 _ThirdColour;
            float4 _FourthColour;

            float _FirstThreshold;
            float _SecondThreshold;
            float _ThirdThreshold;
            float _FourthThreshold;

            float _FrontSpeed;
            float _BackSpeed;
            float _WindModifier;

            float _CenterY;
            float _Radius;
            float _Height;

            uniform float4 transparent = (0, 0, 0, 0);
            sampler2D _MainTex;
            float4 _MainTex_ST;
            int customID;
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
                // Define the center of the shape
                float2 center = float2(0.5, _CenterY);
            
                // Calculate the difference
                float2 diff = abs(coord - center);
            
                // Smoothly interpolate the vertical scaling based on position relative to _CenterY
                float blendFactor = smoothstep(_CenterY - 0.1, _CenterY + 0.1, coord.y);
                diff.y = lerp(diff.y * 2.0, diff.y / _Height, blendFactor);
            
                // Compute the normalized distance
                float dist = length(diff) / radius;
            
                // Generate a smooth radial gradient
                float value = 1.0 - dist * dist;
            
                // Clamp to ensure the gradient remains in range
                return clamp(value, 0.0, 1.0);
            }
            /*
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
                float value = sqrt(1 - dist * dist);
                return clamp(value, 0, 1);
            }
                */

            fixed4 frag (v2f i) : SV_Target
            {
                float2 coord = i.uv * 8;
                float2 fbmCoord = coord / 6.0;

                float egg = eggShape(i.uv, _Radius);                
                egg += eggShape(i.uv, lerp(0, _Radius, 0.75)) / 2;
                egg += eggShape(i.uv, lerp(0, _Radius, 0.5)) / 2;
                egg += eggShape(i.uv, lerp(0, _Radius, 0.25)) / 2;

                float noise1 = noise(coord - float2((_Time.y+customID) * _WindModifier, _Time.y * _FrontSpeed));
                float noise2 = noise(coord - float2((_Time.y+customID) * _WindModifier + 0.5, _Time.y * _BackSpeed
                    ));
                float combinedNoise = (noise1+noise2) / 2;

                float fbmNoise = fbm(fbmCoord - float2(0, (_Time.y+customID) * 3));
                fbmNoise = overlay(fbmNoise, 0.9-i.uv.y);
 
                float combined = combinedNoise * fbmNoise * egg;


                float val = combined;
                
                if (combined < _FourthThreshold)
                {
                    return fixed4(0, 0, 0, 0);
                }

                else if (combined < _ThirdThreshold)
                {
                    return _FourthColour;
                }

                else if (combined < _SecondThreshold)
                {
                    return _ThirdColour;
                }

                else if (combined < _FirstThreshold)
                {
                    return _SecondColour;
                }

                else
                {
                    return _FirstColour;
                }
            }
            ENDCG
        }
    }
}
