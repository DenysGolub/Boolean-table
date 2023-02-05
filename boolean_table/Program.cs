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
        /*Returns the binary from of the length n for the decimal d*/
        static string GetBinaryForm(int decim, int targetLength)
        {
            string shortBinaryForm = Convert.ToString(decim, 2);
            return shortBinaryForm.PadLeft(targetLength, '0');
        }

        /*Replaces all 'vars' in 'formula' with corresponding binary 'values'
         represented as string of 0s and 1s*/
        static string GetFormulaForSet(string formula, string vars, string set)
        {
            for (int i = 0; i < vars.Length; i++)
                formula = formula.Replace(vars[i], set[i]);
            return formula;
        }

        /*Returns all vars that appears in formula in one copy*/
        static string GetVarsForSet(string formula)
        {
            formula = formula.Replace("(", "").Replace(")", "");
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

        /*Return new formula which performed actions (replaces) according to the "symbol" */

        static string GetResultForOperationsInFormula(string formula, char symbol)
        {

            string symb = Convert.ToString(symbol);
            switch (symb)
            {
                case "¬":
                    formula = LogicalNO(formula);
                    break;
                case "˄":
                    formula = Conjuction(formula);
                    break;
                case "v":
                    formula = Disjunction(formula);
                    break;
                case "→":
                    formula = Implication(formula);
                    break;
                case "⇔":
                    formula = Equivalence(formula);
                    break;
                case "↑":
                    formula = Sheffler(formula);
                    break;
                case "↓":
                    formula = Pirs(formula);
                    break;
                case "⊕":
                    formula = XOR(formula);
                    break;
            }
            return formula;
        }

        /*Returns the substrings enclosed in parentheses
         example: Av((B^C)v(AvC))
         1) AvC
         2) B^C
         3) (B^C)v(AvC) */

        private static IEnumerable<String> Brackets(string value)
        {
            if (string.IsNullOrEmpty(value))
                yield break; // or throw exception

            Stack<int> brackets = new Stack<int>();

            for (int i = 0; i < value.Length; ++i)
            {
                char ch = value[i];

                if (ch == '(')
                    brackets.Push(i);
                else if (ch == ')')
                {
                    int openBracket = brackets.Pop();

                    yield return value.Substring(openBracket + 1, i - openBracket - 1);
                }

            }
            yield return value;
        }

        /*START BLOCK WITH LOGICAL OPERATIONS
         in this block, the methods return a formula in which replaces are made according to the symbol
         and logical table for each logical operation. Name of methods == logical operation. */
        static string LogicalNO(string formula)
        {
            var sb = new StringBuilder(formula);

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

        static string Conjuction(string formula)
        {
            var sb = new StringBuilder(formula);

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

        static string Disjunction(string formula)
        {
            var sb = new StringBuilder(formula);

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

        static string Implication(string formula)
        {
            var sb = new StringBuilder(formula);

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

        static string Equivalence(string formula)
        {
            var sb = new StringBuilder(formula);

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

        static string Sheffler(string formula)
        {
            var sb = new StringBuilder(formula);

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

        static string Pirs(string formula)
        {
            var sb = new StringBuilder(formula);

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

        static string XOR(string formula)
        {
            var sb = new StringBuilder(formula);

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

        /*END BLOCK WITH LOGICAL OPERATIONS*/


        /*Return a result (0 or 1) for formula.
        //Logical operations has a priority, so this method using foreach to check operations one by one.*/

        static string PositionByPriority (string formula)
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
                                formula = GetResultForOperationsInFormula(formula, symbol);      
                                count++;
                            }
                        }
                    }
                }
                if (formula.Length == 3)
                {
                    formula = GetResultForOperationsInFormula(formula, symbol);
                }
            }
            return formula;
        }


        /*Return DDNF for set*/
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
                    sb.Remove(i+zaper, 1);
                    if (binary == "0")
                    {
                        sb.Insert(i+zaper, "¬" + letter);
                        zaper++;
                    }
                    else if (binary == "1")
                    {
                        sb.Insert(i+zaper, letter);
                    }
                }
            }
            sb.Insert(0, "(");
            sb.Insert(sb.Length, ")");
            formula = sb.ToString();
            return formula;
        }

        /*Return DDNF for set*/

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
                    sb.Remove(i + zaper+disjunction, 1);
                    if (binary == "1")
                    {
                        sb.Insert(i + zaper+disjunction, "¬" + letter);
                        zaper++;
                    }
                    else if (binary == "0")
                    {
                        sb.Insert(i + zaper+disjunction, letter);
                    }
                }
                disjunction++;
                sb.Insert(i+zaper+disjunction, "v");
            }
            sb.Remove(sb.Length-1, 1);
            sb.Insert(0, "(");
            sb.Insert(sb.Length, ")");

            formula = sb.ToString();
            return formula;
        }



        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            //¬, ˄, v, →, ⇔, ↑, ↓, ⊕

            string formula = "A⇔(B↑C)→B";   //"¬Av(B⇔C)→A"
            string vars = GetVarsForSet(formula);

            Console.WriteLine(formula);
            Console.WriteLine($"{vars}F");

            int countOfVars = vars.Length;
            int countOfSet = 1 << countOfVars;

            var sb = new StringBuilder();
            string table = "000";
            string dknf = DKNF(table, vars);
            string ddnf = DDNF(table, vars);

            var dd = new StringBuilder(ddnf); //StringBuilder for DDNF
            var dk = new StringBuilder(dknf); //StringBuilder for DKNF

            dd.Clear();
            dk.Clear();

            for (int i = 0; i < countOfSet; i++)
            {
                string set = GetBinaryForm(i, countOfVars);
                string formulaForSet = GetFormulaForSet(formula, vars, set);

                var result = "";
                var r = new StringBuilder(result);
                var form = new StringBuilder(formulaForSet);

                bool check = formulaForSet.Contains("("); //Check for brackets in formula
                r.Clear();
                string formula_test = form.ToString();

                if (formulaForSet.Contains("(") == true)
                {
                    for (int n = 0; check != false;)
                    {
                        formula_test = form.ToString();
                        check = formula_test.Contains("(");

                        result = Brackets(formula_test).ElementAtOrDefault(n);
                        r.Append(result);
                        r.Insert(0, "(");
                        r.Insert(r.Length, ")");

                        form.Replace(r.ToString(), PositionByPriority(result));
                        r.Clear();
                    }
                }

                Console.WriteLine(set + PositionByPriority(formula_test));
                sb.Append(PositionByPriority(formula_test));
                
                ddnf = DDNF(set, vars);
                dknf = DKNF(set, vars);


                if (PositionByPriority(formula_test) == "1")
                {
                    dd.Append(ddnf);
                    dd.Append("v");
                }

                else if (PositionByPriority(formula_test) == "0")
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
            Console.WriteLine();
            Console.WriteLine($"ДКНФ: {dk}");

            Console.ReadKey();
        }
    }
}
