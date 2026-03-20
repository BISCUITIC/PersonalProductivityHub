interface ProblemDetails {
    type?: string;
    title: string;
    status: number;
    detail?: string;
    traceID?: string;
    errors?: Record<string, string[]>;
}

export function handleApiError(error: any): never {
    if (error.response) {
        const data = error.response.data as ProblemDetails;
       
        if (data && data.title) {            
            if (data.errors) {
                const messages = Object.entries(data.errors)
                                       .map( ([field, errs]) => errs.join(", "))
                                       .join(" ");
                throw new Error(messages);
            }
            
            throw new Error(data.detail || data.title);
        }
        
        switch (error.response.status) {
            case 400:
                throw new Error("Bad Request");
            case 401:
                throw new Error("Unauthorized");
            case 403:
                throw new Error("Forbidden");
            case 404:
                throw new Error("Not Found");
            case 500:
                throw new Error("Internal Server Error");
            default:
                throw new Error(error.response.data?.message || "An unknown error occurred");
        }
    }
    
    throw new Error("Couldn't connect to the server");
}