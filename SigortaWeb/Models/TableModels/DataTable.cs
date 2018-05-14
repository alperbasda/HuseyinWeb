using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SigortaWeb.Models.TableModels
{
    public class DataTable : BasicTable
    {
        public List<Data> RowList { get; set; }

        public List<string> HeaderList { get; set; }

        public DataTable(BasicTable T)
        {
            SetTable(T);
        }

        public override MvcHtmlString GetTable(int id)
        {
            if (id == 7)
            {
                HeaderList = new List<string>();
                RowList = Context.Datas.Take(20).ToList();
                FillPropNames(RowList.First());
                TagBuilder tableBuilder = new TagBuilder("table");
                tableBuilder.AddCssClass("table");
                tableBuilder.AddCssClass("table-hover");
                tableBuilder.GenerateId("myTable");
                #region thbuilder

                tableBuilder.InnerHtml += "<thead>";
                tableBuilder.InnerHtml += "<tr>";
                foreach (string th in HeaderList)
                {
                    tableBuilder.InnerHtml += "<th scope='col'>" + th + "</th>";
                }
                tableBuilder.InnerHtml += "<th scope='col'>Düzenle</th>";
                tableBuilder.InnerHtml += "<th scope='col'>Sil</th>";
                tableBuilder.InnerHtml += "</tr>";
                tableBuilder.InnerHtml += "</thead>";
                #endregion

                #region trbuilder

                tableBuilder.InnerHtml += "<tbody>";
                foreach (Data item in RowList)
                {
                    tableBuilder.InnerHtml += "<tr>";
                    foreach (string prop in HeaderList)
                    {
                        tableBuilder.InnerHtml += "<td name='"+prop+"'>" + item.GetType().GetProperty(prop)?.GetValue(item) + "</td>";
                    }
                    tableBuilder.InnerHtml += "<td id='d-" + item.GetType().GetProperty("id")?.GetValue(item) + "'><i onclick='Update(this)'; class='fa fa-edit' style='cursor:pointer'></i> </td>";
                    tableBuilder.InnerHtml += "<td id='d-" + item.GetType().GetProperty("id")?.GetValue(item) + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i> </td>";
                    tableBuilder.InnerHtml += "</tr>";
                }
                tableBuilder.InnerHtml += "</tbody>";

                #endregion

                return MvcHtmlString.Create("<div class='row'><input type='text' class='form-control col-lg-8' id='TbSearch' placeholder='Arama için Yazınız'><input type='text' class='form-control col-lg-2' id='TbFilter' placeholder='Kriter Yazınız'><button id='BtnSearch' data-id='d' class='btn btn-info col-lg-2'>Ara</button></div>" + tableBuilder.ToString(TagRenderMode.Normal) + "<br><br><div class='text-center'><button id='BtnNext' data-id='d' class='btn btn-lg btn-success'> Devamını Gör</button></div><br><br><br>");
            }
            return NextTable.GetTable(id);
        }

        public override bool RemoveData(int id)
        {
            try
            {
                Context.Datas.Remove(Context.Datas.Find(id));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void FillPropNames(Data T)
        {
            if (T != null)
            {
                PropertyInfo[] properties1 = T.GetType().GetProperties();
                foreach (var p in properties1)
                {
                    string s = p.PropertyType.ToString();
                    if (!p.PropertyType.IsClass || p.PropertyType.ToString().Contains("String"))
                    {
                        if (!p.PropertyType.ToString().Contains("ICollection"))
                            HeaderList.Add(p.Name);
                    }
                }
            }
        }
    }
}