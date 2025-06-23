Shader "Custom/NoFogForUnlit"
{
    Properties
    {
        _TopColor("Top Sky Color", Color) = (0.2, 0.4, 1, 1)
        _MidColor("Middle Sky Color", Color) = (0.5, 1.0, 1.0, 1)
        _BottomColor("Horizon Color", Color) = (1, 1, 1, 1)
    }

        SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            Name "GradientSky"
            Tags { "LightMode" = "UniversalForward" }

            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 position : POSITION;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 clipPosition : SV_POSITION;
                float3 worldPosition : TEXCOORD0;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 _TopColor;
            float4 _MidColor;
            float4 _BottomColor;

            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.worldPosition = mul(unity_ObjectToWorld, v.position).xyz;
                o.clipPosition = UnityObjectToClipPos(v.position.xyz);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float height = saturate((i.worldPosition.y + 10.0) / 20.0);

                float t1 = saturate(height * 2.0);
                float t2 = saturate((height - 0.5) * 2.0);
                float3 color = lerp(_BottomColor.rgb, _MidColor.rgb, t1);
                color = lerp(color, _TopColor.rgb, t2);

                return float4(color, 1);
            }
            ENDCG
        }
    }

        FallBack Off
}
