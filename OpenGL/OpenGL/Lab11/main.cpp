#include <GL/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <vector>
#include <iostream>

GLuint Program;
// ID ��������
GLint Attrib_vertex;
// ID Vertex Buffer Object
GLuint VBO;
GLuint Color_VBO;

const char* VertexShaderSource = R"(
 #version 330 core
 in vec2 coord;
 void main() {
 gl_Position = vec4(coord, 0.0, 1.0);
 }
)";

const char* VertexGradientShaderSource = R"(
    #version 330 core
    attribute vec3 col;
    in vec2 coord;
    out vec3 fragColor;
    void main() {
        gl_Position = vec4(coord, 0.0, 1.0);
        fragColor = col;
    }
)";

const char* FragGradientShaderSource = R"(
    #version 330 core
    in vec3 fragColor;
    out vec4 color;
    void main() {
        color = vec4(fragColor, 1.0);
    }
)";

const char* FragUniformShaderSource = R"(
    #version 330 core
    out vec4 color;
    uniform vec4 u_color;
    void main() {
        color = u_color;
    }
)";

const char* FragShaderSource = R"(
 #version 330 core
 out vec4 color;
 const vec4 const_color = vec4(1, 0, 0, 1);
 void main() {
 color = const_color;
 }
)";

float colors[] = {
     1.0f, 0.0f, 0.0f,  // �������
     0.0f, 1.0f, 0.0f,  // �������
     0.0f, 0.0f, 1.0f,  // �����
     1.0f, 1.0f, 0.0f,   // ������
     1.0f, 0.0f, 1.0f,   // ����������

};


const char* CURRENT_VERTEX_SHADER = VertexGradientShaderSource;
const char* CURRENT_FRAGMENT_SHADER = FragGradientShaderSource;
enum Task {CONSTANT_COLOR, UNIFORM, GRADIENT};
const Task cur = GRADIENT;
const int verteces = 5;

struct Vertex {
    GLfloat x;
    GLfloat y;
};

void checkOpenGLerror() {

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


void InitVBO(Vertex* verts, int size) {
    glGenBuffers(1, &VBO);
    // ������� ������ ������������

    // �������� ������� � �����
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, size, verts, GL_STATIC_DRAW);

    glGenBuffers(1, &Color_VBO);
    glBindBuffer(GL_ARRAY_BUFFER, Color_VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);
    checkOpenGLerror(); //������ ������� ���� � ������������
    // �������� ������ OpenGL, ���� ����, �� ����� � ������� ��� ������
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
    checkOpenGLerror();
}


// ������� ��������������� ��������� �����
void Draw() {
    glUseProgram(Program); // ������������� ��������� ��������� �������
    int c = glGetAttribLocation(Program, "col");
    switch (cur)
    {
    case CONSTANT_COLOR:
        glEnableVertexAttribArray(Attrib_vertex); // �������� ������ ���������
        glBindBuffer(GL_ARRAY_BUFFER, VBO); // ���������� VBO
        // �������� OpenGL ��� �� ������ ���������������� ��������� ������.
        glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, 0, 0);
        glBindBuffer(GL_ARRAY_BUFFER, 0); // ��������� VBO
        break;
    case UNIFORM:
    {
        glEnableVertexAttribArray(Attrib_vertex); // �������� ������ ���������
        glBindBuffer(GL_ARRAY_BUFFER, VBO); // ���������� VBO
        // �������� OpenGL ��� �� ������ ���������������� ��������� ������.
        glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, 0, 0);
        int u = glGetUniformLocation(Program, "u_color");
        glUniform4f(u, 0, 1, 0.5, 1);
        glBindBuffer(GL_ARRAY_BUFFER, 0); // ��������� VBO
        
        break;
    }
    case GRADIENT:
    {
        glEnableVertexAttribArray(Attrib_vertex); // �������� ������ ���������
        glBindBuffer(GL_ARRAY_BUFFER, VBO); // ���������� VBO
        // �������� OpenGL ��� �� ������ ���������������� ��������� ������.
        glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, 0, 0);
        glBindBuffer(GL_ARRAY_BUFFER, Color_VBO);
        glVertexAttribPointer(c, 3, GL_FLOAT, GL_FALSE, 0, 0);
        glBindBuffer(GL_ARRAY_BUFFER, 0); // ��������� VBO
        glEnableVertexAttribArray(c);
        break;
    }
        
    default:
        break;
    }
    
    glDrawArrays(GL_POLYGON, 0, 5); // �������� ������ �� ����������(������)

    glDisableVertexAttribArray(Attrib_vertex); // ��������� ������ ���������
    glDisableVertexAttribArray(c);
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
    Vertex triangle[3] = {
{ -1.0f, -1.0f },
{ 0.0f, 1.0f },
{ 1.0f, -1.0f }
    };

    Vertex square[4] = {
    { -0.9f, -0.9f },
    { 0.9f, -0.9f },
    { 0.9f, 0.9f },
    { -0.9f, 0.9f },
    };

    Vertex pentagon[5] = {
    { 0.0f, 1.0f },
    { -0.951f, 0.309f },
    { -0.588f, -0.809f },
    { 0.588f, -0.809f },
    { 0.951f, 0.309f },
    };
    
    // �������
    InitShader();
    // ��������� �����
    InitVBO(pentagon, sizeof(pentagon));
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
        }
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        Draw();
        window.display();
    }
    Release();
    return 0;
}