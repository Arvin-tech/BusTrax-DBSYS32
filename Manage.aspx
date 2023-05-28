<%@ Page Title="Manage" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="BusTrax.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     
    <main aria-labelledby="title">
         
        <h2 id="title">Manage Bus Companies</h2>

         <section class="row">

             <div class="col-md-15">
               <div class="form-group">
                   <label for="GridView1"><b>Information</b></label>
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped" AutoGenerateColumns="false" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDataBound="GridView1_RowDataBound" DataKeyNames="BusComp_ID">                            
                            <Columns>
                                <asp:BoundField HeaderText="Company ID" DataField="BusComp_ID" ReadOnly="true" />
                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <%# Eval("BusComp_Name") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtUpdatedCompanyName" runat="server" Text='<%# Bind("BusComp_Name") %>' CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ControlToValidate="txtUpdatedCompanyName" ErrorMessage="Company Name is required!" CssClass="text-danger"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact No">
                                    <ItemTemplate>
                                        <%# Eval("BusComp_ContactNo") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtUpdatedContactNum" runat="server" Text='<%# Bind("BusComp_ContactNo") %>' CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvContactNo" runat="server" ControlToValidate="txtUpdatedContactNum" ErrorMessage="Contact No is required!" CssClass="text-danger"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email">
                                    <ItemTemplate>
                                        <%# Eval("Email") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtUpdatedEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtUpdatedEmail" ErrorMessage="Email is required!" CssClass="text-danger"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regexEmail" runat="server" ControlToValidate="txtUpdatedEmail" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" ErrorMessage="Invalid Email" CssClass="text-danger"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="true" />
                                <asp:CommandField ShowCancelButton="true" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete"
                                            CommandArgument='<%# Eval("BusComp_ID") %>' CssClass="btn btn-danger"
                                            OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                        
                     </asp:GridView>  

               </div>
           </div>

             <div class="col-md-2">
                <div class="form-group">
                    <label for="txtSearchCompanyId">Search by Company ID</label>
                    <asp:TextBox ID="txtSearchCompanyId" runat="server" CssClass="form-control" AutoCompleteType="Disabled" AutoPostBack="true" OnTextChanged="txtSearchCompanyId_TextChanged"></asp:TextBox>
                    
                </div>
                <div class="form-group">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                </div>
             </div>

            <div class="col-md-9">
                <div class="form-group"> 
                    <div class="col-md-10" style="height: 20px;"></div>
                    <asp:GridView ID="GridView2" runat="server" CssClass="table table-striped" AutoGenerateColumns="false">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoResults" runat="server" Text="No bus company found" CssClass="no-results-message" ForeColor="Red"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField HeaderText="Company ID" DataField="BusComp_ID" ReadOnly="true" />
                            <asp:BoundField HeaderText="Company Name" DataField="BusComp_Name" />
                            <asp:BoundField HeaderText="Contact No" DataField="BusComp_ContactNo" />
                            <asp:BoundField HeaderText="Email" DataField="Email" />
                        </Columns>

                    </asp:GridView>
                   
                </div>
            </div>

         </section>                       
    </main>
</asp:Content>