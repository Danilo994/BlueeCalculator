namespace BlueeCalc.Core
{
    public class CalculatorEngine
    {
        private double _current = 0;
        private string _operator = "";
        private bool _resetDisplay = false;
        private string _expression = "";

        public CalcResult Input(string value, string display)
        {
            var result = new CalcResult();

            if (double.TryParse(value, out _))
            {
                if (display == "0" || _resetDisplay || display == "-")
                {
                    _resetDisplay = false;
                    result.Display = value;

                    if (string.IsNullOrEmpty(_expression))
                    {
                        _expression = value;
                    }
                    else
                    {
                        int lastSpace = _expression.LastIndexOf(' ');
                        if (lastSpace == -1)
                            _expression = value;
                        else
                            _expression = _expression.Substring(0, lastSpace + 1) + value;
                    }
                }
                else
                {
                    result.Display = display + value;

                    int lastSpace = _expression.LastIndexOf(' ');
                    if (lastSpace == -1)
                        _expression = result.Display;
                    else
                        _expression = _expression.Substring(0, lastSpace + 1) + result.Display;
                }

                result.Expression = _expression;
                return result;
            }


            if (value == "-" && (display == "0" || _resetDisplay))
            {
                _resetDisplay = false;
                result.Display = "-";
                if (!string.IsNullOrEmpty(_expression))
                {
                    if (_expression.EndsWith("+") || _expression.EndsWith("-") ||
                        _expression.EndsWith("*") || _expression.EndsWith("/"))
                    {
                        _expression = _expression.TrimEnd() + " -";
                    }
                    else
                    {
                        _expression = "-";
                    }
                }
                else
                {
                    _expression = "-";
                }

                result.Expression = _expression;
                return result;
            }

            switch (value)
            {
                case "C":
                    _current = 0;
                    _operator = "";
                    _expression = "";
                    result.Display = "0";
                    result.Expression = "";
                    return result;

                case "+":
                case "-":
                case "*":
                case "/":
                    if (!string.IsNullOrEmpty(_operator))
                    {
                        var partial = Calculate(display);
                        result.Display = partial.Display;
                        if (!double.TryParse(partial.Display, out _current))
                            _current = 0;
                    }
                    else
                    {
                        if (!double.TryParse(display, out _current))
                            _current = 0;
                        result.Display = display;
                    }

                    _operator = value;
                    _expression = $"{_current} {_operator} ";
                    _resetDisplay = true;

                    result.Expression = _expression;
                    return result;

                case "=":
                    var calc = Calculate(display);
                    result.Display = calc.Display;
                    result.Expression = calc.Expression;
                    return result;

                default:
                    result.Display = display;
                    result.Expression = _expression;
                    return result;
            }
        }

        private CalcResult Calculate(string display)
        {
            var resultObj = new CalcResult();

            double number;
            if (!double.TryParse(display, out number))
                number = 0;

            double result = _current;

            switch (_operator)
            {
                case "+": result += number; break;
                case "-": result -= number; break;
                case "*": result *= number; break;
                case "/": result = number == 0 ? 0 : result / number; break;
                default: result = number; break;
            }

            string finalExpression = $"{_expression.Trim()} =".Trim();

            _operator = "";
            _resetDisplay = true;
            _expression = "";

            resultObj.Display = result.ToString();
            resultObj.Expression = finalExpression;
            return resultObj;
        }
    }
}
