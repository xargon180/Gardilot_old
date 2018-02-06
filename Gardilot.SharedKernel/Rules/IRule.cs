namespace Gardilot.SharedKernel.Rules
{
    interface IRule
    {
        bool IsEnabled { get; }
        void Enable();
        void Disable();
    }
}
