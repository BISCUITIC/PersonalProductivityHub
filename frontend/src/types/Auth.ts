export interface RegisterRequest {
    userName: string;
    email: string;
    password: string;
}

export interface LoginRequest {
    userName: string;
    password: string;
}

export interface AuthResponse {
    userName: string;
    email: string;
    createdAt: string;
}