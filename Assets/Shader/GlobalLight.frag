#version 330 core
in vec2 fUv;

uniform sampler2D _BlitTexture;
uniform vec3 _Color;
uniform float _Intensity;
out vec4 FragColor;

void main()
{
    vec4 color = texture(_BlitTexture, fUv);
    vec3 intensity = vec3(_Intensity);
    
    FragColor = texture(_BlitTexture, fUv);
}