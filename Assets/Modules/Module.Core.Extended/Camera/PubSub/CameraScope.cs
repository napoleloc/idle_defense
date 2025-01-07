using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.PubSub;

namespace Module.Core.Extended.Camera
{
    public readonly struct CameraScope : IEquatable<CameraScope>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CameraScope other)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is CameraScope;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => base.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => nameof(CameraScope);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(CameraScope left, CameraScope right)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(CameraScope left, CameraScope right)
            => false;
    }

    public static class MessengerCameraScopeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageSubscriber.Subscriber<CameraScope> CameraScope([NotNull] this MessageSubscriber self)
            => self.Scope<CameraScope>(default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessagePublisher.Publisher<CameraScope> CameraScope([NotNull] this MessagePublisher self)
            => self.Scope<CameraScope>(default);
    }
}
