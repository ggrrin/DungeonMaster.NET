namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class ActionStateX
    {
        public ActionState Action { get; }

        public int Specifer { get; }

        public ActionStateX(ActionState action, int specifer = -1)
        {
            Action = action;
            Specifer = specifer;
        }
    }
}