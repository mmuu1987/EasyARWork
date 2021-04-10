Shader "Unlit/VideoShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Alpha("Alpha",2D) = "white"{}
        _AlphaBack("AlphaBack",2D) = "white"{}

        _AlphaRange("AlphaRange",range(0,1))=0.9
        _AlphaBackRange("AlphaRange",range(0,1))=0.05

         
         _ScaleUV("ScaleUV",vector)= (1,1,0,0)
    }
    SubShader
    {
          Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Alpha;
            sampler2D _AlphaBack;

            
            float4 _ScaleUV;

            float _AlphaRange;
            float _AlphaBackRange;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture  
                fixed4 col = tex2D(_MainTex, float2(i.uv.x* _ScaleUV.x + _ScaleUV.z , i.uv.y* _ScaleUV.y + _ScaleUV.w));

               
                fixed4 colAlpha = tex2D(_Alpha, i.uv);
                fixed4 alphaBack = tex2D(_AlphaBack, i.uv);

                 if(alphaBack.a<=_AlphaBackRange)
                 {
                     col = colAlpha;
                 }
                 else
                 {

                 }
                     
                   // col = colAlpha;
                

               

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
