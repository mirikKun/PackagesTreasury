using System.Threading.Tasks;

namespace Project.Scripts.Sounds
{
    public interface ISoundSystem
    {
        Task LoadSoundAsset(string addressablePath);
        
        void UnloadSoundAsset(string addressablePath);
        
        void InvokeEvent(string eventName);
    }
}