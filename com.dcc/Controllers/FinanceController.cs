using com.dcc.BLL;
using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace com.dcc.Controllers
{
    [Authorize]
    public class FinanceController : Controller
    {
        
            
        // GET: Finance
        public ActionResult Index()
        {  
            return View();
        }

        public ActionResult DuesSetUp()
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            var _dues = FinanceBLL.GetDuesList;
            return View(_dues);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Due model)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
        
            bool status ;
            try{
                if (ModelState.IsValid)
                {
                    status = FinanceBLL.InsertPaymentType(model);
                    if (status)
                        return RedirectToAction("DuesSetUp");
                    else
                        return View();
                }
                else
                {
                    return View();
                }
            }catch{
                return  View();
            }
            
        }

        public ActionResult Edit(int id)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            var _dues = FinanceBLL.RetrieveSinglePayment(id);
            return View(_dues);
        }

        [HttpPost]
        public ActionResult Edit(Due model)
        {
            bool status;
            try
            {
                if (ModelState.IsValid)
                {
                    status = FinanceBLL.EditPaymentType(model);
                    if (status)
                        return RedirectToAction("DuesSetUp");
                    else
                        return View();
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }

        }

        public JsonResult DeleteDues(int id)
        {
            var _delete = FinanceBLL.DeleteDue(id);
            return Json(_delete, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MembersInDeptFin()
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            int recordCount = FinanceBLL.CheckIfPaymentRecordExistForNewMonth();
            if (recordCount == 0)
            {
                FinanceBLL.MoveDeptMembersRecordToPaymentTable();
            }
            ViewBag.Dept = ProcessDept.GetSingleDepartment(DeptId);
            var _member = ProcessUser.GetAllMemberInDept(DeptId);
            return View(_member);
        }

        public ActionResult DuesEntry(string MemberId)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            if (String.IsNullOrEmpty(MemberId))
            {
                return RedirectToAction("MembersInDeptFin");
            }
            var SingleUser = ProcessUser.GetSingleUser(MemberId);
            string Year = DateTime.Now.Year.ToString();
            ViewBag.PaymentRecord = FinanceBLL.GetDuesPaymentRecordList(PaymentType: Convert.ToString(2), Year: Year, MemberId: MemberId) ?? null;
            ViewBag.ErrorRes = TempData["ErrorRes"] != null ? TempData["ErrorRes"].ToString() : null;
            return View(SingleUser);
        }

        public ActionResult DuesPayment(string MemberId)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            ViewBag.DuesList = FinanceBLL.GetDuesList;
            ViewBag.MemberId = MemberId;
            ViewBag.Month = FinanceBLL.GetMonths;
            return PartialView();
        }

        

        public ActionResult InsertDuesPayment(Payment model, FormCollection collection)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            int PaymentMethod = Convert.ToInt32(collection["PaymentMethod"].ToString());
            int CheckRecs;
            string Years = DateTime.Now.Year.ToString();
            
            //check if record already exist
            if (model.DuesId == 4)
            {
                CheckRecs = FinanceBLL.CheckIfRecordExist(DueType: (int)model.DuesId, month: (int)model.Month, BelieverId: (int)model.MemberId, year: Years);
            }
            else
            {
                CheckRecs = FinanceBLL.CheckIfRecordExist(DueType: (int)model.DuesId,BelieverId: (int)model.MemberId);
            }

            //insert to database 
            if (CheckRecs == 0)
            {
                if (PaymentMethod == 1)
                {
                    string WalletBallance = FinanceBLL.GetWalletBalance(Convert.ToString(model.MemberId));
                    decimal _WalletBalance = (!string.IsNullOrWhiteSpace(WalletBallance))?Convert.ToDecimal(WalletBallance):0;
                    if (_WalletBalance < model.Amount)
                    {
                        TempData["ErrorRes"] = "Insufficient Funds in Wallet";
                        return RedirectToAction("DuesEntry", "Finance", new { MemberId = Convert.ToString(model.MemberId) });
                    }
                    else
                    {
                        FinanceBLL.TopUpWallet(Paymentmodel:model);
                    }
                }
                int outcome = FinanceBLL.InsertDuesPayment(model);
            }
            else
            {
                TempData["ErrorRes"] = "Record Already Exist";
            }
            return RedirectToAction("DuesEntry", "Finance", new { MemberId = Convert.ToString(model.MemberId) });
        }

        public ActionResult TopUpWallet(string MemberId)
        {
            ViewBag.MemberId = MemberId;
            return PartialView();
        }

        public ActionResult InsertToUpWallet(Wallet model)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            FinanceBLL.TopUpWallet(Walletmodel:model);
            return RedirectToAction("DuesEntry", "Finance", new { MemberId = Convert.ToString(model.MemberId) });
        }

        public ActionResult GetWalletBalance(string id)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            string WalletBallance = FinanceBLL.GetWalletBalance(id);
            return Json(WalletBallance, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAmount(int DuesId)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            var amount = FinanceBLL.GetAmountPerTransacton(DuesId);
            return Json(amount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaymentHistory(string MemberId)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            ViewBag.DuesList = FinanceBLL.GetDuesList;
            ViewBag.MemberId = MemberId;
            ViewBag.Month = FinanceBLL.GetMonths;
            return PartialView();
        }

        public ActionResult SearchPaymentRecord(FormCollection collection, Payment model)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            var SingleUser = ProcessUser.GetSingleUser(Convert.ToString(model.MemberId));
            string Year = DateTime.Now.Year.ToString();
            string StartDate = collection["StartDate"].ToString();
            string EndDate = collection["EndDate"].ToString();
            DateTime? _StartDate = CommonBLL.GetDateFromString(StartDate);
            DateTime? _EndDate = CommonBLL.GetDateFromString(EndDate);
            ViewBag.PaymentRecord = FinanceBLL.GetDuesPaymentRecordList(StartDate: _StartDate, EndDate: _EndDate, MemberId: Convert.ToString(model.MemberId),PaymentType:Convert.ToString(model.DuesId)) ?? null;
            
            return View("DuesEntry",SingleUser);
        }

        public ActionResult Expense()
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            ViewBag.PaymentRecord = FinanceBLL.GetExpenditureList();
            return View();
        }


        public ActionResult NewExpense()
        {
            return PartialView();
        }

        public ActionResult InsertExpense(Payment model)
        {
            model.Amount = -model.Amount;
            int outcome = FinanceBLL.InsertDuesPayment(model);
            return RedirectToAction("Expense");
        }

        public ActionResult ExpenseHistory()
        {
            return PartialView();
        }
        
        public ActionResult SearchExpenseRecord(FormCollection collection)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            string StartDate = collection["StartDate"].ToString();
            string EndDate = collection["EndDate"].ToString();
            DateTime? _StartDate = CommonBLL.GetDateFromString(StartDate);
            DateTime? _EndDate = CommonBLL.GetDateFromString(EndDate);
            ViewBag.PaymentRecord = FinanceBLL.GetExpenditureList(StartDate:_StartDate,EndDate:_EndDate);
            return View("Expense");
        }

        public ActionResult DeptBalance()
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            string WalletBallance = FinanceBLL.GetDeptBalance();
            return Json(WalletBallance, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report()
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId  = Convert.ToInt32(DeptId);
            var DueList = FinanceBLL.GetDuesList;
            ViewBag.DuesList = DueList.Where(m => m.DuesId != 4).ToList();
            ViewBag.Months = CommonBLL.GetAllMonths();
            return View();
        }

        public ActionResult SearchReport(FormCollection collection)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            string StartDate = collection["StartDate"].ToString();
            string EndDate = collection["EndDate"].ToString();
            int TransactionType = Convert.ToInt32(collection["TransactionType"].ToString());
            DateTime? _StartDate = CommonBLL.GetDateFromString(StartDate);
            DateTime? _EndDate = CommonBLL.GetDateFromString(EndDate);
            ViewBag.StartDate = _StartDate.Value.ToLongDateString();
            ViewBag.EndDate = _EndDate.Value.ToLongDateString();
            var SearchReport = FinanceBLL.SearchReport(TransactionType: TransactionType, StartDate: _StartDate, EndDate: _EndDate);
            return PartialView(SearchReport);
        }

        public ActionResult SearchDuesReport(FormCollection collection)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            string StartDate = collection["StartDate"].ToString();
            string EndDate = collection["EndDate"].ToString();
            int DuesId = Convert.ToInt32(collection["DuesId"].ToString());
            DateTime? _StartDate = CommonBLL.GetDateFromString(StartDate);
            DateTime? _EndDate = CommonBLL.GetDateFromString(EndDate);
            ViewBag.StartDate = _StartDate.Value.ToLongDateString();
            ViewBag.EndDate = _EndDate.Value.ToLongDateString();
            var SearchReport = FinanceBLL.DuesReport(DuesId: DuesId, StartDate: _StartDate, EndDate: _EndDate);
            return PartialView(SearchReport);
        }

        public ActionResult SearchMontlyDuesReport(FormCollection collection)
        {
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            int StartMonth = Convert.ToInt32(collection["StartMonth"].ToString());
            int EndMonth = Convert.ToInt32(collection["EndMonth"].ToString()); 
            string Year = collection["Year"].ToString();
            var MontlyDuesReport = FinanceBLL.SearchMontlyDuesReport(_StartMonth: StartMonth, _EndMonth: EndMonth, _Year: Year);
            return PartialView(MontlyDuesReport);
        }
    }
}