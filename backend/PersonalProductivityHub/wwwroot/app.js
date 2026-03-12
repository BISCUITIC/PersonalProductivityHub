const apiBase = ''; // если фронтенд на том же домене

// Регистрация
document.getElementById('btn-register').addEventListener('click', async () => {
    const username = document.getElementById('reg-username').value;
    const email = document.getElementById('reg-email').value;
    const password = document.getElementById('reg-password').value;

    const res = await fetch(`${apiBase}/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ UserName: username, Email: email, Password: password }),
        credentials: 'include'
    });

    const data = await res.json().catch(() => res.status);
    alert('Register result: ' + JSON.stringify(data));
});

// Логин
document.getElementById('btn-login').addEventListener('click', async () => {
    const username = document.getElementById('login-username').value;
    const password = document.getElementById('login-password').value;

    const res = await fetch(`${apiBase}/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ userName: username, password }),
        credentials: 'include'
    });

    const data = await res.text();
    alert('Login result: ' + data);
});

// Получение профиля
document.getElementById('btn-profile').addEventListener('click', async () => {
    const res = await fetch(`${apiBase}/profile`, {
        method: 'GET',
        credentials: 'include'
    });

    const data = await res.json().catch(() => res.status);
    document.getElementById('profile-output').textContent = JSON.stringify(data.createdAt, null, 2);
});