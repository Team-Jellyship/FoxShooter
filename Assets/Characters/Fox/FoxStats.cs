namespace FoxShooter.Characters.Fox
{
    public class FoxStats : CharacterStats
    {
        public override void Kill(CharacterStats source, DamageType type = DamageType.Unaspected)
        {
            Game.Game.instance.RestartCurrentLevel();
        }
    }
}