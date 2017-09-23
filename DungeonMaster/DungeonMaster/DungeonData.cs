using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DungeonMasterParser.Descriptors;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using HtmlAgilityPack;
using static DungeonMasterParser.Enums.ObjectCategory;

namespace DungeonMasterParser
{
    public class DungeonData
    {
        private readonly Dictionary<string, DescriptorBase> descriptorMaping = new Dictionary<string, DescriptorBase>();

        public IList<GraphicsDescriptor> WallDecorations { get; }

        public IList<GraphicsDescriptor> FloorDecorations { get; }

        public IList<GraphicsDescriptor> DoorDecorations { get; }

        public IList<ItemDescriptor> ItemDescriptors { get; }

        public IList<CreatureDescriptor> CreatureDescriptors { get; }

        public IList<FightActionDescriptor> FightActions { get; }

        public IList<FightComboDescriptor> FightCombos { get; }

        public IList<WeaponDescriptor> WeaponDescriptors { get; }
        public IList<ArmorDescriptor> ArmorDescriptors { get; }
        public IList<ScrollDescriptor> ScrollDescriptors { get; }
        public IList<ContainerDescriptor> ContainerDescriptors { get; }
        public IList<PotionDescriptor> PotionDescriptors { get; }
        public IList<MiscDescriptor> MiscDescriptors { get; }

        public IList<DoorDescriptor> DoorDescriptors { get; }
        public IList<ChampionDescriptor> ChamptionDescriptors { get; }

        public DungeonData()
        {
            WallDecorations = GetWallTextureNames();
            FloorDecorations = GetFloorTextureNames();
            DoorDecorations = GetDoorTexturNames();

            ItemDescriptors = GetItemsDescriptors();
            ItemGlobalIdentifers = CreateGlobalIndexLookup();

            WeaponDescriptors = ParseWeaponDescriptors();
            ArmorDescriptors = ParseArmorDescriptors();
            ScrollDescriptors = ParseScrollDescriptors();
            ContainerDescriptors = ParseContainerDescriptors();
            PotionDescriptors = ParsePotionDescriptors();
            MiscDescriptors = ParseMiscDescriptors();
            DoorDescriptors = GetDoorDescriptors();
            ChamptionDescriptors = GetChamptionDescriptors();

            ConnectDescriptorToItemDescriptor();

            FightActions = ParseFightActions();
            FightCombos = ParseFightCombos();

            CreatureDescriptors = ParseCreatureDatas();
        }

        private IList<DoorDescriptor> GetDoorDescriptors()
        {
            return new[]
            {
                new DoorDescriptor
                {
                    Type = DoorType.GrateDoor,
                    Animated = false,
                    ItemsPassThrough = true,
                    CreatureSeeThrough = true,
                    Resistance = 0x6E,
                },
                new DoorDescriptor
                {
                    Type = DoorType.WoodenDoor,
                    Animated = false,
                    ItemsPassThrough = false,
                    CreatureSeeThrough = false,
                    Resistance = 0x2A,
                },
                new DoorDescriptor
                {
                    Type = DoorType.IronDoor,
                    Animated = false,
                    ItemsPassThrough = false,
                    CreatureSeeThrough = false,
                    Resistance = 0xE6,
                },
                new DoorDescriptor
                {
                    Type = DoorType.RaDoor,
                    Animated = true,
                    ItemsPassThrough = false,
                    CreatureSeeThrough = true,
                    Resistance = 0xFF,
                }
            };

            //http://dmweb.free.fr/?q=node/1395#toc19
            //03 6E Grate door: Not animated, items can pass through, creatures can see through, Resistance = 6E
            //00 2A Wooden door: Not animated, items cannot pass through, creatures cannot see through, Resistance = 2A
            //00 E6 Iron door: Not animated, items cannot pass through, creatures cannot see through, Resistance = E6
            //05 FF Ra door: Animated, items cannot pass through, creatures can see through, Resistance = FF
        }

        private IList<ChampionDescriptor> GetChamptionDescriptors()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/champions.html"));

            var champions = documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)//skipp header
                .Select(tr =>
                {
                    var res = new ChampionDescriptor()
                    {
                        Identifer = int.Parse(tr.Element("td").InnerText),
                        TexturePath = GetTextureName(tr, "./Textures/Champions/", 1, 0)
                    };
                    return res;
                })
                .ToArray();

