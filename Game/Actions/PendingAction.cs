namespace SWDB.Game.Actions
{
    public delegate void Callback();
    
    public class PendingAction
    {
        public Action Action { get; private set; }
        private Callback? Callback { get; set; }
        public bool ShouldPassAction { get; private set; }

        public static PendingAction Of(Action action) {
            return new PendingAction(action);
        }

        public static PendingAction Of(Action action, Callback callback) {
            return new PendingAction(action, callback);
        }

        public static PendingAction Of(Action action, bool shouldPassTurn) {
            return new PendingAction(action, shouldPassTurn);
        }

        public static PendingAction Of(Action action, Callback callback, bool shouldPassTurn) {
            return new PendingAction(action, callback, shouldPassTurn);
        }

        public PendingAction(Action action) {
            Action = action;
            Callback = null;
            ShouldPassAction = false;
        }

        public PendingAction(Action action, Callback callback) {
            Action = action;
            Callback = callback;
            ShouldPassAction = false;
        }

        public PendingAction(Action action, bool shouldPassAction) {
            Action = action;
            Callback = null;
            ShouldPassAction = shouldPassAction;
        }

        public PendingAction(Action action, Callback callback, bool shouldPassAction) {
            Action = action;
            Callback = callback;
            ShouldPassAction = shouldPassAction;
        }

        public void ExecuteCallback()
        {
            Callback?.Invoke();
        }
    }
}