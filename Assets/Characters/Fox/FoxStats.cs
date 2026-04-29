using FoxShooter.Game;
using FoxShooter.Game.GamemodeGraph.Runtime;

namespace FoxShooter.Characters.Fox
{
    public class FoxStats : CharacterStats
    {
        public override void Kill(CharacterStats source, DamageType type = DamageType.Unaspected)
        {
            base.Kill(source, type);
            
            Game.Game.instance.character = this;

            // Game.Game.instance.RestartCurrentLevel();
            var timer = TimerManager.instance.CreateTimer(this, () => Game.Game.instance.Command(GamemodeTransitionFlag.Death));
            timer.Start(1.0f);
        }
    }
}