
namespace MyNLog.Commands
{
    public interface ICommand : System.Windows.Input.ICommand
    {
        bool CanExecute();
        void Execute();
    }

    public interface ICommand<TParam> : ICommand
    {
        bool CanExecute(TParam parameter);
        void Execute(TParam parameter);
    }

    public interface ICommand<TParam1, TParam2> : ICommand
    {
        bool CanExecute(TParam1 parameter1, TParam2 parameter2);
        void Execute(TParam1 parameter1, TParam2 parameter2);
    }
}
