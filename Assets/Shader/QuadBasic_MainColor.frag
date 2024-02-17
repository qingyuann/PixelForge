#version 330 core
in vec2 fUv;

uniform vec3 mainColor;
out vec4 FragColor;

void main()
{
    FragColor = vec4(mainColor, 1.0);
}