using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Liferoad
{
    public abstract class GameScenario
    {
        public string Name;
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(ContentManager Content, SpriteBatch _spriteBatch);
        public string GetName()
        {
            return Name;
        }
    }
}
