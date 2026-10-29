namespace Runtime.Gameplay.TypingMiniGame.States
{
    public interface IMiniGameStateHandler
    {
        public void TransitionIntoThis();
        public void TransitionOutOfThis();
    }
}