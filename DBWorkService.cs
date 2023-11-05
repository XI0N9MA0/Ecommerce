using Guna.UI2.WinForms;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Guna.UI2.Native.WinApi;

namespace ecommercetestttt
{
    public class DBWorkService
    {
        //SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-4FMPOU4\SQLEXPRESS;Initial Catalog=dbECommerce;Integrated Security=True");
        private SqlConnection sqlConnection = EcommerceDB.GetConnection();
        private SqlCommand cmd;
        private SqlDataReader reader;
        private string sql;


        public void Insert_IntoInvoice(Invoice invoice, List<INVOICEITEM> products)
        {
            sqlConnection.Open();
            sql = @"INSERT INTO Invoices (invoiceDate, total, discount, subTotal, paidAmount, cusID)
                    VALUES (@invoiceDate, @total, @discount, @subTotal, @paidAmount, @cusID)";
            cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@invoiceDate", invoice.InvoiceDate);
            cmd.Parameters.AddWithValue("@total", invoice.Total);
            cmd.Parameters.AddWithValue("@discount", invoice.Discount);
            cmd.Parameters.AddWithValue("@subTotal", invoice.Subtotal);
            cmd.Parameters.AddWithValue("@paidAmount", invoice.PaidAmount);
            cmd.Parameters.AddWithValue("@cusID", invoice.CusID);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();

            //add points to customer based on total value
            AddPointToCustomer(invoice.CusID, invoice.Total);

            //to read the latest inserted invoiceID value 
            //later can use to set value in invoice detail table
            sqlConnection.Open();
            sql = @"SELECT MAX(invoiceID) AS 'invoiceID' FROM Invoices";
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                invoice.InvoiceID = int.Parse(reader["invoiceID"].ToString());
            }
            sqlConnection.Close();

            sqlConnection.Open();
            foreach (INVOICEITEM product in products)
            { 
                if (product.Id.Contains("B"))
                {
                    sql = @"INSERT INTO BookInvoiceDetail (invoiceID, bookID, qty, unitPrice, totalAmount)
                    VALUES (@invoiceID, @bookID, @qty, @unitPrice, @totalAmount)";
                    cmd = new SqlCommand(sql, sqlConnection);
                    cmd.Parameters.AddWithValue("@invoiceID", invoice.InvoiceID);
                    cmd.Parameters.AddWithValue("@bookID", product.Id);
                    cmd.Parameters.AddWithValue("@qty", product.Qty);
                    cmd.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
                    cmd.Parameters.AddWithValue("@totalAmount", product.TotalAmount);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    sql = @"INSERT INTO ElectInvoiceDetail (invoiceID, electronicID, qty, unitPrice, totalAmount)
                    VALUES (@invoiceID, @electronicID, @qty, @unitPrice, @totalAmount)";
                    cmd = new SqlCommand(sql, sqlConnection);
                    cmd.Parameters.AddWithValue("@invoiceID", invoice.InvoiceID);
                    cmd.Parameters.AddWithValue("@electronicID", product.Id);
                    cmd.Parameters.AddWithValue("@qty", product.Qty);
                    cmd.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
                    cmd.Parameters.AddWithValue("@totalAmount", product.TotalAmount);
                    cmd.ExecuteNonQuery();
                }
            }
            sqlConnection.Close();

            
        }

        public void AddPointToCustomer(string cusID, decimal total)
        {
            int previousPoint = 0;
            sqlConnection.Open();
            sql = @"SELECT point FROM Customers WHERE cusID ="+cusID;
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                previousPoint = int.Parse(reader["point"].ToString());
            }
            sqlConnection.Close();

            int newPoint = previousPoint + Info.Get_Point(total);

            //Maximum point is 3000 points  
            //if customer reach beyond that do not add more point to customer
            if (newPoint >= 3000)
            {
                sqlConnection.Open();
                sql = @"UPDATE Customers set point = @point WHERE cusID = " + cusID;
                cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@point", 3000);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            else
            {
                sqlConnection.Open();
                sql = @"UPDATE Customers set point = @point WHERE cusID = " + cusID;
                cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@point", newPoint);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            
        }

        public Invoice Get_InvoiceInfo(Invoice invoice, string cusID, int cusPoint)
        {
            invoice.CusID = cusID;
            sqlConnection.Open();
            sql = @"SELECT * FROM Carts WHERE cusID = " + invoice.CusID;
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                invoice.Total += decimal.Parse(reader["total_amount"].ToString());                
            }
            sqlConnection.Close();
            invoice.InvoiceDate = DateTime.Now.Date.ToString("yyyy/MM/dd");

            //check discount result based on customer point 
            invoice.Discount = Info.Get_Discount(cusPoint);
            invoice.Subtotal = invoice.Total - (invoice.Total * invoice.Discount);
            invoice.PaidAmount = invoice.Subtotal;

            return invoice;
        }

