Shader "Card/Screen"
{
    Properties
    {
        _Noise ("Noise Texture", 2D) = "white" {}
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

                const float4 distortion_pos = float4(get_distortion(i.uv) - 0.5, 0.0, 1.0);
                const float4 clip_space_distortion = UnityObjectToClipPos(distortion_pos);
                float4 grab_distortion = ComputeGrabScreenPos(clip_space_distortion);
                grab_distortion.xy /= grab_distortion.w;

                const float2 grab_uv = i.grab_pos.xy / i.grab_pos.w;
                const float2 uv = lerp(grab_uv, grab_distortion, _Intensity);
                
                return tex2D(_GrabPassTransparent, uv);
                
                float4 distortionPos = float4(float2(0.5, 0.5) - 0.5, 0.0, 1.0);
                float4 clipSpaceDistortion = UnityObjectToClipPos(distortionPos);
                float4 grabDistortion = ComputeGrabScreenPos(clipSpaceDistortion);
                grabDistortion.xy /= grabDistortion.w;
 
                float2 grabUV = i.grab_pos.xy / i.grab_pos.w;
                grabUV = lerp(grabUV, grabDistortion, _Intensity);
 
                half4 col = tex2D(_GrabPassTransparent, grabUV);
                return col;
                
                // float4 distortion_pos = float4(get_distortion(i.uv) - 0.5, 0.0, 1.0);
                // float4 clip_space_distortion = UnityObjectToClipPos(distortion_pos);
                // float4 grab_distortion = ComputeGrabScreenPos(clip_space_distortion);
                // grab_distortion.xy /= grab_distortion.w;
                //
                // float2 grab_uv = i.grab_pos.xy / i.grab_pos.w;
                // grab_uv = lerp(grab_uv, grab_distortion, _Intensity);
                //
                // float4 col = tex2D(_GrabPassTransparent, grab_uv);
                // return float4(col.rgb, 0);
            }
            ENDCG
        }
    }
}
