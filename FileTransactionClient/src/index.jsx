import React from "react";
import { createRoot } from "react-dom/client";
import App from "./App.jsx";
import "./style/reset.css";
import "./style/general.css";

const container = document.getElementById("root");
const root = createRoot(container); // createRoot(container!) if you use TypeScript
root.render(<App />);
