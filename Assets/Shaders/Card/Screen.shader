Shader "Card/Screen"
{
    Properties
    {
        _Noise ("Noise Texture", 2D) = "white" {}
        _NoiseThreshold ("Noise Threshold", Range(0, 1)) = .5
        _NoiseScroll ("Noise Scroll Speed", Float) = 1
        _NoiseExponent ("Noise Exponent", Float) = 1
        _NoiseOffset ("Noise Offset", Range(0, 1)) = 0
        
        _Mask ("Mask Texture", 2D) = "white" {}
		_Intensity ("Glitch Intensity", Range(0, 1)) = 1
        
        _SplitDispAmt ("Split Displacement Amount", Float) = 0.1
        _SplitFreq ("Split Frequency", Float) = 0.1
        
        _WiggleDispAmt ("Wiggle Displacement Amount", Float) = 0.1
        _WiggleFreq ("Wiggle Frequency", Float) = 0.1
        _WiggleScroll ("Wiggle Scroll Speed", Float) = 10
        
        [IntRange] _StencilRef ("Stencil Reference Value", Range(0, 255)) = 0
    }
    SubShader
    {
        Stencil
        {
            Ref [_StencilRef]
            Comp Equal
            Pass Keep
        }
        
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        
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
                float4 grab_pos : TEXCOORD1;
            };


            sampler2D _Noise;
            float _NoiseThreshold;
            float _NoiseScroll;
            float _NoiseExponent;
            float _NoiseOffset;
            
            sampler2D _Mask;
            sampler2D _GrabPassTransparent;

            float _Intensity;
            
            float _SplitDispAmt;
            float _SplitFreq;

            float _WiggleDispAmt;
            float _WiggleFreq;
            float _WiggleScroll;

            float2 get_distortion(float2 uv)
            {
                float split = sin(uv.y * _SplitFreq);
                split = (split + 1) * 0.5;
                split = round(split);
                split = split * 2 - 1;

                float wiggle = sin(uv.y * _WiggleFreq + _Time[1] * _WiggleScroll);
                
                return float2(uv.x + split * _SplitDispAmt + wiggle * _WiggleDispAmt, uv.y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.grab_pos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                clip(tex2D(_Mask, i.uv) - 1);

                float glitch = tex2D(_Noise, float2(_Time[0] * _NoiseScroll, _NoiseOffset));
                glitch = (glitch + 1.0) / 2.0;
                glitch = pow(glitch, _NoiseExponent);
                glitch = glitch >= _NoiseThreshold ? 1 : 0;
                

                const float4 distortion_pos = float4(get_distortion(i.uv) - 0.5, 0.0, 1.0);
                const float4 clip_space_distortion = UnityObjectToClipPos(distortion_pos);
                float4 grab_distortion = ComputeGrabScreenPos(clip_space_distortion);
                grab_distortion.xy /= grab_distortion.w;

                const float2 grab_uv = i.grab_pos.xy / i.grab_pos.w;
                const float2 uv = lerp(grab_uv, grab_distortion, glitch * _Intensity);
                
                return tex2D(_GrabPassTransparent, uv);
            }
            ENDCG
        }
    }
}
