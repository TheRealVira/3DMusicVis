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

float distance;
float distortion;
float4 colorDist;

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float f = 0;
	float r2 = (input.TextureCordinate.x - 0.5) * (input.TextureCordinate.x - 0.5) + (input.TextureCordinate.y - 0.5) * (input.TextureCordinate.y - 0.5);

	// Only compute the cubic distortion if necessary.
	if (distortion == 0.0)
		f = 1 + r2 * distance;
	else
		f = 1 + r2 * (distance + distortion * sqrt(r2));

	// Distort each color channel seperately to get a chromatic distortion effect.
	float4 outColor;
	float4 distort = f.xxxx + colorDist;

	for (int i = 0; i < 4; i++)
	{
		float x = distort[i] * (input.TextureCordinate.x - 0.5) + 0.5;
		float y = distort[i] * (input.TextureCordinate.y - 0.5) + 0.5;
		outColor[i] = tex2D(TextureSampler, float4(x,y,0,0))[i];
	}

	return outColor;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};