using Forms.BuildingBlocks.Interfaces.DI;

namespace Forms.BuildingBlocks
{
    public static class BuildingBlocks
    {
        public static IContainer Container { get; private set; }
        public static void Init(IContainer container)
        {
            Container = container;
        }
    }
}
