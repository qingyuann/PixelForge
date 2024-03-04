#version 330 core
in vec2 fUv;


uniform vec4 _Time;

out vec4 FragColor;
uniform sampler2D MainTex;
uniform sampler2D PerlinNoise;
void main()
{
    vec2 noiseSample = fUv + sin(_Time.y) * 0.5 + 0.5;
    float noise = texture(PerlinNoise, noiseSample).x;

    vec4 texColor = texture(MainTex, noiseSample);
    FragColor = vec4(noise);
}

