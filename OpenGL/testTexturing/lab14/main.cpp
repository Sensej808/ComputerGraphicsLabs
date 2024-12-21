    #include <glad/glad.h>
    #include <GLFW/glfw3.h>
    #include <stb_image.h>

    #include <glm/glm.hpp>
    #include <glm/gtc/matrix_transform.hpp>
    #include <glm/gtc/type_ptr.hpp>

    #include "shaders.h"
    #include "camera.h"
    #include "model.h"

    #include <iostream>

    void framebuffer_size_callback(GLFWwindow* window, int width, int height);
    void mouse_callback(GLFWwindow* window, double xpos, double ypos);
    void scroll_callback(GLFWwindow* window, double xoffset, double yoffset);
    void processInput(GLFWwindow* window);
    unsigned int loadTexture(const char* path);

    // settings
    const unsigned int SCR_WIDTH = 800;
    const unsigned int SCR_HEIGHT = 600;
    bool blinnKeyPressed = false;

    // camera
    Camera camera(glm::vec3(0.0f, 3.0f, 6.0f));
    float lastX = (float)SCR_WIDTH / 2.0;
    float lastY = (float)SCR_HEIGHT / 2.0;
    bool firstMouse = true;

    // timing
    float deltaTime = 0.0f;
    float lastFrame = 0.0f;

    //light
    glm::vec4 light_pos = glm::vec4(0, 10, 0, 1);
    float speed_light = 0.1;
    bool is_point = true;
    bool is_spotlight = false;
    float power = 10.0;
    float cutOff = 0.3;

    int main()
    {
        // glfw: initialize and configure
        // ------------------------------
        glfwInit();
        glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
        glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

    #ifdef __APPLE__
        glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
    #endif

        // glfw window creation
        // --------------------
        GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "LearnOpenGL", NULL, NULL);
        if (window == NULL)
        {
            std::cout << "Failed to create GLFW window" << std::endl;
            glfwTerminate();
            return -1;
        }
        glfwMakeContextCurrent(window);
        glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
        glfwSetCursorPosCallback(window, mouse_callback);
        glfwSetScrollCallback(window, scroll_callback);

        // tell GLFW to capture our mouse
        glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

        // glad: load all OpenGL function pointers
        // ---------------------------------------
        if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
        {
            std::cout << "Failed to initialize GLAD" << std::endl;
            return -1;
        }

        // configure global opengl state
        // -----------------------------
        glEnable(GL_DEPTH_TEST);
        glEnable(GL_BLEND);
        glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

        // build and compile shaders
        // -------------------------
        Shader shader("lighting_fong.vs", "lighting_fong.fs");
        Shader shaderToon("lighting_fong.vs", "lighting_toon.fs");
        Shader shaderMinnaert("lighting_fong.vs", "lighting_minnaert.fs");
        //Model maxwell("blocks/iron_block/iron_block.obj");
        //Model maxwell ("blocks/bricks/bricks.obj");
        //Model teapotToon("blocks/iron_block/iron_block.obj");
        Model maxwell("teapot/teapot.obj");
        Model teapotToon("teapot/teapot.obj");
        Model teapotMinnaert("teapot/teapot.obj");
        // set up vertex data (and buffer(s)) and configure vertex attributes
        // ------------------------------------------------------------------
        float planeVertices[] = {
            // positions            // normals         // texcoords
             10.0f, -0.5f,  10.0f,  0.0f, 1.0f, 0.0f,  10.0f,  0.0f,
            -10.0f, -0.5f,  10.0f,  0.0f, 1.0f, 0.0f,   0.0f,  0.0f,
            -10.0f, -0.5f, -10.0f,  0.0f, 1.0f, 0.0f,   0.0f, 10.0f,

             10.0f, -0.5f,  10.0f,  0.0f, 1.0f, 0.0f,  10.0f,  0.0f,
            -10.0f, -0.5f, -10.0f,  0.0f, 1.0f, 0.0f,   0.0f, 10.0f,
             10.0f, -0.5f, -10.0f,  0.0f, 1.0f, 0.0f,  10.0f, 10.0f
        };
        // plane VAO
        unsigned int planeVAO, planeVBO;
        glGenVertexArrays(1, &planeVAO);
        glGenBuffers(1, &planeVBO);
        glBindVertexArray(planeVAO);
        glBindBuffer(GL_ARRAY_BUFFER, planeVBO);
        glBufferData(GL_ARRAY_BUFFER, sizeof(planeVertices), planeVertices, GL_STATIC_DRAW);
        glEnableVertexAttribArray(0);
        glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
        glEnableVertexAttribArray(1);
        glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
        glEnableVertexAttribArray(2);
        glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(6 * sizeof(float)));
        glBindVertexArray(0);

        // load textures
        // -------------
        unsigned int floorTexture = loadTexture("wood.png");
        unsigned int bricksTexture = loadTexture("blocks/bricks/bricks.png");
        unsigned int ironTexture = loadTexture("blocks/iron_block/iron_block.png");
        unsigned int magentaTexture = loadTexture("blocks/magenta_wool/magenta_wool.png");
        // shader configuration
        // --------------------
        // light properties
        shader.use();
        //shader.setInt("texture1", 2);
        shader.setVec3("light.ambient", 0.2f, 0.2f, 0.2f);
        shader.setVec3("light.diffuse", 0.5f, 0.5f, 0.5f);
        shader.setVec3("light.specular", 1.0f, 1.0f, 1.0f);
        shader.setFloat("light.constant", 2.0f);
        shader.setFloat("light.linear", 0.09f);
        shader.setFloat("light.quadratic", 0.032f);

        shaderToon.use();
        //shaderToon.setInt("texture1", 3);
        shaderToon.setVec3("light.ambient", 0.2f, 0.2f, 0.2f);
        shaderToon.setVec3("light.diffuse", 0.5f, 0.5f, 0.5f);
        shaderToon.setVec3("light.specular", 1.0f, 1.0f, 1.0f);
        shaderToon.setFloat("light.constant", 2.0f);
        shaderToon.setFloat("light.linear", 0.09f);
        shaderToon.setFloat("light.quadratic", 0.032f);

        shaderMinnaert.use();
        //shaderMinnaert.setInt("texture1", 4);
        shaderMinnaert.setVec3("light.ambient", 0.2f, 0.2f, 0.2f);
        shaderMinnaert.setVec3("light.diffuse", 0.5f, 0.5f, 0.5f);
        shaderMinnaert.setVec3("light.specular", 1.0f, 1.0f, 1.0f);
        shaderMinnaert.setFloat("light.constant", 2.0f);
        shaderMinnaert.setFloat("light.linear", 0.09f);
        shaderMinnaert.setFloat("light.quadratic", 0.032f);
        // lighting info
        // -------------

        // render loop
        // -----------
        while (!glfwWindowShouldClose(window))
        {
            // per-frame time logic
            // --------------------
            float currentFrame = static_cast<float>(glfwGetTime());
            deltaTime = currentFrame - lastFrame;
            lastFrame = currentFrame;

            // input
            // -----
            processInput(window);

            // render
            // ------
            glClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

            // draw objects
            shader.use();

        

            glm::mat4 projection = glm::perspective(glm::radians(camera.Zoom), (float)SCR_WIDTH / (float)SCR_HEIGHT, 0.1f, 100.0f);
            glm::mat4 view = camera.GetViewMatrix();
            shader.setMat4("projection", projection);
            shader.setMat4("view", view);
            // set light uniforms
            shader.setVec3("viewPos", camera.Position);
            shader.setVec4("lightPos", is_spotlight ? glm::vec4(camera.Position, 1.0) : light_pos);
            //for spotlight
            shader.setVec3("light.direction", camera.Front);
            shader.setFloat("light.cutOff", glm::cos(glm::radians(12.5f)));
            shader.setBool("light.is_SpotLight", is_spotlight);
        
            glm::mat4 model = glm::mat4(1.0f);
            model = glm::scale(model, glm::vec3(1.0f, 1.0f, 1.0f));
            shader.setMat4("model", model);
            maxwell.Draw(shader);




            //TOON SHADER
            shaderToon.use();
            shaderToon.setMat4("projection", projection);
            shaderToon.setMat4("view", view);
            // set light uniforms
            shaderToon.setVec3("viewPos", camera.Position);
            shaderToon.setVec4("lightPos", is_spotlight ? glm::vec4(camera.Position, 1.0) : light_pos);
            //for spotlight
            shaderToon.setVec3("light.direction", camera.Front);
            shaderToon.setFloat("light.cutOff", glm::cos(glm::radians(12.5f)));
            shaderToon.setBool("light.is_SpotLight", is_spotlight);

            model = glm::mat4(1.0f);
            model = glm::translate(model, glm::vec3(5.0f, 0.0f, 2.0f));
            model = glm::scale(model, glm::vec3(1.0f, 1.0f, 1.0f));
            shaderToon.setMat4("model", model);
            teapotToon.Draw(shaderToon);
        

            //MINNAERT SHADER
            shaderMinnaert.use();
            shaderMinnaert.setMat4("projection", projection);
            shaderMinnaert.setMat4("view", view);
            // set light uniforms
            shaderMinnaert.setVec3("viewPos", camera.Position);
            shaderMinnaert.setVec4("lightPos", is_spotlight ? glm::vec4(camera.Position, 1.0) : light_pos);
            //for spotlight
            shaderMinnaert.setVec3("light.direction", camera.Front);
            shaderMinnaert.setFloat("light.cutOff", glm::cos(glm::radians(12.5f)));
            shaderMinnaert.setBool("light.is_SpotLight", is_spotlight);

            model = glm::mat4(1.0f);
            model = glm::translate(model, glm::vec3(-5.0f, 0.0f, 2.0f));
            model = glm::scale(model, glm::vec3(1.f, 1.0f, 1.0f));
            shaderMinnaert.setMat4("model", model);
            teapotMinnaert.Draw(shaderMinnaert);

            // floor
            shader.use();
            model = glm::mat4(1.0f);
            shader.setMat4("model", model);
            glBindVertexArray(planeVAO);
            glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D, ironTexture);
            glDrawArrays(GL_TRIANGLES, 0, 6);

            // glfw: swap buffers and poll IO events (keys pressed/released, mouse moved etc.)
            // -------------------------------------------------------------------------------
            glfwSwapBuffers(window);
            glfwPollEvents();
        }

        // optional: de-allocate all resources once they've outlived their purpose:
        // ------------------------------------------------------------------------
        glDeleteVertexArrays(1, &planeVAO);
        glDeleteBuffers(1, &planeVBO);

        glfwTerminate();
        return 0;
    }

    // process all input: query GLFW whether relevant keys are pressed/released this frame and react accordingly
    // ---------------------------------------------------------------------------------------------------------
    void processInput(GLFWwindow* window)
    {
        if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
            glfwSetWindowShouldClose(window, true);

        if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
            camera.ProcessKeyboard(FORWARD, deltaTime);
        if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
            camera.ProcessKeyboard(BACKWARD, deltaTime);
        if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
            camera.ProcessKeyboard(LEFT, deltaTime);
        if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
            camera.ProcessKeyboard(RIGHT, deltaTime);

        if (glfwGetKey(window, GLFW_KEY_UP) == GLFW_PRESS)
            light_pos.y += 0.1 * speed_light;
        if (glfwGetKey(window, GLFW_KEY_DOWN) == GLFW_PRESS)
            light_pos.y -= 0.1 * speed_light;
        if (glfwGetKey(window, GLFW_KEY_LEFT) == GLFW_PRESS)
            light_pos.x -= 0.1 * speed_light;
        if (glfwGetKey(window, GLFW_KEY_RIGHT) == GLFW_PRESS)
            light_pos.x += 0.1 * speed_light;

        if (glfwGetKey(window, GLFW_KEY_L) == GLFW_PRESS)
            light_pos.w = 1 - light_pos.w;
        if (glfwGetKey(window, GLFW_KEY_V) == GLFW_PRESS)
            is_spotlight = !is_spotlight;
    }

    // glfw: whenever the window size changed (by OS or user resize) this callback function executes
    // ---------------------------------------------------------------------------------------------
    void framebuffer_size_callback(GLFWwindow* window, int width, int height)
    {
        // make sure the viewport matches the new window dimensions; note that width and 
        // height will be significantly larger than specified on retina displays.
        glViewport(0, 0, width, height);
    }

    // glfw: whenever the mouse moves, this callback is called
    // -------------------------------------------------------
    void mouse_callback(GLFWwindow* window, double xposIn, double yposIn)
    {
        float xpos = static_cast<float>(xposIn);
        float ypos = static_cast<float>(yposIn);
        if (firstMouse)
        {
            lastX = xpos;
            lastY = ypos;
            firstMouse = false;
        }

        float xoffset = xpos - lastX;
        float yoffset = lastY - ypos; // reversed since y-coordinates go from bottom to top

        lastX = xpos;
        lastY = ypos;

        camera.ProcessMouseMovement(xoffset, yoffset);
    }

    // glfw: whenever the mouse scroll wheel scrolls, this callback is called
    // ----------------------------------------------------------------------
    void scroll_callback(GLFWwindow* window, double xoffset, double yoffset)
    {
        camera.ProcessMouseScroll(static_cast<float>(yoffset));
    }

    // utility function for loading a 2D texture from file
    // ---------------------------------------------------
    unsigned int loadTexture(char const* path)
    {
        unsigned int textureID;
        glGenTextures(1, &textureID);

        int width, height, nrComponents;
        unsigned char* data = stbi_load(path, &width, &height, &nrComponents, 0);
        if (data)
        {
            GLenum format;
            if (nrComponents == 1)
                format = GL_RED;
            else if (nrComponents == 3)
                format = GL_RGB;
            else if (nrComponents == 4)
                format = GL_RGBA;

            glBindTexture(GL_TEXTURE_2D, textureID);
            glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, GL_UNSIGNED_BYTE, data);
            glGenerateMipmap(GL_TEXTURE_2D);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, format == GL_RGBA ? GL_CLAMP_TO_EDGE : GL_REPEAT); // for this tutorial: use GL_CLAMP_TO_EDGE to prevent semi-transparent borders. Due to interpolation it takes texels from next repeat 
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, format == GL_RGBA ? GL_CLAMP_TO_EDGE : GL_REPEAT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

            stbi_image_free(data);
        }
        else
        {
            std::cout << "Texture failed to load at path: " << path << std::endl;
            stbi_image_free(data);
        }

        return textureID;
    }