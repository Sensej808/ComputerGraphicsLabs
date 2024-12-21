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
// ID атрибута
GLint Attrib_vertex;
GLint Attrib_color;
// ID Vertex Buffer Object
GLuint VBO;
GLuint VBO_color;
// ID юниформ переменной цвета
GLint Unif_xangle;
GLint Unif_yangle;
GLint Unif_zangle;

const char* VertexGradientShaderSource = R"(
    #version 330 core
    in vec3 coord;
    in vec4 color;
    out vec3 vPosition;
    out vec4 vert_color;
    uniform float x_angle;
    uniform float y_angle;
    uniform float z_angle;
    void main() {
        vPosition = coord;
        vec3 position = coord * mat3(
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

GLfloat x_angle = 1.0;
GLfloat y_angle = -1.0;
GLfloat z_angle = 1.0;

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

void rotate(GLfloat dx, GLfloat dy, GLfloat dz) {
    x_angle += dx;
    y_angle += dy;
    z_angle += dz;
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
    // Вершины кубика
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
    //colors-массив цветов для соответствующих вершин
    float colors[36][4];


    for (int i = 0; i < 36; i++)
    {
        colors[i][0] = cube[i].x == -0.5 ? 0.0 : 1.0;
        colors[i][1] = cube[i].y == -0.5 ? 0.0 : 1.0;
        colors[i][2] = cube[i].z == -0.5 ? 0.0 : 1.0;
        colors[i][3] = 1.0;

    }
   
     // Передаем вершины в буфер
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(cube), cube, GL_STATIC_DRAW);
    // Передаем цвета вершин в буфер
    glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
    glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);
    checkOpenGLerror();
}


void InitShader() {
    // Создаем вершинный шейдер
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    // Передаем исходный код
    glShaderSource(vShader, 1, &CURRENT_VERTEX_SHADER, NULL);
    // Компилируем шейдер
    glCompileShader(vShader);
    std::cout << "vertex shader \n";
    // Функция печати лога шейдера
    ShaderLog(vShader); //Пример функции есть в лабораторной
    // Создаем фрагментный шейдер
    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
    // Передаем исходный код
    glShaderSource(fShader, 1, &CURRENT_FRAGMENT_SHADER, NULL);
    // Компилируем шейдер
    glCompileShader(fShader);
    std::cout << "fragment shader \n";
    // Функция печати лога шейдера
    ShaderLog(fShader);
    // Создаем программу и прикрепляем шейдеры к ней
    Program = glCreateProgram();
    glAttachShader(Program, vShader);
    glAttachShader(Program, fShader);
    // Линкуем шейдерную программу
    glLinkProgram(Program);
    // Проверяем статус сборки
    int link_ok;
    glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
    if (!link_ok) {
        std::cout << "error attach shaders \n";
        return;
    }
    // Вытягиваем ID атрибута из собранной программы
    const char* attr_name = "coord"; //имя в шейдере
    Attrib_vertex = glGetAttribLocation(Program, attr_name);
    if (Attrib_vertex == -1) {
        std::cout << "could not bind attrib " << attr_name << std::endl;
        return;
    }

    // Вытягиваем ID атрибута цвета
    Attrib_color = glGetAttribLocation(Program, "color");
    if (Attrib_color == -1)
    {
        std::cout << "could not bind attrib color" << std::endl;
        return;
    }
    // Вытягиваем ID юниформ угла поворота по ox
    const char* unif_name = "x_angle";
    Unif_xangle = glGetUniformLocation(Program, unif_name);
    if (Attrib_color == -1)
    {
        std::cout << "could not bind uniform " << unif_name << std::endl;
        return;
    }
    // Вытягиваем ID юниформ угла поворота по oy
    unif_name = "y_angle";
    Unif_yangle = glGetUniformLocation(Program, unif_name);
    if (Attrib_color == -1)
    {
        std::cout << "could not bind uniform " << unif_name << std::endl;
        return;
    }
    // Вытягиваем ID юниформ угла поворота по oz
    unif_name = "z_angle";
    Unif_zangle = glGetUniformLocation(Program, unif_name);
    if (Attrib_color == -1)
    {
        std::cout << "could not bind uniform " << unif_name << std::endl;
        return;
    }
    checkOpenGLerror();
}


// Функция непосредственно отрисовки сцены
void Draw() { //куб
    glUseProgram(Program); // Устанавливаем шейдерную программу текущей

    glUniform1f(Unif_xangle, x_angle);
    glUniform1f(Unif_yangle, y_angle);
    glUniform1f(Unif_zangle, z_angle);

    glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов
    glEnableVertexAttribArray(Attrib_color);
    glBindBuffer(GL_ARRAY_BUFFER, VBO); // Подключаем VBO
    // Указываем, как и куда читаем данные из VBO
    glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 0, 0);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
    glVertexAttribPointer(Attrib_color, 4, GL_FLOAT, GL_FALSE, 0, 0);

    glBindBuffer(GL_ARRAY_BUFFER, 0); // Отключаем 
    glDrawArrays(GL_TRIANGLES, 0, 36); // Рисуем

    glDisableVertexAttribArray(Attrib_vertex);
    glDisableVertexAttribArray(Attrib_color);

    glUseProgram(0); // Отключаем шейдерную программу
    checkOpenGLerror();
}



// Освобождение шейдеров
void ReleaseShader() {
    // Передавая ноль, мы отключаем шейдерную программу
    glUseProgram(0);
    // Удаляем шейдерную программу
    glDeleteProgram(Program);
}


void ReleaseVBO() {
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glDeleteBuffers(1, &VBO);
}

void Release() {

    // Шейдеры
    ReleaseShader();
    // Вершинный буфер
    ReleaseVBO();
}


void SetIcon(sf::Window& wnd)
{
    sf::Image image;

    // Вместо рисования пикселей, можно загрузить иконку из файла (image.loadFromFile("icon.png"))
    image.create(16, 16);
    for (int i = 0; i < 16; ++i)
        for (int j = 0; j < 16; ++j)
            image.setPixel(i, j, {
                (sf::Uint8)(i * 16), (sf::Uint8)(j * 16), 0 });

    wnd.setIcon(image.getSize().x, image.getSize().y, image.getPixelsPtr());
}

// В момент инициализации разумно произвести загрузку текстур, моделей и других вещей
void Init() {

    // Шейдеры
    InitShader();
    // Вершинный буфер
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
                case (sf::Keyboard::W): rotate(1 * deg2rad, 0, 0); break;
                case (sf::Keyboard::S): rotate(-1 * deg2rad, 0, 0); break;
                case (sf::Keyboard::A):rotate(0, 1 * deg2rad, 0); break;
                case (sf::Keyboard::D): rotate(0, -1 * deg2rad, 0); break;
                case (sf::Keyboard::E): rotate(0, 0, 1 * deg2rad); break;
                case (sf::Keyboard::Q): rotate(0, 0, -1 * deg2rad); break;
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