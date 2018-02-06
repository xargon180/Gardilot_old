namespace Gardilot.SharedKernel.Actors
{
    interface IActor<TMessage>
    {
        void Act(TMessage message);
    }
}