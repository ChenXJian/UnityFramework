Shader "Custom/Debug"
{
	SubShader
	{

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma enable_d3d11_debug_symbols

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 color : COLOR0;
				float4 vertex : SV_POSITION;
			};
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				//法线
				o.color = fixed4(v.normal * 0.5 + fixed3(0.5, 0.5, 0.5), 1.0);

				//切线
				//o.color = fixed4(v.tangent.xyz * 0.5 + fixed3(0.5, 0.5, 0.5), 1.0);

				//副切线
				//fixed3 binormal = cross(v.normal, v.tangent.xyz) * v.tangent.w;
				//o.color = fixed4(binormal * 0.5 + fixed3(0.5, 0.5, 0.5), 1.0);
				
				//纹理坐标
				//o.color = fixed4(v.texcoord.xy, 0.0, 1.0);

				//纹理坐标小数部分
				//o.color = frac(v.texcoord);
				//if (any(saturate(v.texcoord) - v.texcoord))
				//{
				//	o.color.b = 0.5;
				//}
				//o.color.a = 1.0;

				//顶点色
				//o.color = v.color;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}
	}
}
