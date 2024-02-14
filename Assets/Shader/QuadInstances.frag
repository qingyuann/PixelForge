#version 330 core
out vec4 FragColor;

uniform vec3 uColor;
uniform sampler2D _QuadInstanceMainTex;
in vec2 fUv;
void main()
{
    vec4 mainTex = texture(_QuadInstanceMainTex, fUv);    
    vec4 color = vec4(uColor, 1.0);
    FragColor = mainTex+color*2;
}