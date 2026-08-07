using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Liferoad
{
    public class GameTile
    {
        Texture2D Image;
        public float PositionX, PositionY;
        public Rectangle SolidBody;
        public bool IsSolid;

        public GameTile(float PositionX, float PositionY, Texture2D Image, bool IsSolid)
        {
            this.PositionX = PositionX;
            this.PositionY = PositionY;
            SolidBody = new Rectangle((int)Math.Round(PositionX), (int)Math.Round(PositionY), Core.TILE_SIZE, Core.TILE_SIZE);
            this.Image = Image;
            this.IsSolid = IsSolid;
        }

        public Texture2D GetImage()
        {
            return Image;
        }
    }
}
