using Helpers;

namespace AGP_Warcraft
{
    public sealed class GameEconomy : MonoSingleton<GameEconomy>
    {
        public float MineActivationFrequency;
        public int MineYieldedQuantity;
        public int MineCost;
        public int MineMaintenance;

        public float GoldSpawnFrequency;

        public float GoldValue;
        public float StoneValue;

        //Advanced feature
        public int StonesToStore;
        public int TotalStonesNeeded;
        public int StonesDemand;
    }
}
