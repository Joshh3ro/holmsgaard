using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Contracts.Dto;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Services;

public sealed class TimeCalculationService(
    ITimeRegistrationRepository timeRegistrationRepository,
    IEmployeeRepository employeeRepository,
    IActivityRepository activityRepository)
{
    public TimeRegistrationDto RegisterTime(RegisterEmployeeTimeCommand command)
    {
        if (employeeRepository.GetById(command.EmployeeId) is null)
        {
            throw new InvalidOperationException("Employee does not exist.");
        }

        if (activityRepository.GetById(command.ActivityId) is null)
        {
            throw new InvalidOperationException("Activity does not exist.");
        }

        var registration = new TimeRegistration(Guid.NewGuid(), command.EmployeeId, command.ActivityId, command.WorkDate, command.Hours, command.Note);
        timeRegistrationRepository.Add(registration);
        return registration.ToDto();
    }

    public EmployeeTimeSummaryDto? GetEmployeeTimeSummary(GetEmployeeTimeSummaryQuery query)
    {
        var employee = employeeRepository.GetById(query.EmployeeId);
        if (employee is null)
        {
            return null;
        }

        var totalHours = timeRegistrationRepository.GetByEmployeeId(query.EmployeeId).Sum(registration => registration.Hours);
        return new EmployeeTimeSummaryDto(employee.Id, employee.FullName, totalHours, employee.HourlyRate, totalHours * employee.HourlyRate);
    }
}
