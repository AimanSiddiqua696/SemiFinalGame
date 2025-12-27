using System;
using System.Drawing;

namespace SemiFinalGame.Animation
{
    public class Animations
    {
        // Animation frames for different states
        public Image[] IdleFrames;
        public Image[] WalkFrames;
        public Image[] AttackFrames;
        public Image[] DeathFrames;

        private float frameTime;      // Time per frame
        private float timer;          // Timer for updating frames
        private int currentFrame;     // Current frame index
        private Image[] currentFrames; // Current animation frames

        public Animations(float frameTime = 0.2f)
        {
            this.frameTime = frameTime;
            timer = 0;
            currentFrame = 0;
        }

        // Set current animation frames based on state
        public void SetState(string state)
        {
            switch (state)
            {
                case "Idle":
                    currentFrames = IdleFrames;
                    break;
                case "Walk":
                    currentFrames = WalkFrames;
                    break;
                case "Attack":
                    currentFrames = AttackFrames;
                    break;
                case "Death":
                    currentFrames = DeathFrames;
                    break;
                default:
                    currentFrames = IdleFrames;
                    break;
            }
            currentFrame = 0;
            timer = 0;
        }

        // Update animation frames
        public void Update(float deltaTime)
        {
            if (currentFrames == null || currentFrames.Length <= 1)
                return;

            timer += deltaTime;
            if (timer >= frameTime)
            {
                timer = 0;
                currentFrame++;
                if (currentFrame >= currentFrames.Length)
                    currentFrame = 0; // Loop animation
            }
        }

        // Get current frame
        public Image GetCurrentFrame()
        {
            if (currentFrames == null || currentFrames.Length == 0)
                return null;

            return currentFrames[currentFrame];
        }
    }
}
