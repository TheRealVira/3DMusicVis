#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCordinate : TEXCOORD0;
};
//------------------------------ TEXTURE PROPERTIES ----------------------------
// This is the texture that SpriteBatch will try to set before drawing
texture ScreenTexture;
 
// Our sampler for the texture, which is just going to be pretty simple
sampler TextureSampler = sampler_state
{
    Texture = <ScreenTexture>;
};

float width;
float4 toBe;

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(TextureSampler, input.TextureCordinate);
	
	//if (color.r<width&& color.g < width && color.b < width)return color;
	if (color.a < width)return color;
		return toBe;
		
	float4 highest = float4(0,0,0,0);

	[loop]
	for (int y = -2; y < 2; y++)
	{
		[loop]
		for (int x = -5; x < 5; x++)
		{
			float2 pos = float2(input.TextureCordinate.x + (x - 2), input.TextureCordinate.y + (y - 1));
			float4 c = tex2D(TextureSampler, pos);
			float3 sub = c.rgb - highest.rgb;
			float sol = sub.r + sub.g + sub.b;

			//float val = (c.r + c.g + c.b) - (highest.r + highest.g + highest.b);

			if (sol>0) {
				highest = c;
			}
		}
	}

	if (highest.r > 0.5) {
		highest.r = 1;
	}

	if (highest.g > 0.5) {
		highest.g = 1;
	}

	if (highest.b > 0.5) {
		highest.b = 1;
	}


	///*color.rgb /= color.a;
	color.rgb = ((color.rgb - 0.5f) * max(1, 0)) + 0.8f;

	return highest;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};