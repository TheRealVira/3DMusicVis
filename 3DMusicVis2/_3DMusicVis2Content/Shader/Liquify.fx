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

float width;
float4 toBe;
float4 da(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(TextureSampler, input.TextureCordinate);

	if (color.r<width&& color.g < width && color.b < width)return color;

	/*color.rgb /= color.a;
	color.rgb = ((color.rgb - 0.5f) * max(1, 0)) + 0.8f;*/

	return toBe;
}

//-----------------------------------------------------------------------------
// Techniques.
//-----------------------------------------------------------------------------

technique DeleteAlpha
{
	pass
	{
		PixelShader = compile ps_2_0 da();
	}
}