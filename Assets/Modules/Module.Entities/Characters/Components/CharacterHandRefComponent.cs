using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules;
using Module.GameCommon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Characters
{
    [System.Serializable]
    public struct HandEntry
    {
        public Transform transform;
        public string boneName;
        public HandType handType;
    }

    public class CharacterHandRefComponent : MonoBehaviour
    {
        [SerializeField, OnValueChanged("OnValueChange")]
        private HandEntry[] _hands;

        public ReadOnlyMemory<HandEntry> Hands
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _hands;
        }

        private void OnValueChange()
        {
            var hands = Hands.Span;

            for (int i = 0; i < hands.Length; i++)
            {
                var hand = hands[i];

                if (hand.transform.IsInvalid())
                {
                    continue;
                }
                _hands[i] = new HandEntry() {transform = hand.transform, boneName = hand.transform.name, handType = hand.handType };
            }
        }
    }
}
