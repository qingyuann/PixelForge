#version 330 core
in vec2 fUv;


uniform vec4 _Time;

out vec4 FragColor;
uniform sampler2D MainTex;
uniform sampler2D PerlinNoise;
uniform float Resolution;
float pseudoRandom(float seed) {
    return fract(sin(seed * 12.9898));
}

float pseudoRandom(vec2 seed) {
    return fract(sin(dot(seed.xy, vec2(12.9898, 78.233))) );
}

void main()
{
    //16*16
    float res = 16;
    vec2 uv = ceil(fUv * res) / res;

    //noise block 0-1
    vec2 moveSpeed = vec2(0.01, 0.05);
    vec2 noiseSample = uv + vec2(pseudoRandom(_Time.z * moveSpeed.x), pseudoRandom(_Time.z * moveSpeed.y));
    float noise = texture(PerlinNoise, noiseSample).x;

    //random color
    float ranColorFrac = ceil((pseudoRandom(uv+_Time.y/10000) * 0.5 + 0.5) * 16);//0-16
    vec2 ranColorUv = vec2(floor(ranColorFrac / 4), mod(ranColorFrac, 4)) / 4;
    vec3 texColor = texture(MainTex, ranColorUv+0.001).xyz;
    FragColor = vec4(texColor, 1);
//    FragColor = vec4(ranColorUv.xyx, 1);
}

