using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using System.Data;
using System.Collections;
using System.Globalization;
using BookLibrary;

namespace _3342_Project_2
{
    public partial class Bookstore : System.Web.UI.Page
    {

        private const int firstColumn = 0;
        private const int quantityColumn = 5;
        private const int totalColumn = 6;

        // creating a bunch of object
        DBConnect DBConnect = new DBConnect();
        ArrayList bookArray = new ArrayList();
        CheckBox chkBox;
        DropDownList ddl;
        DropDownList ddl2;
        TextBox txtBox;

        protected void Page_Load(object sender, EventArgs e)
        {
            DBConnect objDB = new DBConnect();
            DataSet myDS;
            string strSQL = "Select * FROM Books";

            myDS = objDB.GetDataSet(strSQL);
            gvBookstore.DataSource = myDS;
            gvBookstore.DataBind();
        }


        // this method validate the gridview input
        public Boolean checkGridview(String drp, String rdb, String txBox)
        {
            int num = 0;
            Boolean flag = true;

            if (drp == "Select")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert11", "alert('Please choose the book choice!');", true);
                flag = false;
            }

            if (rdb == "" || rdb == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert12", "alert('Pick rent or buy for your book!');", true);
                flag = false;
            }

            if (txBox == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert13", "alert('Please put the quantity of book you like!');", true);
                flag = false;
            }
            else if (!txBox.All(char.IsNumber))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert14", "alert('Please put a number for your quantity!');", true);
                flag = false;
            }
            return flag;
        } // checkGridview


        protected void btnOrder_Click(object sender, EventArgs e)
        {

            Boolean flag2 = false;

            for (int i = 0; i < gvBookstore.Rows.Count; i++)
            {
                BookLibrary.BooksDisplay books = new BookLibrary.BooksDisplay();
                chkBox = (CheckBox)gvBookstore.Rows[i].FindControl("cbSelectBook");
                ddl = (DropDownList)gvBookstore.Rows[i].FindControl("ddBookType");
                ddl2 = (DropDownList)gvBookstore.Rows[i].FindControl("ddRB");
                txtBox = (TextBox)gvBookstore.Rows[i].FindControl("txtQuantity");

                Boolean flag = validate(tbName.Text, tbStudentID.Text, tbAddress.Text, tbPhone.Text, tbCampus.Text,
                    ddl.Text, ddl2.Text, txtBox.Text);

                if (flag == true)
                {
                    // if the box is checked. Add the values of the row into the book object
                    if (chkBox.Checked)
                    {
                        if (flag == true)
                        {
                            flag2 = true;
                            // set the properties of book object with values
                            books.Title = gvBookstore.Rows[i].Cells[2].Text;
                            books.ISBN = gvBookstore.Rows[i].Cells[1].Text;
                            books.BookType = ddl.Text;
                            books.RentBuy = ddl2.Text;
                            books.Quantity = txtBox.Text;

                            // check gridview input
                            Boolean flagGridview = checkGridview(books.BookType, books.RentBuy, books.Quantity);

                            if (flagGridview == true)
                            {
                                // get the price of the book selected
                                DataSet getBooksFromDB = DBConnect.GetDataSet("SELECT * FROM Books WHERE ISBN = " + books.ISBN);
                                foreach (DataRow info in getBooksFromDB.Tables[0].Rows)
                                {
                                    // set the price of the book to the object price values
                                    books.Price = float.Parse(info["BasePrice"].ToString());

                                    int convIntQuantity = int.Parse(books.Quantity.ToString());

                                    BookLibrary.Books b = new BookLibrary.Books();

                                    // update the rent and sold quantity
                                    if (books.RentBuy == "Rent")
                                    {
                                        b.updatingRentAndTotalSales(books.RentBuy, books.ISBN, books.Price, int.Parse(txtBox.Text));
                                    }
                                    else if (books.RentBuy == "Buy")
                                    {
                                        b.updatingSoldAndTotalSales(books.RentBuy, books.ISBN, books.Price, int.Parse(txtBox.Text));
                                    }

                                    // call a calculator method from the book_controller class from booklibrary and set it to the total cost value
                                    books.TotalCost = b.CalculateTotal(books.Price, txtBox.Text);

                                    // add the book into the array and bind it to the gridview
                                    bookArray.Add(books);
                                    gvOutputGrid.DataSource = bookArray;
                                    gvOutputGrid.DataBind();

                                    // make the input invisible and show the output result
                                    lblStudentID.Visible = false;
                                    invisible();

                                    // make the input grid invisible
                                    gvBookstore.Visible = false;

                                    // make the output grid visible
                                    gvOutputGrid.Visible = true;

                                    // make the show report button visible 
                                    btnShowReport.Visible = true;

                                    // print the users input of their personal information
                                    printName.Text = tbName.Text; ;
                                    printStudentID.Text = tbStudentID.Text;
                                    PrintCampus.Text = tbCampus.Text;
                                    printAddress.Text = tbAddress.Text;
                                    printPhoneNumber.Text = tbPhone.Text;

                                    //make the order button visible if the order go through
                                    btnOrder.Visible = false;
                                    orderAgain.Visible = true;
                                }
                            }
                        }
                    }
                }
            }// end for loops 

            // add the footer
            int count = 0;
            int quantityTotal = 0;
            double salesOverAllTotal = 0;

            // calculate the total on the output gridview
            for (int i = 0; i < gvOutputGrid.Rows.Count; i++)
            {
                quantityTotal = quantityTotal + int.Parse(gvOutputGrid.Rows[i].Cells[quantityColumn].Text);
                salesOverAllTotal = salesOverAllTotal + double.Parse(gvOutputGrid.Rows[i].Cells[totalColumn].Text, NumberStyles.Currency);
            }

            //put the totals footer into the gridview
            gvOutputGrid.Columns[firstColumn].FooterText = "Totals:";
            gvOutputGrid.Columns[quantityColumn].FooterText = quantityTotal.ToString();
            gvOutputGrid.Columns[totalColumn].FooterText = salesOverAllTotal.ToString("C2");

            // bind the footer into the outputGrid
            gvOutputGrid.DataBind();

            // if the book are not selected
            if (flag2 == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('You need to select a book and then fill in your information of that book!');", true);
            }

        }

        protected void gvBookReport_Click(object sender, EventArgs e)
        {
            // disable the store grid and hide it
            gvBookstore.Enabled = false;
            gvBookstore.Visible = false;

            // enable the managment report grid and show it
            gvReportGrid.Enabled = true;
            gvReportGrid.Visible = true;


            DBConnect dbConnect = new DBConnect();
            String StringSQL = "SELECT * FROM Books";

            // set the management report grid data source to the data taking from the database and bind it
            gvReportGrid.DataSource = dbConnect.GetDataSet(StringSQL);
            gvReportGrid.DataBind();
        }

        protected void btnTotalSalesSort_Click(object sender, EventArgs e)
        {
            // disable the store grid and hide it
            gvBookstore.Enabled = false;
            gvBookstore.Visible = false;

            // end able the management report grid and show it
            gvReportGrid.Enabled = true;
            gvReportGrid.Visible = true;

            DBConnect dbConnect = new DBConnect();
            String strSQL = "SELECT * FROM Books " + "ORDER BY TotalSales DESC";

            // set the management report grid data source to the data taking from the database and bind it
            gvReportGrid.DataSource = dbConnect.GetDataSet(strSQL);
            gvReportGrid.DataBind();
        }

        protected void btnQuantityRentSort_Click(object sender, EventArgs e)
        {
            // disable the store grid and hide it
            gvBookstore.Enabled = false;
            gvBookstore.Visible = false;

            // end able the management report grid and show it
            gvReportGrid.Enabled = true;
            gvReportGrid.Visible = true;

            DBConnect dbConnect = new DBConnect();
            String strSQL = "SELECT * FROM Books " + "ORDER BY TotalQuantityRented DESC";

            // set the management report grid data source to the data taking from the database and bind it
            gvReportGrid.DataSource = dbConnect.GetDataSet(strSQL);
            gvReportGrid.DataBind();
        }

        protected void btnQuantitySoldSort_Click(object sender, EventArgs e)
        {
            // disable the store grid and hide it
            gvBookstore.Enabled = false;
            gvBookstore.Visible = false;

            // end able the management report grid and show it
            gvReportGrid.Enabled = true;
            gvReportGrid.Visible = true;

            DBConnect dbConnect = new DBConnect();
            String strSQL = "SELECT * FROM Books " + "ORDER BY TotalQuantitySold DESC";

            // set the management report grid data source to the data taking from the database and bind it
            gvReportGrid.DataSource = dbConnect.GetDataSet(strSQL);
            gvReportGrid.DataBind();
        }

        protected void btnOrderAgain_Click(object sender, EventArgs e)
        {
            // reset the page method
            resetOrder();
        }

        protected Boolean validate(string name, string id, string address, string phone, string campus, 
            string bookType, string rentOrBuy, string quantity)
        {
            Boolean flag = true;
            if (name == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert1", "alert('Please Enter A Name');", true);
                flag = false;
            }

            if (id == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert2", "alert('Please enter your student ID!');", true);
                flag = false;
            }

            if (address == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert3", "alert('Please enter your address!');", true);
                flag = false;
            }

            if (phone == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert4", "alert('Please enter your phone number!');", true);
                flag = false;
            }

            if (campus == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert5", "alert('Please pick a campus!');", true);
                flag = false;
            }
            else if (campus == "Select")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert6", "alert('Please Pick a Campus!');", true);
                flag = false;
            }
            return flag;
        }

        public void DisplayInputGrid(Boolean tf)
        {
            gvBookstore.Visible = tf;
            gvBookstore.Enabled = tf;
        }

        public void DisplayOutputGrid(Boolean tf)
        {
            gvOutputGrid.Visible = tf;
            gvOutputGrid.Enabled = tf;
        }

        public void DisplayManagementReport(Boolean tf)
        {
            gvReportGrid.Visible = tf;
            gvReportGrid.Enabled = tf;
        }

        // this method cotrol the visibility of all grids until the order is reset
        protected void invisible()
        {
            if (lblStudentID.Visible == true)
            {
                printName.Visible = false;
                printlblName.Visible = false;
                printAddress.Visible = false;
                printlblAddress.Visible = false;
                printStudentID.Visible = false;
                printlblStudentID.Visible = false;
                printPhoneNumber.Visible = false;
                printlblPhoneNumber.Visible = false;
                PrintCampus.Visible = false;
                printlblCampus.Visible = false;
            }
            else
            {
                printName.Visible = true;
                printlblName.Visible = true;
                printAddress.Visible = true;
                printlblAddress.Visible = true;
                printStudentID.Visible = true;
                printlblStudentID.Visible = true;
                printPhoneNumber.Visible = true;
                printlblPhoneNumber.Visible = true;
                PrintCampus.Visible = true;
                printlblCampus.Visible = true;

                lblStudentID.Visible = false;
                lblName.Visible = false;
                lblPhone.Visible = false;
                lblCampus.Visible = false;
                lblAddress.Visible = false;
                tbName.Visible = false;
                tbAddress.Visible = false;
                tbPhone.Visible = false;
                tbStudentID.Visible = false;
                tbCampus.Visible = false;

                btnTotalSalesSort.Visible = true;
                btnQuantityRentSort.Visible = true;
                btnQuantitySoldSort.Visible = true;
            }
        } // end invisible

        protected void resetOrder()
        {
            gvReportGrid.Visible = false;
            gvBookstore.Visible = true;
            gvBookstore.Enabled = true;
            btnOrder.Visible = true;
            tbName.Text = "";
            tbAddress.Text = "";
            tbPhone.Text = "";
            tbStudentID.Text = "";
            tbCampus.Text = "Select";


            orderAgain.Visible = false;
            pageLoadProduct();

            printName.Visible = false;
            printlblName.Visible = false;
            printAddress.Visible = false;
            printlblAddress.Visible = false;
            printStudentID.Visible = false;
            printlblStudentID.Visible = false;
            printPhoneNumber.Visible = false;
            printlblPhoneNumber.Visible = false;
            PrintCampus.Visible = false;
            printlblCampus.Visible = false;

            lblName.Visible = true;
            lblAddress.Visible = true;
            lblPhone.Visible = true;
            lblCampus.Visible = true;
            lblStudentID.Visible = true;
            tbName.Visible = true;
            tbAddress.Visible = true;
            tbCampus.Visible = true;
            tbPhone.Visible = true;
            tbStudentID.Visible = true;

            btnShowReport.Visible = false;
            btnTotalSalesSort.Visible = false;
            btnQuantityRentSort.Visible = false;
            btnQuantitySoldSort.Visible = false;

            gvOutputGrid.Visible = false;
        } // end resetOrder

        protected void pageLoadProduct()
        {
            DBConnect dbConnect = new DBConnect();
            String StringSQL = "SELECT * FROM Books";

            gvBookstore.DataSource = dbConnect.GetDataSet(StringSQL);
            gvBookstore.DataBind();
        } // end pageLoadProduct
    }
}