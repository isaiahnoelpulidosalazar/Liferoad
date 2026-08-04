using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Liferoad
{
    public class GameScenarioManager
    {
        List<GameScenario> Scenarios = new List<GameScenario>();
        int CurrentScenarioIndex = 0;

        public void AddScenario(GameScenario Scenario)
        {
            Scenarios.Add(Scenario);
        }

        public void ChangeScenario(int Index)
        {
            CurrentScenarioIndex = Index;
        }

        public void Update()
        {
            Scenarios[CurrentScenarioIndex].Update();
        }

        public void Draw(ContentManager Content, SpriteBatch _spriteBatch)
        {
            Scenarios[CurrentScenarioIndex].Draw(Content, _spriteBatch);
        }
    }
}
