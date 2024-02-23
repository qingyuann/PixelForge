#version 330 core
in vec2 fUv;

uniform sampler2D _BlitTexture;
uniform sampler2D _MergeTexture;
out vec4 FragColor;

void main()
{
    vec4 color1 = texture(_BlitTexture, fUv);
    vec4 color2 = texture(_MergeTexture, fUv);
    FragColor = color1 + color2;
}