using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Scripts.Sounds
{
    public class SoundSystem:ISoundSystem
    {
          private readonly List<SoundSystemAsset> m_LoadedAssets = new();
        private readonly Dictionary<string, AsyncOperationHandle<SoundSystemAsset>> m_LoadedHandlers = new();
        private readonly List<AudioSource> m_PlayedAudioSources = new();
        private readonly GameObject m_RootGameObject;

        private SoundSystem()
        {
            m_RootGameObject = new GameObject(nameof(SoundSystem));
            Object.DontDestroyOnLoad(m_RootGameObject);
        }

        async Task ISoundSystem.LoadSoundAsset(string addressablePath)
        {
            if (m_LoadedHandlers.ContainsKey(addressablePath))
                return;

            AsyncOperationHandle<SoundSystemAsset> resourceHandle = Addressables.LoadAssetAsync<SoundSystemAsset>(addressablePath);
            m_LoadedHandlers[addressablePath] = resourceHandle;

            if (!resourceHandle.IsDone)
            {
                await resourceHandle.Task;
            }

            if (resourceHandle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception();
            }

            m_LoadedAssets.Add(resourceHandle.Result);
        }

        void ISoundSystem.UnloadSoundAsset(string addressablePath)
        {
            if (!m_LoadedHandlers.ContainsKey(addressablePath))
                return;

            AsyncOperationHandle<SoundSystemAsset> loadedHandler = m_LoadedHandlers[addressablePath];
            SoundSystemAsset soundSystemAsset = loadedHandler.Result;
            StopAllAssetSounds(soundSystemAsset);
            m_LoadedAssets.Remove(soundSystemAsset);
            Addressables.Release(loadedHandler);
            m_LoadedHandlers.Remove(addressablePath);
        }
        
        async void ISoundSystem.InvokeEvent(string eventName)
        {
            foreach (SoundSystemAsset soundSystemAsset in m_LoadedAssets)
            {
                foreach (SoundSystemEvent soundSystemEvent in soundSystemAsset.SoundSystemEvents.Where(soundEvent =>
                             soundEvent.EventName == eventName))
                {
                    foreach (ISoundSystemActionConfig actionConfig in soundSystemEvent.Actions)
                    {
                        SoundSystemActionContext soundSystemActionContext = new()
                        {
                            Parent = m_RootGameObject.transform,
                            CurrentAsset = soundSystemAsset,
                            LoadedAssets = m_LoadedAssets,
                            AudioSources = m_PlayedAudioSources
                        };

                        actionConfig.Invoke(soundSystemActionContext);

                        if (actionConfig.WaitType == SoundSystemActionWaitType.RunNextActionAfterComplete)
                            await Awaiters.Until(() => actionConfig.IsCompleted);
                    }
                }
            }
        }

        private void StopAllAssetSounds(SoundSystemAsset soundSystemAsset)
        {
            foreach (AudioSource playedAudioSource in m_PlayedAudioSources.ToList().Where(playedAudioSource => soundSystemAsset.ClipGroups.Any(clipGroup => clipGroup.Clips.Contains(playedAudioSource.clip))))
            {
                playedAudioSource.Stop();
                playedAudioSource.clip = null;
                m_PlayedAudioSources.Remove(playedAudioSource);
                Object.Destroy(playedAudioSource.gameObject);
            }
        }
    }
}