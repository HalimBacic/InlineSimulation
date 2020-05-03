using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace InlineVer3C__
{
    class Parser
    {
        /*Class for user defined exceptions*/
        public class ExpressionException : Exception
        {
            public ExpressionException(string message)
            : base(message)
            { }
        }

        public string leftside;  //Name of expression
        public string rigthside; //Members in expression
        public double valueMath; //If MathExpression I use this
        public string valueText; //If concaternation I use this

        ////////////////*Method for removing blank spaces*////////////////////
        public string Normalize(string s)
        {
            return s.Replace(" ", "");
        }

        public string NormalizeExp(string s)
        {
            string e = "2.71828182";
            string pi = "3.14159265";
            s = s.Replace(" ", "");
            s = s.Replace("e", e);
            s = s.Replace("π", pi);
            return s;
        }

        public string NormalizeText(string s)
        {

            s = s.Replace("+", "");
            s = s.Replace("\"", "");
            return s;
        }


        //////////////////////////////////////////////////////////////////////
        //////////////*Method for variable name and right side*//////////////
        public void LeftandRight(string s)
        {
            Regex rgx = new Regex(@"(^\[[A-Za-z\d]*\])([\s=])*([A-Za-z\d\+\-\*\/()\[\]\""]*)$");
            Match m = rgx.Match(s);
            leftside = m.Groups[1].Value;
            rigthside = m.Groups[3].Value;
        }

        ///////////////////*Method for setting values*/////////////////////////////////////////
        public void Values(string line)
        {
            LeftandRight(line);
            valueMath = Evaluate(rigthside);
            valueText = NormalizeText(rigthside);
        }
        //////////////////////////////////////////////////////////////////////////////////////

        //////////////METHODS FOR EXPRESSION CHECKING///////////////////////////////////////////
        /*Brackets checking*/
        public bool Brackets(string line)
        {
            int sb = 0, mb = 0, lb = 0; //Three tipes of brackets
            foreach (char s in line)
            {
                if (s == '(') sb++;
                if (s == ')') sb--;
                if (s == '[') mb++;
                if (s == ']') mb--;
                if (s == '{') lb++;
                if (s == '}') lb--;
            }
            if (sb != 0 || mb != 0 || lb != 0)
                return true;
            else
                return false;
        }

        /*Division with zero*/
        public bool DivZero(string line)
        {
            for (int i = 0; i < line.Length; i++)
                if (line[i] == '/' && line[i + 1] == '0')
                    return true;
            return false;
        }

        /*Checking form of expression*/
        public bool ExpForm(string line)
        {
            string operator_num = @"[\+\*\/\-\%]";
            Regex op1 = new Regex(operator_num);
            for (int i = 0; i < line.Length - 1; i++)
            {
                string first = line[i].ToString();
                string second = line[i + 1].ToString();
                if (op1.IsMatch(first) && op1.IsMatch(second))
                    return true;
            }
            string last = line[line.Length - 1].ToString();
            if (op1.IsMatch(last))
                return true;
            return false;
        }

        /*Check AIO*/
        public void AllCheck(string line)
        {
            if (Brackets(line)) throw new ExpressionException("Not same number of brackets");
            if (DivZero(line)) throw new ExpressionException("Dividion with 0 doesn't allowed");
            if (ExpForm(line)) throw new ExpressionException("Wrong expression form");
        }
        ///////////////////////////////////////////////////////////////////////////////////////



        /*PRIMARY FOR EVALUATE*/

        private enum Operator { Addition = 1, Subtraction = 1, Multiplication = 2, Division = 2, Modulo = 2, Exponent = 3, Null = 0 };
        static private bool wasDigit;
        static private string number;

        static public double Evaluate(string expression)
        {
            Stack<double> Operators = new Stack<double>();
            Stack<char> Operands = new Stack<char>();

            // Format the expression so that its calculatable


            // Do the calculations and return the result
            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                if (IsDigit(c))
                {
                    wasDigit = true;
                    number += c.ToString();
                    if (i == expression.Length - 1) // last iteration
                    {
                        double num = double.Parse(number);
                        Operators.Push(num);
                        wasDigit = false;
                        number = "";
                    }
                }
                else
                {
                    if (wasDigit) // Add the previous number to our stack
                    {
                        double num = double.Parse(number);
                        Operators.Push(num);
                        wasDigit = false;
                        number = "";
                    }

                    if (IsLeftPar(c))
                    {
                        Operands.Push(c);
                    }
                    else if (IsRightPar(c))
                    {
                        while (!IsLeftPar(c))
                        {
                            double num1 = Operators.Pop();
                            double num2 = Operators.Pop();
                            char op = Operands.Pop();
                            double endResult = CalculateTwo(num1, num2, op);
                            Operators.Push(endResult);
                            c = Operands.Pop();
                        }
                    }
                    else if (IsOperator(c))
                    {
                        if (wasDigit) // Add the previous number to our stack
                        {
                            double num = double.Parse(number);
                            Operators.Push(num);
                            wasDigit = false;
                            number = "";
                        }

                        bool empty = Operands.Count == 0;
                        if (!empty)
                        {
                            char c2 = Operands.Peek();
                            if (OperatorPower(c2) >= OperatorPower(c))
                            {
                                double num1 = Operators.Pop();
                                double num2 = Operators.Pop();
                                char op = Operands.Pop();

                                double endResult = CalculateTwo(num1, num2, op);

                                Operators.Push(endResult);
                            }
                        }
                        Operands.Push(c);
                    }
                }
            }

            while (Operands.Count > 0)
            {
                double num1 = Operators.Pop();
                double num2 = Operators.Pop();
                char op = Operands.Pop();

                double endResult = CalculateTwo(num1, num2, op);
                Operators.Push(endResult);

            }

            return Math.Round(Operators.Pop(), 8);
        }

        static bool IsDigit(char c)
        {
            return Char.IsNumber(c) || c == '.';
        }

        static bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '%' || c == '^';
        }

        static bool IsLeftPar(char c)
        {
            return c == '(';
        }

        static bool IsRightPar(char c)
        {
            return c == ')';
        }

        static double CalculateTwo(double num1, double num2, char op)
        {
            double result = 0;
            if (op == '+')
                result = num2 + num1;
            else if (op == '-')
                result = num2 - num1;
            else if (op == '*')
                result = num2 * num1;
            else if (op == '/')
                result = num2 / num1;
            else if (op == '^')
                result = Math.Pow(num2, num1);
            else if (op == '%')
                result = num2 % num1;

            return result;
        }

        static Operator OperatorPower(char c)
        {
            Operator op = c == '+' ? Operator.Addition : (c == '-' ? Operator.Subtraction : (c == '*' ? Operator.Multiplication : (c == '/' ? Operator.Division : (c == '%' ? Operator.Modulo : (c == '^' ? Operator.Exponent : Operator.Null)))));
            return op;
        }
    }
}

