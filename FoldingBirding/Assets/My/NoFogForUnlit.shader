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

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
            };

            float4 _TopColor;
            float4 _MidColor;
            float4 _BottomColor;

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionHCS = TransformWorldToHClip(positionWS);
                output.positionWS = positionWS;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float height = saturate((input.positionWS.y + 10.0) / 20.0); // y = -10 ~ +10 → 0 ~ 1

                // 2단계 그라데이션 (하단↔중간↔상단)
                float t1 = saturate(height * 2.0);
                float t2 = saturate((height - 0.5) * 2.0);
                float3 color = lerp(_BottomColor.rgb, _MidColor.rgb, t1);
                color = lerp(color, _TopColor.rgb, t2);

                return float4(color, 1);
            }
            ENDHLSL
        }
    }

        FallBack Off
}
