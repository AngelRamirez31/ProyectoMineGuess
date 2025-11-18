using System;

namespace MVP_ProyectoFinal.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
