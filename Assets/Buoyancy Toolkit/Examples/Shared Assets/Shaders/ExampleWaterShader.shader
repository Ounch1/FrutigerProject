// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Buoyancy Toolkit/Example Water Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5
        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "ForceNoShadowCasting"="True"
        }

        CGPROGRAM
        #pragma surface surf Standard vertex:vert alpha noshadow
        #pragma target 3.0

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            INTERNAL_DATA
        };

        void vert(inout appdata_full v)
        {
            float3 p = mul(unity_ObjectToWorld, v.vertex);

            // Matches the custom wave function in CustomFluidVolume.cs
            v.vertex.y += sin(p.x * 0.5 + _Time.y) * 0.8 + sin(p.z * 0.5 + _Time.y) * 0.2;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            half4 c = _Color * tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Diffuse"
}