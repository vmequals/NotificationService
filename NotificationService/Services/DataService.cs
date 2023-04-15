using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Data.DTOs;
using NotificationService.Models;
using NotificationService.Services.Interfaces;
using System.Linq;
using static System.Linq.Queryable;


namespace NotificationService.Services
{
    public class DataService : IDataService
    {
        private readonly NotificationDbContext _context;
        private readonly IMapper _mapper;

        public DataService(NotificationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Notification CRUD
        public async Task<NotificationDTO> CreateNotificationAsync(NotificationDTO notificationDto)
        {
            var notification = _mapper.Map<Notification>(notificationDto);
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return _mapper.Map<NotificationDTO>(notification);
        }

        public async Task<NotificationDTO> GetNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            return _mapper.Map<NotificationDTO>(notification);
        }

        // Subject CRUD
        public async Task<SubjectDTO> CreateSubjectAsync(SubjectDTO subjectDto)
        {
            var subject = _mapper.Map<Subject>(subjectDto);
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubjectDTO>(subject);
        }

        public async Task<SubjectDTO> GetSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            return _mapper.Map<SubjectDTO>(subject);
        }

        // Employee CRUD
        public async Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employeeDto)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            employee.TeamId = employeeDto.TeamId;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return _mapper.Map<EmployeeDTO>(employee);
        }


        public async Task<EmployeeDTO> GetEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            return _mapper.Map<EmployeeDTO>(employee);
        }

        // Team CRUD
        public async Task<TeamDTO> CreateTeamAsync(TeamDTO teamDto)
        {
            var team = _mapper.Map<Team>(teamDto);
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return _mapper.Map<TeamDTO>(team);
        }

        public async Task<TeamDTO> GetTeamAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            return _mapper.Map<TeamDTO>(team);
        }
        
        public async Task<(List<string> TeamEmails, List<string> EmployeeEmails)> GetEmailsByNotificationIdAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification == null)
            {
                return (new List<string>(), new List<string>());
            }

            var subject = await _context.Subjects.FindAsync(notification.SubjectId);

            if (subject == null)
            {
                return (new List<string>(), new List<string>());
            }

            var subjectWithTeams = await _context.Subjects
                .Include(s => s.SubjectTeams)
                .ThenInclude(st => st.Team)
                .SingleOrDefaultAsync(s => s.SubjectId == subject.SubjectId);

            var teamEmails = subjectWithTeams.SubjectTeams
                .Select(st => st.Team.Email)
                .Distinct()
                .ToList();

            var teamIds = subjectWithTeams.SubjectTeams
                .Select(st => st.TeamId)
                .ToList();

            var employeeEmails = await _context.Employees
                .Include(e => e.EmployeeTeams)
                .Where(e => e.EmployeeTeams.Any(te => teamIds.Contains(te.TeamId)))
                .Select(e => e.Email)
                .ToListAsync();

            return (teamEmails, employeeEmails);
        }

    }
}

