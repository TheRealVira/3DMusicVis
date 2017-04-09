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

// Pixel shader applies a one dimensional gaussian blur filter.
// This is used twice by the bloom postprocess, first to
// blur horizontally, and then again to blur vertically.

sampler TextureSampler : register(s0);

#define SAMPLE_COUNT 15

float2 SampleOffsets[SAMPLE_COUNT];
float SampleWeights[SAMPLE_COUNT];

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
    float4 c = 0;
    
    // Combine a number of weighted image filter taps.
    for (int i = 0; i < SAMPLE_COUNT; i++)
    {
        c += tex2D(TextureSampler, input.TextureCordinate + SampleOffsets[i]) * SampleWeights[i];
    }
    
    return c;
}

technique GaussianBlur
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
};