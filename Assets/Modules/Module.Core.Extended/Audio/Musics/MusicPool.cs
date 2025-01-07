using Cysharp.Threading.Tasks;
using EncosyTower.Modules.Vaults;
using Sirenix.OdinInspector;

namespace Module.Core.Extended.Audio.Musics
{
    public class MusicPool : AudioPool, IMusicPool
    {
        [BoxGroup("Debugging", centerLabel: true)]
        private MusicContainer _musicContainer;

        protected override void OnAwake()
        {
            base.OnAwake();

            GlobalObjectVault.TryGet(MusicContainer.PresetId, out _musicContainer);
        }

        public void PlayMusic()
        {
            if(Validate() == false)
            {
                return;
            }

            PlayMusicAndForget().Forget();
        }

        public async UniTask PlayMusicAsync()
        {
            if(Validate() == false)
            {
                return;
            }

            await PlayMusicAsyncInternal();
        }

        public void PauseMusic()
        {
            if(Validate() == false || isPaused == true || isPlaying == false)
            {
                return;
            }
        }

        public void UnpauseMusic()
        {
            if(Validate() == false || isPaused == false || isPlaying == false)
            {
                return;
            }

            audioSource.UnPause();
        }

        public void StopMusic()
        {
            if(Validate() == false)
            {
                return;
            }
        }

        private async UniTaskVoid PlayMusicAndForget()
        {
            await PlayMusicAsyncInternal();
        }

        private async UniTask PlayMusicAsyncInternal()
        {
            var token = this.GetCancellationTokenOnDestroy();
            await UniTask.WaitUntil(() => isLoop == false && audioSource.isPlaying == false, cancellationToken: token);

            _musicContainer.ReturnToPool(this);
        }
    }
}
