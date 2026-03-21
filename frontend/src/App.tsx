import { BrowserRouter, Routes, Route } from "react-router-dom";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";
import PrivateRoute from "./components/PrivateRoute"
import ProfilePage from "./pages/ProfilePage"
import { useState, useEffect } from "react";
import { me } from "./api/AuthApi";

export default function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);    

    const [loading, setLoading] = useState(true);

    useEffect(() => {        
        const checkAuth = async () => {
            try {
                await me();
                setIsAuthenticated(true);
            }
            catch {
                setIsAuthenticated(false);
            }
            finally {
                setLoading(false);
            }
        };

        checkAuth();
    },[]);

    return (
        <BrowserRouter>
            <Routes>
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/login" element={<LoginPage setIsAuthenticated={ setIsAuthenticated} />} />   
                <Route path="/profile"
                    element={<PrivateRoute element={<ProfilePage />} isAuthenticated={isAuthenticated} loading={loading} />} />
            </Routes>
        </BrowserRouter>
    );
}