import { useTranslation } from "react-i18next";
import AppForm, { FieldTypes } from "../../components/form/form";
import { useEffect, useMemo, useState } from "react";
import { TestimonialSchema } from "./form.schema";
import { useNavigate, useParams } from "react-router-dom";
import useFetch from "../../hooks/use-fetch";

export default function TestimonialForm() {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { id } = useParams();
  const [testimonial, setTestimonial] = useState<any>({});
  const { get, post } = useFetch();
  const onSubmit = async (data: any) => {
    await post("/testimonials", { ...data, Id: id });
    navigate("/testimonials");
  };
  useEffect(() => {
    (async function () {
      if (id) {
        const prop = await get(`/testimonials/${id}`, {
          query: { forEdit: true },
        });
        setTestimonial((prop as any) || {});
        return;
      }
      setTestimonial({});
    })();
  }, [id]);
  return (
    <AppForm
      key={testimonial?.Id}
      defaultValues={testimonial}
      files={{ Images: testimonial?.Images }}
      onSubmit={onSubmit}
      fields={fields}
      validationSchema={TestimonialSchema}
    />
  );
}

const fields = [
  {
    id: "Name",
    label: "Name",
    name: "Name",
    type: FieldTypes.TEXT,
  },
  {
    id: "Title",
    label: "Title",
    name: "Title",
    type: FieldTypes.TEXT,
  },
  {
    id: "Comments",
    label: "Comments",
    name: "Comments",
    type: FieldTypes.TEXT,
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
