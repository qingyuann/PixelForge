#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec2 vUv;

#define MAX_INSTANCE 50

out vec2 fUv;

uniform float posOffset[MAX_INSTANCE*3];
uniform float scaleOffset[MAX_INSTANCE*2];
uniform float rotationOffset[MAX_INSTANCE];

uniform mat4 viewMatrix;

void main()
{

    vec3 pos= vec3(vPos.x*scaleOffset[gl_InstanceID*2],vPos.y*scaleOffset[gl_InstanceID*2+1],vPos.z);
    float angle = rotationOffset[gl_InstanceID];
    float s = sin(angle*3.14159265/180.0);
    float c = cos(angle*3.14159265/180.0);
    mat3 rotationMatrix = mat3(c, -s, 0,
                                s, c, 0,
                                0, 0, 1);
    pos = rotationMatrix*pos;
    vec3 offset = vec3(posOffset[gl_InstanceID*3], posOffset[gl_InstanceID*3+1], posOffset[gl_InstanceID*3+2]);
    gl_Position = viewMatrix*vec4(pos.xyz+offset.xyz, 1.0);
    fUv = vUv;
}