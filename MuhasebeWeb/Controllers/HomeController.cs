using MuhasebeWeb.Models;
using NihaiWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Spreadsheet;
using DevExpress.Web.Mvc;
namespace MuhasebeWeb.Controllers
{
    public class HomeController : Controller
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
        #region partials

        public ActionResult BranchGroup()
        {
            return PartialView(Context.branchGroups.ToList());
        }
        public ActionResult Branch()
        {
            return PartialView();
        }
        public ActionResult Account()
        {
            return PartialView();
        }
        public ActionResult calculate()
        {
            return PartialView(Context.Calcs.ToList());
        }
        public ActionResult CompanyTypeSelector()
        {
            return PartialView(Context.CompanyTypes.ToList());
        }
        public ActionResult CompanySelector()
        {
            return PartialView();
        }
        #endregion

        [HttpGet]
        public JsonResult BranchJson(int id)
        {

#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
            if (id != null)
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                return Json(Context.Branchs.Where(s => s.branchGroupId == id).ToList(), JsonRequestBehavior.AllowGet);

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CompanyJson(int id)
        {
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
            if (id != null)
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
            {
                List<Company> cmp = Context.Companies.Where(s => s.companyTypeId == id).ToList();
                return Json(cmp, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Hesapla(int[] branches, int[] calculates, string[] dates, int[] companies)
        {
            if (dates[0].ToString().Length == 0 || branches[0].ToString().Length == 0 || calculates[0].ToString().Length == 0 || companies[0].ToString().Length == 0)
                return Json(-1, JsonRequestBehavior.AllowGet);

            try
            {

                string strbran = "";
                for (int i = 0; i < branches.Length; i++)
                {
                    strbran += branches[i];
                    if (i != branches.Length - 1) strbran += ",";
                }

                string strdate = "";
                for (int i = 0; i < dates.Length; i++)
                {
                    DateTime d = Convert.ToDateTime(dates[i]);
                    strdate += ((char)39).ToString() + d.Year + "-" + d.Month + "-01" + ((char)39).ToString();
                    if (i != dates.Length - 1) strdate += ",";
                }

                string strComp = "";
                string smallQuery = "";
                bool add = false;
                bool addc = false;
                string sorgu = "";
                for (int i = 0; i < companies.Length; i++)
                {
                    if (companies[i] == 72 || companies[i] == 73 || companies[i] == 74 || companies[i] == 75)
                    {
                        smallQuery += companies[i];
                        add = true;
                        if (i != companies.Length - 1) smallQuery += ",";
                    }
                    else
                    {
                        strComp += companies[i];
                        addc = true;
                        if (i != companies.Length - 1) strComp += ",";
                    }
                }
                if (smallQuery != "")
                {
                    if (smallQuery[smallQuery.Length - 1] == ',')
                    {
                        smallQuery = smallQuery.Substring(0, smallQuery.Length - 1);
                    }
                }
                if (strComp != "")
                {
                    if (strComp[strComp.Length - 1] == ',')
                    {
                        strComp = strComp.Substring(0, strComp.Length - 1);
                    }
                }
                //Toplam İşlemleri
                if (add)
                {
                    for (int u = 0; u < calculates.Length; u++)
                    {
                        int ct = calculates[u];
                        Calc c = Context.Calcs.Where(s => s.id == ct).FirstOrDefault();
                        if (c.sumAll)
                        {

                            sorgu += " select 'Toplam '+ (select name from CompanyTypes where id=companyTypeId) as companyName ,date,(select name from Calcs where id = " + calculates[u] + " )as calcName,sum(amount) as amount from Data B where B.date in  (" + strdate + ") and  B.branchId in(" + strbran + ") and B.companyTypeId in(select companyTypeId from Companies C where C.companyTypeId in(select companyTypeId from Companies where id in(" + smallQuery + "))) and B.accountId in (select A.accountId from AccountCalcRelations A where A.calcId = " + calculates[u] + ") group by  date,companyTypeId ";

                        }
                        //buraya sorgu yazılacak üstteki sorgunun farklı formul türevi 
                        else
                        {
                            string[] parse = c.formul.Split('/')[0].Split(',').ToArray();
                            string ss = "";
                            string ss2 = "";
                            int k = 0;
                            foreach (string item in parse)
                            {
                                if (int.TryParse(item, out k))
                                {
                                    ss += item + ",";
                                }
                            }
                            ss = ss.Substring(0, ss.Length - 1);
                            parse = c.formul.Split('/')[1].Split(',').ToArray();
                            foreach (string item in parse)
                            {
                                if (int.TryParse(item, out k))
                                {
                                    ss2 += item + ",";
                                }
                            }
                            ss2 = ss2.Substring(0, ss2.Length - 1);
                            
                            sorgu += " select x.companyName,x.date,x.calcName, ((x.amount*100) / y.amount ) as amount from(select 'Toplam '+ (select name from CompanyTypes where id=companyTypeId) as companyName ,date,(select name from Calcs where id = " + calculates[u] + " )as calcName,sum(amount) as amount from Data B where B.date in  (" + strdate + ") and  B.branchId in(" + strbran + ") and B.companyTypeId in(select companyTypeId from Companies C where C.companyTypeId in(select companyTypeId from Companies where id in(" + smallQuery + "))) and B.accountId in (" + ss + ") group by  date,companyTypeId) x join ( select 'Toplam '+ (select name from CompanyTypes where id=companyTypeId) as companyName ,date,(select name from Calcs where id = " + calculates[u] + " )as calcName,sum(amount) as amount from Data B where B.date in  (" + strdate + ") and  B.branchId in(" + strbran + ") and B.companyTypeId in(select companyTypeId from Companies C where C.companyTypeId in(select companyTypeId from Companies where id in(" + smallQuery + "))) and B.accountId in (" + ss2 + ") group by  date,companyTypeId) y on x.companyName = y.companyName and x.date = y.date ";
                        }


                        if (u != calculates.Length - 1) sorgu += " union all ";
                    }
                    if (addc) sorgu += " union all ";
                }
                if (addc)
                {


                    for (int i = 0; i < calculates.Length; i++)
                    {
                        int ct = calculates[i];
                        Calc c = Context.Calcs.Where(s => s.id == ct).FirstOrDefault();
                        //stardart queryler
                        if (c.sumAll)
                        {
                            sorgu += " select ( select name from Companies where id = companyId) as companyName,date,(select name from Calcs where id = " + calculates[i] + " ) as calcName,sum(amount) as amount from Data B where B.companyId in(" + strComp + ") and B.date in (" + strdate + ")   and B.branchId in(" + strbran + ") and B.accountId in (select A.accountId from AccountCalcRelations A where A.calcId = " + calculates[i] + ")   group by  date,companyId ";
                        }
                        //bölümlü queryler
                        else
                        {
                            string[] parse = c.formul.Split('/')[0].Split(',').ToArray();
                            string ss = "";
                            string ss2 = "";
                            int k = 0;
                            foreach (string item in parse)
                            {
                                if (int.TryParse(item, out k))
                                {
                                    ss += item + ",";
                                }
                            }
                            ss = ss.Substring(0, ss.Length - 1);
                            parse = c.formul.Split('/')[1].Split(',').ToArray();
                            foreach (string item in parse)
                            {
                                if (int.TryParse(item, out k))
                                {
                                    ss2 += item + ",";
                                }
                            }
                            ss2 = ss2.Substring(0, ss2.Length - 1);

                            sorgu += " select x.companyName,x.date,x.calcName, ((x.amount*100) / y.amount ) as amount from( select(select name from Companies where id = companyId) as companyName, date, (select name from Calcs where id = " + calculates[i] + ") as calcName,sum(amount) as amount from Data B where B.companyId in(" + strComp + ") and B.date in (" + strdate + ")   and B.branchId in(" + strbran + ") and B.accountId in (" + ss + ")   group by  date,companyId) x join ( select(select name from Companies where id = companyId) as companyName, date, (select name from Calcs where id = " + calculates[i] + ") as calcName,sum(amount) as amount from Data B where B.companyId in(" + strComp + ") and B.date in (" + strdate + ")   and B.branchId in(" + strbran + ") and B.accountId in (" + ss2 + ") group by  date,companyId) y on x.companyName = y.companyName and x.date = y.date ";
                        }
                        if (i != calculates.Length - 1) sorgu += " union all ";
                    }
                }

                List<companyDateHelper> adder = new List<companyDateHelper>();
                for (int k = 0; k < calculates.Length; k++)
                {
                    int dene2 = calculates[k];
                    string sc = Context.Calcs.Where(s => s.id == dene2).Select(a => a.name).FirstOrDefault();

                    for (int j = 0; j < companies.Length; j++)
                    {
                        int dene = companies[j];
                        string scom = "";
                        scom = Context.Companies.Where(s => s.id == dene).Select(a => a.name).FirstOrDefault();
                        for (int i = 0; i < dates.Length; i++)
                        {
                            adder.Add(new companyDateHelper()
                            {
                                calcType = sc,
                                indexer = k,
                                amount = "0",
                                companyName = scom,
                                date = Convert.ToDateTime(dates[i]),
                            });
                        }
                    }
                }


                using (SqlConnection con = new SqlConnection(@"server=.\SQLEXPRESS;database=NihaiVeri;Integrated Security=True"))
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand(sorgu, con) { CommandTimeout = 3600 };
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        companyDateHelper data = adder.Where(s => s.date == (DateTime)dr["date"] && s.companyName.ToLower() == dr["companyName"].ToString().ToLower() && s.calcType == dr["calcName"].ToString()).FirstOrDefault();
                        data.amount = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", dr["amount"]);
                    }
                    con.Close();

                }
                return Json(adder, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }

        #region hesaplamalar


        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        

    }
}