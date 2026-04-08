import { AxiosError } from "axios";

interface ProblemDetails {
    type?: string;
    title: string;
    status: number;
    detail?: string;
    traceId?: string;
    errors?: Record<string, string[]>;
}

export class ApiError extends Error {
    status?: number;
    traceId?: string;

    constructor(message: string, status?: number, traceId?: string) {
        super(message);
        this.status = status;
        this.traceId = traceId;
    }
}

export function handleApiError(error: unknown): never {
    if (error instanceof AxiosError && error.response) {
        const data = error.response.data as ProblemDetails;

        if (data) {
            if (data.errors) {
                const messages = Object.values(data.errors)
                    .flat()
                    .join(", ");

                throw new ApiError(messages, data.status, data.traceId);
            }

            throw new ApiError(
                data.detail || data.title,
                data.status,
                data.traceId
            );
        }

        switch (error.response.status) {
            case 400:
                throw new ApiError("Bad Request", 400);
            case 401:
                throw new ApiError("Unauthorized", 401);
            case 403:
                throw new ApiError("Forbidden", 403);
            case 404:
                throw new ApiError("Not Found", 404);
            case 500:
                throw new ApiError("Internal Server Error", 500);
            default:
                throw new ApiError("Unknown error", error.response.status);
        }
    }

    throw new ApiError("Couldn't connect to the server");
}