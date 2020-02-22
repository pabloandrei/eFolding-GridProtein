using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GridProteinFolding.Entities.Internal;
using GridProteinFolding.Entities.Membership;
using System.Web.Security;

using System.Net;
using System.ComponentModel;
using System.Threading;

public partial class Pages_Simulation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblLegend.Text = "List";

        if (!IsPostBack)
        {
            List();
            bttFirst.Enabled = false;
            bttPrevious.Enabled = false;
        }
    }

    private void List()
    {
        List(string.Empty);
    }

    private void List(string text)
    {

        List<Process> processes = new List<Process>();

        using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        {

            if (text == string.Empty)
            {
                foreach (Process temp in ctx.Process.Include("DataToProcess").Include("Status").Where(p => p.guidFather == null).ToList().OrderByDescending(p => p.date))
                {
                    Process item = new Process();
                    item.label = temp.label;
                    item.date = temp.date;
                    item.guid = temp.guid;
                    item.userId = temp.userId;
                    item.status_id = temp.status_id;
                    item.Status = temp.Status;
                    item.DataToProcess = temp.DataToProcess;
                    processes.Add(item);
                }
            }
            else
            {
                foreach (Process temp in ctx.Process.Include("DataToProcess").Include("Status").ToList().Where(p => p.guidFather == null && p.label.Contains(text)).OrderByDescending(p => p.date))
                {
                    Process item = new Process();
                    item.label = temp.label;
                    item.date = temp.date;
                    item.guid = temp.guid;
                    item.userId = temp.userId;
                    item.status_id = temp.status_id;
                    item.Status = temp.Status;
                    item.DataToProcess = temp.DataToProcess;
                    processes.Add(item);
                }
            }
        }

        GridViewProcess.DataSource = processes;
        GridViewProcess.DataBind();

        lblTotal.Text = processes.Count.ToString();
        lblActualPage.Text = (GridViewProcess.PageIndex + 1).ToString();
        lblTotalPages.Text = GridViewProcess.PageCount.ToString();
    }

    private void UpdateStatus(Guid guid, int value)
    {
        using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        {

            Process process = ctx.Process.FirstOrDefault(p => p.guid == guid);
            process.status_id = Convert.ToByte(value);

            ctx.SaveChanges();
        }

    }

    private void Delete(Guid guid)
    {

        using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        {
            var model = ctx.Model.FirstOrDefault(f => f.process_guid == guid);
            ctx.Model.Remove(model);


            var data = ctx.DataToProcess.FirstOrDefault(f => f.process_guid == guid);
            ctx.DataToProcess.Remove(data);

            var form = ctx.Process.FirstOrDefault(f => f.guid == guid);
            ctx.Process.Remove(form);

            ctx.SaveChanges();

        }

        List();

    }



    private string User(Guid userId)
    {
        string userName = string.Empty;

        using (GridProteinFolding_MemberShipEntities ctx = new GridProteinFolding_MemberShipEntities())
        {
            aspnet_Users user = ctx.aspnet_Users.FirstOrDefault(c => c.UserId == userId);
            if (user == null)
                userName = "<unknown>";
            else
                userName = user.UserName;
        }

        return userName;
    }

    protected void gvChildGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int status = Convert.ToInt32(e.Row.Cells[2].Text.ToString());

            e.Row.Cells[2].Text = Status(status);


            if (status != 0)//"created"
            {
                ((DropDownList)e.Row.Cells[3].FindControl("ddListOption")).Enabled = false;

                ((LinkButton)e.Row.Cells[5].FindControl("lbDelete")).Enabled = false;
                ((LinkButton)e.Row.Cells[5].FindControl("lbDelete")).Visible = false;


                ((LinkButton)e.Row.Cells[4].FindControl("lbEdit")).Text = "View";
            }

            if (status != 9)//"resultsprocessed"
            {
                ((HyperLink)e.Row.Cells[6].FindControl("lbDownload")).Enabled = false;
                ((HyperLink)e.Row.Cells[6].FindControl("lbDownload")).Visible = false;
            }
        }
    }

    protected void GridViewProcess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Guid user = new Guid(e.Row.Cells[4].Text.ToString());
            e.Row.Cells[4].Text = User(user);

            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                List<Process> processes = new List<Process>();

                GridView gvChildGrid = (GridView)e.Row.FindControl("gvChildGrid");
                Guid gui = new Guid(e.Row.Cells[2].Text.Trim());

                foreach (Process temp in ctx.Process.Include("DataToProcess").Include("Status").Where(p => p.guidFather == gui || p.guid == gui).ToList().OrderBy(p => p.date))
                {
                    Process item = new Process();
                    item.label = temp.label;
                    item.date = temp.date;
                    item.guid = temp.guid;
                    item.userId = temp.userId;
                    item.status_id = temp.status_id;
                    item.Status = temp.Status;
                    item.DataToProcess = temp.DataToProcess;

                    processes.Add(item);
                }

                gvChildGrid.DataSource = processes;
                gvChildGrid.DataBind();
            }
        }
    }

    private string Status(int value)
    {
        switch (value)
        {
            case 0:
                return "Created";
            case 1:
                return "Waiting";
            case 2:
                return "Processing";
            case 3:
                return "Processed";
            case 4:
                return "Upload";
            case 5:
                return "BULK";
            case 6:
                return "Clear Temp Client";
            case 7:
                return "Clear Temp Server";
            case 8:
                return "Finalized";
            case 9:
                return "Results Processed";
            case 96:
                return " ETL Error";
            case 97:
                return "No Change Status";
            case 98:
                return "Declined";
            case 99:
                return "Error";
        }
        return null;
    }

    protected void GridViewProcess_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Guid guid = new Guid(e.CommandArgument.ToString());
        switch (e.CommandName)
        {
            case "Delete":
                Delete(guid);
                break;
            case "Edit":
                Response.Redirect(string.Format("~/Pages/SimulationAddEdit.aspx?guId={0}", e.CommandArgument));
                break;
        }
    }
    protected void ddListOption_SelectedIndexChanged(object sender, EventArgs e)
    {

        int item = Convert.ToInt16(((ListControl)(sender)).SelectedValue);

        if (item != -1)
        {

            GridViewRow gvRow = (GridViewRow)((ListControl)(sender)).NamingContainer;

            Guid guid = new Guid(gvRow.Cells[0].Text.ToString());
            UpdateStatus(guid, item);
        }

        List();

    }
    protected void bttAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/SimulationAddEdit.aspx");
    }
    protected void bttSearch_Click(object sender, EventArgs e)
    {
        List(txtSearch.Text.Trim());
    }

    protected void GridViewProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewProcess.PageIndex = e.NewPageIndex;
        List();
    }


    protected void bttFirst_Click(object sender, EventArgs e)
    {
        GridViewProcess.PageIndex = 0;
        bttFirst.Enabled = false;
        bttPrevious.Enabled = false;
        bttLast.Enabled = true;
        bttNext.Enabled = true;

        List();
    }


    protected void bttNext_Click(object sender, EventArgs e)
    {
        int i = GridViewProcess.PageIndex + 1;
        if (i <= GridViewProcess.PageCount)
        {
            GridViewProcess.PageIndex = i;
            bttLast.Enabled = true;
            bttPrevious.Enabled = true;
            bttFirst.Enabled = true;
        }

        if (GridViewProcess.PageCount - 1 == GridViewProcess.PageIndex)
        {
            bttNext.Enabled = false;
            bttLast.Enabled = false;
        }

        List();
    }

    protected void bttPrevious_Click(object sender, EventArgs e)
    {
        int i = GridViewProcess.PageCount;
        if (GridViewProcess.PageIndex > 0)
        {

            GridViewProcess.PageIndex = GridViewProcess.PageIndex - 1;
            bttLast.Enabled = true;
        }

        if (GridViewProcess.PageIndex == 0)
        {
            bttFirst.Enabled = false;
        }
        if (GridViewProcess.PageCount - 1 == GridViewProcess.PageIndex + 1)
        {
            bttNext.Enabled = true;
        }
        if (GridViewProcess.PageIndex == 0)
        {
            bttPrevious.Enabled = false;
        }

        List();
    }

    protected void bttLast_Click(object sender, EventArgs e)
    {
        GridViewProcess.PageIndex = GridViewProcess.PageCount;
        bttLast.Enabled = false;
        bttFirst.Enabled = true;

        List();
    }
}