import React, { PropsWithChildren, useEffect } from "react";
import {
  Navigate,
  Outlet,
  Route,
  Routes,
  useLocation,
  useRoutes,
} from "react-router-dom";
import { useSelector } from "react-redux";

// All layouts containers
import VerticalLayout from "../layouts/Vertical";

import { authProtectedFlattenRoutes } from "./index";
import { loggedInState, useRoot, useUser } from "../root-context";
import useFetch from "../hooks/use-fetch";
// import Login from "../pages/auth/Login";

// auth
const Login = React.lazy(() => import("../pages/auth/Login"));
const Logout = React.lazy(() => import("../pages/auth/Logout"));
const Confirm = React.lazy(() => import("../pages/auth/Confirm"));
const ForgetPassword = React.lazy(() => import("../pages/auth/ForgetPassword"));
const Register = React.lazy(() => import("../pages/auth/Register"));
const LockScreen = React.lazy(() => import("../pages/auth/LockScreen"));

interface RoutesProps {}

const AllRoutes = (props: RoutesProps) => {
  const user = useUser();
  return (
    <React.Fragment>
      {user && user.Id ? (
        <AuthenticatedTemplate />
      ) : (
        <UnAuthenticatedTemplate />
      )}
      <Routes>
        <Route path="/auth/logout" element={<Logout />} />
      </Routes>
    </React.Fragment>
  );
};

function UnAuthenticatedTemplate(props: PropsWithChildren<any>) {
  const temp = useLocation();
  return (
    <Routes>
      <Route path="/" element={<Navigate to={"/auth/login"} />}></Route>
      <Route path="auth">
        <Route path="login" element={<Login />} />
        <Route path="register" element={<Register />} />
        <Route path="confirm" element={<Confirm />} />
        <Route path="forget-password" element={<ForgetPassword />} />
        <Route path="lock-screen" element={<LockScreen />} />
      </Route>
      <Route
        path="*"
        element={<Navigate to={`/auth/login?next=${temp.pathname}`} />}
      ></Route>
    </Routes>
  );
}

function AuthenticatedTemplate(props: PropsWithChildren<any>) {
  return (
    <Routes>
      {authProtectedFlattenRoutes.map((route, idx) => (
        <Route
          path={route.path}
          element={<VerticalLayout {...props}>{route.element}</VerticalLayout>}
          key={idx}
        />
      ))}
    </Routes>
  );
}

export default AllRoutes;
