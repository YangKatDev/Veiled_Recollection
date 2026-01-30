Shader "Custom/UVFootstepOutline"
{
    Properties
    {
        _FootprintTex ("Footprint Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (0.5,0,1,1)
        _GlowIntensity ("Glow Intensity", Float) = 5
        _ConeAngle ("Spotlight Cone Angle", Float) = 0.5
        _UVActive ("UV Light Active", Float) = 0   // 🔥 NEW
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend One One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _FootprintTex;
            float3 _LightPos;
            float3 _LightDir;
            float _ConeAngle;
            float _UVActive;       // 🔥 NEW
            fixed4 _GlowColor;
            float _GlowIntensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 🔥 If UV light is OFF → return invisible
                if (_UVActive < 0.5)
                    return fixed4(0,0,0,0);

                float alpha = tex2D(_FootprintTex, i.uv).r;
                float edge = smoothstep(0.1, 0.3, alpha);

                float3 fragDir = normalize(i.worldPos - _LightPos);
                float dotAngle = dot(fragDir, normalize(_LightDir));
                float intensity = smoothstep(cos(_ConeAngle), 1.0, dotAngle);

                fixed4 finalCol = fixed4(0,0,0,0);
                finalCol.rgb += _GlowColor.rgb * _GlowIntensity * intensity * edge;
                finalCol.a = intensity * edge;

                return finalCol;
            }
            ENDCG
        }
    }
}
