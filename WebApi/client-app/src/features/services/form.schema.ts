import * as yup from "yup";

export const ServiceSchema = yup.object().shape({
  Title: yup.string().required().max(100),
  TitleKz: yup.string().required().max(100),
  TitleRu: yup.string().required().max(100),
  Description: yup.string().required().max(4000),
  DescriptionKz: yup.string().required().max(4000),
  DescriptionRu: yup.string().required().max(4000),
});
