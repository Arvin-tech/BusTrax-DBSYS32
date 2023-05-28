using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BusTrax
{
    public partial class _Default : Page
    {
        public static MongoClient client = new MongoClient("mongodb://localhost:27017");
        public static IMongoDatabase database = client.GetDatabase("bustrax");
        public static IMongoCollection<BusCompanies> buscompaniesCollection = database.GetCollection<BusCompanies>("buscompanies");
        public static IMongoCollection<BusInformation> businfoCollection = database.GetCollection<BusInformation>("businfo");

        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None; //for field validators

            if (!IsPostBack)
            {
                BindGridView(); //load
                BindGridView2();
            }
        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {
            // Generate a random company ID
            Random random = new Random();

            //get the value inputted @ txtboxes   
            string companyId = random.Next(1000, 9999).ToString();
            string busNum = txtBusNumber.Text.Trim();

            // Check if the bus company already exists
            var filter = Builders<BusCompanies>.Filter.Eq("BusComp_ID", companyId);
            var existingCompany = buscompaniesCollection.Find(filter).FirstOrDefault();
            if (existingCompany != null)
            {
                //already exist
                Response.Write("<script>alert('Bus company already exists!');</script>");
                ClearTextBoxes();
                return;
            }

            //check if bus number already exists in bus routes collection
            var businfoFilter = Builders<BusInformation>.Filter.Eq("Bus_ID", busNum);
            var existingBusNum = businfoCollection.Find(businfoFilter).FirstOrDefault();
            if(existingBusNum != null)
            {
                //already exist
                Response.Write("<script>alert('Bus already existed!'); </script>");
                ClearTextBoxes();
                return;
            }


            //insert value from txtbox to mongo collections buscompanies and businfo
            BusCompanies buscomp = new BusCompanies(companyId, txtCompanyName.Text, txtContactNo.Text, txtEmail.Text);
            buscompaniesCollection.InsertOne(buscomp);
            BusInformation businfo = new BusInformation(txtBusNumber.Text, companyId, txtBusDriver.Text, txtBusConductor.Text, txtBusRoute.Text, txtPlateNumber.Text);
            businfoCollection.InsertOne(businfo);

            Response.Write("<script>alert('Record successfully saved!');</script>");
            ClearTextBoxes(); //clear inputs
            BindGridView(); //display data from bus companies collection
            BindGridView2(); //display data from bus info collection
        }

        //display data from mongo to grid view
        private void BindGridView()
        {
            //read data from bus companies collection
            List<BusCompanies> list = buscompaniesCollection.AsQueryable().ToList();
            GridView1.DataSource = list;
            GridView1.DataBind(); 
 
        }

        private void BindGridView2()
        {
            //read data from bus information collection
            List<BusInformation> list = businfoCollection.AsQueryable().ToList();
            GridView2.DataSource = list;
            GridView2.DataBind();

        }

        //for grid view headers
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell companyIdCell = e.Row.Cells[0];
                companyIdCell.Style["color"] = "red";
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Company ID";
                e.Row.Cells[1].Text = "Company Name";
                e.Row.Cells[2].Text = "Contact No";
                e.Row.Cells[3].Text = "Email";                  
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell busIdCell = e.Row.Cells[0];
                busIdCell.Style["color"] = "red";
               
                TableCell companyIdCell = e.Row.Cells[1];
                companyIdCell.Style["color"] = "red";
                
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Bus ID";
                e.Row.Cells[1].Text = "Company ID";
                e.Row.Cells[2].Text = "Driver";
                e.Row.Cells[3].Text = "Conductor";
                e.Row.Cells[4].Text = "Route";
                e.Row.Cells[5].Text = "Plate Number";
            }
        }

        //clear text box inputs
        private void ClearTextBoxes()
        {
            //company
            txtCompanyName.Text = string.Empty;
            txtContactNo.Text = string.Empty;
            txtEmail.Text = string.Empty;
            //bus info txtboxes
            txtBusNumber.Text = string.Empty;
            txtBusRoute.Text = string.Empty;
            txtBusDriver.Text = string.Empty;
            txtBusConductor.Text = string.Empty;
            txtPlateNumber.Text = string.Empty;

        }

    }
}