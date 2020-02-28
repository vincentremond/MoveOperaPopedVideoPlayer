namespace MoveWindows.Contracts
{
    internal interface ISingleInstanceChecker
    {
        void KillOtherInstances();
    }
}