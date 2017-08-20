using System.Collections.Generic;
using System.Security.Claims;
using DataModel.Enities;

namespace Security
{
    public interface ISecurityContext
    {
        User User { get; }
        void Init(IEnumerable<Claim> claims);
    }
}