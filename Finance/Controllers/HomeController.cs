using Finance.DBset;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finance.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Cost c = new Cost();
            List<Cost> cost = c.GetCosts();
            int total = 0;
            foreach (var item in cost)
            {
                string spent = item.cost;
                total += Convert.ToInt32(spent);
            }
            ViewBag.totaCost = total;
            return View(cost);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Cost_Save(string classification, string name, string cost)
        {
            JObject jo = new JObject();
            Cost c = new Cost();
            runResult r = c.ACost(name, classification, cost);
            if (r.Count == 1) { jo.Add("Msg", "儲存成功"); }
            else { jo.Add("Msg", r.ErrorMsg); }
            return Json(jo, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(int ID)
        {
            Cost c = new Cost();
            List<Cost> costs = c.GetCosts();
            var queryFD = costs.Where(m => m.ID == ID).FirstOrDefault();
            return View(queryFD);
        }
        [HttpPost]
        public JsonResult Update_Save(string classification, string name, string cost, int ID)
        {
            JObject jo = new JObject();
            Cost c = new Cost();
            runResult r = c.UpdateACost(name, classification, cost, ID);
            if (r.Count == 1) { jo.Add("Msg", "儲存成功"); }
            else { jo.Add("Msg", r.ErrorMsg); }
            return Json(jo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int ID)
        {
            JObject jo = new JObject();
            Cost c = new Cost();
            runResult r = c.DeleteACost(ID);
            return RedirectToAction("Index");
        }

        public ActionResult Test()
        {
            return View();
        }
        [HttpPost]

        public JsonResult _Save(string REQUESTS)
        {

            DateTime nowTime = DateTime.Now;
            JObject jo = new JObject();
            List<Cost> Costs = new List<Cost>();
            Costs = JsonConvert.DeserializeObject<List<Cost>>(REQUESTS);

            string msg = "";
            try

            {
                for (int i = 0; i < Costs.Count; i++)
                {
                    Cost c = new Cost();
                    string name = Costs[i].name;
                    string classification = Costs[i].classification;
                    string cost = Costs[i].cost;
                    //int ID = Costs[i].ID;
                    runResult r = c.ACost(name, classification, cost);
                    if (r.Count == 0)
                    {
                        msg = i + "儲存錯誤" + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("輸出訊息" + ex.Message);
            }
            jo.Add("msg", msg);
            return Json(jo.ToString(), JsonRequestBehavior.AllowGet);

        }
    }
}