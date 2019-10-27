
using OpenTK.Graphics.OpenGL;

namespace Sin
{
    class VertexBuffers
    {
        public static bool Init(int programId, float[] vertices)
        {
            int vbo;
            GL.GenBuffers(1, out vbo);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            int positionLocation = GL.GetAttribLocation(programId, "position");
            if (positionLocation == -1)
            {
                return false;
            }
            GL.VertexAttribPointer(positionLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(positionLocation);

            return true;
        }
    }
}
