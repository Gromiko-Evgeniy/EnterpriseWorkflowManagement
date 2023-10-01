import axios from 'axios';

const authorize = async (email, password, role) => {
    var controller = ''
    if (role === 0){
        controller = 'customers'
    }
    if (role === 1 || role === 2){
        controller = 'workers'
    }

    const response = await axios.post('https://localhost:2000/' + controller +  '/log-in', 
    {
        "email": email,
        "password": password,
    }, { withCredentials: true })
    if(response.data.success === false) console.log(response.data.message)
    return response.data
}
    
export default authorize
