export function handleApiError(error: any): never {
    if (error.response) {
        switch (error.response.status) {
            case 400:
                throw new Error("Invalid request data");
            case 401:
                throw new Error("Invalid username or password");
            case 403:
                throw new Error("Access denied");
            case 404:
                throw new Error("Resource not found");
            case 500:
                throw new Error("Server error");
            default:
                throw new Error(error.response.data?.message || "An error has occurred");
        }
    }

    throw new Error("Couldn't connect to the server");
}