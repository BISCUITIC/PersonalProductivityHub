import { apiClient } from "./ApiClient";

export interface RegisterRequest {
    userName: string;
    email: string;
    password: string;
}

export interface AuthResponse {
    userName: string;
    email: string;
    createdAt: string;
}

export async function register(registerData: RegisterRequest): Promise<AuthResponse>{

    const response = await apiClient.post<AuthResponse>("/register", registerData);

    return response.data;
}
    
export async function login(userName: string, password: string): Promise<AuthResponse> {

    const response = await apiClient.post<AuthResponse>("/login", { userName, password });

    return response.data;
}

export async function getProfile() {

    const response = await apiClient.get<AuthResponse>("/profile");

    return response.data;
}
