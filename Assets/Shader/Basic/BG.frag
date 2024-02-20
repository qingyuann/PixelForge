#version 330 core
in vec2 fUv;

uniform sampler2D uTexture0;
uniform sampler2D uTexture1;
uniform sampler2D uTexture2;

out vec4 FragColor;
 
void main()
{
    vec4 color0 = texture(uTexture0, fUv);

    vec4 color1 = texture(uTexture1, fUv);

    vec4 color2 = texture(uTexture2, fUv);
    color2 = color2*0.01+vec4(0,0,0,0);
    FragColor = mix(color0, color1, color1.w);
    FragColor = mix(FragColor, color2, color2.w);
}