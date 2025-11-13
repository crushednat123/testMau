using MauiAppWeb.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppWeb.Components.Pages
{
    public partial class Employees
    {
        private List<Worker> employees = new();
        private bool isLoading;

        [Inject]
        private HrDbContext DbContext { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadEmployees();
        }

        private async Task LoadEmployees()
        {
            isLoading = true;

            try
            {
                employees = await DbContext.Workers
                    .Include(w => w.Vacations)
                    .Include(w => w.Movements)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex}");
            }
            finally
            {
                isLoading = false;
            }
        }

        private string GetFullName(Worker worker)
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(worker.SurName)) parts.Add(worker.SurName);
            if (!string.IsNullOrEmpty(worker.Name)) parts.Add(worker.Name);
            if (!string.IsNullOrEmpty(worker.Patronymic)) parts.Add(worker.Patronymic);

            return parts.Any() ? string.Join(" ", parts) : "Не указано";
        }

        private string GetInitials(string fullName)
        {
            if (string.IsNullOrEmpty(fullName) || fullName == "Не указано")
                return "??";

            var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
                return $"{parts[ 0 ][ 0 ]}{parts[ 1 ][ 0 ]}".ToUpper();
            else if (parts.Length == 1)
                return parts[ 0 ].Length >= 2 ? parts[ 0 ].Substring(0, 2).ToUpper() : parts[ 0 ].ToUpper();
            else
                return "??";
        }

        private string FormatSalary(decimal? salary)
        {
            return salary?.ToString("C2", new System.Globalization.CultureInfo("ru-RU")) ?? "Не указана";
        }

        private string FormatDate(DateOnly? date)
        {
            return date?.ToString("dd.MM.yyyy") ?? "Не указана";
        }

        private string GetEmploymentStatus(Worker worker)
        {
            var lastMovement = worker.Movements?.OrderByDescending(m => m.DateOfAcceptance).FirstOrDefault();
            if (lastMovement == null) return "unknown";

            return lastMovement.DateOfDismissal.HasValue ? "dismissed" : "employed";
        }

        private string GetEmploymentStatusText(Worker worker)
        {
            return GetEmploymentStatus(worker) switch
            {
                "employed" => "Работает",
                "dismissed" => "Уволен",
                _ => "Неизвестно"
            };
        }

        private string GetVacationInfo(Worker worker)
        {
            var vacation = worker.Vacations?.FirstOrDefault();
            if (vacation?.VacationDate == null) return "Нет отпуска";

            var endDate = vacation.VacationEndDate;
            var isActive = endDate == null || endDate >= DateOnly.FromDateTime(DateTime.Today);

            return isActive
                ? $"В отпуске до {FormatDate(endDate)}"
                : $"Отпуск до {FormatDate(endDate)}";
        }

        private string GetBusinessTripInfo(Worker worker)
        {
            var vacation = worker.Vacations?.FirstOrDefault();
            if (vacation?.DateOfBusinessTrip == null) return "Нет командировки";

            var endDate = vacation.BusinessTripEndDate;
            var isActive = endDate == null || endDate >= DateOnly.FromDateTime(DateTime.Today);

            return isActive
                ? $"В командировке до {FormatDate(endDate)}"
                : $"Командировка до {FormatDate(endDate)}";
        }

        private string GetSickLeaveInfo(Worker worker)
        {
            var vacation = worker.Vacations?.FirstOrDefault();
            if (vacation?.SickLeaveStartDate == null) return "Нет больничного";

            var endDate = vacation.SickLeaveEndDate;
            var isActive = endDate == null || endDate >= DateOnly.FromDateTime(DateTime.Today);

            return isActive
                ? $"На больничном до {FormatDate(endDate)}"
                : $"Больничный до {FormatDate(endDate)}";
        }

        private decimal GetTotalSalary()
        {
            return employees.Where(w => w.Salary.HasValue).Sum(w => w.Salary.Value);
        }

        private decimal GetFilteredTotalSalary()
        {
            return FilteredEmployees.Where(w => w.Salary.HasValue).Sum(w => w.Salary!.Value);
        }

        private string searchQuery = string.Empty;

        private IEnumerable<Worker> FilteredEmployees =>
            employees.Where(e =>
                string.IsNullOrWhiteSpace(searchQuery) ||
                GetFullName(e).Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                (e.Post?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false)
            );

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private void OpenUserCard(Worker worker)
        {
            
            Navigation.NavigateTo($"/UserCard/{worker.Id}");
        }

    }
}
