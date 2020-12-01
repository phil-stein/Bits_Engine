#version 330 core
    layout (location = 0) in vec4 aPosition; //the position is in attrib-index '0'

    uniform mat4 projection; //matrix for camera projection
    uniform mat4 view;
    uniform mat4 model; //matrix for applying the objects transform
    
    void main() 
    {
        //don't change the order in the multiplication!
        gl_Position = projection * view * model * vec4(aPosition.xyz, 1f);
    }