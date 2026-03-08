using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;


namespace E_education.App_Code
{
    public class cryptography
    {
       
        internal string imagename { get; set; }
        internal string imagecode { get; set; }
        internal cryptography getimagenamecode()
        {
           DeleteCaptcha();
            cryptography c = new cryptography();
            Bitmap b = new Bitmap(140, 30);
            Pen p = new Pen(Color.Navy);
            Font f = new Font("Cursive", 17, FontStyle.Strikeout);
            SolidBrush sb = new SolidBrush(color: Color.Maroon);
            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.Pink);
            g.DrawRectangle(p, 4, 120, 120, 25);
            chaptchacodegenerater cg = new chaptchacodegenerater();
            string captchacode=cg.getcaptchacode();
            c.imagecode = captchacode;
            g.DrawString(c.imagecode, f, sb, 3, 2);
            g.Flush();
            c.imagename = Path.GetRandomFileName() + ".jpg";
            string path = HttpContext.Current.Server.MapPath( "~/content/captchapic");
            if (!(Directory.Exists(path)))
                Directory.CreateDirectory(path);
            b.Save(path + "/" + c.imagename, ImageFormat.Jpeg);
            return c;
        }
        public void DeleteCaptcha()
        {
            string path = HttpContext.Current.Server.MapPath("~/content/captchapic");
            DirectoryInfo i=new DirectoryInfo(path);
            FileInfo[] f = i.GetFiles();
            foreach(FileInfo f2 in f)
            {
                f2.Delete();
            }
        }

    }
}