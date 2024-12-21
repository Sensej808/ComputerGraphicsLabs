#include <GL/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <vector>
#include <array>
#include <iostream>
#include "math.h"
#define STB_IMAGE_IMPLEMENTATION
#include "stb_image.h"

#define PI  3.1415
#define deg2rad PI /180.0

GLuint Program;
// ID ��������
GLint Attrib_vertex;
GLint Attrib_color;
// ID Vertex Buffer Object
GLuint VBO;
// ID ������� ���������� �������
GLuint texture1;
GLuint texture2;

GLint unifTexture1;
GLint unifTexture2;

sf::Texture ourTexture1;
sf::Texture ourTexture2;

GLint textureHandle1;
GLint textureHandle2;

// ������������� �������
GLint Unif_reg;


const char* TEX1_NAME = "01.jpg";
const char* TEX2_NAME = "02.jpg";

const char* VertexGradientShaderSource = R"(
    #version 330 core
    layout (location = 0) in vec3 position;
    out vec2 TexCoord;
    void main() {
        gl_Position = vec4(position, 1.0f);
        TexCoord = position.xy;
}

)";

const char* FragGradientShaderSource = R"(
    #version 330 core
    uniform float reg;
    uniform sampler2D ourTexture1;
    uniform sampler2D ourTexture2;
    in vec2 TexCoord;
    out vec4 color;
    void main(){
        color = mix(texture(ourTexture1, TexCoord), texture(ourTexture2, TexCoord), 0.2f);
        color = vec4(1, t, 1, 1);
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
    // ������� ������
    Vertex3D cube[] = {
        { -0.5, -0.5, +0.5 }, { -0.5, +0.5, +0.5 }, { +0.5, +0.5, +0.5 },
        { +0.5, +0.5, +0.5 }, { +0.5, -0.5, +0.5 }, { -0.5, -0.5, +0.5 },
        { -0.5, -0.5, -0.5 }, { +0.5, +0.5, -0.5 }, { -0.5, +0.5, -0.5 },
        { +0.5, +0.5, -0.5 }, { -0.5, -0.5, -0.5 }, { +0.5, -0.5, -0.5 },

        { -0.5, +0.5, -0.5 }, { -0.5, +0.5, +0.5 }, { +0.5, +0.5, +0.5 },
        { +0.5, +0.5, +0.5 }, { +0.5, +0.5, -0.5 }, { -0.5, +0.5, -0.5 },
        { -0.5, -0.5, -0.5 }, { +0.5, -0.5, +0.5 }, { -0.5, -0.5, +0.5 },
        { +0.5, -0.5, +0.5 }, { -0.5, -0.5, -0.5 }, { +0.5, -0.5, -0.5 },

        { +0.5, -0.5, -0.5 }, { +0.5, -0.5, +0.5 }, { +0.5, +0.5, +0.5 },
        { +0.5, +0.5, +0.5 }, { +0.5, +0.5, -0.5 }, { +0.5, -0.5, -0.5 },
        { -0.5, -0.5, -0.5 }, { -0.5, +0.5, +0.5 }, { -0.5, -0.5, +0.5 },
        { -0.5, +0.5, +0.5 }, { -0.5, -0.5, -0.5 }, { -0.5, +0.5, -0.5 },
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
    glBufferData(GL_ARRAY_BUFFER, sizeof(cube), cube, GL_STATIC_DRAW);
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
    const char* attr_name = "position"; //��� � �������
    Attrib_vertex = glGetAttribLocation(Program, attr_name);
    if (Attrib_vertex == -1) {
        std::cout << "could not bind attrib " << attr_name << std::endl;
        return;
    }

   
    unifTexture1 = glGetUniformLocation(Program, "ourTexture1");
    if (unifTexture1 == -1)
    {
        std::cout << "could not bind uniform ourTexture1" << std::endl;
        return;
    }

    unifTexture2 = glGetUniformLocation(Program, "ourTexture2");
    if (unifTexture2 == -1)
    {
        std::cout << "could not bind uniform ourTexture2" << std::endl;
        return;
    }

    
    
    checkOpenGLerror();
}


// ������� ��������������� ��������� �����
void Draw() { //���
    glUseProgram(Program);

    // ���������� ���������� ���� 0, ������ ����� �� �����������, �� ���������
    // � ��� ����������� GL_TEXTURE0, ��� ����� ��� ������������� ���������� �������
    glActiveTexture(GL_TEXTURE0);
    // ������ SFML �� opengl �������� glBindTexture
    sf::Texture::bind(&ourTexture1);
    // � uniform ������� ���������� ������ ����������� ����� (��� GL_TEXTURE0 - 0, ��� GL_TEXTURE1 - 1 � ��)
    glUniform1i(unifTexture1, 0);


    glActiveTexture(GL_TEXTURE1);
    // ������ SFML �� opengl �������� glBindTexture
    sf::Texture::bind(&ourTexture2);
    // � uniform ������� ���������� ������ ����������� ����� (��� GL_TEXTURE0 - 0, ��� GL_TEXTURE1 - 1 � ��)
    glUniform1i(unifTexture2, 1); //�������� 2


    glBindBuffer(GL_ARRAY_BUFFER, 0); // ��������� 
    glDrawArrays(GL_TRIANGLES, 0, 36); // ������

    glDisableVertexAttribArray(Attrib_vertex);

    glUseProgram(0); // ��������� ��������� ���������
    checkOpenGLerror();
}


//float vertex[] = { -1, -1, 0, 1, -1 , 0, 1, 1, 0, -1, 1, 0 };
//float texCoord[] = { 0,0,1,0,1,1,0,1 };
//
//void GH()
//{
//    glEnable(GL_TEXTURE_2D);
//    glBindTexture(GL_TEXTURE_2D, texture1);
//    glColor3f(1, 1, 1);
//    glPushMatrix();
//    glEnableClientState(GL_VERTEX_ARRAY);
//    glEnableClientState(GL_TEXTURE_COORD_ARRAY);
//
//    glVertexPointer(3, GL_FLOAT, 0, vertex);
//    glTexCoordPointer(2, GL_FLOAT, 0, texCoord);
//    glDrawArrays(GL_TRIANGLE_FAN, 0, 4);
//
//    glDisableClientState(GL_VERTEX_ARRAY);
//    glDisableClientState(GL_TEXTURE_COORD_ARRAY);
//    glPopMatrix();
//}

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

void LoadTextures() {
    int h, w, cnt;
    unsigned char* data = stbi_load(TEX1_NAME, &w, &h, &cnt, 0);

    /*struct { unsigned char r, g, b, a; } data[2][2];
    memset(data, 0, sizeof(data));
    
    data[0][0].r = 255;
    data[1][0].g = 255;
    data[1][1].b = 255;
    data[0][1].r = 255;
    data[0][1].g = 255;

    glGenTextures(1, &texture1);
    glBindTexture(GL_TEXTURE_2D, texture1);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, 2, 2, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
    glBindTexture(GL_TEXTURE_2D, 0);*/

    glGenTextures(1, &texture1);
    glBindTexture(GL_TEXTURE_2D, texture1);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, w, h, 0, GL_RGB, GL_UNSIGNED_BYTE, data);
    glBindTexture(GL_TEXTURE_2D, 0);
    stbi_image_free(data);

    data = stbi_load(TEX2_NAME, &w, &h, &cnt, 0);
    glGenTextures(1, &texture2);
    glBindTexture(GL_TEXTURE_2D, texture2);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, w, h, 0, GL_RGB, GL_UNSIGNED_BYTE, data);
    glBindTexture(GL_TEXTURE_2D, 0);
    stbi_image_free(data);
}

void InitTextures()
{
    // ��������� �������� �� �����
    if (!ourTexture1.loadFromFile(TEX1_NAME))
    {
        // �� ����� ��������� ��������
        return;
    }
    // ������ �������� openGL ���������� ��������
    textureHandle1 = ourTexture1.getNativeHandle();

    if (!ourTexture2.loadFromFile(TEX2_NAME))
    {
        // �� ����� ��������� ��������
        return;
    }
    // ������ �������� openGL ���������� ��������
    textureHandle2 = ourTexture2.getNativeHandle();

}
// � ������ ������������� ������� ���������� �������� �������, ������� � ������ �����
void Init() {

    // �������
    InitShader();
    // ��������� �����
    InitVBO();

    InitTextures();
    //LoadTextures();
    glEnable(GL_DEPTH_TEST);
}

int main() {
    sf::Window window(sf::VideoMode(600, 600), "My OpenGL window", sf::Style::Default, sf::ContextSettings(24));
    window.setVerticalSyncEnabled(true);
    window.setActive(true);
    glewInit();
    Init();
    while (window.isOpen()) {
        glClearColor(0, 1, 1, 1);
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