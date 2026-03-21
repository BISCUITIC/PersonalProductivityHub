import { apiClient } from "./ApiClient";
import type { RegisterRequest, LoginRequest, AuthResponse } from "../types/Auth"
import { handleApiError } from "../api/HandleApiError"

export async function register(registerData: RegisterRequest): Promise<AuthResponse>{

    try {
        const response = await apiClient.post<AuthResponse>("/auth/register", registerData);
       
        return response.data;
    }
    catch (error: any) {
        handleApiError(error);        
    }
}
    
export async function login(loginData: LoginRequest): Promise<AuthResponse> {

    try {
        const response = await apiClient.post<AuthResponse>("/auth/login", loginData);

        return response.data;
    }
    catch (error: any) {
        handleApiError(error);
    }
}

export async function logout(): Promise<AuthResponse> {

    try {
        const response = await apiClient.post<AuthResponse>("/auth/logout");

        return response.data;
    }
    catch (error: any) {
        handleApiError(error);
    }
}

export async function me() : Promise<AuthResponse> {
    try {
        const response = await apiClient.get<AuthResponse>("/auth/me");
        
        return response.data;
    }
    catch (error: any) {
        handleApiError(error);
    }
}