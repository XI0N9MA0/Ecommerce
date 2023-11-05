using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    public class Customer
    {
        protected int id;
        protected string firstName;
        protected string lastName;
        protected string birthDate;
        protected char gender;
        protected string phoneNo;
        protected string email;
        protected int point;
        protected string address;
        protected Image profilePicture;
        protected byte[] BinProfilePicture;

        public Customer()
        {
            ID = 0;
            FirstName = null;
            LastName = null;
            Gender = 'U';
            PhoneNo = null;
            Email = null;
            Point = 0;
            Address = null;
            ProfilePicture = null;
        }

        public int ID { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public char Gender { get => gender; set => gender = value; }
        public string PhoneNo { get => phoneNo; set => phoneNo = value; }
        public string Email { get => email; set => email = value; }
        public int Point { get => point; set => point = value; }
        public string Address { get => address; set => address = value; }
        public Image ProfilePicture { get => profilePicture; set => profilePicture = value; }
        public string BirthDate { get => birthDate; set => birthDate = value; }
        public byte[] BinProfilePicture1 { get => BinProfilePicture; set => BinProfilePicture = value; }

        public byte[] get_ImgToBinary_ProfilePic()
        {
            byte[] binaryPic;
            ImageConverter converter = new ImageConverter();
            binaryPic = (byte[])converter.ConvertTo(ProfilePicture, typeof(byte[]));
            return binaryPic;
        }



    }
}
