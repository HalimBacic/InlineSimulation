using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace InlineVer3C__
{
    class Sintax
    {
        public static string promjenljiva = @"\[[A-Za-z\d]*\]";
        public Regex rgxTextDecl = new Regex(@"(^\[[A-Za-z\d\s]*\])=([A-Za-z\d\+\[\]\""\s]*)$");
        public Regex rgxInclude = new Regex(@"\#include");
        public Regex rgxDecl = new Regex(@"(^\[[A-Za-z\d]*\])([\s\=]*)([\d\+\-\*\/()\[\]A-Za-z]*)$");
        public Regex rgxPromjenljiva = new Regex(promjenljiva);

        public bool IsPrintable(string s)
        {
            if (!rgxDecl.IsMatch(s) && !rgxTextDecl.IsMatch(s))
                return true;
            else
                return false;
        }

        public bool IsDeklaracija(string s)
        {
            if (rgxDecl.IsMatch(s))
                return true;
            else
                return false;
        }

        public bool IsInclude(string s)
        {
            if (rgxInclude.IsMatch(s))
                return true; //Ili koristiti metodu
            else
                return false;
        }

        public bool IsTextDecl(string s)
        {
            
            if (rgxTextDecl.IsMatch(s))
                return true; //Ili koristiti metodu
            else
                return false;
        }
        
    }
}

