using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.Sounds
{
    public class SoundSystemAssetLoadComponent : MonoBehaviour
    {
        [SerializeField]
        private string m_AssetAddressablePath;

        [SerializeField]
        private bool m_LoadOnAwake = true;

        [SerializeField]
        private UnityEvent m_OnAssetLoaded;

        [SerializeField]
        private bool m_UnloadOnDestroy = true;

        private ISoundSystem m_SoundSystem;

        //[Inject]
        private void Construct(ISoundSystem soundSystem)
        {
            m_SoundSystem = soundSystem;
        }

        private void Awake()
        {
            if (m_LoadOnAwake)
                LoadSoundAsset();
        }

        private void OnDestroy()
        {
            if (m_UnloadOnDestroy)
                UnloadSoundAsset();
        }
        
        public async void LoadSoundAsset()
        {
            await m_SoundSystem.LoadSoundAsset(m_AssetAddressablePath);
            m_OnAssetLoaded.Invoke();
        }
        
        public void UnloadSoundAsset()
        {
            m_SoundSystem.UnloadSoundAsset(m_AssetAddressablePath);
        }
    }
}