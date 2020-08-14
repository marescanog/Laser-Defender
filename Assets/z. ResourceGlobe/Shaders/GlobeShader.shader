Shader "Unlit/HealthBarNew"
{
	Properties
	{
	    [Header(Color Settings)]
	    [Toggle] _Linear ("Linear Color Space", Float) = 1.0
	    
	    [Header(Texture Settings)]
		_MainTex ("Primary Texture", 2D) = "transparent" {}
		_SecondaryTex ("Secondary Texture", 2D) = "transparent" {}
		_ThirdTexture ("Third Texture", 2D) = "transparent" {}
		_WaterLineTexture ("WaterLine Texture", 2D) = "transparent" {}
		_MaskTexture ("Mask Texture", 2D) = "transparent" {}
        _NormalTexture ("Normal Map Texture", 2D) = "transparent" {}
		_MainBrightness ("Main Brightness", Float) = 3.0
		
		[Header(Health Settings)]
		_Health ("Health Value", Float) = 100.0
		_MaximumHealth ("Maximum Health Value", Float) = 100.0
		
		[Header(Water Settings)]
		_RotationSpeed ("Rotation Speed", Float) = 0.0
		_ParticleRotationSpeed ("Particle Rotation Speed", Float) = 0.0
		[Toggle] _Wave ("Waves", Float) = 0.0
		[Toggle] _WaterLine ("WaterLine", Float) = 0.0
		_OutOfLine ("Crop around Line", Range (0, 100)) = 25
		_WaterLineColor ("WaterLine Color", Vector) = (1.0, 1.0, 1.0, 1.0)
		_LineBoldness ("WaterLine Boldness", Float) = 5.0
	}
	SubShader
	{
		Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
		LOD 100

		Pass
		{   
            Cull Off
            Lighting Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _SecondaryTex;
			sampler2D _ThirdTexture;
			sampler2D _ThirdTextureAlpha;
			sampler2D _WaterLineTexture;
			sampler2D _MaskTexture;
			sampler2D _NormalTexture;
			
			static float PI = 3.141592653589793238462643383;
			
			float4 _MainTex_ST;
			float4 _WaterLineColor;
			
			float _Health; 
			float _MaximumHealth; 
			float _RotationSpeed;
			float _ParticleRotationSpeed;
			float _MainBrightness;
			float _Wave;
			float _WaterLine;
			float _LineBoldness;
			float _OutOfLine;
			
			float _Linear;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{			    
				// Copying uv to make some calculations on it.
                float2 newUV = (i.uv - 0.5) * 2;
                
                // Calculating the percentage of the health.
                float healthPercentage = (_Health / _MaximumHealth - 0.5) * 2;
                
                // Masking the output with a masking texture.
                float alpha = tex2D(_MaskTexture,i.uv).r;
                                                
                // Applying sin waves according to the position being near to percentage.
                if(_Wave != 0) {
                    newUV.y += sin(newUV.x * 10.0 + _Time.y * 5) / 50.0 * (1 + newUV.y - healthPercentage);
                }
                                                
                // Cropping the circle according to the health percentage.
                float healthDistance = newUV.y;
                float pwidth = fwidth(healthDistance);
                
                alpha -= smoothstep(healthPercentage - pwidth * 10 , healthPercentage + (_OutOfLine / 100), healthDistance);
                alpha = max(alpha, 0);
                                
                //Line
                float4 waterLine = float4(0.0, 0.0, 0.0, 0.0);
                
                if(_WaterLine != 0) {
                    waterLine = _WaterLineColor;
                    float2 lineUV = newUV;
                    lineUV.y -= healthPercentage;
                    lineUV.x += _Time / 2;
                    waterLine *= tex2D(_WaterLineTexture, lineUV);
                    
                    if(lineUV.y < (healthPercentage / 2 - 0.5) / 2 || lineUV.y > 0.5 +   (healthPercentage / 2 - 0.5) / 2) {
                        waterLine.a = 0;
                    }
			    }
                
                // Shading to make it look like a sphere.
                float normal = tex2D(_NormalTexture, i.uv).b;
                newUV.x = 0.5 + atan2(newUV.x, normal) / (2 * PI);
                newUV.y = asin(newUV.y) / PI - 0.5; 
                
                // Creating Particle UV and rotating UVs
                float2 particleUV = newUV;
                particleUV.y -= _Time * _ParticleRotationSpeed;
                newUV.x -= _Time * _RotationSpeed;
                
                float4 textureColor = 
                                tex2D(_MainTex, newUV * 1.2) *
                                tex2D(_SecondaryTex, -1 * particleUV * 1.4) * 
                                tex2D(_ThirdTexture, particleUV * 1.1) *  
                                1 * ((pow(_MainBrightness, 0.5) * (1 - _Linear)) + (_MainBrightness * _Linear));
                                      
                textureColor = float4(textureColor.rgb * alpha , alpha);
                                                                
                fixed4 finalWaterLineColor = ((textureColor * waterLine * ((1 * (1 - _Linear)) + (10 * _Linear))) + waterLine) * waterLine.a * 4 * _WaterLineColor.a * (_LineBoldness - 2 * (1 - _Linear)) * alpha;
                                
                fixed4 col = fixed4(textureColor.rgb + finalWaterLineColor.rgb, textureColor.a);
                
				return col;
			}
			ENDCG
		}
	}
}
