Shader "Custom/UI-DefaultGlass"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

		_Size ("Size", Range(0, 20)) = 1
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]


	    // Horizontal blur  
	    GrabPass 
		{                      
	        Tags { "LightMode" = "Always" }  
	    }  

	    Pass 
		{  
	        Tags { "LightMode" = "Always" }  
	
	        CGPROGRAM  
	        #pragma vertex vert  
	        #pragma fragment frag  
	        #pragma fragmentoption ARB_precision_hint_fastest  
	        #include "UnityCG.cginc"  
			#include "UnityUI.cginc"

	        struct appdata_t {  
	            float4 vertex : POSITION;  
	            float2 texcoord: TEXCOORD0;  
	        };  
	
	        struct v2f {  
	            float4 vertex : POSITION;  
	            float4 uvgrab : TEXCOORD0;  
				half2 texcoord  : TEXCOORD1;  
				float4 worldPosition : TEXCOORD2;
	        };  
	
	        sampler2D _GrabTexture;  
	        float4 _GrabTexture_TexelSize;  
	        float _Size;  

			half4 grabPixelX(fixed weight, fixed kernelx, v2f i)
			{
				return tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x * kernelx*_Size, i.uvgrab.y, i.uvgrab.z, i.uvgrab.w))) * weight;
			}
	
	        v2f vert (appdata_t v)
			{  
	            v2f o;  
				o.worldPosition = v.vertex;
				o.vertex = UnityObjectToClipPos(o.worldPosition);
				o.texcoord = v.texcoord;

	            #if UNITY_UV_STARTS_AT_TOP  
	            float scale = -1.0;  
	            #else  
	            float scale = 1.0;  
	            #endif  
	            o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;  
	            o.uvgrab.zw = o.vertex.zw;  
	            return o;  
	        }  
		
			sampler2D _MainTex;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;

	        half4 frag( v2f i ) : COLOR 
			{  
				half4 color = (tex2D(_MainTex, i.texcoord) + _TextureSampleAdd);
				
				color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);

				clip (color.a - 0.001);

	            half4 sum = half4(0,0,0,0);   
	            sum += grabPixelX(0.05, -4.0, i);  
	            sum += grabPixelX(0.09, -3.0, i);  
	            sum += grabPixelX(0.12, -2.0, i);  
	            sum += grabPixelX(0.15, -1.0, i);  
	            sum += grabPixelX(0.18,  0.0, i);  
	            sum += grabPixelX(0.15, +1.0, i);  
	            sum += grabPixelX(0.12, +2.0, i);  
	            sum += grabPixelX(0.09, +3.0, i);  
	            sum += grabPixelX(0.05, +4.0, i);  
	
	            return sum;  
	        }  
	
	        ENDCG  
	    }  

	    // Vertical blur  
	    GrabPass
		{                          
	        Tags { "LightMode" = "Always" }  
	    }  
	    Pass 
		{  
	        Tags { "LightMode" = "Always" }  
	
	        CGPROGRAM  
	        #pragma vertex vert  
	        #pragma fragment frag  
	        #pragma fragmentoption ARB_precision_hint_fastest  
	        #include "UnityCG.cginc"  
			#include "UnityUI.cginc"

	        struct appdata_t {  
	            float4 vertex : POSITION;  
	            float2 texcoord: TEXCOORD0;  
	        };  
	
	        struct v2f {  
	            float4 vertex : POSITION;  
	            float4 uvgrab : TEXCOORD0;
				half2 texcoord  : TEXCOORD1;  
				float4 worldPosition : TEXCOORD2;
	        };  
	
	
	        sampler2D _GrabTexture;  
	        float4 _GrabTexture_TexelSize;  
	        float _Size;  
	
			half4 grabPixelY(fixed weight, fixed kernely, v2f i)
			{
				return tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x, i.uvgrab.y + _GrabTexture_TexelSize.y * kernely*_Size, i.uvgrab.z, i.uvgrab.w))) * weight;
			}
	
	        v2f vert (appdata_t v)
			{  
	            v2f o;  
				o.worldPosition = v.vertex;
				o.vertex = UnityObjectToClipPos(o.worldPosition);
				o.texcoord = v.texcoord;
	            #if UNITY_UV_STARTS_AT_TOP  
	            float scale = -1.0;  
	            #else  
	            float scale = 1.0;  
	            #endif  
	            o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;  
	            o.uvgrab.zw = o.vertex.zw;  
	            return o;  
	        }  
	
			sampler2D _MainTex;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;

	        half4 frag( v2f i ) : COLOR
			{  
				half4 color = (tex2D(_MainTex, i.texcoord) + _TextureSampleAdd);
				
				color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);

				clip (color.a - 0.001);

	            half4 sum = half4(0,0,0,0);  
	            sum += grabPixelY(0.05, -4.0, i);  
	            sum += grabPixelY(0.09, -3.0, i);  
	            sum += grabPixelY(0.12, -2.0, i);  
	            sum += grabPixelY(0.15, -1.0, i);  
	            sum += grabPixelY(0.18,  0.0, i);  
	            sum += grabPixelY(0.15, +1.0, i);  
	            sum += grabPixelY(0.12, +2.0, i);  
	            sum += grabPixelY(0.09, +3.0, i);  
	            sum += grabPixelY(0.05, +4.0, i);  
	
	            return sum;  
	        }  
	        ENDCG  
	    }  

		//Õý³£
		Pass
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};
			
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0) * float2(-1,1) * OUT.vertex.w;
				#endif
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
}
