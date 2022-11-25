Shader "Unlit/LineShader"
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

            struct ropeSegment
            {
                float2 oldPos;
                float2 currentPos;
            };


            sampler2D _MainTex;
            float4 _MainTex_ST;
            float numberOfSegments = 10;
            float segmentSize = 0.25;

            float gravityScale = 1;


            ropeSegment segments[500];

            void constraint()
            {
                for (int i = 0; i < 500 - 1; i++)
                {   
                    ropeSegment firstSeg = segments[i];
                    ropeSegment secondSeg = segments[i + 1];

                    // The distance between the two rope segments

                    float2 diff = float2(secondSeg.currentPos.x - firstSeg.currentPos.x, secondSeg.currentPos.y - firstSeg.currentPos.y);
                    float dist = sqrt(pow(diff.x, 2) + pow(diff.y, 2));

                    // The amount that the rope is stretched out of it's normal operating range
                    float error = dist - segmentSize;

                    // Normalized vector between the two segments
                    float2 changeDir = float2(normalize(diff.x), normalize(diff.y));

                    // the vector that the rope segment has to move to pull itself back to normal operating range
                    float2 changeAmount = changeDir * error;

                    if (i != 0)
                    {
                        firstSeg.currentPos -= changeAmount * 0.5f;
                        segments[i] = firstSeg;
                        secondSeg.currentPos += changeAmount * 0.5f;
                        segments[i + 1] = secondSeg;
                    }
                    else
                    {
                        secondSeg.currentPos += changeAmount;
                        segments[i + 1] = secondSeg;
                    }
                }
            }

            void simulate()
            {
                        // SIMULATION
                for (int i = 1; i < 500; i++)
                {
                    ropeSegment firstSeg = segments[i];
                    float2 velocity = float2(firstSeg.currentPos.x - firstSeg.oldPos.x, firstSeg.currentPos.y - firstSeg.oldPos.y);

                    /*
                    if (i != numberOfPoints - 1 && i != 0)
                    {
                        velocity += Random.Range(GustPowerFrom, GustPowerTo) * Time.deltaTime * NormalizedWindDirection.normalized;
                    }*/

                    firstSeg.oldPos = firstSeg.currentPos;
                    firstSeg.currentPos += velocity;
                    firstSeg.currentPos.y -= gravityScale * 0.01;

                    segments[i] = firstSeg;
                }

                //CONSTRAINTS
                for (int j = 0; j < 100; j++)
                {
                    constraint();
                }
            }

            v2f vert (appdata v)
            {
                v2f o;

                simulate();

                v.vertex.y = segments[v.vertexID].currentPos.y;
                v.vertex.x = segments[v.vertexID].currentPos.x;
                o.vertex = UnityObjectToClipPos(v.vertex);


                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                return col;
            }
            ENDCG
        }
    }
}
