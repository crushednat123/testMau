using System;
using System.Collections.Generic;

namespace MauiAppWeb.Entities;

public partial class Movement
{
    public int Id { get; set; }

    public DateOnly? DateOfAcceptance { get; set; }

    public DateOnly? DateOfDismissal { get; set; }

    public int? IdUser { get; set; }

    public int? IdWorker { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual Worker? IdWorkerNavigation { get; set; }
}
