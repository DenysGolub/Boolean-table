using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            string vars = "XYZTK";
            int countOfVars = vars.Length;
            int countOfSet = 1 << countOfVars;
            string formula = "X˄Y→Z˅Y˅T˄K";

            for (int i = 0; i < countOfSet; i++)
            {
                string set = GetASet(i, countOfVars);
                string formulaForSet = GetFormulaForSet(formula, vars, set);
                Console.WriteLine(formulaForSet);
            }

            Console.ReadKey();
        }
    }
}
