#version 330 core
in vec2 fUv;

out vec4 FragColor;
uniform sampler2D MainTex;
void main()
{
    vec4 texColor = texture(MainTex, fUv);
    FragColor = texColor;
}
