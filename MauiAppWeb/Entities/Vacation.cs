using System;
using System.Collections.Generic;

namespace MauiAppWeb.Entities;

public partial class Vacation
{
    public int Id { get; set; }

    public int? IdUser { get; set; }

    public int? IdWorkes { get; set; }

    public DateOnly? VacationDate { get; set; }

    public DateOnly? VacationEndDate { get; set; }

    public DateOnly? DateOfBusinessTrip { get; set; }

    public DateOnly? BusinessTripEndDate { get; set; }

    public DateOnly? SickLeaveStartDate { get; set; }

    public DateOnly? SickLeaveEndDate { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual Worker? IdWorkesNavigation { get; set; }
}
