namespace FoxShooter.Characters.AI.StateTree.Tasks
{
    public interface IContext<out T>
    {
        T Evaluate(TreeContext treeContext);
    }
}