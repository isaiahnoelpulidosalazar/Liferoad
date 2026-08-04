using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Liferoad
{
    public class Entity
    {
        public float Speed, PositionX = 0f, PositionY = 0f;
        public Rectangle SolidBody, TriggerArea;
        public bool Up, Down, Left, Right;

        public void CheckCollision(Entity Entity, List<GameTile> Tiles)
        {
            foreach (GameTile Tile in Tiles)
            {
                Rectangle tempUP = new Rectangle(Entity.SolidBody.X, (int)Math.Ceiling(Entity.SolidBody.Y - Entity.Speed), Entity.SolidBody.Width, Entity.SolidBody.Height);
                Rectangle tempDOWN = new Rectangle(Entity.SolidBody.X, (int)Math.Ceiling(Entity.SolidBody.Y + Entity.Speed), Entity.SolidBody.Width, Entity.SolidBody.Height);
                Rectangle tempLEFT = new Rectangle((int)Math.Ceiling(Entity.SolidBody.X - Entity.Speed), Entity.SolidBody.Y, Entity.SolidBody.Width, Entity.SolidBody.Height);
                Rectangle tempRIGHT = new Rectangle((int)Math.Ceiling(Entity.SolidBody.X + Entity.Speed), Entity.SolidBody.Y, Entity.SolidBody.Width, Entity.SolidBody.Height);

                if (Tile.IsSolid)
                {
                    if (tempUP.Intersects(Tile.SolidBody))
                    {
                        Up = false;
                    }
                    if (tempDOWN.Intersects(Tile.SolidBody))
                    {
                        Down = false;
                    }
                    if (tempLEFT.Intersects(Tile.SolidBody))
                    {
                        Left = false;
                    }
                    if (tempRIGHT.Intersects(Tile.SolidBody))
                    {
                        Right = false;
                    }
                }
            }
        }

        public void CheckObjectCollision(Entity Entity, List<GameObject> Objects)
        {
            foreach (GameObject Object in Objects)
            {
                Rectangle tempUP = new Rectangle(Entity.SolidBody.X, (int)Math.Ceiling(Entity.SolidBody.Y - Entity.Speed), Entity.SolidBody.Width, Entity.SolidBody.Height);
                Rectangle tempDOWN = new Rectangle(Entity.SolidBody.X, (int)Math.Ceiling(Entity.SolidBody.Y + Entity.Speed), Entity.SolidBody.Width, Entity.SolidBody.Height);
                Rectangle tempLEFT = new Rectangle((int)Math.Ceiling(Entity.SolidBody.X - Entity.Speed), Entity.SolidBody.Y, Entity.SolidBody.Width, Entity.SolidBody.Height);
                Rectangle tempRIGHT = new Rectangle((int)Math.Ceiling(Entity.SolidBody.X + Entity.Speed), Entity.SolidBody.Y, Entity.SolidBody.Width, Entity.SolidBody.Height);

                if (Object.IsSolid)
                {
                    if (tempUP.Intersects(Object.SolidBody))
                    {
                        Up = false;
                    }
                    if (tempDOWN.Intersects(Object.SolidBody))
                    {
                        Down = false;
                    }
                    if (tempLEFT.Intersects(Object.SolidBody))
                    {
                        Left = false;
                    }
                    if (tempRIGHT.Intersects(Object.SolidBody))
                    {
                        Right = false;
                    }
                }
            }
        }
    }
}
