using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using NLog;
using Perevorot.Domain.IRepositories;
using Perevorot.Domain.Models.DomainEntities;
using Perevorot.Web.ResourceLocator;
using WebMatrix.WebData;

namespace Perevorot.Web.Security
{
    public class EfMembershipProvider : ExtendedMembershipProvider
    {
        //
        // Global connection string, generated password length, generic exception message, event log info.
        //

        private const int NewPasswordLength = 8;
        private const string ExceptionMessage = "An exception occurred. Please check the Log.";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Lazy<ILoginRepository> loginRepository =
            new Lazy<ILoginRepository>(IoC.Resolve<ILoginRepository>);

        //
        // Used when determining encryption key values.
        //

        private MachineKeySection machineKey;

        //
        // If false, exceptions are thrown to the caller. If true,
        // exceptions are written to the event log.
        //

        //
        // System.Web.Security.MembershipProvider properties.
        //

        private string pApplicationName;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private int pMaxInvalidPasswordAttempts;
        private int pMinRequiredNonAlphanumericCharacters;
        private int pMinRequiredPasswordLength;
        private int pPasswordAttemptWindow;
        private MembershipPasswordFormat pPasswordFormat;
        private string pPasswordStrengthRegularExpression;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private bool pWriteExceptionsToEventLog;

