// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MultyFuncMask/3D/Mask" {
	Properties {
		_MainTexKoef("Screen koef", Vector) = (0, 0, 1, 1)
		_Color("Color", Color) = (1,1,1,1)
		_MaskTex("Mask texture", 2D) = "gray" {}
		_BigMaskTex("Offset mask map", 2D) = "white" {}

		_offset("Offset", Float) = 0.05
		_nX("QuialityX", int) = 3
		_nY("QuialityY", int) = 3

		_iterMin("Iteration Min", Float) = 0
		_iterMax("Iteration Max", Float) = 1
		_min("min", Float) = 0
		_max("max", Float) = 1
		_offsetNormal("Offset normal", Float) = 0.15
	}
	SubShader {
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Lighting Off Cull Off ZWrite Off Fog{ Mode Off }
			GrabPass{}
			//Blend SrcAlpha OneMinusSrcAlpha
		Pass{
			//Cull off
			ZWrite On
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma vertex vert
			#pragma fragment frag
			//#pragma target 4.0
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 projPos : TEXCOORD1;
			};

			uniform float4 _MainTexKoef;
			float4 _Color;
			sampler2D _MainTex;
			sampler2D _GrabTexture;

			uniform sampler2D _MaskTex;
			uniform sampler2D _BigMaskTex;
			uniform float4   _CameraDepthTexture_ST;
			uniform sampler2D _CameraDepthTexture;

			float _offset;
			uniform int _nX;
			uniform int _nY;
			float _iterMin;
			float _iterMax;
			float _min;
			float _max;
			float _offsetNormal;
			
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = _Color;
				OUT.projPos = ComputeScreenPos(UnityObjectToClipPos(IN.vertex - IN.normal * _offsetNormal));
				OUT.projPos.xy = OUT.projPos.xy * _MainTexKoef.zw + _MainTexKoef.xy;
				//ComputeScreenPos(OUT.vertex - IN.normal * _offsetNormal);
				return OUT;
			}

			fixed4 frag(v2f input) : SV_Target
			{
				half alpha = /*tex2D(_MainTex, input.texcoord).a **/ input.color.a;

				input.projPos /= input.projPos.w;
				input.projPos.xy -= floor(input.projPos.xy);

				half yCoord = input.projPos.y;
				/*#ifdef SHADER_API_D3D11
					yCoord = 1 - input.projPos.y;
				#endif*/

				half3 grabCol = tex2D(_GrabTexture, float2(input.projPos.x, yCoord)).rgb;

				half3 c = half3(0, 0, 0);//grabCol * 2;
				fixed sum = 0;
				fixed bKoef = tex2D(_BigMaskTex, fixed2(input.texcoord.x, 1 - input.texcoord.y)).r * tex2D(_BigMaskTex, fixed2(input.texcoord.x, 1 - input.texcoord.y)).a;
				fixed off = bKoef * _offset / max(_nX, _nY);
				fixed widthValue = _iterMax - _iterMin;
				fixed halfNX = _nX / 2;
				fixed halfNY = _nY / 2;
				fixed2 tempCoord = float2(input.projPos.x, yCoord) - fixed2(off * halfNX, off * halfNY);
				
				for (int x = 0; x < _nX; x++)
				{
					fixed minCoordX = (x + 0.5) / _nX;
					fixed coordX = tempCoord.x + off * x;
					for (int y = 0; y < _nY; y++)
					{
						fixed4 value = tex2D(_MaskTex, fixed2(minCoordX, (y + 0.5) / _nY));//_maskArr[x][y];//tex2D(_MaskTex, fixed2(minCoordX, (y + 0.5) / _nY));
						sum += value;
						value = widthValue * value + _iterMin;
						c += value *  tex2D(_GrabTexture, fixed2(coordX, tempCoord.y + off * y))* input.color;

					}
				}
				c /= sum;
				c = (_max - _min) * c + _min;
				c = c.rgb * alpha + grabCol * (1 - alpha);
				return half4(c, alpha);// ret;
			}

		    

			ENDCG
		}
	}
	FallBack "Diffuse"
}
