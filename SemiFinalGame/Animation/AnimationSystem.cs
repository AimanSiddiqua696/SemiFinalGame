using SemiFinalGame.Entities;
using SemiFinalGame.Interfaces;

public class AnimationSystem : IAnimation
{
    private Dictionary<string, List<Image>> animations;
    private string current;
    private int frame;
    private float timer;
    private float frameDuration = 0.08f;
    private bool shouldReset = false;

    public AnimationSystem(Dictionary<string, List<Image>> animations, string startState)
    {
        this.animations = animations;
        current = startState;
        frame = 0;
        timer = 0;
    }

    public void SetSpeed(float duration)
    {
        frameDuration = duration;
    }

    public void Update(GameTime time)
    {
        if (!animations.ContainsKey(current) || animations[current].Count == 0) return;

        timer += time.DeltaTime;

        if (timer >= frameDuration)
        {
            if (shouldReset)
            {
                frame = 0;
                shouldReset = false;
            }
            else
            {
                frame = (frame + 1) % animations[current].Count;
            }
            timer = 0;
        }
    }

    public Image GetCurrentFrame()
    {
        if (animations.ContainsKey(current) && animations[current].Count > 0)
        {
             // Safety check for frame index
             if (frame >= animations[current].Count) frame = 0;
             return animations[current][frame];
        }
        return null;
    }

    public void Play(string name)
    {
        if (current == name) return;
        current = name;
        frame = 0;
        timer = 0;
        shouldReset = false;
    }
}
