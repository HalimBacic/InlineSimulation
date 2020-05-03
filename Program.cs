using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace InlineVer3C__
{

    class Program
    {

        static void Main(string[] args)
        {
            Dictionary<string, string> Exps = new Dictionary<string, string>();
            Parser mat = new Parser();
            Sintax sin = new Sintax();
            foreach (string line in File.ReadLines("C:\\Users\\bacic\\Desktop\\S1\\FM\\InlineVer3C++\\file.txt"))
            {
              mat.LeftandRight(line);//Success
              /*  if (sin.IsPrintable(line))
                {
                    string line2=mat.NormalizeText(line);
                    MatchCollection matches = Regex.Matches(line2, @"\[[\""A-Za-z\d]*\]");
                    foreach (Match match in matches)
                    {
                        line2=line.Replace(match.Value, Exps[match.Value]);
                        Console.WriteLine(line2);
                    }
                    
                }
                if (sin.IsTextDecl(line)) //Success
                {
                    string line2=mat.NormalizeText(line);
                    mat.Values(line2);
                    MatchCollection matches = Regex.Matches(mat.rigthside, @"\[[\""A-Za-z\d]*\]");
                    foreach (Match match in matches)
                        Exps.Add(mat.leftside, mat.rigthside);
                }
                if (sin.IsInclude(line))
                { Console.WriteLine("Command Missing."); }
                */if (sin.IsDeklaracija(line))  //Success
                {
                
                    string line2;
                    line2=mat.NormalizeExp(line);
                    mat.LeftandRight(line2);
                    mat.Values(line2);
                    mat.rigthside = mat.valueMath.ToString();
                    Exps.Add(mat.leftside,mat.rigthside);
                }
            }
        }
    }
}