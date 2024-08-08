<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreativePage.aspx.cs" Inherits="WebApplicationCreative.CreativePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 260px;
        }
        .search-container {
    display: flex;
    align-items: center;
}
 .search-textbox {
    flex-grow: 0; /* This makes the textbox take the remaining space */
}

.search-button {
    margin-left: 10px; /* Adds some space between the textbox and the button */
    background-color: #ff0066;
    border: none;
    color: white;
    padding: 8px 16px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
    cursor: pointer;
}
.search-button1{
    margin-right:10px;
    background-color:blue;
    color:white;
    padding:8px 16px;
    text-align:center;
    display:inline-block;
    font-size:16px;
    cursor:pointer;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">ProductId</td>
                    <td>
                        <asp:TextBox ID="txtProductId" runat="server" Width="229px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">ProductName</td>
                    <td>
                        <asp:TextBox ID="txtProductName" runat="server" Width="238px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">ProductDescription</td>
                    <td>
                        <asp:TextBox ID="txtProductDescription" runat="server" Width="232px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">CategoryName</td>
                    <td>
                        <asp:DropDownList ID="DropDownList1Category" runat="server">
                             <asp:ListItem Value="" Text="Select Item"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Electronics"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Mobile"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Cloth"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">UserName</td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" Width="229px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Label runat="server" ID="IblMsg" ForeColor="Green"></asp:Label>

        
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="169px" OnClick="btnSubmit_Click" />
    </div>
        <br /><br />
        <div class="search-container" >
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter ProductId" CssClass="search-textbox"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" BackColor="#ff0066" CssClass="search-button" Text="Search" OnClick="btnSearch_Click" />
            <asp:Button ID="btnReset" runat="server" BackColor="#3366ff" CssClass="search-button" Text="Reset" OnClick="btnReset_Click" />
        </div>
 <div>


           <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" OnRowEditing="GridView1_RowEditing"
                OnRowUpdating="GridView1_RowUpdating" 
                OnRowCancelingEdit="GridView1_RowCancelingEdit" 
                OnRowDeleting="GridView1_RowDeleting" OnRowDataBound="GridView1_RowDataBound">
               <Columns>
                <asp:BoundField DataField="ProductId" HeaderText="Product ID" ReadOnly="True" />
                
                <asp:TemplateField HeaderText="Product Name">
                    <ItemTemplate>
                        <%# Eval("ProductName") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtProductName" runat="server" Text='<%# Eval("ProductName") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Product Description">
                    <ItemTemplate>
                        <%# Eval("ProductDescription") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtProductDescription" runat="server" Text='<%# Eval("ProductDescription") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Category Name">
                    <ItemTemplate>
                        <%# Eval("CategoryName") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlCategoryName" runat="server">
                             <asp:ListItem Value="" Text="Select Item"></asp:ListItem>
                      <asp:ListItem Value="1" Text="Electronics"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Mobile"></asp:ListItem>
                         <asp:ListItem Value="3" Text="Cloth"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="User Name">
                    <ItemTemplate>
                        <%# Eval("UserName") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtUserName" runat="server" Text='<%# Eval("UserName") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
            </asp:GridView>

        </div>

    </form>
</body>
</html>
