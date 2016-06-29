namespace DungeonMasterEngine.DungeonContent.Actuators.Renderers
{
    public class ChampionDecoration : DecorationItem
    {
        public bool ShowChampion { get; set; }

        public ChampionDecoration(bool showChampion)
        {
            ShowChampion = showChampion;
        }
    }
}