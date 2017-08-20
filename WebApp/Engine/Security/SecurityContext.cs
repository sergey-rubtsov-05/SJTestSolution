using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DataAccess;
using DataModel.Enities;
using Security;

namespace WebApp.Engine.Security
{
    public class SecurityContext : ISecurityContext
    {
        private readonly IUnitOfWork _uow;
        private bool _isInitialized;
        private User _user;
        private string _username;

        public SecurityContext(IUnitOfWork uow)
        {
            _uow = uow;
        }

        private string Username
        {
            get
            {
                CheckState();
                return _username;
            }
            set => _username = value;
        }

        public User User
        {
            get
            {
                if (_user != null)
                    return _user;

                _user = _uow.Query<User>().SingleOrDefault(u => u.Name == Username);
                if (_user == null)
                {
                    _user = new User { Name = Username };
                    _uow.Add(_user);
                    _uow.SaveChanges();
                }

                return _user;
            }
        }

        public void Init(IEnumerable<Claim> claims)
        {
            Username = claims.Single(c => c.Type == Options.UsernameClaimType).Value;

            _isInitialized = true;
        }

        private void CheckState()
        {
            if (!_isInitialized)
                throw new NotSupportedException($"{nameof(SecurityContext)} is not initialized");
        }
    }
}