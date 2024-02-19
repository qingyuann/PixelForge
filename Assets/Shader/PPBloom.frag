#version 330 core
in vec2 fUv;

uniform sampler2D _BlitTexture;

out vec4 FragColor;
 
void main()
{
    vec4 color0 = texture(_BlitTexture, fUv);
    FragColor = color0;
}