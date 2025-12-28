using SemiFinalGame.Entities;
using System.Drawing;

namespace SemiFinalGame.Movements
{
    public class HorizontalMovement
    {
        private float speed;

        public HorizontalMovement(float moveSpeed = 5f)
        {
            speed = moveSpeed;
        }

        // Move left
        public void MoveLeft(GameObject obj)
        {
            obj.Position = new PointF(obj.Position.X - speed, obj.Position.Y);

            // Prevent going beyond left bound
            if (obj.Position.X < 0)
                obj.Position = new PointF(0, obj.Position.Y);
        }

        // Move right
        public void MoveRight(GameObject obj, float rightBound)
        {
            obj.Position = new PointF(obj.Position.X + speed, obj.Position.Y);

            // Prevent going beyond right bound
            if (obj.Position.X > rightBound)
                obj.Position = new PointF(rightBound, obj.Position.Y);
        }
    }
}
