import * as yup from "yup";

export const CarSchema = yup.object().shape({
  //   Name: yup
  //     .string()
  //     .transform((value) => value || "")
  //     .required("Property Name is required.")
  //     .max(100),
  Name: yup.string().required().max(100),
  PriceCurrency: yup.string().required().max(100),
  Battery: yup.string().required().max(4000),
  BatteryKz: yup.string().required().max(4000),
  BatteryRu: yup.string().required().max(4000),
  Efficiency: yup.string().required().max(4000),
  EfficiencyKz: yup.string().required().max(4000),
  EfficiencyRu: yup.string().required().max(4000),
  Performance: yup.string().required().max(4000),
  PerformanceKz: yup.string().required().max(4000),
  PerformanceRu: yup.string().required().max(4000),
  Range: yup.string().required().max(4000),
  RangeKz: yup.string().required().max(4000),
  RangeRu: yup.string().required().max(4000),
  Price: yup.number().required().min(0),
  EfficiencyVal: yup.number().transform((value) => value || 0),
  BatteryPower: yup.number().transform((value) => value || 0),
  RealRange: yup.number().transform((value) => value || 0),
  TopSpeed: yup.number().transform((value) => value || 0),
  BrandId: yup
    .number()
    .transform((value) => value || 0)
    .moreThan(0, "Please select a brand."),
});
