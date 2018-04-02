using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using NihaiWeb.Models;

namespace MuhasebeWeb.Models.TableModels.TableCRUD
{
    public class BusinessLayer<T> : IBusinessLayer<T> where T : class, new()
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
        public T Update(T entity)
        {
            try
            {
                var update = context.Entry(entity);
                update.State = EntityState.Modified;
                context.SaveChanges();
                return entity;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public List<T> GetList(int skip, Expression<Func<T, bool>> filter = null)
        {
            return filter != null ? context.Set<T>().Where(filter).Skip(skip).Take(20).ToList() : context.Set<T>().Skip(skip).Take(20).ToList();
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            try
            {

            
            return context.Set<T>().SingleOrDefault(filter);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool Delete(T entity)
        {
            try
            {    
                var delete = context.Entry(entity);
                delete.State = EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}