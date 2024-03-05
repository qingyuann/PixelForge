#version 330 core

in vec2 fUv;
out vec4 FragColor;

uniform sampler2D _BlitTexture;

uniform float intensity;
uniform vec3 color;
uniform float merge; // 0:multiply,1:soft light

void main(void) {
    vec4 B = texture(_BlitTexture, fUv);
    if (B.a < 0.1) {
        discard;
    }
    vec3 A = vec3(color);

    if (merge < 0.5) {
        FragColor = vec4(B.xyz * A, B.w);
    } else {
        vec4 result = vec4(1);
        if (B.x < 0.5) {
            result.x = A.x * B.x / 0.5 + A.x * A.x * (1.0 - 2 * B.x);
        } else {
            result.x = A.x * (1.0 - B.x) / 0.5 + sqrt(A.x) * (2 * B.x - 1);
        }
        if (B.y < 0.5) {
            result.y = A.y * B.y / 0.5 + A.y * A.y * (1.0 - 2 * B.y);
        } else {
            result.y = A.y * (1.0 - B.y) / 0.5 + sqrt(A.y) * (2 * B.y - 1);
        }
        if (B.z < 0.5) {
            result.z = A.z * B.z / 0.5 + A.z * A.z * (1.0 - 2 * B.z);
        } else {
            result.z = A.z * (1.0 - B.z) / 0.5 + sqrt(A.z) * (2 * B.z - 1);
        }
        FragColor = result;
    }
    FragColor = mix(B, FragColor, intensity);
}