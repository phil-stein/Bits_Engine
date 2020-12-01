#version 330 core

    struct Material {
        vec3 ambient;
        vec3 diffuse;
        vec3 specular;
        float shininess;
    };

    struct Light {
        vec3 position;
  
        vec3 ambient;
        vec3 diffuse;
        vec3 specular;
    };

  
    out vec4 FragColor;

    in vec3 Normal;
    in vec3 FragPos;
    in vec4 vertexColor;
    //in vec2 TexCoord;

    //uniform sampler2D ourTexture;
    uniform Material material;
    uniform Light light;  
    uniform vec3 viewPos;

    void main() 
    {
        //ambient----------------------------------
        vec3 ambient = light.ambient * material.ambient;

        //diffuse----------------------------------
        //get surface normal and the dir the light is coming from
        vec3 norm = normalize(Normal);
        vec3 lightDir = normalize(light.position - FragPos);

        //dot product between surface-normal and light-dir, the clamped to get a value between 0-1, would otherwise be neg. if the angle was greater than 90° 
        float diff = max(dot(norm, lightDir), 0.0);
        vec3 diffuse = light.diffuse * (diff * ((material.diffuse.x + material.diffuse.y + material.diffuse.z) * .33));

        //specular----------------------------------
        //get the angle betwee the reflected light-ray and the view-direction        
        vec3 viewDir = normalize(viewPos - FragPos);
        vec3 reflectDir = reflect(-lightDir, norm); //lightDir negated, because reflect() wants a Vec3 pointing from the light-source toward the fragment

        //the shininess-value dictates how focused the spot of reflected light is
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess * 128);
        vec3 specular = light.specular * (spec * ((material.specular.x + material.specular.y + material.specular.z) * .33)); 

        vec3 height = vec3(.1+(FragPos.y*.001), .1+(FragPos.y*.001), .1+(FragPos.y*.001));
        
        //if(Normal.y * 21 > 1 ) { height.y *= 10; } //use for grass etc.
        float near = 0;
        float n = pow(norm.y * 1, 12);
        float nInv = (n-1) * -1;
        if( FragPos.y > -3) 
        { 
            //near = ((-1.4 + FragPos.y) +.5) * -1;
            height.x *= (1.1 * n) + (nInv * .8); 
            height.y *= (1.2 * n) + (nInv * .4); 
            height.z = .01;
        }


        if(FragPos.y < -2) { height.z *= max(pow((FragPos.y *-1)-1.2, 5) *.1, 1); }

        vec3 result = (height + diffuse + specular); //norm.xyz; //vec3(norm.z); //<- Blue-Channel isolated
        FragColor = vec4(result, 1.0);
    }