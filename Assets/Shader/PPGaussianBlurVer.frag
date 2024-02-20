#version 330 core

in vec2 fUv;
out vec4 FragColor;

uniform sampler2D _BlitTexture;
uniform float screenWidth;
uniform float screenHeight;
uniform float offset;

float weight[5] = float[](0.0545, 0.2442, 0.4026, 0.2442, 0.0545);

void main()
{
    vec2 tex_offset = vec2(1.0 / screenWidth, 1.0 / screenHeight);

    vec3 result = vec3(0, 0, 0);

    for (int i = 0; i < 5; i++)
    {
        result += texture(_BlitTexture, fUv + vec2(0, (i - 2) * offset) * tex_offset).rgb * weight[i];
    }

    FragColor = vec4(result, 1.0);
}