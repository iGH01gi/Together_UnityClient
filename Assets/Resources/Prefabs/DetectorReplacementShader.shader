Shader "Unlit/DetectorReplacementShader"
{
    Properties
    {
        _SetTex ("Main Texture", 2D) = "black" {} // Texture property
        _Color ("Main Color", Color) = (1, 1, 1, 1) // Color property
        _Smoothness("_Smoothness", Range(0, 1)) = 0
        _Metallic ("Metallic", Range(0,1)) = 0.0 // Float range property
        
        _HighlightValue ("HighlightValue", Float) = 0.0
        [HDR]_Rim_Color("Rim Color", Color) = (0, 0, 0, 0)
        _BaseColor("_BaseColor", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard

        // Include necessary libraries
        #include "UnityCG.cginc"

        // Define the properties in CGPROGRAM
        sampler2D _SetTex; // Texture sampler
        fixed4 _Color; // Fixed4 is a 4-component vector (RGBA color)
        half _Smoothness; // Half is a half-precision float
        half _Metallic; // Half is a half-precision float

        half _HighlightValue;
        fixed4 _Rim_Color;
        fixed4 _BaseColor;

        struct Input
        {
            float2 uv_MainTex; // UV coordinates for the main texture
        };

        // Surface shader function
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the texture using the UV coordinates
            fixed4 tex = tex2D(_SetTex, IN.uv_MainTex);

            // Use the texture color multiplied by the _Color property
            //o.Albedo = tex.rgb * _Color.rgb;
            o.Albedo = tex.rgb * lerp(_BaseColor,_Rim_Color,_HighlightValue);

            // Set smoothness and metallic based on properties
            o.Smoothness = _Smoothness;
            o.Metallic = _Metallic;
            o.Occlusion = 0.5;
            
            //o.Emission = _Rim_Color.rgb * _HighlightValue;
            
           // o.Normal
            //o.Emission
            //o.Alpha
        }

        /*half4 frag (Input IN) : SV_Target
            {
                // Determine the color output based on _HighlightValue
                fixed4 color = tex2D(_SetTex, IN.uv_MainTex);
                
                if (_HighlightValue <= 0.1)
                {
                    discard;
                }

                // Output color and alpha
                return color;
            }*/

        
        ENDCG
    }
    Fallback "Diffuse"
   /*Properties
    {
        _HighlightValue ("HighlightValue", Float) = 0.0
        [HDR]_Rim_Color("Rim Color", Color) = (0, 0, 0, 0)
        _BaseColor("_BaseColor", Color) = (1, 1, 1, 1)

    }
    SubShader
    {
        Tags {
            "RenderType"="Opaque"
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _EmissionMap;
            float _HighlightValue;
            fixed4 _Rim_Color;
            fixed4 _BaseColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_EmissionMap, i.uv);
                return col;
            }
            ENDCG
        }
    }
    Fallback Off*/
}
