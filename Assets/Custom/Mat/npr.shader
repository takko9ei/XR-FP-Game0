Shader "Custom/npr"
{
    Properties
    {
        //need: two tex, first for color ramping, second for styleize
        //      two ranges, first for control edge hardness, second for control which color do  screen tone picks
        //      one color, controlling outline color
        //      another color, control edge highlight color
        //      one float, control highlight range and hard
        //      color ramp map has 2 steps, dark side has gradual change
        //      hightlight using blinn phong, because we do not need ggx shape
        //      one float control its range, one color controls its color

        _MainTex ("Texture", 2D) = "white" {}

        //map
        _RampMap("Ramp Map",2D)="black"{}
        _ToneMap("Tone Map",2D)="black"{}

        _SpecularCol("Specular Color",Color) = (0,0,0,0)
        _OutlineCol("Outline Color",Color)=(0,0,0,0)

        _StepControl0("Control Tone Range",Range(0,1))=0
        _StepControl1("Control Tone Range Another Side",Range(0,1))=0
        _EdgeSoftness("Edge Softness",Range(0.01,0.99))=0.01
        _AddOnLambert("Lambert Diffuse Brightness",Range(-0.5,0.5))=0
        _ToneSampling("Color of Tone",Range(0.01,0.99))=0
        _HlControl("Hightlight Controlling",Range(0,20))=0
        _HlStep("Step Contol hl",Range(0,1))=0
        _OutlineWidth("Outline Width",Range(0,1))=0
        _Smoothness("Smoothness for contol highlight",Range(0,1))=0

        [KeywordEnum(A,B,C)]WIDTH("Width changing",int)=0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque""RenderPipeline"="UniversalPipeline"}
        LOD 100

        Pass
        {   
            Tags{"LightMode"="UniversalForward"}
            Cull Back
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            //#pragma multi_compile WIDTH_A WIDTH_B WIDTH_C

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half3 normal:NORMAL;
            };

            struct v2f
            {
                float4 uv_scruv : TEXCOORD0;
                half4 scrPos:TEXCOORD1;
                half3 normal:TEXCOORD2;
                half3 worldVertex:TEXCOORD3;
                half3 worldView:TEXCOORD4;
                float4 vertex : SV_POSITION;
            };

            TEXTURE2D(_RampMap);
            SAMPLER(sampler_RampMap);
            TEXTURE2D(_ToneMap);
            SAMPLER(sampler_ToneMap);

            half4 _RampMap_ST;
            half4 _ToneMap_ST;
            half _StepControl0;
            half _StepControl1;
            half _AddOnLambert;
            half _EdgeSoftness;
            half _ToneSampling;
            half _HlControl;
            half _HlStep;
            half4 _SpecularCol;
            half _Smoothness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.worldVertex=TransformObjectToWorld(v.vertex.xyz);
                half2 xy=TRANSFORM_TEX(v.uv, _ToneMap);
                o.uv_scruv.xy = TRANSFORM_TEX(v.uv, _ToneMap);
                o.scrPos = ComputeScreenPos(o.vertex);
                half2 zw=o.scrPos.xy/o.scrPos.w;
                o.uv_scruv.zw=TRANSFORM_TEX(zw, _ToneMap);
                o.normal=mul(v.normal,(float3x3)unity_WorldToObject);
                //half worldVert=TransformObjectToWorldDir(v.vertex.xyz);
                o.worldView=normalize(GetCameraPositionWS()-o.worldVertex.xyz);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                i.worldView=normalize(i.worldView);
                half4 shadowCoord = TransformWorldToShadowCoord(i.worldVertex);

				Light mainLight = GetMainLight(shadowCoord);
                half3 lightDir = normalize(mainLight.direction);
                half3 h=normalize(lightDir+i.worldView);
                half nh=max(dot(i.normal,h),0);
                half specular=pow(nh,_HlControl);


                i.normal = normalize(i.normal);
                //half h=-normalize(lightDir+viewDir);
                half nl=max(saturate(dot(i.normal, lightDir)), 0.01)*mainLight.shadowAttenuation;
                //half nh=max(saturate(dot(i.normal, h)),0.01);
                half diffuse=max(min(saturate(nl+_AddOnLambert),0.99),0.01);
                half2 sampRampPos=half2(diffuse,_EdgeSoftness);
                half2 toneColorPos=half2(_ToneSampling,_EdgeSoftness);
                half4 rampCol=SAMPLE_TEXTURE2D(_RampMap,sampler_RampMap,sampRampPos);
                half4 toneTint=SAMPLE_TEXTURE2D(_RampMap,sampler_RampMap,toneColorPos);//***********************
                //half2 screenUV = i.scrPos.xy/i.scrPos.w;
                half4 tone = SAMPLE_TEXTURE2D(_ToneMap,sampler_ToneMap,i.uv_scruv.zw);
                
                float step0=step(_StepControl0,nl);
                float step1=step(nl,_StepControl1);//use half bug here
                half toneRange=step0*step1;

                tone=toneRange*tone;
                half oneMinusPointAlpha = 1-tone.x;
                tone=tone*toneTint;

                half4 col=rampCol*oneMinusPointAlpha+tone;

                half stepSpecular=step(_HlStep,specular);
                half4 specularColor=stepSpecular*_SpecularCol*_Smoothness;
                //#if defined(WIDTH_A)
                return col+specularColor;
                //#else
                //return col;
                //#endif
            }
            ENDHLSL
        }
        Pass{
            //NAME "OUTLINE"
            Cull Front
            
            HLSLPROGRAM
            #pragma vertex overt
            #pragma fragment ofrag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma multi_compile WIDTH_A WIDTH_B WIDTH_C

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            half _OutlineWidth;
            half4 _OutlineCol;

            v2f overt (appdata v)
            {
                v2f o;
                float width=0;
                #if defined(WIDTH_A)
                    width = 0.1;
                #endif

                #if defined(WIDTH_B)
                    width = 0.01;
                #endif

                #if defined(WIDTH_C)
                    width = 0.001;
                #endif
                o.vertex = TransformObjectToHClip(_OutlineWidth*v.normal*width+v.vertex.xyz);
                return o;
            }

            half4 ofrag (v2f i) : SV_Target
            {
                return _OutlineCol;
            }

            ENDHLSL

        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
