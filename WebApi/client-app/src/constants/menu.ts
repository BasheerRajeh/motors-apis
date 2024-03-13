export interface MenuItemTypes {
  key: string;
  label: string;
  isTitle?: boolean;
  icon?: string;
  url?: string;
  badge?: {
    variant: string;
    text: string;
  };
  parentKey?: string;
  target?: string;
  children?: MenuItemTypes[];
}

const MENU_ITEMS: MenuItemTypes[] = [
  { key: "cars", label: "Cars", isTitle: true },
  {
    key: "cars-create",
    label: "Create Car",
    isTitle: false,
    icon: "package",
    url: "/cars/form",
  },
  {
    key: "cars-view",
    label: "View Cars",
    isTitle: false,
    icon: "package",
    url: "/cars",
  },
  { key: "bookings", label: "bookings", isTitle: true },
  {
    key: "bookings-view",
    label: "View Bookings",
    isTitle: false,
    icon: "package",
    url: "/bookings",
  },
  { key: "services", label: "services", isTitle: true },
  {
    key: "services-create",
    label: "Create Service",
    isTitle: false,
    icon: "package",
    url: "/services/form",
  },
  {
    key: "services-view",
    label: "View Services",
    isTitle: false,
    icon: "package",
    url: "/services",
  },
  { key: "articles", label: "articles", isTitle: true },
  {
    key: "articles-create",
    label: "Create Article",
    isTitle: false,
    icon: "package",
    url: "/articles/form",
  },
  {
    key: "articles-view",
    label: "View Articles",
    isTitle: false,
    icon: "package",
    url: "/articles",
  },
  { key: "testimonials", label: "testimonials", isTitle: true },
  {
    key: "testimonials-create",
    label: "Create Testimonial",
    isTitle: false,
    icon: "package",
    url: "/testimonials/form",
  },
  {
    key: "testimonials-view",
    label: "View Testimonials",
    isTitle: false,
    icon: "package",
    url: "/testimonials",
  },
  { key: "users", label: "users", isTitle: true },
  {
    key: "users-view",
    label: "View Users",
    isTitle: false,
    icon: "package",
    url: "/users",
  },
];

export { MENU_ITEMS };
