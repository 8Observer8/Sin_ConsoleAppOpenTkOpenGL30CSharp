
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Sin
{
    class Window : GameWindow
    {
        private ShaderProgram _shaderProgram;
        private float[] _vertices;

        private const float _leftBorder = (-1f) * 2 * (float)Math.PI;
        private const float _rightBorder = 2 * (float)Math.PI;
        private const float _amplitute = 1f;

        private bool _canDraw = false;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Sin. C#, OpenTK";
            Width = 256;
            Height = 256;

            Console.WriteLine("OpenGL Version: " + GL.GetString(StringName.Version));

            _shaderProgram = new ShaderProgram(
                "Assets/Shaders/VertexShader.glsl",
                "Assets/Shaders/FragmentShader.glsl");
            if (!_shaderProgram.Init())
            {
                return;
            }

            var projMatrix = Matrix4.CreateOrthographicOffCenter(
                left: _leftBorder, right: _rightBorder,
                bottom: -2f, top: 2f,
                zNear: 10f, zFar: -10f
            );
            _shaderProgram.SetProjMatrix(projMatrix);

            var viewMatrix = Matrix4.LookAt(
                eye: new Vector3(0f, 0f, 5f),
                target: new Vector3(0f, 0f, 0f),
                up: new Vector3(0f, 1f, 0f));
            _shaderProgram.SetViewMatrix(viewMatrix);

            int programId = _shaderProgram.Id;

            _vertices = GetSinVertices();

            if (!VertexBuffers.Init(programId, GetSinVertices()))
            {
                return;
            }

            GL.ClearColor(0.9f, 0.9f, 0.9f, 1f);

            _canDraw = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (_canDraw)
            {
                Draw();
            }

            SwapBuffers();
        }

        private void Draw()
        {
            GL.DrawArrays(PrimitiveType.LineStrip, 0, _vertices.Length / 2);
        }

        private float[] GetSinVertices()
        {
            float angle = _leftBorder;
            float angleStep = (float)Math.PI / 16f;
            int amountOfPoints = 1 + (int) ((_rightBorder - _leftBorder) / angleStep);
            float[] vertices = new float[amountOfPoints * 2];

            float y;
            int index = 0;
            for (int i = 0; i < amountOfPoints; i++)
            {
                y = _amplitute * (float)Math.Sin(angle);
                vertices[index++] = angle;
                vertices[index++] = y;
                angle += angleStep;
            }

            return vertices;
        }
    }
}
