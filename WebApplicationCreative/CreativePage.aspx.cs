using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationCreative
{
    public partial class CreativePage : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CreateDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
                LoadCategories();
            }
        }

        private void BindGridView(string searchTerm = null)
        {
           

            string query;
            if (string.IsNullOrEmpty(searchTerm))
            {
                query = @"
            SELECT 
                p.ProductId, 
                p.ProductName, 
                p.ProductDescription, 
                p.UserName, 
                p.CategoryId, 
                c.CategoryName 
            FROM  
                Product p  
            INNER JOIN 
                Category c  
            ON 
                p.CategoryId = c.CategoryId";
            }
            else
            {
                query = @"
            SELECT 
                p.ProductId, 
                p.ProductName, 
                p.ProductDescription, 
                p.UserName, 
                p.CategoryId, 
                c.CategoryName 
            FROM  
                Product p  
            INNER JOIN 
                Category c  
            ON 
                p.CategoryId = c.CategoryId 
            WHERE 
                p.ProductId = @ProductId";
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", searchTerm);
                    }
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }

        }
        private void LoadCategories()
        {
            string cs = ConfigurationManager.ConnectionStrings["CreateDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("GetCategories", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DropDownList1Category.DataSource = reader;
                    DropDownList1Category.DataTextField = "CategoryName"; // Text to display in the DropDownList
                    DropDownList1Category.DataValueField = "CategoryId";  // Value associated with the text
                    DropDownList1Category.DataBind();
                }
            }

            // Add a default item at the beginning
            DropDownList1Category.Items.Insert(0, new ListItem("Select Category", "0"));
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertPro", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@ProductId", txtProductId.Text);
                    cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@ProductDescription", txtProductDescription.Text);
                    cmd.Parameters.AddWithValue("@CategoryName", DropDownList1Category.SelectedValue);
                    cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@CategoryId",DropDownList1Category.SelectedValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            IblMsg.Text = "Product added successfully!";
            BindGridView();

            txtProductId.Text = "";
            txtProductName.Text = "";
            txtProductDescription.Text = "";
            DropDownList1Category.SelectedIndex = -1; // Reset DropDownList
            txtUserName.Text = "";

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
             GridView1.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];

          
            int productId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            // Get the updated values from the controls
            string productName = (row.FindControl("txtProductName") as TextBox).Text;
            string productDescription = (row.FindControl("txtProductDescription") as TextBox).Text;
            string userName = (row.FindControl("txtUserName") as TextBox).Text;
            //DropDownList ddlCategoryName = row.FindControl("ddlCategoryName") as DropDownList;
            //string categoryName = ddlCategoryName.SelectedItem.Text; // Use the selected text
            DropDownList ddlCategoryName = row.FindControl("ddlCategoryName") as DropDownList;
            string categoryId = ddlCategoryName.SelectedValue;
            // Update the database
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateProductList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@ProductDescription", productDescription);
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    //cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                    // Open the connection, execute the command, then close the connection
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            // Exit edit mode and rebind the GridView to reflect the changes
            GridView1.EditIndex = -1;
            BindGridView();

        }


        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex= -1;
            BindGridView();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteProduct", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            BindGridView();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlCategoryName = (DropDownList)e.Row.FindControl("ddlCategoryName");
                if (ddlCategoryName != null)
                {
                    // Populate the dropdown list
                    ddlCategoryName.Items.Clear();
                    ddlCategoryName.Items.Add(new ListItem("Select Item", ""));
                    ddlCategoryName.Items.Add(new ListItem("Electronics", "1"));
                    ddlCategoryName.Items.Add(new ListItem("Mobile", "2"));
                    ddlCategoryName.Items.Add(new ListItem("Cloth", "3"));

                    // Check the actual type of the data item and cast accordingly
                    if (e.Row.DataItem is DataRowView drv)
                    {
                        ddlCategoryName.SelectedValue = drv["CategoryId"].ToString(); // Ensure you're using the correct field
                    }
                    else if (e.Row.DataItem is IDataRecord record)
                    {
                        // Debugging: Print available columns
                        for (int i = 0; i < record.FieldCount; i++)
                        {
                            System.Diagnostics.Debug.WriteLine("Column {0}: {1}", i, record.GetName(i));
                        }

                        // Use the correct column name
                        string correctColumnName = "CategoryId"; // Ensure you're using the correct field
                        ddlCategoryName.SelectedValue = record[correctColumnName].ToString();
                    }
                    else
                    {
                        throw new InvalidCastException($"Unsupported type: {e.Row.DataItem.GetType().FullName}");
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            BindGridView(searchTerm);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text= string.Empty;
            // BindGridView(txtSearch.Text);
            BindGridView();
        }
    }
    
    
}