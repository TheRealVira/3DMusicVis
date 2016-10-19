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

	//if (color.r<width&& color.g < width && color.b < width)return color;
	if (color.a < width)return color;
	return toBe;

	//float4 highest = float4(0,0,0,0);

	//[loop]
	//for (int y = 0; y < 2; y++)
	//{
	//	[loop]
	//	for (int x = 0; x < 5; x++)
	//	{
	//		float2 pos = float2(input.TextureCordinate.x + (x - 2), input.TextureCordinate.y + (y - 1));
	//		float4 c = tex2D(TextureSampler, pos);
	//		float3 sub = c.rgb - highest.rgb;
	//		float sol = sub.r + sub.g + sub.b;

	//		//float val = (c.r + c.g + c.b) - (highest.r + highest.g + highest.b);

	//		if (sol>0) {
	//			highest = c;
	//		}
	//	}
	//}

	/*if (highest.r > 0.5) {
		highest.r = 1;
	}

	if (highest.g > 0.5) {
		highest.g = 1;
	}

	if (highest.b > 0.5) {
		highest.b = 1;
	}*/


	///*color.rgb /= color.a;
	//color.rgb = ((color.rgb - 0.5f) * max(1, 0)) + 0.8f;*/

	//return highest;
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