using System;
using System.Web.Security;

namespace Perevorot.Web
{
    //[Table("aspnet_UsersInRoles")]
    //public class UserRoles
    //{
    //    [Key]
    //    [Column(Order = 1)]
    //    public Guid UserId { get; set; }
    //    [Key]
    //    [Column(Order = 2)]
    //    public Guid RoleId { get; set; }
    //}


    public class EfMembershipUser : MembershipUser
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public EfMembershipUser(
            string providername,
            string username,
            object providerUserKey,
            string email,
            string passwordQuestion,
            string comment,
            bool isApproved,
            bool isLockedOut,
            DateTime creationDate,
            DateTime lastLoginDate,
            DateTime lastActivityDate,
            DateTime lastPasswordChangedDate,
            DateTime lastLockedOutDate) :
            base(
                providername,
                username,
                providerUserKey,
                email,
                passwordQuestion,
                comment,
                isApproved,
                isLockedOut,
                creationDate,
                lastLoginDate,
                lastActivityDate,
                lastPasswordChangedDate,
                lastLockedOutDate) { }

        public EfMembershipUser() { }

       

        //public static User GetUserById(int Id)
        //{
        //     using (LoginRepository.CreateUnitOfWork())
        //    {
        //        return context.UserData
        //            .Where(u => u.Id.Equals(Id))
        //            .FirstOrDefault();
        //    }
        //}

        //public List<EfMembershipUser> Paged(int maximumRows, int startRowIndex, string search, string filter)
        //{
        //    EfMembershipUser user = new EfMembershipUser();
        //    List<EfMembershipUser> users = new List<EfMembershipUser>();
        //     using (LoginRepository.CreateUnitOfWork())
        //    {
        //        List<Member> members = new List<Member>();
        //        if (!string.IsNullOrEmpty(search))
        //        {
        //            members = context.MembershipData
        //                .Where(m => m.User.UserName.Contains(search))
        //                .OrderBy(m => m.User.UserName)
        //                .Skip(startRowIndex)
        //                .Take(maximumRows)
        //                .ToList();
        //        }
        //        else if (!string.IsNullOrEmpty(filter))
        //        {
        //            members = context.MembershipData
        //                .Where(m => m.User.UserName.StartsWith(filter))
        //                .OrderBy(m => m.User.UserName)
        //                .Skip(startRowIndex)
        //                .Take(maximumRows)
        //                .ToList();
        //        }
        //        else
        //        {
        //            members = context.MembershipData
        //               .OrderBy(m => m.User.UserName)
        //               .Skip(startRowIndex)
        //               .Take(maximumRows)
        //               .ToList();
        //        }
        //        foreach (Member member in members)
        //        {
        //            user = new EfMembershipUser("EfMembershipProvider",
        //                member.User.UserName,
        //                member.User.UserId,
        //                member.Email,
        //                member.PasswordQuestion,
        //                member.Comment,
        //                member.IsApproved,
        //                member.IsLockedOut,
        //                member.CreateDate,
        //                member.LastLoginDate,
        //                member.User.LastActivityDate,
        //                member.LastPasswordChangedDate,
        //                member.LastLockoutDate);
        //            user.Id = member.User.Id;
        //            user.FirstName = member.User.FirstName;
        //            user.LastName = member.User.LastName;
        //            users.Add(user);
        //        }
        //    }
        //    return users;
        //}

        //public int Count(string search, string filter)
        //{
        //     using (LoginRepository.CreateUnitOfWork())
        //    {
        //        if (!string.IsNullOrEmpty(search))
        //        {
        //            return context.UserData
        //                .Where(u => u.UserName.Contains(search))
        //                .Count();
        //        }
        //        else if (!string.IsNullOrEmpty(filter))
        //        {
        //            return context.UserData
        //                .Where(u => u.UserName.StartsWith(filter))
        //                .Count();
        //        }
        //        else
        //        {
        //            return context.UserData
        //                .Count();
        //        }
        //    }
        //}
    }
}
