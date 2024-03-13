import * as yup from "yup";

export const TestimonialSchema = yup.object().shape({
  Name: yup.string().required().max(100),
  Title: yup.string().required().max(100),
  Comments: yup.string().required().max(600),
});
