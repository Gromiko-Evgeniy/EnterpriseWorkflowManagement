import {HubConnectionBuilder, HttpTransportType, LogLevel} from '@microsoft/signalr';

const fetchProjectWithTasks = async (processingMethod, setConnecion, jwt) => {
    try{
        const connection = new HubConnectionBuilder()
        .withUrl(
            'https://localhost:4000/project-groups',
            {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets
            }
        )
        .configureLogging(LogLevel.Information)
        .build()

        connection.on('ReceiveProjects', async (project) => {await processingMethod(project)})

        await connection.start()
        await connection.invoke('SendProjectsAsync', jwt)

        setConnecion(connection)
    }
    catch (ex) {
        console.log(ex)
    }
}

const updateProject = async (connection, project, jwt) => {
    await connection.invoke('UpdateProjectAsync', {...project, jwt: jwt})
}

const updateTask = async (connection, task, jwt) => {
    await connection.invoke('UpdateTaskAsync', {...task, jwt: jwt})
}

const cancelProject = async (connection, id, jwt) => {
    await connection.invoke('CancelProjectAsync', {id: id, jwt: jwt})
}

const cancelTask = async (connection, id, jwt) => {
    await connection.invoke('CancelTaskAsync', {id: id, jwt: jwt})
}

export {fetchProjectWithTasks, updateProject, cancelProject, updateTask, cancelTask}