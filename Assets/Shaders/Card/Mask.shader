Shader "Card/Mask"
{
	Properties
	{
		_MainTex ("Mask", 2D) = "white" {}
		_Col ("Colour", Color) = (1, 1, 1, 1)
		
		_Dissolve ("Dissolve Texture", 2D) = "white" {}
		_DissolveScale ("Dissolve Texture Scale", Vector) = (1, 1, 0, 0)
		[HDR] _DissolveColor ("Dissolve Colour", Color) = (1, 1, 1, 1)
		_DissolveAmt ("Dissolve Amount", Range(0, 1)) = 0
		_DissolveThickness ("Dissolve Thickness", Range(0, 1)) = .1
		
		[IntRange] _StencilRef ("Stencil Reference Value", Range(0, 255)) = 0
	}

	SubShader
	{
		Stencil
		{
			Ref [_StencilRef]
			Comp Always
			Pass Replace
		}
		
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
		
		ZWrite Off

		Pass
		{
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            	float4 screen_position : TEXCOORD1;
            };

            sampler2D _MainTex;			
			float4 _Col;
			
            sampler2D _Dissolve;
			float2 _DissolveScale;
			float4 _DissolveColor;
			float _DissolveAmt;
			float _DissolveThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
            	o.screen_position = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

            	clip(col.a - 1.0);
            	
            	float2 screen_uv = i.screen_position.xy / i.screen_position.w;

            	const float aspect = _ScreenParams.x / _ScreenParams.y;
            	screen_uv.x *= aspect;
            	
            	const float dissolve_guide = tex2D(_Dissolve, _DissolveScale * screen_uv).r;
            	
            	const float dis = (_DissolveAmt * 2) - 1;
            	const float clip_value = col.a - (dissolve_guide + dis);

            	clip(clip_value);

            	if (clip_value < _DissolveThickness)
            		return _DissolveColor;
            	
                return col * _Col;
            }
            ENDCG
		}
	}
}