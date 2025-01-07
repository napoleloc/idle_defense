using Cysharp.Threading.Tasks;
using EncosyTower.Modules.Vaults;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Audio.Sounds
{
    public class SoundPool : AudioPool, ISoundPool
    {
        [BoxGroup("Debugging", centerLabel: true)]
        [SerializeField, ReadOnly]
        private SoundContainer _container;

        protected override void OnAwake()
        {
            base.OnAwake();

            GlobalObjectVault.TryGet(SoundContainer.PresetId, out _container);
        }

        public void PlaySound()
        {
            if(Validate() == false)
            {
                return;
            }

            PlaySoundAndForget().Forget();
        }

        public async UniTask PlaySoundAsync()
        {
            if(Validate() == false)
            {
                return;
            }

            await PlaySoundAsyncInternal();
        }

        public void PauseSound()
        {
            if(Validate() == false || isPaused == true || isPlaying == false)
            {
                return;
            }

            audioSource.Pause();
            isPaused = true;
        }

        public void UnpauseSound()
        {
            if(Validate() == false || isPaused == false || isPlaying == false)
            {
                return;
            }

            audioSource.UnPause();
            isPaused = false;
        }

        public void StopSound()
        {
            if(Validate() == false)
            {
                return;
            }

            audioSource.Stop();
            _container.ReturnToPool(this);
        }

        private async UniTaskVoid PlaySoundAndForget()
        {
            await PlaySoundAsyncInternal();
        }

        private async UniTask PlaySoundAsyncInternal()
        {
            audioSource.Play();
            isPlaying = audioSource.isPlaying;

            var token = this.GetCancellationTokenOnDestroy();
            await UniTask.WaitUntil(() => audioSource.isPlaying == false && isLoop == false, cancellationToken: token);

            _container.ReturnToPool(this);
        }
    }
}
