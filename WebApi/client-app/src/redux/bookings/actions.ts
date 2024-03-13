import { BookingActionTypes } from "./constants";

export interface BookingActionType {
  type: BookingActionTypes;
  payload?: {};
}

export const bookingsApiResponseSuccess = (
  actionType: string,
  data: any
): BookingActionType => ({
  type: BookingActionTypes.API_RESPONSE_SUCCESS,
  payload: { actionType, data },
});
export const bookingsApiResponseError = (
  actionType: string,
  error: string
): BookingActionType => ({
  type: BookingActionTypes.API_RESPONSE_ERROR,
  payload: { actionType, error },
});

export const viewBookings = (): BookingActionType => ({
  type: BookingActionTypes.VIEW_LIST,
});
