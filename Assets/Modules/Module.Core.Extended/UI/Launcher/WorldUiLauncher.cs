using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.PubSub;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.PubSub;
using Sirenix.OdinInspector;
using UnityEngine;
using ZBase.UnityScreenNavigator.Core;
using ZBase.UnityScreenNavigator.Core.Activities;
using ZBase.UnityScreenNavigator.Core.Modals;
using ZBase.UnityScreenNavigator.Core.Screens;
using ZBase.UnityScreenNavigator.Core.Windows;

namespace Module.Core.Extended.UI
{
    public class WorldUiLauncher : UnityScreenNavigatorLauncher
    {
        public static readonly Id<WorldUiLauncher> PresetId = default;

        [Title("Window Container IDs", titleAlignment: TitleAlignments.Centered)]
        [LabelText("Screen", false)]
        [InlineProperty]
        [SerializeField]
        private WindowContainerId _screenId = new("Screens", 1);

        [LabelText("Modal")]
        [InlineProperty]
        [SerializeField]
        private WindowContainerId _modalId = new("Modals", 2);

        [LabelText("Activity")]
        [InlineProperty]
        [SerializeField]
        private WindowContainerId _activityId = new("Activities", 3);

        [Title("Addressable Keys", titleAlignment: TitleAlignments.Centered)]
        [LabelText("Loading Activity")]
        [SerializeField]
        private AssetKey.Serializable _loadingActivityKey;

        private ScreenContainer _screenContainer;
        private ModalContainer _modalContainer;
        private ActivityContainer _activityContainer;

        protected override void OnCreateContainer(
            [NotNull] WindowContainerConfig config
            , [NotNull] WindowContainerBase container
        )
        {
            switch (config.containerType)
            {
                case WindowContainerType.Screen:
                {
                    if (container.TryAddToGlobalObjectVaultAs(_screenId, ref _screenContainer))
                    {
                        return;
                    }
                    break;
                }

                case WindowContainerType.Modal:
                {
                    if (container.TryAddToGlobalObjectVaultAs(_modalId, ref _modalContainer))
                    {
                        return;
                    }
                    break;
                }

                case WindowContainerType.Activity:
                {
                    if (container.TryAddToGlobalObjectVaultAs(_activityId, ref _activityContainer))
                    {
                        return;
                    }
                    break;
                }
            }
        }

        protected override void OnPostCreateContainers()
        {
            InitializeAndForget().Forget();
        }

