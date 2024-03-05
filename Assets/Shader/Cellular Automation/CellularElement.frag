#version 330 core
in vec2 fUv;


uniform vec4 _Time;

out vec4 FragColor;
uniform sampler2D PerlinNoise;

uniform sampler2D WaterColPanel;
uniform sampler2D FireColPanel;
uniform sampler2D StoneColPanel;
uniform sampler2D SandColPanel;
uniform sampler2D OilColPanel;
uniform sampler2D AcidColPanel;

uniform sampler2D WaterTex;
uniform sampler2D FireTex;
uniform sampler2D OilTex;
uniform sampler2D AcidTex;
uniform sampler2D LavaTex;
uniform sampler2D StoneTex;
uniform sampler2D SandTex;
uniform sampler2D SmokeTex;
uniform sampler2D SteamTex;

uniform float Resolution;
float pseudoRandom(float seed) {
    return fract(sin(seed * 12.9898));
}

float pseudoRandom(vec2 seed) {
    return fract(sin(dot(seed.xy, vec2(12.9898, 78.233))));
}

void main()
{
    vec4 col = vec4(0, 0, 0, 0);

    //left top corner is (0,0)2
    float picRes = 256;
    vec2 uv = ceil(fUv * picRes) / picRes;

    //noise block 0-1
    vec2 moveSpeed = vec2(0.002, 0.001);
    vec2 noiseSample1 = uv + vec2(pseudoRandom(_Time.z * moveSpeed.x), pseudoRandom(_Time.z * moveSpeed.y));
    float noise_1 = texture(PerlinNoise, noiseSample1 * 5).x;
    vec2 noiseSample2 = uv + vec2(pseudoRandom(_Time.z * moveSpeed.x * 1.2), pseudoRandom(_Time.z * moveSpeed.y * 0.2258));
    float noise_2 = texture(PerlinNoise, noiseSample2 * 3).x;
    vec2 noiseSample3 = uv + vec2(pseudoRandom(_Time.z * moveSpeed.x * 3), pseudoRandom(_Time.z * moveSpeed.y * 2));
    float noise_3 = texture(PerlinNoise, noiseSample3 * 4).x;
    float noise = mix(noise_1, noise_2, 0.6);
    noise = mix(noise, noise_3, 0.2388);

    vec2 noiseSample_stable = uv + vec2(pseudoRandom(moveSpeed.x * 3), pseudoRandom(moveSpeed.y * 2));
    float noise_stable = texture(PerlinNoise, noiseSample_stable * 2).x;

    vec2 uvH = vec2(noise, 0.5);
    vec2 uvH1 = vec2(noise_1, 0.5);
    vec2 uvH2 = vec2(noise_2, 0.5);
    vec2 uvH3 = vec2(noise_3, 0.5);

    ///////////////////////
    ///////// water////////
    ///////////////////////
    vec4 waterCol = vec4(0, 0, 0, 0);
    float stepLength = 0.002;
    float maxStep = 20;
    float i = 0;
    float alpha = 1;
    while (alpha > 0.1) {
        i++;
        vec2 tempUV = vec2(uv.x, uv.y - stepLength * i);
        alpha = texture(WaterTex, tempUV).a;
        if (i >= maxStep) {
            break;
        }
    }
    vec2 waterColorUV = vec2(noise, 1 - i / (maxStep + 1));
    waterCol = texture(WaterColPanel, waterColorUV);
    col = mix(col, waterCol, texture(WaterTex, uv).a);

    ///////////////////////
    ///////// fire ////////
    ///////////////////////
    vec4 fireCol = vec4(0, 0, 0, 0);
    vec3 fireMask = texture(FireTex, uv).rba;
    float fireM = fireMask.x;
    float time = fireMask.y + 0.5;
    float life = fireMask.z;
    float fireAttenuation = life * time;
    fireCol = texture(FireColPanel, uvH * fireAttenuation);

    if (fireM < 0.1) {
        float right = texture(FireTex, vec2(uv.x + 1 / picRes, uv.y)).r;
        float left = texture(FireTex, vec2(uv.x - 1 / picRes, uv.y)).r;
        float up = texture(FireTex, vec2(uv.x, uv.y + 1 / picRes)).r;
        float down = texture(FireTex, vec2(uv.x, uv.y - 1 / picRes)).r;
        fireM += (right + left + up + down);
        if (fireM > 0.1) {
            fireCol = texture(FireColPanel, uvH * noise) * 0.5;
        } else {
            float rightUp = texture(FireTex, vec2(uv.x + 1 / picRes, uv.y + 1 / picRes)).r;
            float leftUp = texture(FireTex, vec2(uv.x - 1 / picRes, uv.y + 1 / picRes)).r;
            float rightDown = texture(FireTex, vec2(uv.x + 1 / picRes, uv.y - 1 / picRes)).r;
            float leftDown = texture(FireTex, vec2(uv.x - 1 / picRes, uv.y - 1 / picRes)).r;
            fireM += (rightUp + leftUp + rightDown + leftDown);
            if (fireM > 0.1) {
                fireCol = texture(FireColPanel, uvH * noise) * 0.2;
            }
        }
    }

    col = mix(col, vec4(fireCol), fireM);

    /////////////////////////
    ///////// stone /////////
    /////////////////////////
    vec4 stoneCol = vec4(0, 0, 0, 0);
    stepLength = 0.01;
    maxStep = 10;
    i = 0;
    alpha = 1;
    while (alpha > 0.1) {
        i++;
        vec2 tempUV = vec2(uv.x, uv.y - stepLength * i);
        alpha = texture(StoneTex, tempUV).a;
        if (i >= maxStep) {
            break;
        }
    }

    float uvPos = (1 - i / (maxStep + 1)) * 1.7 + 0.4;
    vec2 stoneColorUV = vec2(noise_stable * uvPos, 0.5);
    stoneCol = texture(StoneColPanel, stoneColorUV);
    col = mix(col, stoneCol, texture(StoneTex, uv).a);

    ///////////////////////////
    ///////// SandTex /////////
    ///////////////////////////
    vec4 sandCol = vec4(0, 0, 0, 0);
    float sandMask = texture(SandTex, uv).a;
    vec2 sandUV = vec2(uv.y * 25 + sin(uv.x * 10) * noise_stable, 0.5);
    sandCol = texture(SandColPanel, sandUV) * sandMask;
    col = mix(col, sandCol, sandMask);

    vec4 oilCol = texture(OilColPanel, uvH1);
    col = mix(col, oilCol, texture(OilTex, uv).a);

    vec4 acidCol = texture(AcidColPanel, uvH2);
    col = mix(col, acidCol, texture(AcidTex, uv).a);

    vec4 lavaCol = texture(FireColPanel, uvH3);
    col = mix(col, lavaCol, texture(LavaTex, uv).a);

    //smoke
    col = mix(col, vec4(0.87, 0.8, 0.82, 1) * (noise_2 - 0.1) * (noise_3), texture(SmokeTex, uv).a);

    //steam
    col = mix(col, vec4(0.77, 0.95, 0.92, 1) * noise_1, texture(SteamTex, uv).a);

    FragColor = vec4(col);
    if (col.a < 0.1) {
        discard;
    }

    //    FragColor = vec4(alpha, alpha, alpha, 1);
    //    FragColor = vec4(noise, noise, noise, 1);
    //    FragColor = vec4(i / 5, i / 5, i / 5, 1);
}