import * as yup from "yup";

export const ArticleSchema = yup.object().shape({
  Title: yup.string().required().max(100),
  SubTitle: yup.string().required().max(100),
  ParagraphOne: yup.string().required().max(4000),
  ParagraphTwo: yup.string().max(4000),
});
