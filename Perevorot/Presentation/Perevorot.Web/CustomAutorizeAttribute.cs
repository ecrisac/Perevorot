using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;

namespace Perevorot.Web
{
    public class CustomAutorizeAttribute : AuthorizeAttribute
    {
        private readonly object _typeId = new object();

         private string _roles;
          private string[] _rolesSplit = new string[0];
          private string _users;
          private string[] _usersSplit = new string[0];
   
          public string Roles
          {
              get { return _roles ?? String.Empty; }
              set
              {
                  _roles = value;
                  _rolesSplit = SplitString(value);
              }
          }
   
          public override object TypeId
          {
              get { return _typeId; }
          }
   
          public string Users
          {
              get { return _users ?? String.Empty; }
              set
              {
                  _users = value;
                  _usersSplit = SplitString(value);
              }
          }
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
          if (httpContext == null)
               {
                   throw new ArgumentNullException("httpContext");
               }
    
               var user = httpContext.User;
               if (!user.Identity.IsAuthenticated)
               {
                   return false;
               }
    
               if (_usersSplit.Length > 0 && !_usersSplit.Contains(
                      user.Identity.Name, StringComparer.OrdinalIgnoreCase))
               {
                   return false;
               }
    
               if (_rolesSplit.Length > 0 && !_rolesSplit.Any(user.IsInRole))
               {
                   return false;
               }
    
               return true;
        }

         internal static string[] SplitString(string original)
          {
              if (String.IsNullOrEmpty(original))
              {
                  return new string[0];
              }
   
              var split = from piece in original.Split(',')
                          let trimmed = piece.Trim()
                          where !String.IsNullOrEmpty(trimmed)
                          select trimmed;
              return split.ToArray();
          }
    }
}