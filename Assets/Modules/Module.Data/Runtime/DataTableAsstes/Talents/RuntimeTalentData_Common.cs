using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Module.Worlds.BattleWorld.Attribute;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;

namespace Module.Data.Runtime.DataTableAsstes.Talents
{
    [System.Serializable]
    public struct RuntimeTalentIdData
    {
        [HideLabel]
        [HorizontalGroup]
        [SerializeField]
        private AttributeKind _kind;
        [HideLabel]
        [HorizontalGroup]
        [ValueDropdown("GetTypesByKind")]
        [SerializeField]
        private AttributeType _type;

        public AttributeKind Kind { get => Get_Kid(); init => Set_Kind(value); }
        public AttributeType Type { get => Get_Type(); init => Set_Type(value); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AttributeKind Get_Kid() { return _kind; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set_Kind(AttributeKind kind) { _kind = kind; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AttributeType Get_Type () { return _type; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set_Type(AttributeType type) { _type = type; }

        public override string ToString()
        {
            return $"{_kind} - {_type}";
        }

#if UNITY_EDITOR


        private List<AttributeType> GetTypesByKind()
        {
            var offensiveAttributes = OffensiveAttributeTypeExtensions.Values.AsNativeArray(Allocator.Temp);
            var defenseAttributes = DefenseAttributeTypeExtensions.Values.AsNativeArray(Allocator.Temp);

            var types = new List<AttributeType>();

            if(_kind == AttributeKind.Offensive)
            {
                var span = offensiveAttributes.AsReadOnlySpan();
                for( var i = 0; i < span.Length; i++)
                {
                    types.Add(span[i].ToAttributeType());
                }
            }

            if( _kind == AttributeKind.Defense)
            {
                var span = defenseAttributes.AsReadOnlySpan();
                for ( var i = 0; (i < span.Length); i++)
                {
                    types.Add(span[i].ToAttributeType());
                }
            }

            return types;
        }
#endif
    }

    [System.Serializable]
    public struct RuntimeTalentDataEntry
    {
        [SerializeField]
        [InlineProperty]
        private RuntimeTalentIdData _id;
        [SerializeField]
        private int _level;

        public RuntimeTalentIdData Id { get => Get_Id(); init => Set_Id(value); }
        public int Level { get => Get_Level(); init => Set_Level(value); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private RuntimeTalentIdData Get_Id() => _id;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set_Id(RuntimeTalentIdData id) {  this._id = id; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int Get_Level() => _level;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set_Level(int level) {  this._level = level; }

        private string GetLabel()
        {
            return _id.ToString();
        }
    }
}
