import React from "react";
import type { ReactElement } from "react";
import { Navigate } from "react-router-dom";


interface PrivateRouteProps {
    element: ReactElement;           
    isAuthenticated: boolean;      
    loading: boolean;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ element, isAuthenticated, loading }) => {
    if (loading) return <div>Loading...</div>;
    return isAuthenticated ? element : <Navigate to="/login" replace />;
};

export default PrivateRoute;