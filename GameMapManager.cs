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

        public GameMapManager()
        {
            DirectoryInfo DirectoryInfo = new DirectoryInfo("Content\\Maps");

            foreach (FileInfo File in DirectoryInfo.GetFiles("*.lrmap"))
            {
                Debug.WriteLine(File.Name);
                Maps.Add(new GameMap(File.Name.Split('.')[0]));
            }
        }

        public void ChangeMap(string Name)
        {
            CurrentMap = Maps.Find(Map => Map.GetName() == Name);
        }

        public void Draw(ContentManager Content, SpriteBatch _spriteBatch)
        {
            for (int a = 0; a < CurrentMap.GetTiles().Count; a++)
            {
                GameTile temp = CurrentMap.GetTiles()[a];
                _spriteBatch.Draw(temp.GetImage(), new Vector2(temp.PositionX, temp.PositionY), Color.White);
            }

            for (int a = 0; a < CurrentMap.GetObjects().Count; a++)
            {
                GameObject temp = CurrentMap.GetObjects()[a];
                _spriteBatch.Draw(temp.GetImage(), new Vector2(temp.PositionX, temp.PositionY), Color.White);
            }
        }
    }
}
