using EncosyTower.Modules.PubSub;
using EncosyTower.Modules;
using System.Runtime.CompilerServices;
using System;
using UnityEngine;

namespace Module.Core.Extended.UI
{
    public readonly record struct ShowModalMessage(
           string ResourcePath
         , bool PlayAnimation
         , bool LoadAsync
         , float? BackdropAlpha
         , Memory<object> Args
     ) : IMessage
    {
        public ShowModalMessage(
              string resourcePath
            , bool playAnimation
            , bool loadAsync
            , Memory<object> args
        )
            : this(resourcePath, playAnimation, loadAsync, default, args)
        { }
    }

    public readonly record struct HideModalMessage(bool PlayAnimation) : IMessage;

    public readonly record struct HideAllModalMessage(bool PlayAnimation = false, int Count = 0) : IMessage;

    public static class ResourceKeyModalMessageExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ShowModalMessage ToShowModalMessage(
              this AssetKey<GameObject> key
            , bool playAnimation = false
            , bool loadAsync = true
            , float? backdropAlpha = default
            , params object[] args
        )
        {
            return new(key.Value, playAnimation, loadAsync, backdropAlpha, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ShowModalMessage ToShowModalMessage(
              this string key
            , bool playAnimation = false
            , bool loadAsync = true
            , float? backdropAlpha = default
            , params object[] args
        )
        {
            return new(key, playAnimation, loadAsync, backdropAlpha, args);
        }
    }
}
