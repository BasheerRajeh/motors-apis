import { useTranslation } from "react-i18next";
import AppForm, { FieldTypes } from "../../components/form/form";
import { useEffect, useMemo, useState } from "react";
import { CarSchema } from "./form.schema";
import { useNavigate, useParams } from "react-router-dom";
import useFetch from "../../hooks/use-fetch";

export default function CarForm() {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { id } = useParams();
  const [car, setCar] = useState<any>({});
  const { get, post } = useFetch();
  const onSubmit = async (data: any) => {
    // console.log(data);
    await post("/cars", { ...data, Id: id });
    navigate("/cars");
  };
  useEffect(() => {
    (async function () {
      if (id) {
        const prop = await get(`/cars/${id}`, { query: { forEdit: true } });
        setCar((prop as any) || {});
        return;
      }
      setCar({});
    })();
  }, [id]);
  return (
    <AppForm
      key={car?.Id}
      defaultValues={car}
      files={{ Images: car?.Images }}
      masterData={{ brands: [{ value: 1, label: "Toyota" }] }}
      onSubmit={onSubmit}
      fields={fields}
      validationSchema={CarSchema}
    />
  );
}

const countries = [
  { value: "kz", label: "Kazakhstan" },
  { value: "uae", label: "United Arab Emirates" },
];

const fields = [
  {
    id: "Name",
    label: "Name",
    name: "Name",
    type: FieldTypes.TEXT,
  },
  {
    id: "BrandId",
    label: "Brand",
    name: "BrandId",
    type: FieldTypes.SELECT,
    optionsName: "brands",
  },
  {
    id: "BatteryPower",
    label: "Battery Power",
    name: "BatteryPower",
    type: FieldTypes.NUMBER,
  },
  {
    id: "RealRange",
    label: "Real Range",
    name: "RealRange",
    type: FieldTypes.NUMBER,
  },
  {
    id: "TopSpeed",
    label: "Top Speed",
    name: "TopSpeed",
    type: FieldTypes.NUMBER,
  },
  {
    id: "EfficiencyVal",
    label: "Efficiency Value",
    name: "EfficiencyVal",
    type: FieldTypes.NUMBER,
  },
  {
    id: "Price",
    label: "Price",
    name: "Price",
    type: FieldTypes.NUMBER,
  },
  {
    id: "PriceCurrency",
    label: "Price Currency",
    name: "PriceCurrency",
    type: FieldTypes.TEXT,
  },

  {
    id: "Battery",
    label: "Battery",
    name: "Battery",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "BatteryKz",
    label: "Battery KZ",
    name: "BatteryKz",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "BatteryRu",
    label: "Battery RU",
    name: "BatteryRu",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "Performance",
    label: "Performance",
    name: "Performance",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "PerformanceKz",
    label: "Performance KZ",
    name: "PerformanceKz",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "PerformanceRu",
    label: "Performance RU",
    name: "PerformanceRu",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "Range",
    label: "Range",
    name: "Range",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "RangeKz",
    label: "Range KZ",
    name: "RangeKz",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "RangeRu",
    label: "Range RU",
    name: "RangeRu",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "Efficiency",
    label: "Efficiency",
    name: "Efficiency",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "EfficiencyKz",
    label: "Efficiency KZ",
    name: "EfficiencyKz",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "EfficiencyRu",
    label: "Efficiency RU",
    name: "EfficiencyRu",
    type: FieldTypes.TEXTAREA,
    grid: { md: 12 },
  },
  {
    id: "BestSeller",
    label: "Best Seller",
    name: "BestSeller",
    type: FieldTypes.CHECKBOX,
  },
  {
    id: "Featured",
    label: "Featured",
    name: "Featured",
    type: FieldTypes.CHECKBOX,
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
