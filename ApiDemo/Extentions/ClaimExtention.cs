using System.Security.Claims;

namespace ApiDemo.Extentions
{
    public static class ClaimExtention
    {
        public static string RetriveEmailPrincipale(this ClaimsPrincipal user)
            =>user.FindFirstValue(ClaimTypes.Email);
    }
}
