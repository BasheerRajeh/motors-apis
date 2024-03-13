import { useNavigate } from "react-router-dom";
import { loggedInState, useBlocker, useLogout, useRoot } from "../root-context";
import { useEffect, useState } from "react";
import jwtDecode from "jwt-decode";
import config from "../config";
import { AsyncLocalStorage } from "async_hooks";

export const USER_KEY = "user-token-data";

export default function useFetch(baseUrl = config.API_URL) {
  const request = useRequest();
  return {
    get: (path: string, options = {}) => {
      return request(baseUrl + path, options);
    },
    post: (path: string, data: any, options = {}) => {
      return request(baseUrl + path, {
        ...options,
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
        method: "POST",
        body: JSON.stringify(data),
      });
    },
    postFile: (path: string, data: any) => {
      let body = new FormData();
      for (const file of data) {
        body.append("files", file, file.name);
      }
      return request(baseUrl + path, {
        headers: {
          Accept: "application/json",
        },
        method: "POST",
        body: body,
      });
    },
  };
}

function useRequest() {
  const navigate = useNavigate();
  const showError = useError();
  const root = useRoot();
  const acquireTokenInProgress = useState(false);
  const { block, unblock } = useBlocker();
  return function (
    url: string,
    {
      method,
      headers,
      body,
      fileName,
      isFile,
      query,
      options,
      anonymous,
    }: any | undefined
  ) {
    return new Promise(async (resolve, reject) => {
      !options?.skipBlocker && block();

      let authHeaderVal;
      if (!anonymous) {
        const { noSession, token } = await acquireToken();
        if (noSession) {
          !options?.skipBlocker && unblock();
          root?.noSessionHandler();
          reject({ noSession, token });
          return;
        }
        authHeaderVal = "bearer " + token;
      }
      const queryParams = query ? "?" + serializeQuery(query) : "";
      fetch(url + queryParams, {
        method: method,
        headers: {
          ...headers,
          Authorization: authHeaderVal,
        },
        body: body,
      })
        .then(function (resp) {
          if (resp.ok) {
            if (resp.status === 204) {
              resolve(null);
              return;
            }
            //   if (isFile) {
            //     resp.blob().then(function (blob) {
            //       saveAs(
            //         blob,
            //         ((fileName && `${fileName} `) || "") +
            //           moment().format("YYMMDD_HHmmss")
            //       );
            //       resolve(null);
            //     });
            //     return;
            //   }
            resp.json().then(function (data) {
              resolve(data);
            });
            return;
          } else if (resp.status === 401) {
            root?.setUser(null);
            // navigate("/login");
            reject(resp);
            return;
          } else if (resp.status === 403) {
            showError("You are not authorized to perform this action");
            reject(resp);
            return;
          } else if (resp.status === 400 || resp.status === 500) {
            resp
              .json()
              .then(function (data) {
                showError(
                  data.Message ||
                    "Something went wrong please contact the system administrator."
                );
              })
              .catch((e) => {
                showError(
                  "Something went wrong please contact the system administrator."
                );
              })
              .finally(() => reject(resp));
            return;
          }
          showError(
            "Something went wrong please contact the system administrator. status code:" +
              resp.status
          );
          reject(resp);
        })
        .catch((e) => {
          reject(e);
          console.log(e);
          showError(
            "Something went wrong please contact the system administrator."
          );
        })
        .finally(() => {
          !options?.skipBlocker && unblock();
        });
    });
  };
}

// export function useGet(url, query){
//   const request = useRequest()
//  const [data, setData]= useState()
//  const [state, setState]= useState<string>()
//   useEffect(function () {
//     request
//   }, [query]);
// }

export function useError() {
  const root = useRoot();

  return function (msg: string) {
    alert(msg);
    // root?.messageApi.open({
    //   type: "error",
    //   content: msg,
    // });
  };
}

async function acquireToken() {
  try {
    let user = JSON.parse(localStorage.getItem(USER_KEY) || "");
    if (!user || !user.Token) {
      return { noSession: true };
    }
    const decoded: any = jwtDecode(user?.Token?.AccessToken);
    const currentTime = Date.now() / 1000;
    if (decoded.exp > currentTime) {
      return { token: user?.Token?.AccessToken };
    }
    // if(acquireTokenInProgress[0]){
    //   return {};
    // }
    const resp = await fetch(config.API_URL + "/auth/refresh-token", {
      method: "POST",
      body: JSON.stringify(user?.Token),
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
    });
    if (resp.status > 299 || resp.status < 200) {
      console.error(resp);
      return { noSession: true };
    }
    const userFresh = await resp.json();
    localStorage.setItem(USER_KEY, JSON.stringify(userFresh));
    return { token: userFresh?.Token?.AccessToken };
  } catch (error) {
    console.error(error);
    return { noSession: true };
  }
}

function serializeQuery(obj: any) {
  var str = [];
  for (var p in obj)
    if (obj.hasOwnProperty(p) && obj[p] !== undefined) {
      str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
    }
  return str.join("&");
}
