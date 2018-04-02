﻿using DevExpress.Web.Mvc;
using NihaiWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MuhasebeWeb.Models;
using DevExpress.Spreadsheet;
using MuhasebeWeb.Models.TableModels;
using MuhasebeWeb.Models.TableModels.TableCRUD;

namespace MuhasebeWeb.Controllers
{
    public class AdminController : Controller
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

        public ActionResult Index()
        {
            if (Session["isAdmin"] == null)
                return RedirectToAction("Login", "Admin");

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string mailAddress, string pass)
        {
            if (mailAddress == "asd" && pass == "123")
                Session["isAdmin"] = true;
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Admin");
        }

        #region CreateMetots
        [HttpGet]
        public JsonResult CreateCompanyType(string type)
        {
            if (string.IsNullOrEmpty(type)) return Json(0, JsonRequestBehavior.AllowGet);
            if (Session["isAdmin"] == null)
                return Json(-1, JsonRequestBehavior.AllowGet);
            try
            {
                Context.CompanyTypes.Add(new CompanyType() { name = type });
                Context.SaveChanges();
                return Json(type, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public JsonResult CreateBranchGroup(string name)
        {
            if (string.IsNullOrEmpty(name)) return Json(0, JsonRequestBehavior.AllowGet);
            if (Session["isAdmin"] == null)
                return Json(-1, JsonRequestBehavior.AllowGet);
            try
            {
                Context.branchGroups.Add(new BranchGroup() { name = name });
                Context.SaveChanges();
                return Json(name, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CreateAccount(string name)
        {
            if (string.IsNullOrEmpty(name)) return Json(0, JsonRequestBehavior.AllowGet);
            if (Session["isAdmin"] == null)
                return Json(-1, JsonRequestBehavior.AllowGet);
            try
            {
                Context.Accounts.Add(new Account() { name = name });
                Context.SaveChanges();
                return Json(name, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult CreateCompany(string typeId, string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(typeId)) return Json(0, JsonRequestBehavior.AllowGet);
            if (Session["isAdmin"] == null)
                return Json(-1, JsonRequestBehavior.AllowGet);
            try
            {
                Context.Companies.Add(new Company() { name = name, companyTypeId = Convert.ToInt32(typeId) });
                Context.SaveChanges();
                return Json(name, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CreateBranch(string groupId, string name, string branchCode)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(branchCode) || string.IsNullOrEmpty(groupId)) return Json(0, JsonRequestBehavior.AllowGet);
            if (Session["isAdmin"] == null)
                return Json(-1, JsonRequestBehavior.AllowGet);
            try
            {
                Context.Branchs.Add(new Branch() { branchCode = Convert.ToInt32(branchCode), name = name, branchGroupId = Convert.ToInt32(groupId) });
                Context.SaveChanges();
                return Json(name, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CreateCalculate(string name)
        {
            if (string.IsNullOrEmpty(name)) return Json(0, JsonRequestBehavior.AllowGet);
            if (Session["isAdmin"] == null)
                return Json(-1, JsonRequestBehavior.AllowGet);
            try
            {
                Context.Calcs.Add(new Calc() { name = name, sumAll = false, isNew = true });
                Context.SaveChanges();
                return Json(name, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CreateAccountCalcRelation(string bolen, string bolunen, string hesap, string sumAll)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();


            if (bolunen.Length <= 2 || string.IsNullOrEmpty(hesap)) return Json(0, JsonRequestBehavior.AllowGet);

            string formul = "";
            string bolunenler = string.Join(",", js.Deserialize<string[]>(bolunen));
            string bolenler = string.Join(",", js.Deserialize<string[]>(bolen));

            if (Session["isAdmin"] == null)
                return Json(-1, JsonRequestBehavior.AllowGet);
            try
            {

                formul = bolen.Length >= 0 ? bolunenler + "/" + bolenler : bolunenler;
                int id = Convert.ToInt32(hesap);
                int id2 = Convert.ToInt32(hesap);
                Calc c = Context.Calcs.Where(s => s.id == id).FirstOrDefault();
                c.isNew = false;
                c.sumAll = sumAll == "on" ? true : false;
                foreach (string s in bolunenler.Split(','))
                {
                    id = Convert.ToInt32(s);
                    Context.AccountCalcRelation.Add(new AccountCalcRelation() { accountId = id, calcId = id2 });
                }

                if (bolenler.Length >= 0)
                {
                    foreach (string s in bolenler.Split(','))
                    {
                        id = Convert.ToInt32(s);
                        Context.AccountCalcRelation.Add(new AccountCalcRelation() { accountId = id, calcId = id2 });
                    }
                }
                c.formul = c.sumAll == false ? formul : null;
                Context.SaveChanges();
                return Json(c.name + " ilişkileri ", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult DownloadEmptyFile()
        {
            string file = HostingEnvironment.MapPath("~/Content/Document/taslak.xlsx");
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = Path.GetFileName(file);
            return File(file, contentType, fileName);
        }

        [HttpPost]
        public JsonResult UploadData(HttpPostedFileBase file)
        {
            if (file == null) return Json(0, JsonRequestBehavior.AllowGet);
            if (Path.GetExtension(file.FileName) != ".xlsx") return Json(-1, JsonRequestBehavior.AllowGet);

            string path = "~/Content/Excel/veri" + Path.GetExtension(file.FileName);
            if (System.IO.File.Exists(Server.MapPath(path)))
                System.IO.File.Delete(Server.MapPath(path));
            file.SaveAs(Server.MapPath(path));

            return Json(1, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region getPartials
        public JsonResult CompanyTypeSelect()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    List<CompanyType> ct = Context.CompanyTypes.ToList();
                    return Json(ct, JsonRequestBehavior.AllowGet);
                }
                return Json(new List<CompanyType>() { new CompanyType { id = 1, name = "Modalı kapatıp tekrar Açınız" } });
            }
            catch (Exception e)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult EmptyCalcsSelector()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    List<Calc> ct = Context.Calcs.Where(s => s.isNew).ToList();
                    return ct != null ? Json(ct, JsonRequestBehavior.AllowGet) : Json(new List<CompanyType>() { new CompanyType { id = 1, name = "Yeni hesaplama yok" } });
                }
                return Json(new List<CompanyType>() { new CompanyType { id = 1, name = "Modalı kapatıp tekrar Açınız" } });
            }
            catch (Exception e)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AccountSelector()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    List<Account> ct = Context.Accounts.ToList();
                    return Json(ct.Select(s => new AccountCalcRelationHelper { name = s.name, id = s.id }), JsonRequestBehavior.AllowGet);
                }
                return Json(new List<CompanyType>() { new CompanyType { id = 1, name = "Modalı kapatıp tekrar Açınız" } });
            }
            catch (Exception e)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BranchGroupSelector()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    List<BranchGroup> ct = Context.branchGroups.ToList();
                    return Json(ct, JsonRequestBehavior.AllowGet);
                }
                return Json(new List<BranchGroup>() { new BranchGroup() { id = 1, name = "Modalı kapatıp tekrar Açınız" } });
            }
            catch (Exception e)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion



        public ActionResult SpreadsheetPartial()
        {
            return PartialView("_SpreadsheetPartial");
        }
        [HttpPost]
        public ActionResult CreateData()
        {

            IWorkbook wbook = SpreadsheetExtension.GetCurrentDocument("veri");
            // Load a workbook from the stream. 
            using (FileStream stream = new FileStream(Server.MapPath("~/Content/Excel/veri.xlsx"), FileMode.Open))
            {
                wbook.LoadDocument(stream, DocumentFormat.OpenXml);
            }
            Worksheet qwe = wbook.Worksheets.ActiveWorksheet;
            Range range = qwe.GetUsedRange();
            int rowcount = range.RowCount;
            for (int i = 1; i < rowcount; i++)
            {

                decimal d = Convert.ToDecimal(qwe.Cells[i, 3].Value.ToString());
                Data dd = new Data();
                dd.companyId = Convert.ToInt32(qwe.Cells[i, 0].Value.ToString());
                dd.branchId = Convert.ToInt32(qwe.Cells[i, 1].Value.ToString());
                dd.date = Convert.ToDateTime(qwe.Cells[i, 2].Value.ToString());
                dd.amount = d;
                dd.accountId = Convert.ToInt32(qwe.Cells[i, 4].Value.ToString());
                dd.CompanyTypeId = Convert.ToInt32(qwe.Cells[i, 5].Value.ToString());
                Context.Datas.Add(dd);
            }
            Context.SaveChanges();
            var asdas = qwe.Cells[1, 1].Value;
            var asd = wbook.Worksheets.ActiveWorksheet.Range["A1"].Value;
            return RedirectToAction("Index");

        }



        public ActionResult GetTable(int id)
        {
            if (Session["isAdmin"] == null)
                return Json(-1, JsonRequestBehavior.AllowGet);
            BasicTable t1 = new CompanyTypeTable(null);
            BasicTable t2 = new CompanyTable(t1);
            BasicTable t3 = new BranchGroupTable(t2);
            BasicTable t4 = new BranchTable(t3);
            BasicTable t5 = new AccountTable(t4);
            BasicTable t6 = new CalcTable(t5);
            BasicTable t7 = new AccountCalcRelationTable(t6);
            BasicTable t8 = new DataTable(t7);
            return View(t8.GetTable(id));
        }

        [HttpGet]
        public JsonResult Remove(string str)
        {
            try
            {
                if (str != null)
                {
                    string[] dizi = str.Split('-').ToArray();
                    int id = Convert.ToInt32(dizi[1]);
                    switch (dizi[0])
                    {
                        case "acr":
                            Context.AccountCalcRelation.Remove(Context.AccountCalcRelation.FirstOrDefault(s => s.id == id));
                            break;
                        case "a":
                            Context.Accounts.Remove(Context.Accounts.FirstOrDefault(s => s.id == id));
                            break;
                        case "bg":
                            Context.branchGroups.Remove(Context.branchGroups.FirstOrDefault(s => s.id == id));
                            break;
                        case "b":
                            Context.Branchs.Remove(Context.Branchs.FirstOrDefault(s => s.id == id));
                            break;
                        case "cc":
                            Context.Calcs.Remove(Context.Calcs.FirstOrDefault(s => s.id == id));
                            break;
                        case "c":
                            Context.Companies.Remove(Context.Companies.FirstOrDefault(s => s.id == id));
                            break;
                        case "ct":

                            Context.CompanyTypes.Remove(Context.CompanyTypes.FirstOrDefault(s => s.id == id));
                            break;
                        case "d":

                            foreach (Data deleted in Context.Datas.Where(k => k.date == Context.Datas.FirstOrDefault(s => s.id == id).date).ToList())
                            {
                                Context.Entry(deleted).State = EntityState.Deleted;
                            }
                            break;
                    }
                    Context.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }

                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Update(Data d)
        {
            try
            {
                DataBusiness bb = new DataBusiness();
                bb.Update(d);
                return RedirectToAction("GetTable", new { id = 7 });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public JsonResult Search(int id, int skip)
        {
            try
            {
                DataBusiness bb = new DataBusiness();
                return Json(bb.GetList(skip), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult SearchParameter(string type, string type2, string filter)
        {
            bool isInt = false;
            bool isDate = false;
            int filteri = 0;
            DateTime d= DateTime.Now;
            string rows = "";
            if (type2.Contains("id") || type2.Contains("Id")|| type2.Contains("Code")|| type2.Contains("code"))
            {
                isInt = true;
                filteri = Convert.ToInt32(filter);
            }
            else if (type2.Contains("date"))
            {
                isDate = true;
                d = Convert.ToDateTime(filter);
            }

            switch (type)
            {
                case "acr":
                    var parameter = Expression.Parameter(typeof(AccountCalcRelation), "s"); // s =>
                    var left = Expression.PropertyOrField(parameter, type2); // s.Property
                    var right = isInt ? Expression.Constant(filteri, left.Type) : Expression.Constant(filter, left.Type);
                    var condition = Expression.Equal(left, right); // s.Property == null
                    var predicate = Expression.Lambda<Func<AccountCalcRelation, bool>>(condition, parameter);
                    foreach (var item in Context.AccountCalcRelation.Where(predicate).ToList())
                    {
                        rows += "<tr>";
                        rows += "<td name='id'>" + item.id + "</td>";
                        rows += "<td name='accountId'>" + item.accountId + "</td>";
                        rows += "<td name='calcId'>" + item.calcId + "</td>";
                        rows += "<td id = 'acr-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                        rows += "<td id='acr-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                        rows += "</tr>";
                    }
                    return Json(rows, JsonRequestBehavior.AllowGet);
                case "a":
                    var p = Expression.Parameter(typeof(Account), "s"); // s =>
                    var l = Expression.PropertyOrField(p, type2); // s.Property
                    var r = isInt ? Expression.Constant(filteri, l.Type) : Expression.Constant(filter, l.Type);
                    var c = Expression.Equal(l, r); // s.Property == null
                    var pr = Expression.Lambda<Func<Account, bool>>(c, p);
                    foreach (var item in Context.Accounts.Where(pr).ToList())
                    {
                        rows += "<tr>";
                        rows += "<td name='id'>" + item.id + "</td>";
                        rows += "<td name='name'>" + item.name + "</td>";
                        rows += "<td id = 'a-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                        rows += "<td id='a-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                        rows += "</tr>";
                    }
                    return Json(rows, JsonRequestBehavior.AllowGet);

                case "bg":
                    var pa = Expression.Parameter(typeof(BranchGroup), "s"); // s =>
                    var le = Expression.PropertyOrField(pa, type2); // s.Property
                    var ri = isInt ? Expression.Constant(filteri, le.Type) : Expression.Constant(filter, le.Type);
                    var co = Expression.Equal(le, ri); // s.Property == null
                    var pre = Expression.Lambda<Func<BranchGroup, bool>>(co, pa);
                    foreach (var item in Context.branchGroups.Where(pre).ToList())
                    {
                        rows += "<tr>";
                        rows += "<td name='id'>" + item.id + "</td>";
                        rows += "<td name='name'>" + item.name + "</td>";
                        rows += "<td id = 'bg-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                        rows += "<td id='bg-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                        rows += "</tr>";
                    }
                    return Json(rows, JsonRequestBehavior.AllowGet);
                case "b":
                    var par = Expression.Parameter(typeof(Branch), "s"); // s =>
                    var lef = Expression.PropertyOrField(par, type2); // s.Property
                    var rig = isInt ? Expression.Constant(filteri, lef.Type) : Expression.Constant(filter, lef.Type);
                    var con = Expression.Equal(lef, rig); // s.Property == null
                    var pred = Expression.Lambda<Func<Branch, bool>>(con, par);
                    foreach (var item in Context.Branchs.Where(pred).ToList())
                    {
                        rows += "<tr>";
                        rows += "<td name='id'>" + item.id + "</td>";
                        rows += "<td name='name'>" + item.name + "</td>";
                        rows += "<td name='branchCode'>" + item.branchCode + "</td>";
                        rows += "<td name='branchGroupId'>" + item.branchGroupId + "</td>";
                        rows += "<td id = 'b-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                        rows += "<td id='b-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                        rows += "</tr>";
                    }
                    return Json(rows, JsonRequestBehavior.AllowGet);
                case "cc":
                    var para = Expression.Parameter(typeof(Calc), "s"); // s =>
                    var leftt = Expression.PropertyOrField(para, type2); // s.Property
                    var righ = isInt ? Expression.Constant(filteri, leftt.Type) : Expression.Constant(filter, leftt.Type);
                    var cond = Expression.Equal(leftt, righ); // s.Property == null
                    var predi = Expression.Lambda<Func<Calc, bool>>(cond, para);
                    foreach (var item in Context.Calcs.Where(predi).ToList())
                    {
                        rows += "<tr>";
                        rows += "<td name='id'>" + item.id + "</td>";
                        rows += "<td name='name'>" + item.name + "</td>";
                        rows += "<td name='sumAll'>" + item.sumAll + "</td>";
                        rows += "<td name='formul'>" + item.formul + "</td>";
                        rows += "<td name='isNew'>" + item.isNew + "</td>";
                        rows += "<td id = 'cc-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                        rows += "<td id='cc-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                        rows += "</tr>";
                    }
                    return Json(rows, JsonRequestBehavior.AllowGet);

                case "c":
                    var param = Expression.Parameter(typeof(Company), "s"); // s =>
                    var lefttt = Expression.PropertyOrField(param, type2); // s.Property
                    var rightt = isInt ? Expression.Constant(filteri, lefttt.Type) : Expression.Constant(filter, lefttt.Type);
                    var condi = Expression.Equal(lefttt, rightt); // s.Property == null
                    var predic = Expression.Lambda<Func<Company, bool>>(condi, param);
                    foreach (var item in Context.Companies.Where(predic).ToList())
                    {
                        rows += "<tr>";
                        rows += "<td name='id'>" + item.id + "</td>";
                        rows += "<td name='name'>" + item.name + "</td>";
                        rows += "<td name='companyTypeId'>" + item.companyTypeId + "</td>";
                        rows += "<td id = 'c-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                        rows += "<td id='c-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                        rows += "</tr>";
                    }
                    return Json(rows, JsonRequestBehavior.AllowGet);

                case "ct":
                    var parame = Expression.Parameter(typeof(CompanyType), "s"); // s =>
                    var leftttt = Expression.PropertyOrField(parame, type2); // s.Property
                    var righttt = isInt ? Expression.Constant(filteri, leftttt.Type) : Expression.Constant(filter, leftttt.Type);
                    var condit = Expression.Equal(leftttt, righttt); // s.Property == null
                    var predica = Expression.Lambda<Func<CompanyType, bool>>(condit, parame);
                    foreach (var item in Context.CompanyTypes.Where(predica).ToList())
                    {
                        rows += "<tr>";
                        rows += "<td name='id'>" + item.id + "</td>";
                        rows += "<td name='name'>" + item.name + "</td>";
                        rows += "<td id = 'ct-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                        rows += "<td id='ct-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                        rows += "</tr>";
                    }
                    return Json(rows, JsonRequestBehavior.AllowGet);
                case "d":
                    var paramet = Expression.Parameter(typeof(Data), "s"); // s =>
                    var lefttttt = Expression.PropertyOrField(paramet, type2); // s.Property
                    var rightttt = isInt ? Expression.Constant(filteri, lefttttt.Type) :( isDate ? Expression.Constant(d, lefttttt.Type) : Expression.Constant(filter, lefttttt.Type));
                    var conditi = Expression.Equal(lefttttt, rightttt); // s.Property == null
                    var predicat = Expression.Lambda<Func<Data, bool>>(conditi, paramet);
                    foreach (var item in Context.Datas.Where(predicat).Take(30).ToList())
                    {
                        rows += "<tr>";
                        rows += "<td name='id'>" + item.id + "</td>";
                        rows += "<td name='companyId'>" + item.companyId + "</td>";
                        rows += "<td name='branchId'>" + item.branchId + "</td>";
                        rows += "<td name='accountId'>" + item.accountId + "</td>";
                        rows += "<td name='date'>" + item.date + "</td>";
                        rows += "<td name='amount'>" + item.amount + "</td>";
                        rows += "<td name='CompanyTypeId'>" + item.CompanyTypeId + "</td>";
                        rows += "<td id = 'd-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td>";
                        rows += "<td id='d-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                        rows += "</tr>";
                    }
                    return Json(rows, JsonRequestBehavior.AllowGet);
            }


            return Json("dsa", JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int id)
        {
            try
            {
                DataBusiness bb = new DataBusiness();
                return Json(bb.Get(s => s.id == id), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Next(string type, int skip)
        {
            try
            {
                string rows = "";
                switch (type)
                {
                    case "ac":
                        foreach (var item in Context.AccountCalcRelation.OrderBy(s => s.id).Skip(skip).Take(20).ToList())
                        {
                            rows += "<tr>";
                            rows += "<td name='id'>" + item.id + "</td>";
                            rows += "<td name='accountId'>" + item.accountId + "</td>";
                            rows += "<td name='calcId'>" + item.calcId + "</td>";
                            rows += "<td id = 'acr-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                            rows += "<td id='acr-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                            rows += "</tr>";
                        }
                        return Json(rows, JsonRequestBehavior.AllowGet);
                    case "a":
                        foreach (var item in Context.Accounts.OrderBy(s => s.id).Skip(skip).Take(20).ToList())
                        {
                            rows += "<tr>";
                            rows += "<td name='id'>" + item.id + "</td>";
                            rows += "<td name='name'>" + item.name + "</td>";
                            rows += "<td id = 'a-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                            rows += "<td id='a-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                            rows += "</tr>";
                        }
                        return Json(rows, JsonRequestBehavior.AllowGet);

                    case "bg":
                        foreach (var item in Context.branchGroups.OrderBy(s => s.id).Skip(skip).Take(20).ToList())
                        {
                            rows += "<tr>";
                            rows += "<td name='id'>" + item.id + "</td>";
                            rows += "<td name='name'>" + item.name + "</td>";
                            rows += "<td id = 'bg-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                            rows += "<td id='bg-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                            rows += "</tr>";
                        }
                        return Json(rows, JsonRequestBehavior.AllowGet);
                    case "b":
                        foreach (var item in Context.Branchs.OrderBy(s => s.id).Skip(skip).Take(20).ToList())
                        {
                            rows += "<tr>";
                            rows += "<td name='id'>" + item.id + "</td>";
                            rows += "<td name='name'>" + item.name + "</td>";
                            rows += "<td name='branchCode'>" + item.branchCode + "</td>";
                            rows += "<td name='branchGroupId'>" + item.branchGroupId + "</td>";
                            rows += "<td id = 'b-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                            rows += "<td id='b-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                            rows += "</tr>";
                        }
                        return Json(rows, JsonRequestBehavior.AllowGet);
                    case "cc":
                        foreach (var item in Context.Calcs.OrderBy(s => s.id).Skip(skip).Take(20).ToList())
                        {
                            rows += "<tr>";
                            rows += "<td name='id'>" + item.id + "</td>";
                            rows += "<td name='name'>" + item.name + "</td>";
                            rows += "<td name='sumAll'>" + item.sumAll + "</td>";
                            rows += "<td name='formul'>" + item.formul + "</td>";
                            rows += "<td name='isNew'>" + item.isNew + "</td>";
                            rows += "<td id = 'cc-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                            rows += "<td id='cc-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                            rows += "</tr>";
                        }
                        return Json(rows, JsonRequestBehavior.AllowGet);
                    case "c":
                        foreach (var item in Context.Companies.OrderBy(s => s.id).Skip(skip).Take(20).ToList())
                        {
                            rows += "<tr>";
                            rows += "<td name='id'>" + item.id + "</td>";
                            rows += "<td name='name'>" + item.name + "</td>";
                            rows += "<td name='companyTypeId'>" + item.companyTypeId + "</td>";
                            rows += "<td id = 'c-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                            rows += "<td id='c-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                            rows += "</tr>";
                        }
                        return Json(rows, JsonRequestBehavior.AllowGet);
                    case "ct":
                        foreach (var item in Context.CompanyTypes.OrderBy(s => s.id).Skip(skip).Take(20).ToList())
                        {
                            rows += "<tr>";
                            rows += "<td name='id'>" + item.id + "</td>";
                            rows += "<td name='name'>" + item.name + "</td>";
                            rows += "<td id = 'ct-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td >";
                            rows += "<td id='ct-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                            rows += "</tr>";
                        }
                        return Json(rows, JsonRequestBehavior.AllowGet);
                    case "d":
                        foreach (var item in Context.Datas.OrderBy(s => s.id).Skip(skip).Take(20).ToList())
                        {
                            rows += "<tr>";
                            rows += "<td name='id'>" + item.id + "</td>";
                            rows += "<td name='companyId'>" + item.companyId + "</td>";
                            rows += "<td name='branchId'>" + item.branchId + "</td>";
                            rows += "<td name='accountId'>" + item.accountId + "</td>";
                            rows += "<td name='date'>" + item.date + "</td>";
                            rows += "<td name='amount'>" + item.amount + "</td>";
                            rows += "<td name='CompanyTypeId'>" + item.CompanyTypeId + "</td>";
                            rows += "<td id = 'd-" + item.id + "' ><i onclick = 'Update(this)'; class='fa fa-edit' style='cursor:pointer'></i></td>";
                            rows += "<td id='d-" + item.id + "'><i onclick='Remove(this)'; class='fa fa-remove' style='color:red;cursor:pointer'></i></td>";
                            rows += "</tr>";
                        }
                        return Json(rows, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }



    }


}