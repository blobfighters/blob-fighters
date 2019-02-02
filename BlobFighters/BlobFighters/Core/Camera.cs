using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using BlobFighters;

namespace MonoEngine.Core
{
    public class Camera
    {
        /// <summary>
        /// The Camera's Viewport.
        /// </summary>
        public Viewport Viewport
        {
            get
            {
                return GameManager.Instance.GraphicsDevice.Viewport;
            }
        }

        /// <summary>
        /// The origin (center) of the camera.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// The position of the camera.
        /// </summary>
        public Vector2 Position { get; set; }
        
        /// <summary>
        /// The rotation of the camera.
        /// </summary>
        public float Rotation { get; set; }
        
        /// <summary>
        /// The scale of the camera.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Gets the Camera's view matrix.
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(-Position.X, -Position.Y, 0f) *
                    Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0f) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Scale.X, Scale.Y, 1f) *
                    Matrix.CreateTranslation(Origin.X, Origin.Y, 0f);
            }
        }

        /// <summary>
        /// Gets the Camera's view matrix in sim units.
        /// </summary>
        public Matrix SimViewMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(ConvertUnits.ToSimUnits(-Position.X), ConvertUnits.ToSimUnits(-Position.Y), 0f) *
                    Matrix.CreateTranslation(ConvertUnits.ToSimUnits(-Origin.X), ConvertUnits.ToSimUnits(-Origin.Y), 0f) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Scale.X, Scale.Y, 1f) *
                    Matrix.CreateTranslation(ConvertUnits.ToSimUnits(Origin.X), ConvertUnits.ToSimUnits(Origin.Y), 0f);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Camera"/> instance.
        /// </summary>
        public Camera()
        {
            Origin = new Vector2(Viewport.Width * 0.5f, Viewport.Height * 0.5f);
            Position = Vector2.Zero;
            Rotation = 0f;
            Scale = Vector2.One;
        }
    }
}