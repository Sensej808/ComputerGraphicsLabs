#include <GL/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <vector>
#include <array>
#include <iostream>
#include "math.h"

#define PI  3.1415
#define deg2rad PI /180.0

GLuint Program;
// ID ��������
GLint Attrib_vertex;
GLint Attrib_color;
// ID Vertex Buffer Object
GLuint VBO;
GLuint VBO_color;
// ID ������� ���������� �����
GLint Unif_xangle;
GLint Unif_yangle;
GLint Unif_zangle;

const char* VertexGradientShaderSource = R"(
    #version 330 core
    in vec3 coord;
    in vec4 color;
    out vec3 vPosition;
    out vec4 vert_color;
    uniform float x_offset;
    uniform float y_offset;
    uniform float z_offset;
    void main() {
        vPosition = coord;
        vec3 position = coord + vec3(x_offset, y_offset, z_offset);
        gl_Position = vec4(position, 1.0);
        vert_color=color;
    }
)";

const char* FragGradientShaderSource = R"(
    #version 330 core
    in vec4 vert_color;
    out vec4 color;
    void main() {
         color=vert_color;
    }
)";



const char* CURRENT_VERTEX_SHADER = VertexGradientShaderSource;
const char* CURRENT_FRAGMENT_SHADER = FragGradientShaderSource;

GLfloat x_offset = 0;
GLfloat y_offset = 0;
GLfloat z_offset = 0;

struct Vertex3D {
    GLfloat x;
    GLfloat y;
    GLfloat z;
};

void checkOpenGLerror() {
    GLenum errCode;
    if ((errCode = glGetError()) != GL_NO_ERROR)
        std::cout << "OpenGl error!: " << errCode << std::endl;
}

void move(GLfloat dx, GLfloat dy, GLfloat dz) {
    x_offset += dx;
    y_offset += dy;
    z_offset += dz;
}

void ShaderLog(unsigned int shader)
{
    int infologLen = 0;
    glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
    if (infologLen > 1)
    {
        int charsWritten = 0;
        std::vector<char> infoLog(infologLen);
        glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog.data());
        std::cout << "InfoLog: " << infoLog.data() << std::endl;
    }
}


void InitVBO() {
    glGenBuffers(1, &VBO);
    glGenBuffers(1, &VBO_color);
    // ������� ������
    Vertex3D tetra[] = {
       { 0.2, 0.45, -0.5 }, { -0.6, 0, -0.5 }, { 0, 0, 0.5 },
       { -0.6, 0, -0.5 }, { 0, 0, 0.5 }, { 0.2, -0.45, -0.5 },
       { 0, 0, 0.5 }, { 0.2, 0.45, -0.5 }, { 0.2, -0.45, -0.5 },

    };
    float colors[9][4] = {
        { 0.0, 0.0, 1.0, 1.0 }, { 1.0, 0.0, 0.0, 1.0 }, { 1.0, 1.0, 1.0, 1.0 },
        { 1.0, 0.0, 0.0, 1.0 }, { 1.0, 1.0, 1.0, 1.0 }, { 0.0, 1.0, 0.0, 1.0 },
        { 1.0, 1.0, 1.0, 1.0 }, { 0.0, 0.0, 1.0, 1.0 }, { 0.0, 1.0, 0.0, 1.0 },
    };
    
    //colors-������ ������ ��� ��������������� ������
    //float colors[36][4];


    /*for (int i = 0; i < 36; i++)
    {
        colors[i][0] = triangle[i].x == -0.5 ? 0.0 : 1.0;
        colors[i][1] = triangle[i].y == -0.5 ? 0.0 : 1.0;
        colors[i][2] = triangle[i].z == -0.5 ? 0.0 : 1.0;
        colors[i][3] = 1.0;

    }*/

    // �������� ������� � �����
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(tetra), tetra, GL_STATIC_DRAW);
    // �������� ����� ������ � �����
    glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
    glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);
    checkOpenGLerror();
}


