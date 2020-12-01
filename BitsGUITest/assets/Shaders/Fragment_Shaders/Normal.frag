#version 330 core
    out vec4 FragColor;

    //in vec4 vertexColor;
    //in vec2 TexCoord;
    //in vec3 objectColor;
    in vec3 Normal;

    //uniform sampler2D ourTexture;
    //uniform vec3 objectColor;
    //uniform vec3 lightColor;

    void main() 
    {
        //FragColor = vec4(lightColor * objectColor, 1.0);
        FragColor = vec4(Normal.xyz, 1.0);
        //FragColor = vertexColor * texture(ourTexture, TexCoord);
        //FragColor = texture(ourTexture, TexCoord);
    }