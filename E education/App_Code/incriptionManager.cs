using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_education.App_Code
{
    public class incriptionManager
    {
       
        internal int Ascii;
        internal int NewAscii;
        internal string encryptmypass(string PlainText)
        {
             string encryptText = " ";
            foreach(char c in PlainText)
            {
                Ascii = c;
                if(Ascii>=65 && Ascii <= 90)
                {
                    NewAscii= Ascii+32;

                }
                else if(Ascii>=97 && Ascii <= 122)
                {
                    NewAscii = Ascii - 32;
                }
                else if(Ascii>=48 && Ascii <= 57)
                {
                    NewAscii = Ascii + 5;
                }
                else
                {
                    NewAscii = Ascii;
                }
                char ch = (char) NewAscii;
                encryptText = encryptText + ch;
            }
            return encryptText;
        }
    }
}