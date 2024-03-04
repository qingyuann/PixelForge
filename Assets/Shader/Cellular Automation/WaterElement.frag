#version 330 core
in vec2 fUv;


uniform vec4 _Time;

out vec4 FragColor;
uniform sampler2D PerlinNoise;

uniform sampler2D WaterColPanel;

uniform sampler2D WaterTex;
uniform sampler2D FireTex;
uniform sampler2D OilTex;
uniform sampler2D AcidTex;
uniform sampler2D LavaTex;
uniform sampler2D StoneTex;
uniform sampler2D SandTex;
uniform sampler2D SmokeTex;

uniform float Resolution;
float pseudoRandom(float seed) {
    return fract(sin(seed * 12.9898));
}

float pseudoRandom(vec2 seed) {
    return fract(sin(dot(seed.xy, vec2(12.9898, 78.233))));
}

void main()
{
    vec3 blank = vec3(0, 0, 0);

    //16*16
    float picRes = 256;
    vec2 uv = ceil(fUv * picRes) / picRes;

    float water = texture(WaterTex, uv).a;
    float fire = texture(FireTex, uv).a;
    float oil = texture(OilTex, uv).a;
    float acid = texture(AcidTex, uv).a;
    float lava = texture(LavaTex, uv).a;
    float stone = texture(StoneTex, uv).a;

    //noise block 0-1
    vec2 moveSpeed = vec2(0.01, 0.05);
    vec2 noiseSample1 = uv + vec2(pseudoRandom(_Time.z * moveSpeed.x), pseudoRandom(_Time.z * moveSpeed.y));
    float noise_1 = texture(PerlinNoise, noiseSample1 * 5).x;
    vec2 noiseSample2 = uv + vec2(pseudoRandom(_Time.z * moveSpeed.x * 1.2), pseudoRandom(_Time.z * moveSpeed.y * 0.2258));
    float noise_2 = texture(PerlinNoise, noiseSample2 * 2.24544).x;
    vec2 noiseSample3 = uv + vec2(pseudoRandom(_Time.z * moveSpeed.x * 3), pseudoRandom(_Time.z * moveSpeed.y * 2));
    float noise_3 = texture(PerlinNoise, noiseSample3 * 1.2).x;
    float noise = mix(noise_1, noise_2, 0.6);
    noise = mix(noise, noise_3, 0.2388);

    ///////////////////////
    ///////// water////////
    ///////////////////////
    float stepLength = 0.002;
    float maxStep = 20;
    float i = 1;
    float alpha = 1;
    while (alpha > 0.1) {
        vec2 tempUV = vec2(uv.x, uv.y - stepLength * i);
        alpha = texture(WaterTex, tempUV).a;
        i++;
        if (i > maxStep) {
            break;
        }
    }
    vec2 waterColorUV = vec2(noise, 1 - i / (maxStep+1));
    //    waterColorUV = vec2(i / maxStep,i / maxStep);
    vec3 waterCol = texture(WaterColPanel, waterColorUV).xyz;
    waterCol = mix(blank, waterCol, water);


    ///////////////////////
    ///////// fire ////////
    ///////////////////////
    
    
    
    FragColor = vec4(waterCol, 1);
    //    FragColor = vec4(alpha, alpha, alpha, 1);
    //    FragColor = vec4(noise, noise, noise, 1);
//    FragColor = vec4(i / 5, i / 5, i / 5, 1);
}