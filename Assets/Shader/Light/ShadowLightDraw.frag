#version 330 core
//inpute screen rt and check if the pixel is in the shadow
#define PI 3.14159265359
//https://github.com/mattdesl/lwjgl-basics/wiki/2D-Pixel-Perfect-Shadows
//inputs from vertex shader
in vec2 fUv;
out vec4 FragColor;


uniform sampler2D _ShadowMap;

uniform vec2 lightPosPix;
uniform float screenW;
uniform float screenH;
uniform float lightRadiusPix;
uniform vec3 lightColor;
uniform float falloff;
uniform float intensity;
uniform float volumeIntensity;
uniform float edgeInfringe;

float angleBetweenVectors(vec2 v1, vec2 v2) {
    float dotProduct = dot(normalize(v1), normalize(v2));
    float angle = acos(clamp(dotProduct, -1.0, 1.0));

    if (cross(vec3(v1, 0.0), vec3(v2, 0.0)).z > 0.0) {
        angle = 2.0 * PI - angle;
    }

    return angle;
}

void main(void) {
    vec2 uvPosPix = vec2(fUv.x * screenW, fUv.y * screenH);
    float dist = distance(uvPosPix, lightPosPix);

    //if the pixel is outside the light radius, return the screen color
    if (dist > lightRadiusPix) {
        FragColor = vec4(0.0, 0.0, 0.0, 0.0);
        return;
    }

    vec2 direction = uvPosPix - lightPosPix;
    float angle = angleBetweenVectors(vec2(0.0, 1.0), direction);
    float angleNorm = angle / (2.0 * PI);

    float shadow = texture(_ShadowMap, vec2(angleNorm, 0.5)).r;

    float radialFalloff = pow(1 - dist / lightRadiusPix, falloff);
    if (dist > shadow * lightRadiusPix) {
        float distToEdge = dist - shadow * lightRadiusPix;
        if (distToEdge < edgeInfringe) {
            vec3 cor = lightColor.xyz * radialFalloff * intensity * (1 - distToEdge / edgeInfringe);
            FragColor = vec4(cor, 0.3);
            return;
        }
        else {
            FragColor = vec4(0.0, 0.0, 0.0, 0.0);
            return;
        }
        return;
    }
    else {
        vec3 cor = radialFalloff * lightColor * intensity * volumeIntensity;
        FragColor = vec4(cor, 0.3);

        return;
    }
}
//sample from the 1D distance map
