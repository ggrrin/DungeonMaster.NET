namespace DungeonMasterEngine.DungeonContent.GrabableItems.Initializers
{
    public interface IWeaponInitializer
    {
        bool IsBroken { get;  }

        int ChargeCount { get;  }

        bool IsPoisoned { get;  }

        bool IsCursed { get;  }
    }
}