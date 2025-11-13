using System;
using System.Collections.Generic;

namespace MauiAppWeb.Entities;

public partial class Worker
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? SurName { get; set; }

    public string? Patronymic { get; set; }

    public string? Post { get; set; }

    public decimal? Salary { get; set; }

    public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();

    public virtual ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();
}