        public List<INVOICEITEM> Get_AllProduct_FromCarts(List<INVOICEITEM> products, string cusID)
        {
            sqlConnection.Open();
            sql = @"SELECT * FROM Carts WHERE cusID = " + cusID;
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                INVOICEITEM item = new INVOICEITEM();
                item.Id = reader["productID"].ToString();
                item.ItemName = reader["productName"].ToString();
                item.Qty = int.Parse(reader["qty"].ToString());
                item.UnitPrice = decimal.Parse(reader["unit_price"].ToString());
                item.TotalAmount = decimal.Parse(reader["total_amount"].ToString());
                products.Add(item);
            }

            sqlConnection.Close();
            return products;
        }

        public void display_invoice(frmInvoice formInvoice, string cusID, string invoiceID)
        {
            INVOICEITEM item = new INVOICEITEM();
            try
            {
                //read data from InvoiceBookDetail
                sqlConnection.Open();
                sql = @"SELECT 
	                    O.bookID,
                        B.title,
	                    O.qty,
	                    O.unitPrice,
	                    O.totalAmount
                    FROM Invoices I
                    JOIN(BookInvoiceDetail O 
                    JOIN Books B
                    ON O.bookID = B.bookID)
                    ON I.invoiceID = O.invoiceID
                    WHERE I.cusID = " + cusID+" AND I.invoiceID =" +invoiceID;
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new INVOICEITEM();
                    item.Id = reader["bookID"].ToString();
                    item.ItemName = reader["title"].ToString();
                    item.Qty = int.Parse(reader["qty"].ToString());
                    item.UnitPrice = decimal.Parse(reader["unitPrice"].ToString());
                    item.TotalAmount = decimal.Parse(reader["totalAmount"].ToString());
                    formInvoice.FLPItemList.Controls.Add(item);
                }
                sqlConnection.Close();

                //read data from ElectInvoiceDetail
                sqlConnection.Open();
                sql = @"SELECT 
	                    O.electronicID,
	                    E.electronicName,
	                    O.qty,
	                    O.unitPrice,
	                    O.totalAmount
                    FROM Invoices I
                    JOIN(ElectInvoiceDetail O 
                    JOIN Electronics E
                    ON O.electronicID = E.electronicID)
                    ON I.invoiceID = O.invoiceID
                    WHERE I.cusID = " + cusID + " AND I.invoiceID =" + invoiceID;
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new INVOICEITEM();
                    item.Id = reader["electronicID"].ToString();
                    item.ItemName = reader["electronicName"].ToString();
                    item.Qty = int.Parse(reader["qty"].ToString());
                    item.UnitPrice = decimal.Parse(reader["unitPrice"].ToString());
                    item.TotalAmount = decimal.Parse(reader["totalAmount"].ToString());
                    formInvoice.FLPItemList.Controls.Add(item);
                }
                sqlConnection.Close();

                //read data from invoice
                sqlConnection.Open();
                Invoice invoice = new Invoice();
                sql = @"SELECT 
	                        I.invoiceID,
	                        I.invoiceDate,
	                        I.total,
	                        I.discount,
	                        I.subTotal,
                            I.paidAmount,
	                        C.address
                        FROM Invoices I JOIN Customers C ON I.cusID = C.cusID
                        WHERE I.cusID = " + cusID + " AND I.invoiceID =" + invoiceID;
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    invoice.InvoiceID = int.Parse(reader["invoiceID"].ToString());
                    invoice.InvoiceDate = reader["invoiceDate"].ToString();
                    invoice.Total = decimal.Parse(reader["total"].ToString());
                    invoice.Discount = decimal.Parse(reader["discount"].ToString());
                    invoice.Subtotal = decimal.Parse(reader["subTotal"].ToString());
                    invoice.PaidAmount = decimal.Parse(reader["paidAmount"].ToString());
                    invoice.CusID = cusID;
                    invoice.Address = reader["address"].ToString();
                }

                formInvoice.lblDate.Text = DateTime.Parse(invoice.InvoiceDate).ToString("dd/MM/yyyy");
                formInvoice.lblID.Text = invoice.InvoiceID.ToString();
                formInvoice.lblAddress.Text = invoice.Address;
                formInvoice.lblCusID.Text = invoice.CusID;
                formInvoice.lblTotal.Text = "$" + string.Format("{0:F2}", invoice.Total);
                formInvoice.lblDiscount.Text = String.Format("{0:F0}", invoice.Discount * 100)+"%";
                formInvoice.lblSubtotal.Text = "$" + string.Format("{0:F2}", invoice.Subtotal);
                formInvoice.lblPaidAmount.Text = "$" + string.Format("{0:F2}", invoice.PaidAmount);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"frm invoice");
            }
        }

        public void Delete_FromCarts()
        {
            sqlConnection.Open();
            sql = @"DELETE FROM Carts";
            cmd = new SqlCommand(sql, sqlConnection);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void Delete_SpecificItemFromCart(string cusID, string productID)
        {
            sqlConnection.Open();
            sql = @"DELETE FROM Carts WHERE cusID ='" + cusID + "' AND productID = '" + productID + "'";
            cmd = new SqlCommand(sql, sqlConnection);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public int Get_ProductQty(string productID)
        {
            int qty = 0;
            if (productID.Contains("B"))
            {
                sqlConnection.Open();
                sql = @"SELECT bookID, qty FROM Books WHERE bookID = '" + productID+"'";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    qty = int.Parse(reader["qty"].ToString());
                }
                sqlConnection.Close();
                return qty;
            }
            else
            {
                sqlConnection.Open();
                sql = @"SELECT electronicID, qty FROM Electronics WHERE electronicID = '" + productID+"'";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    qty = int.Parse(reader["qty"].ToString());
                }
                sqlConnection.Close();
                return qty;
            }
        }

        

        

        public void Get_AllBook(ref List<Book> books)
        {
            Book bk = new Book();
            sqlConnection.Open();
            sql = @"SELECT * FROM Books";
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                bk = new Book();
                bk.Id = reader["bookID"].ToString();
                bk.Qty = int.Parse(reader["qty"].ToString());
                bk.SoldQty = int.Parse(reader["soldQty"].ToString());
                books.Add(bk);
            }
            sqlConnection.Close();
           
        }

        public void Get_AllElectronic(ref List<Electronic> electronics)
        {
            Electronic elec = new Electronic();
            sqlConnection.Open();
            sql = @"SELECT * FROM Electronics";
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                elec = new Electronic();
                elec.Id = reader["electronicID"].ToString();
                elec.Qty = int.Parse(reader["qty"].ToString());
                elec.SoldQty = int.Parse(reader["soldQty"].ToString());
                electronics.Add(elec);
            }
            sqlConnection.Close();
        }

        public void Decrease_ProductQty_Increase_ProductSoldQty(List<INVOICEITEM> products)
        {
            List<Book> books = new List<Book>();
            List<Electronic> electronics = new List<Electronic>();

            Get_AllBook(ref books);
            Get_AllElectronic(ref electronics);

            sqlConnection.Open();
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Id.Contains("B"))
                {
                    for (int j = 0; j < books.Count; j++)
                    {
                        if (books[j].Id == products[i].Id)
                        {
                            sql = @"UPDATE Books Set qty = @qty, soldQty = @soldQty, inStock_Status = @inStock_Status WHERE bookID = '" + products[i].Id + "'";
                            cmd = new SqlCommand(sql, sqlConnection);
                            int bookQTY = books[j].Qty - products[i].Qty;
                            int soldQTY = books[j].SoldQty + products[i].Qty;
                            cmd.Parameters.AddWithValue("@qty", bookQTY);
                            cmd.Parameters.AddWithValue("@soldQty", soldQTY);
                            if (bookQTY <= 0)
                            {
                                cmd.Parameters.AddWithValue("@inStock_Status", 0);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@inStock_Status", 1);
                            }
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < electronics.Count; j++)
                    {
                        if (electronics[j].Id == products[i].Id)
                        {
                            sql = @"UPDATE Electronics Set qty = @qty, soldQty = @soldQty, inStock_Status = @inStock_Status WHERE electronicID = '" + products[i].Id + "'";
                            cmd = new SqlCommand(sql, sqlConnection);
                            int elecQty = electronics[j].Qty - products[i].Qty;
                            int soldQTY = books[j].SoldQty + products[i].Qty;
                            cmd.Parameters.AddWithValue("@qty", elecQty);
                            cmd.Parameters.AddWithValue("@soldQty", soldQTY);
                            if (elecQty <= 0)
                            {
                                cmd.Parameters.AddWithValue("@inStock_Status", 0);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@inStock_Status", 1);
                            }
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            sqlConnection.Close();
        }

        
        public void FromCarts_To_EachCartItem_AndDisplayCartItems(frmCart frmcart, string cusID, int point)
        {
            decimal totalValue = 0m;
            try
            {
                if (cusID != null)
                {
                    sqlConnection.Open();
                    sql = @"SELECT * FROM Carts WHERE cusID = " + cusID;
                    cmd = new SqlCommand(sql, sqlConnection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CartItem cartItem = new CartItem(cusID, frmcart);
                        cartItem.ProductId = reader["productId"].ToString();
                        cartItem.Product_Name = reader["productName"].ToString();
                        cartItem.Picture = Info.Get_BinaryToImg((byte[])reader["picture"]);
                        cartItem.UnitPrice = decimal.Parse(reader["unit_price"].ToString());
                        cartItem.Qty = int.Parse(reader["qty"].ToString());
                        cartItem.TotalAmount = decimal.Parse(reader["total_amount"].ToString());
                        frmcart.cartFLP.Controls.Add(cartItem);
                        totalValue += cartItem.TotalAmount;
                    }
                    sqlConnection.Close();
                    frmcart.lblTotal.Text = totalValue.ToString("c");
                    decimal discount = Info.Get_Discount(point);
                    frmcart.lblDiscount.Text = (discount * 100).ToString()+"%";
                    frmcart.lblSubTotal.Text = (totalValue-totalValue*discount).ToString("c");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<Invoice> Get_InvoiceIDNDate_AndAddtoSearchCollection(string cusID, List<Invoice> invoices, frmInvoiceList frminvlist)
        {
            try
            {
                AutoCompleteStringCollection invlist = new AutoCompleteStringCollection();
                Invoice invoice;
                sqlConnection.Open();
                sql = @"SELECT invoiceID, invoiceDate FROM Invoices WHERE cusID = " + cusID+ " ORDER BY invoiceID DESC";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    invoice = new Invoice();
                    invoice.InvoiceID = int.Parse(reader["invoiceID"].ToString());
                    invoice.InvoiceDate = reader["invoiceDate"].ToString();
                    invoices.Add(invoice);
                    invlist.Add(invoice.InvoiceID.ToString());
                }
                frminvlist.txtSearch.AutoCompleteCustomSource = invlist;
                sqlConnection.Close();
                return invoices;
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            
        }

        

        

        

        public void Get_UserInfo(User user)
        {
            sqlConnection.Open();
            sql = @"SELECT username, point, profilePicture, cusID
                    FROM Accounts A 
                    JOIN Customers C 
                    ON A.accountID = C.accountID
                    WHERE A.accountID = '" + user.AccountID + "'";
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user.Pfpicture = Info.Get_BinaryToImg((byte[])reader["profilePicture"]);
                user.Username = reader["username"].ToString();
                user.UserPoint = int.Parse(reader["point"].ToString());
                user.CusID = reader["cusID"].ToString();
            }
            sqlConnection.Close();
        }

        public void Get_AllProducts_AndAddToStringCollection(User user, FormUser frmUser)
        {
            if (frmUser.cmbCategory.SelectedItem.ToString() == "Books")
            {
                AutoCompleteStringCollection bookCollection = new AutoCompleteStringCollection();

                sqlConnection.Open();
                sql = "SELECT * FROM Books ORDER BY soldQty DESC";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ITEM item = new ITEM(frmUser.cmbCategory.SelectedItem.ToString(), user.CusID);
                    item.Id = reader["bookID"].ToString();
                    item.Title = reader["title"].ToString();
                    item.Author = reader["author"].ToString();
                    item.Price = decimal.Parse(reader["price"].ToString());
                    item.Rate = decimal.Parse(reader["rate"].ToString());
                    item.Picture = Info.Get_BinaryToImg((byte[])reader["picture"]);

                    //gather all titles into 1 collection
                    //for search suggestion
                    bookCollection.Add(item.Title);
                    //add items into flowlayoutpanel
                    frmUser.FLPItem.Controls.Add(item);

                }
                sqlConnection.Close();
                frmUser.txtSearch.AutoCompleteCustomSource = bookCollection;
            }
            else
            {
                AutoCompleteStringCollection elecCollection = new AutoCompleteStringCollection();

                sqlConnection.Open();
                sql = "SELECT * FROM Electronics ORDER BY soldQty DESC";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ITEM item = new ITEM(frmUser.cmbCategory.SelectedItem.ToString(), user.CusID);
                    item.Id = reader["electronicID"].ToString();
                    item.Title = reader["electronicName"].ToString();
                    item.Company = reader["company"].ToString();
                    item.Price = decimal.Parse(reader["price"].ToString());
                    item.Rate = decimal.Parse(reader["rate"].ToString());
                    item.Picture = Info.Get_BinaryToImg((byte[])reader["picture"]);

                    //gather all titles into 1 collection
                    //for search suggestion
                    elecCollection.Add(item.Title);
                    //add items into flowlayoutpanel
                    frmUser.FLPItem.Controls.Add(item);

                }
                frmUser.txtSearch.AutoCompleteCustomSource = elecCollection;
                sqlConnection.Close();
            }
        }

        public void Search_Products(User user, FormUser frmUser)
        {
            if (frmUser.cmbCategory.SelectedItem.ToString() == "Books")
            {
                sqlConnection.Open();
                sql = "SELECT * FROM Books ORDER BY soldQty DESC";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //covert all strings to lower case to prevent case sensitivity while searching
                    //search by product or search by product ID
                    if (reader["title"].ToString().ToLower().Contains(frmUser.txtSearch.Text.ToLower()) || reader["bookID"].ToString().ToLower().Contains(frmUser.txtSearch.Text.ToLower()))
                    {
                        ITEM item = new ITEM(frmUser.cmbCategory.SelectedItem.ToString(), user.CusID);
                        item.Id = reader["bookID"].ToString();
                        item.Title = reader["title"].ToString();
                        item.Author = reader["author"].ToString();
                        item.Price = decimal.Parse(reader["price"].ToString());
                        item.Rate = decimal.Parse(reader["rate"].ToString());
                        item.Picture = Info.Get_BinaryToImg((byte[])reader["picture"]);
                        frmUser.FLPItem.Controls.Add(item);
                    }
                }
                sqlConnection.Close();
            }
            else
            {
                sqlConnection.Open();
                sql = "SELECT * FROM Electronics ORDER BY soldQty DESC";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["electronicName"].ToString().ToLower().Contains(frmUser.txtSearch.Text.ToLower()) || reader["electronicID"].ToString().ToLower().Contains(frmUser.txtSearch.Text.ToLower()))
                    {
                        ITEM item = new ITEM(frmUser.cmbCategory.SelectedItem.ToString(), user.CusID);
                        item.Id = reader["electronicID"].ToString();
                        item.Title = reader["electronicName"].ToString();
                        item.Company = reader["company"].ToString();
                        item.Price = decimal.Parse(reader["price"].ToString());
                        item.Rate = decimal.Parse(reader["rate"].ToString());
                        item.Picture = Info.Get_BinaryToImg((byte[])reader["picture"]);
                        frmUser.FLPItem.Controls.Add(item);
                    }
                }
                sqlConnection.Close();
            }
        }

        public void SearchInvoices(string cusID, frmInvoiceList frmInvlist)
        {
            InvoiceListItem item;
            sqlConnection.Open();
            sql = "SELECT * FROM Invoices WHERE cusID = "+cusID;
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //covert all strings to lower case to prevent case sensitivity while searching
                //search by product or search by product ID
                if (reader["invoiceID"].ToString().ToLower().Contains(frmInvlist.txtSearch.Text.ToLower()))
                {
                    item = new InvoiceListItem(cusID);
                    item.InvoiceID = reader["invoiceID"].ToString();
                    item.InvoiceDate = reader["invoiceDate"].ToString();
                    frmInvlist.FLPInvoicesList.Controls.Add(item);
                }
            }
            sqlConnection.Close();
        }

    }


}