            Array.Sort(champions, Comparer<ChampionDescriptor>.Create((x, y) => x.Identifer.CompareTo(y.Identifer)));
            return champions;
        }

        private Dictionary<int, ItemDescriptor> CreateGlobalIndexLookup()
        {
            return ItemDescriptors.ToDictionary(x => x.GlobalItemIndex);
        }

        public IReadOnlyDictionary<int, ItemDescriptor> ItemGlobalIdentifers { get; }

        private IList<MiscDescriptor> ParseMiscDescriptors()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/misc.html"));
            var foodRegex = new Regex("Food Value: *([0-9]+)", RegexOptions.IgnoreCase);
            var waterRegex = new Regex("Water Value: *([0-9]+)", RegexOptions.IgnoreCase);

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)//skip header
                .Select(tr =>
                {
                    var columns = tr.Elements("td").Select(td => td.InnerText).ToArray();
                    int value;
                    var res = new MiscDescriptor()
                    {
                        Name = columns[0].Trim(),
                        FoodValue = int.TryParse(foodRegex.Match(columns[3]).Groups[1].Value, out value) ? value : (int?)null,
                        WaterValue = int.TryParse(waterRegex.Match(columns[3]).Groups[1].Value, out value) ? value : (int?)null,
                        Weight = (int)(10 * float.Parse(columns[1].Trim(), CultureInfo.InvariantCulture)),
                        TexturePath = GetTextureName(tr, "./Textures/GrabableItems/")
                    };
                    descriptorMaping.Add(GetKey(res.Name), res);
                    return res;
                })
                .ToArray();
        }

        private IList<PotionDescriptor> ParsePotionDescriptors()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/potions.html"));

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)//skipp header
                .Select(tr =>
                {
                    var columns = tr.Elements("td").Select(td => td.InnerText).ToArray();
                    var res = new PotionDescriptor()
                    {
                        Name = columns[0].Trim(),
                        Weight = (int)(10 * float.Parse(columns[1].Trim(), CultureInfo.InvariantCulture)),
                        TexturePath = GetTextureName(tr, "./Textures/GrabableItems/")
                    };
                    descriptorMaping.Add(GetKey(res.Name), res);
                    return res;
                })
                .ToArray();
        }

        private IList<ContainerDescriptor> ParseContainerDescriptors()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/container.html"));

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)//skipp header
                .Select(tr =>
                {
                    var columns = tr.Elements("td").Select(td => td.InnerText).ToArray();
                    var res = new ContainerDescriptor()
                    {
                        Name = columns[0].Trim(),
                        Weight = (int)(10 * float.Parse(columns[1].Trim(), CultureInfo.InvariantCulture)),
                        TexturePath = GetTextureName(tr, "./Textures/GrabableItems/")
                    };
                    descriptorMaping.Add(GetKey(res.Name), res);
                    return res;
                })
                .ToArray();
        }

        private IList<ScrollDescriptor> ParseScrollDescriptors()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/scroll.html"));

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)//skipp header
                .Select(tr =>
                {
                    var columns = tr.Elements("td").Select(td => td.InnerText).ToArray();
                    var res = new ScrollDescriptor()
                    {
                        Name = columns[0].Trim(),
                        Weight = (int)(10 * float.Parse(columns[1].Trim(), CultureInfo.InvariantCulture)),
                        TexturePath = GetTextureName(tr, "./Textures/GrabableItems/")
                    };
                    descriptorMaping.Add(GetKey(res.Name), res);
                    return res;
                })
                .ToArray();
        }

        private IList<ArmorDescriptor> ParseArmorDescriptors()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/armor.html"));

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)//skipp header
                .Select(tr =>
                {
                    var columns = tr.Elements("td").Select(td => td.InnerText).ToArray();
                    var classDelta = columns[3].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    var res = new ArmorDescriptor
                    {
                        Name = columns[0].Trim(),
                        Weight = (int)(10 * float.Parse(columns[1].Trim(), CultureInfo.InvariantCulture)),
                        ArmorStrength = int.Parse(classDelta[0]),
                        SharpResistance = int.Parse(classDelta[1]),
                        TexturePath = GetTextureName(tr, "./Textures/GrabableItems/")
                    };
                    descriptorMaping.Add(GetKey(res.Name), res);
                    return res;
                })
                .ToArray();
        }

        private void ConnectDescriptorToItemDescriptor()
        {
            foreach (var desc in ItemDescriptors)
            {
                var key = GetKey(desc.Name);
                var descriptor = descriptorMaping[key];
                if (descriptor != null)
                    descriptor.Identifer = desc.InCategoryIndex;
            }
            var comparer = Comparer<DescriptorBase>.Create((w1, w2) => w1.Identifer.CompareTo(w2.Identifer));
            Array.Sort((DescriptorBase[])WeaponDescriptors, comparer);
            Array.Sort((DescriptorBase[])ArmorDescriptors, comparer);
            Array.Sort((DescriptorBase[])ScrollDescriptors, comparer);
            Array.Sort((DescriptorBase[])ContainerDescriptors, comparer);
            Array.Sort((DescriptorBase[])PotionDescriptors, comparer);
            Array.Sort((DescriptorBase[])MiscDescriptors, comparer);
        }

        private static string GetKey(string s)
        {
            Func<string, int> getLength = str =>
            {
                var length = str.IndexOfAny(new[] { '/', '\n', '\r', '(' });

                if (length == -1)
                    length = str.Length;
                return length;
            };

            return s.Substring(0, getLength(s)).Trim();
        }

        private IList<FightComboDescriptor> ParseFightCombos()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/combos.html"));

            var data = documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)
                .Select(tr => tr.Elements("td").Select(td => td.InnerText).ToArray())
                .ToArray();

            var res = new List<FightComboDescriptor>();
            for (int i = 0; i < data.Length; i += 3)
            {
                var entries = new List<ComboEntry>();
                for (int j = i; j < i + 3; j++)
                {
                    int offset = j == i ? 1 : 0;

                    int fightActionIndex = int.Parse(data[j][offset]);
                    if (fightActionIndex == 255)
                        break;

                    entries.Add(new ComboEntry
                    {
                        ActionDescriptor = FightActions[fightActionIndex],
                        UseCharges = int.Parse(data[j][offset + 2]),
                        MinimumSkillLevel = int.Parse(data[j][offset + 3])
                    });
                }

                res.Add(new FightComboDescriptor
                {
                    ComboIndex = i / 3,
                    Actions = entries
                });
            }

            return res;
        }

        private IList<FightActionDescriptor> ParseFightActions()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/actions.html"));

            string x = FightActionDescriptor.GenerateClassString(documet);

            var properties = FightActionDescriptor.GetPropertyNames(documet);

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)
                .Select(tr =>
                {
                    var res = new FightActionDescriptor();
                    foreach (var tuple in tr.Elements("td").Select(td => td.InnerText).Zip(properties, Tuple.Create))
                    {
                        int val;

                        if (tuple.Item2 == nameof(res.ImprovedSkill))
                        {
                            res.ImprovedSkill = int.Parse(tuple.Item1.Split(':')[0]);
                            continue;
                        }

                        if (tuple.Item2 == nameof(res.Number))
                        {
                            res.Number = (FightActionEnum)int.Parse(tuple.Item1);
                            continue;
                        }

                        if (int.TryParse(tuple.Item1, out val))
                            res.GetType().GetProperty(tuple.Item2).SetValue(res, val);
                        else
                            res.GetType().GetProperty(tuple.Item2).SetValue(res, tuple.Item1);
                    }
                    return res;
                })
                .ToArray();
        }

        public IList<CreatureDescriptor> ParseCreatureDatas()
        {
            HtmlDocument d = new HtmlDocument();
            d.LoadHtml(File.ReadAllText("Data/creatures.html"));

            var data = d.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Where(tr => tr.Elements("td").Count() > 1)
                .Select(tr => tr.Elements("td").Skip(0).Select(td => td.InnerText.Trim()).ToList())
                .ToArray();

            var names = d.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .First()
                .Elements("th")
                .Skip(1)
                .Select(x => x.InnerText)
                .ToArray();

            return names.Select((x, i) =>
            {
                var res = new CreatureDescriptor(x.Trim());

                foreach (var row in data)
                {
                    res.GetType().GetProperty(CreatureDescriptor.ConvertToPopertyName(row[0]))
                        .SetValue(res, int.Parse(row[i + 1]));
                }
                return res;
            }).ToArray();
        }

        private string GetTextureName(HtmlNode tr, string directory, int column = 0, int imageIndex = 0)
        {
            var pathx = tr.Elements("td").ElementAt(column).Elements("img").ElementAt(imageIndex).GetAttributeValue("src", null);
            return directory + Path.GetFileNameWithoutExtension(pathx);
        }

        private IList<WeaponDescriptor> ParseWeaponDescriptors()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/weapons.html"));

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)//skipp header
                .Select(tr =>
                {
                    var columns = tr.Elements("td").Select(td => td.InnerText).ToArray();
                    var classDelta = columns[6].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    int val;
                    var res = new WeaponDescriptor
                    {
                        Name = columns[0].Trim(),
                        Weight = (int)(10 * float.Parse(columns[1].Trim(), CultureInfo.InvariantCulture)),
                        Strength = int.Parse(columns[3]),
                        KineticEnergy = int.Parse(columns[4]),
                        ShootDamage = int.Parse(columns[5]),
                        Class = int.Parse(classDelta[0]),
                        DeltaEnergy = int.TryParse(classDelta[1], out val) ? (int?)val : null,
                        TexturePath = GetTextureName(tr, "./Textures/GrabableItems/")
                    };
                    descriptorMaping.Add(GetKey(res.Name), res);
                    return res;
                })
                .ToArray();
        }

        public ItemDescriptor GetItemDescriptor(ObjectCategory category, int categoryIndexType)
        {
            int baseIndex = GetCategoryBaseIndex(category);
            var index = baseIndex + categoryIndexType;
            return ItemDescriptors[index];
        }

        private static int GetCategoryBaseIndex(ObjectCategory category)
        {
            int baseIndex = 0;
            switch (category)
            {
                //                    +0(1 item type)
                case Scroll:
                    baseIndex = 0;
                    break;
                //                    +1(1 item types)
                case Container:
                    baseIndex = 1;
                    break;
                //                    +2(21 item types)
                case Potion:
                    baseIndex = 2;
                    break;
                //                    +23(46 item types)
                case Weapon:
                    baseIndex = 23;
                    break;
                //                    +69(58 item types)
                case Clothe:
                    baseIndex = 69;
                    break;
                //                    +127(53 item types)
                case Miscellenaous:
                    baseIndex = 127;
                    break;

                default: throw new NotSupportedException();
            }

            return baseIndex;
        }

        private IList<ItemDescriptor> GetItemsDescriptors()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/itemDescriptors.html"));

            var decimalVal = new Regex(@"\((0|[1-9][0-9]*)\)");

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)//skipp header
                .Select(tr =>
                {
                    var columns = tr.Elements("td").Select(td => td.InnerText).ToArray();
                    var res = new ItemDescriptor()
                    {
                        GlobalItemIndex = int.Parse(decimalVal.Match(columns[0]).Groups[1].Value),
                        FloorGraphicsIndex = int.Parse(decimalVal.Match(columns[1]).Groups[1].Value),
                        AttackCombo = int.Parse(decimalVal.Match(columns[2]).Groups[1].Value),
                        CarryLocation = (CarrryLocations)int.Parse(decimalVal.Match(columns[3]).Groups[1].Value),
                        TableIndex = int.Parse(columns[5]),
                        InCategoryIndex = int.Parse(columns[6]),
                        Category = (ObjectCategory)Enum.Parse(typeof(ObjectCategory), columns[7].Trim(), ignoreCase: true),
                        Name = columns[8].Trim()
                    };
                    return res;

                })
                .ToArray();
        }

        private IList<GraphicsDescriptor> GetWallTextureNames()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/wallgraphics.html"));

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Select(tr =>
                {
                    var columns = tr.Elements("td").Select(td => td.InnerText).ToArray();
                    var res = new GraphicsDescriptor()
                    {
                        Name = columns[0].Trim(),
                        Type = (GraphicsItemState)int.Parse(columns[1])
                    };
                    return res;
                })
                .ToArray();
        }

        private IList<GraphicsDescriptor> GetDoorTexturNames()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/doorgraphics.html"));

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Select(tr =>
                {
                    var columns = tr.Elements("td").Select(td => td.InnerText).ToArray();
                    var res = new GraphicsDescriptor()
                    {
                        Name = columns[0],
                        Type = (GraphicsItemState)int.Parse(columns[1])
                    };
                    return res;
                })
                .ToArray();
        }

        private IList<GraphicsDescriptor> GetFloorTextureNames()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/floorgraphics.html"));

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Select(tr =>
                {
                    var columns = tr.Elements("td").Select(td => td.InnerText).ToArray();
                    var res = new GraphicsDescriptor()
                    {
                        Name = columns[0],
                        Type = (GraphicsItemState)int.Parse(columns[1])
                    };
                    return res;
                })
                .ToArray();
        }


        //        Here is a detailed description of the contents of the DUNGEON.DAT file from Dungeon Master for PC.
        //This example is used to show the structure of a dungeon file.Offsets would be different in other dungeons.
        //Notes:

        //    This file is 33357 bytes long.

        //In this part of the document, each section of the file is entitled with the following information: Offset hexadecimal (offset decimal) Size of section in bytes - Description of section.
        //Below the title of each section are details about its content.

        #region 0000h(00000) 44 bytes - File header      

        //    1 word: 0063 (99) : Random seed and Dungeon ID.This value is used to define where to display random decorations on walls and floors.
        //   It is also a unique value identifying the dungeon.Hint Oracle hints files refer to this value to associate the dungeon with the hints file.It is stored in the header of the hints file as described on Technical Documentation - File Formats - Hint Oracle Hints File (HCSB.HTC).
        //    Values are 99 (Dungeon Master), 8 (Chaos Strikes Back Prison), 13 (Chaos Strikes Back Dungeon), 0 (Dungeon Master II).
        public int DungenSeed { get; set; }

        //    1 word: 2FFB(12283) : Size of global map data in bytes(total size of all maps)
        public int GlobalMapDataSize { get; set; }

        //    1 byte: 0E (14) : Number of maps(in Dungeon Master and Chaos Strikes Back, number of maps = number of levels)
        public int MapsCount { get; set; }

        //    1 byte:  (0) : Unused, padding

        //    1 word: 06D5 (1749): Text data size in words(1749 words* 2 = 3498 bytes)

        public int TextDataSize { get; set; }

        //    1 word: 0861 (00 10 00011 00001 : X=1, Y=3, south direction) : Starting party position(must start on map 0)
        //        Bits 15-12: Unused
        //        Bits 11-10: Direction
        //            '00' North
        //            '01' East
        //            '10' South
        //            '11' West
        //        Bits 9-5: Y coordinate
        //        Bits 4-0: X coordinate
        public MapPosition StartPosition { get; set; }

        //    1 word: 068F (1679) : Object list size in words(1679 words* 2 = 3358 bytes)
        public int ObjectListSize { get; set; }

        //    16 words: Number of objects of each type:

        //        1 word: 00AA(170) : Number of doors
        public int DoorsCount { get; set; }

        //        1 word: 00B3(179) : Number of teleporters
        public int TeleportsCount { get; set; }

        //        1 word: 007D (125) : Number of texts
        public int TextsCount { get; set; }

        //        1 word: 02AC(684) : Number of actuators
        public int ActuatorsCount { get; set; }

        //        1 word: 00B6(182) : Number of creatures
        public int CreaturesCount { get; set; }

        //        1 word: 006B(107) : Number of weapons
        public int WeaponsCount { get; set; }

        //        1 word: 0079 (121) : Number of clothes
        public int ClothesCount { get; set; }

        //        1 word: 0023 (35) : Number of scrolls
        public int ScrollsCount { get; set; }

        //        1 word: 0038 (56) : Number of potions
        public int PotionsCount { get; set; }

        //        1 word: 000C(12) : Number of containers
        public int ContainersCount { get; set; }

        //        1 word: 0118 (280) : Number of miscellaneous items
        public int MiscellaneousItemsCount { get; set; }

        //        3 words: 00 00 00 : Unused


        //        1 word: 00 (0) : Number of missiles
        public int MissilesCount { get; set; }

        //        1 word: 00 (0) : Number of clouds
        public int CloudsCount { get; set; }

        #endregion

        #region 002Ch(44) 224 bytes - Map definitions

        //Each map is defined by 16 bytes(14 maps* 16 bytes = 224 bytes)
        //The Dungeon Master dungeon contains 14 maps, one per level.

        //->>DungeonMap.cs
        public IList<DungeonMap> Maps { get; set; }


        //The Chaos Strikes Back Prison dungeon contains 2 maps, one per level.
        //The Chaos Strikes Back dungeon contains 11 maps, one per level.The last map is empty.
        //The Dungeon Master II dungeon contains 44 maps on 7 levels.Here is a short description of each map:

        //     Level 7: Hall of champions
        //    01 Level 1: Skullkeep, Roof
        //    02 Level 2: Skullkeep, Lightnings corridor, Skeleton and Onyx keys
        //    03 Level 2: Skullkeep, Door to access Dragoth map
        //     Level 3: Skullkeep, Rams pushing to pits
        //     Level 3: Skullkeep, Moving teleporters, Fireball reflection
        //    06 Level 4: Skullkeep, Multiple doors and switches
        //    07 Level 4: Skullkeep, Flying chest(below map 4)
        //    08 Level 5: Skullkeep, Vexirks and corridor of fire
        //    09 Level 5: Skullkeep, Zo pit, first machine room
        //    10 Level 6: Skullkeep, Boiler and Pyro
        //    11 Level 6: Skullkeep, Entrance
        //    12 Level 7: Dru Tan map, Blood key
        //    13 Level 8: Fire elementals
        //    14 Level 6: Thorn demons
        //    15 Level 6: Wolves(Second key)
        //    16 Level 6: Exterior: Glops(Beginning of game)
        //    17 Level 6: Cemetery
        //    18 Level 6: Merchant
        //    19 Level 6: Food merchant
        //    20 Level 7: Cave below wolves map
        //    21 Level 7: Leads below Skullkeep(Second Tempest)
        //    22 Level 6: Home, Altar of Vi
        //    23 Level 6: Temple: moving pits
        //    24 Level 6: Merchant
        //    25 Level 6: Trees, marsh
        //    26 Level 6: Gigglers, Fireballs
        //    27 Level 6: Non material creatures(First key)
        //    28 Level 6: Two merchants(clothes and weapons)
        //    29 Level 6: Axes map(Third key)
        //    30 Level 6: Above map 30, access to merchants
        //    31 Level 6: Merchant
        //    32 Level 6: Weapons merchant(Vorax)
        //    33 Level 6: Exterior: near Skullkeep entrance
        //    34 Level 6: Merchant
        //    35 Level 7: Cave: Bats
        //    36 Level 7: Below the temple(random doors)
        //    37 Level 6: Not accessible
        //    38 Level 10: Dragoth map
        //    39 Level 6: Exterior: Access to Wolves map
        //    40 Level 7: Skeletons below Skullkeep
        //    41 Level 6: Exterior: access to cemetery, back to temple, merchant
        //    42 Level 6: Exterior: access to axes map
        //    43 Level 6: Bats under Skulkeep(pits to close)

        #endregion

        #region 01 (268) 818 bytes - Index of tiles with objects on them(per column)

        //This section contains one word for each column in each map of the dungeon, from left to right and above to below.The sum of the widths of all maps is 409. (409 columns* 2 bytes = 818 bytes)

        //Each word contains the number of tiles having one(or more) object(s) on them encountered so far while browsing all the dungeon columns.The first word is always 0 because at the beginning of the very first column, no object has been encountered yet.The second word contains the number of tiles having objects on them in the first column.The third word contains the number of tiles having objects on them in the first and second columns, etc...Consequently, each word value is greater or equal to the previous value.The last word in this section contains the number of tiles having objects on them in the whole dungeon, excepted those in the last column of the last map.As there are no objects in the last column of the original Dungeon Master dungeon, the last word contains exactly the number of tiles having objects on them in the whole dungeon(this value is 1300).
        //The content of this section is used as a shortcut as it can be rebuilt easily.
        #endregion

        #region 043Eh (1086) 3358 bytes - List of object IDs of first objects on tiles

        //Each ID is defined by 2 bytes. 1679 (item list size) * 2 = 3358 bytes
        //There is free space at the end, filled with FF FF words.An FF FF word means that no object is referenced.
        //26 bytes used(13 objects), 758 (379 objects) FF bytes

        //Each tile has a bit to specify if there are objects on it.Tiles are ordered as in the dungeon file: from up to down, left to right, first map to the last map.The first object ID is located on the first tile having an object, etc...

        //When several objects are located on the same tile, only the first one is referenced here.Each object references the next object ID located on the same tile.The last object in a linked list has this next object ID: FFFEh

        //A given object should NOT be referenced by several other objects.The game engine does not allow this and will often crash or corrupt save games when you do so.However, there are few situations where it will work without causing any trouble.

        public IList<ObjectID> ObjectIDs { get; set; }

        #endregion

        #region 115Ch (4444) 3498 bytes - Text Data

        public MemoryStream TextDataStream { get; set; }

        //Dungeon Master and Chaos Strikes Back

        //Four types of text strings are found in a dungeon:

        //    Wall text: text that is displayed on a wall in the dungeon view
        //    Message: text that is displayed on the bottom of the screen(for example when entering the Prison in Chaos Strikes Back)
        //    Scroll text: text that is displayed when looking at a scroll in a champion's inventory
        //    Champion text: texts that are never displayed.They are used to store the initial champion name, skills and statistics.

        //Dungeon Master II

        //Champion names, skills and statistics are stored in the graphics.dat file.
        //Some English text data is present in all Dungeon Master II dungeon.dat files (even in the non English versions). However this text data is not used in the dungeon (at least never visible while playing the game), except in Dungeon Master for PC 0.9 Beta. All other versions of Dungeon Master II store all the real text data in the graphics.dat file instead.
        //Fonts

        //The game uses two fonts to print text on screen. There are two versions of each font, an 'old' font that is used only in Dungeon Master for Atari ST versions 1.0, 1.1 and 1.2 and a 'new' font that is used in all other versions of Dungeon Master and Chaos Strikes Back. The differences between the two fonts are highlighted on the images below:

        //    A font of 36 characters for wall texts (stored in graphics.dat item #120 in game engine versions 1.x and 2.x, or in item #258 in game engine versions 3.x)
        //    Font Used For Wall Texts
        //   A font of 128 characters for messages and scroll texts (stored in graphics.dat item #557 in game engine versions 1.x and 2.x and also in Dungeon Master for X680 3.0, or in item #695 in game engine versions 3.x)
        //    Font Used For Interface And Scrolls

        //Some versions include a third font in graphics.dat item #12 but it is never used. Later versions of the games do not include it anymore.
        //Encoding

        //Text data is stored as words. Each word contains 3 codes of 5 bits, and 1 unused bit:

        //    1 word: 3 characters
        //       Bit 15: Unused, always 0
        //        Bits 14-10: First code
        //       Bits 9-5: Second code
        //       Bits 4-0: Third code

        //Here is the meaning of each 5 bits code:
        //0 to 25: Letters 'A' to 'Z'
        //26: ' ' (Space)
        //27: '.'
        //28: Separator
        //29: Escape code 1
        //30: Escape code 2
        //31: End of text

        //When the game engine decodes dungeon text data, it outputs a string of bytes where each byte is the index of a character in one of the two fonts.Because the two fonts are quite different(numbers of characters and character indices) the game decodes wall texts differently than other texts.

        // Codes 0 to 27: 'A' to 'Z', space and '.'
        //        Wall text: a byte with the same value as the code is written in the ouput string.
        //        Other text: the ASCII code of the character is written in the output string (ASCII codes correspond to the appropriate character indices in the font).

        //    For example, if code 0 (letter 'A') is found in a wall text, a 00h byte is written in the output string because character 0 in the wall text font is an 'A'. However, if code 0 is found in another type of text, a 41h byte is written instead because the 'A' character has index 41h in the other font(this matches the ASCII code for 'A').
        //    Code 28: Separator
        //    The separator character is decoded differently depending on the type of text:
        //        Wall texts: Separator is decoded as a 80h byte.
        //        Message: Separator is decoded as a 'Space' character
        //        Scroll and champion texts: Separator is decoded as a 'Line Feed' character
        //    Code 29: Escape code 1
        //    When code 29 is found, the next code in the text data is used as an index in a table of zero terminated strings.The corresponding string in the table is appended to the output string. Nothing prevents this escape code from being used in a wall text however it cannot work because the character indices in the table are all higher than 35 which is the index of the last character in the font for wall texts.
        //    Each string in the table contains a single character (2 bytes per string). The table is identical in all versions of Dungeon Master and Chaos Strikes Back:

        //    61  62  63  64  65  66  67  68  
        //    69  6A  6B  6C  6D  6E  6F  70  
        //    71  72  73  74  75  76  77  78  
        //    30  31  32  33  34  35  36  37  

        //    Note that this escape code and the table above has no real use in the games.
        //    This escape code is used only twice in the French dungeons in the following versions:
        //        Dungeon Master for Atari ST version 1.3 FR
        //        Dungeon Master for Amiga version 2.0 FR
        //        Dungeon Master for PC version 3.4 EN FR GE

        //    Both times it is followed by code 1Ah which outputs a '2' character in the middle of strings where it is clearly a typo, in the message at (00,09,01) and in the scroll at(13,23,04).
        //    The values in the table seem to refer to the magic symbols in the font, but with a shift of 1.
        //    Code 30: Escape code 2
        //    When code 30 is found, the next code in the text data is used as an index in a table of zero terminated strings.The corresponding string in the table is appended to the output string. There are two distinct tables, one is used when decoding wall texts and the other when decoding other types of texts.
        //    Each string in the table contains up to 7 characters (8 bytes per string). The content of this table is not the same in all versions of Dungeon Master and Chaos Strikes Back.

        //    Table for Escape code 2 in Wall texts: Each byte in these strings is a character index in the font for wall texts

        //    Dungeon Master for Atari ST versions 1.0, 1.1 and 1.2 use this table:

        //                String (Hex)              Corresponding characters in font
        //    Code 0-1:             ''
        //    Code 2:     13 07  1A       'THE '
        //    Code 3:     18 0E 14 1A       'YOU '
        //    Codes 4-31:           ''

        //    These versions of the game use the 'old' font.

        //    Dungeon Master for Atari ST version 1.3 and all other versions of Dungeon Master and Chaos Strikes Back use this table:

        //                 String (Hex)              Corresponding characters in font
        //    Code 0:      1C          ' '
        //    Code 1:      1D          ' '
        //    Code 2:      13 07  1A       'THE '
        //    Code 3:      18 0E 14 1A       'YOU '
        //    Code 4:      1E          ' '
        //    Code 5:      1F          ' '
        //    Code 6:      20          ' '
        //    Code 7:      21          '''
        //    Code 8:      22          ' '
        //    Code 9:      23          ' '
        //    Codes 10-31:           ' '

        //    These versions of the game use the 'new' font.

        //    Table for Escape code 2 in other texts: Each byte in these strings is a character index in the font for interface and scrolls.
        //    Note that when printing scroll texts on screen, 64 is substracted from the character indices of letters(A to Z only) so that they use the characters #1 to #26 in the font instead of the default characters #65 to #90.
        //    Dungeon Master for Atari ST versions 1.0, 1.1 and 1.2 use this table:

        //                String (Hex)              Corresponding characters in font
        //    Code 0:     3F          '?'
        //    Code 1:     21          '!'
        //    Code 2:     54 48 45 20       'THE '
        //    Code 3:     59 4F 55 20       'YOU '
        //    Codes 4-31:           ''

        //    These versions of the game use the 'old' font.

        //    Dungeon Master for Atari ST version 1.3 and all other versions of Dungeon Master and Chaos Strikes Back use this table:

        //                 String (Hex)               Corresponding characters in font
        //    Code 0:      78          '?'          Bug: this code is used in texts
        //    Code 1:      79          ' '    <---- and should be '!' but character
        //    Code 2:      54 48 45 20       'THE '       #79h is empty in the font.
        //    Code 3:      59 4F 55 20       'YOU '
        //    Code 4:      7A          ' '
        //    Code 5:      7B          ' '
        //    Code 6:      7C          ' '
        //    Code 7:      7D          '''
        //    Code 8:      7E          ' '
        //    Code 9:      7F          ' '
        //    Codes 10-31:           ''

        //    These versions of the game use the 'new' font.

        //    Both tables are stored in item 559 in the Atari ST versions and directly in the executable in other versions (you can refer to Technical Documentation - Graphics.dat Item 559 for the Atari ST versions).
        //    Code 31: End of text
        //    When this code is found, text decoding stops.

        //In Dungeon Master and Chaos Strikes Back, the initial champion statistics and skills are stored as text in the dungeon file. In Dungeon Master II, this information is stored in the graphics.dat file.
        //These special texts have the following structure:

        //    Name: 7 characters maximum, followed by a carriage return.
        //    Title: 19 characters maximum. The title string is split in two parts of 17 + 2 characters.Each part is followed by a carriage return. The game concatenates the two parts of the title.
        //   Gender: 1 character(must be 'M' or 'F') followed by a carriage return. The gender makes not difference, it is not used by the game engine.
        //  Health: 4 characters.
        //  Stamina: 4 characters.This value is always 10 times the value displayed in the game.
        //    Mana: 4 characters followed by a carriage return.
        //    Luck: 2 characters.This statistic is not visible in the game.
        //    Strength: 2 characters.
        //  Dexterity: 2 characters.
        //  Wisdom: 2 characters.
        //  Vitality: 2 characters.
        //  Anti-magic: 2 characters.
        //  Anti-fire: 2 characters followed by a carriage return.
        //    Fighter: 4 characters, one for each hidden skill.
        //  Ninja: 4 characters, one for each hidden skill.
        //  Priest: 4 characters, one for each hidden skill.
        //  Wizard: 4 characters, one for each hidden skill, followed by an end of text character.

        //Notes:

        //  The name, title and gender use the same encoding as normal texts (see above).
        //    The statistics and skills use an hexadecimal encoding: only character codes 0 to 15 (0h to Fh) are allowed.Example: if in the champion text, the string for health consists of the four characters 0h, 0h, 3h and Bh then it must be interpreted as the hexadecimal value 0x003B which is equal to 59 health points in decimal.
        //    For the skills, each character specifies the initial starting level in each hidden skill and not the initial starting experience points.
        //    The visible skill levels are computed by adding the experience points (not levels) in the 4 hidden skills and then convert this experience amount to the corresponding skill level.
        //    Please refer to the Technical Documentation - Dungeon Master and Chaos Strikes Back Skills and Statistics for the link between experience points and experience levels.
        //    Dungeon Master II requires double amounts of experience points for the same experience level compared to Dungeon Master and Chaos Strikes Back.

        #endregion

        #region 1F06h (7942) 680 bytes - List of doors

        //Each door is defined by 4 bytes. (170 doors * 4 bytes = 680 bytes)

        //->>Door.cs
        public IList<DoorItem> Doors { get; set; }

        #endregion

        #region 21AEh(8622) 1074 bytes - List of teleporters

        //Each teleporter is defined by 6 bytes. (179 doors* 6 bytes = 1074 bytes)
        //Teleporter objects must always be created on teleporter tiles.

        //->>Teleport.cs
        public IList<TeleporterItem> Teleports { get; set; }

        #endregion

        #region 25E0h(9696) 5 bytes - List of texts(wall texts, champion skills and statistics, messages)

        //Each text is defined by 4 bytes. (125 texts* 4 bytes = 5 bytes)

        //->>TextData.cs
        public IList<TextDataItem> Texts { get; set; }

        #endregion

        #region 27D4h (10196) 5472 bytes - List of actuators

        //Each actuator is defined by 8 bytes. (684 actuators* 8 bytes = 5472 bytes)

        //->>Actuator.cs
        public IList<ActuatorItemData> Actuators { get; set; }

        #endregion

        #region 3D34h(15668) 2912 bytes - List of creatures

        //Each creature is defined by 16 bytes. (182 creatures * 16 bytes = 2912 bytes)

        //->>Creature.cs
        public IList<CreatureItem> Creatures { get; set; }

        #endregion

        #region 4894h(18580) 428 bytes - List of weapons

        //Each weapon is defined by 4 bytes. (107 weapons * 4 bytes = 428 bytes)
        public IList<WeaponItemData> Weapons { get; set; }

        #endregion

        #region 4A40h(19008) 484 bytes - List of clothes

        //Each clothe is defined by 4 bytes. (121 clothes * 4 bytes = 484 bytes)

        public IList<ClothItemData> Clothes { get; set; }

        #endregion

        #region 4C24h(19492) 140 bytes - List of scrolls

        //Each scroll is defined by 4 bytes. (35 scrolls * 4 bytes = 680 bytes)

        public IList<ScrollItemData> Scrolls { get; set; }


        #endregion

        #region 4CB0h(19632) 224 bytes - List of potions

        //Each potion is defined by 4 bytes. (56 potions * 4 bytes = 224 bytes)

        public IList<PotionItemData> Potions { get; set; }


        #endregion

        #region 4D90h(19856) 96 bytes - List of containers

        //Each container is defined by 8 bytes. (12 containers * 8 bytes = 96 bytes)

        public IList<ContainerItemData> Containers { get; set; }

        #endregion

        #region  4DF0h(19952) 1120 bytes - List of miscellaneous items

        // Each miscellaneous item is defined by 4 bytes. (280 miscellaneous items * 4 bytes = 1120 bytes)

        public IList<MiscellaneousItemData> MiscellaneousItems { get; set; }

        #endregion

        #region 4DF0h(19952) 0 bytes - List of missiles

        //Each missile is defined by 8 bytes.

        //    1 word: Next object ID.
        //    1 word: Missile object (dagger, fireball, ...)
        //    1 byte: Range Energy remaining.This value is decreased each time the missile moves.When it reaches 0, the missile stops moving. This value is used to compute missile damage when a collision occurs.
        //    1 byte: Damage Energy remaining.This value is decreased each time the missile moves.This value is also used to compute missile damage when a collision occurs.
        //    1 word:
        //    Timer index


        #endregion

        #region 4DF0h(19952) 0 bytes - List of clouds

        //Each cloud is defined by 4 bytes.

        //    1 word: Next object ID.
        //    1 word:
        //    Bits 15 - 8: Value.Like Damage.
        //        Bit 7: Unknown
        //        Bit 6 - 0: Type(50 is special to creatures ?)(fluxcage ?)


        #endregion

        #region 5250h(21072) 12283 bytes - Map data

        //Each map is defined by a variable number of bytes. For each map in order, there is one byte per tile, plus one byte for each creature graphic, wall decoration graphic, floor decoration graphic and door decoration graphics(in that order).The number of graphics of each kind is specified in the map definition section.

        //Each tile is defined by 1 byte.

        //   1 byte: Tile definition
        //        Bits 7 - 5: Tile type:
        //            '000'(0) Wall
        //            '001'(1) Floor
        //            '010'(2) Pit
        //            '011'(3) Stairs
        //            '100'(4) Door
        //            '101'(5) Teleporter
        //            '110'(6) Trick wall
        //            '111'(7) Empty tile.Only valid in Dungeon Master II.
        //       Bit 4: Object(s) on this tile
        //            '0' No object on this tile
        //            '1' A list of object on this tile
        //        Bits 3 - 0: Tile attributes depending on tile type:
        //            Wall
        //                Bit 3:
        //                    '0' Do not allow random decoration on North side
        //                    '1' Allow random decoration on North side
        //                Bit 2:
        //                    '0' Do not allow random decoration on East side
        //                    '1' Allow random decoration on East side
        //                Bit 1:
        //                    '0' Do not allow random decoration on South side
        //                    '1' Allow random decoration on South side
        //                Bit 0:
        //                    '0' Do not allow random decoration on West side
        //                    '1' Allow random decoration on West side
        //            Floor
        //                Bit 3:
        //                    '0' Do not allow random decoration
        //                    '1' Allow random decoration
        //                Bit 2 - 0: Unused
        //            Pit
        //                Bit 0:
        //                    '0' Normal
        //                    '1' Imaginary
        //                Bit 1: Unused
        //                Bit 2:
        //                    '0' Visible
        //                    '1' Invisible
        //                Bit 3:
        //                    '0' Closed
        //                    '1' Open
        //            Stairs
        //                Bit 3: Orientation
        //                    '0' West - East
        //                    '1' North - South
        //                Bit 2: Direction
        //                    '0' Down
        //                    '1' Up
        //                Bit 1 - 0: Unused
        //            Door
        //                Bit 2 - 0: State
        //                    '000' Open
        //                    '001' 1 / 4 closed
        //                    '010' 1 / 2 closed
        //                    '011' 3 / 4 closed
        //                    '100' Closed
        //                    '101' Bashed
        //                    '110' Invalid
        //                    '111' Invalid
        //                Bit 3: Orientation
        //                    '0' West - East
        //                    '1' North - South
        //            Teleporter
        //                Bit 1 - 0: Unused
        //                Bit 2: Visibility
        //                    '0' Invisible
        //                    '1' Visible(blue haze)
        //                Bit 3:
        //                    '0' Closed
        //                    '1' Open
        //            Trick wall
        //                Bit 0:
        //                    '0' False
        //                    '1' Imaginary
        //                Bit 1: Unused
        //                Bit 2:
        //                    '0' Closed
        //                    '1' Open
        //                Bit 3:
        //                    '0' Do not allow random decoration
        //                    '1' Allow random decoration
        //            Empty tile
        //                Bit 3 - 0: Unused.

        //Each creature graphic is defined by 1 byte.The number of bytes is specified in the map definition.

        //Each wall decoration graphic is defined by 1 byte.The number of bytes is specified in the map definition.Here is the list of possible values for Dungeon Master:

        //     Unreadable Wall Inscription
        //    01 Square Alcove
        //    02 Vi Altar
        //    03 Arched Alcove
        //    04 Hook
        //    05 Iron Lock
        //    06 Wood Ring
        //    07 Small Switch
        //    08 Dent 1
        //    09 Dent 2
        //    10 Iron Ring
        //    11 Crack
        //    12 Slime Outlet
        //    13 Dent 3
        //    14 Tiny Switch
        //    15 Green Switch Out
        //    16 Blue Switch Out
        //    17 Coin Slot
        //    18 Double Iron Lock
        //    19 Square Lock
        //    20 Winged Lock
        //    21 Onyx Lock
        //    22 Stone Lock
        //    23 Cross Lock
        //    24 Topaz Lock
        //    25 Skeleton Lock
        //    26 Gold Lock
        //    27 Tourquoise Lock
        //    28 Emerald Lock
        //    29 Ruby Lock
        //    30 Ra Lock
        //    31 Master Lock
        //    32 Gem Hole
        //    33 Slime
        //    34 Grate
        //    35 Fountain
        //    36 Manacles
        //    37 Ghoul's Head
        //    38 Empty Torch Holder
        //    39 Scratches
        //    40 Poison Holes
        //    41 Fireball Holes
        //    42 Dagger Holes
        //    43 Champion Mirror
        //    44 Lever Up
        //    45 Lever Down
        //    46 Full Torch Holder
        //    47 Red Switch Out
        //    48 Eye Switch
        //    49 Big Switch Out
        //    50 Crack Switch Out
        //    51 Green Switch In
        //    52 Blue Switch In
        //    53 Red Switch In
        //    54 Big Switch In
        //    55 Crack Switch In
        //    56 Amalgam(Encased Gem)
        //    57 Amalgam(Free Gem)
        //    58 Amalgam(Without Gem)
        //    59 Lord Order(Outside)

        //Each floor decoration graphic is defined by 1 byte.The number of bytes is specified in the map definition.Here is the list of possible values for Dungeon Master:

        //     Square Grate
        //    01 Square Pressure Pad
        //    02 Moss
        //    03 Round Grate
        //     Round Pressure Plate
        //     Black Flame Pit
        //    06 Crack
        //    07 Tiny Pressure Pad
        //    08 Puddle

        //Each door decoration graphic is defined by 1 byte.The number of bytes is specified in the map definition.Here is the list of possible values for Dungeon Master:

        //     Square Grid
        //    01 Iron Bars
        //    02 Jewels
        //    03 Wooden Bars
        //     Arched Grid
        //     Block Lock
        //    06 Corner Lock
        //    07 Black door(Dungeon Entrance)
        //    08 Red Triangle Lock
        //    09 Triangle Lock
        //    10 Ra Door Energy
        //    11 Iron Door Damages


        #endregion

        #region 824Bh(33355) 1 word - Checksum

        //This word contains the checksum of all the previous data in the file.

        //Tested on PC34ML version(to be tested with other versions): if checksum is not present, no problem. If present and correct value, no problem. If present but wrong value, then the message "The game is damaged!" is displayed by the game.
        //824Dh(33357) 0 bytes - End of file
        //Compressed dungeons

        //This section describes the format of a compressed dungeon file. Once uncompressed, the data obtained has exactly the same format as an uncompressed dungeon.
        //0000h(00000) 28 bytes - File header

        //    1 word: 8104(33028) : File signature. If a dungeon file starts with this word value, the game will know that the dungeon is compressed.
        //    1 dword: Size of the uncompressed dungeon data.
        //    1 word: Unknown use.   or 0008h have been encountered.
        //    4 bytes: List of the 4 most common byte values in the dungeon data.
        //    16 bytes: List of the 16 less common byte values in the dungeon data.

        //0000h(00028) x bytes -Compressed data

        //The most common bytes in the uncompressed dungeon data are encoded as three bits '0xx'.The two bits after the first '0' bit define the index in the table of most common bytes.
        //The less common bytes in the uncompressed dungeon data are encoded as six bits '10xxxx'.The four bits after the first two '10' bits define the index in the table of less common bytes.
        //The least common bytes in the uncompressed dungeon data are encoded as ten bits '11xxxxxxxx'.The eight bits after the first two '11' bits directly define the byte value.

        //In each byte of compressed data, bits are read from the most significant one to the least significant one. Once all bits in a byte have been used, the next byte is read from the file.

        #endregion
    }
}
