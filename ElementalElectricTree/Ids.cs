using SRML.Utils.Enum;

#pragma warning disable 0649

namespace ElementalElectricTree
{
    [EnumHolder]
    public class Ids
    {
        public static readonly Identifiable.Id ELECTRIC_PLORT;

        public static readonly Identifiable.Id ELECTRIC_SLIME;
        public static readonly PediaDirector.Id ELECTRIC_SLIME_ENTRY;
        public static readonly Identifiable.Id FORM_2_ELECTRIC_SLIME;
        
        public static readonly PediaDirector.Id FORM_2_ELECTRIC_SLIME_ENTRY;

        public static readonly Identifiable.Id PLASMA_SLIME;
        public static readonly PediaDirector.Id PLASMA_SLIME_ENTRY;

        public static readonly Identifiable.Id ELECTRIC_HEN;
        public static readonly Identifiable.Id ELECTRIC_ROOSTER;

        public static readonly Vacuumable.LaunchSource NONE;

        public static readonly LandPlot.Upgrade ELECTROMETER;

        public static readonly ExchangeDirector.NonIdentReward REWARD_ELECTRIC_CONTAINER_UPGRADE;

        public static readonly Identifiable.Id ELECTRIC_CANNON_SHOT;

        public static readonly ZoneDirector.Zone ELECTRIC_ISLAND;
    }
}
#pragma warning restore 0649