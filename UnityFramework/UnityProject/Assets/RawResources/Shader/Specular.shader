// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Specular"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Tint("Tint", Color) = (1,1,1,1)
		_Specular ("Specular", Color) = (1,1,1,1)
		_Gloss("Gloss", Range(5.0, 100)) = 20
	}
	SubShader
	{

		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldVertex : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};

			fixed4 _Tint;
			fixed4 _Specular;
			float _Gloss;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldVertex = mul(unity_ObjectToWorld, v.vertex).xyz;
				//将贴图的缩放与平移应用至UV
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 worldN = normalize(i.worldNormal);
				fixed3 worldL = normalize(UnityWorldSpaceLightDir(i.worldVertex));
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldVertex));
				fixed3 halfDir = normalize(worldL + viewDir);


				fixed3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint;
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				//diffuse = (light * albedoColor) * max(0, (n dot l))
				fixed3 diffuse = unity_LightColor[0].rgb * albedo * saturate(dot(worldN, worldL));
				//(Blinn-phong)specular  = (light * specularColor) * max(0, (n dot h)) pow gloss
				fixed3 specular = unity_LightColor[0].rgb * _Specular.rgb * pow(saturate(dot(worldN, halfDir)), _Gloss);


				//fixed3 refDir = normalize(reflect(-worldL, worldN));
				//fixed3 specular = unity_LightColor[0].rgb * _Specular.rgb * pow(saturate(dot(refDir, viewDir)), _Gloss);

				fixed3 color = ambient + diffuse + specular;
				return fixed4(color,1.0);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
