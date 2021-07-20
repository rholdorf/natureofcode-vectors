using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhatIsAVector
{
    public class Shapes : IDisposable
    {
        private const int MAX_VERTEX_COUNT = 1024;
        private const int MAX_INDEX_COUNT = MAX_VERTEX_COUNT * 3;
        private const float MIN_LINE_THICKNESS = 1f;
        private const float MAX_LINE_THICKNESS = 10f;

        private Game _game;
        private BasicEffect _basicEffect;

        private VertexPositionColor[] _vertices;
        private int[] _indices;

        private int _vertexCount;
        private int _indexCount;
        private int _shapeCount;

        private bool _isStarted;
        private bool _disposed;

        public Shapes(Game game)
        {
            _game = game;
            _vertices = new VertexPositionColor[MAX_VERTEX_COUNT];
            _indices = new int[MAX_INDEX_COUNT];

            _basicEffect = new BasicEffect(_game.GraphicsDevice)
            {
                TextureEnabled = false,
                FogEnabled = false,
                LightingEnabled = false,
                VertexColorEnabled = true,
                World = Matrix.Identity,
                View = Matrix.Identity,
                Projection = Matrix.Identity
            };

            Viewport viewport = _game.GraphicsDevice.Viewport;
            _basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, 0, viewport.Height, 0f, 1f);
        }

        public void Begin()
        {
            if (_isStarted)
                throw new InvalidOperationException("Batch already started.");

            _isStarted = true;
        }

        public void End()
        {
            Flush();
            _isStarted = false;
        }

        public void Flush()
        {
            if (_shapeCount == 0)
                return;

            EnsureStarted();

            // send to GPU
            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList, // its a list of triangles
                    _vertices, // all the vertex info
                    0, // no offset, draw them all
                    _vertexCount, // how many vertex we have in the list
                    _indices, // all the index array
                    0, // no need to skip any index
                    _indexCount / 3); // how many primitives this is supposed to produce (two triangles for each rectangle)
            }

            // data was sent to GPU, need to reset counters
            _shapeCount = 0;
            _vertexCount = 0;
            _indexCount = 0;
        }

        private void EnsureStarted()
        {
            if (!_isStarted)
                throw new InvalidOperationException("Batch is not started.");
        }

        private void EnsureSpace(int vertexCount, int indexCount)
        {
            if (vertexCount > MAX_VERTEX_COUNT)
                throw new ArgumentOutOfRangeException(nameof(vertexCount), "Max vertex count reached: " + MAX_VERTEX_COUNT);

            if (indexCount > MAX_INDEX_COUNT)
                throw new ArgumentOutOfRangeException(nameof(indexCount), "Max index count reached: " + MAX_INDEX_COUNT);

            if (_vertexCount + vertexCount > MAX_VERTEX_COUNT || _indexCount + indexCount > MAX_INDEX_COUNT)
                Flush();
        }

        public void DrawRectangle(float x, float y, float width, float height, Color color)
        {
            /*
             *    a +------+ b       indices       array
             *      |\     |         a = 0         0 --+
             *      | \    |         b = 1         1   +-- first rectangle (made of vector indices 0, 1 and 2)
             *      |  \   |         c = 2         2 --+
             *      |   \  |         d = 3              
             *      |    \ |                       0 --+  
             *      |     \|                       2   +-- second rectangle (made of vector indices 0, 2 and 3)
             *    d +------+ c                     3 --+
             *    
             *    A rectangle is made of two triangles (a,b,c) and (a,c,d),
             *    which are sent to the GPU as 4 vectors (a,b,c,d) and an array
             *    telling the order how to define each rectangle.
             */

            EnsureStarted();
            EnsureSpace(4, 6); // 4 vectors (the corners) and and array of 6 indices (three for each vertex)

            var left = x;
            var right = x + width;
            var bottom = y;
            var top = y + height;

            var a = new Vector2(left, top);
            var b = new Vector2(right, top);
            var c = new Vector2(right, bottom);
            var d = new Vector2(left, bottom);

            // fill the array with the index "pointers", adding the vertexCount
            // so they will point to the correct vertex. If we have two or more
            // rectangles already in the array and they are yet to be sent to
            // the GPU, then we want to keep adding and shifting the indexes
            // accordingly. That way, every set of 6 indices will always
            // point to a related set of four vertices.
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;


            // take each vertex and the color information and then add to the vertices array,
            // passing 0f as z axis because this is a 2D rectangle and should be drawn in
            // the same plane
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(a, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(b, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(c, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(d, 0f), color);
            _shapeCount++;
        }

        public void DrawLine(Vector2 a, Vector2 b, float thickness, Color color)
        {
            EnsureStarted();
            EnsureSpace(4, 6); // 4 vectors (the corners) and and array of 6 indices (three for each vertex)

            thickness.Clamp(MIN_LINE_THICKNESS, MAX_LINE_THICKNESS);

            var halfThickness = thickness / 2f;
            var e1 = b - a;
            e1.Normalize();
            e1 *= halfThickness;

            var e2 = -e1;
            var n1 = new Vector2(-e1.Y, e1.X);
            var n2 = -n1;
            var q1 = a + n1 + e2;
            var q2 = b + n1 + e1;
            var q3 = b + n2 + e1;
            var q4 = a + n2 + e2;

            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q1, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q2, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q3, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q4, 0f), color);
            _shapeCount++;
        }

        // it is faster when doing all calculations using only floats instead of vectors
        public void DrawLine(float ax, float ay, float bx, float by, float thickness, Color color)
        {
            EnsureStarted();
            EnsureSpace(4, 6); // 4 vectors (the corners) and and array of 6 indices (three for each vertex)

            thickness.Clamp(MIN_LINE_THICKNESS, MAX_LINE_THICKNESS);

            var halfThickness = thickness / 2f;
            var e1x = bx - ax;
            var e1y = by - ay;

            // normalize
            var len = (float)Math.Sqrt(e1x * e1x + e1y * e1y);
            e1x /= len;
            e1y /= len;

            e1x *= halfThickness;
            e1y *= halfThickness;
            var e2x = -e1x;
            var e2y = -e1y;
            var n1x = -e1y;
            var n1y = e1x;
            var n2x = -n1x;
            var n2y = -n1y;
            var q1x = ax + n1x + e2x;
            var q1y = ay + n1y + e2y;
            var q2x = bx + n1x + e2x;
            var q2y = by + n1y + e2y;
            var q3x = bx + n2x + e1x;
            var q3y = by + n2y + e1y;
            var q4x = ax + n2x + e2x;
            var q4y = ay + n2y + e2y;

            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q1x, q1y, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q2x, q2y, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q3x, q3y, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q4x, q4y, 0f), color);
            _shapeCount++;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _basicEffect?.Dispose();
            _disposed = true;
        }
    }
}
