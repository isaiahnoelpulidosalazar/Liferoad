using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Liferoad
{
    public class GameMapManager
    {
        List<GameMap> Maps = new List<GameMap>();
        static GameMap CurrentGameMap;

        public GameMapManager()
        {
            DirectoryInfo DirectoryInfo = new DirectoryInfo("Content\\Maps");

            foreach (FileInfo File in DirectoryInfo.GetFiles("*.lrmap"))
            {
                Maps.Add(new GameMap(File.Name.Split('.')[0]));
            }
        }

        public void ChangeMap(string Name)
        {
            CurrentGameMap = Maps.Find(Map => Map.GetName() == Name);
        }

        public void Draw(ContentManager Content, SpriteBatch _spriteBatch)
        {
            for (int a = 0; a < CurrentGameMap.GetTiles().Count; a++)
            {
                GameTile temp = CurrentGameMap.GetTiles()[a];
                _spriteBatch.Draw(temp.GetImage(), new Vector2(temp.PositionX, temp.PositionY), Color.White);
            }

            for (int a = 0; a < CurrentGameMap.GetObjects().Count; a++)
            {
                GameObject temp = CurrentGameMap.GetObjects()[a];
                _spriteBatch.Draw(temp.GetImage(), new Vector2(temp.PositionX, temp.PositionY), Color.White);
            }
        }

        public GameMap GetCurrentGameMap()
        {
            return CurrentGameMap;
        }
    }
}
