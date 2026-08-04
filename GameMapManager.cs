using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static Liferoad.GlobalComponents;

namespace Liferoad
{
    public class GameMapManager
    {
        List<GameMap> Maps = new List<GameMap>();
        int Index = -1;

        public GameMapManager()
        {
            DirectoryInfo DirectoryInfo = new DirectoryInfo("Content\\Maps");

            foreach (FileInfo File in DirectoryInfo.GetFiles("*.lrmap"))
            {
                Debug.WriteLine(File.Name);
                Maps.Add(new GameMap(File.Name.Split('.')[0]));
            }
        }

        public void SelectMap(int Index)
        {
            this.Index = Index;
            CurrentMap = Maps[Index];
        }

        public void Draw(ContentManager Content, SpriteBatch _spriteBatch)
        {
            for (int a = 0; a < Maps[Index].GetTiles().Count; a++)
            {
                GameTile temp = Maps[Index].GetTiles()[a];
                _spriteBatch.Draw(temp.GetImage(), new Vector2(temp.PositionX, temp.PositionY), Color.White);
            }

            for (int a = 0; a < Maps[Index].GetObjects().Count; a++)
            {
                GameObject temp = Maps[Index].GetObjects()[a];
                _spriteBatch.Draw(temp.GetImage(), new Vector2(temp.PositionX, temp.PositionY), Color.White);
            }
        }
    }
}
