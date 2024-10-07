using System;
using Common.AI.Points;
using ServiceLocator;

namespace Common.AI.GameMaster
{
    [Serializable]
    public class GMSettings : IService
    {
        public Room Level1;
        public Room Level2;
        public Room Level3;
    }
}