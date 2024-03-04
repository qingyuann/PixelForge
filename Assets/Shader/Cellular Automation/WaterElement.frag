#version 330 core
in vec2 fUv;


uniform vec4 _Time;

out vec4 FragColor;
uniform sampler2D MainTex;
uniform sampler2D PerlinNoise;

float pseudoRandom(float seed) {
    return fract(sin(seed * 12.9898));
}

void main()
{
    vec2 moveSpeed = vec2(0.01, 0.05);
    vec2 noiseSample = fUv + vec2(pseudoRandom(_Time.z * moveSpeed.x), pseudoRandom(_Time.z * moveSpeed.y));
    float noise = texture(PerlinNoise, noiseSample).x;//0-1
    noise = ceil(noise * 16);
    vec2 uv = fUv * noise;

    vec3 texColor = texture(MainTex, uv).xyz;
    FragColor = vec4(texColor, 1);
}

