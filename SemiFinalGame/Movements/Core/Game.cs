using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SemiFinalGame.Entities
{
    public partial class Game
    {
        private List<GameObject> objects = new List<GameObject>();

        public List<GameObject> Objects => objects;

        /// Add a game object to the scene.
        /// Encapsulation: Game manages the collection so external code doesn't manipulate the list directly.
        public void AddObject(GameObject obj)
        {
            objects.Add(obj);
        }

        /// Update all active objects. The Game is responsible for iterating objects and orchestrating(managing and arranging things in a controlled and systematic way) the update sequence.
        /// This separation of concerns keeps the game loop logic central and simple.
        public void Update(GameTime gameTime)
        {
            foreach (var obj in objects.Where(o => o.IsActive))
            {
                obj.Update(gameTime);
            }
        }

        /// Draw all active objects by delegating to each object's Draw implementation (polymorphism).
        public void Draw(Graphics g)
        {
            foreach (var obj in objects.Where(o => o.IsActive))
            {
                obj.Draw(g);
            }
        }

        /// Remove objects that are no longer active; keeps the object list clean.
        public void Cleanup()
        {
            objects.RemoveAll(o => !o.IsActive);
        }
    }
}
