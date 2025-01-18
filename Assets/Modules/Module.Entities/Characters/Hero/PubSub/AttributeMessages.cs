using EncosyTower.Modules.PubSub;
using Module.Worlds.BattleWorld.Attribute;
using Module.Worlds.BattleWorld.Attribute.Modifiers;

namespace Module.Entities.Characters.Hero.PubSub
{
    public readonly struct UpgradeAttributeMessage : IMessage
    {
        public readonly AttributeType Type;
        public readonly ushort Scope;
        public readonly Modifier Modifier;

        public UpgradeAttributeMessage(AttributeType type, ushort scope, Modifier modifier)
        {
            Type = type;
            Scope = scope;
            Modifier = modifier;
        }
    }
}
