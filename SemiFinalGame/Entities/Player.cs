using SemiFinalGame.Entities;
using SemiFinalGame.Extensions;
using SemiFinalGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SemiFinalGame.Animation;
using System.Drawing;


namespace SemiFinalGame.Entities
{
    public class Player : GameObject
    {
        private AnimationSystem animation;
        private string currentDirection = "Down";

        // Movement strategy: demonstrates composition over inheritance.
        // Different movement behaviors can be injected (KeyboardMovement, PatrolMovement, etc.).
        public IMovement? Movement { get; set; }

        // Domain state
        public int Health { get; set; } = 100;
        public int Score { get; set; } = 0;
        public AnimationSystem? Animation { get; private set; }
        public void SetAnimation(Dictionary<string, List<Image>> animations, string startDirection)
        {
            animation = new AnimationSystem(animations, startDirection);
            currentDirection = startDirection;
            Sprite = animation.GetCurrentFrame();
        }

        public void PlayAnimation(string direction)
    {
        Animation?.Play(direction);
    }
        /// Update the player: delegate movement to the Movement strategy (if provided) and then apply base update.
        /// Shows the Strategy pattern (movement behavior varies independently from Player class).
        public override void Update(GameTime gameTime)
        {
            Movement?.Move(this, gameTime);

            animation?.Update(gameTime);
            Sprite = animation?.GetCurrentFrame();

            base.Update(gameTime);
        }
        public void ChangeDirection(string direction)
        {
            if (currentDirection == direction) return;

            currentDirection = direction;
            animation?.Play(direction);
        }



        /// Draw uses base implementation; override if player needs custom visuals.

        public override void Draw(Graphics g)
        {
            if (Animation != null)
                g.DrawImage(Animation.GetCurrentFrame(), Bounds);
            else
                base.Draw(g);
        }

        /// Collision reaction for the player. Demonstrates single responsibility: domain reaction is handled here.
        public override void OnCollision(GameObject other)
        {
            if (other is Enemy)
                Health -= 10;

            if (other is PowerUp)
                Health += 20;
        }
    }
}

