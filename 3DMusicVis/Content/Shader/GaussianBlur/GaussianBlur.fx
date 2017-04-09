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

#define RADIUS  7
#define KERNEL_SIZE (RADIUS * 2 + 1)

//-----------------------------------------------------------------------------
// Globals.
//-----------------------------------------------------------------------------

float weights[KERNEL_SIZE];
float2 offsets[KERNEL_SIZE];

//-----------------------------------------------------------------------------
// Textures.
//-----------------------------------------------------------------------------

texture colorMapTexture;

sampler2D colorMap = sampler_state
{
	Texture = <colorMapTexture>;
	MipFilter = Linear;
	MinFilter = Linear;
	MagFilter = Linear;
};

float4 PS_GaussianBlur(VertexShaderOutput input) : COLOR
{
	float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);

	for (int i = 0; i < KERNEL_SIZE; ++i)
		color += tex2D(colorMap, input.TextureCordinate + offsets[i]) * weights[i];

	return color;
}

technique GaussianBlur
{
	pass
	{
		PixelShader = compile PS_SHADERMODEL PS_GaussianBlur();
	}
};