using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using Persistence;
namespace Logic
{
    public class Parser
    {
        private Points DeserializeJson(string JsonText)
        {
            try
            {
                Points points = JsonConvert.DeserializeObject<Points>(JsonText);
                SortByText(ref points);
                return points;
            }
            catch (Exception e) { SceneManager.LoadScene("InsertJson"); }
            return null;
        }

        private void SortByText(ref Points p)
        {
            foreach (Point points in p.points)
            {
                points.OptionalText = points.OptionalText.OrderBy(o => o.whenToDisplay).ToList<Optionaltext>();
            }
        }
        public Points ReadPoints()
        {
            Persistence.JsonReader reader = new Persistence.JsonReader();
            string json = reader.GetJson();
            return DeserializeJson(json);
        }
    }
};