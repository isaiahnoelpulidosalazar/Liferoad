using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using static Liferoad.GlobalComponents;

namespace Liferoad
{
    public class Player : Entity
    {
        public GameObject PlayerNearestGameObject;

        public void SetSpeed(float Speed)
        {
            this.Speed = Speed;
        }

        public void SetPosition(float PositionX, float PositionY)
        {
            this.PositionX = PositionX;
            this.PositionY = PositionY;
        }

        public void Update()
        {
            Up = Inputs.Up;
            Down = Inputs.Down;
            Left = Inputs.Left;
            Right = Inputs.Right;

            bool UL = Up && Left;
            bool UR = Up && Right;
            bool DL = Down && Left;
            bool DR = Down && Right;

            CheckCollision(this, CurrentMap.GetTiles());
            CheckObjectCollision(this, CurrentMap.GetObjects());
            PlayerNearestGameObject = NearestGameObject(this, CurrentMap.GetObjects());

            if (Inputs.Interact)
            {
                if (PlayerNearestGameObject != null)
                {
                    PlayerNearestGameObject.DoEvent();
                }
            }

            if (UL || UR || DL || DR)
            {
                float NormalizedSpeed = Speed * 0.75f;

                if (Up)
                {
                    PositionY -= NormalizedSpeed;
                }
                if (Down)
                {
                    PositionY += NormalizedSpeed;
                }
                if (Left)
                {
                    PositionX -= NormalizedSpeed;
                }
                if (Right)
                {
                    PositionX += NormalizedSpeed;
                }
            }
            else
            {
                if (Up)
                {
                    PositionY -= Speed;
                }
                if (Down)
                {
                    PositionY += Speed;
                }
                if (Left)
                {
                    PositionX -= Speed;
                }
                if (Right)
                {
                    PositionX += Speed;
                }
            }

            if (!Up && !Down && !Left && !Right)
            {
                PositionX = (float)Math.Round(PositionX);
                PositionY = (float)Math.Round(PositionY);
            }

            SolidBody = new Rectangle((int)PositionX + 8, (int)PositionY + 32, 32, 16);
            TriggerArea = new Rectangle((int)PositionX - 4, (int)PositionY - 4, TILE_SIZE + 8, TILE_SIZE + 8);
        }

        public void Draw(ContentManager Content, SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(Content.Load<Texture2D>("player_down"), new Vector2(PositionX, PositionY), Color.White);
        }
    }
}