        private async UniTaskVoid InitializeAndForget()
        {
            if (_loadingActivityKey.IsValid)
            {
                var loadingOptions = new ActivityOptions(_loadingActivityKey.Value, false);
                await _activityContainer.ShowAsync(loadingOptions);
            }

            var subscriber = WorldMessenger.Subscriber.UIScope().WithState(this);

            subscriber.Subscribe<AsyncMessage<ShowScreenMessage>>(static (state, message) => state.HandleAsync(message));
            subscriber.Subscribe<ShowScreenMessage>(static (state, message) => state.Handle(message));
            subscriber.Subscribe<HideScreenMessage>(static (state, message) => state.Handle(message));

            subscriber.Subscribe<AsyncMessage<ShowModalMessage>>(static (state, message) => state.HandleAsync(message));
            subscriber.Subscribe<ShowModalMessage>(static (state, message) => state.Handle(message));
            subscriber.Subscribe<HideModalMessage>(static (state, message) => state.Handle(message));

            subscriber.Subscribe<AsyncMessage<HideAllModalMessage>>(static (state, message) => state.HandleAsync(message));
            subscriber.Subscribe<HideAllModalMessage>(static (state, message) => state.Handle(message));

            subscriber.Subscribe<AsyncMessage<ShowActivityMessage>>(static (state, message) => state.HandleAsync(message));
            subscriber.Subscribe<ShowActivityMessage>(static (state, message) => state.Handle(message));
            subscriber.Subscribe<HideActivityMessage>(static (state, message) => state.Handle(message));

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        protected override void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private async UniTask HandleAsync(AsyncMessage<ShowScreenMessage> asyncMessage)
        {
            if (_screenContainer == false) return;

            var message = asyncMessage.Message;
            var options = new ScreenOptions(
                message.ResourcePath
                , message.PlayAnimation
                , loadAsync: message.LoadAsync
            );

            await _screenContainer.PushAsync(options, message.Args);
        }

        private void Handle(ShowScreenMessage message)
        {
            if (_screenContainer == false) return;

            var options = new ScreenOptions(
                message.ResourcePath
                , message.PlayAnimation
                , loadAsync: message.LoadAsync
            );

            _screenContainer.Push(options, message.Args);
        }

        private void Handle(HideScreenMessage message)
        {
            if (_screenContainer == false) return;

            _screenContainer.Pop(message.PlayAnimation);
        }

        private async UniTask HandleAsync(AsyncMessage<ShowModalMessage> asyncMessage)
        {
            if (_modalContainer == false) return;

            var message = asyncMessage.Message;
            var options = new ModalOptions(
                message.ResourcePath
                , message.PlayAnimation
                , loadAsync: message.LoadAsync
            );

            await _modalContainer.PushAsync(options, message.Args);
        }

        private void Handle(ShowModalMessage message)
        {
            if (_modalContainer == false) return;

            var options = new ModalOptions(
                message.ResourcePath
                , message.PlayAnimation
                , loadAsync: message.LoadAsync
                , backdropAlpha: message.BackdropAlpha
            );

            _modalContainer.Push(options, message.Args);
        }

        private void Handle(HideModalMessage message)
        {
            if (_modalContainer == false) return;

            _modalContainer.Pop(message.PlayAnimation);
        }

        private async UniTask HandleAsync(AsyncMessage<HideAllModalMessage> asyncMessage)
        {
            if (_modalContainer == false) return;
            if (_modalContainer.Modals == null) return;

            var message = asyncMessage.Message;

            if (message.Count <= 0)
            {
                while (_modalContainer.Modals.Count > 0)
                {
                    await _modalContainer.PopAsync(message.PlayAnimation);
                }
                return;
            }

            var count = 0;

            while (_modalContainer.Modals.Count > 0 && count < message.Count)
            {
                await _modalContainer.PopAsync(message.PlayAnimation);
                count++;
            }
        }

        private void Handle(HideAllModalMessage message)
        {
            if (_modalContainer == false) return;

            if (_modalContainer.Modals != null)
            {
                HideAll(message.Count).Forget();
            }

            async UniTaskVoid HideAll(int hideCount)
            {
                if (hideCount <= 0)
                {
                    while (_modalContainer.Modals.Count > 0)
                    {
                        await _modalContainer.PopAsync(message.PlayAnimation);
                    }

                    return;
                }

                var count = 0;

                while (_modalContainer.Modals.Count > 0 && count <= message.Count)
                {
                    await _modalContainer.PopAsync(message.PlayAnimation);
                    count++;
                }
            }
        }

        private async UniTask HandleAsync(AsyncMessage<ShowActivityMessage> asyncMessage)
        {
            if (_activityContainer == false) return;

            var message = asyncMessage.Message;
            var options = new ActivityOptions(message.ResourcePath, message.PlayAnimation);
            await _activityContainer.ShowAsync(options, message.Args);
        }

        private void Handle(ShowActivityMessage message)
        {
            if (_activityContainer == false) return;

            var options = new ActivityOptions(message.ResourcePath, message.PlayAnimation);
            _activityContainer.Show(options, message.Args);
        }

        private void Handle(HideActivityMessage message)
        {
            if (_activityContainer == false) return;

            _activityContainer.Hide(message.ResourcePath, message.PlayAnimation);
        }
    }
}
