#version 330 core
#define PI 3.14
//https://github.com/mattdesl/lwjgl-basics/wiki/2D-Pixel-Perfect-Shadows
//inputs from vertex shader
in vec2 fUv;

//uniform values
uniform sampler2D _BlitTexture;
uniform float lightRadiusResolution;
uniform float ratioScale;

//alpha threshold for our occlusion map
const float THRESHOLD = 0.75;
out vec4 FragColor;

void main(void) {
    float d = 1.0;

    for (float y = 0.0; y < lightRadiusResolution; y += 1.0) {
        //rectangular to polar filter
        float angle = fUv.x * PI * 2.0; //0,2pi
        float radius = y / lightRadiusResolution; //0,1

        //coord which we will sample from occlude map
        //r*[(-1,1),(-1,1) -> (0,1),(0,1)]
        vec2 coord=vec2(0.0);
        if (ratioScale > 0) {
            coord = radius * vec2(sin(angle), cos(angle) * ratioScale) / 2.0 + 0.5;
        }else{
            coord = radius * vec2(sin(angle)/ratioScale, cos(angle)) / 2.0 + 0.5;
        }
        
        //sample the occlusion map
        vec4 data = texture(_BlitTexture, coord);

        //the current distance is how far from the top we've come

        //if we've hit an opaque fragment (occluder), then get new distance
        //if the new distance is below the current, then we'll use that for our ray
        if (data.a > THRESHOLD) {
            d = radius;
            break;
        }
    }
    FragColor = vec4(vec3(d), 1.0);
    //    FragColor = vec4(vec3(fUv.x), 1.0);
}