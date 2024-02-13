#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec2 vUv;

#define MAX_INSTANCE 500

out vec2 fUv;
uniform float posOffset[MAX_INSTANCE*2];
uniform mat4 viewMatrix;

void main()
{
    vec2 offset = vec2(posOffset[gl_InstanceID*2], posOffset[gl_InstanceID*2+1]);    
    gl_Position = viewMatrix*vec4(vPos.xy+offset, vPos.z, 1.0);
    fUv = vUv;
}