Shader "Custom/WorldPositionTileTop"
{
    Properties
    {
        [MainTexture] _TopTex("Top", 2D) = "white" {}
    }

    SubShader
    {
        Pass
        {
            Tags
            {
                "Queue" = "Lit"
                "RenderPipeline" = "UniversalPipeline"
            }
            
            AlphaToMask On
            
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"

            #pragma vertex vert
            #pragma fragment frag
            

            struct Attributes
            {
                float4 positionOS      : POSITION;
                float2 uv              : TEXCOORD0;
                float3 normal          : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float2 uv: TEXCOORD0;
            };
            
            TEXTURE2D(_TopTex);
            SAMPLER(sampler_TopTex);
            CBUFFER_START(UnityPerMaterial)
                float4 _TopTex_ST;
            CBUFFER_END
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;                
                
                float4 posWs = float4(IN.positionOS.xyz, 1.0);
                OUT.positionCS = mul(UNITY_MATRIX_MVP, posWs);
                VertexPositionInputs positions = GetVertexPositionInputs(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(positions.positionWS.xz, _TopTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_TopTex, sampler_TopTex, IN.uv);
            }
            
            ENDHLSL
        }
    }
}
