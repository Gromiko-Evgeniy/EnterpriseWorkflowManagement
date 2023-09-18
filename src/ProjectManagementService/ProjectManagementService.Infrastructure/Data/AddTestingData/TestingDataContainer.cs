using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Domain.Enumerations;

namespace HiringService.Infrastructure.Data.AddTestingData;

public static class TestingDataContainer
{
    public static async Task AddTestingData(ICustomerRepository customerRepository,
        IWorkerRepository workerRepository, IProjectTaskRepository taskRepository,
        IProjectRepository projectRepository)
    {
        //check if there is no data in database
        if (workerRepository.GetFirstAsync(_ => true) is not null) return;
        if (projectRepository.GetFirstAsync(_ => true) is not null) return;
        if (taskRepository.GetFirstAsync(_ => true) is not null) return;
        if (customerRepository.GetFirstAsync(_ => true) is not null) return;

        var testWorkers = await AddTestWorkers(workerRepository);
        var worker1 = testWorkers[0];
        var worker2 = testWorkers[1];
        var projectLeader = testWorkers[2];
        var departmentHead = testWorkers[3];

        var testCustomers = await AddTestCustomers(customerRepository);
        var customer1 = testCustomers[0];
        var customer2 = testCustomers[1];

        var testProjects = await CreateTestProjects(projectRepository,
            customer1, customer2, projectLeader);
        var project1 = testProjects[0];
        var project2 = testProjects[2];

        await AddTestTasksAndUpdateTestWorkers(taskRepository, workerRepository,
            project1, project2, worker1, worker2);
    }

    private static async Task<List<Customer>> AddTestCustomers(ICustomerRepository customerRepository)
    {
        var customer1 = new Customer()
        {
            Name = "Customer1",
            Email = "customer1@gmail.com",
        };

        var customer2 = new Customer()
        {
            Name = "Customer1",
            Email = "customer2@gmail.com",
        };

        var testCustomers = new List<Customer> { customer1, customer2 };
        await customerRepository.AddManyAsync(testCustomers);

        return testCustomers;
    }
    private static async Task<List<Worker>> AddTestWorkers(IWorkerRepository workerRepository)
    {
        var worker1 = new Worker()
        {
            Name = "worker1",
            Email = "worker1@gmail.com",
        };

        var worker2 = new Worker()
        {
            Name = "worker2",
            Email = "worker2@gmail.com",
        };

        var projectLeader = new Worker()
        {
            Name = "ProjectLeader",
            Email = "plead@gmail.com",
        };

        var departmentHead = new Worker()
        {
            Name = "DepartmentHead",
            Email = "dephead@gmail.com",
        };

        var testWorkers = new List<Worker> { worker1, worker2, projectLeader, departmentHead };
        await workerRepository.AddManyAsync(testWorkers);

        return testWorkers;
    }
    private static async Task<List<Project>> CreateTestProjects(IProjectRepository projectRepository,
        Customer customer1, Customer customer2, Worker projectLeader)
    {
        var project1 = new Project()
        {
            Objective = "create Project1",
            Description = "Project1",
            CustomerId = customer1.Id,
            LeadWorkerId = projectLeader.Id,
            Status = ProjectStatus.WaitingToStart
        };

        var project2 = new Project()
        {
            Objective = "create Project2",
            Description = "Project2",
            CustomerId = customer2.Id,
            LeadWorkerId = projectLeader.Id,
            Status = ProjectStatus.WaitingToStart
        };

        var testProjects = new List<Project> { project1, project2 };
        await projectRepository.AddManyAsync(testProjects);

        return testProjects;
    }

    private static async Task AddTestTasksAndUpdateTestWorkers(
        IProjectTaskRepository taskRepository, IWorkerRepository workerRepository,
        Project project1, Project project2, Worker worker1, Worker worker2)
    {
        var task1 = new ProjectTask()
        {
            Name = "Task 1 of Project 1",
            Description = "Task 1",
            ProjectId = project1.Id,
            Priority = Priority.High,
            Status = ProjectTaskStatus.ToDo,
            StartTime = new DateTime(2023, 9, 10, 13, 0, 0),
            FinishTime = new DateTime(2023, 9, 18, 13, 0, 0),
        };

        var task2 = new ProjectTask()
        {
            Name = "Task 2 of Project 1",
            Description = "Task 2",
            ProjectId = project1.Id,
            Priority = Priority.High,
            Status = ProjectTaskStatus.ToDo,
            StartTime = new DateTime(2023, 9, 19, 13, 0, 0),
        };

        var task3 = new ProjectTask()
        {
            Name = "Task 1 of Project 2",
            Description = "Task 3",
            ProjectId = project2.Id,
            Priority = Priority.High,
            Status = ProjectTaskStatus.ToDo,
            StartTime = new DateTime(2023, 9, 19, 13, 0, 0),
        };

        var testTasks = new List<ProjectTask> { task1, task2, task3 };
        await taskRepository.AddManyAsync(testTasks);

        await workerRepository.UpdateProjectAsync(worker1.Id, task2.ProjectId);
        await workerRepository.UpdateTaskAsync(worker1.Id, task2.Id);

        await workerRepository.UpdateProjectAsync(worker2.Id, task3.ProjectId);
        await workerRepository.UpdateTaskAsync(worker2.Id, task3.Id);
    }  
}
