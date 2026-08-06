using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Liferoad.GlobalComponents;

namespace Liferoad
{
    public class GameScenarioManager
    {
        List<GameScenario> Scenarios = new List<GameScenario>();

        public void AddScenario(GameScenario Scenario)
        {
            Scenarios.Add(Scenario);
        }

        public void ChangeScenario(string Name)
        {
            CurrentScenario = Scenarios.Find(Scenario => Scenario.GetName() == Name);
        }

        public void Update()
        {
            CurrentScenario.Update();
        }

        public void Draw(ContentManager Content, SpriteBatch _spriteBatch)
        {
            CurrentScenario.Draw(Content, _spriteBatch);
        }
    }
}
