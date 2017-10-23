namespace DukeLabs.Core.UI.Component.Interface
{
    public interface IUpdatable
    {
        void OnUpdate();
        void OnFixedUpdate();
        void OnLateUpdate();
        
        bool IsFixed { get; }
        bool IsLate { get; }
    }
}