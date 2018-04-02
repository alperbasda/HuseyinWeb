using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using NihaiWeb.Models;

namespace MuhasebeWeb.Models.TableModels
{
    public abstract class BasicTable
    {
        private DB context;

        public DB Context
        {
            get
            {
                context = context ?? new DB();
                return context;
            }
            set { context = value; }
        }

        public BasicTable NextTable { get; set; }

        public void SetTable(BasicTable table)
        {
            NextTable = table;
        }

        public abstract MvcHtmlString GetTable(int id);

        public abstract bool RemoveData(int id);





    }
}