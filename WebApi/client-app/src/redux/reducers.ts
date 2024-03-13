import { combineReducers } from "redux";

// import Auth from "./auth/reducers";
import Property from "./properties/reducers";
import Booking from "./bookings/reducers";
import User from "./users/reducers";
import Layout from "./layout/reducers";

export default combineReducers({
  // Auth,
  Property,
  Layout,
  Booking,
  User,
});
