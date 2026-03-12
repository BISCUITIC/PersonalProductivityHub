// frontend/src/pages/LoginPage.tsx
import { useState } from "react";
import { login } from "../api/AuthApi";
import { useNavigate } from "react-router-dom";

export default function LoginPage() {
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);

    const navigate = useNavigate();

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setLoading(true);

        try {
            await login(userName, password);
            alert("Login successful");
            navigate("/profile");
        } catch (error: any) {
            alert("Login failed: " + error.response?.data);
        } finally {
            setLoading(false);
        }
    }

    return (
        <div>
            <h2>Login</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Username</label>
                    <input value={userName} onChange={(e) => setUserName(e.target.value)} />
                </div>

                <div>
                    <label>Password</label>
                    <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
                </div>

                <button type="submit" disabled={loading}>Login</button>
            </form>
        </div>
    );
}