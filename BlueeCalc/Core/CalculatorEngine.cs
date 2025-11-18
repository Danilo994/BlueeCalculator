namespace BlueeCalc.Core
{
    public class CalculatorEngine
    {
        private double _current = 0;
        private string _operator = "";
        private bool _resetDisplay = false;

        public string Input(string value, string display)
        {
            if (double.TryParse(value, out _))
            {
                if (display == "0" || _resetDisplay)
                {
                    _resetDisplay = false;
                    return value;
                }
                return display + value;
            }

            switch (value)
            {
                case "C":
                    _current = 0;
                    _operator = "";
                    return "0";

                case "+":
                case "-":
                case "*":
                case "/":
                    _current = double.Parse(display);
                    _operator = value;
                    _resetDisplay = true;
                    return display;

                case "=":
                    return Calculate(display);

                default:
                    return display;
            }
        }

        private string Calculate(string display)
        {
            double number = double.Parse(display);
            double result = _current;

            switch (_operator)
            {
                case "+": result += number; break;
                case "-": result -= number; break;
                case "*": result *= number; break;
                case "/": result = number == 0 ? 0 : result / number; break;
            }

            _operator = "";
            _resetDisplay = true;

            return result.ToString();
        }
    }
}
