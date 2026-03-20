import { BrowserRouter, Routes, Route } from "react-router-dom";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";
import PrivateRoute from "./components/PrivateRoute"
import ProfilePage from "./pages/ProfilePage"
import { useState } from "react";

export default function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    return (
        <BrowserRouter>
            <Routes>
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/login" element={<LoginPage setIsAuthenticated={ setIsAuthenticated} />} />   
                <Route
                    path="/profile"
                    element={<PrivateRoute element={<ProfilePage />} isAuthenticated={isAuthenticated} />}
                />

            </Routes>
        </BrowserRouter>
    );
}