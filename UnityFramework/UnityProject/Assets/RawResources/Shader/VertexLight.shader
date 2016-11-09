// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/VertexLight"
{
	Properties
	{
		_Diffuse("Diffuse", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "LightMode"="ForwardBase" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				fixed3 color : COLOR;
				float4 vertex : SV_POSITION;
			};

			fixed4 _Diffuse;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				fixed3 worldN = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
				fixed3 worldL = normalize(_WorldSpaceLightPos0.xyz);

				fixed3 diffuse = unity_LightColor[0].rgb * _Diffuse * saturate(dot(worldN, worldL));

				o.color = ambient + diffuse;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				return fixed4(i.color,1.0);
			}
			ENDCG
		}
	}
}
