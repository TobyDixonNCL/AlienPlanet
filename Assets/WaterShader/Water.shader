// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Water" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Scale ("Scale", float) = 1
        _Speed ("Speed", float) = 1
        _Freq ("Freq", float) = 1
    }
    SubShader {
        Tags {"RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert alpha
        #pragma surface surf Standard fullforwardshadows 

        UNITY_DECLARE_TEXCUBE(_RealtimeCubemap);

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        float _Scale, _Speed, _Freq;


        struct Input {
            float2 uv_MainTex;
            float3 customValue;
            float3 worldRefl;
            float3 viewDir;
        };

        // Adjust vertex positions
        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            half offset = (v.vertex.x * v.vertex.x) + (v.vertex.z * v.vertex.z);
            half value = _Scale * sin(_Time.x *_Speed * offset * _Freq);

            v.vertex.y += value;
            v.normal.y += value;
            o.customValue = value;
        }

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

         

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        half4 _RealtimeCubemap_HDR;

        void surf(Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = _Color.rgb;
            o.Alpha = 0.2f;
            o.Normal.y += IN.customValue;

            Unity_GlossyEnvironmentData glossIn = UnityGlossyEnvironmentSetup(_Glossiness, IN.viewDir, o.Normal, half3(0, 0, 0));
            half3 env0 = Unity_GlossyEnvironment(UNITY_PASS_TEXCUBE(_RealtimeCubemap), _RealtimeCubemap_HDR, glossIn);
 
            o.Emission = env0 * 0.5;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
