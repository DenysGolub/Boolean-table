using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace fromLKN
{
    class Program
    {
        static string GetASet(int number, int length)
        {
            return Convert.ToString(number, 2).PadLeft(length, '0');
        }

        static string GetFormulaForSet(string formula, string vars, string set)
        {
            for (int i = 0; i < vars.Length; i++)
                formula = formula.Replace(vars[i], set[i]);
            return formula;
        }
        static string GetVarsForSet(string formula)
        {
            string pattern = @"¬|˄|v|→|⇔|↑|↓|⊕";
            Regex regex = new Regex(pattern);
            string vars = regex.Replace(formula, "");

            var unique = new HashSet<char>(vars);
            string resultstring = String.Empty;
            foreach (char c in unique)
            {
                resultstring += c;
            }
            return resultstring;
        }

        static string GetTableFromVars(string formula, string varstotal)
        {
            string pattern = @"¬|˄|v|→|⇔|↑|↓|⊕";
            Regex regex = new Regex(pattern);
            string vars = regex.Replace(formula, "");
            string resultstring = vars.Substring(0, varstotal.Length);
            return resultstring;
        }

        //Operations             string operations = "¬˄v→⇔↑↓⊕";


        static string GetResultForOperationsInFormula(string formula, char symbol)
        {

            string symb = Convert.ToString(symbol);
            switch (symb)
            {
                case "¬":
                    formula = Zaperech(formula, symb);
                    break;
                case "˄":
                    formula = Conjuction(formula, symb);
                    break;
                case "v":
                    formula = Disjunction(formula, symb);
                    break;
                case "→":
                    formula = Implication(formula, symb);
                    break;
                case "⇔":
                    formula = Equivalence(formula, symb);
                    break;
                case "↑":
                    formula = Sheffler(formula, symb);
                    break;
                case "↓":
                    formula = Pirs(formula, symb);
                    break;
                case "⊕":
                    formula = XOR(formula, symb);
                    break;
            }
            return formula;
        }

        private static IEnumerable<String> Brackets(string value)
        {
            if (string.IsNullOrEmpty(value))
                yield break; // or throw exception

            Stack<int> brackets = new Stack<int>();
            var sb = new StringBuilder(value);

            for (int i = 0; i < value.Length; ++i)
            {
                char ch = value[i];

                if (ch == '(')
                    brackets.Push(i);
                else if (ch == ')')
                {
                    //TODO: you may want to check if close ']' has corresponding open '['
                    // i.e. stack has values: if (!brackets.Any()) throw ...
                    int openBracket = brackets.Pop();


                    yield return value.Substring(openBracket + 1, i - openBracket - 1);

                }

            }
            //TODO: you may want to check here if there're too many '['
            // i.e. stack still has values: if (brackets.Any()) throw ... 
            yield return value;
        }




        static string Zaperech(string formula, string symb)
        {
            var sb = new StringBuilder(formula);
            int pos = formula.IndexOf(symb);

            if (formula.Contains("¬0") == true)
            {
                sb.Replace("¬0", "1");
            }
            else if (formula.Contains("¬1") == true)
            {
                sb.Replace("¬1", "0");
            }

            if (formula.Contains("¬¬0") == true)
            {
                sb.Replace("¬¬0", "0");
            }
            else if (formula.Contains("¬¬1") == true)
            {
                sb.Replace("¬¬1", "1");

            }
            string formula_new = sb.ToString();

            return formula_new;
        }

        static string Conjuction(string formula, string symb)
        {
            var sb = new StringBuilder(formula);
            int pos = formula.IndexOf(symb);


            if (formula.Contains("1˄1") == true)
            {
                sb.Replace("1˄1", "1");
            }
            else if ((formula.Contains("1˄0") == true) || (formula.Contains("0˄1") == true) || (formula.Contains("0˄0") == true))
            {
                sb.Replace("1˄0", "0");
                sb.Replace("0˄1", "0");
                sb.Replace("0˄0", "0");
            }

            formula = sb.ToString();
            return formula;
        }

        static string Disjunction(string formula, string symb)
        {
            var sb = new StringBuilder(formula);
            int pos = formula.IndexOf(symb);

            if (formula.Contains("0v0") == true)
            {
                sb.Replace("0v0", "0");
            }
            else if ((formula.Contains("1v0") == true) || (formula.Contains("0v1") == true) || (formula.Contains("1v1") == true))
            {
                sb.Replace("1v0", "1");
                sb.Replace("0v1", "1");
                sb.Replace("1v1", "1");
            }

            formula = sb.ToString();
            return formula;
        }

        static string Implication(string formula, string symb)
        {
            var sb = new StringBuilder(formula);

            int pos = formula.IndexOf(symb);

            if (formula.Contains("1→0") == true || formula.Length == 2)
            {
                sb.Replace("1→0", "0");
            }
            else if ((formula.Contains("0→1") == true) || (formula.Contains("0→0") == true) || (formula.Contains("1→1") == true))
            {
                sb.Replace("0→1", "1");
                sb.Replace("0→0", "1");
                sb.Replace("1→1", "1");

            }

            formula = sb.ToString();

            return formula;
        }

        static string Equivalence(string formula, string symb)
        {
            var sb = new StringBuilder(formula);

            int pos = formula.IndexOf(symb);

            if (formula.Contains("1⇔1") == true || formula.Contains("0⇔0"))
            {
                sb.Replace("0⇔0", "1");
                sb.Replace("1⇔1", "1");

            }
            else if ((formula.Contains("0⇔1") == true) || (formula.Contains("1⇔0") == true))
            {
                sb.Replace("1⇔0", "0");
                sb.Replace("0⇔1", "0");
            }

            formula = sb.ToString();

            return formula;
        }

        static string Sheffler(string formula, string symb)
        {
            var sb = new StringBuilder(formula);

            int pos = formula.IndexOf(symb);

            if (formula.Contains("1↑1") == true)
            {
                sb.Replace("1↑1", "0");
            }
            else if ((formula.Contains("0↑1") == true) || (formula.Contains("0↑0") == true) || (formula.Contains("1↑0") == true))
            {
                sb.Replace("0↑1", "1");
                sb.Replace("0↑0", "1");
                sb.Replace("1↑0", "1");

            }

            formula = sb.ToString();

            return formula;
        }

        static string Pirs(string formula, string symb)
        {
            var sb = new StringBuilder(formula);

            int pos = formula.IndexOf(symb);

            if (formula.Contains("0↓0") == true)
            {
                sb.Replace("0↓0", "1");
            }
            else if ((formula.Contains("0↓1") == true) || (formula.Contains("1↓0") == true) || (formula.Contains("1↓1") == true))
            {
                sb.Replace("0↓1", "0");
                sb.Replace("1↓0", "0");
                sb.Replace("1↓1", "0");
            }

            formula = sb.ToString();

            return formula;
        }

        static string XOR(string formula, string symb)
        {
            var sb = new StringBuilder(formula);

            int pos = formula.IndexOf(symb);

            if (formula.Contains("1⊕1") == true || formula.Contains("0⊕0"))
            {
                sb.Replace("0⊕0", "0");
                sb.Replace("1⊕1", "0");

            }
            else if ((formula.Contains("0⊕1") == true) || (formula.Contains("1⊕0") == true))
            {
                sb.Replace("1⊕0", "1");
                sb.Replace("0⊕1", "1");
            }

            formula = sb.ToString();

            return formula;
        }

        static string PositionByPriority(string formula)
        {
            int count = 0;
            string operations = "¬˄v→⇔↑↓⊕";
            foreach (char op in operations)
            {
                count = 0;
                char symbol = Convert.ToChar(op);
                for (int counter = 1; counter <= formula.Count(x => x == symbol); counter++)
                {

                    foreach (char c in formula)
                    {
                        if (count < formula.Count(x => x == symbol))
                        {
                            for (int i = formula.IndexOf(symbol); i > -1; i = formula.IndexOf(symbol))
                            {

                                //Console.WriteLine(formula);
                                formula = GetResultForOperationsInFormula(formula, symbol);
                                count++;
                            }
                        }
                    }
                }

                /*if (c == symbol)
                {
                    Console.WriteLine($"{symbol}: {formula.IndexOf(symbol)}");
                };*/

                if (formula.Length == 3)
                {
                    formula = GetResultForOperationsInFormula(formula, symbol);
                }
            }
            return formula;
        }

        static string DDNF(string formula, string vars)
        {
            var sb = new StringBuilder(formula);
            int zaper = 0;
            for (int i = 0; i < vars.Length; i++)
            {

                string letter = Convert.ToString(vars[i]);
                string binary = Convert.ToString(formula[i]);

                if (zaper >= 0)
                {
                    sb.Remove(i + zaper, 1);
                    if (binary == "0")
                    {
                        sb.Insert(i + zaper, "¬" + letter);
                        zaper++;
                    }
                    else if (binary == "1")
                    {
                        sb.Insert(i + zaper, letter);
                    }
                }
            }
            sb.Insert(0, "(");
            sb.Insert(sb.Length, ")");

            formula = sb.ToString();
            return formula;
        }


        static string DKNF(string formula, string vars)
        {
            var sb = new StringBuilder(formula);
            int zaper = 0;
            int disjunction = 0;
            for (int i = 0; i < vars.Length; i++)
            {

                string letter = Convert.ToString(vars[i]);
                string binary = Convert.ToString(formula[i]);

                if (zaper >= 0)
                {
                    sb.Remove(i + zaper + disjunction, 1);
                    if (binary == "1")
                    {
                        sb.Insert(i + zaper + disjunction, "¬" + letter);
                        zaper++;
                    }
                    else if (binary == "0")
                    {
                        sb.Insert(i + zaper + disjunction, letter);
                    }
                }
                disjunction++;
                sb.Insert(i + zaper + disjunction, "v");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Insert(0, "(");
            sb.Insert(sb.Length, ")");

            formula = sb.ToString();
            return formula;
        }



        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            //¬, ˄, v, →, ⇔, ↑, ↓, ⊕

            string formula = "¬X⊕¬Y⊕¬Z";   //"¬Av(B⇔C)→A"
            string vars = GetVarsForSet(formula);
            Console.WriteLine($"{vars}F");

            int countOfVars = vars.Length;
            int countOfSet = 1 << countOfVars;

            var sb = new StringBuilder();
            string table = "000";
            string dknf = DKNF(table, vars);
            string ddnf = DDNF(table, vars);

            var dd = new StringBuilder(ddnf);
            var dk = new StringBuilder(dknf);

            dd.Clear();
            dk.Clear();



            for (int i = 0; i < countOfSet; i++)
            {
                string set = GetASet(i, countOfVars);
                string formulaForSet = GetFormulaForSet(formula, vars, set);

                table = GetTableFromVars(formulaForSet, vars);
                Console.WriteLine(table + PositionByPriority(formulaForSet));
                sb.Append(PositionByPriority(formulaForSet));

                ddnf = DDNF(table, vars);
                dknf = DKNF(table, vars);


                if (PositionByPriority(formulaForSet) == "1")
                {
                    dd.Append(ddnf);
                    dd.Append("v");
                }

                else if (PositionByPriority(formulaForSet) == "0")
                {
                    dk.Append(dknf);
                    dk.Append("^");
                }

            }

            string binary = sb.ToString();
            string decim = Convert.ToInt64(binary, 2).ToString();

            dk.Remove(dk.Length - 1, 1);
            dd.Remove(dd.Length - 1, 1);


            Console.WriteLine($"{binary}={decim}");
            Console.WriteLine($"ДДНФ: {dd}");
            Console.WriteLine($"ДКНФ: {dk}");

            string formula_test = "¬0v(((0⇔0)→(¬0⇔0)))";   //"¬Av(B⇔C)→A"
            var result = "";
            var r = new StringBuilder(result);
            var form = new StringBuilder(formula_test);
            bool check = formula_test.Contains("(");
            r.Clear();


            /*for (int i = 0;check != false;)
            {
                formula_test = form.ToString();
                check = formula_test.Contains("(");
                result = Brackets(formula_test).ElementAtOrDefault(i);
                r.Append(result);
                r.Insert(0, "(");
                r.Insert(r.Length, ")");
                form.Replace(r.ToString(), PositionByPriority(result));
                Console.WriteLine(r);
                r.Clear();
                Console.WriteLine(formula_test);
            }*/

            Console.ReadKey();
        }
    }
}
