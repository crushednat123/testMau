using MauiAppWeb.Entities;
using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MauiAppWeb.Components.Pages
{
    public partial class UserCard
    {
        [Parameter]
        public int Id { get; set; }

        private Worker? worker;

        [Inject]
        private HrDbContext DbContext { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;


        protected override async Task OnParametersSetAsync()
        {
            worker = await DbContext.Workers
                .Include(w => w.Vacations)
                .Include(w => w.Movements)
                .FirstOrDefaultAsync(w => w.Id == Id);
           
        }

        // Можно вынести сюда же вспомогательные методы
        private string GetFullName(Worker worker) =>
            string.Join(" ", new[] { worker.SurName, worker.Name, worker.Patronymic }
                .Where(p => !string.IsNullOrEmpty(p))) ?? "Не указано";

        private string FormatSalary(decimal? salary) =>
            salary?.ToString("C2", new System.Globalization.CultureInfo("ru-RU")) ?? "Не указана";

        private string FormatDate(DateOnly? date) =>
            date?.ToString("dd.MM.yyyy") ?? "Не указана";

        private string GetEmploymentStatus(Worker worker)
        {
            var lastMovement = worker.Movements?.OrderByDescending(m => m.DateOfAcceptance).FirstOrDefault();
            if (lastMovement == null) return "unknown";
            return lastMovement.DateOfDismissal.HasValue ? "dismissed" : "employed";
        }

        private string GetEmploymentStatusText(Worker worker) =>
            GetEmploymentStatus(worker) switch
            {
                "employed" => "Работает",
                "dismissed" => "Уволен",
                _ => "Неизвестно"
            };

        private string GetVacationInfo(Worker worker)
        {
            var vacation = worker.Vacations?.FirstOrDefault();
            if (vacation?.VacationDate == null) return "Нет отпуска";
            var endDate = vacation.VacationEndDate;
            var isActive = endDate == null || endDate >= DateOnly.FromDateTime(DateTime.Today);
            return isActive ? $"В отпуске до {FormatDate(endDate)}" : $"Отпуск до {FormatDate(endDate)}";
        }

        private string GetBusinessTripInfo(Worker worker)
        {
            var vacation = worker.Vacations?.FirstOrDefault();
            if (vacation?.DateOfBusinessTrip == null) return "Нет командировки";
            var endDate = vacation.BusinessTripEndDate;
            var isActive = endDate == null || endDate >= DateOnly.FromDateTime(DateTime.Today);
            return isActive ? $"В командировке до {FormatDate(endDate)}" : $"Командировка до {FormatDate(endDate)}";
        }

        private string GetSickLeaveInfo(Worker worker)
        {
            var vacation = worker.Vacations?.FirstOrDefault();
            if (vacation?.SickLeaveStartDate == null) return "Нет больничного";
            var endDate = vacation.SickLeaveEndDate;
            var isActive = endDate == null || endDate >= DateOnly.FromDateTime(DateTime.Today);
            return isActive ? $"На больничном до {FormatDate(endDate)}" : $"Больничный до {FormatDate(endDate)}";
        }

        private async Task SaveChanges()
        {
            if (worker != null)
            {
                DbContext.Workers.Update(worker);
                await DbContext.SaveChangesAsync();
                Navigation.NavigateTo("/employees");
            }
        }

        private void CancelEdit()
        {
            Navigation.NavigateTo("/employees");
        }
    }
}
