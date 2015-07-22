
Shader "Custom/Sprite-Transparent" 
{
	Properties 
	{
		_BackgroundColor("Sprite Color", Color) = (1, 1, 1, 1)
		_Color ("LOS Color", Color) = (1,1,1,0.5)
		_Fog ("Fog Color", Color) = (1,1,1,1)
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		
		_Center1 ("Center_1", Vector) = (0,0,0,0)
		_Radius1 ("Radius_1", Float) = 3.0
		_Blurring1 ("Blurring Distance_1", Float) = 1.0
		
		_Vector11 ("_Vector11", Vector) = (0,0,0,0)
		_Vector12 ("_Vector12", Vector) = (0,0,0,0)
		_Vector13 ("_Vector13", Vector) = (0,0,0,0)
		_Vector14 ("_Vector14", Vector) = (0,0,0,0)
		_Vector15 ("_Vector15", Vector) = (0,0,0,0)
		_Vector16 ("_Vector16", Vector) = (0,0,0,0)
		_Vector17 ("_Vector17", Vector) = (0,0,0,0)
		_Vector18 ("_Vector18", Vector) = (0,0,0,0)
		_Vector19 ("_Vector19", Vector) = (0,0,0,0)
		_Vector110 ("_Vector110", Vector) = (0,0,0,0)
		_Vector111 ("_Vector111", Vector) = (0,0,0,0)
		_Vector112 ("_Vector112", Vector) = (0,0,0,0)
		_Vector113 ("_Vector113", Vector) = (0,0,0,0)
		_Vector114 ("_Vector114", Vector) = (0,0,0,0)
		_Vector115 ("_Vector115", Vector) = (0,0,0,0)
		_Vector116 ("_Vector116", Vector) = (0,0,0,0)
		_Vector117 ("_Vector117", Vector) = (0,0,0,0)
		_Vector118 ("_Vector118", Vector) = (0,0,0,0)
		_Vector119 ("_Vector119", Vector) = (0,0,0,0)
		_Vector120 ("_Vector120", Vector) = (0,0,0,0)

		_Bool10 ("_Bool10", Vector) = (0,0,0,0)
		_Bool11 ("_Bool11", Vector) = (0,0,0,0)
		_Bool12 ("_Bool12", Vector) = (0,0,0,0)
		_Bool13 ("_Bool13", Vector) = (0,0,0,0)
		_Bool14 ("_Bool14", Vector) = (0,0,0,0)

		_Center2 ("_Center2", Vector) = (0,0,0,0)
		_Radius2 ("Radius_2", Float) = 0
		_Blurring2 ("Blurring Distance_2", Float) = 1.0
		
		_Vector21 ("_Vector21", Vector) = (0,0,0,0)
		_Vector22 ("_Vector22", Vector) = (0,0,0,0)
		_Vector23 ("_Vector23", Vector) = (0,0,0,0)
		_Vector24 ("_Vector24", Vector) = (0,0,0,0)
		_Vector25 ("_Vector25", Vector) = (0,0,0,0)
		_Vector26 ("_Vector26", Vector) = (0,0,0,0)
		_Vector27 ("_Vector27", Vector) = (0,0,0,0)
		_Vector28 ("_Vector28", Vector) = (0,0,0,0)

		_Bool20 ("_Bool20", Vector) = (0,0,0,0)
		_Bool21 ("_Bool21", Vector) = (0,0,0,0)

		_Center3 ("_Center3", Vector) = (0,0,0,0)
		_Radius3 ("_Radius3", Float) = 0.0
		_Blurring3 ("_Blurring3", Float) = 0.0
	}

	SubShader 
	{
		
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _BackgroundColor;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _BackgroundColor;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
				c.rgb *= c.a;
				return c;
			}
			ENDCG
		}

		Pass 
		{
			CGPROGRAM
		    #pragma vertex vert  
			#pragma fragment frag
			#pragma target 3.0 
			
			float4 _Center1;
			float4 _Fog;
			float4 _Color;
			float _Radius1;
			float _Blurring1;

			float4 _Vector11;
			float4 _Vector12;
			float4 _Vector13;
			float4 _Vector14;
			float4 _Vector15;
			float4 _Vector16;
			float4 _Vector17;
			float4 _Vector18;
			float4 _Vector19;
			float4 _Vector110;
			float4 _Vector111;
			float4 _Vector112;
			float4 _Vector113;
			float4 _Vector114;
			float4 _Vector115;
			float4 _Vector116;
			float4 _Vector117;
			float4 _Vector118;
			float4 _Vector119;
			float4 _Vector120;

			float4 _Bool10;
			float4 _Bool11;
			float4 _Bool12;
			float4 _Bool13;
			float4 _Bool14;

			float4 _Center2;
			float _Radius2;
			float _Blurring2;

			float4 _Vector21;
			float4 _Vector22;
			float4 _Vector23;
			float4 _Vector24;
			float4 _Vector25;
			float4 _Vector26;
			float4 _Vector27;
			float4 _Vector28;

			float4 _Bool20;
			float4 _Bool21;

			float4 _Center3;
			float _Radius3;
			float _Blurring3;

			 struct vertexInput 
			 {
				float4 vertex : POSITION;
			 };

			 struct vertexOutput 
			 {
				float4 pos : SV_POSITION;
				float4 position_in_world_space : TEXCOORD0;
			 };

			 float IsOnSameSide(float2 pos1, float2 pos2, float2 mnc)
			 {
				float k = (pos2.y - pos1.y)/(pos2.x - pos1.x);
				float numerator = k*pos1.x + mnc.y - pos1.y;
				float denominator = k - mnc.x;
				float x = numerator / denominator;

				if(((x < pos1.x) && (x > pos2.x)) || ((x < pos2.x) && (x > pos1.x)))
				{
					return 0;
				}
				else
				{
					return 1;
				}
			 }

			 float2 ReturnMandC(float2 pos1, float2 pos2)
			 {
				float m = (pos2.y - pos1.y) / (pos2.x - pos1.x);
				float c = pos1.y - (m * pos1.x);
				return float2(m, c);
			 }

			 float CheckInShadowOf2(float2 pos1, float2 pos2, float2 position)
			 {
				float2 centerPos = float2(_Center2.x, _Center2.y);

				float A = IsOnSameSide(position, pos1, ReturnMandC(centerPos, pos2)); // True Wanted
				if(centerPos.x == pos2.x)
				{
					if((position.x < pos2.x && pos1.x < pos2.x) || (position.x > pos2.x && pos1.x > pos2.x))
						A = 1;
					else
						A = 0;
				}
				
				float B = IsOnSameSide(position, pos2, ReturnMandC(centerPos, pos1)); // True Wanted
				if(centerPos.x == pos1.x)
				{
					if((position.x > centerPos.x && pos2.x > centerPos.x) || (position.x < centerPos.x && pos2.x < centerPos.x))
						B = 1;
					else
						B = 0;
				}

				float C = IsOnSameSide(position, centerPos, ReturnMandC(pos1, pos2)); // False Wanted
				if(pos1.x == pos2.x)
				{
					if((position.x > pos1.x && centerPos.x > pos1.x) || (position.x < pos1.x && centerPos.x < pos1.x))
						C = 1;
					else
						C = 0;
				}

				if((A == 1) && (B == 1) && (C == 0))
					return 1;
				else
					return 0;
			 }

			 float CheckInShadowOf(float2 pos1, float2 pos2, float2 position)
			 {
				float2 centerPos = float2(_Center1.x, _Center1.y);

				float A = IsOnSameSide(position, pos1, ReturnMandC(centerPos, pos2)); // True Wanted
				if(centerPos.x == pos2.x)
				{
					if((position.x < pos2.x && pos1.x < pos2.x) || (position.x > pos2.x && pos1.x > pos2.x))
						A = 1;
					else
						A = 0;
				}
				
				float B = IsOnSameSide(position, pos2, ReturnMandC(centerPos, pos1)); // True Wanted
				if(centerPos.x == pos1.x)
				{
					if((position.x > centerPos.x && pos2.x > centerPos.x) || (position.x < centerPos.x && pos2.x < centerPos.x))
						B = 1;
					else
						B = 0;
				}

				float C = IsOnSameSide(position, centerPos, ReturnMandC(pos1, pos2)); // False Wanted
				if(pos1.x == pos2.x)
				{
					if((position.x > pos1.x && centerPos.x > pos1.x) || (position.x < pos1.x && centerPos.x < pos1.x))
						C = 1;
					else
						C = 0;
				}

				if((A == 1) && (B == 1) && (C == 0))
					return 1;
				else
					return 0;
			 }

			 float CheckForShadow(float2 position)
			 {
				float2 pos1, pos2;
				float2 centerPos = float2(_Center1.x, _Center1.y);
				float possibility;

				// Vector11
				if(_Bool10.x == 0)
					return 0;

				pos1 = float2(_Vector11.x, _Vector11.y);
				pos2 = float2(_Vector11.z, _Vector11.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// Vector12
				if(_Bool10.y == 0)
					return 0;

				pos1 = float2(_Vector12.x, _Vector12.y);
				pos2 = float2(_Vector12.z, _Vector12.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// Vector13
				if(_Bool10.z == 0)
					return 0;

				pos1 = float2(_Vector13.x, _Vector13.y);
				pos2 = float2(_Vector13.z, _Vector13.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// Vector14
				if(_Bool10.w == 0)
					return 0;

				pos1 = float2(_Vector14.x, _Vector14.y);
				pos2 = float2(_Vector14.z, _Vector14.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector15
				if(_Bool11.x == 0)
					return 0;

				pos1 = float2(_Vector15.x, _Vector15.y);
				pos2 = float2(_Vector15.z, _Vector15.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector16
				if(_Bool11.y == 0)
					return 0;

				pos1 = float2(_Vector16.x, _Vector16.y);
				pos2 = float2(_Vector16.z, _Vector16.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector17
				if(_Bool11.z == 0)
					return 0;

				pos1 = float2(_Vector17.x, _Vector17.y);
				pos2 = float2(_Vector17.z, _Vector17.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector18
				if(_Bool11.w == 0)
					return 0;

				pos1 = float2(_Vector18.x, _Vector18.y);
				pos2 = float2(_Vector18.z, _Vector18.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector19
				if(_Bool12.x == 0)
					return 0;

				pos1 = float2(_Vector19.x, _Vector19.y);
				pos2 = float2(_Vector19.z, _Vector19.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector110
				if(_Bool12.y == 0)
					return 0;

				pos1 = float2(_Vector110.x, _Vector110.y);
				pos2 = float2(_Vector110.z, _Vector110.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector111
				if(_Bool12.z == 0)
					return 0;

				pos1 = float2(_Vector111.x, _Vector111.y);
				pos2 = float2(_Vector111.z, _Vector111.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector112
				if(_Bool12.w == 0)
					return 0;

				pos1 = float2(_Vector112.x, _Vector112.y);
				pos2 = float2(_Vector112.z, _Vector112.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector113
				if(_Bool13.x == 0)
					return 0;

				pos1 = float2(_Vector113.x, _Vector113.y);
				pos2 = float2(_Vector113.z, _Vector113.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector114
				if(_Bool13.y == 0)
					return 0;

				pos1 = float2(_Vector114.x, _Vector114.y);
				pos2 = float2(_Vector114.z, _Vector114.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector115
				if(_Bool13.z == 0)
					return 0;

				pos1 = float2(_Vector115.x, _Vector115.y);
				pos2 = float2(_Vector115.z, _Vector115.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector116
				if(_Bool13.w == 0)
					return 0;

				pos1 = float2(_Vector116.x, _Vector116.y);
				pos2 = float2(_Vector116.z, _Vector116.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector117
				if(_Bool14.x == 0)
					return 0;

				pos1 = float2(_Vector117.x, _Vector117.y);
				pos2 = float2(_Vector117.z, _Vector117.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector118
				if(_Bool14.y == 0)
					return 0;

				pos1 = float2(_Vector118.x, _Vector118.y);
				pos2 = float2(_Vector118.z, _Vector118.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector119
				if(_Bool14.z == 0)
					return 0;

				pos1 = float2(_Vector119.x, _Vector119.y);
				pos2 = float2(_Vector119.z, _Vector119.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector120
				if(_Bool14.w == 0)
					return 0;

				pos1 = float2(_Vector120.x, _Vector120.y);
				pos2 = float2(_Vector120.z, _Vector120.w);
				possibility = CheckInShadowOf(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				return 0;
			 }

			 float CheckForShadow2(float2 position)
			 {
				float2 pos1, pos2;
				float2 centerPos = float2(_Center2.x, _Center2.y);
				float possibility;

				// Vector21
				if(_Bool20.x == 0)
					return 0;

				pos1 = float2(_Vector21.x, _Vector21.y);
				pos2 = float2(_Vector21.z, _Vector21.w);
				possibility = CheckInShadowOf2(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// Vector22
				if(_Bool20.y == 0)
					return 0;

				pos1 = float2(_Vector22.x, _Vector22.y);
				pos2 = float2(_Vector22.z, _Vector22.w);
				possibility = CheckInShadowOf2(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// Vector23
				if(_Bool20.z == 0)
					return 0;

				pos1 = float2(_Vector23.x, _Vector23.y);
				pos2 = float2(_Vector23.z, _Vector23.w);
				possibility = CheckInShadowOf2(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// Vector14
				if(_Bool20.w == 0)
					return 0;

				pos1 = float2(_Vector24.x, _Vector24.y);
				pos2 = float2(_Vector24.z, _Vector24.w);
				possibility = CheckInShadowOf2(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector25
				if(_Bool21.x == 0)
					return 0;

				pos1 = float2(_Vector25.x, _Vector25.y);
				pos2 = float2(_Vector25.z, _Vector25.w);
				possibility = CheckInShadowOf2(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector26
				if(_Bool21.y == 0)
					return 0;

				pos1 = float2(_Vector26.x, _Vector26.y);
				pos2 = float2(_Vector26.z, _Vector26.w);
				possibility = CheckInShadowOf2(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector27
				if(_Bool21.z == 0)
					return 0;

				pos1 = float2(_Vector27.x, _Vector27.y);
				pos2 = float2(_Vector27.z, _Vector27.w);
				possibility = CheckInShadowOf2(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				// _Vector28
				if(_Bool21.w == 0)
					return 0;

				pos1 = float2(_Vector28.x, _Vector28.y);
				pos2 = float2(_Vector28.z, _Vector28.w);
				possibility = CheckInShadowOf2(pos1, pos2, position);
				if(possibility == 1)
					return 1;

				return 0;
			 }

			 vertexOutput vert(vertexInput input) 
			 {
				vertexOutput output; 
 				output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
				output.position_in_world_space = mul(_Object2World, input.vertex); 
				return output;
			 }

			 float4 DetermineColor(vertexOutput input, float distSquared, float radiusSquared, float blurringRadius)
			 {
				if (distSquared < radiusSquared)
				{
					float isInShadow = CheckForShadow(float2(input.position_in_world_space.x, input.position_in_world_space.y));
					if(isInShadow == 1)
						return _Fog;
					
					if(distSquared < blurringRadius * blurringRadius)
						return _Color;
					else
					{
						float distance = sqrt(distSquared);
						float factor = distance - blurringRadius;
						factor *= blurringRadius;
						return float4(_Fog.r, _Fog.g, _Fog.b, _Fog.a * factor);
					}
				}
				else
				{
					return _Fog;
				}
			 }
 
			float4 DetermineColor2(vertexOutput input, float distSquared, float radiusSquared, float blurringRadius)
			 {
				if (distSquared < radiusSquared)
				{
					float isInShadow = CheckForShadow2(float2(input.position_in_world_space.x, input.position_in_world_space.y));
					if(isInShadow == 1)
						return _Fog;
					
					if(distSquared < blurringRadius * blurringRadius)
						return _Color;
					else
					{
						float distance = sqrt(distSquared);
						float factor = distance - blurringRadius;
						factor *= blurringRadius;
						return float4(_Fog.r, _Fog.g, _Fog.b, _Fog.a * factor);
					}
				}
				else
				{
					return _Fog;
				}
			 }

			 float4 DetermineColor3(vertexOutput input, float distSquared, float radiusSquared, float blurringRadius)
			 {
				if (distSquared < radiusSquared)
				{
					if(distSquared < blurringRadius * blurringRadius)
						return _Color;
					else
					{
						float distance = sqrt(distSquared);
						float factor = distance - blurringRadius;
						factor *= blurringRadius;
						return float4(_Fog.r, _Fog.g, _Fog.b, _Fog.a * factor);
					}
				}
				else
				{
					return _Fog;
				}
			 }

			 float4 frag(vertexOutput input) : COLOR 
			 {
				float distSquared = (input.position_in_world_space.x -_Center1.x)*(input.position_in_world_space.x -_Center1.x)
							+ (input.position_in_world_space.y - _Center1.y)*(input.position_in_world_space.y - _Center1.y);
				float radiusSquared = _Radius1 * _Radius1;
				float blurringRadius = _Radius1 - _Blurring1;
				
				float distSquared2 = (input.position_in_world_space.x -_Center2.x)*(input.position_in_world_space.x -_Center2.x)
							+ (input.position_in_world_space.y - _Center2.y)*(input.position_in_world_space.y - _Center2.y);
				float radiusSquared2 = _Radius2 * _Radius2;
				float blurringRadius2 = _Radius2 - _Blurring2;

				float distSquared3 = (input.position_in_world_space.x -_Center3.x)*(input.position_in_world_space.x -_Center3.x)
							+ (input.position_in_world_space.y - _Center3.y)*(input.position_in_world_space.y - _Center3.y);
				float radiusSquared3 = _Radius3 * _Radius3;
				float blurringRadius3 = _Radius3 - _Blurring3;

				float4 color1 = DetermineColor(input, distSquared, radiusSquared, blurringRadius);
				float4 color2 = DetermineColor2(input, distSquared2, radiusSquared2, blurringRadius2);
				float4 color3 = DetermineColor3(input, distSquared3, radiusSquared3, blurringRadius3);

				float4 color;

				if(color2.a < color1.a)
				{
					color = color2;
				}
				else
				{
					color = color1;
				}

				if(color.a < color3.a)
				{
					color = color;
				}
				else
				{
					color = color3;
				}

				return color;
			 }
 
			 ENDCG  
        }

	} 

	FallBack "Diffuse"
}