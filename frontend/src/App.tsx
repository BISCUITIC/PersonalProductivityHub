import { Routes, Route } from "react-router-dom";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";
import PrivateRoute from "./components/PrivateRoute"
import ProfilePage from "./pages/ProfilePage"
import ProjectDetailsPage from "./pages/ProjectDetailsPage"
import { useState, useEffect } from "react";
import { me } from "./api/AuthApi";
import { useNavigate } from "react-router";

export default function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);    

    const [loading, setLoading] = useState(true);

    const navigate = useNavigate();

    useEffect(() => {        
        const checkAuth = async () => {
            try {
                await me();
                setIsAuthenticated(true);                
                navigate("/profile");
            }
            catch {
                setIsAuthenticated(false);
                navigate("/login");
            }
            finally {
                setLoading(false);
            }
        };

        checkAuth();
    },[]);

    return (
        <Routes>
            <Route path="/register"
                element={<RegisterPage />}
            />
            <Route path="/login"
                element={<LoginPage setIsAuthenticated={setIsAuthenticated} />}
            />   
            <Route path="/profile"
                element={<PrivateRoute element={<ProfilePage setIsAuthenticated={setIsAuthenticated} />}
                                       isAuthenticated={isAuthenticated}
                                       loading={loading} />}
            />
            <Route path="/projects/:projectId"
                element={<PrivateRoute element={<ProjectDetailsPage/>}
                                       isAuthenticated={isAuthenticated}
                                       loading={loading} />}
            />
        </Routes>
    );
}