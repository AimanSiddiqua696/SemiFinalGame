using SemiFinalGame.Entities;
using SemiFinalGame.Interfaces;
using System;
using System.Drawing;

namespace SemiFinalGame.Movements
{
    public class ChasePlayerMovement : IMovement
    {
        private GameObject player;
        private float speed;

        public ChasePlayerMovement(GameObject target, float chaseSpeed = 2f)
        {
            player = target;
            speed = chaseSpeed;
        }

        public void Move(GameObject obj, GameTime gameTime)
        {
            // Direction vector from enemy to player
            float dx = player.Position.X - obj.Position.X;
            float dy = player.Position.Y - obj.Position.Y;

            // Update Sprite based on horizontal direction to player
            if (dx < 0)
            {
                obj.Sprite = SemiFinalGame.Properties.Resources.leftside;
            }
            else if (dx > 0)
            {
                obj.Sprite = SemiFinalGame.Properties.Resources.rightside;
            }

            // Distance between enemy and player
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            // Avoid division by zero
            if (distance == 0)
                return;

            // Normalize direction
            dx /= distance;
            dy /= distance;

            // Move enemy toward player
            obj.Position = new PointF(
                obj.Position.X + dx * speed,
                obj.Position.Y + dy * speed
            );
        }
    }
}
