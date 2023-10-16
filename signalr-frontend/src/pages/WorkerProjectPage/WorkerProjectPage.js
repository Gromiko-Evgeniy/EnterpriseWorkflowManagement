import React, {useContext, useState, useEffect} from 'react';
import styles from './WorkerProjectPage.module.css'
import ProjectAndTasksInfo from '../../components/ProjectWithTaskList/ProjectAndTasksInfo';
import { fetchProjectWithTasks } from '../../fetching/SignalRInteraction';
import Navbar from '../../components/Navbar/Navbar';
import { UserContext } from '../../context/UserContext';

const SingleProjectPage = () => {

    const {user, setUser} = useContext(UserContext);
    const [project, setProject] = useState({tasks: []})
    const [error, setError] = useState(null)

    useEffect(()=>{
        fetchProjectWithTasks(processProjectWithTasks, _ => {},  user.jwt)
    }, [])

    const processProjectWithTasks = (newProject) => {
        console.log(newProject); 

        if (typeof newProject === 'string') {
            console.log(newProject); 
            console.log('string'); 
            setError(error => newProject)
            setProject(project => null)
        }
        else{
            console.log(newProject); 
            console.log('project'); 
            setProject(project => newProject)
            setError(error => null)
        }
    }
    
    return (
        <div className={styles.page}>
            <Navbar/>

            {!error ? 
                <div className={styles.projectContent}>
                    <ProjectAndTasksInfo
                        project={project}
                        tasks={project.tasks}
                    />
                </div>
                :
                <div>{error}</div>
            }   
        </div>
    );
};

export default SingleProjectPage;
