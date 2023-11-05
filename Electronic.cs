using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    public class Electronic : Product
    {
        private string name;
        private string releaseDate;
        private string color;
        private string company;
        private decimal size;
        private string type;
        private int electronicTypeID;

        public string ReleaseDate { get { return releaseDate; } set { releaseDate = value; } }
        public string Color { get { return color; } set { color = value; } }

        public string Company { get => company; set => company = value; }
        public decimal Size { get => size; set => size = value; }
        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public int ElectronicTypeID { get => electronicTypeID; set => electronicTypeID = value; }

        public Electronic() : base()
        {
            Name = null;
            Color = null;
            ReleaseDate = DateTime.Now.ToString("yyyy/MM/dd");
            Company = null;
            Size = 0.0m;
            Type = null;
            ElectronicTypeID = 0;
        }
    }
}
