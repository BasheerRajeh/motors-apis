import { PropertyActionTypes } from "./constants";

export interface PropertyActionType {
  type: PropertyActionTypes;
  payload?: {};
}

export const propertiesApiResponseSuccess = (
  actionType: string,
  data: any
): PropertyActionType => ({
  type: PropertyActionTypes.API_RESPONSE_SUCCESS,
  payload: { actionType, data },
});
export const propertiesApiResponseError = (
  actionType: string,
  error: string
): PropertyActionType => ({
  type: PropertyActionTypes.API_RESPONSE_ERROR,
  payload: { actionType, error },
});

export const viewProperties = (filter: any): PropertyActionType => ({
  type: PropertyActionTypes.VIEW_LIST,
  payload: filter,
});

export const refreshCategories = (): PropertyActionType => ({
  type: PropertyActionTypes.REFRESH_CATEGORIES_LIST,
});

export const loadPropertyData = (propId: string): PropertyActionType => ({
  type: PropertyActionTypes.LOAD,
  payload: propId,
});

export const resetPropertyData = (): PropertyActionType => ({
  type: PropertyActionTypes.RESET_DATA,
  payload: {},
});

export const createProperty = (formData: any): PropertyActionType => ({
  type: PropertyActionTypes.CREATE,
  payload: formData,
});
