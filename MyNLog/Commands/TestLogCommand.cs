namespace MyNLog.Commands
{
    [PrismResourceInjection]
    public class TestLogCommand : InjectableCommand<TestLogCommand>
    {
        protected override bool CanExecuteInternal(object parameter)
        {
            return true;
        }

        protected override void ExecuteInternal(object parameter)
        {
            Logger.Info("Test log");
        }
    }
}
