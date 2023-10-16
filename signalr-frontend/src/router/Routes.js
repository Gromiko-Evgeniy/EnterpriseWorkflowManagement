import LoginPage from "../pages/LoginPage/LoginPage"
import CustomerProjectsPage from "../pages/CustomerProjectsPage/CustomerProjectsPage"
import WorkerProjectPage from "../pages/WorkerProjectPage/WorkerProjectPage"

export const commonRoutes = [
    {path: '/login', element: <LoginPage/>},
    {path: '/*', element: <LoginPage/>},
]

export const workerRoutes = [
    {path: '/projects', element: <WorkerProjectPage/>}
]

export const customerRoutes = [
    {path: '/projects', element: <CustomerProjectsPage/>},  
]
