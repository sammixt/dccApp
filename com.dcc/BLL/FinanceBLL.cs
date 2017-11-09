using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using com.dcc.Models;
using com.dcc.ViewModel;

namespace com.dcc.BLL
{
    public class FinanceBLL
    {
        public static string DeptId;
        public static int _DeptId ;
        public static bool InsertPaymentType(Due model)
        {
            model.CreationDate = DateTime.Today;
            model.DeptId = _DeptId;
            try 
            {
                var context = new DCCEntities();
                context.Dues.Add(model);
                context.SaveChanges();
                context.Dispose();
                return true;
            }
            catch(Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return false;
            }
        }

        public static List<Due> GetDuesList
        {
            
            get
            {
                List<Due> _Dues = new List<Due>();
                try
                {
                    using (var context = new DCCEntities())
                    {
                        _Dues = context.Dues.Where(m => m.DeptId == _DeptId).ToList();
                        return _Dues;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                    return null;
                }
            }
        }

        public static string DeleteDue(int id)
        {
           
            try
                {
                    using (var context = new DCCEntities())
                    {
                        var query = context.Dues.Where(m => m.DuesId == id).FirstOrDefault();
                        if(query != null){
                            context.Dues.Remove(query);
                            context.SaveChanges();
                            return "success";
                        }else{
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                    return null;
                }
        }

        internal static Due RetrieveSinglePayment(int id)
        {
            Due _Dues = new Due();
            try
            {
                using (var context = new DCCEntities())
                {
                    _Dues = context.Dues.Where(m => m.DuesId == id).FirstOrDefault();
                    return _Dues;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        internal static bool EditPaymentType(Due model)
        {
            try
            {
                var context = new DCCEntities();
                var query = context.Dues.Where(m => m.DuesId == model.DuesId).FirstOrDefault();
                if (query != null)
                {
                    query.DuesName = model.DuesName;
                    query.DuesDesc = model.DuesDesc;
                    query.DuesType = model.DuesType;
                    query.Amount = model.Amount;
                    context.SaveChanges();
                    context.Dispose();
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return false;
            }
        }

        internal static decimal? GetAmountPerTransacton(int DuesId)
        {
            decimal? _Dues ;
            try
            {
                using (var context = new DCCEntities())
                {
                    _Dues = context.Dues.Where(m => m.DuesId == DuesId && m.DeptId == _DeptId).Select(m => m.Amount).FirstOrDefault();
                    return _Dues;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static  List<Month> GetMonths
        {
            get
            {
                List<Month> _Months = new List<Month>();
                try
                {
                    using (var context = new DCCEntities())
                    {
                        _Months = context.Months.OrderBy(m => m.MonthId).ToList();
                        return _Months;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                    return null;
                }
            }
        }

        internal static int InsertDuesPayment(Payment model)
        {
            int returnValue;
            model.Month = (model.Month == 0) ? DateTime.Now.Month : model.Month;
            model.EntryDate = DateTime.Today;
            model.DeptId = _DeptId;
            try
            {
                var context = new DCCEntities();
                if (model.DuesId == 4)
                {
                    var query = context.Payments.Where(m => m.MemberId == model.MemberId && m.DeptId == _DeptId && m.Month == model.Month && m.Year == model.Year && m.DuesId == model.DuesId).FirstOrDefault();
                    if (query != null)
                    {
                       query.Amount =  model.Amount;
                        query.EntryDate = model.EntryDate;
                        query.PaymentDate = model.PaymentDate;
                        query.PaymentSatus = model.PaymentSatus;
                        query.TransactionType = model.TransactionType;
                        context.SaveChanges();
                    }

                }
                else
                {
                    context.Payments.Add(model);
                    context.SaveChanges();
                   
                }
                context.Dispose();
                
                returnValue = (model.MemberId != null) ? (int)model.MemberId : 0;
                return returnValue;
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return 0;
            }
        }

        internal static PaymentViewModel GetDuesPaymentRecord(string PaymentId = null, string PaymentType = null, string Month = null, string Year = null, string MemberId = null)
        {
            PaymentViewModel _payment = new PaymentViewModel();
            var context = new DCCEntities();
            if (PaymentId != null)
            {
                int? _PaymentId = Convert.ToInt32(PaymentId);
                var query = context.Payments.Where(m => m.PaymentId == _PaymentId && m.DeptId == _DeptId).Include(m => m.Month1).DefaultIfEmpty().Include(m => m.Due).FirstOrDefault();
                if (query != null)
                {
                    _payment.DueType = query.Due.DuesName;
                    _payment.Amount = (decimal)query.Amount;
                    _payment.TransactionType = query.TransactionType;
                    _payment.Month = query.Month1.Month1 ?? "";
                    _payment.PaymentDate = (DateTime)query.PaymentDate;
                    _payment.PaymentStatus = query.PaymentSatus;
                    return _payment;
                }
                return null;
                
            }
            else
            {
                return null;
            }
            
        }


        internal static List<PaymentViewModel> GetDuesPaymentRecordList(string PaymentId = null, string PaymentType = null, string Month = null, string Year = null, string MemberId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            List<PaymentViewModel> _payment = new List<PaymentViewModel>();
            var context = new DCCEntities();
            
            if (PaymentType != null && Year != null && MemberId != null)
            {
                int? _PaymentType = Convert.ToInt32(PaymentType);
                int? _MemberId = Convert.ToInt32(MemberId);
                var query = context.Payments.Where(m => m.MemberId == _MemberId && m.DeptId == _DeptId).Include(m => m.Month1).DefaultIfEmpty().Include(m => m.Due).OrderBy(m => m.PaymentId).ToList();
                if (query[0] != null)
                {
                    foreach (var item in query)
                    {
                        _payment.Add(new PaymentViewModel
                        {
                            DueType = item.Due.DuesName,
                            Amount = (decimal)item.Amount,
                            TransactionType = item.TransactionType??null,
                            Month = item.Month1.Month1,
                            Year = item.Year??null,
                            PaymentDate = item.PaymentDate??null,
                            PaymentStatus = item.PaymentSatus

                        });
                    }
                    return _payment;
                }
                return null;

            }
            else if (StartDate != null && EndDate != null)
            {
                int? _PaymentType = Convert.ToInt32(PaymentType);
                int? _MemberId = Convert.ToInt32(MemberId);
                var query = context.Payments.Where(m => m.MemberId == _MemberId && m.DeptId == _DeptId && m.DuesId == _PaymentType && (m.PaymentDate >= StartDate && m.PaymentDate <= EndDate)).Include(m => m.Month1).DefaultIfEmpty().Include(m => m.Due).OrderBy(m => m.PaymentId).ToList();
                if (query[0] != null)
                {
                    foreach (var item in query)
                    {
                        _payment.Add(new PaymentViewModel
                        {
                            DueType = item.Due.DuesName,
                            Amount = (decimal)item.Amount,
                            TransactionType = item.TransactionType,
                            Month = item.Month1.Month1,
                            PaymentDate = (DateTime)item.PaymentDate,
                            PaymentStatus = item.PaymentSatus
                        });
                    }
                    return _payment;
                }
                return null;
            }
            else
            {
                return null;
            }

        }

        internal static int CheckIfRecordExist(int DueType = 0, int month = 0, int BelieverId = 0, string year = null)
        {
            int outcome;
            var context = new DCCEntities();
            if (DueType == 4)
            {
                outcome = context.Payments.Where(m => m.DuesId == DueType && m.Month == month && m.MemberId == BelieverId && m.Year == year && m.DeptId == _DeptId && m.PaymentSatus == "PAID").Count();
            }
            else
            {
                outcome = context.Payments.Where(m => m.DuesId == DueType && m.MemberId == BelieverId && m.DeptId == _DeptId).Count();
            }
            return outcome;
        }

        internal static void TopUpWallet(Wallet Walletmodel = null,Payment Paymentmodel = null)
        {
            var context = new DCCEntities();
            if (Paymentmodel != null)
            {
                Walletmodel = new Wallet();
                Walletmodel.MemberId = Paymentmodel.MemberId;
                Walletmodel.Amount = -Paymentmodel.Amount;
                Walletmodel.Description = Paymentmodel.Narration;
                Walletmodel.TransactionType = "Debit";
            }
            Walletmodel.DeptId = _DeptId;
            Walletmodel.TransactionDate = DateTime.Now;
            context.Wallets.Add(Walletmodel);
            context.SaveChanges();
            context.Dispose();
        }

        internal static string GetWalletBalance(string id)
        {
            int _id = Convert.ToInt32(id);
            var context = new DCCEntities();
            decimal? SumTotal = context.Wallets.Where(m => m.MemberId == _id && m.DeptId == _DeptId).Sum(m => m.Amount);
            return Convert.ToString(SumTotal);
        }

        internal static string GetDeptBalance()
        {
            
            var context = new DCCEntities();
            decimal? SumTotal = context.Payments.Where(m => m.DeptId == _DeptId).Sum(m => m.Amount);
            return Convert.ToString(SumTotal);
        }

        internal static List<Payment> GetExpenditureList(DateTime? StartDate = null, DateTime? EndDate = null)
        {
            List<Payment> _payment = new List<Payment>();
            var context = new DCCEntities();

            if (StartDate == null && EndDate == null)
            {
                var query = context.Payments.Where(m => m.DeptId == _DeptId && m.TransactionType == "Debit").OrderBy(m => m.PaymentId).ToList();
                if (query != null)
                {

                    return query;
                }
                return null;

            }
            else if (StartDate != null && EndDate != null)
            {

                var query = context.Payments.Where(m => m.DeptId == _DeptId && m.TransactionType == "Debit" && (m.PaymentDate >= StartDate && m.PaymentDate <= EndDate)).OrderBy(m => m.PaymentId).ToList();
                if (query != null)
                {

                    return query;
                }
                return null;
            }
            else
            {
                return null;
            }

        }

        internal static List<Payment> SearchReport(int TransactionType, DateTime? StartDate, DateTime? EndDate)
        {
            List<Payment> _payment = new List<Payment>();
            var context = new DCCEntities();

            if (TransactionType == 0)
            {
                _payment = context.Payments.Where(m => m.DeptId == _DeptId && (m.PaymentDate >= StartDate && m.PaymentDate <= EndDate)).Include(d => d.Due).DefaultIfEmpty().Include(m => m.Believer).DefaultIfEmpty().OrderBy(m => m.PaymentId).ToList();
                

            }
            else if (TransactionType == 1)
            {
                _payment = context.Payments.Where(m => m.DeptId == _DeptId && m.TransactionType == "Credit" && (m.PaymentDate >= StartDate && m.PaymentDate <= EndDate)).Include(d => d.Due).DefaultIfEmpty().Include(m => m.Believer).DefaultIfEmpty().OrderBy(m => m.PaymentId).ToList();
                
            }
            else if (TransactionType == 2)
            {
                _payment = context.Payments.Where(m => m.DeptId == _DeptId && m.TransactionType == "Debit" && (m.PaymentDate >= StartDate && m.PaymentDate <= EndDate)).Include(d => d.Due).DefaultIfEmpty().Include(m => m.Believer).DefaultIfEmpty().OrderBy(m => m.PaymentId).ToList();
                
            }
            else
            {
                return null;
            }
            if (_payment != null)
            {
                return _payment;
            }
            else
            {
                return null;
            }

        }

        internal static List<Payment> DuesReport(int DuesId, DateTime? StartDate, DateTime? EndDate)
        {
            List<Payment> _payment = new List<Payment>();
            var context = new DCCEntities();
            _payment = context.Payments.Where(m => m.DeptId == _DeptId && m.DuesId == DuesId && (m.PaymentDate >= StartDate && m.PaymentDate <= EndDate)).Include(d => d.Due).DefaultIfEmpty().Include(m => m.Believer).DefaultIfEmpty().Include(m => m.Month1).DefaultIfEmpty().OrderBy(m => m.PaymentId).ToList();
            if (_payment != null)
            {
                return _payment;
            }
            else
            {
                return null;
            }  
        }

        public static int CheckIfPaymentRecordExistForNewMonth()
        {
            string Year = DateTime.Now.Year.ToString();
            int Month = DateTime.Today.Month;
            var context = new DCCEntities();
            var CheckpaymentRecord = context.Payments.Where(m => m.DuesId == 4 && m.Month == Month && m.Year == Year && m.DeptId == _DeptId).Count();
            return CheckpaymentRecord;
        }

        public static void MoveDeptMembersRecordToPaymentTable()
        {
            var context = new DCCEntities();
            var memebers = ProcessUser.GetAllMemberInDept(Convert.ToString(_DeptId));
            string Year = DateTime.Now.Year.ToString();
            int Month = DateTime.Today.Month;
            Payment model = new Payment();
            foreach (var item in memebers)
            {
                model.Amount = 0;
                model.DeptId = _DeptId;
                model.DuesId = 4;
                model.EntryDate = null;
                model.MemberId = item.MemberId;
                model.Month = Month;
                model.Year = Year;
                model.PaymentSatus = "UNPAID";
                model.TransactionType = null;
                AddMonthlyPaymentRec(model);
            }
        }

        public static void AddMonthlyPaymentRec(Payment model)
        {
            var context = new DCCEntities();
            context.Payments.Add(model);
            context.SaveChanges();
            context.Dispose();
        }

        internal static object SearchMontlyDuesReport(int _StartMonth, int _EndMonth, string _Year)
        {
            var context = new DCCEntities();
            DuesReportViewModel _Report = new DuesReportViewModel();
            try
            {
                var query = context.Months.Where(m => m.MonthId >= _StartMonth && m.MonthId <= _EndMonth).ToList();
                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                        _Report._MONTHS.Add(new Month
                        {
                            Month1 = item.Month1,
                            MonthId = item.MonthId
                        });

                    }
                }

                var GetUserPaymentStatus = context.Payments.Include(m => m.Believer).Where(d => d.DeptId == _DeptId && d.Year == _Year).ToList();
                if (GetUserPaymentStatus.Count() > 0)
                {
                    _Report._PAYMENTS = GetUserPaymentStatus;
                }

                var GetMember = context.Members.Where(m => m.DeptId == _DeptId).ToList();
                if (GetMember.Count() > 0)
                {
                    _Report._MEMBERS = GetMember;
                }
                
            }
            catch(Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                _Report = null;
            }
            finally
            {
                context.Dispose();
            }
            return _Report;
        }
    }
}