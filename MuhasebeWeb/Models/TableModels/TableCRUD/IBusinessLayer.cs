using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Spreadsheet.Formulas;

namespace MuhasebeWeb.Models.TableModels.TableCRUD
{
    public interface IBusinessLayer<T> where T:class,new()
    {
        //Create işlemi direk controllerda yapıldı buraya almaya uğraşma
        T Update(T entity);
        List<T> GetList(int skip, Expression<Func<T, bool>> filter=null);
        T Get(Expression<Func<T, bool>> filter);
        bool Delete(T entity);
    }
}
