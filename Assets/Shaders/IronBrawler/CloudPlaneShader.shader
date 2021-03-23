// Toony Colors Pro+Mobile 2
// (c) 2014-2020 Jean Moreno

Shader "CloudPlaneShader"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		[HDR] _MainColor ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		_Albedo ("Albedo", 2D) = "white" {}
		[TCP2UVScrolling] _Albedo_SC ("Albedo UV Scrolling", Vector) = (1,1,0,0)
		 _Albedo1 ("Albedo Color", Color) = (1,1,1,1)
		[TCP2Separator]

		[Header(Vertex Waves Animation)]
		_WavesSpeed ("Speed", Float) = 2
		_WavesHeight ("Height", Float) = 0.1
		_WavesFrequency ("Frequency", 2D) = "white" {}
		
		[Header(Depth Based Effects)]
		[TCP2ColorNoAlpha] _DepthColor ("Depth Color", Color) = (0,0,1,1)
		[PowerSlider(5.0)] _DepthColorDistance ("Depth Color Distance", Range(0.01,3)) = 0.5
		[PowerSlider(5.0)] _DepthAlphaDistance ("Depth Alpha Distance", Range(0.01,10)) = 0.5
		_DepthAlphaMin ("Depth Alpha Min", Range(0,1)) = 0.5
		
		[TCP2HeaderHelp(Vertical Fog)]
			_VerticalFogThreshold ("Y Threshold", Float) = 0
			_VerticalFogSmoothness ("Smoothness", Float) = 0.5
			_VerticalFogColor ("Fog Color", 2D) = "white" {}
			[TCP2UVScrolling] _VerticalFogColor_SC ("Fog Color UV Scrolling", Vector) = (1,1,0,0)
			 _VerticalFogColor1 ("Vertical Fog Color Color", Color) = (1,1,1,1)
		[TCP2Separator]

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
		}

		HLSLINCLUDE
		#define fixed half
		#define fixed2 half2
		#define fixed3 half3
		#define fixed4 half4

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

		// Uniforms

		// Shader Properties
		sampler2D _WavesFrequency;
		sampler2D _Albedo;
		sampler2D _VerticalFogColor;

		CBUFFER_START(UnityPerMaterial)
			
			// Shader Properties
			float4 _WavesFrequency_ST;
			float _WavesHeight;
			float _WavesSpeed;
			float4 _Albedo_ST;
			half4 _Albedo_SC;
			fixed4 _Albedo1;
			half4 _MainColor;
			fixed4 _DepthColor;
			float _DepthColorDistance;
			float _DepthAlphaDistance;
			float _DepthAlphaMin;
			fixed4 _SColor;
			fixed4 _HColor;
			float _VerticalFogThreshold;
			float _VerticalFogSmoothness;
			float4 _VerticalFogColor_ST;
			half4 _VerticalFogColor_SC;
			fixed4 _VerticalFogColor1;
		CBUFFER_END
		
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
			#pragma multi_compile_fog

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex Vertex
			#pragma fragment Fragment

			// vertex input
			struct Attributes
			{
				float4 vertex       : POSITION;
				float3 normal       : NORMAL;
				float4 tangent      : TANGENT;
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
				float4 screenPosition : TEXCOORD3;
				float pack1 : TEXCOORD4; /* pack1.x = fogFactor */
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			Varyings Vertex(Attributes input)
			{
				Varyings output = (Varyings)0;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				float3 worldPosUv = mul(unity_ObjectToWorld, input.vertex).xyz;

				// Shader Properties Sampling
				float __wavesFrequency = ( tex2Dlod(_WavesFrequency, float4(worldPosUv.xz * _WavesFrequency_ST.xy + _WavesFrequency_ST.zw, 0, 0)).r );
				float __wavesHeight = ( _WavesHeight );
				float __wavesSpeed = ( _WavesSpeed );

				float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
				
				// Vertex water waves
				float _waveFrequency = __wavesFrequency;
				float _waveHeight = __wavesHeight;
				float3 _vertexWavePos = worldPos.xyz * _waveFrequency;
				float _phase = _Time.y * __wavesSpeed;
				float waveFactorX = sin(_vertexWavePos.x + _phase) * _waveHeight;
				float waveFactorZ = sin(_vertexWavePos.z + _phase) * _waveHeight;
				input.vertex.y += (waveFactorX + waveFactorZ);
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				output.shadowCoord = GetShadowCoord(vertexInput);
			#endif
				float4 clipPos = vertexInput.positionCS;

				float4 screenPos = ComputeScreenPos(clipPos);
				output.screenPosition.xyzw = screenPos;

				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normal);
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				// Vertex lighting
				output.vertexLights = VertexLighting(vertexInput.positionWS, vertexNormalInput.normalWS);
			#endif

				// world position
				output.worldPosAndFog = float4(vertexInput.positionWS.xyz, 0);

				// Computes fog factor per-vertex
				output.worldPosAndFog.w = ComputeFogFactor(vertexInput.positionCS.z);

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

				// Shader Properties Sampling
				float4 __albedo = ( tex2D(_Albedo, positionWS.xz * _Albedo_ST.xy + frac(_Time.yy * _Albedo_SC.xy) + _Albedo_ST.zw).rgba * _Albedo1.rgba );
				float4 __mainColor = ( _MainColor.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );
				float3 __depthColor = ( _DepthColor.rgb );
				float __depthColorDistance = ( _DepthColorDistance );
				float __depthAlphaDistance = ( _DepthAlphaDistance );
				float __depthAlphaMin = ( _DepthAlphaMin );
				float __ambientIntensity = ( 1.0 );
				float3 __shadowColor = ( _SColor.rgb );
				float3 __highlightColor = ( _HColor.rgb );
				float __verticalFogThreshold = ( _VerticalFogThreshold );
				float __verticalFogSmoothness = ( _VerticalFogSmoothness );
				float4 __verticalFogColor = ( tex2D(_VerticalFogColor, positionWS.xz * _VerticalFogColor_ST.xy + frac(_Time.yy * _VerticalFogColor_SC.xy) + _VerticalFogColor_ST.zw).rgba * _VerticalFogColor1.rgba );

				// Sample depth texture and calculate difference with local depth
				//float sceneDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, input.[[INPUT_VALUE:screenPosition]].xy / input.[[INPUT_VALUE:screenPosition]].w);
				float sceneDepth = SampleSceneDepth(input.screenPosition.xyzw.xy / input.screenPosition.xyzw.w);
				if (unity_OrthoParams.w > 0.0)
				{
					// Orthographic camera
					#if defined(UNITY_REVERSED_Z)
						sceneDepth = 1.0 - sceneDepth;
					#endif
					sceneDepth = (sceneDepth * _ProjectionParams.z) + _ProjectionParams.y;
				}
				else
				{
					// Perspective camera
					sceneDepth = LinearEyeDepth(sceneDepth, _ZBufferParams);
				}
				
				//float localDepth = LinearEyeDepth(worldPos, UNITY_MATRIX_V);
				float localDepth = LinearEyeDepth(input.screenPosition.xyzw.z / input.screenPosition.xyzw.w, _ZBufferParams);
				float depthDiff = abs(sceneDepth - localDepth);

				// main texture
				half3 albedo = __albedo.rgb;
				half alpha = __alpha;
				half3 emission = half3(0,0,0);
				
				albedo *= __mainColor.rgb;
				
				// Depth-based color
				half3 depthColor = __depthColor;
				half3 depthColorDist = __depthColorDistance;
				albedo.rgb = lerp(depthColor, albedo.rgb, saturate(depthColorDist * depthDiff));
				
				// Depth-based alpha
				alpha *= saturate((__depthAlphaDistance * depthDiff) + __depthAlphaMin);

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

				half atten = mainLight.shadowAttenuation;

				half ndl = dot(normalWS, lightDir);
				half3 ramp;
				
				ndl = saturate(ndl);
				ramp = float3(1, 1, 1);

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
					ramp = float3(1, 1, 1);

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

				color += emission;

				// Mix the pixel color with fogColor. You can optionally use MixFogColor to override the fogColor with a custom one.
				float fogFactor = input.worldPosAndFog.w;
				color = MixFog(color, fogFactor);
				
					//Vertical Fog
					half vertFogThreshold = input.worldPosAndFog.xyz.y;
					half verticalFogThreshold = __verticalFogThreshold;
					half verticalFogSmooothness = __verticalFogSmoothness;
					half verticalFogMin = verticalFogThreshold - verticalFogSmooothness;
					half verticalFogMax = verticalFogThreshold + verticalFogSmooothness;
					half4 fogColor = __verticalFogColor;
				#if defined(UNITY_PASS_FORWARDADD)
					fogColor.rgb = half3(0, 0, 0);
				#endif
					half vertFogFactor = 1 - saturate((vertFogThreshold - verticalFogMin) / (verticalFogMax - verticalFogMin));
					color.rgb = lerp(color.rgb, fogColor.rgb, vertFogFactor);

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
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
				float4 screenPosition : TEXCOORD0;
				float3 pack1 : TEXCOORD1; /* pack1.xyz = positionWS */
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

				float3 worldPosUv = mul(unity_ObjectToWorld, input.vertex).xyz;

				// Shader Properties Sampling
				float __wavesFrequency = ( tex2Dlod(_WavesFrequency, float4(worldPosUv.xz * _WavesFrequency_ST.xy + _WavesFrequency_ST.zw, 0, 0)).r );
				float __wavesHeight = ( _WavesHeight );
				float __wavesSpeed = ( _WavesSpeed );

				float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
				
				// Vertex water waves
				float _waveFrequency = __wavesFrequency;
				float _waveHeight = __wavesHeight;
				float3 _vertexWavePos = worldPos.xyz * _waveFrequency;
				float _phase = _Time.y * __wavesSpeed;
				float waveFactorX = sin(_vertexWavePos.x + _phase) * _waveHeight;
				float waveFactorZ = sin(_vertexWavePos.z + _phase) * _waveHeight;
				input.vertex.y += (waveFactorX + waveFactorZ);
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);

				//Screen Space UV
				float4 screenPos = ComputeScreenPos(vertexInput.positionCS);
				output.screenPosition.xyzw = screenPos;
				output.pack1.xyz = vertexInput.positionWS;

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

				float3 positionWS = input.pack1.xyz;

				// Shader Properties Sampling
				float4 __albedo = ( tex2D(_Albedo, positionWS.xz * _Albedo_ST.xy + frac(_Time.yy * _Albedo_SC.xy) + _Albedo_ST.zw).rgba * _Albedo1.rgba );
				float4 __mainColor = ( _MainColor.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );

				half3 albedo = __albedo.rgb;
				half alpha = __alpha;
				half3 emission = half3(0,0,0);

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

	}

	FallBack "Hidden/InternalErrorShader"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(unity:"2020.1.3f1";ver:"2.6.0";tmplt:"SG2_Template_URP";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","VERTEX_SIN_WAVES","FOG","NO_RAMP_UNLIT","VERTICAL_FOG","VSW_WORLDPOS","DEPTH_BUFFER_COLOR","DEPTH_BUFFER_ALPHA","TEMPLATE_LWRP"];flags:list[];flags_extra:dict[pragma_gpu_instancing=list[]];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0",RIM_LABEL="Rim Outline",BLEND_TEX1_CHNL="g"];shaderProperties:list[sp(name:"Albedo";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:False;scr:True;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:5;cc:4;chan:"RGBA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:WorldPosition;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_Albedo";md:"";custom:False;refs:"";guid:"41bd495f-da97-46c3-a0dc-c3491d3ed56e";op:Multiply;lbl:"Albedo";gpu_inst:False;locked:False;impl_index:-1),imp_mp_color(def:RGBA(1.000, 1.000, 1.000, 1.000);hdr:False;cc:4;chan:"RGBA";prop:"_Albedo1";md:"";custom:False;refs:"";guid:"748c77eb-dba4-4ab6-8ec6-6362ee889b1f";op:Multiply;lbl:"Albedo Color";gpu_inst:False;locked:False;impl_index:-1)]),sp(name:"Main Color";imps:list[imp_mp_color(def:RGBA(1.000, 1.000, 1.000, 1.000);hdr:True;cc:4;chan:"RGBA";prop:"_MainColor";md:"";custom:False;refs:"";guid:"d712c7ac-dc7b-4e59-a230-fdaf187f016b";op:Multiply;lbl:"Color";gpu_inst:False;locked:False;impl_index:-1)]),,,,,,,sp(name:"Waves Frequency";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:4;cc:1;chan:"R";mip:0;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:WorldPosition;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_WavesFrequency";md:"";custom:False;refs:"";guid:"27681e86-6b31-43b6-837d-8e031162182b";op:Multiply;lbl:"Frequency";gpu_inst:False;locked:False;impl_index:-1)]),,,,,,,sp(name:"Vertical Fog Color";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:False;scr:True;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:5;cc:4;chan:"RGBA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:WorldPosition;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_VerticalFogColor";md:"";custom:False;refs:"";guid:"6a666d30-8f6d-426e-8250-8ea449d927f4";op:Multiply;lbl:"Fog Color";gpu_inst:False;locked:False;impl_index:-1),imp_mp_color(def:RGBA(1.000, 1.000, 1.000, 1.000);hdr:False;cc:4;chan:"RGBA";prop:"_VerticalFogColor1";md:"";custom:False;refs:"";guid:"b062bcf7-da35-4200-b570-15a005a0993d";op:Multiply;lbl:"Vertical Fog Color Color";gpu_inst:False;locked:False;impl_index:-1)]),,,,,,,,,,,,,,,sp(name:"Foam Texture Custom";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;notile:False;triplanar_local:False;def:"black";locked_uv:False;uv:5;cc:3;chan:"RGB";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:WorldPosition;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_FoamTex";md:"";custom:False;refs:"";guid:"238e69e8-9ef3-4381-8b0e-0781addc03ec";op:Multiply;lbl:"Foam Texture Custom";gpu_inst:False;locked:False;impl_index:0)])];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False)) */
/* TCP_HASH 7366c5f084f1afbb6d29835f7b12ca03 */
