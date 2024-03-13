import React, { useState } from "react";
import { Link } from "react-router-dom";
import classNames from "classnames";
import FeatherIcon from "feather-icons-react";

// actions
import { changeSidebarType } from "../redux/actions";

//constants
import { LayoutTypes, SideBarTypes } from "../constants/layout";

import MaximizeScreen from "../components/MaximizeScreen";
import ProfileDropdown from "../components/ProfileDropdown";

// images
import profilePic from "../assets/images/users/avatar-1.jpg";
import avatar4 from "../assets/images/users/avatar-4.jpg";
import logoSm from "../assets/images/logo-sm.webp";
import logoDark from "../assets/images/logo-dark.webp";
import logoLight from "../assets/images/logo-light.webp";
import { useLayout } from "../contexts/layout/layout-context";
import { useUser } from "../root-context";

export interface NotificationItem {
  id: number;
  text: string;
  subText: string;
  icon?: string;
  avatar?: string;
  bgColor?: string;
}

const ProfileMenus = [
  {
    label: "Logout",
    icon: "log-out",
    redirectTo: "/auth/logout",
  },
];

interface TopbarProps {
  hideLogo?: boolean;
  navCssClasses?: string;
  openLeftMenuCallBack?: () => void;
  topbarDark?: boolean;
}

const Topbar = ({
  hideLogo,
  navCssClasses,
  openLeftMenuCallBack,
  topbarDark,
}: TopbarProps) => {
  const user = useUser();
  const [isopen, setIsopen] = useState<boolean>(false);

  const navbarCssClasses: string = navCssClasses || "";
  const containerCssClasses: string = !hideLogo ? "container-fluid" : "";

  const { layout, dispatch } = useLayout();
  const { layoutType, leftSideBarType } = layout;

  const handleLeftMenuCallBack = () => {
    setIsopen(!isopen);
    if (openLeftMenuCallBack) openLeftMenuCallBack();
  };

  /**
   * Toggles the left sidebar width
   */
  const toggleLeftSidebarWidth = () => {
    if (leftSideBarType === "default" || leftSideBarType === "compact")
      dispatch(changeSidebarType(SideBarTypes.LEFT_SIDEBAR_TYPE_CONDENSED));
    if (leftSideBarType === "condensed")
      dispatch(changeSidebarType(SideBarTypes.LEFT_SIDEBAR_TYPE_DEFAULT));
  };

  return (
    <React.Fragment>
      <div className={`navbar-custom ${navbarCssClasses}`}>
        <div className={containerCssClasses}>
          {!hideLogo && (
            <div className="logo-box">
              <Link to="/" className="logo logo-dark">
                <span className="logo-sm">
                  <img src={logoSm} alt="" height="24" />
                </span>
                <span className="logo-lg">
                  <img src={logoDark} alt="" height="24" />
                </span>
              </Link>
              <Link to="/" className="logo logo-light">
                <span className="logo-sm">
                  <img src={logoSm} alt="" height="24" />
                </span>
                <span className="logo-lg">
                  <img src={logoLight} alt="" height="24" />
                </span>
              </Link>
            </div>
          )}

          <ul className="list-unstyled topnav-menu float-end mb-0">
            <li className="dropdown d-none d-lg-inline-block">
              <MaximizeScreen />
            </li>
            <li className="dropdown notification-list topbar-dropdown">
              <ProfileDropdown
                profilePic={profilePic}
                menuItems={ProfileMenus}
                username={`${user?.FirstName} ${user?.LastName}`}
              />
            </li>
          </ul>

          <ul className="list-unstyled topnav-menu topnav-menu-left m-0">
            {layoutType !== LayoutTypes.LAYOUT_HORIZONTAL && (
              <li>
                <button
                  className="button-menu-mobile d-none d-lg-block"
                  onClick={toggleLeftSidebarWidth}
                >
                  <FeatherIcon icon="menu" />
                  <i className="fe-menu"></i>
                </button>
              </li>
            )}

            <li>
              <button
                className="button-menu-mobile d-lg-none d-bolck"
                onClick={handleLeftMenuCallBack}
              >
                <FeatherIcon icon="menu" />
              </button>
            </li>
            <li>
              <Link
                to="#"
                className={classNames("navbar-toggle nav-link", {
                  open: isopen,
                })}
                onClick={handleLeftMenuCallBack}
              >
                <div className="lines">
                  <span></span>
                  <span></span>
                  <span></span>
                </div>
              </Link>
            </li>
          </ul>
        </div>
      </div>
    </React.Fragment>
  );
};

export default Topbar;
