
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Image = System.Drawing.Image;


namespace ecommercetestttt
{
    public static class Info
    {

        public static byte[] Get_ImgToBinary(Image img)
        {
            byte[] binaryPic;
            ImageConverter converter = new ImageConverter();
            binaryPic = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return binaryPic;
        }

        public static Image Get_BinaryToImg(byte[] BinImg)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(BinImg))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }

        }

        public static string GetPhone_whitespace(string phoneNo)
        {
            phoneNo = phoneNo.Replace(" ", string.Empty);
            //first ensure there is no whitespace between each character
            string phoneNo_whiteSpace = string.Empty;

            int countChar = 0;
            //insert whitespace into the string after 3 character ex: 012 345 678
            foreach (char c in phoneNo)
            {
                if (countChar == 3)
                {
                    phoneNo_whiteSpace += ' ';
                    phoneNo_whiteSpace += c;
                    countChar = 0;
                }
                else
                {
                    phoneNo_whiteSpace += c;
                }
                countChar++;
            }

            return phoneNo_whiteSpace;
        }

        public static Bitmap Get_Uploaded_Picture()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image (*.jpg)(*.jpeg)(*.png)|*.jpg;*.jpeg;*.png*";
            file.Title = "Please Select an Image";

            if (file.ShowDialog() == DialogResult.OK)
            {
                return new Bitmap(file.OpenFile());
            }
            return null;
        }


        public static decimal Get_Discount(int cusPoint)
        {
            decimal Discount;
            if (cusPoint >= 500 && cusPoint < 1000)
            {
                Discount = 0.05m;
            }
            else if (cusPoint >= 1000 && cusPoint < 2000)
            {
                Discount = 0.1m;
            }
            else if (cusPoint >= 2000 && cusPoint < 3000)
            {
                Discount = 0.15m;
            }
            else if (cusPoint == 3000)
            {
                Discount = 0.2m;
            }
            else
            {
                Discount = 0;
            }
            return Discount;
        }

        public static int Get_Point(decimal total)
        {
            int point;
            if (total >= 50 && total < 100)
            {
                point = 20;
            }
            else if(total >= 100 && total < 200)
            {
                point = 50;
            }
            else if(total >= 200 && total < 300)
            {
                point = 100;
            }
            else if(total >= 300 && total < 400)
            {
                point = 200;
            }
            else if(total >= 400 && total < 700)
            {
                point = 600;
            }
            else if(total >= 700 && total < 1000)
            {
                point = 900;
            }
            else if(total > 1000)
            {
                point = 1200;
            }
            else
            {
                point = 0;
            }
            return point;
        }

        



    }
}
