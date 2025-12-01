Shader "PointCloud/HexagonAlpha" {

    Properties {

        _Size("Point Size", float) = 0.01

        _Alpha("Global Alpha", Range(0.0, 1.0)) = 1.0
        
        // Filter Properties
        _FilterMin("Filter Min Value", float) = 0.0
        _FilterMax("Filter Max Value", float) = 1.0
    }

    SubShader {

        LOD 200

        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass {

            CGPROGRAM

            #pragma vertex vert

            #pragma fragment frag

            #pragma geometry geom

            #pragma target 4.0

            #include "UnityCG.cginc"


            struct vertexIn {

                float4 pos : POSITION;

                float4 color : COLOR;
                
                // Added for scalar data
                float2 uv : TEXCOORD0; 
            };

            struct vertexOut {

                float4 pos : SV_POSITION;
                float4 color : COLOR0;
                
                // Added to pass scalar data
                float scalarValue : TEXCOORD0; 
            };

            struct geomOut {

                float4 pos : POSITION;
                float4 color : COLOR0;
                
                // Added to pass scalar data
                float scalarValue : TEXCOORD0; 
            };


            float _Size;

            float _Alpha;
            
            // Filter variables
            float _FilterMin;
            float _FilterMax;

          
            vertexOut vert(vertexIn i) {

                vertexOut o;
                o.pos = UnityObjectToClipPos(i.pos);

                o.color = i.color;
                
                // Read scalar value from UV.x
                o.scalarValue = i.uv.x; 

                return o;
            }


            [maxvertexcount(12)]

            void geom(point vertexOut IN[1], inout TriangleStream<geomOut> OutputStream) {

                geomOut OUT;
                OUT.color = IN[0].color;
                
                // Pass scalar value to fragment shader
                OUT.scalarValue = IN[0].scalarValue;

                float2 p[6];

                float xyscale = _ScreenParams.y / _ScreenParams.x;

                float r = _Size; // radius of the hexagon


                p[0] = r * float2(-xyscale, 0);
                p[1] = r * float2(-0.5 * xyscale, 0.866);
                p[2] = r * float2(0.5 * xyscale, 0.866);
                p[3] = r * float2(xyscale, 0);
                p[4] = r * float2(0.5 * xyscale, -0.866);
                p[5] = r * float2(-0.5 * xyscale, -0.866);


                // Add triangles for the hexagon
                // OUT.scalarValue is carried with OUT
                OUT.pos = IN[0].pos + float4(p[0], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[1], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[2], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[0], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[2], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[3], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[0], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[3], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[4], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[0], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[5], 0, 0);
                OutputStream.Append(OUT);

                OUT.pos = IN[0].pos + float4(p[4], 0, 0);
                OutputStream.Append(OUT);
            }


            float4 frag(geomOut i) : COLOR {

                // Filtering Logic: Discard pixels outside the min/max range
                // Discard if scalar < min
                clip(i.scalarValue - _FilterMin); 
                // Discard if scalar > max
                clip(_FilterMax - i.scalarValue); 

                float4 color = i.color;
                color.a *= _Alpha;

                return color;
            }

            ENDCG

        }

    }

    FallBack "Diffuse"

}