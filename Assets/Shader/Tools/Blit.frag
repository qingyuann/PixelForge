#version 330 core
in vec2 fUv;

uniform sampler2D _BlitTexture;
out vec4 FragColor;

void main()
{
    FragColor = texture(_BlitTexture, fUv);
}