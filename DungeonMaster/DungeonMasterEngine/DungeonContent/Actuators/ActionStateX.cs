namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class ActionStateX
    {
        public int TimeDelay { get; }
        public bool IsOnceOnly { get; }
        public ActionState Action { get; }

        public int Specifer { get; }

        public bool Used { get; set; } = false;

        public ActionStateX(ActionState action, int timeDelay, bool isOnceOnly, int specifer = -1)
        {
            this.TimeDelay = timeDelay;
            IsOnceOnly = isOnceOnly;
            Action = action;
            Specifer = specifer;
        }


        public override string ToString()
        {
            return $"action: {Action} delay: {TimeDelay} onceOnly: {IsOnceOnly}"; 
        }
    }
}