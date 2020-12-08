#version 330 core
    layout (location = 0) in vec4 vertex; //vec2 coords, vec2 uv

    out vec2 TexCoords;

    uniform mat4 model; //matrix for applying the objects transform
    uniform mat4 projection; //matrix to go from World-Space to Normalized-Device-Coord

    
    void main() 
    {
        //the uv-coords are stored in the second half of the given vertex vec4
        TexCoords = vertex.zw;

        //don't change the order in the multiplication!
        //the coords are stored in the first half of the given vertex vec4
        //gl_Position = projection * model * vec4(vertex.xy, 0.0,  1.0);
        gl_Position = projection * vec4(vertex.xy, 0.0,  1.0);

    }