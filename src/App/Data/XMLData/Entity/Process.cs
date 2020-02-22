using GridProteinFolding.Data.XMLData.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Data.XMLData.Entity
{
    public class Process
    {
        public Guid guid;
        public int status;
        public DateTime date;

        public Process()
        { }


        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
    }
}
