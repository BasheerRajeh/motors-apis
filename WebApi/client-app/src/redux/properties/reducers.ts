// apicore
// import { APICore } from "../../helpers/api/apiCore";

// constants
import { PropertyActionTypes } from "./constants";

// const api = new APICore();

const INIT_STATE = {
  properties: { Data: [], PageCount: 0 },
  categories: [],
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

interface BlogActionType {
  type:
    | PropertyActionTypes.API_RESPONSE_SUCCESS
    | PropertyActionTypes.API_RESPONSE_ERROR
    | PropertyActionTypes.VIEW_LIST;
  payload: {
    actionType?: string;
    data?: any;
    error?: string;
  };
}

interface State {
  properties?: any;
  loading?: boolean;
  error?: string;
}

const Property = (state: State = INIT_STATE, action: BlogActionType): any => {
  switch (action.type) {
    case PropertyActionTypes.API_RESPONSE_SUCCESS:
      switch (action.payload.actionType) {
        case PropertyActionTypes.VIEW_LIST: {
          return {
            ...state,
            properties: action.payload.data,
            loading: false,
          };
        }
        case PropertyActionTypes.REFRESH_CATEGORIES_LIST: {
          return {
            ...state,
            categories: action.payload.data,
            loading: false,
          };
        }
        case PropertyActionTypes.LOAD: {
          return {
            ...state,
            property: action.payload.data,
            loading: false,
          };
        }
        default:
          return { ...state };
      }

    case PropertyActionTypes.API_RESPONSE_ERROR:
      switch (action.payload.actionType) {
        case PropertyActionTypes.LOAD:
        case PropertyActionTypes.REFRESH_CATEGORIES_LIST:
        case PropertyActionTypes.VIEW_LIST: {
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

export default Property;
