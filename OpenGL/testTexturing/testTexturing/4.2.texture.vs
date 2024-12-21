#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec2 aTexCoord;

uniform float x_angle;
uniform float y_angle;
uniform float z_angle;


out vec3 ourColor;
out vec2 TexCoord;

void main()
{

	vec3 position = aPos * mat3(
   1, 0, 0,
    0, cos(x_angle), -sin(x_angle),
    0, sin(x_angle), cos(x_angle)
) * mat3(
    cos(y_angle), 0, sin(y_angle),
    0, 1, 0,
    -sin(y_angle), 0, cos(y_angle)
)  * mat3(
    cos(z_angle), sin(z_angle),0,
    -sin(z_angle),cos(z_angle) , 0,
    0, 0, 1
);
	gl_Position = vec4(position, 1.0);
	ourColor = aColor;
	TexCoord = vec2(aTexCoord.x, aTexCoord.y);
}