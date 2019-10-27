#version 130

in vec2 position;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

void main()
{
	gl_Position = projectionMatrix * viewMatrix * vec4(position, 0.0, 1.0);
}
