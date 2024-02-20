#version 330 core
out vec4 FragColor;

uniform vec3 uColor;
uniform sampler2D MainTex;
in vec2 fUv;
void main()
{
    vec4 mainTex = texture(MainTex, fUv);    
    vec4 color = vec4(uColor, 1.0);
    FragColor = mainTex+color*2;
}