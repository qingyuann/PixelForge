#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec2 vUv;

out vec2 fUv;
uniform mat4 viewMatrix;

void main()
{   
    gl_Position = viewMatrix*vec4(vPos.x, vPos.y, vPos.z, 1.0);
    fUv = vUv;
}