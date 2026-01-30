Shader "Custom/SimpleAlwaysOutline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0.0001, 0.1)) = 0.02
    }

    SubShader
    {
        // Draw after normal object so outline is visible
        Tags { "RenderType"="Opaque" "Queue"="Transparent+100" }

        Cull Front
        ZWrite Off
        ZTest Less

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _OutlineWidth;
            float4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;

                // -----------------------------
                // AUTO SCALING OUTLINE
                // -----------------------------
                float3 worldScale = float3(
                    length(unity_ObjectToWorld._11_12_13),
                    length(unity_ObjectToWorld._21_22_23),
                    length(unity_ObjectToWorld._31_32_33)
                );

                // Use the largest axis as scale reference
                float scale = max(max(worldScale.x, worldScale.y), worldScale.z);

                // Normalize normals and apply scaled width
                float3 norm = normalize(v.normal) * (_OutlineWidth / scale);

                // Push vertices outward
                o.pos = UnityObjectToClipPos(v.vertex + float4(norm, 0));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
