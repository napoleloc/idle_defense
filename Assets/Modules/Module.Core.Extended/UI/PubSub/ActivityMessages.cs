using EncosyTower.Modules.PubSub;
using EncosyTower.Modules;
using System.Runtime.CompilerServices;
using System;
using UnityEngine;

namespace Module.Core.Extended.UI
{
    public readonly record struct ShowActivityMessage(
         string ResourcePath
       , bool PlayAnimation
       , Memory<object> Args
    ) : IMessage;

    public readonly record struct HideActivityMessage(string ResourcePath, bool PlayAnimation) : IMessage;

    public static class ResourceKeyActivityMessageExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ShowActivityMessage ToShowActivityMessage(
              this AssetKey<GameObject> key
            , bool playAnimation = false
            , params object[] args
        )
        {
            return new(key.Value, playAnimation, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HideActivityMessage ToHideActivityMessage(
              this AssetKey<GameObject> key
            , bool playAnimation = false
        )
        {
            return new(key.Value, playAnimation);
        }
    }
}
