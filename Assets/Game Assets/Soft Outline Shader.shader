Shader "Custom/SoftOutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range (0.0, 10)) = 0.01
    }
    
    SubShader
    {
        Tags {"Queue"="Overlay" "RenderType"="Opaque"}
        LOD 100
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR0;
            };
            
            float _OutlineWidth;
            fixed4 _MainColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _MainColor;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                return i.color; // Return the main color
            }
            ENDCG
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
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
            };
            
            float _OutlineWidth;
            fixed4 _OutlineColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                float4 offset = float4(v.vertex.xyz, 1) * _OutlineWidth;
                o.pos = UnityObjectToClipPos(v.vertex + offset);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor; // Return the outline color
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}
