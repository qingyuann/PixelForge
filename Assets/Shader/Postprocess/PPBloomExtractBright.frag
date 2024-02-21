#version 330 core

in vec2 fUv;
out vec4 FragColor;

uniform sampler2D _BlitTexture;
uniform float bloomThreshold;
uniform float bloomIntensity; 

void main()
{
    // get lightness
    vec4 col = texture(_BlitTexture, fUv);
    float brightness = dot(col, vec4(0.2126, 0.7152, 0.0722,0.0));

    // if brightness is less than threshold, set color to 0
    if (brightness < bloomThreshold)
    {
        col = vec4(0.0);
    }
    // enhance the color
    col *= bloomIntensity;
    
    // output the color
    FragColor = col;
}
