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
    public IReadOnlyCollection<TimeRegistrationDto> GetTimeRegistrations(Guid? employeeId = null)
    {
        var registrations = employeeId.HasValue
            ? timeRegistrationRepository.GetByEmployeeId(employeeId.Value)
            : timeRegistrationRepository.GetAll();

        return registrations
            .OrderByDescending(registration => registration.WorkDate)
            .ThenBy(registration => registration.Id)
            .Select(registration => registration.ToDto())
            .ToArray();
    }

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

    public TimeRegistrationDto? UpdateTimeRegistration(UpdateTimeRegistrationCommand command)
    {
        var registration = timeRegistrationRepository.GetById(command.Id);
        if (registration is null)
        {
            return null;
        }

        if (employeeRepository.GetById(command.EmployeeId) is null)
        {
            throw new InvalidOperationException("Employee does not exist.");
        }

        if (activityRepository.GetById(command.ActivityId) is null)
        {
            throw new InvalidOperationException("Activity does not exist.");
        }

        registration.Update(command.EmployeeId, command.ActivityId, command.WorkDate, command.Hours, command.Note);
        timeRegistrationRepository.Update(registration);
        timeRegistrationRepository.SaveChanges();
        return registration.ToDto();
    }

    public bool DeleteTimeRegistration(Guid id)
    {
        if (timeRegistrationRepository.GetById(id) is null)
        {
            return false;
        }

        timeRegistrationRepository.Delete(id);
        return true;
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
