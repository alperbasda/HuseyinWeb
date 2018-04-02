using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MuhasebeWeb.Models;

namespace MuhasebeWeb.Models.TableModels.TableCRUD
{
    public class DataBusiness : BusinessLayer<Data>
    {
        public bool DeleteAll(DateTime d)
        {
            foreach (Data data in Context.Datas.Where(ds=>ds.date==d))
            {
                var delete = Context.Entry(data);
                delete.State = EntityState.Deleted;
            }
            return true;
        }
    }
}