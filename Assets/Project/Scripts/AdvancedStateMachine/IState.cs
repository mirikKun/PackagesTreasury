namespace Project.Scripts.AdvancedStateMachine {
    public interface IState {
        void Update() { }
        void FixedUpdate() { }
        void OnEnter() { }
        void OnExit() { }
        void Dispose() { }
    }
}