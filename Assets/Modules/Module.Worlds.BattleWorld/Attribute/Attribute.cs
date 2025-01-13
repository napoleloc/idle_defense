namespace Module.Worlds.BattleWorld.Attribute
{
    public class Attribute
    {
        private float _currentValue;
        private bool _isDirty;

        public Attribute() { }

        public Attribute(float baseValue)
        {
            _currentValue = baseValue;
            _isDirty = false;
        }
    }
}
