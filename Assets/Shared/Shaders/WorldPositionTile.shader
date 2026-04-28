Shader "Custom/WorldPositionTile"
{
    Properties
    {
        [MainTexture] _TopTex("Top", 2D) = "white" {}
        _FrontTex("Front", 2D) = "white" {}
        _SideTex("Side", 2D) = "white" {}
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
                int texture_selection : TEXCOORD3;
            };
            
            TEXTURE2D(_TopTex);
            TEXTURE2D(_FrontTex);
            TEXTURE2D(_SideTex);
            SAMPLER(sampler_TopTex);
            SAMPLER(sampler_FrontTex);
            SAMPLER(sampler_SideTex);
            CBUFFER_START(UnityPerMaterial)
                float4 _TopTex_ST;
                float4 _FrontTex_ST;
                float4 _SideTex_ST;
            CBUFFER_END
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;                
                
                float4 posWs = float4(IN.positionOS.xyz, 1.0);
                OUT.positionCS = mul(UNITY_MATRIX_MVP, posWs);
                VertexPositionInputs positions = GetVertexPositionInputs(IN.positionOS);
                
                float4 normalWs = mul(UNITY_MATRIX_M, float4(IN.normal, 0.0));
                OUT.texture_selection = abs(normalWs.y) > 0.707106781187 ? 0 : abs(normalWs.x) > abs(normalWs.z) ? 1 : 2;
                switch (OUT.texture_selection)
                {
                case 1:
                    OUT.uv = TRANSFORM_TEX(positions.positionWS.yz, _SideTex);
                    break;
                case 2:
                    OUT.uv = TRANSFORM_TEX(positions.positionWS.xy, _FrontTex);
                    break;
                default:
                    OUT.uv = TRANSFORM_TEX(positions.positionWS.xz, _TopTex);
                    break;
                }
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                switch (IN.texture_selection)
                {
                case 1:
                    return SAMPLE_TEXTURE2D(_SideTex, sampler_SideTex, IN.uv);
                case 2:
                    return SAMPLE_TEXTURE2D(_FrontTex, sampler_FrontTex, IN.uv);
                    
                default:
                    return SAMPLE_TEXTURE2D(_TopTex, sampler_TopTex, IN.uv);
                }
            }
            
            ENDHLSL
        }
    }
}
