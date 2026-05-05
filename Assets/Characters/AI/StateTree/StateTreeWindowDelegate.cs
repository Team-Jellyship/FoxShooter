using FoxShooter.Characters.AI.StateTree.Graph;

namespace FoxShooter.Characters.AI.StateTree.UI
{
    /**
     * This class exists to allow the runtime only class
     * to open the window
     */
    public static class StateTreeWindowDelegate
    {
        public delegate void OpenHandler(StateTreeGraph asset);

        public static OpenHandler handler;

        public static void Open(StateTreeGraph asset)
        {
            handler?.Invoke(asset);
        }
    }
}