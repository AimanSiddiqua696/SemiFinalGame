using SemiFinalGame.Entities;
using SemiFinalGame.Interfaces;
using SemiFinalGame.Movements;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SemiFinalGame.Entities
{
    public class Coin
    {
        public GameObject Body { get; set; }
        public PictureBox Sprite { get; set; }
        public int Value { get; set; }
        public IMovement Movement { get; set; } // Horizontal movement
        public float SpawnXRatio { get; private set; } // use for resize the form
        public float SpawnYRatio { get; private set; } // use for resize the form

        public Coin(Image image, Point startPos, int value, Size size, float leftBound, float rightBound, int formWidth, int formHeight)
        {
            Value = value;

            // PictureBox setup
            Sprite = new PictureBox();
            Sprite.Image = image;
            Sprite.Size = size;
            Sprite.Location = startPos;
            Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            Sprite.BackColor = Color.Transparent;

            // GameObject for position
            Body = new GameObject();
            Body.Position = new PointF(startPos.X, startPos.Y);

            // Assign horizontal patrol movement
            Movement = new HorizontalPatrolMovement(leftBound, rightBound);
            
            // Calculate ratios based on the passed form dimensions
            SpawnXRatio = startPos.X / (float)formWidth;
            SpawnYRatio = startPos.Y / (float)formHeight;
        }
    }
}