void InitShader() {
    // ������� ��������� ������
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    // �������� �������� ���
    glShaderSource(vShader, 1, &CURRENT_VERTEX_SHADER, NULL);
    // ����������� ������
    glCompileShader(vShader);
    std::cout << "vertex shader \n";
    // ������� ������ ���� �������
    ShaderLog(vShader); //������ ������� ���� � ������������
    // ������� ����������� ������
    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
    // �������� �������� ���
    glShaderSource(fShader, 1, &CURRENT_FRAGMENT_SHADER, NULL);
    // ����������� ������
    glCompileShader(fShader);
    std::cout << "fragment shader \n";
    // ������� ������ ���� �������
    ShaderLog(fShader);
    // ������� ��������� � ����������� ������� � ���
    Program = glCreateProgram();
    glAttachShader(Program, vShader);
    glAttachShader(Program, fShader);
    // ������� ��������� ���������
    glLinkProgram(Program);
    // ��������� ������ ������
    int link_ok;
    glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
    if (!link_ok) {
        std::cout << "error attach shaders \n";
        return;
    }
    // ���������� ID �������� �� ��������� ���������
    const char* attr_name = "coord"; //��� � �������
    Attrib_vertex = glGetAttribLocation(Program, attr_name);
    if (Attrib_vertex == -1) {
        std::cout << "could not bind attrib " << attr_name << std::endl;
        return;
    }

    // ���������� ID �������� �����
    Attrib_color = glGetAttribLocation(Program, "color");
    if (Attrib_color == -1)
    {
        std::cout << "could not bind attrib color" << std::endl;
        return;
    }
    // ���������� ID ������� ���� �������� �� ox
    const char* unif_name = "x_offset";
    Unif_xangle = glGetUniformLocation(Program, unif_name);
    if (Attrib_color == -1)
    {
        std::cout << "could not bind uniform " << unif_name << std::endl;
        return;
    }
    // ���������� ID ������� ���� �������� �� oy
    unif_name = "y_offset";
    Unif_yangle = glGetUniformLocation(Program, unif_name);
    if (Attrib_color == -1)
    {
        std::cout << "could not bind uniform " << unif_name << std::endl;
        return;
    }
    // ���������� ID ������� ���� �������� �� oz
    unif_name = "z_offset";
    Unif_zangle = glGetUniformLocation(Program, unif_name);
    if (Attrib_color == -1)
    {
        std::cout << "could not bind uniform " << unif_name << std::endl;
        return;
    }
    checkOpenGLerror();
}


// ������� ��������������� ��������� �����
void Draw() { //���
    glUseProgram(Program); // ������������� ��������� ��������� �������

    glUniform1f(Unif_xangle, x_offset);
    glUniform1f(Unif_yangle, y_offset);
    glUniform1f(Unif_zangle, z_offset);

    glEnableVertexAttribArray(Attrib_vertex); // �������� ������ ���������
    glEnableVertexAttribArray(Attrib_color);
    glBindBuffer(GL_ARRAY_BUFFER, VBO); // ���������� VBO
    // ���������, ��� � ���� ������ ������ �� VBO
    glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 0, 0);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
    glVertexAttribPointer(Attrib_color, 4, GL_FLOAT, GL_FALSE, 0, 0);

    glBindBuffer(GL_ARRAY_BUFFER, 0); // ��������� 
    glDrawArrays(GL_TRIANGLES, 0, 36); // ������

    glDisableVertexAttribArray(Attrib_vertex);
    glDisableVertexAttribArray(Attrib_color);

    glUseProgram(0); // ��������� ��������� ���������
    checkOpenGLerror();
}



// ������������ ��������
void ReleaseShader() {
    // ��������� ����, �� ��������� ��������� ���������
    glUseProgram(0);
    // ������� ��������� ���������
    glDeleteProgram(Program);
}


void ReleaseVBO() {
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glDeleteBuffers(1, &VBO);
}

void Release() {

    // �������
    ReleaseShader();
    // ��������� �����
    ReleaseVBO();
}


void SetIcon(sf::Window& wnd)
{
    sf::Image image;

    // ������ ��������� ��������, ����� ��������� ������ �� ����� (image.loadFromFile("icon.png"))
    image.create(16, 16);
    for (int i = 0; i < 16; ++i)
        for (int j = 0; j < 16; ++j)
            image.setPixel(i, j, {
                (sf::Uint8)(i * 16), (sf::Uint8)(j * 16), 0 });

    wnd.setIcon(image.getSize().x, image.getSize().y, image.getPixelsPtr());
}

// � ������ ������������� ������� ���������� �������� �������, ������� � ������ �����
void Init() {

    // �������
    InitShader();
    // ��������� �����
    InitVBO();

    glEnable(GL_DEPTH_TEST);
}

int main() {
    sf::Window window(sf::VideoMode(600, 600), "My OpenGL window", sf::Style::Default, sf::ContextSettings(24));
    window.setVerticalSyncEnabled(true);
    window.setActive(true);
    glewInit();
    Init();
    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) { window.close(); }
            else if (event.type == sf::Event::Resized) { glViewport(0, 0, event.size.width, event.size.height); }

            else if (event.type == sf::Event::KeyPressed) {
                switch (event.key.code) {
                case (sf::Keyboard::D): move(0.01, 0, 0); break;
                case (sf::Keyboard::A): move(-0.01, 0, 0); break;
                case (sf::Keyboard::W):move(0, 0.01, 0); break;
                case (sf::Keyboard::S): move(0, -0.01, 0); break;
                case (sf::Keyboard::Q): move(0, 0, -0.01); break;
                case (sf::Keyboard::E): move(0, 0, 0.01); break;
                default: break;
                }
            }
        }
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        Draw();
        window.display();
    }
    Release();
    return 0;
}