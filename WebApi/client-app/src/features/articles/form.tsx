import { useTranslation } from "react-i18next";
import { VerticalForm, FormInput } from "../../components";
import FeatherIcons from "feather-icons-react";
// import { AppDispatch, RootState } from "../../redux/store";
// import { useDispatch, useSelector } from "react-redux";
import { Button, Card, Col, Row } from "react-bootstrap";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import AppForm, { FieldTypes } from "../../components/form/form";
import { useEffect, useMemo, useState } from "react";
import { ArticleSchema } from "./form.schema";
import { useNavigate, useParams } from "react-router-dom";
import useFetch from "../../hooks/use-fetch";

export default function ArticleForm() {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { artId } = useParams();
  const [article, setArticle] = useState<any>({});
  const { get, post } = useFetch();
  const onSubmit = async (data: any) => {
    await post("/articles", { ...data, Id: artId });
    navigate("/articles");
  };
  useEffect(() => {
    (async function () {
      if (artId) {
        const prop = await get(`/articles/${artId}`, {
          query: { forEdit: true },
        });
        setArticle((prop as any) || {});
        return;
      }
      setArticle({});
    })();
  }, [artId]);
  return (
    <AppForm
      key={article?.Id}
      defaultValues={article}
      files={{ Images: article?.Images }}
      onSubmit={onSubmit}
      fields={fields}
      validationSchema={ArticleSchema}
    />
  );
}

const fields = [
  {
    id: "Title",
    label: "Title",
    name: "Title",
    type: FieldTypes.TEXT,
  },
  {
    id: "SubTitle",
    label: "Sub Title",
    name: "SubTitle",
    type: FieldTypes.TEXT,
  },
  {
    id: "ParagraphOne",
    label: "Paragraph One",
    name: "ParagraphOne",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "ParagraphTwo",
    label: "Paragraph Two",
    name: "ParagraphTwo",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "images",
    label: "Images",
    name: "Images",
    type: FieldTypes.IMAGES,
    grid: { md: 12 },
  },
];
