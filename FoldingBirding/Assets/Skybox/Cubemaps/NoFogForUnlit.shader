// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/NoFogForUnlit"
{
    Properties
    {
        _TopSkyColor("Top Sky Color", Color) = (0.5, 0.5, 0.5, 1)
        _MiddleSkyColor("Middle Sky Color", Color) = (0.2, 0.2, 0.2, 1)
        _HorizonColor("Horizon Color", Color) = (1, 1, 1, 1)
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            uniform float4 _TopSkyColor;
            uniform float4 _MiddleSkyColor;
            uniform float4 _HorizonColor;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            // float4x4 _Object2World;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float height = saturate((i.worldPos.y + 5.0) / 10.0); // -5~5 ¡æ 0~1

                float4 grad = lerp(_MiddleSkyColor, _TopSkyColor, height);
                float4 finalColor = lerp(_HorizonColor, grad, height);

                return finalColor;
            }
            ENDCG
        }
    }
}
