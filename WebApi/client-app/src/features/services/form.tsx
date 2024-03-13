import { useTranslation } from "react-i18next";
import AppForm, { FieldTypes } from "../../components/form/form";
import { useEffect, useMemo, useState } from "react";
import { ServiceSchema } from "./form.schema";
import { useNavigate, useParams } from "react-router-dom";
import useFetch from "../../hooks/use-fetch";

export default function ServiceForm() {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { id } = useParams();
  const [service, setService] = useState<any>({});
  const { get, post } = useFetch();
  const onSubmit = async (data: any) => {
    await post("/services", { ...data, Id: id });
    navigate("/services");
  };
  useEffect(() => {
    (async function () {
      if (id) {
        const prop = await get(`/services/${id}`, { query: { forEdit: true } });
        setService((prop as any) || {});
        return;
      }
      setService({});
    })();
  }, [id]);
  return (
    <AppForm
      key={service?.Id}
      defaultValues={service}
      files={{ Images: service?.Images }}
      onSubmit={onSubmit}
      fields={fields}
      validationSchema={ServiceSchema}
    />
  );
}

const fields = [
  {
    id: "Title",
    label: "Title",
    name: "Title",
    type: FieldTypes.TEXT,
    grid: { md: 12 },
  },
  {
    id: "TitleKz",
    label: "Title KZ",
    name: "TitleKz",
    type: FieldTypes.TEXT,
    grid: { md: 12 },
  },
  {
    id: "TitleRu",
    label: "Title RU",
    name: "TitleRu",
    type: FieldTypes.TEXT,
    grid: { md: 12 },
  },
  {
    id: "Description",
    label: "Description",
    name: "Description",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "DescriptionKz",
    label: "Description KZ",
    name: "DescriptionKz",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "DescriptionRu",
    label: "Description RU",
    name: "DescriptionRu",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "Active",
    label: "Active",
    name: "Active",
    type: FieldTypes.CHECKBOX,
  },
  {
    id: "images",
    label: "Images",
    name: "Images",
    type: FieldTypes.IMAGES,
    grid: { md: 12 },
  },
];
