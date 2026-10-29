#region

using Runtime.Utility.Bootstrap;

#endregion

namespace Runtime.Gameplay.Bootstrap
{
    public class GameplaySceneArgs : ISceneArgs
    {
        public MiniGameType MiniGameType { get; }

        public GameplaySceneArgs(MiniGameType miniGameType)
        {
            MiniGameType = miniGameType;
        }
    }
}