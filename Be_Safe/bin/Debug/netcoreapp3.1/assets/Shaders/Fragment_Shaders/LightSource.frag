#version 330 core
    out vec4 FragColor;

    //uniform sampler2D ourTexture;
    uniform vec3 lightColor;

    void main() 
    {
        float strength = 0.5f;
        FragColor = vec4(lightColor.x + strength, lightColor.y + strength, lightColor.z + strength, 1.0);
    }