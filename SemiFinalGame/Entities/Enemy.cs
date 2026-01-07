using SemiFinalGame.Entities;
using SemiFinalGame.Extensions;
using SemiFinalGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemiFinalGame.Entities
{
    public class Enemy : GameObject
    {
        // Optional movement behavior: demonstrates composition and allows testable movement logic.
        public IMovement? Movement { get; set; }

        // Default enemy velocity is set in constructor to give basic movement out-of-the-box.
        public Enemy()
        {
            Velocity = new PointF(-2, 0);
        }

        /// Update will call movement behavior (if any) and then apply base update to move by velocity.
        public override void Update(GameTime gameTime)
        {
            Movement?.Move(this, gameTime); // movement must be called
            base.Update(gameTime);
        }



        /// On collision, enemy deactivates when hit by bullets (encapsulation of reaction logic inside the entity).
        public override void OnCollision(GameObject other)
        {
            if (other is Bullet)
                IsActive = false;
        }
    }
}
