Shader "Custom/AlphaBlendCulloff"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Tint("Tint", Color) = (1,1,1,1)
		_AlphaScale("Alpha cutoff", Range(0, 1)) = 1
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector" = "true"}
		
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			Cull Front
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float3 worldNormal : TEXCOORD0;
				float3 worldVertex : TEXCOORD1;
				float2 uv : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Tint;
			fixed _AlphaScale;

			v2f vert(appdata v) {
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				o.worldVertex = mul(unity_ObjectToWorld, v.vertex).xyz;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 worldN = normalize(i.worldNormal);
				fixed3 worldL = normalize(UnityWorldSpaceLightDir(i.worldVertex));

				fixed4 texcor = tex2D(_MainTex, i.uv);

				fixed3 albedo = texcor.rgb * _Tint.rgb;

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

				fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldN, worldL));

				return fixed4(ambient + diffuse, texcor.a * _AlphaScale);
			}
			ENDCG
		}

		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			Cull Back
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float3 worldNormal : TEXCOORD0;
				float3 worldVertex : TEXCOORD1;
				float2 uv : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Tint;
			fixed _AlphaScale;

			v2f vert(appdata v) 
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				o.worldVertex = mul(unity_ObjectToWorld, v.vertex).xyz;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed3 worldN = normalize(i.worldNormal);
				fixed3 worldL = normalize(UnityWorldSpaceLightDir(i.worldVertex));

				fixed4 texcor = tex2D(_MainTex, i.uv);

				fixed3 albedo = texcor.rgb * _Tint.rgb;

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

				fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldN, worldL));

				return fixed4(ambient + diffuse, texcor.a * _AlphaScale);
			}
			ENDCG
		}
	}
	FallBack "Transparent/VertexLit"
}
