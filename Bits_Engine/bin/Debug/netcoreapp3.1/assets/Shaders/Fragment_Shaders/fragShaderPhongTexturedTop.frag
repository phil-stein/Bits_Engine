#version 330 core

    struct Material {
        sampler2D diffuse;
        sampler2D specular;
        float shininess;
        vec2 tile;
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
    in vec2 TexCoord;

    uniform Material materialOne;
    uniform Material materialTwo;
    uniform Light light;  
    uniform vec3 viewPos;

    void main() 
    {
        vec2 normTexCoords = materialOne.tile * TexCoord;

        //get surface normal and the dir the light is coming from
        vec3 norm = normalize(Normal);
        vec3 lightDir = normalize(light.position - FragPos);
        float diff = max(dot(norm, lightDir), 0.0);

        //get the angle betwee the reflected light-ray and the view-direction   
        vec3 viewDir = normalize(viewPos - FragPos);
        vec3 reflectDir = reflect(-lightDir, norm); //lightDir negated, because reflect() wants a Vec3 pointing from the light-source toward the fragment


        //materialOne_one start---------------------------------------------------------------------------------------------
        //diffuseOne----------------------------------
        //dot product between surface-normal and light-dir, the clamped to get a value between 0-1, would otherwise be neg. if the angle was greater than 90° 
        vec3 diffuseOne = light.diffuse * diff * vec3(texture(materialOne.diffuse, normTexCoords));

        //ambientOne----------------------------------
        vec3 ambientOne = light.ambient * vec3(texture(materialOne.diffuse, normTexCoords)); //light.ambientOne * materialOne.ambientOne;

        //specOneular----------------------------------

        //the shininess-value dictates how focused the spot of reflected light is
        float specOne = pow(max(dot(viewDir, reflectDir), 0.0), materialOne.shininess * 128);
        vec3 specularOne= light.specular * specOne * vec3(texture(materialOne.specular, normTexCoords)); 
        //materialOne_one end---------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------

        //materialTwo_two start-------------------------------------------------------------------------------------------
        //diffuseTwo----------------------------------
        //dot product between surface-normal and light-dir, the clamped to get a value between 0-1, would otherwise be neg. if the angle was greater than 90° 
        vec3 diffuseTwo = light.diffuse * diff * vec3(texture(materialTwo.diffuse, normTexCoords));

        //ambientTwo----------------------------------
        vec3 ambientTwo = light.ambient * vec3(texture(materialTwo.diffuse, normTexCoords)); //light.ambientTwo * materialTwo.ambientTwo;

        //specular----------------------------------

        //the shininess-value dictates how focused the spot of reflected light is
        float specTwo = pow(max(dot(viewDir, reflectDir), 0.0), materialTwo.shininess * 128);
        vec3 specularTwo = light.specular * specTwo * vec3(texture(materialTwo.specular, normTexCoords));
        //materialTwo_two end---------------------------------------------------------------------------------------------
        
        //*1.5 remaps the nomal.y value to a range on 0.0 - 1.5, instead of 0.0 - 1.0
        //because of this we can take that value to the power of 2 which will return smaller values for all values < 1.0 
        //and greater values for all values > 1.0
        //* 0.4 to get the values to a more reasonable range (0.0 - 0.99) 
        
        float materialOneStrength = pow((norm.y * 1.5), 2) * 0.44;
        float materialTwoStrength = 1 - materialOneStrength;

        //output = output_start + ((output_end - output_start) / (input_end - input_start)) * (input - input_start)
        //float output = output_start + (-output_start / input_end) * input
        //float output = 1 + (-1 / 1) * materialOneStrength
        
        vec3 ambient = (ambientOne * materialOneStrength) + (ambientTwo * materialTwoStrength);
        vec3 diffuse = (diffuseOne * materialOneStrength) + (diffuseTwo * materialTwoStrength);
        vec3 specular = (specularOne * materialOneStrength) + (specularTwo * materialTwoStrength);

        //vec3 result = (ambient + diffuse + specular); //norm.xyz; //vec3(norm.z); //<- Blue-Channel isolated
        //FragColor = vec4(vec3(materialTwoStrength), 1.0);
        FragColor = vec4((ambient + diffuse + specular), 1.0);
    }