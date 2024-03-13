// apicore
// import { APICore } from "../../helpers/api/apiCore";

// constants
import { UsersActionTypes } from "./constants";

// const api = new APICore();

const INIT_STATE = {
  users: [],
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

interface UserActionType {
  type:
    | UsersActionTypes.API_RESPONSE_SUCCESS
    | UsersActionTypes.API_RESPONSE_ERROR
    | UsersActionTypes.VIEW_LIST;
  payload: {
    actionType?: string;
    data?: any;
    error?: string;
  };
}

interface State {
  users?: any;
  loading?: boolean;
  error?: string;
}

const User = (state: State = INIT_STATE, action: UserActionType): any => {
  switch (action.type) {
    case UsersActionTypes.API_RESPONSE_SUCCESS:
      switch (action.payload.actionType) {
        case UsersActionTypes.VIEW_LIST: {
          return {
            ...state,
            users: action.payload.data,
            loading: false,
          };
        }
        default:
          return { ...state };
      }

    case UsersActionTypes.API_RESPONSE_ERROR:
      switch (action.payload.actionType) {
        case UsersActionTypes.VIEW_LIST: {
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

export default User;
