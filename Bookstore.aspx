<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bookstore.aspx.cs" Inherits="_3342_Project_2.Bookstore" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link rel="stylesheet" href="Bookstore.css" />

    <title>Eric's Book Store</title>
    
</head>
<body>
    <form id="form1" runat="server">
        <div id="textarea">
            <asp:Label ID="lblStudentID" runat="server" CssClass="label" ForeColor="#252525" Text="Student ID"></asp:Label>
            <asp:TextBox ID="tbStudentID" runat="server" CssClass="textbox"></asp:TextBox>
            <br />
            <asp:Label ID="lblName" runat="server" CssClass="label" ForeColor="#252525" Text="Name"></asp:Label>
            <asp:TextBox ID="tbName" runat="server" CssClass="textbox"></asp:TextBox>
            <br />
            <asp:Label ID="lblAddress" runat="server" CssClass="label" ForeColor="#252525" Text="Address"></asp:Label>
            <asp:TextBox ID="tbAddress" runat="server" CssClass="textbox"></asp:TextBox>
            <br />
            <asp:Label ID="lblPhone" runat="server" CssClass="label" ForeColor="#252525" Text="Phone Number"></asp:Label>
            <asp:TextBox ID="tbPhone" runat="server" CssClass="textbox"></asp:TextBox>
            <br />
            <asp:Label ID="lblCampus" runat="server" CssClass="label" ForeColor="#252525" Text="Campus"></asp:Label>
            <asp:TextBox ID="tbCampus" runat="server" CssClass="textbox"></asp:TextBox>
        </div>

           <asp:Button class="btn btn-primary" ID="btnOrder" runat="server" Text="Order Book" OnClick="btnOrder_Click" />
           <asp:Button class="btn btn-primary" ID="btnShowReport" runat="server" Text="Show Sales Report" OnClick="gvBookReport_Click" Enabled="True" Visible="False" />
           <asp:Button class="btn btn-primary" ID="btnTotalSalesSort" runat="server" Text="Sales Report by Total Sales" OnClick="btnTotalSalesSort_Click" Enabled="True" Visible="False" />
           <asp:Button class="btn btn-primary" ID="btnQuantityRentSort" runat="server" Text="Sales Report by Quantity Rented" OnClick="btnQuantityRentSort_Click" Enabled="True" Visible="False" />
           <asp:Button class="btn btn-primary" ID="btnQuantitySoldSort" runat="server" Text="Sales Report by Quantity Sold" OnClick="btnQuantitySoldSort_Click" Enabled="True" Visible="False" />
        <br />

        <!-- Store gridview -->
        <asp:GridView ID="gvBookstore" runat="server" AutoGenerateColumns="False" Height="167px" Width="1192px" CssClass="gridStyle" RowStyle-CssClass="rows" HeaderStyle-CssClass="gridHeader" CellPadding="10" CellSpacing="2" >
            <Columns>
                <asp:TemplateField HeaderText="Select Book">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelectBook" runat="server" CssClass="checkbox" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Title" HeaderText="Title" />
                <asp:BoundField DataField="Authors" HeaderText="Authors" />
                <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                <asp:TemplateField HeaderText="Book Type">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddBookType" runat="server" CssClass="ddList">
                            <asp:ListItem>Hardcover</asp:ListItem>
                            <asp:ListItem>Paper-Back</asp:ListItem>
                            <asp:ListItem>Audiobook</asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rent or Buy">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddRB" runat="server" CssClass="ddList">
                            <asp:ListItem>Rent</asp:ListItem>
                            <asp:ListItem>Buy</asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="textbox"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="gridHeader" />
            <RowStyle CssClass="rows" />
        </asp:GridView>

         <asp:GridView ID="gvReportGrid" runat="server" AutoGenerateColumns="False" class="gridView table table-bordered" Enabled="False" Visible="False">
                    <Columns>
                        <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="TotalSales" HeaderText="Total Sales" DataFormatString="{0:c}" />
                        <asp:BoundField DataField="TotalQuantityRented" HeaderText="Total Quantity Rented" />
                        <asp:BoundField DataField="TotalQuantitySold" HeaderText="Total Quantity Sold" />
                    </Columns>
                </asp:GridView>

                <br />
                   <div class="rightSideBoxFlex bg-dark p-3 pt-md-2 px-md-5 mr-md-3 col-md-5 text-center text-white overflow-hidden">
                <div class="my-3 py-3">
                    <h2 class="display-5">Your Purchases Information</h2>
                </div>

                <div class="row">
                    <div class="col-md-6 mb-3">
                       
                        <asp:Label ID="printlblName" runat="server" Text="Label">Name</asp:Label>
                        <br />
                        <asp:Label class="outputResult" ID="printName" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-6 mb-3">
                      
                  
                        <asp:Label ID="printlblStudentID" runat="server" Text="Label">Student ID</asp:Label>
                        <br />
                        <asp:Label class="outputResult" ID="printStudentID" runat="server" Text=""></asp:Label>
                    </div>
                </div>

                <div class="mb-3">
                    
                    <asp:Label ID="printlblAddress" runat="server" Text="Label">Address</asp:Label>
                    <br />
                    <asp:Label class="outputResult" ID="printAddress" runat="server" Text=""></asp:Label>
                </div>

                <div class="row">
                    <div class="col-md-8 mb-3">
                   
                        <asp:Label ID="printlblPhoneNumber" runat="server" Text="Label">Phone Number</asp:Label>
                        <br />
                        <asp:Label class="outputResult" ID="printPhoneNumber" runat="server"></asp:Label>
                    </div>
           
                    <div class="col-md-4 mb-3">
                      
                        <asp:Label ID="printlblCampus" runat="server" Text="Label">Campus</asp:Label>
                        <br />
                        <asp:Label class="outputResult" ID="PrintCampus" runat="server" Text=""></asp:Label>
                    </div>
                </div>

                <br />

               
                <asp:GridView ID="gvOutputGrid" runat="server" AutoGenerateColumns="False" class="gridView table table-bordered" Visible="False" ShowFooter="True">
                    <Columns>
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                        <asp:BoundField DataField="BookType" HeaderText="Type of Book" />
                        <asp:BoundField DataField="RentBuy" HeaderText="Buy/Rent" />
                        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:c}" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                        <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" DataFormatString="{0:c}" />
                    </Columns>
                </asp:GridView>
        
                <asp:Button class="btn btn-primary" ID="orderAgain" runat="server" Text="New Order" OnClick="btnOrderAgain_Click" Visible="False" />
            </div>
        
    </form>
</body>
</html>
