using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_education.App_Code
{
    public class chaptchacodegenerater
    {
        string code = " ";
        char ch;
        Random r=new Random();
        internal string getcaptchacode()
        {
            int c = r.Next(65, 90);
            ch= (char)c;
            code = code + ch;
            c = r.Next(97, 122);
            ch = (char)c;
            code= code + ch;
            c = r.Next(65, 90);
            ch=(char)c;
            c = r.Next(0, 9);

            if (c % 2 == 0)
            {
                code = code + c;
            }
            else
            {
                code = code + (c - 1);
            }
            c = r.Next(97, 122);
            ch=(char)c;
            code = code + ch;
            c = r.Next(48, 57);
            code = code + c;
            return code;
        }
    }
}