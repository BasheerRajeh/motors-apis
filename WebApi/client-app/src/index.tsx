import React from "react";
import ReactDOM from "react-dom/client";

import "./i18n";

import App from "./App";

// import { Provider } from "react-redux";
// import { configureStore } from "./redux/store";
import { BrowserRouter } from "react-router-dom";
import RootProvider from "./root-context";
import LayoutProvider from "./contexts/layout/layout-context";

const root = ReactDOM.createRoot(document.getElementById("root")!);
root.render(
  <BrowserRouter basename={process.env.PUBLIC_URL}>
    <LayoutProvider>
      <RootProvider>
        <App />
      </RootProvider>
    </LayoutProvider>
  </BrowserRouter>
);
