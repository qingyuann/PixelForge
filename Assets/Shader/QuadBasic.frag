#version 330 core
in vec2 fUv;

out vec4 FragColor;
uniform sampler2D MainTex;
void main()
{
    vec4 texColor = texture(MainTex, fUv);
    texColor=texColor+0.1* vec4(1, 1,1, 1.0);
    FragColor = texColor;
}

