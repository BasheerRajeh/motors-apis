import React, { useEffect } from "react";
import { Row, Col } from "react-bootstrap";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { useDispatch } from "react-redux";

//actions
import { logoutUser } from "../../redux/actions";

// components
import AuthLayout from "./AuthLayout";

// images
import logoDark from "../../assets/images/logo-dark.png";
import logoLight from "../../assets/images/logo-light.png";
import { useRoot } from "../../root-context";

const LogoutIcon = () => {
  return (
    <svg
      version="1.1"
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 130.2 130.2"
    >
      <circle
        className="path circle"
        fill="none"
        stroke="#4bd396"
        strokeWidth="6"
        strokeMiterlimit="10"
        cx="65.1"
        cy="65.1"
        r="62.1"
      />
      <polyline
        className="path check"
        fill="none"
        stroke="#4bd396"
        strokeWidth="6"
        strokeLinecap="round"
        strokeMiterlimit="10"
        points="100.2,40.2 51.5,88.8 29.8,67.5 "
      />
    </svg>
  );
};

/* bottom link */
const BottomLink = () => {
  const { t } = useTranslation();
  return (
    <Row className="mt-3">
      <Col xs={12} className="text-center">
        <p className="text-muted">
          {t("Back to")}{" "}
          <Link to={"/auth/login"} className="text-primary fw-bold ms-1">
            {t("Sign In")}
          </Link>
        </p>
      </Col>
    </Row>
  );
};

const Logout = () => {
  const { t } = useTranslation();
  const root = useRoot();

  useEffect(() => {
    root?.logout();
  }, []);

  return (
    <>
      <AuthLayout bottomLinks={<BottomLink />}>
        <div className="text-center">
          <div className="auth-logo mx-auto">
            <Link to="/" className="logo logo-dark text-center">
              <span className="logo-lg">
                <img src={logoDark} alt="" height="24" />
              </span>
            </Link>

            <Link to="/" className="logo logo-light text-center">
              <span className="logo-lg">
                <img src={logoLight} alt="" height="24" />
              </span>
            </Link>
          </div>

          <div className="mt-4">
            <div className="logout-checkmark">
              <LogoutIcon />
            </div>
          </div>

          <h4 className="h4 mb-0 mt-2">{t("See you again !")}</h4>

          <p className="text-muted mt-1 mb-2">
            {t("You are now successfully sign out.")}
          </p>
        </div>
      </AuthLayout>
    </>
  );
};

export default Logout;
