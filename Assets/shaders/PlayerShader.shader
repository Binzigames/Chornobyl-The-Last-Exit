Shader "Custom/BlurPlayer"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 10)) = 2.0
        _EdgeThreshold ("Edge Threshold", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlurSize;
            float _EdgeThreshold;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float edgeFactor = smoothstep(_EdgeThreshold, _EdgeThreshold + 0.05, abs(i.normal.z));
                float2 texelSize = 1.0 / _ScreenParams.xy;
                float4 blurredColor = tex2D(_MainTex, i.uv);
                
                blurredColor += tex2D(_MainTex, i.uv + texelSize * _BlurSize);
                blurredColor += tex2D(_MainTex, i.uv - texelSize * _BlurSize);
                blurredColor += tex2D(_MainTex, i.uv + float2(texelSize.x, -texelSize.y) * _BlurSize);
                blurredColor += tex2D(_MainTex, i.uv + float2(-texelSize.x, texelSize.y) * _BlurSize);
                blurredColor /= 5.0;
                
                // Робимо силует чорним і додаємо розмиття тільки до країв
                fixed4 silhouette = fixed4(0, 0, 0, 1);
                return lerp(silhouette, blurredColor, edgeFactor * 2.0);
            }
            ENDCG
        }
    }
}