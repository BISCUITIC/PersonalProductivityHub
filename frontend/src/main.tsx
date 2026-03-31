import ReactDOM from "react-dom/client";
import App from "./App.tsx"
import "./styles/index.css"
import "./styles/AuthPage.css"
import { BrowserRouter } from "react-router-dom";

var rootNode = document.getElementById('root');

if (!rootNode) throw new Error("Root element not found");
const root = ReactDOM.createRoot(rootNode);

root.render(
    <BrowserRouter>
        <App />
    </BrowserRouter>
);
