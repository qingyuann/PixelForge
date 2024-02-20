#version 330 core

in vec2 fUv;
out vec4 FragColor;

uniform sampler2D _BlitTexture;
uniform sampler2D _ScreenCol;

void main()
{
    // get lightness
    vec3 bloom = texture(_BlitTexture, fUv).rgb;
    vec3 screen = texture(_ScreenCol, fUv).rgb;

    bloom+=screen;
    
    // output the color
    FragColor = vec4(bloom, 1.0);
}
