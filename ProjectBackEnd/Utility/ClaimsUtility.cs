using Shared.Utility;
using System.Security.Claims;

namespace ProjectBackEnd.Utility;

public static class ClaimsUtility
{
    public static int ReadCurrentUserId(IEnumerable<Claim> claims)
    {
        int userId = Convert.ToInt32(claims.First(c => c.Type == "Id").Value);
        return userId;
    }

    public static string ReadCurrentUserRole(IEnumerable<Claim> claims)
    {
        string userRole = claims.First(c => c.Type == ClaimTypes.Role).Value;
        return userRole;
    }

    public static int ReadCurrentUserRole2(IEnumerable<Claim> claims)
    {
        string role = claims.First(claims => claims.Type == ClaimTypes.Role).Value;
        if (role == UserRole.Manager)
            return 1;
        else if (role == UserRole.Recepsionist)
            return 2;
        else
            return 3;
    }


    public static string ReadCurrentUserEmail(IEnumerable<Claim> claims)
    {
        string userEmail = claims.First(c => c.Type == "Email").Value;
        return userEmail;
    }
}