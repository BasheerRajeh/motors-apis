import {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import useFetch, { USER_KEY } from "./hooks/use-fetch";
import { useNavigate } from "react-router-dom";
import {
  LayoutTypes,
  LayoutWidth,
  MenuPositions,
  SideBarTheme,
  SideBarTypes,
  TopbarTheme,
} from "./constants";

const RootContext = createContext<RootContextType | null>(null);

export const useRoot = function () {
  return useContext(RootContext);
};

export const useBlocker = function () {
  const root = useRoot();
  return {
    block: function () {
      root?.setBlocked(true);
    },
    unblock: function () {
      root?.setBlocked(false);
    },
  };
};

export const useUser = function () {
  const root = useRoot();
  return root?.user;
};

export const useConfirmation = function () {
  const root = useRoot();
  return function (message: string, cb: () => void) {
    root?.setConfirmation({ message, onOk: cb });
  };
};

export const useLogout = function () {
  const root = useContext(RootContext);
  return function () {
    root?.setUser(null);
    // root?.setLoggedIn(false);
  };
};

let userSession: UserType | null = null;
try {
  userSession = JSON.parse(localStorage.getItem("user-token-data") || "");
} catch (error) {
  console.error(error);
}

export default function RootProvider(props: PropsWithChildren<any>) {
  useEffect(function () {}, []);
  const navigate = useNavigate();
  // const hasSession = !!localStorage.getItem(USER_KEY);
  const { get } = useFetch();
  const [user, setUser] = useState<UserType | null>(userSession);
  const [confirmation, setConfirmation] = useState<ConfirmationType>();
  // const [loggedInStatus, setLoggedInStatus] = useState<loggedInState | null>(
  //   null
  // );
  const [blocked, setBlocked] = useState<boolean>(false);
  async function login() {
    const user = await get("/auth/user");
    setUser(user as UserType);
  }
  async function logout() {
    // setConfirmation({
    //   message: "Are you sure you want to logout?",
    //   onOk: async function () {
    try {
      await get("/auth/logout");
    } finally {
      _logout();
    }
    //   },
    // });
    // if (!window.confirm()) {
    //   return;
    // }
  }
  function onOkHandler(cb: () => void) {
    cb && cb();
  }
  function noSessionHandler() {
    _logout();
    // const result = confirm(
    //   "Your session has been expired you want to login again?"
    // );
    // if (result) {
    //   _logout();
    // }
  }

  function _logout() {
    localStorage.removeItem(USER_KEY);
    setUser(null);
    // setLoggedInStatus(null);
    navigate("/");
  }
  // const [messageApi, contextHolder] = message.useMessage();
  return (
    <RootContext.Provider
      value={{
        user,
        setUser,
        // loggedInStatus,
        // setLoggedInStatus,
        blocked,
        setBlocked,
        logout,
        confirmation,
        setConfirmation,
        noSessionHandler,
      }}
    >
      {props.children}
    </RootContext.Provider>
  );
}

export type ConfirmationType = {
  message: string | null;
  onOk: () => void;
};
export type RootContextType = {
  user: UserType | null;
  setUser: React.Dispatch<any>;
  // loggedInStatus: loggedInState | null;
  // setLoggedInStatus: React.Dispatch<loggedInState | null>;
  confirmation: ConfirmationType | undefined;
  setConfirmation: React.Dispatch<ConfirmationType | undefined>;
  blocked: boolean;
  setBlocked: React.Dispatch<boolean>;
  logout: () => void;
  noSessionHandler: () => void;
};

export enum loggedInState {
  inprogress = 0,
  success = 1,
  failed = 3,
}

export type UserType = {
  Id: number;
  IsSuperAdmin: boolean;
  IsAdmin: boolean;
  IsManager: boolean;
  IsSupervisor: boolean;
  IsVendor: boolean;
  Email: string | null;
  FirstName: string;
  LastName: string;
  OrganizationId: number;
  VendorId: number;
  OrganizationName: string;
};
