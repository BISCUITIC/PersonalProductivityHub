import { useEffect, useState } from "react";
import { getProfile } from "../api/AuthApi";
import type { AuthResponse } from "../api/AuthApi";

export default function ProfilePage() {
    const [profile, setProfile] = useState<AuthResponse | null>(null);

    useEffect(() => {
        getProfile().then(setProfile).catch(console.error);
    }, []);

    if (!profile) return <div>Loading...</div>;

    return (
        <div>
            <h2>Profile</h2>
            <p>Username: {profile.userName}</p>
            <p>Email: {profile.email}</p>
            <p>Created at: {new Date(profile.createdAt).toLocaleString()}</p>
        </div>
    );
}