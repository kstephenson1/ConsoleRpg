namespace ConsoleRpgEntities.Models.Interfaces.UnitBehaviors
{
    public interface ITargetable
    {
        // Interface that allows units to be attacked.
        public void Damage(int damage);
        public void Heal(int damage);
        void OnHealthChanged();
        void OnDeath();
        bool IsDead();

    }
}
