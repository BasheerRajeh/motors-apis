import React from "react";
import { Navigate, Route, RouteProps } from "react-router-dom";

// components
import PrivateRoute from "./PrivateRoute";
import ViewCars from "../features/cars/table";
import CarForm from "../features/cars/form";
import ViewBookings from "../features/bookings/table";
import ViewUsers from "../features/users/table";
import ArticleForm from "../features/articles/form";
import ViewArticles from "../features/articles/table";
import ServiceForm from "../features/services/form";
import ViewServices from "../features/services/table";
import TestimonialForm from "../features/testimonials/form";
import ViewTestimonials from "../features/testimonials/table";
// import Root from "./Root";

// lazy load all the views

export interface RoutesProps {
  path: RouteProps["path"];
  name?: string;
  element?: RouteProps["element"];
  route?: any;
  exact?: boolean;
  icon?: string;
  header?: string;
  roles?: string[];
  children?: RoutesProps[];
}

const carRoutes: RoutesProps = {
  path: "/",
  name: "cars",
  route: PrivateRoute,
  icon: "uil-briefcase",
  children: [
    {
      path: "/cars/form/:id?",
      name: "cars-form",
      element: <CarForm />,
      route: PrivateRoute,
    },
    {
      path: "/cars",
      name: "cars-view",
      element: <ViewCars />,
      route: PrivateRoute,
    },
  ],
};

const servicesRoutes: RoutesProps = {
  path: "/",
  name: "services",
  route: PrivateRoute,
  icon: "uil-briefcase",
  children: [
    {
      path: "/services/form/:id?",
      name: "services-form",
      element: <ServiceForm />,
      route: PrivateRoute,
    },
    {
      path: "/services",
      name: "services-view",
      element: <ViewServices />,
      route: PrivateRoute,
    },
  ],
};

const testimonialsRoutes: RoutesProps = {
  path: "/",
  name: "testimonials",
  route: PrivateRoute,
  icon: "uil-briefcase",
  children: [
    {
      path: "/testimonials/form/:id?",
      name: "testimonials-form",
      element: <TestimonialForm />,
      route: PrivateRoute,
    },
    {
      path: "/testimonials",
      name: "testimonials-view",
      element: <ViewTestimonials />,
      route: PrivateRoute,
    },
  ],
};
const bookingsRoutes: RoutesProps = {
  path: "/",
  name: "bookings",
  route: PrivateRoute,
  icon: "uil-briefcase",
  children: [
    {
      path: "/bookings",
      name: "bookings-view",
      element: <ViewBookings />,
      route: PrivateRoute,
    },
  ],
};
const usersRoutes: RoutesProps = {
  path: "/",
  name: "users",
  route: PrivateRoute,
  icon: "uil-briefcase",
  children: [
    {
      path: "/users",
      name: "users-view",
      element: <ViewUsers />,
      route: PrivateRoute,
    },
  ],
};
const articleRoutes: RoutesProps = {
  path: "/",
  name: "articles",
  route: PrivateRoute,
  icon: "uil-briefcase",
  children: [
    {
      path: "/articles/form/:artId?",
      name: "articles-form",
      element: <ArticleForm />,
      route: PrivateRoute,
    },
    {
      path: "/articles",
      name: "articles-view",
      element: <ViewArticles />,
      route: PrivateRoute,
    },
  ],
};

const appRoutes = [
  carRoutes,
  bookingsRoutes,
  usersRoutes,
  articleRoutes,
  servicesRoutes,
  testimonialsRoutes,
  // dashboardRoutes,
];

// flatten the list of all nested routes
const flattenRoutes = (routes: RoutesProps[]) => {
  let flatRoutes: RoutesProps[] = [];

  routes = routes || [];
  routes.forEach((item: RoutesProps) => {
    flatRoutes.push(item);

    if (typeof item.children !== "undefined") {
      flatRoutes = [...flatRoutes, ...flattenRoutes(item.children)];
    }
  });
  return flatRoutes;
};

const authProtectedFlattenRoutes = flattenRoutes([...appRoutes]);
export { authProtectedFlattenRoutes };
