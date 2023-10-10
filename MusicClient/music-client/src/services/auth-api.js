const localhost = "http://localhost:5268/";

export const loginUser = async (email, password) => {
    const response = await fetch(localhost + 'api/Auth/login', {
        method: 'POST',
        headers: {
            'Content-type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
    });
    
    if (response.ok) {
        const data = await response.json();
        console.log(data);
        return { success: true, data };
    } else {
        const errorText = await response.text();
        console.error('Error response:', errorText); // Log the error response
        return { success: false, error: `Error: ${errorText}` };
    }
    
};
