using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WebApp.Engine.Security
{
    public class UserContext
    {
        private bool _isInitialized;
        private string _username;

        public string Username
        {
            get
            {
                CheckState();
                return _username;
            }
            private set => _username = value;
        }

        private void CheckState()
        {
            if (!_isInitialized)
                throw new NotSupportedException($"{nameof(UserContext)} is not initialized");
        }

        public void Init(IEnumerable<Claim> claims)
        {
            Username = claims.Single(c => c.Type == nameof(Username)).Value;

            _isInitialized = true;
        }
    }
}