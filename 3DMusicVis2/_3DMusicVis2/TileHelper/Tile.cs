#region License

// Copyright (c) 2015, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Tile.cs
// Date - created: 2015.08.26 - 14:45
// Date - current: 2016.05.23 - 21:16

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.TileHelper
{
    public class Tile
    {
        public bool UpdatedSide;

        public Tile(GraphicsDevice device, Vector3 position, float width, float height, float depth, Color color)
        {
            Position = position;
            Width = width;
            CenterHeight = height;
            Depth = depth;

            Verts = new VertexPositionColor[24]
            {
                new VertexPositionColor(new Vector3(Position.X, Position.Y, Position.Z), color), //Left bottom
                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y + height/2, Position.Z + depth/2),
                    color), //MIDDLE!!! [1]
                new VertexPositionColor(new Vector3(Position.X, Position.Y, Position.Z + depth/2), color), //left middle

                new VertexPositionColor(new Vector3(Position.X, Position.Y, Position.Z), color), //Left bootom
                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y, Position.Z), color),
                //Bottom midle
                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y + height/2, Position.Z + depth/2),
                    color), //MIDDLE!!! [5]

                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y, Position.Z), color),
                //Bottom midle
                new VertexPositionColor(new Vector3(Position.X + width, Position.Y, Position.Z), color), //Right bottom
                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y + height/2, Position.Z + depth/2),
                    color), //MIDDLE!!! [8]

                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y + height/2, Position.Z + depth/2),
                    color), //MIDDLE!!! [9]
                new VertexPositionColor(new Vector3(Position.X + width, Position.Y, Position.Z), color), //Right bottom
                new VertexPositionColor(new Vector3(Position.X + width, Position.Y, Position.Z + depth/2), color),
                //Right Middle

                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y + height/2, Position.Z + depth/2),
                    color), //MIDDLE!!! [12]
                new VertexPositionColor(new Vector3(Position.X + width, Position.Y, Position.Z + depth/2), color),
                //Right Middle
                new VertexPositionColor(new Vector3(Position.X + width, Position.Y, Position.Z + depth), color),
                //Right top

                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y + height/2, Position.Z + depth/2),
                    color), //MIDDLE!!! [15]
                new VertexPositionColor(new Vector3(Position.X + width, Position.Y, Position.Z + depth), color),
                //Right top
                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y, Position.Z + depth), color),
                //middle top

                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y + height/2, Position.Z + depth/2),
                    color), //MIDDLE!!! [18]
                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y, Position.Z + depth), color),
                //middle top
                new VertexPositionColor(new Vector3(Position.X, Position.Y, Position.Z + depth), color), //left top

                new VertexPositionColor(new Vector3(Position.X + width/2, Position.Y + height/2, Position.Z + depth/2),
                    color), //MIDDLE!!! [21]
                new VertexPositionColor(new Vector3(Position.X, Position.Y, Position.Z + depth), color), //left top
                new VertexPositionColor(new Vector3(Position.X, Position.Y, Position.Z + depth/2), color) //left middle
            };
        }

        public VertexPositionColor[] Verts { get; }

        public float CenterHeight { get; set; }
        public Vector3 Position { get; set; }
        public float Width { get; set; }
        public float Depth { get; set; }
        public Color CenterColor { get; private set; }
        public float LastSampleValue { get; set; }

        public void Draw(BasicEffect basicEffect, GraphicsDevice device)
        {
            for (var i = 0; i < basicEffect.CurrentTechnique.Passes.Count; i++)
            {
                basicEffect.CurrentTechnique.Passes[i].Apply();
                device.DrawUserPrimitives(PrimitiveType.TriangleList, Verts, 0, Verts.Length/3);
            }
        }

        public void ChangeAllColors(Color color)
        {
            for (var i = 0; i < Verts.Length; i++)
            {
                Verts[i].Color = color;
            }
        }

        public void ChangeSideColor(Color color)
        {
            Verts[0].Color = color;
            Verts[2].Color = color;
            Verts[3].Color = color;
            Verts[4].Color = color;
            Verts[6].Color = color;
            Verts[7].Color = color;
            Verts[10].Color = color;
            Verts[11].Color = color;
            Verts[13].Color = color;
            Verts[14].Color = color;
            Verts[16].Color = color;
            Verts[17].Color = color;
            Verts[19].Color = color;
            Verts[20].Color = color;
            Verts[22].Color = color;
            Verts[23].Color = color;
        }

        public void ChangeSideColorsDynamic(Color fadeOut)
        {
            // Also a great performance eater
            // TODO: Gather more performances
            Verts[0].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[0].Position.Y.Normalize(0, 1));
            Verts[2].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[2].Position.Y.Normalize(0, 1));
            Verts[3].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[3].Position.Y.Normalize(0, 1));
            Verts[4].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[4].Position.Y.Normalize(0, 1));
            Verts[6].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[6].Position.Y.Normalize(0, 1));
            Verts[7].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[7].Position.Y.Normalize(0, 1));
            Verts[10].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[10].Position.Y.Normalize(0, 1));
            Verts[11].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[11].Position.Y.Normalize(0, 1));
            Verts[13].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[13].Position.Y.Normalize(0, 1));
            Verts[14].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[14].Position.Y.Normalize(0, 1));
            Verts[16].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[16].Position.Y.Normalize(0, 1));
            Verts[17].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[17].Position.Y.Normalize(0, 1));
            Verts[19].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[19].Position.Y.Normalize(0, 1));
            Verts[20].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[20].Position.Y.Normalize(0, 1));
            Verts[22].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[22].Position.Y.Normalize(0, 1));
            Verts[23].Color = Color.Lerp(CenterColor, fadeOut, 1 - Verts[23].Position.Y.Normalize(0, 1));
        }

        public void ChangeMiddleColors(Color color)
        {
            Verts[1].Color = color;
            Verts[5].Color = color;
            Verts[8].Color = color;
            Verts[9].Color = color;
            Verts[12].Color = color;
            Verts[15].Color = color;
            Verts[18].Color = color;
            Verts[21].Color = color;

            CenterColor = color;
        }

        public void ChangeCenterHeight(float height, float heightmultiplier = 1f)
        {
            Verts[1].Position.Y = height;
            Verts[5].Position.Y = height;
            Verts[8].Position.Y = height;
            Verts[9].Position.Y = height;
            Verts[12].Position.Y = height;
            Verts[15].Position.Y = height;
            Verts[18].Position.Y = height;
            Verts[21].Position.Y = height;
            CenterHeight = height;
        }

        public void ChangeTopRightCorner(float height)
        {
            Verts[14].Position.Y = height;
            Verts[16].Position.Y = height;
        }

        public void ChangeBottomRightCorner(float height)
        {
            Verts[7].Position.Y = height;
            Verts[10].Position.Y = height;
        }

        public void ChangeTopLeftCorner(float height)
        {
            Verts[20].Position.Y = height;
            Verts[22].Position.Y = height;
        }

        public void ChangeBottomLeftCorner(float height)
        {
            Verts[0].Position.Y = height;
            Verts[3].Position.Y = height;
        }

        public void ChangeTopEdge(float height)
        {
            Verts[17].Position.Y = height;
            Verts[19].Position.Y = height;
        }

        public void ChangeRightEdge(float height)
        {
            Verts[11].Position.Y = height;
            Verts[13].Position.Y = height;
        }

        public void ChangeBottomEdge(float height)
        {
            Verts[4].Position.Y = height;
            Verts[6].Position.Y = height;
        }

        public void ChangeLeftEdge(float height)
        {
            Verts[2].Position.Y = height;
            Verts[23].Position.Y = height;
        }

        public void UpdateOutsideHeight(Tile[,] field, Point myPosition)
        {
            // Also a great performance eater
            // TODO: Gather more performances
            int fieldWidth = field.GetLength(0) - 1, fieldHeight = field.GetLength(1) - 1;
            var right = myPosition.X == fieldWidth ? fieldWidth : myPosition.X + 1;
            var top = myPosition.Y == 0 ? 0 : myPosition.Y - 1;
            var bottom = myPosition.Y == fieldHeight ? fieldHeight : myPosition.Y + 1;
            var left = myPosition.X == 0 ? 0 : myPosition.X - 1;

            ChangeTopRightCorner(
                (field[right, bottom].CenterHeight
                 + field[myPosition.X, bottom].CenterHeight
                 + field[right, myPosition.Y].CenterHeight
                 + CenterHeight)
                /4.0f);
            ChangeBottomRightCorner(
                (field[right, top].CenterHeight
                 + field[right, myPosition.Y].CenterHeight
                 + field[myPosition.X, top].CenterHeight
                 + CenterHeight)
                /4.0f);
            ChangeTopLeftCorner(
                (field[left, bottom].CenterHeight
                 + field[myPosition.X, bottom].CenterHeight
                 + field[left, myPosition.Y].CenterHeight
                 + CenterHeight)
                /4.0f);
            ChangeBottomLeftCorner(
                (field[left, top].CenterHeight
                 + field[left, myPosition.Y].CenterHeight
                 + field[myPosition.X, top].CenterHeight
                 + CenterHeight)
                /4.0f);

            ChangeTopEdge((field[myPosition.X, bottom].CenterHeight + CenterHeight)/2.0f);
            ChangeRightEdge((field[right, myPosition.Y].CenterHeight + CenterHeight)/2.0f);
            ChangeBottomEdge((field[myPosition.X, top].CenterHeight + CenterHeight)/2.0f);
            ChangeLeftEdge((field[left, myPosition.Y].CenterHeight + CenterHeight)/2.0f);
        }

        public void ChangeOutSideHeihgt(float height)
        {
            ChangeTopRightCorner(height);
            ChangeBottomRightCorner(height);
            ChangeTopLeftCorner(height);
            ChangeBottomLeftCorner(height);
            ChangeTopEdge(height);
            ChangeRightEdge(height);
            ChangeBottomEdge(height);
            ChangeLeftEdge(height);
        }
    }
}