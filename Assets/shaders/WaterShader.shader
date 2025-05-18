Shader "Custom/WaterTransparentLit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Wave Amplitude", Float) = 0.5
        _Speed ("Wave Speed", Float) = 1.0
        _Color ("Tint Color", Color) = (1,1,1,0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Amplitude;
            float _Speed;
            fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float offset = sin(_Time.y * _Speed) * _Amplitude;
                o.uv.y += offset;

                o.normalDir = UnityObjectToWorldNormal(v.normal);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texcol = tex2D(_MainTex, i.uv) * _Color;


                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                float NdotL = max(0, dot(i.normalDir, lightDir));

                fixed4 col = texcol;
                col.rgb *= (0.3 + 0.7 * NdotL); 

                col.a = texcol.a;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
