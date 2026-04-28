Shader "Custom/Billboard"
{
    Properties
    {
        [MainTexture] _MainTex("Diffuse", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "DisableBatching" = "True"
        }
        Pass
        {
            Tags
            {
                "Queue" = "Overlay"
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
                half4 color            : COLOR;
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float2 uv: TEXCOORD0;
                float3 positionWS : TEXCOORD2;
                half4 color : COLOR;
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
            CBUFFER_END
            
            matrix look_at(float3 direction)
            {
                float3 up = float3(0, 1, 0);

                // Create LookAt matrix
                float3 zaxis = direction;
                float3 xaxis = cross(up, zaxis);
                float3 yaxis = cross(zaxis, xaxis);

                return matrix
                (
                    xaxis.x,            yaxis.x,            zaxis.x,       0,
                    xaxis.y,            yaxis.y,            zaxis.y,       0,
                    xaxis.z,            yaxis.z,            zaxis.z,       0,
                    0, 0, 0,  1
                );
            }
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                
                matrix model = UNITY_MATRIX_M;
                float4 modelPositionWs = float4(model[0][3], model[1][3], model[2][3], 0.0);
                float3 scale = float3
                (
                    sqrt(model[0][0] * model[0][0] + model[0][1] * model[0][1] + model[0][2] * model[0][2]),
                    sqrt(model[1][0] * model[1][0] + model[1][1] * model[1][1] + model[1][2] * model[1][2]),
                    sqrt(model[2][0] * model[2][0] + model[2][1] * model[2][1] + model[2][2] * model[2][2])
                );
                matrix translateScaleModel = matrix
                (
                    scale.x, 0, 0, modelPositionWs.x,
                    0, scale.y, 0, modelPositionWs.y,
                    0, 0, scale.z, modelPositionWs.z,
                    0, 0, 0, 1
                );
                
                float3 cameraLook = GetViewForwardDir();
                cameraLook.y = 0.0;
                cameraLook = normalize(cameraLook);
                
                
                float4 posWs = float4(IN.positionOS.xyz, 1.0);
                posWs = mul(look_at(cameraLook), posWs);
                posWs = mul(translateScaleModel, posWs);
                OUT.positionCS = mul(UNITY_MATRIX_VP, posWs);

                VertexPositionInputs positions = GetVertexPositionInputs(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.positionWS = positions.positionWS.xyz;
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color;
            }
            
            ENDHLSL
        }
    }
}
