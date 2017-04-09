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

int ImageHeight;
float4 LineColor;
float4 ScanLine(VertexShaderOutput input) : COLOR
{
	float4 c = tex2D(TextureSampler, input.TextureCordinate);
	/*int a = saturate((input.TextureCordinate.y * ImageHeight) % 5);
	int b = saturate((input.TextureCordinate.y * ImageHeight + 1) % 5);
	float m = min(a,b);

	c.rgb *= m;
	return c;*/

	if ((input.TextureCordinate.y * ImageHeight + 1) % 5 < 3) {
		c = LineColor;
	}

	return c;
}

float4 RainbowShader(VertexShaderOutput input) : COLOR0
{
	float4 Color = tex2D(TextureSampler, input.TextureCordinate);
	Color.r = Color.r*sin(input.TextureCordinate.x * 100) * 2;
	Color.g = Color.g*cos(input.TextureCordinate.x * 150) * 2;
	Color.b = Color.b*sin(input.TextureCordinate.x * 50) * 2;

	return Color;
}

technique GaussianBlur
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL ScanLine();
	}

	/*pass Pass2 {
		PixelShader = compile PS_SHADERMODEL RainbowShader();
	}*/
};