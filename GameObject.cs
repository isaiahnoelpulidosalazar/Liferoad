using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Liferoad
{
    public class GameObject
    {
        Texture2D Image;
        public float PositionX, PositionY;
        public Rectangle SolidBody;
        public bool IsSolid;
        int LightLevel = 0;
        Action Event;
        public bool IsPlayerNear { get; set; }

        public GameObject(float PositionX, float PositionY, Texture2D Image, bool IsSolid)
        {
            this.PositionX = PositionX;
            this.PositionY = PositionY;
            SolidBody = new Rectangle((int)Math.Round(PositionX), (int)Math.Round(PositionY), Core.TILE_SIZE, Core.TILE_SIZE);
            this.Image = Image;
            this.IsSolid = IsSolid;
        }

        public void SetLightLevel(int LightLevel)
        {
            this.LightLevel = LightLevel;
        }

        public int GetLightLevel()
        {
            return LightLevel;
        }

        public void SetEvent(Action Event)
        {
            this.Event = Event;
        }

        public void DoEvent()
        {
            Event?.Invoke();
        }

        public Texture2D GetImage()
        {
            return Image;
        }
    }
}
