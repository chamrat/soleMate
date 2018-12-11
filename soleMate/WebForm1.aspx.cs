using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;



namespace soleMate
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        string mainConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(); //object that allows connection

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                countryDropDown();
                shoeDropDown();
            }


        }

        //Countries retreived from the DBO
        protected void countryDropDown()
        {

            try
            {
                SqlConnection sqlConn = new SqlConnection(mainConn);
                sqlConn.Open();
                SqlCommand sqlcomm = new SqlCommand("select * from [dbo].[countries]", sqlConn);
                sqlcomm.CommandType = CommandType.Text;
                CountryDD.DataSource = sqlcomm.ExecuteReader();
                CountryDD.DataTextField = "country";
                CountryDD.DataValueField = "countryID";
                CountryDD.DataBind();
                CountryDD.Items.Insert(0, new ListItem("--Select Your Country--", "0"));
                sqlConn.Close();
            }
            catch (Exception)
            {

                errorText.Text = "Error!";
            }


        }

        //States retreived per the country selected
        protected void CountryDD_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                int countryID = Convert.ToInt32(CountryDD.SelectedValue);
                SqlConnection sqlconn = new SqlConnection(mainConn);
                sqlconn.Open();
                SqlCommand sqlcomm = new SqlCommand("select * from [dbo].[states] where countryID =" + countryID, sqlconn);
                sqlcomm.CommandType = CommandType.Text;
                stateDD.DataSource = sqlcomm.ExecuteReader();
                stateDD.DataTextField = "states";
                stateDD.DataValueField = "stateID";
                stateDD.DataBind();
                stateDD.Items.Insert(0, new ListItem("--Select Your State--", "0"));
            }
            catch (Exception)
            {
                errorText.Text = "Error!";
            }


        }

        //shoe size retreived from the DBO
        protected void shoeDropDown()
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(mainConn);
                sqlConn.Open();
                SqlCommand sqlcomm = new SqlCommand("select * from [dbo].[sizes]", sqlConn);
                sqlcomm.CommandType = CommandType.Text;
                sizeID.DataSource = sqlcomm.ExecuteReader();
                sizeID.DataTextField = "size";
                sizeID.DataValueField = "sizeID";
                sizeID.DataBind();
                sizeID.Items.Insert(0, new ListItem("--Select Shoe Size--", "0"));
                sqlConn.Close();

            }
            catch (Exception)
            {

                errorText.Text = "Error!";
            }

        }

        //Checking Data and Inserting if it's not a duplicate
        protected void Click_Submit(object sender, EventArgs e)
        {

            //check the DBO for duplicate entries before storing
            string connString = "Server=den1.mssql7.gear.host;" + "Initial Catalog=cis4160dbchamal;"
            + "User ID=cis4160dbchamal;" + "Password=Mk45!7z9?5vR;" + "Integrated Security=false";
            conn.ConnectionString = connString;
            conn.Open();
            //declaring the parameter to check the database for duplicates
            SqlParameter FNcheck = new SqlParameter("@firstName", SqlDbType.NVarChar);
            SqlParameter LNcheck = new SqlParameter("@lastName", SqlDbType.NVarChar);
            SqlParameter EMcheck = new SqlParameter("@email", SqlDbType.NVarChar);
            try
            {
                //attempt to have the check here and call it on click_submit
                SqlCommand checkUsr = new SqlCommand("SELECT COUNT(*) FROM [dbo].[applicants] " +
                    "WHERE firstName LIKE @firstName AND lastName LIKE @lastName AND email LIKE @email", conn);
                checkUsr.Parameters.AddWithValue("@firstName", firstName.Text);
                checkUsr.Parameters.AddWithValue("@lastName", lastName.Text);
                checkUsr.Parameters.AddWithValue("@email", email.Text);
                int firstExist = (int)checkUsr.ExecuteScalar();
                int lastExist = (int)checkUsr.ExecuteScalar();
                int emailExist = (int)checkUsr.ExecuteScalar();

                //if the applicant is already in the system update the changed country and state and output that it's already in the system
                if (firstExist > 0 && lastExist > 0 && emailExist > 0)
                {
                    //update applicants shoe information and country and state/province information if they are already in the system
                    conn = new SqlConnection(connString);
                    SqlCommand updateRec = new SqlCommand("[dbo].[updateApplicants2]", conn);
                    updateRec.CommandType = CommandType.StoredProcedure;
                    updateRec.Parameters.AddWithValue("firstName", firstName.Text);
                    updateRec.Parameters.AddWithValue("lastName", lastName.Text);
                    updateRec.Parameters.AddWithValue("email", email.Text);
                    updateRec.Parameters.AddWithValue("sizeID", sizeID.Text);
                    updateRec.Parameters.AddWithValue("stateID", stateDD.Text);
                    updateRec.Parameters.AddWithValue("countryID", CountryDD.Text);
                    updateRec.Parameters.AddWithValue("leg", legCheck.Text);
                    conn.Open();
                    int prompt = updateRec.ExecuteNonQuery();
                    conn.Close();

                    //show the contacts for matching amputees
                    conn = new SqlConnection(connString);
                    SqlCommand findMatch = new SqlCommand("[dbo].[findMatch]", conn);
                    findMatch.CommandType = CommandType.StoredProcedure;
                    findMatch.Parameters.AddWithValue("firstName", firstName.Text);
                    findMatch.Parameters.AddWithValue("lastName", lastName.Text);
                    findMatch.Parameters.AddWithValue("email", email.Text);
                    findMatch.Parameters.AddWithValue("sizeID", sizeID.Text);
                    findMatch.Parameters.AddWithValue("stateID", stateDD.Text);
                    findMatch.Parameters.AddWithValue("countryID", CountryDD.Text);
                    findMatch.Parameters.AddWithValue("leg", legCheck.Text);
                    conn.Open();

                    //print matching amputees for the opposite leg
                    string title = "";
                    string strDBResult = " ";
                    SqlDataReader reader = findMatch.ExecuteReader();
                    title += "<p> Matching Amputees</p>";
                    strDBResult += "<ul>";

                    while (reader.Read())
                    {
                        strDBResult += "</p>";
                        strDBResult += "<p>Name: ";
                        strDBResult += reader[1].ToString();
                        strDBResult += " ";
                        strDBResult += reader[2].ToString();
                        strDBResult += "<p>Email: ";
                        strDBResult += reader[3].ToString();
                        strDBResult += " </p>";
                        strDBResult += "===================";
                    }

                    strDBResult += "</ul>";
                    divinPanel.InnerHtml = strDBResult;
                    conn.Close();
                    //show a prompt confirming the update of the record
                    if (prompt != 0)

                    {
                        showMsg.Text = "Record Updated Succesfully";
                        showMsg.ForeColor = System.Drawing.Color.CornflowerBlue;
                        
                    }

                }

                //if the applicant is new insert the values into the database
                else
                {
                    conn = new SqlConnection(connString);
                    SqlCommand insertRec = new SqlCommand("[dbo].[insertApplicants]", conn);
                    insertRec.CommandType = CommandType.StoredProcedure;
                    insertRec.Parameters.AddWithValue("firstName", firstName.Text);
                    insertRec.Parameters.AddWithValue("lastName", lastName.Text);
                    insertRec.Parameters.AddWithValue("email", email.Text);
                    insertRec.Parameters.AddWithValue("sizeID", sizeID.Text);
                    insertRec.Parameters.AddWithValue("stateID", stateDD.Text);
                    insertRec.Parameters.AddWithValue("countryID", CountryDD.Text);
                    insertRec.Parameters.AddWithValue("leg", legCheck.Text);
                    conn.Open();
                    //show a prompt confirming the entry to the applicant
                    int prompt = insertRec.ExecuteNonQuery();
                    if (prompt != 0)

                    {
                        showMsg.Text = "Record Inserted Succesfully into the Database";
                        showMsg.ForeColor = System.Drawing.Color.CornflowerBlue;
                        conn.Close();
                    }

                    //show the contacts for matching amputees
                    conn = new SqlConnection(connString);
                    SqlCommand findMatch = new SqlCommand("[dbo].[findMatch]", conn);
                    findMatch.CommandType = CommandType.StoredProcedure;
                    findMatch.Parameters.AddWithValue("firstName", firstName.Text);
                    findMatch.Parameters.AddWithValue("lastName", lastName.Text);
                    findMatch.Parameters.AddWithValue("email", email.Text);
                    findMatch.Parameters.AddWithValue("sizeID", sizeID.Text);
                    findMatch.Parameters.AddWithValue("stateID", stateDD.Text);
                    findMatch.Parameters.AddWithValue("countryID", CountryDD.Text);
                    findMatch.Parameters.AddWithValue("leg", legCheck.Text);
                    conn.Open();

                    //print matching amputees for the opposite leg
                    string strDBResult = " ";
                    SqlDataReader reader = findMatch.ExecuteReader();
                                        
                    while (reader.Read())
                    {
                        strDBResult += "</p>";
                        strDBResult += "<p>Name: ";
                        strDBResult += reader[1].ToString();
                        strDBResult += " ";
                        strDBResult += reader[2].ToString();                                              
                        strDBResult += "<p>Email: ";                        
                        strDBResult += reader[3].ToString();
                        strDBResult += " </p>";
                        strDBResult += "===================";

                    }
                    
                    divinPanel.InnerHtml = strDBResult;
                    conn.Close();


                }

            }

            catch (Exception)
            {

                errorText.Text = "Error!";
            }
                                                         
        }

    }

}