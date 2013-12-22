using System;
using Perevorot.Domain.IRepositories;
using Perevorot.Domain.IServices.DomainInterfaces;
using Perevorot.Domain.Models.DomainEntities;
using Perevorot.Domain.Models.Exceptions;

namespace Perevorot.Domain.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;


        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public User GetUserByLoginData(string username, string password)
        {
            using (_loginRepository.CreateUnitOfWork())
            {
                var user = _loginRepository.GetUserByUserNameAndPassword(username, password);

                if (user == null)
                    throw new FailedLoginException("User not found.");

                if (!user.IsActive)
                    throw new FailedLoginException("User is disabled.");

                if (user.Password != password)
                    throw new FailedLoginException("Wrong password.");

                user.LastLogin = DateTime.Now;
                _loginRepository.SaveOrUpdate(user);
                return user;
            }
        }
    }
}