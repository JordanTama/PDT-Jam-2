Shader "Card/Mask"
{
	Properties
	{
		_MainTex ("Mask", 2D) = "white" {}
		_Col ("Colour", Color) = (1, 1, 1, 1)
		[IntRange] _StencilRef ("Stencil Reference Value", Range(0, 255)) = 0
		_DissolveAmt ("Dissolve Amount", Range(0, 1)) = 0
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
            };

            sampler2D _MainTex;
			float4 _Col;

			float _DissolveAmt;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
            	clip(col.a - 1 - (i.uv.y < _DissolveAmt ? 1 : 0));
                return col * _Col;
            }
            ENDCG
		}
	}
}