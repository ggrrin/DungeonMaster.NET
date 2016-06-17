using System.Linq;
using HtmlAgilityPack;

namespace DungeonMasterParser.Descriptors
{
    public class FightActionDescriptor
    {

        public static string[] GetPropertyNames(HtmlDocument d)
        {
            return d.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .First()
                .Elements("th")
                .Select(th => CreatureDescriptor.ConvertToPopertyName(th.InnerText))
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


        public FightActionEnum Number { get; set; }
        public string Name { get; set; }
        public int ImprovedSkill { get; set; }
        public int ExperienceGain { get; set; }
        public int DefenseModifier { get; set; }
        public int Stamina { get; set; }
        public int HitProbability { get; set; }
        public int Damage { get; set; }
        public int Fatigue { get; set; }
    }

    public enum FightActionEnum
    {
        C000_ACTION_N = 0
        , C001_ACTION_BLOCK = 1
        , C002_ACTION_CHOP = 2
        , C003_ACTION_X = 3
        , C004_ACTION_BLOW_HORN = 4
        , C005_ACTION_FLIP = 5
        , C006_ACTION_PUNCH = 6
        , C007_ACTION_KICK = 7
        , C008_ACTION_WAR_CRY = 8
        , C009_ACTION_STAB = 9
        , C010_ACTION_CLIMB_DOWN = 10
        , C011_ACTION_FREEZE_LIFE = 11
        , C012_ACTION_HIT = 12
        , C013_ACTION_SWING = 13
        , C014_ACTION_STAB = 14
        , C015_ACTION_THRUST = 15
        , C016_ACTION_JAB = 16
        , C017_ACTION_PARRY = 17
        , C018_ACTION_HACK = 18
        , C019_ACTION_BERZERK = 19
        , C020_ACTION_FIREBALL = 20
        , C021_ACTION_DISPELL = 21
        , C022_ACTION_CONFUSE = 22
        , C023_ACTION_LIGHTNING = 23
        , C024_ACTION_DISRUPT = 24
        , C025_ACTION_MELEE = 25
        , C026_ACTION_X = 26
        , C027_ACTION_INVOKE = 27
        , C028_ACTION_SLASH = 28
        , C029_ACTION_CLEAVE = 29
        , C030_ACTION_BASH = 30
        , C031_ACTION_STUN = 31
        , C032_ACTION_SHOOT = 32
        , C033_ACTION_SPELLSHIELD = 33
        , C034_ACTION_FIRESHIELD = 34
        , C035_ACTION_FLUXCAGE = 35
        , C036_ACTION_HEAL = 36
        , C037_ACTION_CALM = 37
        , C038_ACTION_LIGHT = 38
        , C039_ACTION_WINDOW = 39
        , C040_ACTION_SPIT = 40
        , C041_ACTION_BRANDISH = 41
        , C042_ACTION_THROW = 42
        , C043_ACTION_FUSE = 43

    }
}