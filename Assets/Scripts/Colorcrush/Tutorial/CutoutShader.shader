Shader "UI/Transparent Hole"
{
    Properties
    {
        _OverlayColor ("Overlay Color", Color) = (0,0,0,0.6)
        _HoleCenter ("Hole Center", Vector) = (0.5,0.5,0,0)
        _HoleSize ("Hole Size", Vector) = (0.2,0.2,0,0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "CanvasOverlay"="True" "PreviewType"="Plane" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off ZWrite Off ZTest Always

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _OverlayColor;
            float2 _HoleCenter;
            float2 _HoleSize;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 diff = abs(i.uv - _HoleCenter);

                if (diff.x < _HoleSize.x * 0.5 && diff.y < _HoleSize.y * 0.5)
                    return fixed4(0,0,0,0); // hole

                return _OverlayColor; // overlay
            }
            ENDHLSL
        }
    }
}
