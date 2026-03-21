import { useState } from "react";
import { login } from "../api/AuthApi"
import { useNavigate } from "react-router";

interface LoginPageProps {
    setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>;
}

export default function LoginPage({ setIsAuthenticated } : LoginPageProps) {
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");       

    const [showPassword, setShowPassword] = useState(false);

    const [loading, setLoading] = useState(false);

    const [error, setError] = useState("");

    const navigate = useNavigate();

    function formIsValid() : boolean {
        if (!userName) {
            setError("Username is required");
            return false;
        }

        if (!password) {
            setError("Password is required");
            return false;
        }

        setError("");

        return true;
    }

    async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {   
        e.preventDefault();

        if (!formIsValid()) return;

        setLoading(true);
        try {
            await login({ userName, password });
            setIsAuthenticated(true);
            navigate("/profile");
        }
        catch (err: any) {
            if (err instanceof Error) {
                setError(err.message);
            } else {
                setError("Login failed");
            }
        }
        finally {
            setLoading(false);
        }
    }

    return (
        <div className="auth-page">
            <form className="auth-form" onSubmit={handleSubmit}>
                <h2>Login</h2>

                <div className="form-group">
                    <label>Username</label>
                    <input
                        value={userName}
                        onChange={(e) => setUserName(e.target.value)}
                        disabled={loading}
                    />
                </div>

                <div className="form-group password-group">
                    <label>Password</label>
                    <div className="password-wrapper">
                        <input
                            type={showPassword ? "text" : "password"}
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            disabled={loading}
                        />
                        <button
                            type="button"
                            className={`toggle-password ${showPassword ? "active" : ""}`}
                            onClick={() => setShowPassword(!showPassword)}
                        >
                            {showPassword ? (                                
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="white" width="20" height="20">
                                    <path d="M12 5c-7 0-11 7-11 7s4 7 11 7 11-7 11-7-4-7-11-7zm0 12a5 5 0 110-10 5 5 0 010 10z" />
                                    <circle cx="12" cy="12" r="2.5" fill="white" />
                                </svg>
                            ) : (
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="white" width="20" height="20">
                                    <path d="M12 5c-7 0-11 7-11 7s4 7 11 7c2.3 0 4.4-.7 6.1-1.8l-1.4-1.4c-1.2.8-2.6 1.2-4.7 1.2-4 0-7.2-2.5-8.7-5.2l1.6-1.6A7.935 7.935 0 0012 7c2.1 0 3.9.8 5.3 2.1l1.4-1.4C16.4 6.2 14.3 5 12 5z" />
                                    <path d="M0 0h24v24H0z" fill="none" />
                                </svg>
                            )}
                        </button>
                    </div>
                </div>

                {error && <p className="error">{error}</p>}

                <button type="submit" disabled={loading}>
                    {loading ? "Logging in..." : "Login"}
                </button>

                <p style={{ textAlign: "center", marginTop: "10px" }}>
                    Dont have an account?{" "}
                    <span
                        style={{ color: "#4a90e2", cursor: "pointer" }}
                        onClick={() => navigate("/register")}
                    >
                        Register
                    </span>
                </p>
            </form>
        </div>
    );
}