Shader "Card/Screen"
{
    Properties
    {
        _Noise ("Noise Texture", 2D) = "white" {}
        _Mask ("Mask Texture", 2D) = "white" {}
		_Intensity ("Glitch Intensity", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        
        Pass
        {
            Blend Zero One
            
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
                float2 noise_uv : TEXCOORD1;
                float4 screen_pos : TEXCOORD2;
            };

            sampler2D _Noise;
            float4 _Noise_ST;

            sampler2D _Mask;
            uniform sampler2D _GrabPassTransparent;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.noise_uv = TRANSFORM_TEX(v.uv, _Noise);
                o.screen_pos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                clip(tex2D(_Mask, i.uv) - 1);
                
                fixed4 col = tex2D(_GrabPassTransparent, i.screen_pos / i.screen_pos.w);
                return float4(col.rgb, 0);
            }
            ENDCG
        }
    }
}
