using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Liferoad
{
    public abstract class GameScenario
    {
        public abstract void Update();
        public abstract void Draw(ContentManager Content, SpriteBatch _spriteBatch);
    }
}
