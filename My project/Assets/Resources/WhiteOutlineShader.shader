Shader "Custom/WhiteOutlineShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1) // Kolor emisji
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off
            ZWrite Off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _EmissionColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Wymuszenie bia³ego koloru i dodanie emisji
                fixed4 color = tex2D(_MainTex, i.texcoord);
                color.rgb = 1; // Kolor bia³y
                return color + _EmissionColor; // Dodanie emisji
            }
            ENDCG
        }
    }
}