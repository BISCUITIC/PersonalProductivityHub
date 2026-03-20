import ReactDOM from "react-dom/client";
import React from "react";
import App from "./App.tsx"
import "./styles/index.css"
import "./styles/AuthPage.css"

var rootNode = document.getElementById('root');

if (!rootNode) throw new Error("Root element not found");
const root = ReactDOM.createRoot(rootNode);

root.render(
    <React.StrictMode>
        <App />
    </React.StrictMode>
);
