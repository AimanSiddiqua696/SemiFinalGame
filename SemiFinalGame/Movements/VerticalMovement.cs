using SemiFinalGame.Entities;
using System.Drawing;

namespace SemiFinalGame.Movements
{
    public class VerticalMovement
    {
        private float speed;

        public VerticalMovement(float moveSpeed = 5f)
        {
            speed = moveSpeed;
        }

        // Move up
        public void MoveUp(GameObject obj)
        {
            obj.Position = new PointF(obj.Position.X, obj.Position.Y - speed);

            // Prevent going above top bound
            if (obj.Position.Y < 0)
                obj.Position = new PointF(obj.Position.X, 0);
        }

        // Move down
        public void MoveDown(GameObject obj, float bottomBound)
        {
            obj.Position = new PointF(obj.Position.X, obj.Position.Y + speed);

            // Prevent going below bottom bound
            if (obj.Position.Y > bottomBound)
                obj.Position = new PointF(obj.Position.X, bottomBound);
        }
    }
}
