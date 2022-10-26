Shader "Unlit/FogShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Strength("Fog Strength", Float) = 1
        _Colour("Fog Colour", Color) = (1, 1, 1, 1)
        _Speed("Fog Speed", Float) = 0.5
        _Pixelation("Fog Pixelation", Range(0.00001, 0.5)) = 0.00001
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

            struct meshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            uniform int octaves = 4;

            float _Pixelation;
            float _Speed;
            float4 _Colour;
            float _Strength;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (meshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float rand(float2 coord)
            {         
                return frac(sin(dot(coord, float2(56, 78)) * 1000.0) * 1000.0);
            }

            float noise(float2 coord)
            {

                float2 i = floor(coord) ;
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

                for (int i = 0; i < 4; i++){
                    value += noise(coord) * scale;
                    coord *= 2;
                    scale *= 0.5;
                    }

                 return value;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 coord = i.uv * 25;

                float2 motion = (float2)(fbm(coord + _Time.y * _Speed));            
                
                float final = fbm(coord + motion);

               float ax = i.uv.x > 0.5 ? 1 - i.uv.x : i.uv.x;
               float ay = i.uv.y > 0.5 ? 1 - i.uv.y : i.uv.y;

                float a = abs(ax * ay);

                return fixed4(_Colour.x, _Colour.y, _Colour.z, (final - final % _Pixelation) * _Strength * a);
            }

            ENDCG
        }
    }
}
