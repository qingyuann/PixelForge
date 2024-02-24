#version 330 core
#define PI 3.14
//https://github.com/mattdesl/lwjgl-basics/wiki/2D-Pixel-Perfect-Shadows
//inputs from vertex shader
in vec2 fUv;

//uniform values
uniform sampler2D _BlitTexture;
uniform vec2 lightMapUVMove;


//alpha threshold for our occlusion map
out vec4 FragColor;

void main(void) {
    float distance = 1.0;
    vec2 sampleUV = fUv + lightMapUVMove;
    vec4 lightMap = texture(_BlitTexture, sampleUV);
    FragColor = lightMap;
}