import { type ReactNode, useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import { getProfile } from "../api/AuthApi";

interface ProtectedRouteProps {
    children: ReactNode;
}

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
    const [loading, setLoading] = useState(true);
    const [authorized, setAuthorized] = useState(false);

    useEffect(() => {
        async function checkAuth() {
            try {
                await getProfile(); // גûחמג /profile
                setAuthorized(true);
            } catch {
                setAuthorized(false);
            } finally {
                setLoading(false);
            }
        }
        checkAuth();
    }, []);

    if (loading) return <div>Loading...</div>;
    if (!authorized) return <Navigate to="/login" replace />;

    return <>{children}</>;
}