using System;
using System.Linq;

namespace StudyProject
{
    static class Symbol
    {
        static public bool isNumber(char chr)
        {
            char[] arr = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            for (int i = 0; i < arr.Length; i++)
            {
                if (chr == arr[i]) return true;
            }
            return false;
        }
        static public bool isOperation(char chr)
        {
            char[] arr = { '-', '+', '*', '/' };
            for (int i = 0; i < arr.Length; i++)
            {
                if (chr == arr[i]) return true;
            }
            return false;
        }
    }

    class Stack
    {
        private int[] _numbers;

        public int Length { get; private set; } = 0;
        public Stack(int predictNumbers = 10)
        {
            _numbers = new int[(predictNumbers - (predictNumbers % 10) + 10)];
        }
        public void push(int value)
        {
            if (Length >= _numbers.Length)
            {
                int[] temp = new int[_numbers.Length + 10];
                for (int i = 0; i < _numbers.Length; i++)
                    temp[i] = _numbers[i];
                _numbers = temp;
            }
            _numbers[Length] = value;
            Length++;
        }
        public int pop()
        {
            if (Length <= 0)
            {
                return default;
            }
            int temp = _numbers[Length - 1];
            _numbers[Length - 1] = default;
            Length--;
            return temp;   
        }
        public void print(string sep = "\n")
        {
            for (int i = 0; i < Length; i++)
            {
                Console.WriteLine($"{_numbers[i]}{sep}");
            }
        }
    }


    class PolandCalculator
    {
        private string expression;
        private int firstOperationPosition;
        public string Expression
        {
            set
            {
                var temp = expression;
                expression = value;
                if (!CheckValidation())
                {
                    expression = temp;
                    Console.WriteLine("Строка содержала ошибку, выражение изменено не было");
                }
            }
        }

        private Stack localStack;
        public PolandCalculator(string expression)
        {
            this.expression = expression;
            localStack = new Stack();
        }
        private bool CheckValidation()
        {
            if (expression is null || expression.Length < 5) return false;
            char i = expression[0] != ' ' ? expression[0] : '\0';
            for (int j = 0; j < expression.Length; j++)
            {
                if (!Symbol.isNumber(expression[j]) && !Symbol.isOperation(expression[j]) && expression[j] != ' ') return false;
                if (expression[j] == ' ' && j != expression.Length)
                {
                    if (expression[j + 1] == ' ') return false;
                }
            }
            int numberCount = 0;
            int iter;
            for (iter = 0; iter < expression.Length && !Symbol.isOperation(expression[iter]); iter++)
            {   
                if (expression[iter] == ' ')
                {
                    numberCount++;
                }
            }
            if (numberCount == 0) return false;
            //i = expression[0];
            int operationsCount = 1;
            while (!Symbol.isOperation(i) && iter < expression.Length)
            {
                if (expression[iter] == ' ')
                {
                    operationsCount++;
                }
                iter++;
            }
            bool result = (numberCount - operationsCount == 1 ? true : false);
            return result;
        } //Поменять модификатор доступа на private при релизе
        private void FillingStack()
        {
            
            int iter = 0;
            string temp = "";
            while (!Symbol.isOperation(expression[iter]))
            {
                if (expression[iter] != ' ')
                    temp += expression[iter];
                else
                {
                    localStack.push(int.Parse(temp));
                    temp = "";
                }
                iter++;
            }
            firstOperationPosition = iter;
        }
        public int GetResult()
        {
            if (CheckValidation())
            {
                FillingStack();
                for (int i = firstOperationPosition; i < expression.Length; i++)
                {
                    switch (expression[i])
                    {
                        case '+':
                            localStack.push(localStack.pop() + localStack.pop());
                            break;
                        case '-':
                            localStack.push(localStack.pop() - localStack.pop());
                            break;
                        case '*':
                            localStack.push(localStack.pop() * localStack.pop());
                            break;
                        case '/':
                            localStack.push(localStack.pop() / localStack.pop()); 
                            break;
                        default:
                            break;
                    }
                }
                return localStack.pop();
            }
            throw new InvalidOperationException("Выражение введено в неверном формате");
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var polCalc = new PolandCalculator(null);
            Console.WriteLine(polCalc.GetResult());
        }
    }
}

