Shader "Custom/Sun"
{
    Properties
    {
        _CoreColor("Core Color", Color) = (1, 1, 1, 1)
        _GlowColor("Glow Color", Color) = (0.6, 0.8, 1.0, 1)
        _GlowPower("Glow Power", Float) = 1.5
        _GlowFalloff("Glow Falloff", Float) = 1.5
        _GlowOffset("Glow Offset", Float) = 0.1
    }

        SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Transparent" }

        Pass
        {
            Name "SunGlow"
            Tags { "LightMode" = "UniversalForward" }

            Blend One One      // Additive blending (Glow È¿°ú)
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 viewDirWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            float4 _CoreColor;
            float4 _GlowColor;
            float _GlowPower;
            float _GlowFalloff;
            float _GlowOffset;

            Varyings vert(Attributes input)
            {
                Varyings o;
                float3 posWS = TransformObjectToWorld(input.positionOS.xyz);
                o.positionHCS = TransformWorldToHClip(posWS);
                o.normalWS = TransformObjectToWorldNormal(input.normalOS);
                o.viewDirWS = normalize(_WorldSpaceCameraPos - posWS);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float NdotV = saturate(dot(i.normalWS, i.viewDirWS));
                float glow = pow(max(NdotV - _GlowOffset, 0), _GlowFalloff);
                float3 color = _CoreColor.rgb + _GlowColor.rgb * glow * _GlowPower;
                return float4(color, 1);
            }
            ENDHLSL
        }
    }

        FallBack Off
}
