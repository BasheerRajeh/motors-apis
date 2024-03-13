import {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
  useReducer,
  useState,
} from "react";
import useFetch, { USER_KEY } from "../../hooks/use-fetch";
import { useNavigate } from "react-router-dom";
import {
  LayoutTypes,
  LayoutWidth,
  MenuPositions,
  SideBarTheme,
  SideBarTypes,
  TopbarTheme,
} from "../../constants";
import { getLayoutConfigs } from "../../utils";

const INIT_STATE = {
  layoutType: LayoutTypes.LAYOUT_VERTICAL,
  layoutWidth: LayoutWidth.LAYOUT_WIDTH_FLUID,
  menuPosition: MenuPositions.MENU_POSITION_FIXED,
  leftSideBarTheme: SideBarTheme.LEFT_SIDEBAR_THEME_LIGHT,
  leftSideBarType: SideBarTypes.LEFT_SIDEBAR_TYPE_DEFAULT,
  showSidebarUserInfo: false,
  topbarTheme: TopbarTheme.TOPBAR_THEME_LIGHT,
  isOpenRightSideBar: false,
};

const LayoutContext = createContext<LayoutContextType>({
  dispatch: (x) => x,
  layout: INIT_STATE,
});

export const useLayout = function () {
  return useContext(LayoutContext);
};

export default function LayoutProvider(props: PropsWithChildren<any>) {
  const [layout, dispatch] = useReducer(reducer, INIT_STATE);
  return (
    <LayoutContext.Provider
      value={{
        layout,
        dispatch,
      }}
    >
      {props.children}
    </LayoutContext.Provider>
  );
}

export type LayoutContextType = {
  layout: any;
  dispatch: React.Dispatch<any>;
};

function reducer(state: any, action: any) {
  switch (action.type) {
    case LayoutActionTypes.CHANGE_LAYOUT:
      return {
        ...state,
        layoutType: action.payload,
      };
    case LayoutActionTypes.CHANGE_LAYOUT_WIDTH:
      const layoutConfig = getLayoutConfigs(action.payload! && action.payload);
      return {
        ...state,
        layoutWidth: action.payload,
        ...layoutConfig,
      };
    case LayoutActionTypes.CHANGE_MENU_POSITIONS:
      return {
        ...state,
        menuPosition: action.payload,
      };
    case LayoutActionTypes.CHANGE_SIDEBAR_THEME:
      return {
        ...state,
        leftSideBarTheme: action.payload,
      };
    case LayoutActionTypes.CHANGE_SIDEBAR_TYPE:
      return {
        ...state,
        leftSideBarType: action.payload,
      };
    case LayoutActionTypes.TOGGLE_SIDEBAR_USER_INFO:
      return {
        ...state,
        showSidebarUserInfo: action.payload,
      };
    case LayoutActionTypes.CHANGE_TOPBAR_THEME:
      return {
        ...state,
        topbarTheme: action.payload,
      };
    case LayoutActionTypes.SHOW_RIGHT_SIDEBAR:
      return {
        ...state,
        isOpenRightSideBar: true,
      };
    case LayoutActionTypes.HIDE_RIGHT_SIDEBAR:
      return {
        ...state,
        isOpenRightSideBar: false,
      };
    default:
      return state;
  }
}

enum LayoutActionTypes {
  CHANGE_LAYOUT = "@@layout/CHANGE_LAYOUT",
  CHANGE_LAYOUT_WIDTH = "@@layout/CHANGE_LAYOUT_WIDTH",
  CHANGE_MENU_POSITIONS = "@@layout/CHANGE_MENU_POSITIONS",
  CHANGE_SIDEBAR_THEME = "@@layout/CHANGE_SIDEBAR_THEME",
  CHANGE_SIDEBAR_TYPE = "@@layout/CHANGE_SIDEBAR_TYPE",
  TOGGLE_SIDEBAR_USER_INFO = "@@layout/TOGGLE_SIDEBAR_USER_INFO",
  CHANGE_TOPBAR_THEME = "@@layout/CHANGE_TOPBAR_THEME",

  SHOW_RIGHT_SIDEBAR = "@@layout/SHOW_RIGHT_SIDEBAR",
  HIDE_RIGHT_SIDEBAR = "@@layout/HIDE_RIGHT_SIDEBAR",
}
