// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "kef/Water"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoizeTex ("Noize", 2D) = "black" {}
		_SpeedX("Speed X", Float) = 5
		_SpeedY("Speed Y", Float) = 5
		_Power("Distortion Power", Range(0, 1)) = 0.1
		_Opacity("Opacity", Range(0, 1)) = 1
		[Toggle(CLIP)] _Clip("Clip negalive uv values", Float) = 0
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma shader_feature CLIP

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 vertex : SV_POSITION;
				UNITY_FOG_COORDS(2)
			};

			sampler2D _MainTex;
			sampler2D _NoizeTex;
			float4 _MainTex_ST;
			float4 _NoizeTex_ST;
			float _SpeedX;
			float _SpeedY;
			float _Power;
			half _Opacity;
			half _Clip;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = v.uv;
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				half speedX = _Time.x *_SpeedX;
				half speedY = _Time.x *_SpeedY;
				fixed2 noizeUv = TRANSFORM_TEX(i.uv1, _NoizeTex);
				fixed4 noize = tex2D(_NoizeTex, half2(noizeUv.x + speedX, noizeUv.y + speedY));
				// make offset value change from -0.5 to 0.5
				half offset = noize.r - 0.5;
				// dont offset uv close to uv edges
				offset = offset * _Power;
				half2 uv = half2(i.uv.x + offset, i.uv.y + offset);
#ifdef CLIP
				clip((1 - (pow((uv.x * 2 - 1), 2))) * (1 - (pow((uv.y * 2 - 1), 2))));
#endif
				fixed4 col = tex2D(_MainTex, uv);
				col *= (col.a * _Opacity);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
