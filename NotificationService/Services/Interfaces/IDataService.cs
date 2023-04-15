
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationService.Data.DTOs;
using NotificationService.Models;

namespace NotificationService.Services.Interfaces
{
    public interface IDataService
    {
        // Notification CRUD
        Task<NotificationDTO> CreateNotificationAsync(NotificationDTO notification);
        Task<NotificationDTO> GetNotificationAsync(int id);

        // Subject CRUD
        Task<SubjectDTO> CreateSubjectAsync(SubjectDTO subject);
        Task<SubjectDTO> GetSubjectAsync(int id);

        // Employee CRUD
        Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employee);
        Task<EmployeeDTO> GetEmployeeAsync(int id);

        // Team CRUD
        Task<TeamDTO> CreateTeamAsync(TeamDTO team);
        Task<TeamDTO> GetTeamAsync(int id);
        
        Task<(List<string> TeamEmails, List<string> EmployeeEmails)> GetEmailsByNotificationIdAsync(int notificationId);

    }
}
