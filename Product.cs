using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    public class Product
    {
        private string id;
        private string title;
        private string description;
        private decimal price;
        private int qty;
        private int soldQty;
        private Boolean inStockStatus;
        private Image picture;
        private decimal rate;
        private int totalRate;

        public string Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public decimal Price { get => price; set => price = value; }
        public int Qty { get => qty; set => qty = value; }
        public int SoldQty { get => soldQty; set => soldQty = value; }
        public bool InStockStatus { get => inStockStatus; set => inStockStatus = value; }

        public decimal Rate { get => rate; set => rate = value; }
        public int TotalRate { get => totalRate; set => totalRate = value; }
        public Image Picture { get => picture; set => picture = value; }

        //Default Contructor
        public Product()
        {
            this.id = null;
            this.title = null;
            this.description = null;
            this.price = 0;
            this.qty = 0;
            this.soldQty = 0;
            this.inStockStatus = false;
            this.picture = null;
            this.rate = 0;
            this.totalRate = 0;
        }

        public Product(INVOICEITEM item)
        {
            this.id = item.Id;
            this.title = item.ProductName;
            this.price = item.UnitPrice;
            this.qty = item.Qty;
        }

        //Constructor
        public Product(string id, string title, string description, decimal price, int qty, int soldQty, bool inStockStatus, Image picture, decimal rate, int totalRate)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.price = price;
            this.qty = qty;
            this.soldQty = soldQty;
            this.inStockStatus = inStockStatus;
            this.picture = picture;
            this.rate = rate;
            this.totalRate = totalRate;
        }
    }
}
