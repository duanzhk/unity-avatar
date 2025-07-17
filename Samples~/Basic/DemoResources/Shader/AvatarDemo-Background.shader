Shader "AvatarDemo/Background"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        [HDR] _UpColor ("Up Color", Color) = (1,1,1,1)
        [HDR] _CenterColor ("Center Color", Color) = (1,1,1,1)
        [HDR] _BottomColor ("Bottom Color", Color) = (1,1,1,1)
        _Ctrl ("x:Power y:Repeat z:Speed w:Scale", Vector) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
        Cull Front

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 xy : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _UpColor;
            float4 _CenterColor;
            float4 _BottomColor;
            float4 _Ctrl;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.xy = v.vertex.xy;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 color;
                
                i.xy.y += _Ctrl.w * (sin(_Ctrl.y * i.xy.x + _Ctrl.z * _Time.x));
                if(i.xy.y > 0)
                {
                    color = lerp(_CenterColor, _UpColor, pow(i.xy.y, _Ctrl.x) + (0.5 + 0.5 * _SinTime.x) * 0.1f);
                }
                else
                {
                    color = lerp(_CenterColor, _BottomColor, pow(-i.xy.y, _Ctrl.x) + (0.5 + 0.5 * _SinTime.x) * 0.1f);
                }
                
                fixed4 col = tex2D(_MainTex, i.uv) * color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
