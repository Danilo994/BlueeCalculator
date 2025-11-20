using ReactiveUI;
using System.Reactive;
using BlueeCalc.Core;

namespace BlueeCalc.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private string _display = "0";
        public string Display
        {
            get => _display;
            set => this.RaiseAndSetIfChanged(ref _display, value);
        }

        private string _expression = "";
        public string Expression
        {
            get => _expression;
            set => this.RaiseAndSetIfChanged(ref _expression, value);
        }

        public CalculatorEngine Engine { get; } = new();

        public ReactiveCommand<string, Unit> ButtonPress { get; }

        public MainWindowViewModel()
        {
            ButtonPress = ReactiveCommand.Create<string>(OnButtonPress);
        }

        private void OnButtonPress(string value)
        {
            var result = Engine.Input(value, Display ?? "0");
            Display = result.Display;
            Expression = result.Expression;
        }
    }
}
