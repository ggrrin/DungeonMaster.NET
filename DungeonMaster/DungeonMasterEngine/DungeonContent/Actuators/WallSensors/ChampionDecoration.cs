namespace DungeonMasterEngine.DungeonContent.Actuators.WallSensors
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