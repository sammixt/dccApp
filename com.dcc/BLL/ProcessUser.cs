using com.dcc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace com.dcc.BLL
{
    public class ProcessUser
    {
        public static string InsertUser(Believer model)
        {
            string MemberId;
            try
            {
                using (var context = new DCCEntities())
                {
                    context.Believers.Add(model);
                    context.SaveChanges();
                    var id = model.MemberId;
                    MemberId = Convert.ToString(id);
                    return MemberId;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static Believer GetSingleUser(string MemberId)
        {
            int _MemeberId = Convert.ToInt32(MemberId);
            Believer member = new Believer();
            try
            {
                using (var context = new DCCEntities())
                {
                    member = context.Believers.Where(m => m.MemberId == _MemeberId).FirstOrDefault();
                    return member;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static List<Believer> GetAllMembers()
        {
            List<Believer> member = new List<Believer>();
            try
            {
                using (var context = new DCCEntities())
                {
                    member = context.Believers.ToList();
                    return member;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        internal static string UpdateUser(Believer model)
        {
            string MemberId;
            try
            {
                using (var context = new DCCEntities())
                {
                    var query = context.Believers.Where(m => m.MemberId == model.MemberId).FirstOrDefault();
                    if (query != null)
                    {
                        query.FirstName = model.FirstName;
                        query.LastName = model.LastName;
                        query.Sex = model.Sex;
                        query.PhoneNumber = model.PhoneNumber;
                        query.StateName = model.StateName;
                        query.AltPhoneNumber = model.AltPhoneNumber;
                        query.MaritalStatus = model.MaritalStatus;
                        query.TwitterHandle = model.TwitterHandle;
                        query.InstagramHandle = model.InstagramHandle;
                        query.FacebookName = model.FacebookName;
                        query.HomeAddressOne = model.HomeAddressOne;
                        query.HomeAddressTwo = model.HomeAddressTwo;
                        query.City = model.City;
                        query.Country = model.Country;
                        query.DateOfBirth = model.DateOfBirth;
                        query.Email = model.Email;
                        query.WeddingAnniversary = model.WeddingAnniversary;
                        context.SaveChanges();
                        MemberId = Convert.ToString(model.MemberId);
                        return MemberId;
                    }
                    return null;
                    
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        internal static string DeleteSingleUser(string MemberId)
        {
            DCCEntities _Believer = new DCCEntities();
            decimal _Member = Convert.ToDecimal(MemberId);
            var member = _Believer.Believers.Where(m => m.MemberId == _Member).FirstOrDefault();
            if (member != null)
            {
                _Believer.Believers.Remove(member);
                _Believer.SaveChanges();
                return "success";
            }
            else
            {
                return null;
            }
        }

        #region member
        public static string InsertMember(Member model)
        {
            string MemberId;
            try
            {
                using (var context = new DCCEntities())
                {
                    context.Members.Add(model);
                    context.SaveChanges();
                    var id = model.MemberId;
                    MemberId = Convert.ToString(id);
                    return MemberId;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        
        public static int CountUserDept(string _MemberId)
        {
            int count = 0;
            int _memberId = Convert.ToInt32(_MemberId);
            try
            {
                using (var context = new DCCEntities())
                {
                    count = context.Members.Where(b => b.BelieverId == _memberId).Count();
                }
                return count;
            }
            catch(Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return count;
            }
        }

        public static int CountUserUnit(string _MemberId, string _DeptId)
        {
            int count = 0;
            int _memberId = Convert.ToInt32(_MemberId);
            int _deptId = Convert.ToInt32(_DeptId);
            
            try
            {
                using (var context = new DCCEntities())
                {
                    count = context.Members.Where(b => b.BelieverId == _memberId && (b.DeptId == _deptId && b.UnitId != null)).Count();
                }
                return count;
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return count;
            }
        }

        public static List<Member> GetMemberDetails(string _MemberId)
        {
            int _memberId = Convert.ToInt32(_MemberId);
            try
            {
                var context = new DCCEntities();
                
                    var query = context.Members.Include(p => p.Department).Include(d => d.Unit).DefaultIfEmpty().Where(m => m.BelieverId == _memberId).ToList();
                    return query;
                    
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        public static string DeleteSingleMember(string MemberId)
        {
            DCCEntities Member = new DCCEntities();
            decimal _memberId = Convert.ToDecimal(MemberId);
            var member = Member.Members.Where(m => m.MemberId == _memberId).FirstOrDefault();
            if (member != null)
            {
                Member.Members.Remove(member);
                Member.SaveChanges();
                return "success";
            }
            else
            {
                return null;
            }
        }

        public static string DeleteSingleMemberFromUnit(string MemberId)
        {
            DCCEntities Member = new DCCEntities();
            decimal _memberId = Convert.ToDecimal(MemberId);
            var member = Member.Members.Where(m => m.MemberId == _memberId).FirstOrDefault();
            if (member != null)
            {
                member.UnitId = null;
                Member.SaveChanges();
                return "success";
            }
            else
            {
                return null;
            }
        }

        public static List<Believer> GetAllMemberInDept(string DeptId)
        {
            DCCEntities Member = new DCCEntities();
            List<Believer> _believe = new List<Believer>();
            decimal _deptId = Convert.ToDecimal(DeptId);
            var member = Member.Members.Where(m => m.DeptId == _deptId).ToList();
            if (member != null)
            {
                foreach (var item in member)
                {
                    var getBeliever = Member.Believers.Where(m => m.MemberId == item.BelieverId).FirstOrDefault();
                    _believe.Add(new Believer
                    {
                        FirstName = getBeliever.FirstName,
                        LastName = getBeliever.LastName,
                        Sex = getBeliever.Sex,
                        PhoneNumber = getBeliever.PhoneNumber,
                        DateOfBirth = getBeliever.DateOfBirth,
                        MemberId = getBeliever.MemberId

                    });
                }
                return _believe;
            }
            else
            {
                return null;
            }
        }


        public static List<Member> GetAllMemberInDeptForAttendance(string DeptId, string Group = null)
        {
            DCCEntities Member = new DCCEntities();
            List<Member> member = new List<Member>();
            decimal _deptId = Convert.ToDecimal(DeptId);
            if(Group != null){
                member = Member.Members.Where(m => m.DeptId == _deptId && m.Groups == Group).ToList();
            }
            else
            {
                member = Member.Members.Where(m => m.DeptId == _deptId).ToList();
            }
            if (member != null)
            {
                return member;
            }
            else
            {
                return null; 
            }
           
        }

        public static IEnumerable<Member> GetAllMemberDetails()
        {
            
            try
            {
                var context = new DCCEntities();
                
                    var query = context.Members.Include(p => p.Department).Include(d => d.Unit).DefaultIfEmpty().ToList();
                    IEnumerable<Member> queries = query.GroupBy(p => p.BelieverId).Select(group => group.First());
                    return queries;
                    
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }
        #endregion

        #region units

        public static List<Believer> GetAllMemberInUnit(string UnitId,string DeptId)
        {
            DCCEntities Member = new DCCEntities();
            List<Believer> _believe = new List<Believer>();
            decimal _unitId = Convert.ToDecimal(UnitId);
            decimal _deptId = Convert.ToDecimal(DeptId);
            var member = Member.Members.Where(m => m.UnitId == _unitId && m.DeptId == _deptId).ToList();
            if (member != null)
            {
                foreach (var item in member)
                {
                    var getBeliever = Member.Believers.Where(m => m.MemberId == item.BelieverId).FirstOrDefault();
                    _believe.Add(new Believer
                    {
                        FirstName = getBeliever.FirstName,
                        LastName = getBeliever.LastName,
                        Sex = getBeliever.Sex,
                        PhoneNumber = getBeliever.PhoneNumber,
                        DateOfBirth = getBeliever.DateOfBirth,
                        MemberId = getBeliever.MemberId

                    });
                }
                return _believe;
            }
            else
            {
                return null;
            }
        }

        //
        public static Member GetSingleMemberInUnit(string MemberId,string DeptId)
        {
            int _MemberId = Convert.ToInt32(MemberId);
            int _DeptId = Convert.ToInt32(DeptId);
            try
            {
                var context = new DCCEntities();

                var query = context.Members.Include(p => p.Department).Include(d => d.Unit).DefaultIfEmpty().Where(m => m.DeptId == _DeptId && m.BelieverId == _MemberId ).FirstOrDefault();
               
                return query;

            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }

        internal static string AssignUserToUnit(Member model)
        {
            string MemberId;
            try
            {
                using (var context = new DCCEntities())
                {
                    var query = context.Members.Where(m => m.MemberId == model.MemberId).FirstOrDefault();
                    if (query != null)
                    {
                        query.Groups = model.Groups;
                        query.Post = model.Post;
                        query.UnitId = model.UnitId;
                        query.Status = model.Status;
                        query.ProbationStatus = model.ProbationStatus;
                        context.SaveChanges();
                        MemberId = Convert.ToString(model.BelieverId);
                        return MemberId;
                    }
                    return null;

                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return null;
            }
        }
        #endregion

        internal static bool CheckUserLoginAccountExist(UsersAccount Data)
        {
            try
            {
                using (var context = new DCCEntities())
                {
                    var query = context.UsersAccounts.Where(m => m.BelieverId == Data.BelieverId && m.DeptId == Data.DeptId).Count();
                    if (query > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return true;
            }
        }

        internal static bool CheckIfUserNameExist(UsersAccount Data)
        {
            try
            {
                using (var context = new DCCEntities())
                {
                    var query = context.UsersAccounts.Where(m => m.UserName == Data.UserName).Count();
                    if (query > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return true;
            }
        }

        internal static void CreateUserLogin(UsersAccount Data)
        {
            try
            {
                Data.CreationDate = DateTime.Now;
                
                using (var context = new DCCEntities())
                {
                    context.UsersAccounts.Add(Data);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
            }
        }
    }
}