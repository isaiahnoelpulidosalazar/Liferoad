using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Liferoad.GlobalComponents;

namespace Liferoad
{
    public class GameMap
    {
        string Name;
        List<GameTile> Tiles = new List<GameTile>();
        List<GameObject> Objects = new List<GameObject>();

        public GameMap(string Name)
        {
            this.Name = Name;

            DirectoryInfo TileDirectoryInfo = new DirectoryInfo("Content\\Tiles");
            DirectoryInfo ObjectDirectoryInfo = new DirectoryInfo("Content\\Objects");

            string[] Map = File.ReadAllLines("Content\\Maps\\" + Name + ".lrmap");
            string[] TileMap = Map.Skip(1).Take(Array.IndexOf(Map, "[OBJECTS]") - 1).ToArray();
            string[] ObjectMap = Map.Skip(Array.IndexOf(Map, "[OBJECTS]") + 1).ToArray();
            string[] TileImages = File.ReadAllLines("Content\\Maps\\" + Name + ".lrtilemap");
            string[] ObjectImages = File.ReadAllLines("Content\\Maps\\" + Name + ".lrobjectmap");

            for (int a = 0; a < TileMap.Length; a++)
            {
                string[] temp = TileMap[a].Split(',');
                for (int b = 0; b < temp.Length; b++)
                {
                    int Index = Convert.ToInt32(temp[b]) - 1;
                    if (Index >= 0)
                    {
                        Tiles.Add(new GameTile(TILE_SIZE * b, TILE_SIZE * a, Texture2D.FromFile(MainGraphicsDevice, "Content\\Tiles\\" + (TileImages[Index].Contains("!") ? TileImages[Index].Substring(0, TileImages[Index].Length - 1) : TileImages[Index]) + ".png"), TileImages[Index].Contains("!")));
                    }
                }
            }
            for (int a = 0; a < ObjectMap.Length; a++)
            {
                string[] temp = ObjectMap[a].Split(',');
                for (int b = 0; b < temp.Length; b++)
                {
                    int Index = Convert.ToInt32(temp[b]) - 1;
                    if (Index >= 0)
                    {
                        Objects.Add(new GameObject(TILE_SIZE * b, TILE_SIZE * a, Texture2D.FromFile(MainGraphicsDevice, "Content\\Objects\\" + (ObjectImages[Index].Contains("!") ? ObjectImages[Index].Substring(0, ObjectImages[Index].Length - 1) : ObjectImages[Index]) + ".png"), ObjectImages[Index].Contains("!")));
                    }
                }
            }
        }

        public List<GameTile> GetTiles()
        {
            return Tiles;
        }

        public List<GameObject> GetObjects()
        {
            return Objects;
        }
    }
}
