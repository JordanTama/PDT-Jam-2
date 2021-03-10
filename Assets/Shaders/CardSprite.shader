Shader "Sprites/Default"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		
		_Dissolve ("Dissolve Texture", 2D) = "white" {}
		_DissolveScale ("Dissolve Texture Scale", Vector) = (1, 1, 0, 0)
		[HDR] _DissolveColor ("Dissolve Colour", Color) = (1, 1, 1, 1)
		_DissolveAmt ("Dissolve Amount", Range(0, 1)) = 0
		_DissolveThickness ("Dissolve Thickness", Range(0, 1)) = .1
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

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
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
				float2 texcoord	: TEXCOORD0;
				float4 screen_pos : TEXCOORD1;
			};
			
			fixed4 _Color;

			sampler2D _Dissolve;
			float2 _DissolveScale;
			float4 _DissolveColor;
			float _DissolveAmt;
			float _DissolveThickness;


			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				OUT.screen_pos = ComputeScreenPos(OUT.vertex);

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif

				return color;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = SampleSpriteTexture (i.texcoord) * i.color;
				
				float2 screen_uv = i.screen_pos.xy / i.screen_pos.w;

            	const float aspect = _ScreenParams.x / _ScreenParams.y;
            	screen_uv.x *= aspect;
            	
            	const float dissolve_guide = tex2D(_Dissolve, _DissolveScale * screen_uv).r;
            	
            	const float dis = (_DissolveAmt * 2) - 1;
            	const float clip_value = col.a - (dissolve_guide + dis);

            	clip(clip_value);

				if (clip_value < _DissolveThickness)
            		return _DissolveColor * col.a;
				
				col.rgb *= col.a;
				return col;
			}
		ENDCG
		}
	}
}