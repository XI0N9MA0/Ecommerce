
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image = System.Drawing.Image;

namespace ecommercetestttt
{
    public class User : Account
    {
        private string cusID;
        private int userPoint;
        private Image pfpicture;
        
        public User() : base()
        {
            cusID = null;
            userPoint = 0;
            Pfpicture = null;
        }

        public string CusID { get => cusID; set => cusID = value; }
        public int UserPoint { get => userPoint; set => userPoint = value; }
        public Image Pfpicture { get => pfpicture; set => pfpicture = value; }
    }
}
