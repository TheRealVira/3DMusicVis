// Our texture sampler
texture Texture;

float width;

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

float4 da(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(TextureSampler, input.TextureCordinate);

	if (color.r<width&& color.g < width && color.b < width)return color;

	/*float high = .6;
	float low = .4;

	if (color.r > high) color.r = 1;
	else if (color.r < low) color.r = 0;

	if (color.g > high) color.g = 1;
	else if (color.g < low) color.g = 0;

	if (color.b > high) color.b = 1;
	else if (color.b < low) color.b = 0;

	return color;*/

	color.rgb /= color.a;

	// Apply contrast.
	color.rgb = ((color.rgb - 0.5f) * max(1, 0)) + 0.8f;

	return color;
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