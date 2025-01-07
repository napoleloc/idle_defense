using EncosyTower.Modules.PubSub;
using EncosyTower.Modules;
using System.Runtime.CompilerServices;
using System;
using UnityEngine;

namespace Module.Core.Extended.UI
{
    public readonly record struct ShowScreenMessage(
          string ResourcePath
        , bool PlayAnimation
        , bool LoadAsync
        , Memory<object> Args
    ) : IMessage;

    public readonly record struct HideScreenMessage(bool PlayAnimation) : IMessage;

    public static class ResourceKeyScreenMessageExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ShowScreenMessage ToShowScreenMessage(
              this AssetKey<GameObject> key
            , bool playAnimation = false
            , bool loadAsync = true
            , params object[] args
        )
        {
            return new(key.Value, playAnimation, loadAsync, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ShowScreenMessage ToShowScreenMessage(
              this string key
            , bool playAnimation = false
            , bool loadAsync = true
            , params object[] args
        )
        {
            return new(key, playAnimation, loadAsync, args);
        }
    }
}
