import moment from "moment";
import { Fragment } from "react";

export const TrayDateFormat = "YYYYMMDD";
export const UserDateFormat = "DD/MM/YYYY";
export const DateFormat = "YYYY-MM-DD";
export const UserDateTimeFormat = "DD/MM/YYYY HH:mm:ss";
export const UserDateTimeFormat2 = "MM/DD/YYYY HH:mm A";
export const UserDayDateTimeFormat = "dd DD/MM/YYYY HH:mm:ss";
export const UserDayDateFormat = "dddd DD/MM/YYYY";

// export const toUtc = (date:string, addDay:number) => {
//   if (date) {
//     const utc = moment(date).add(new Date().getTimezoneOffset(), "minutes");
//     if (addDay) return moment(utc).add(1, "days").format("YYYY-MM-DDTHH:mm:ss");
//     return utc.format("YYYY-MM-DDTHH:mm:ss");
//   }
// };

export const toLocalDateTime = (isoTime: string): string => {
  if (isoTime) {
    // return moment(isoTime).format(UserDateTimeFormat);
    return moment.utc(isoTime).local().format(UserDateTimeFormat);
  }
  return "";
};
export const toLocalDateTime2 = (isoTime: string) => {
  if (isoTime) {
    return moment.utc(isoTime).local().format(UserDateTimeFormat2);
  }
};

export const toLocalDayDateTime = (isoTime: string) => {
  if (isoTime) {
    return moment.utc(isoTime).local().format(UserDayDateTimeFormat);
  }
};

export const toLocalDayDate = (isoTime: string) => {
  if (isoTime) {
    return moment.utc(isoTime).local().format(UserDayDateFormat);
  }
};

export const toLocalDate = (isoTime: string) => {
  if (isoTime) {
    return moment.utc(isoTime).local().format(UserDateFormat);
  }
};
export const toLocalDate2 = (dateString: string) => {
  if (dateString) {
    return moment(dateString, TrayDateFormat).format(UserDateFormat);
  }
};

export function validatePasswordRules(value: string) {
  if (value && value.length < 8) {
    return Promise.reject(
      new Error("Password must be at least 8 characters long.")
    );
  }
  return Promise.resolve();
}

// export function applyLineBreaks(str:string):JSX.Element {
//   return str.split("\n").map((item, idx) => {
//     return (
//       <Fragment key={idx}>
//         {item}
//         <br />
//       </Fragment>
//     );
//   });
// }
