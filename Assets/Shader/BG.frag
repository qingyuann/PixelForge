#version 330 core
in vec2 fUv;

uniform sampler2D uTexture0;
uniform sampler2D uTexture1;

out vec4 FragColor;
 
void main()
{
    vec4 color0 = texture(uTexture0, fUv);
    vec4 color1 = texture(uTexture1, fUv);
    FragColor = mix(color0, color1, color1.w);
}