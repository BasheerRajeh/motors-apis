import { UsersActionTypes } from "./constants";

export interface UserActionType {
  type: UsersActionTypes;
  payload?: {};
}

export const usersApiResponseSuccess = (
  actionType: string,
  data: any
): UserActionType => ({
  type: UsersActionTypes.API_RESPONSE_SUCCESS,
  payload: { actionType, data },
});
export const usersApiResponseError = (
  actionType: string,
  error: string
): UserActionType => ({
  type: UsersActionTypes.API_RESPONSE_ERROR,
  payload: { actionType, error },
});

export const viewUsers = (): UserActionType => ({
  type: UsersActionTypes.VIEW_LIST,
});
