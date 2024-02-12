#version 330 core
in vec2 fUv;

uniform vec3 uColor;
out vec4 FragColor;
 
void main()
{
    FragColor = vec4(uColor, 1.0);
}