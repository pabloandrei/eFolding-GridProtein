<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Simulation.aspx.cs" Inherits="Pages_Simulation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script language="javascript" type="text/javascript">
        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);
            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "images/down-128.png";
            } else {
                div.style.display = "none";
                img.src = "images/up-128.png";
            }
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div class="progress">
                <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%">
                    <span class="sr-only">45% Complete</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container">
                <h2>Simulations
                </h2>

                <div class="panel panel-default">
                    <div class="panel-body">
                        <span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="span2"></asp:TextBox>
                        <asp:Button ID="bttSearch" runat="server" Text="Pesquisar"
                            class="btn" OnClick="bttSearch_Click" />

                        <asp:Button ID="bttAdd" runat="server" class="btn btn-success pull-right"
                            Text="New Simulation" OnClick="bttAdd_Click" />
                    </div>
                </div>

                <legend>
                    <asp:Label ID="lblLegend" runat="server" Text="Legend"></asp:Label></legend>

                <div class="panel panel-default">

                    <div class="panel-heading">Simulations</div>
                    <asp:GridView ID="GridViewProcess" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="GridViewProcess_RowDataBound"
                        CssClass="table table-striped table-bordered table-condensed"
                        EmptyDataText="No data found!"
                        AllowSorting="True"
                        ShowFooter="True"
                        AllowPaging="True"
                        OnPageIndexChanging="GridViewProcess_PageIndexChanging"
                        PageSize="10" EnableSortingAndPagingCallbacks="True" ShowHeaderWhenEmpty="True">
                        <PagerSettings Mode="Numeric" Visible="False" />

                        <Columns>
                            <asp:TemplateField ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <a style="border-color: black" href="JavaScript:divexpandcollapse('div<%# Eval("guid") %>');">
                                        <img id="imgdiv<%# Eval("guid") %>" width="12px" dir="rtl" src="images/up-128.png" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Label" HeaderStyle-ForeColor="Black" HeaderText="Label" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="guid" HeaderStyle-ForeColor="Black" HeaderText="Guid" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="date" HeaderText="Date" />
                            <asp:BoundField DataField="userId" HeaderText="User" />

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <tr>
                                        <td colspan="100%">
                                            <div id="div<%# Eval("guid") %>" style="display: none; position: relative; overflow: auto">
                                                <asp:GridView ID="gvChildGrid" OnRowDataBound="gvChildGrid_RowDataBound"
                                                    OnRowCommand="GridViewProcess_RowCommand"
                                                    CssClass="table table-striped table-bordered table-condensed"
                                                    EmptyDataText="Data Unavailable" runat="server"
                                                    AutoGenerateColumns="false" Width="100%"
                                                    ShowFooter="False">

                                                    <Columns>
                                                        <asp:BoundField DataField="guid" HeaderText="Guid of execution" />
                                                        <asp:BoundField DataField="DataToProcess.Temperature" HeaderText="Temperature" />
                                                        <asp:BoundField DataField="status_id" HeaderText="Status" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddListOption" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddListOption_SelectedIndexChanged">
                                                                    <asp:ListItem Selected="True" Value="-1">Change status</asp:ListItem>
                                                                    <asp:ListItem Value="1">Send to pipeline</asp:ListItem>
                                                                    <asp:ListItem Value="5">Decline</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbEdit" runat="server" CommandName="Edit" CommandArgument='<%#Eval("guid")%>'>Edit</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("guid")%>'>Delete</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>

                                                                <asp:HyperLink ID="lbDownload" runat="server" NavigateUrl='<%# Eval("guid","~/Pages/Handler.ashx?guid={0}") %>'>Download Results</asp:HyperLink>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                    <div class="panel-body">
                        <div class="row">


                            <nav>
                                <ul class="pagination">
                                    <li>
                                        <asp:LinkButton ID="bttFirst" runat="server" OnClick="bttFirst_Click">First</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="bttPrevious" runat="server" OnClick="bttPrevious_Click">Previous</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="bttNext" runat="server" OnClick="bttNext_Click">Next</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="bttLast" runat="server" OnClick="bttLast_Click">Last</asp:LinkButton>
                                    </li>
                                </ul>
                            </nav>
                        </div>

                        <div class="row">
                            <ul class="nav nav-pills" role="tablist">
                                <li role="presentation" class="active"><a href="#">Total: <span class="badge">
                                    <asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label></span></a></li>
                                <li role="presentation"><a href="#">Actual page <span class="badge">
                                    <asp:Label ID="lblActualPage" runat="server" Text="0"></asp:Label>/
                        <asp:Label ID="lblTotalPages" runat="server" Text="0"></asp:Label>
                                </span></a></li>
                            </ul>

                        </div>

                    </div>
                </div>

                <div class="panel panel-default">
                    <div class="panel-body">
                        <asp:Button ID="Button2" runat="server" class="btn btn-success pull-right"
                            Text="New Simulation" OnClick="bttAdd_Click" />
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


