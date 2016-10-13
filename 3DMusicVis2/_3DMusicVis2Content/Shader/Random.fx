// Our texture sampler
texture Texture;

sampler TextureSampler = sampler_state
{
	Texture = <Texture>;
};

struct VertexShaderOutput
{
	float4 Position : TEXCOORD0;
	float4 Color : COLOR0;
	float2 TextureCordinate : TEXCOORD0;
};

int ImageHeight;
float4 ScanLine(VertexShaderOutput input) : COLOR0
{
	float4 c = tex2D(TextureSampler, input.TextureCordinate);
	int a = saturate((input.Position.y * ImageHeight) % 4);
	int b = saturate((input.Position.y * ImageHeight + 1) % 4);
	float m = min(a,b);

	c.rgb *= m;

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
///----------------------------
technique GaussianBlur
{

	pass Pass1
	{
		PixelShader = compile ps_2_0 ScanLine();
	}

	/*pass Pass2 {
		PixelShader = compile ps_2_0 RainbowShader();
	}*/
}