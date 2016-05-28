using System.Linq;
using HtmlAgilityPack;

namespace DungeonMasterParser
{
    public class FightActionDescriptor
    {

        public static string[] GetPropertyNames(HtmlDocument d)
        {
            return d.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .First()
                .Elements("th")
                .Select(th => CreatureData.ConvertToPopertyName(th.InnerText))
                .ToArray();
        }

        public static string GenerateClassString(HtmlDocument d)
        {
            int v;
            var properties = GetPropertyNames(d);

            var values = d.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .ElementAt(1)
                .Elements("td")
                .Select(th => int.TryParse(th.InnerText, out v) ? "int" : "string")
                .ToArray();

            var propStr = string.Concat(properties.Zip(values, (prop, type) => $"public {type} {prop} {{get; set;}}"));
            return $"public class CreatureData  {{ {propStr} }}";
        }


        public int Number { get; set; }
        public string Name { get; set; }
        public string ImprovedSkill { get; set; }
        public int ExperienceGain { get; set; }
        public int DefenseModifier { get; set; }
        public int Stamina { get; set; }
        public int HitProbability { get; set; }
        public int Damage { get; set; }
        public int Fatigue { get; set; }
    }
}