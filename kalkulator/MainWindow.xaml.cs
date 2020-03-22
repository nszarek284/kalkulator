using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kalkulator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, int> operators;
        private int index;
        public MainWindow()
        {
            InitializeComponent();
            operators = new Dictionary<string, int>();
            operatorsDictionary();
            MessageBox.Show(" p - zmiana +/- \r\n 2x del - całkowite czyszczenie \r\n esc - wyjście ", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //Przyciski nieliczbowe i nie operatorowe
        private void AC_Click(object sender, RoutedEventArgs e)
        {
            if (View_tb.Text.Length == 0)
            {
                Result_tb.Text = "";
            }
            View_tb.Text = "";
        }
        private void C_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
            if (!isEmpty())
                View_tb.Text = View_tb.Text.Substring(0, View_tb.Text.Length - 1);
            else
                View_tb.Text = "";
        }
        private void PM_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
            string s = View_tb.Text;
            int len = s.Length;
            if (len == 0)
            {
                View_tb.Text = "-";
            }
            else if (len == 1 && s[len - 1] == '-')
            {
                View_tb.Text = "";
            }
            else if (s[len - 1] == '-')
            {
                View_tb.Text = View_tb.Text.Substring(0, View_tb.Text.Length - 1) + '+';
            }
            else if (s[len - 1] == '+')
            {
                View_tb.Text = View_tb.Text.Substring(0, View_tb.Text.Length - 1) + '-';
            }
            else if (isNumber(s[len - 1]))
            {
                if (!operatorAnywhere())
                {
                    View_tb.Text = "-" + View_tb.Text;

                }
                else
                {
                    index = WhereNearestOp();
                    if (View_tb.Text[index] == '-' && index == 0)
                    {
                        View_tb.Text = View_tb.Text.Substring(1, View_tb.Text.Length - 1);
                    }

                    else if (View_tb.Text[index] == '-')
                    {
                        View_tb.Text = View_tb.Text.Substring(0, index) + "+" + View_tb.Text.Substring(index + 1, (View_tb.Text.Length - index - 1));
                    }
                    else if (View_tb.Text[index] == '+')
                    {
                        View_tb.Text = View_tb.Text.Substring(0, index) + "-" + View_tb.Text.Substring(index + 1, (View_tb.Text.Length - index - 1));
                    }

                    else
                    {
                        View_tb.Text = View_tb.Text.Substring(0, index + 1) + "(-" + View_tb.Text.Substring(index + 1, View_tb.Text.Length - index - 1) + ")";
                    }
                }

            }
            else if (isBracket(s[len - 1]))
            {
                index = WhereNearestOp();
                if (View_tb.Text[index] == '-' && View_tb.Text[index - 1] == '(')
                {
                    View_tb.Text = View_tb.Text.Substring(0, index - 1) + View_tb.Text.Substring(index + 1, View_tb.Text.Length - index - 2);
                }
            }

        }

        //Dot
        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
            int index_operator = View_tb.Text.Length;
            if (View_tb.Text.Length == 0)
            {
                View_tb.Text = "0,";
            }
            else if (operatorAnywhere())
            {
                for (int i = View_tb.Text.Length - 1; i >= 0; i--)
                {
                    if (isOperator(View_tb.Text[i]))
                    {
                        index_operator--;
                        break;
                    }
                    index_operator--;
                }
                if (View_tb.Text.Length == 1 && View_tb.Text[View_tb.Text.Length - 1] == '-')
                {
                    View_tb.Text += "0,";
                }
                else if (!isDot(index_operator) && isNumber(View_tb.Text[View_tb.Text.Length - 1]))
                {
                    View_tb.Text += ',';
                }
            }
            else if (isNumber(View_tb.Text[View_tb.Text.Length - 1]) && !isDot(0))
            {
                View_tb.Text += ',';
            }

        }
        private bool isDot(int indexOp)
        {
            for (int i = View_tb.Text.Length - 1; i >= indexOp; i--)
            {
                if (View_tb.Text[i] == ',')
                {
                    return true;
                }
            }
            return false;
        }

        //Operator
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
            if (View_tb.Text.Length == 1 && p.Content.ToString() == "+" && View_tb.Text[0] == '-')
            {
                View_tb.Text = "";
            }
            else if (View_tb.Text.Length >= 1 && View_tb.Text[View_tb.Text.Length - 1] == '(' && p.Content.ToString() == "-")
            {
                View_tb.Text += p.Content.ToString();
            }
            else if (View_tb.Text.Length != 0 && View_tb.Text[View_tb.Text.Length - 1] != '(')
            {
                if (isOperator(View_tb.Text[View_tb.Text.Length - 1]) || isDot(View_tb.Text.Length - 1))
                {
                    View_tb.Text = View_tb.Text.Remove(View_tb.Text.Length - 1, 1);
                }
                View_tb.Text += p.Content.ToString();
            }
            else if (View_tb.Text.Length == 0 && p.Content.ToString() == "-")
            {
                View_tb.Text += p.Content.ToString();
            }

        }
        private bool isOperator(char a)
        {
            if (a == '+' || a == '-' || a == '*' || a == '/')
                return true;
            else
                return false;
        }
        private bool operatorAnywhere()
        {
            for (int i = 0; i < View_tb.Text.Length; i++)
            {
                if (isOperator(View_tb.Text[i]))
                {
                    return true;
                }
            }
            return false;
        }
        private int WhereSlash(string[] w)
        {
            int index = -1;
            for (int i = 0; i < w.Length - 1; i++)
            {
                if (View_tb.Text[i] == '/')
                {
                    index = i;
                }
            }
            return index;
        }

        private int WhereNearestOp()
        {
            int index;
            for (index = View_tb.Text.Length - 1; index >= 0; index--)
            {
                if (isOperator(View_tb.Text[index]))
                {
                    break;
                }
            }

            return index;
        }
        //Result
        private void Result_Click(object sender, RoutedEventArgs e)
        {
            if (View_tb.Text.Length != 0)
            {
                
                    result(View_tb.Text);
                    if (View_tb.Text[View_tb.Text.Length - 1] == ',')
                    {
                        View_tb.Text = View_tb.Text.Remove(View_tb.Text.Length - 1, 1);
                    }
                    Result_tb.Text += View_tb.Text + "=";

                    if (!MakeDisplayGreatAgain())
                    {
                        Result_tb.Text += "\r\n";
                        View_tb.Text = "";
                    }
                    else
                    {
                        Result_tb.Text += "!!! \r\n";
                        View_tb.Text = "";
                        MessageBox.Show("Podjęto próbę dzielenia przez zero.", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                
            }
        }
        private void operatorsDictionary()
        {
            operators.Add("(", 0);
            operators.Add(")", 1);
            operators.Add("+", 1);
            operators.Add("-", 1);
            operators.Add("/", 2);
            operators.Add("*", 2);
        }
        private string result(string s)
        {
            s = bracketAndMinus(s);
            Console.WriteLine(s);
            string equation = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (isOperator(s[i]))
                {
                    equation += " " + s[i] + " ";
                }
                else if (s[i] == '(')
                {
                    equation += "( ";
                }
                else if (s[i] == ')')
                {
                    equation += " )";
                }
                else equation += s[i];
            }
            Console.WriteLine("Equation: " + equation);
            return equation;
        }
        string ResultToDouble(string s)
        {

            Stack<string> result_st = new Stack<string>();
            string output = "";
            s = bracketAndMinus(s);
            string[] eq = s.Trim().Split();
            foreach (string c in eq)
            {

                try
                {
                    double result_double = Convert.ToDouble(c);
                    output += c + " ";
                }
                catch
                {
                    if (c == "(" || result_st.Count == 0)
                        result_st.Push(c);
                    else if (c == ")")
                    {
                        while (result_st.Peek() != "(")
                        {
                            output += result_st.Pop() + " ";
                        }
                        result_st.Pop();
                    }
                    else
                    {
                        //Console.WriteLine(c);
                        while (result_st.Count != 0 && operators[c] <= operators[result_st.Peek()])
                        {
                            output += result_st.Pop() + " ";
                        }
                        result_st.Push(c);
                    }
                    //Console.WriteLine("Wyjscie" + output);
                }
            }

            while (result_st.Count != 0)
            {
                output += result_st.Pop() + " ";
            }
            Console.WriteLine("ONP: " + output);

            return output;
        }

        private double CalculatePostfix(string s)
        {
            string[] symbols = s.Trim().Split();
            Stack<double> stack = new Stack<double>();

            for (int i = 0; i < symbols.Length; i++)
            {
                try
                {
                    double number = Convert.ToDouble(symbols[i]);
                    stack.Push(number);
                }
                catch
                {

                    double o1 = stack.Pop();
                    double o2 = stack.Pop();
                    stack.Push(operation(o2, o1, symbols[i]));

                }

            }
            return (stack.Pop());

        }
        private double operation(double o2, double o1, string symbol)
        {
            double o3 = 0;
            if (symbol == "+")
            {
                o3 = o2 + o1;
            }
            if (symbol == "-")
            {
                o3 = o2 - o1;
            }
            if (symbol == "/")
            {
                o3 = o2 / o1;
            }
            if (symbol == "*")
            {
                o3 = o2 * o1;
            }
            return o3;
        }

        //Display
        private bool MakeDisplayGreatAgain()
        {
            //string k = View_tb.Text;
            //bracketAndMinus(k);
            //string w = result(View_tb.Text);
            string[] w = result(View_tb.Text).Trim().Split();
            bool isZero = false;
            if (View_tb.Text[0] == '-')
                View_tb.Text = '0' + View_tb.Text;


            if (WhereSlash(w) > -1)
            {
                try
                {
                    double c = Convert.ToDouble(w[WhereSlash(w) + 1]);
                    if (c == 0)
                    {
                        isZero = true;
                    }
                }
                catch
                {
                    
                }

            }
            if (!isZero)
            {

                string s = ResultToDouble(result(View_tb.Text));
                Result_tb.Text += CalculatePostfix(s.ToString());
            }
            return isZero;
        }

        //Number
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button number = (Button)sender;
            if (isEmpty() && number.Content.ToString() == "0") { }
            else if (!isEmpty() && View_tb.Text[View_tb.Text.Length - 1] == ')')
            { }
            else if (View_tb.Text.Length == 1 && isOperator(View_tb.Text[View_tb.Text.Length - 1]) && number.Content.ToString() == "0")
            {

            }
            else
            {
                View_tb.Text += number.Content.ToString();
            }


        }
        private bool isNumber(char a)
        {
            if (a == '1' || a == '2' || a == '3' || a == '4' || a == '5' || a == '6' || a == '7' || a == '8' || a == '9' || a == '0')
                return true;
            else
                return false;
        }



        //Bracket

        private void Bracket_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
            if (p.Content.ToString() == "(" && isEmpty())
            {
                View_tb.Text += p.Content.ToString();
            }
            else if (p.Content.ToString() == "(" && isOperator(View_tb.Text[View_tb.Text.Length - 1]))
            {
                View_tb.Text += p.Content.ToString();
            }
            else if (p.Content.ToString() == ")" && !isEmpty() && isNumber(View_tb.Text[View_tb.Text.Length - 1]) && bracketIsOpen() && operatorAnywhere())
            {
                View_tb.Text += p.Content.ToString();
            }

        }
        private bool isBracket(char a)
        {
            if (a == ')' || a == '(')
                return true;
            else
                return false;
        }
        private bool bracketIsOpen()
        {
            int left = 0;
            int right = 0;
            bool t = false;
            for (int i = 0; i < View_tb.Text.Length; i++)
            {
                if (View_tb.Text[View_tb.Text.Length - 1] == '(')
                {
                    left++;
                }
                else if (View_tb.Text[View_tb.Text.Length - 1] == ')')
                {
                    right++;
                }
            }
            
            if (left > right || right > left)
                t=true;
            return t;
        }
        private string bracketAndMinus(string s) //zwraca pozycje minusa
        {
            string w = s;
            for (int i = 1; i < s.Length; i++)
            {
                if (s[i - 1] == '(' && s[i] == '-')
                {
                    w = s.Substring(0, i) + "0" + s.Substring(i, s.Length - i);
                }
            }
            s = w;
            return s;
        }
        //Empty
        private bool isEmpty()
        {
            if (View_tb.Text.Length == 0)
                return true;
            else
                return false;
        }

        //OFF


        /*************************************************************
         Obsługa przycisków
         *************************************************************/
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1 || e.Key == Key.NumPad1)
            {
                One.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D2 || e.Key == Key.NumPad2)
            {
                Two.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D3 || e.Key == Key.NumPad3)
            {
                Three.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D4 || e.Key == Key.NumPad4)
            {
                Four.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D5 || e.Key == Key.NumPad5)
            {
                Five.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D6 || e.Key == Key.NumPad6)
            {
                Six.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D7 || e.Key == Key.NumPad7)
            {
                Seven.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D8 || e.Key == Key.NumPad8)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                    Star.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                else
                    Eight.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D9 || e.Key == Key.NumPad9)
            {
                Nine.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D0 || e.Key == Key.NumPad0)
            {
                Zero.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

            /**********************************************************/

            /*else if (e.Key == Key.Enter)
            {
                Equals.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }*/
            else if (e.Key == Key.OemComma || e.Key == Key.OemPeriod || e.Key == Key.Decimal)
            {
                Dot.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                Minus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.OemPlus || e.Key == Key.Add)
            {
                Plus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.Delete)
            {
                AC.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.Back)
            {
                C.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.Divide || e.Key == Key.OemBackslash || e.Key == Key.OemQuestion || e.Key == Key.Oem5)
            {
                Slash.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.Multiply)
            {
                Star.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.P)
            {
                MP.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
            Console.WriteLine(e.Key.ToString());

        }
    }
}
