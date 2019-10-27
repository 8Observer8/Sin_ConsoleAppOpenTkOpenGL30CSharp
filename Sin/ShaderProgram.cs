
using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Sin
{
    class ShaderProgram
    {
        public int Id { get; set; }

        private int _projectionMatrixLocation;
        private int _viewMatrixLocation;

        public ShaderProgram(string vShaderFilePath, string fShaderFilePath)
        {
            string vShaderSource = null;
            string fShaderSource = null;

            try
            {
                vShaderSource = File.ReadAllText(vShaderFilePath);
                fShaderSource = File.ReadAllText(fShaderFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read a shader file. Message: " + e.Message);
                return;
            }

            Id = GetProgramId(vShaderSource, fShaderSource);
        }

        public bool Init()
        {
            if (Id == -1)
            {
                return false;
            }

            _projectionMatrixLocation = GL.GetUniformLocation(Id, "projectionMatrix");
            if (_projectionMatrixLocation == -1)
            {
                Console.WriteLine("Failed to get the projection matrix location");
                return false;
            }

            _viewMatrixLocation = GL.GetUniformLocation(Id, "viewMatrix");
            if (_viewMatrixLocation == -1)
            {
                Console.WriteLine("Failed to get the view matrix location");
                return false;
            }

            return true;
        }

        public void SetProjMatrix(Matrix4 matrix)
        {
            GL.UniformMatrix4(_projectionMatrixLocation, false, ref matrix);
        }

        public void SetViewMatrix(Matrix4 matrix)
        {
            GL.UniformMatrix4(_viewMatrixLocation, false, ref matrix);
        }

        private int GetProgramId(string vShaderSource, string fShaderSource)
        {
            int vShaderId = GetShaderId(vShaderSource, ShaderType.VertexShader);
            int fShaderId = GetShaderId(fShaderSource, ShaderType.FragmentShader);

            int programId = GL.CreateProgram();
            GL.AttachShader(programId, vShaderId);
            GL.AttachShader(programId, fShaderId);
            GL.LinkProgram(programId);
            GL.UseProgram(programId);

            return programId;
        }

        private int GetShaderId(string shaderSource, ShaderType type)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, shaderSource);
            GL.CompileShader(shader);

            int ok;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out ok);
            if (ok == 0)
            {
                string errorMessage = GL.GetShaderInfoLog(shader);
                Console.WriteLine(string.Format("Failed to compile the {0} shader. Message: {1}", type.ToString(), errorMessage));
                return -1;
            }

            return shader;
        }
    }
}
