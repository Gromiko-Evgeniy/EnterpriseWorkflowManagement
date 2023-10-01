using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Abstractions.ServiceAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Domain.Enumerations;
using ProjectManagementService.Application.DTOs;

namespace ProjectManagementService.Application.Hubs;

public class ProjectsHub : Hub
{
    private readonly IMapper _mapper;
    private readonly IWorkerRepository _workerRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IJWTExtractorService _JWTExtractorService;
    private const string receiveProjectsMethodName = "ReceiveProjects";

    public ProjectsHub(IMapper mapper, IWorkerRepository workerRepository,
        IProjectRepository projectRepository, IProjectTaskRepository taskRepository,
        ICustomerRepository customerRepository, IJWTExtractorService JWTExtractorService)
    {
        _JWTExtractorService = JWTExtractorService;
        _mapper = mapper;

        _workerRepository = workerRepository;
        _customerRepository = customerRepository;
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
    }

    public async Task SendProjectsAsync(string JWT)
    {
        if (!TryExtractEmailAndRoleFromJWT(JWT, out string email, out ApplicationRole role))
        {
            await BadUserResponceAsync();
            return;
        }

        if (role == ApplicationRole.Worker || role == ApplicationRole.ProjectLeader)
        {
            await SendProjectToWorkerAsync(email);
        }
        if (role == ApplicationRole.Customer)
        {
            await SendProjectsToCustomerAsync(email);
        }
    }

    public async Task UpdateProjectAsync(UpdateProjectDTO update)
    {
        await CheckCustomerAsync(update.JWT);

        var project = await _projectRepository.GetByIdAsync(update.Id);

        if (project is null)
        {
            await _projectRepository.UpdateObjectiveAsync(project.Id, update.Objective);
            await _projectRepository.UpdateDescriptionAsync(project.Id, update.Description);

            await SendProjectsAsync(update.JWT);
        }
    }

    public async Task CancelProjectAsync(JWTAndEmailDTO update)
    {
        await CheckCustomerAsync(update.JWT);

        var project = await _projectRepository.GetByIdAsync(update.Id);
        if (project is not null)
        {
            await _projectRepository.CancelAsync(project.Id);

            await SendProjectsAsync(update.JWT);
        }
    }

    public async Task UpdateTaskAsync(UpdateTaskDTO update)
    {
        await CheckCustomerAsync(update.JWT);

        var task = await _taskRepository.GetByIdAsync(update.Id);

        if (task is not null)
        {
            await _taskRepository.UpdateNameAsync(task.Id, update.Name);
            await _taskRepository.UpdateDescriptionAsync(task.Id, update.Description);

            await SendProjectsAsync(update.JWT);
        }
    }

    public async Task CancelTaskAsync(UpdateTaskDTO update)
    {
        await CheckCustomerAsync(update.JWT);

        var task = await _taskRepository.GetByIdAsync(update.Id);

        if (task is not null)
        {
            await _taskRepository.CancelAsync(task.Id);
            await SendProjectsAsync(update.JWT);

            await SendProjectsAsync(update.JWT);
        }
    }

    private async Task SendProjectToWorkerAsync(string email)
    {
        var worker = await _workerRepository.GetFirstAsync(worker => worker.Email == email);
        if (worker is null)
        {
            await BadUserResponceAsync();
            return; 
        }

        var workerTask = await _taskRepository.GetByWorkerIdAsync(worker.Id);

        if(workerTask is null)
        {
            await noProjectsResponceAsync();
            return;
        }

        var projectDTO = await GetProjectWithTaskListDTO(workerTask.ProjectId);

        await SendData(workerTask.ProjectId, projectDTO);
    }

    private async Task SendProjectsToCustomerAsync(string email)
    {
        var customer = await _customerRepository.GetFirstAsync(worker => worker.Email == email);
        if (customer is null)
        {
            await BadUserResponceAsync();
            return; 
        }

        var projects = await _projectRepository.GetAllCustomerProjectsAsync(customer.Id);

        foreach (var project in projects)
        {
            var projectDTO = await GetProjectWithTaskListDTO(project.Id);

            await SendData(project.Id, projectDTO);
        }        
    }

    private async Task SendData<T>(string groupName, T data )
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName)
                .SendAsync(receiveProjectsMethodName, data);
    }

    private async Task BadUserResponceAsync()
    {
        await SendData("BadUserGroup", "Your data is out of date, try to log in in again");
    }

    private async Task noProjectsResponceAsync()
    {
        await SendData("NoProjectGroup", "You have no projecs now");
    }

    private async Task<ProjectWithTaskListDTO> GetProjectWithTaskListDTO(string projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);

        var projectDTO = _mapper.Map<ProjectWithTaskListDTO>(project);

        var projectTasks = await _taskRepository.GetByProjectIdAsync(project!.Id);

        var taskDTOs = new List<TaskWithWorkerEmailDTO>();

        foreach (var task in projectTasks) 
        {
            var taskDTO = _mapper.Map<TaskWithWorkerEmailDTO>(task);

            try
            {
                var worker = await _workerRepository.GetByIdAsync(task.WorkerId);
                taskDTO.WorkerEmail = worker.Email;
            }
            catch 
            {
                taskDTO.WorkerEmail = "";
            }

            taskDTOs.Add(taskDTO);
        }

        projectDTO.Tasks = taskDTOs;

        return projectDTO;
    }

    private async Task CheckCustomerAsync(string JWT)
    {
        if (!TryExtractEmailAndRoleFromJWT(JWT, out string email, out ApplicationRole role))
        {
            await BadUserResponceAsync();
            return;
        }

        if (role != ApplicationRole.Customer)
        {
            await BadUserResponceAsync();
            return;
        }

        var customer = await _customerRepository.GetFirstAsync(worker => worker.Email == email);
        if (customer is null)
        {
            await BadUserResponceAsync();
            return;
        }
    }

    private bool TryExtractEmailAndRoleFromJWT(string JWT, out string email, out ApplicationRole role)
    {
        try
        {
            email = _JWTExtractorService.ExtractClaimFromJWT(JWT, "email");
            var roleString = _JWTExtractorService.ExtractClaimFromJWT(JWT, "role");
            role = (ApplicationRole)Enum.Parse(typeof(ApplicationRole), roleString, true);
        }
        catch
        {
            email = null;
            role = 0;

            return false;
        }

        return true;
    }
}
