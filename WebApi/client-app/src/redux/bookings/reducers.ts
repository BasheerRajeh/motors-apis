// apicore
// import { APICore } from "../../helpers/api/apiCore";

// constants
import { BookingActionTypes } from "./constants";

// const api = new APICore();

const INIT_STATE = {
  bookings: [],
  loading: false,
};

export interface BlogType {
  id: number;
  bloginfoid: BlogInfo;
  categoryid: string;
  statusid: string;
  main_image_id: MainImage;
  userid: string;
  name_blog: string;
  subtitle: string;
}

export interface BlogInfo {
  id: number;
  images: string[];
  description: string;
  date: Date;
  video_url: string;
}

export interface MainImage {
  id: number;
  image: string;
}

interface BookingActionType {
  type:
    | BookingActionTypes.API_RESPONSE_SUCCESS
    | BookingActionTypes.API_RESPONSE_ERROR
    | BookingActionTypes.VIEW_LIST;
  payload: {
    actionType?: string;
    data?: any;
    error?: string;
  };
}

interface State {
  bookings?: any;
  loading?: boolean;
  error?: string;
}

const Booking = (state: State = INIT_STATE, action: BookingActionType): any => {
  switch (action.type) {
    case BookingActionTypes.API_RESPONSE_SUCCESS:
      switch (action.payload.actionType) {
        case BookingActionTypes.VIEW_LIST: {
          return {
            ...state,
            bookings: action.payload.data,
            loading: false,
          };
        }
        default:
          return { ...state };
      }

    case BookingActionTypes.API_RESPONSE_ERROR:
      switch (action.payload.actionType) {
        case BookingActionTypes.VIEW_LIST: {
          return {
            ...state,
            error: action.payload.error,
            loading: false,
          };
        }
        default:
          return { ...state };
      }

    default:
      return { ...state };
  }
};

export default Booking;
