export interface ProjectResponse {
    id: string;
    name: string;
    description: string | null;
    createdAt: string;
}

export interface ProjectRequest {   
    name: string;
    description: string | null;    
}