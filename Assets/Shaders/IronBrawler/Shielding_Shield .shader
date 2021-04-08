// Toony Colors Pro+Mobile 2
// (c) 2014-2020 Jean Moreno

Shader "Shielding_Shield "
{
	Properties
	{
		[Enum(Front, 2, Back, 1, Both, 0)] _Cull ("Render Face", Float) = 2.0
		[TCP2ToggleNoKeyword] _ZWrite ("Depth Write", Float) = 1.0
		[HideInInspector] _RenderingMode ("rendering mode", Float) = 0.0
		[HideInInspector] _SrcBlend ("blending source", Float) = 1.0
		[HideInInspector] _DstBlend ("blending destination", Float) = 0.0
		[TCP2Separator]

		[TCP2HeaderHelp(Base)]
		[HDR] _BaseColor ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		_BaseMap ("Albedo", 2D) = "white" {}
		[TCP2Vector4FloatsDrawer(Speed,Amplitude,Frequency,Offset)] _BaseMap_SinAnimParams ("Albedo UV Sine Distortion Parameters", Float) = (1, 0.05, 1, 0)
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[TCP2Separator]
		
		[TCP2HeaderHelp(Dissolve)]
		_DissolveMap ("Map", 2D) = "gray" {}
		[TCP2UVScrolling] _DissolveMap_SC ("Map UV Scrolling", Vector) = (1,1,0,0)
		[HDR] _DissolveGradientTexture ("Gradient Texture", Color) = (1,1,1,1)
		_DissolveGradientWidth ("Ramp Width", Range(0,1)) = 0.2
		[TCP2Separator]
		
		[TCP2Vector4Floats(Contrast X,Contrast Y,Contrast Z,Smoothing,1,16,1,16,1,16,0.05,10)] _TriplanarSamplingStrength ("Triplanar Sampling Parameters", Vector) = (8,8,8,0.5)
		// Custom Material Properties
		 _DissolveValue1 ("Dissolve Value", Range(0,1)) = 0.5

		[ToggleOff(_RECEIVE_SHADOWS_OFF)] _ReceiveShadowsOff ("Receive Shadows", Float) = 1

		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType"="Opaque"
			"Queue"="AlphaTest"
		}

		HLSLINCLUDE
		#define fixed half
		#define fixed2 half2
		#define fixed3 half3
		#define fixed4 half4

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

		// Uniforms

		// Custom Material Properties
		
		// Shader Properties
		sampler2D _BaseMap;
		sampler2D _DissolveMap;

		CBUFFER_START(UnityPerMaterial)
			
			// Custom Material Properties
			float _DissolveValue1;

			// Shader Properties
			float4 _BaseMap_ST;
			half4 _BaseMap_SinAnimParams;
			float4 _DissolveMap_ST;
			half4 _DissolveMap_SC;
			float _DissolveGradientWidth;
			half4 _DissolveGradientTexture;
			half4 _BaseColor;
			float _RampThreshold;
			float _RampSmoothing;
			fixed4 _SColor;
			fixed4 _HColor;
			float4 _TriplanarSamplingStrength;
		CBUFFER_END
		
		float4 tex2D_triplanar(sampler2D samp, float4 tiling_offset, float3 worldPos, float3 worldNormal)
		{
			half4 sample_y = ( tex2D(samp, worldPos.xz * tiling_offset.xy + tiling_offset.zw).rgba );
			fixed4 sample_x = ( tex2D(samp, worldPos.zy * tiling_offset.xy + tiling_offset.zw).rgba );
			fixed4 sample_z = ( tex2D(samp, worldPos.xy * tiling_offset.xy + tiling_offset.zw).rgba );
			
			//blending
			half3 blendWeights = pow(abs(worldNormal), _TriplanarSamplingStrength.xyz / _TriplanarSamplingStrength.w);
			blendWeights = blendWeights / (blendWeights.x + abs(blendWeights.y) + blendWeights.z);
			half4 triplanar = sample_x * blendWeights.x + sample_y * blendWeights.y + sample_z * blendWeights.z;
			
			return triplanar;
		}
			
		// Built-in renderer (CG) to SRP (HLSL) bindings
		#define UnityObjectToClipPos TransformObjectToHClip
		#define _WorldSpaceLightPos0 _MainLightPosition
		
		ENDHLSL

		Pass
		{
			Name "Main"
			Tags
			{
				"LightMode"="UniversalForward"
			}
		Blend [_SrcBlend] [_DstBlend]
		Cull [_Cull]
		ZWrite [_ZWrite]

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard SRP library
			// All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 3.0

			// -------------------------------------
			// Material keywords
			//#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ _RECEIVE_SHADOWS_OFF

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

			// -------------------------------------

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex Vertex
			#pragma fragment Fragment

			//--------------------------------------
			// Toony Colors Pro 2 keywords
		#pragma shader_feature _ _ALPHAPREMULTIPLY_ON

			// vertex input
			struct Attributes
			{
				float4 vertex       : POSITION;
				float3 normal       : NORMAL;
				float4 tangent      : TANGENT;
				float4 texcoord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			// vertex output / fragment input
			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
				float3 normal         : NORMAL;
				float4 worldPosAndFog : TEXCOORD0;
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord    : TEXCOORD1; // compute shadow coord per-vertex for the main light
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				half3 vertexLights : TEXCOORD2;
			#endif
				float3 pack0 : TEXCOORD3; /* pack0.xyz = objPos */
				float3 pack1 : TEXCOORD4; /* pack1.xyz = objNormal */
				float4 pack2 : TEXCOORD5; /* pack2.xy = texcoord0  pack2.zw = sinUvAnimVertexPos */
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			Varyings Vertex(Attributes input)
			{
				Varyings output = (Varyings)0;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				// Used for texture UV sine animation
				float2 sinUvAnimVertexPos = input.vertex.xy + input.vertex.yz;
				output.pack2.zw = sinUvAnimVertexPos;

				// Texture Coordinates
				output.pack2.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;

				output.pack0.xyz = input.vertex.xyz;
				output.pack1.xyz = input.normal.xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				output.shadowCoord = GetShadowCoord(vertexInput);
			#endif

				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normal);
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				// Vertex lighting
				output.vertexLights = VertexLighting(vertexInput.positionWS, vertexNormalInput.normalWS);
			#endif

				// world position
				output.worldPosAndFog = float4(vertexInput.positionWS.xyz, 0);

				// normal
				output.normal = NormalizeNormalPerVertex(vertexNormalInput.normalWS);

				// clip position
				output.positionCS = vertexInput.positionCS;

				return output;
			}

			half4 Fragment(Varyings input) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

				float3 positionWS = input.worldPosAndFog.xyz;
				float3 normalWS = NormalizeNormalPerPixel(input.normal);

				float2 uvSinAnim__BaseMap = (input.pack2.zw * _BaseMap_SinAnimParams.z) + (_Time.yy * _BaseMap_SinAnimParams.x);
								// Shader Properties Sampling
				float4 __albedo = ( tex2D(_BaseMap, input.pack2.xy.xy + (((sin(0.9 * uvSinAnim__BaseMap + _BaseMap_SinAnimParams.w) + sin(1.33 * uvSinAnim__BaseMap + 3.14 * _BaseMap_SinAnimParams.w) + sin(2.4 * uvSinAnim__BaseMap + 5.3 * _BaseMap_SinAnimParams.w)) / 3) * _BaseMap_SinAnimParams.y)).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );
				float __dissolveMap = ( tex2D_triplanar(_DissolveMap, float4(_DissolveMap_ST.xy, _DissolveMap_ST.zw + frac(_Time.yy * _DissolveMap_SC.xy)), input.pack0.xyz, input.pack1.xyz) );
				float __dissolveValue = ( _DissolveValue1.x );
				float __dissolveGradientWidth = ( _DissolveGradientWidth );
				float __dissolveGradientStrength = ( 2.0 );
				float __ambientIntensity = ( 1.0 );
				float __rampThreshold = ( _RampThreshold );
				float __rampSmoothing = ( _RampSmoothing );
				float3 __shadowColor = ( _SColor.rgb );
				float3 __highlightColor = ( _HColor.rgb );

				// main texture
				half3 albedo = __albedo.rgb;
				half alpha = __alpha;
				half3 emission = half3(0,0,0);
				
				//Dissolve
				half dissolveMap = __dissolveMap;
				half dissolveValue = __dissolveValue;
				half gradientWidth = __dissolveGradientWidth;
				float dissValue = dissolveValue*(1+2*gradientWidth) - gradientWidth;
				float dissolveUV = smoothstep(dissolveMap - gradientWidth, dissolveMap + gradientWidth, dissValue);
				clip((1-dissolveUV) - 0.001);
				half4 dissolveColor = ( _DissolveGradientTexture.rgba );
				dissolveColor *= __dissolveGradientStrength * dissolveUV;
				emission += dissolveColor.rgb;
				
				albedo *= __mainColor.rgb;

				// main light: direction, color, distanceAttenuation, shadowAttenuation
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord = input.shadowCoord;
			#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
				float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
			#else
				float4 shadowCoord = float4(0, 0, 0, 0);
			#endif
				Light mainLight = GetMainLight(shadowCoord);

				// ambient or lightmap
				// Samples SH fully per-pixel. SampleSHVertex and SampleSHPixel functions
				// are also defined in case you want to sample some terms per-vertex.
				half3 bakedGI = SampleSH(normalWS);
				half occlusion = 1;
				half3 indirectDiffuse = bakedGI;
				indirectDiffuse *= occlusion * albedo * __ambientIntensity;

				half3 lightDir = mainLight.direction;
				half3 lightColor = mainLight.color.rgb;

				half atten = mainLight.shadowAttenuation * mainLight.distanceAttenuation;

				half ndl = dot(normalWS, lightDir);
				half3 ramp;
				
				half rampThreshold = __rampThreshold;
				half rampSmooth = __rampSmoothing * 0.5;
				ndl = saturate(ndl);
				ramp = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);

				// apply attenuation
				ramp *= atten;

				half3 color = half3(0,0,0);
				half3 accumulatedRamp = ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
				half3 accumulatedColors = ramp * lightColor.rgb;

				// Additional lights loop
			#ifdef _ADDITIONAL_LIGHTS
				uint additionalLightsCount = GetAdditionalLightsCount();
				for (uint lightIndex = 0u; lightIndex < additionalLightsCount; ++lightIndex)
				{
					Light light = GetAdditionalLight(lightIndex, positionWS);
					half atten = light.shadowAttenuation * light.distanceAttenuation;
					half3 lightDir = light.direction;
					half3 lightColor = light.color.rgb;

					half ndl = dot(normalWS, lightDir);
					half3 ramp;
					
					ndl = saturate(ndl);
					ramp = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);

					// apply attenuation (shadowmaps & point/spot lights attenuation)
					ramp *= atten;

					accumulatedRamp += ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
					accumulatedColors += ramp * lightColor.rgb;

				}
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				color += input.vertexLights * albedo;
			#endif

				accumulatedRamp = saturate(accumulatedRamp);
				half3 shadowColor = (1 - accumulatedRamp.rgb) * __shadowColor;
				accumulatedRamp = accumulatedColors.rgb * __highlightColor + shadowColor;
				color += albedo * accumulatedRamp;

				// apply ambient
				color += indirectDiffuse;

				// Premultiply blending
				#if defined(_ALPHAPREMULTIPLY_ON)
					color.rgb *= alpha;
				#endif

				color += emission;

				return half4(color, alpha);
			}
			ENDHLSL
		}

		// Depth & Shadow Caster Passes
		HLSLINCLUDE
		#if defined(SHADOW_CASTER_PASS) || defined(DEPTH_ONLY_PASS)

			#define fixed half
			#define fixed2 half2
			#define fixed3 half3
			#define fixed4 half4

			float3 _LightDirection;

			struct Attributes
			{
				float4 vertex   : POSITION;
				float3 normal   : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
				float3 pack0 : TEXCOORD0; /* pack0.xyz = objPos */
				float3 pack1 : TEXCOORD1; /* pack1.xyz = objNormal */
				float4 pack2 : TEXCOORD2; /* pack2.xy = texcoord0  pack2.zw = sinUvAnimVertexPos */
			#if defined(DEPTH_ONLY_PASS)
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			#endif
			};

			float4 GetShadowPositionHClip(Attributes input)
			{
				float3 positionWS = TransformObjectToWorld(input.vertex.xyz);
				float3 normalWS = TransformObjectToWorldNormal(input.normal);

				float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));

			#if UNITY_REVERSED_Z
				positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
			#else
				positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
			#endif

				return positionCS;
			}

			Varyings ShadowDepthPassVertex(Attributes input)
			{
				Varyings output;
				UNITY_SETUP_INSTANCE_ID(input);
				#if defined(DEPTH_ONLY_PASS)
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
				#endif

				// Used for texture UV sine animation
				float2 sinUvAnimVertexPos = input.vertex.xy + input.vertex.yz;
				output.pack2.zw = sinUvAnimVertexPos;

				// Texture Coordinates
				output.pack2.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;

				output.pack0.xyz = input.vertex.xyz;
				output.pack1.xyz = input.normal.xyz;

				#if defined(DEPTH_ONLY_PASS)
					output.positionCS = TransformObjectToHClip(input.vertex.xyz);
				#elif defined(SHADOW_CASTER_PASS)
					output.positionCS = GetShadowPositionHClip(input);
				#else
					output.positionCS = float4(0,0,0,0);
				#endif
				
				return output;
			}

			half4 ShadowDepthPassFragment(Varyings input) : SV_TARGET
			{
				#if defined(DEPTH_ONLY_PASS)
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
				#endif

				float2 uvSinAnim__BaseMap = (input.pack2.zw * _BaseMap_SinAnimParams.z) + (_Time.yy * _BaseMap_SinAnimParams.x);
								// Shader Properties Sampling
				float4 __albedo = ( tex2D(_BaseMap, input.pack2.xy.xy + (((sin(0.9 * uvSinAnim__BaseMap + _BaseMap_SinAnimParams.w) + sin(1.33 * uvSinAnim__BaseMap + 3.14 * _BaseMap_SinAnimParams.w) + sin(2.4 * uvSinAnim__BaseMap + 5.3 * _BaseMap_SinAnimParams.w)) / 3) * _BaseMap_SinAnimParams.y)).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );
				float __dissolveMap = ( tex2D_triplanar(_DissolveMap, float4(_DissolveMap_ST.xy, _DissolveMap_ST.zw + frac(_Time.yy * _DissolveMap_SC.xy)), input.pack0.xyz, input.pack1.xyz) );
				float __dissolveValue = ( _DissolveValue1.x );
				float __dissolveGradientWidth = ( _DissolveGradientWidth );
				float __dissolveGradientStrength = ( 2.0 );

				half3 albedo = __albedo.rgb;
				half alpha = __alpha;
				half3 emission = half3(0,0,0);
				
				//Dissolve
				half dissolveMap = __dissolveMap;
				half dissolveValue = __dissolveValue;
				half gradientWidth = __dissolveGradientWidth;
				float dissValue = dissolveValue*(1+2*gradientWidth) - gradientWidth;
				float dissolveUV = smoothstep(dissolveMap - gradientWidth, dissolveMap + gradientWidth, dissValue);
				clip((1-dissolveUV) - 0.001);
				half4 dissolveColor = ( _DissolveGradientTexture.rgba );
				dissolveColor *= __dissolveGradientStrength * dissolveUV;
				emission += dissolveColor.rgb;

				return 0;
			}

		#endif
		ENDHLSL

		Pass
		{
			Name "ShadowCaster"
			Tags
			{
				"LightMode" = "ShadowCaster"
			}

			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile SHADOW_CASTER_PASS

			// -------------------------------------
			// Material Keywords
			//#pragma shader_feature _ALPHATEST_ON
			//#pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment
			
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

			ENDHLSL
		}

		Pass
		{
			Name "DepthOnly"
			Tags
			{
				"LightMode" = "DepthOnly"
			}

			ZWrite On
			ColorMask 0

			HLSLPROGRAM

			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			// -------------------------------------
			// Material Keywords
			// #pragma shader_feature _ALPHATEST_ON
			// #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile DEPTH_ONLY_PASS

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment
			
			ENDHLSL
		}

	}

	FallBack "Hidden/InternalErrorShader"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(unity:"2020.1.3f1";ver:"2.6.0";tmplt:"SG2_Template_URP";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","OUTLINE_PIXEL_PERFECT","DISSOLVE_CLIP","AUTO_TRANSPARENT_BLENDING","OUTLINE_CONSTANT_SIZE","DISSOLVE","DISSOLVE_GRADIENT","TEMPLATE_LWRP"];flags:list[];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0"];shaderProperties:list[sp(name:"Albedo";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:True;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:True;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:0;cc:4;chan:"RGBA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_BaseMap";md:"";custom:False;refs:"";guid:"2ad3f4db-76f1-4897-9909-e60cef1a1212";op:Multiply;lbl:"Albedo";gpu_inst:False;locked:False;impl_index:0)]),sp(name:"Main Color";imps:list[imp_mp_color(def:RGBA(1.000, 1.000, 1.000, 1.000);hdr:True;cc:4;chan:"RGBA";prop:"_BaseColor";md:"";custom:False;refs:"";guid:"0bc800d6-19cb-417d-9973-c5e4211b355f";op:Multiply;lbl:"Color";gpu_inst:False;locked:False;impl_index:0)]),,,,,,,,sp(name:"Dissolve Map";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:False;scr:True;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;notile:False;triplanar_local:True;def:"gray";locked_uv:False;uv:6;cc:1;chan:"R";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Triplanar;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_DissolveMap";md:"";custom:False;refs:"";guid:"4d880a44-1350-43c6-820a-620f38e46e91";op:Multiply;lbl:"Map";gpu_inst:False;locked:False;impl_index:0)]),sp(name:"Dissolve Value";imps:list[imp_ct(lct:"_DissolveValue1";cc:1;chan:"X";avchan:"X";guid:"8d5627c5-fae0-4556-93b2-e4f110e3228d";op:Multiply;lbl:"Value";gpu_inst:False;locked:False;impl_index:-1)]),sp(name:"Dissolve Gradient Texture";imps:list[imp_mp_color(def:RGBA(1.000, 1.000, 1.000, 1.000);hdr:True;cc:4;chan:"RGBA";prop:"_DissolveGradientTexture";md:"";custom:False;refs:"";guid:"310fa2f9-c25f-4bcc-90f1-d856449f7d6b";op:Multiply;lbl:"Gradient Texture";gpu_inst:False;locked:False;impl_index:-1)])];customTextures:list[ct(cimp:imp_mp_range(def:0.5;min:0;max:1;prop:"_DissolveValue1";md:"";custom:True;refs:"";guid:"a200c61a-3739-4b8e-b3e7-0e20d48c5578";op:Multiply;lbl:"Dissolve Value";gpu_inst:False;locked:False;impl_index:-1);exp:True;uv_exp:False;imp_lbl:"Range")];codeInjection:codeInjection(injectedFiles:list[];mark:False)) */
/* TCP_HASH b0db99b3e2ec3354b29b635c9b0f269f */
