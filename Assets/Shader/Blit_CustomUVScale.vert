#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec2 vUv;

out vec2 fUv;
uniform float _UVScale;
void main()
{
    //Multiplying our uniform with the vertex position, the multiplication order here does matter.
    gl_Position = vec4(vPos.x, vPos.y, vPos.z, 1.0);
    fUv = _UVScale *vUv;
}