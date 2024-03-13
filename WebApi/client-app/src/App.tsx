import React, { Suspense } from "react";

import AllRoutes from "./routes/Routes";

// Themes
// For Default import Theme.scss
import "./assets/scss/Theme.scss";
import RootProvider from "./root-context";
// For Dark import Theme-Dark.scss
// import "./assets/scss/Theme-Dark.scss";

const App = () => {
  return (
    <Suspense fallback={<></>}>
      <AllRoutes />
    </Suspense>
  );
};

export default App;
