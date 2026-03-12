import { useState } from "react";
import { register } from "../api/AuthApi";

export default function RegisterPage() {

    const [userName, setUserName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();

        try
        {
            const result = await register({ userName, email, password });

            console.log(result);

            alert("Registration successful");
        }
        catch (error: any)
        {
            alert("Registration failed: " + error.response?.data?.join(", "));
        }
    }

    return (
        <div>
            <h2>Register</h2>

            <form onSubmit={handleSubmit}>

                <div>
                    <label>Username</label>
                    <input
                        value={userName}
                        onChange={(e) => setUserName(e.target.value)}
                    />
                </div>

                <div>
                    <label>Email</label>
                    <input
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                </div>

                <div>
                    <label>Password</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </div>

                <button type="submit">
                    Register
                </button>

            </form>
        </div>
    );
}