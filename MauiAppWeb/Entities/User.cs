using System;
using System.Collections.Generic;

namespace MauiAppWeb.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Named { get; set; }

    public string? SurName { get; set; }

    public string? Patronymic { get; set; }

    public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();

    public virtual ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();
}
