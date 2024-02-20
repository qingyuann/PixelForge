#version 330 core

in vec2 fUv;
out vec4 FragColor;

uniform sampler2D _BlitTexture;
uniform sampler2D _ScreenCol;

void main()
{
    // get lightness
    vec4 bloom = texture(_BlitTexture, fUv);
    vec4 screen = texture(_ScreenCol, fUv);

    bloom+=screen;
    
    // output the color
    FragColor = bloom;
}
