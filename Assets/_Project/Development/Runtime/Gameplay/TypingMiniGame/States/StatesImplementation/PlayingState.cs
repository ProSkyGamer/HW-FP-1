namespace Runtime.Gameplay.TypingMiniGame.States.StatesImplementation
{
    public class PlayingState : IMiniGameStateHandler
    {
        private readonly TypingMiniGameHandler _typingMiniGameHandler;

        public PlayingState(TypingMiniGameHandler typingMiniGameHandler)
        {
            _typingMiniGameHandler = typingMiniGameHandler;
        }

        public void TransitionIntoThis()
        {
            _typingMiniGameHandler.StartMiniGame();
        }

        public void TransitionOutOfThis()
        {
            _typingMiniGameHandler.EndMiniGame();
        }
    }
}