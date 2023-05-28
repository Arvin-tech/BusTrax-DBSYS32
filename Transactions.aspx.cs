using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amazon.Runtime.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using iTextSharp.text.pdf;
using iTextSharp.text;




namespace BusTrax
{
    public partial class Contact : Page
    {

        public static MongoClient client = new MongoClient("mongodb://localhost:27017");
        public static IMongoDatabase database = client.GetDatabase("bustrax");
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView(); //load
                BindGridView2();
            }
        }
        //subscriptions grid view
        private void BindGridView()
        {
            IMongoCollection<Subscriptions> collection = database.GetCollection<Subscriptions>("subscriptions");
            List<Subscriptions> list = collection.AsQueryable().ToList();
            GridView1.DataSource = list;
            GridView1.DataBind();

        }

        //bus routes grid view
        private void BindGridView2()
        {
            IMongoCollection<BusRoutes> collection = database.GetCollection<BusRoutes>("busroutes");
            List<BusRoutes> list = collection.AsQueryable().ToList();
            GridView2.DataSource = list;
            GridView2.DataBind();
        }
        
        //subscription headers
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {            
                TableCell companyIdCell = e.Row.Cells[0];
                companyIdCell.Style["color"] = "red";
                
                TableCell statusCell = e.Row.Cells[2];
                statusCell.Style["color"] = "green";
        
                if (statusCell.Text == "Inactive")
                {
                    statusCell.Style["color"] = "red";
                
                }
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Bus Company";
                e.Row.Cells[1].Text = "Date";
                e.Row.Cells[2].Text = "Status";
                e.Row.Cells[3].Text = "Type";
                e.Row.Cells[4].Text = "Payment";
            }
        }

        //bus route headers
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell busRouteIdCell = e.Row.Cells[0];
                busRouteIdCell.Style["color"] = "red";
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Route ID";
                e.Row.Cells[1].Text = "Bus Route";
                e.Row.Cells[2].Text = "Origin";
                e.Row.Cells[3].Text = "Destination";
                e.Row.Cells[4].Text = "Fare";
                e.Row.Cells[5].Text = "Estimated Time of Arrival";
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            // Create a StringBuilder to hold the CSV data
            StringBuilder sb = new StringBuilder();

            // Add column headers
            foreach (TableCell headerCell in GridView2.HeaderRow.Cells)
            {
                sb.Append(headerCell.Text + ",");
            }
            sb.Append("\r\n");

            // Add data rows
            foreach (GridViewRow row in GridView2.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    sb.Append(cell.Text + ",");
                }
                sb.Append("\r\n");
            }

            // Set response headers to force download the file
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=BusRoutes.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            // Write the CSV data to the response
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected void btnSaveAsPdf_Click(object sender, EventArgs e)
        {
            string fileName = "BusRoutes.pdf";
            string folderPath = Server.MapPath("~/Downloads/");
            string filePath = Path.Combine(folderPath, fileName);

            // Create the "Downloads" folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            GeneratePdf(filePath);

            // Download the PDF file
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.WriteFile(filePath);
            Response.End();
        }

        protected void GeneratePdf(string filepath)
        {
            // Create a new PDF document
            iTextSharp.text.Document document = new iTextSharp.text.Document();

            try
            {
                // Create a PdfWriter instance to write the document to a file
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filepath, FileMode.Create)); 

                // Open the document for writing
                document.Open();

                // dynamically generate the columns based on the data in GridView2
                int columnCount = GridView2.HeaderRow.Cells.Count;

                // Create a new PdfPTable based on the GridView columns
                PdfPTable table = new PdfPTable(columnCount);
                table.WidthPercentage = 100; 

                // Add table headers
                for (int i = 0; i < columnCount; i++)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(GridView2.Columns[i].HeaderText));
                    table.AddCell(headerCell);
                }

                // Add table data from GridView rows
                for (int rowIndex = 0; rowIndex < GridView2.Rows.Count; rowIndex++)
                {
                    GridViewRow row = GridView2.Rows[rowIndex];

                    for (int cellIndex = 0; cellIndex < row.Cells.Count; cellIndex++)
                    {
                        string cellText = row.Cells[cellIndex].Text;
                        PdfPCell cell = new PdfPCell(new Phrase(cellText));
                        table.AddCell(cell);
                    }
                }

                // Add the table to the document
                document.Add(table);
            }
            finally
            {
                // Close the document
                document.Close();
            }
        }

        protected void SearchRoute()
        {
            //assign the inputted value to route variable
            string route = txtSearchRoute.Text.Trim();

            //establish connection first to mongo db
            var connStr = "mongodb://localhost:27017";
            var client = new MongoClient(connStr);
            var database = client.GetDatabase("bustrax");
            var collection = database.GetCollection<BusRoutes>("busroutes");

            // Check if the route is empty
            if (string.IsNullOrEmpty(route))
            {
                // Route is empty, bind the grid view with all bus routes
                BindGridView2();
                return;
            }

            // Filter by Route_Name (case-insensitive search)
            var filter = Builders<BusRoutes>.Filter.Regex("Route_Name", new BsonRegularExpression(route, "i")); 
            var searchResults = collection.Find(filter).ToList();
            if (searchResults.Count > 0)
            {
                //found
                GridView2.EmptyDataText = string.Empty;
                GridView2.DataSource = searchResults;
                GridView2.DataBind();
                GridView2.Visible = true;
            }
            else
            {
                GridView2.EmptyDataText = "No bus route found.";
                GridView2.DataSource = new List<BusRoutes>(); // Empty collection
                GridView2.DataBind();
            }
        }

        protected void txtSearchRoute_TextChanged(object sender, EventArgs e)
        {
            string route = txtSearchRoute.Text.Trim();

            SearchRoute(); //auto-search

            // Check if the route is empty
            if (string.IsNullOrEmpty(route))
            {
                BindGridView2(); // Re-display all bus routes
            }
        }


    }
}