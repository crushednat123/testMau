using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MauiAppWeb.Components.Pages
{
    public partial class Home
    {
        public LoginModel loginModel = new();
        [Inject]
        private NavigationManager Navigation { get; set; }
        public void HandleLogin()
        {
            if (loginModel.Email == "mizyak.k@mail.ru" && loginModel.Password == "123456")
            {
                Navigation.NavigateTo("/Business-trips");
            }
        }
    }
    public class LoginModel : IValidatableObject
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult("Введите почту", new[] { nameof(Email) });
            }
            else
            {
                var allowedDomains = new[] { ".ru", ".com", ".net" };
                if (!allowedDomains.Any(d => Email.EndsWith(d)) || Email.EndsWith("@"))
                {
                    yield return new ValidationResult("Домен должен быть .ru, .com или .net", new[] { nameof(Email) });
                }
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                yield return new ValidationResult("Введите пароль", new[] { nameof(Password) });
            }
            else if (Password.Length < 6)
            {
                yield return new ValidationResult("Пароль слишком короткий", new[] { nameof(Password) });
            }
        }
    }
}
