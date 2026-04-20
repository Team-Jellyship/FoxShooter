namespace FoxShooter.Characters.Fox
{
    public class FoxStats : CharacterStats
    {
        public override void Kill(CharacterStats source)
        {
            Game.Game.instance.RestartCurrentLevel();
        }
    }
}