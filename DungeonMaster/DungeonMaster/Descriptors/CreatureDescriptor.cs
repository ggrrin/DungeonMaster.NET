using System;
using System.Linq;
using HtmlAgilityPack;

namespace DungeonMasterParser.Descriptors
{

    //http://dmweb.free.fr/?q=node/1363
    public class CreatureDescriptor
    {
        public static string ConvertToPopertyName(string name)
        {
            return string.Concat(name.Trim()
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => char.ToUpper(x[0]) + (x.Length > 1 ? x.Substring(1) : "")))
                    .Replace("-", "")
                    .Replace(Environment.NewLine, "");

        }

        public static void GenerateClassString(HtmlDocument d)
        {
            var properties = d.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Where(tr => tr.Elements("td").Count() > 1)
                .Select(tr => ConvertToPopertyName(tr.Elements("td").First().InnerText))
                .ToArray();

            var propStr = string.Concat(properties.Select(x => $"public int {x} {{get; set;}}"));
            var clascsx = $"public class CreatureData  {{ {propStr} }}";
        }

        public string Name { get; set; }

        public CreatureDescriptor(string name)
        {
            Name = name;
        }

        public int IndexOfGraphicDefinition { get; set; }
        public int IndexOfAttackSoundDefinition { get; set; }
        public int Size { get; set; }
        public int SideAttack { get; set; }
        public int PreferBackRow { get; set; }
        public int AttackAnyChampion { get; set; }
        public int Levitation { get; set; }
        public int Nonmaterial { get; set; }
        public int Height { get; set; }
        public int DropsItems { get; set; }
        public int AbsorbItems { get; set; }
        public int SeeInvisible { get; set; }
        public int NightVision { get; set; }
        public int Archenemy { get; set; }
        public int AdditionalFrontImageGraphics { get; set; }
        public int MirrorFrontImage { get; set; }
        public int SideGraphic { get; set; }
        public int BackGraphic { get; set; }
        public int AttackGraphic { get; set; }
        public int MirrorAndShiftFrontImageAtDistance2 { get; set; }
        public int MirrorAttackImage { get; set; }
        public int MirrorAttackImageDuringAttack { get; set; }
        public int MaximumHorizontalOffset { get; set; }
        public int MaximumVerticalOffset { get; set; }
        public int MovementDuration { get; set; }
        public int AttackDuration { get; set; }
        public int Armor { get; set; }
        public int BaseHealth { get; set; }
        public int AttackPower { get; set; }
        public int Poison { get; set; }
        public int Defense { get; set; }
        public int SightRange { get; set; }
        public int DetectionRange { get; set; }
        public int SpellCastingRange { get; set; }
        public int Bravery { get; set; }
        public int ExperienceClass { get; set; }
        public int Suicidal { get; set; }
        public int FireResistance { get; set; }
        public int PoisonResistance { get; set; }
        public int AttackDisplayDuration { get; set; }
        public int AnimationSpeed { get; set; }
        public int AttackAnimationSpeed { get; set; }
        public int WoundFeet { get; set; }
        public int WoundLegs { get; set; }
        public int WoundTorso { get; set; }
        public int WoundHead { get; set; }
        public int DamageType { get; set; }
        public int Word02Bits1514 { get; set; }
        public int Word04Bit6 { get; set; }
        public int Word04Bit11 { get; set; }
        public int Byte0D { get; set; }
        public int Word0EBits74 { get; set; }
        public int Word10Bits30 { get; set; }
        public int Word12Bits30 { get; set; }
        public int Word12Bits1512 { get; set; }
        public int Word14Bits1512 { get; set; }
        public int Byte19 { get; set; }
    }
}
