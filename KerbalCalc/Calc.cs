// This file is part of KerbalCalc.
// 
// KerbalCalc is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// KerbalCalc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with KerbalCalc. If not, see <http://www.gnu.org/licenses/>.

using System;

namespace KerbalCalc
{
    internal class Calc
    {
        public string Screen { get; set; }
        private Func<double, double, double> _operation;
        private double _heldOperand;
        private bool _reset;

        public Calc()
        {
            Screen = String.Empty;
        }

        public void AddDigit(int digit)
        {
            if (Screen.Contains("ERROR") || _reset)
                Screen = "";

            Screen += digit.ToString();
            _reset = false;
        }

        public void AddDecimal()
        {
            if (Screen.Contains("ERROR") || _reset)
                Screen = "";

            if (Screen.Contains("."))
                return;

            if (String.IsNullOrEmpty(Screen))
                Screen = "0";

            Screen += ".";
            _reset = false;
        }

        public void Calculate()
        {
            double operand;

            if (double.TryParse(Screen, out operand) && _operation != null)
            {
                Screen = _operation(_heldOperand, operand).ToString();
                _reset = true;
            }
            else
                Screen = "ERROR";
        }

        public void StageAdd()
        {
            StageOperator((firstOperand, secondOperand) => firstOperand + secondOperand);
        }

        public void StageSubtract()
        {
            StageOperator((firstOperand, secondOperand) => firstOperand - secondOperand);
        }

        public void StageMultiply()
        {
            StageOperator((firstOperand, secondOperand) => firstOperand*secondOperand);
        }

        public void StageDivide()
        {
            StageOperator((firstOperand, secondOperand) => firstOperand/secondOperand);
        }

        public void StagePower()
        {
            StageOperator(Math.Pow);
        }

        private void StageOperator(Func<double, double, double> operation)
        {
            if (!double.TryParse(Screen, out _heldOperand))
            {
                Screen = "ERROR";
                return;
            }

            _operation = operation;
            Screen = "";
        }

        public void UnaryOperation(Func<double, double> operation)
        {
            double operand;

            if (double.TryParse(Screen, out operand))
            {
                Screen = operation(operand).ToString();
                _reset = true;
            }
            else
                Screen = "ERROR";
        }

        public void NaturalLog()
        {
            UnaryOperation(Math.Log);
        }

        public void Square()
        {
            UnaryOperation(operand => Math.Pow(operand, 2));
        }

        public void Cube()
        {
            UnaryOperation(operand => Math.Pow(operand, 3));
        }
    }
}