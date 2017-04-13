#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

// Our texture sampler
texture Texture;
sampler TextureSampler = sampler_state
{
	Texture = <Texture>;
};

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
///----------------------------
float2 RumbleVectorR;
float2 RumbleVectorG;
float2 RumbleVectorB;
float4 RgbRumble(VertexShaderOutput input) : COLOR0
{
	float3 colorR = tex2D(TextureSampler, input.TextureCordinate + RumbleVectorR);
	float3 colorG = tex2D(TextureSampler, input.TextureCordinate + RumbleVectorG);
	float3 colorB = tex2D(TextureSampler, input.TextureCordinate + RumbleVectorB);
	
	float4 returnColor;
	returnColor.r = colorR.r;
	returnColor.g = colorG.g;
	returnColor.b = colorB.b;
	returnColor.a = tex2D(TextureSampler, input.TextureCordinate).a;
	
	return returnColor;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL RgbRumble();
	}
};