        private ILoginRepository LoginRepository
        {
            get { return loginRepository.Value; }
        }

        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }

        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }


        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }


        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }


        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }


        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }


        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }


        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            //
            // Initialize values from web.config.
            //

            if (config == null)
                throw new ArgumentNullException("config");

            if (string.IsNullOrWhiteSpace(name))
                name = "EfMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Entity Framework Membership Provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            pApplicationName = GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            pMinRequiredNonAlphanumericCharacters =
                Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            pPasswordStrengthRegularExpression =
                Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));

            string tempFormat = config["passwordFormat"] ?? "Hashed";

            switch (tempFormat)
            {
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            // Get encryption and decryption key information from the configuration.
            //Configuration cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            //machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");
            machineKey = (MachineKeySection) WebConfigurationManager.GetSection("system.web/machineKey");

            if (machineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException(
                        "Hashed or Encrypted passwords are not supported with auto-generated keys.");
        }

        //
        // A helper function to retrieve config values from the configuration file.
        //

        private string GetConfigValue(string configValue, string defaultValue)
        {
            return String.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }


        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {
            if (!ValidateUser(username, oldPwd))
                return false;

            var args =
                new ValidatePasswordEventArgs(username, newPwd, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException(
                        "Change password canceled due to new password validation failure.");

            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    Member member = LoginRepository.GetAll<Member>()
                                                   .FirstOrDefault(m => m.User.UserName.Equals(username)
                                                                        &&
                                                                        m.Application.ApplicationName.Equals(
                                                                            pApplicationName));

                    if (member != null)
                    {
                        member.Password = EncodePassword(newPwd);
                        member.LastPasswordChangedDate = DateTime.Now;
                        LoginRepository.SaveOrUpdate(member);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePassword");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }
            return false;
        }

        //
        // MembershipProvider.ChangePasswordQuestionAndAnswer
        //

        public override bool ChangePasswordQuestionAndAnswer(string username,
                                                             string password,
                                                             string newPwdQuestion,
                                                             string newPwdAnswer)
        {
            if (!ValidateUser(username, password))
                return false;

            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    Member member =
                        LoginRepository.GetAll<Member>().FirstOrDefault(m => m.User.UserName.Equals(username)
                                                                             &&
                                                                             m.Application.ApplicationName.Equals(
                                                                                 pApplicationName));
                    if (member != null)
                    {
                        member.PasswordQuestion = newPwdQuestion;
                        member.PasswordAnswer = newPwdAnswer;
                        LoginRepository.SaveOrUpdate(member);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePasswordQuestionAndAnswer");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }

            return false;
        }

        //
        // MembershipProvider.CreateUser
        //

        public override MembershipUser CreateUser(string username,
                                                  string password,
                                                  string email,
                                                  string passwordQuestion,
                                                  string passwordAnswer,
                                                  bool isApproved,
                                                  object providerUserKey,
                                                  out MembershipCreateStatus status)
        {
            var args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            var u = GetUser(username, false);

            if (u == null)
            {
                var createDate = DateTime.Now;
                Application application;

                using (LoginRepository.CreateUnitOfWork())
                {
                    application = LoginRepository.GetAll<Application>().FirstOrDefault(x => x.ApplicationName.Equals(pApplicationName));
                }

                var user = new User(username)
                    {
                        LoweredUserName = username.ToLower(),
                        LastActivityDate = createDate,
                        Application = application
                    };

                var member = new Member
                    {
                        Password = EncodePassword(password),
                        Email = email,
                        PasswordQuestion = passwordQuestion,
                        PasswordAnswer = EncodePassword(passwordAnswer),
                        PasswordSalt = CreateSalt(),
                        IsApproved = isApproved,
                        Comment = string.Empty,
                        CreateDate = createDate,
                        LastPasswordChangedDate = createDate,
                        IsLockedOut = false,
                        FailedPasswordAttemptCount = 0,
                        FailedPasswordAttemptWindowStart = createDate,
                        FailedPasswordAnswerAttemptCount = 0,
                        FailedPasswordAnswerAttemptWindowStart = createDate,
                        Application = application,
                        LastLockoutDate = SqlDateTime.MinValue.Value,
                        LastLoginDate = SqlDateTime.MinValue.Value,
                        User = user
                    };

                try
                {
                    using (var uow = LoginRepository.CreateUnitOfWork())
                    {
                        LoginRepository.SaveOrUpdate(member);
                        uow.Commit();
                        status = MembershipCreateStatus.Success;
                    }
                }
                catch (Exception e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "CreateUser");
                    }
                    status = MembershipCreateStatus.ProviderError;
                }
                return GetUser(username, false);
            }
            status = MembershipCreateStatus.DuplicateUserName;
            return null;
        }

        //
        // MembershipProvider.DeleteUser
        //

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            try
            {
                using (var uow = LoginRepository.CreateUnitOfWork())
                {
                    var member =
                        LoginRepository.GetAll<Member>().FirstOrDefault(m => m.User.UserName.Equals(username) &&
                                                                             m.Application.ApplicationName.Equals(pApplicationName));

                    if (member != null)
                    {
                        var userProfiles = LoginRepository.GetAll<Profile>()
                                                                    .Where(p => p.User.Equals(member.User))
                                                                    .ToList();

                        userProfiles.ForEach(p => LoginRepository.Delete(p));
                        LoginRepository.SaveOrUpdate(member);
                        uow.Commit();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteUser");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }
            return false;
        }

        //
        // MembershipProvider.GetAllUsers
        //

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var startIndex = pageSize*pageIndex;
            var users = new MembershipUserCollection();

            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    var members = LoginRepository.GetAll<Member>()
                                                                .Where(m => m.Application.ApplicationName.Equals(ApplicationName))
                                                                .OrderBy(m => m.User.UserName)
                                                                .Skip(startIndex)
                                                                .Take(pageSize);
                    totalRecords = members.Count();
                    foreach (var member in members)
                    {
                        var user = new EfMembershipUser(
                            Name,
                            member.User.UserName,
                            member.User.Id,
                            member.Email,
                            member.PasswordQuestion,
                            member.Comment,
                            member.IsApproved,
                            member.IsLockedOut,
                            member.CreateDate,
                            member.LastLoginDate,
                            member.User.LastActivityDate,
                            member.LastPasswordChangedDate,
                            member.LastLockoutDate)
                            {
                                Id = member.User.Id,
                                FirstName = member.User.FirstName,
                                LastName = member.User.LastName
                            };
                        users.Add(user);
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllUsers ");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }
            return users;
        }

        //
        // MembershipProvider.GetNumberOfUsersOnline
        //

        public override int GetNumberOfUsersOnline()
        {
            var onlineSpan = new TimeSpan(0, Membership.UserIsOnlineTimeWindow, 0);
            var compareTime = DateTime.Now.Subtract(onlineSpan);
            int numOnline;

            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    numOnline = LoginRepository.GetAll<Member>().Count(m => m.User.LastActivityDate > compareTime
                                                                            &&
                                                                            m.Application.ApplicationName.Equals(
                                                                                pApplicationName));
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetNumberOfUsersOnline");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }

            return numOnline;
        }

        //
        // MembershipProvider.GetPassword
        //

        public override string GetPassword(string username, string answer)
        {
            string password;
            string passwordAnswer;

            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }

            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    Member member =
                        LoginRepository.GetAll<Member>().FirstOrDefault(m => m.User.UserName.Equals(username) &&
                                                                             m.Application.ApplicationName.Equals(
                                                                                 pApplicationName));

                    if (member != null)
                    {
                        if (member.IsLockedOut)
                            throw new MembershipPasswordException("The supplied user is locked out.");
                        password = member.Password;
                        passwordAnswer = member.PasswordAnswer;
                    }
                    else
                    {
                        throw new MembershipPasswordException("The supplied user name is not found.");
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetPassword");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new MembershipPasswordException("Incorrect password answer.");
            }


            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(password);
            }

            return password;
        }

        //
        // MembershipProvider.GetUser(string, bool)
        //

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            EfMembershipUser user = null;

            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    Member member =
                        LoginRepository.GetAll<Member>().FirstOrDefault(m => m.User.UserName.Equals(username)
                                                                             &&
                                                                             m.Application.ApplicationName.Equals(
                                                                                 pApplicationName));

                    if (member != null)
                    {
                        user = new EfMembershipUser(
                            Name,
                            member.User.UserName,
                            member.User.Id,
                            member.Email,
                            member.PasswordQuestion,
                            member.Comment,
                            member.IsApproved,
                            member.IsLockedOut,
                            member.CreateDate,
                            member.LastLoginDate,
                            member.User.LastActivityDate,
                            member.LastPasswordChangedDate,
                            member.LastLockoutDate)
                            {
                                Id = member.User.Id,
                                FirstName = member.User.FirstName,
                                LastName = member.User.LastName
                            };

                        if (userIsOnline)
                        {
                            member.User.LastActivityDate = DateTime.Now;
                            LoginRepository.SaveOrUpdate(member);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(String, Boolean)");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }

            return user;
        }

        //
        // MembershipProvider.GetUser(object, bool)
        //

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            EfMembershipUser user = null;

            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    Member member =
                        LoginRepository.GetAll<Member>().FirstOrDefault(m => m.User.Id.Equals((long) providerUserKey)
                                                                             &&
                                                                             m.Application.ApplicationName.Equals(
                                                                                 pApplicationName));

                    if (member != null)
                    {
                        user = new EfMembershipUser(
                            Name,
                            member.User.UserName,
                            member.User.Id,
                            member.Email,
                            member.PasswordQuestion,
                            member.Comment,
                            member.IsApproved,
                            member.IsLockedOut,
                            member.CreateDate,
                            member.LastLoginDate,
                            member.User.LastActivityDate,
                            member.LastPasswordChangedDate,
                            member.LastLockoutDate)
                            {
                                Id = member.User.Id,
                                FirstName = member.User.FirstName,
                                LastName = member.User.LastName
                            };

                        if (userIsOnline)
                        {
                            member.User.LastActivityDate = DateTime.Now;
                            LoginRepository.SaveOrUpdate(member);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(String, Boolean)");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }

            return user;
        }

        //
        // MembershipProvider.UnlockUser
        //

        public override bool UnlockUser(string username)
        {
            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    Member member =
                        LoginRepository.GetAll<Member>().FirstOrDefault(m => m.User.UserName.Equals(username)
                                                                             &&
                                                                             m.Application.ApplicationName.Equals(
                                                                                 pApplicationName));
                    if (member != null)
                    {
                        member.IsLockedOut = false;
                        member.LastLockoutDate = DateTime.Now;
                        LoginRepository.SaveOrUpdate(member);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UnlockUser");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }
            return false;
        }

        //
        // MembershipProvider.GetUserNameByEmail
        //

        public override string GetUserNameByEmail(string email)
        {
            string username = string.Empty;
            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    Member member =
                        LoginRepository.GetAll<Member>()
                                       .FirstOrDefault(
                                           m =>
                                           m.Email.Equals(email) &&
                                           m.Application.ApplicationName.Equals(pApplicationName));
                    if (member != null)
                    {
                        username = member.User.UserName;
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUserNameByEmail");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }

            return username;
        }

        //
        // MembershipProvider.ResetPassword
        //

        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException("Password reset is not enabled.");
            }

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new ProviderException("Password answer required for password reset.");
            }

            string newPassword = Membership.GeneratePassword(NewPasswordLength, MinRequiredNonAlphanumericCharacters);

            var args =
                new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");

            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    Member member =
                        LoginRepository.GetAll<Member>().FirstOrDefault(m => m.User.UserName.Equals(username)
                                                                             &&
                                                                             m.Application.ApplicationName.Equals(
                                                                                 pApplicationName));
                    string passwordAnswer;
                    if (member != null)
                    {
                        if (member.IsLockedOut)
                            throw new MembershipPasswordException("The supplied user is locked out.");

                        passwordAnswer = member.PasswordAnswer;
                    }
                    else
                    {
                        throw new MembershipPasswordException("The supplied user name is not found.");
                    }

                    if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
                    {
                        UpdateFailureCount(username, "passwordAnswer");

                        throw new MembershipPasswordException("Incorrect password answer.");
                    }

                    member.Password = EncodePassword(newPassword);
                    member.LastPasswordChangedDate = DateTime.Now;
                    LoginRepository.SaveOrUpdate(member);
                    return newPassword;
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ResetPassword");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }
        }

        //
        // MembershipProvider.UpdateUser
        //

        public override void UpdateUser(MembershipUser user)
        {
            var efUser = (EfMembershipUser) user;
            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    Member member =
                        LoginRepository.GetAll<Member>()
                                       .FirstOrDefault(m => m.User.Id.Equals((long) efUser.ProviderUserKey));
                    if (member != null)
                    {
                        member.Comment = efUser.Comment;
                        member.Email = efUser.Email;
                        member.User.FirstName = efUser.FirstName;
                        member.IsApproved = efUser.IsApproved;
                        member.User.LastName = efUser.LastName;
                        LoginRepository.SaveOrUpdate(member);
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateUser");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }
        }
      
        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;

            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    Member member =
                        LoginRepository.GetAll<Member>().FirstOrDefault(m => m.User.UserName.Equals(username)
                                                                             &&
                                                                             m.Application.ApplicationName.Equals(
                                                                                 pApplicationName)
                                                                             && m.IsLockedOut == false);
                    if (member != null)
                    {
                        member.LastLoginDate = DateTime.Now;
                        if (member.IsApproved)
                        {
                            if (CheckPassword(password, member.Password))
                            {
                                isValid = true;
                                LoginRepository.SaveOrUpdate(member);
                            }
                            else
                            {
                                UpdateFailureCount(username, "password");
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ValidateUser");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }
            return isValid;
        }


        //
        // UpdateFailureCount
        //   A helper method that performs the checks and updates associated with
        // password failure tracking.
        //

        private void UpdateFailureCount(string username, string failureType)
        {
            var windowStart = new DateTime();
            var failureCount = 0;

            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    var member =
                        LoginRepository.GetAll<Member>().FirstOrDefault(m => m.User.UserName.Equals(username) &&
                                                                             m.Application.ApplicationName.Equals(pApplicationName));
                    if (member != null)
                    {
                        if (failureType == "password")
                        {
                            failureCount = member.FailedPasswordAttemptCount;
                            windowStart = member.FailedPasswordAttemptWindowStart;
                        }

                        if (failureType == "passwordAnswer")
                        {
                            failureCount = member.FailedPasswordAnswerAttemptCount;
                            windowStart = member.FailedPasswordAttemptWindowStart;
                        }

                        DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                        if (failureCount == 0 || DateTime.Now > windowEnd)
                        {
                            // First password failure or outside of PasswordAttemptWindow. 
                            // Start a new password failure count from 1 and a new window starting now.

                            if (failureType == "password")
                            {
                                member.FailedPasswordAttemptCount = 1;
                                member.FailedPasswordAttemptWindowStart = DateTime.Now;
                            }

                            if (failureType == "passwordAnswer")
                            {
                                member.FailedPasswordAnswerAttemptCount = 1;
                                member.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
                            }
                        }
                        else
                        {
                            if (failureCount++ >= MaxInvalidPasswordAttempts)
                            {
                                // Password attempts have exceeded the failure threshold. Lock out
                                // the user.

                                member.IsLockedOut = true;
                                member.LastLockoutDate = DateTime.Now;
                            }
                            else
                            {
                                if (failureType == "password")
                                    member.FailedPasswordAttemptCount = failureCount;

                                if (failureType == "passwordAnswer")
                                    member.FailedPasswordAnswerAttemptCount = failureCount;
                            }
                        }
                        LoginRepository.SaveOrUpdate(member);
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateFailureCount");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }
        }

        //
        // CheckPassword
        //   Compares password values based on the MembershipPasswordFormat.
        //
        private bool CheckPassword(string password, string dbpassword)
        {
            var pass1 = password;
            var pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
            }

            return pass1 == pass2;
        }


        //
        // EncodePassword
        //   Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        //

        private string EncodePassword(string password)
        {
            var encodedPassword = password;

            if (!string.IsNullOrEmpty(password))
            {
                switch (PasswordFormat)
                {
                    case MembershipPasswordFormat.Clear:
                        break;
                    case MembershipPasswordFormat.Encrypted:
                        encodedPassword =
                            Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                        break;
                    case MembershipPasswordFormat.Hashed:
                        var hash = new HMACSHA1 {Key = HexToByte(machineKey.ValidationKey)};
                        encodedPassword =
                            Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                        break;
                    default:
                        throw new ProviderException("Unsupported password format.");
                }
            }

            return encodedPassword;
        }


        //
        // UnEncodePassword
        //   Decrypts or leaves the password clear based on the PasswordFormat.
        //

        private string UnEncodePassword(string encodedPassword)
        {
            var password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                        Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        //
        // HexToByte
        //   Converts a hexadecimal string to a byte array. Used to convert encryption
        // key values from the configuration.
        //

        private byte[] HexToByte(string hexString)
        {
            var returnBytes = new byte[hexString.Length/2];
            for (var i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i*2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        ///     Creates a random 128 password salt
        /// </summary>
        /// <returns></returns>
        private static string CreateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }


        //
        // MembershipProvider.FindUsersByName
        //

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            var startIndex = pageSize*pageIndex;
            var users = new MembershipUserCollection();
            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    IQueryable<Member> members = LoginRepository.GetAll<Member>()
                                                                .Where(
                                                                    m =>
                                                                    m.User.UserName.StartsWith(
                                                                        usernameToMatch.Replace("%", ""))
                                                                    &&
                                                                    m.Application.ApplicationName.Equals(
                                                                        pApplicationName))
                                                                .OrderBy(m => m.User.UserName)
                                                                .Skip(startIndex)
                                                                .Take(pageSize);

                    totalRecords = members.Count();

                    foreach (var m in members.ToList())
                    {
                        var user = new EfMembershipUser(
                            Name,
                            m.User.UserName,
                            m.User.Id,
                            m.Email,
                            m.PasswordQuestion,
                            m.Comment,
                            m.IsApproved,
                            m.IsLockedOut,
                            m.CreateDate,
                            m.LastLoginDate,
                            m.User.LastActivityDate,
                            m.LastPasswordChangedDate,
                            m.LastLockoutDate)
                            {
                                Id = m.User.Id,
                                FirstName = m.User.FirstName,
                                LastName = m.User.LastName
                            };
                        users.Add(user);
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByName");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }
            return users;
        }

        //
        // MembershipProvider.FindUsersByEmail
        //

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                                  out int totalRecords)
        {
            var startIndex = pageSize*pageIndex;
            var users = new MembershipUserCollection();
            try
            {
                using (LoginRepository.CreateUnitOfWork())
                {
                    var members = LoginRepository.GetAll<Member>()
                                                                .Where(m => m.Email.Contains(emailToMatch)
                                                                            &&
                                                                            m.Application.ApplicationName.Equals(
                                                                                pApplicationName))
                                                                .OrderBy(m => m.User.UserName)
                                                                .Skip(startIndex)
                                                                .Take(pageSize);

                    totalRecords = members.Count();

                    foreach (var m in members.ToList())
                    {
                        var user = new EfMembershipUser(
                            Name,
                            m.User.UserName,
                            m.User.Id,
                            m.Email,
                            m.PasswordQuestion,
                            m.Comment,
                            m.IsApproved,
                            m.IsLockedOut,
                            m.CreateDate,
                            m.LastLoginDate,
                            m.User.LastActivityDate,
                            m.LastPasswordChangedDate,
                            m.LastLockoutDate)
                            {
                                Id = m.User.Id,
                                FirstName = m.User.FirstName,
                                LastName = m.User.LastName
                            };
                        users.Add(user);
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByEmail");

                    throw new ProviderException(ExceptionMessage);
                }
                throw;
            }
            return users;
        }

        //
        // WriteToEventLog
        //   A helper function that writes exception detail to the event log. Exceptions
        // are written to the event log as a security measure to avoid private database
        // details from being returned to the browser. If a method does not return a status
        // or boolean indicating the action succeeded or failed, a generic exception is also 
        // thrown by the caller.
        //

        private void WriteToEventLog(Exception e, string action)
        {
            var message = action + ": An exception occurred communicating with the data source.\n\n";
            Logger.LogException(LogLevel.Error,message, e);
        }

        public override ICollection<OAuthAccountData> GetAccountsForUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override string CreateUserAndAccount(string userName, string password, bool requireConfirmation,
                                                    IDictionary<string, object> values)
        {
            throw new NotImplementedException();
        }

        public override string CreateAccount(string userName, string password, bool requireConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override bool ConfirmAccount(string userName, string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override bool ConfirmAccount(string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAccount(string userName)
        {
            throw new NotImplementedException();
        }

        public override string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow)
        {
            throw new NotImplementedException();
        }

        public override int GetUserIdFromPasswordResetToken(string token)
        {
            throw new NotImplementedException();
        }

        public override bool IsConfirmed(string userName)
        {
            throw new NotImplementedException();
        }

        public override bool ResetPasswordWithToken(string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override int GetPasswordFailuresSinceLastSuccess(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetCreateDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetPasswordChangedDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastPasswordFailureDate(string userName)
        {
            throw new NotImplementedException();

        }
    }
}